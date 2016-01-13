using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;


public class BigInt
{	

	private List<int> integer;
	private bool isNegative;
	private const int threshold = 1000;
	public int Count
	{
		get{
			return integer.Count;
		}
	}
	
	public bool IsZero
	{
		get{
			return (integer !=null && integer.Count == 1 && integer[0] == 0);
		}
	}



	#region CONSTRUCTOR
	public BigInt(BigInt copyCons)
	{
		this.integer = new List<int>(copyCons.integer);
		this.isNegative = copyCons.isNegative;
	}


	public BigInt(string integerStr)
	{
		bool isNegate = false;
		if(integerStr.StartsWith("-")){ 
			isNegate = true;
			integerStr = integerStr.Remove(0,1);
		}


		if(Regex.IsMatch(integerStr,"[^0-9]"))
		{
			Debug.LogError("Its not a NUMBER");
			return;
		}

		int integerArrayLength = GetIntegerArrayLength(integerStr);
		if (integerArrayLength > GetCurrencyUnitLength()) {
			Debug.LogError("Unit over measurement");
			return;
		}

		this.integer = new List<int>();
		this.isNegative = isNegate;
		for (int i = 0; i < integerArrayLength; i++) {
			this.integer.Add(0);
		}

		ConvertToIntegerArray (integerStr, ref this.integer);
		Debug.Log (DebugString ());
	}

	#endregion

	public BigInt Clone()
	{
		return new BigInt(this);
	}


	private int GetCurrencyUnitLength()
	{
		return Enum.GetNames (typeof(InfCurrencyUnit)).Length;
	}


	private int GetIntegerArrayLength(string integer)
	{
		int digitPerArray = Mathf.CeilToInt((float)Math.Log(threshold,10));
		return Mathf.CeilToInt((float)integer.Length/(float)digitPerArray);
	}

	
	private void ConvertToIntegerArray(string fromInteger, ref List<int> toInteger)
	{
		int idx = 0;
		while (fromInteger.Length > 0) {
			int start = Math.Max(0,fromInteger.Length - 3);
			int len  =  Math.Min(fromInteger.Length,3);
			string last3 = fromInteger.Substring(start, len );

			toInteger[idx] = int.Parse(last3);
			fromInteger = fromInteger.Substring(0,start);
			idx++;
		}
	}


	#region ToString
	public  string DebugString()
	{
		string str = "";

		str += "-CURRENCY-\n";
		str += "Default : " + ToString () + "\n";
		str += "Full : " + FullString () + "\n";
		str += "Simple : " + SimpleString () + "\n";

		return str;

	}
	public override string ToString()
	{
		return FullString ().Replace (".", "");
	}

	public string FullString()
	{
		string str = "";

		if(this.isNegative)
			str += "-";

		for (int i = integer.Count-1; i >=0 ; i--) {
			if(i!=integer.Count-1)
				str+=".";

			if(i == integer.Count-1)
				str+= integer[i].ToString();
			else 
				str+= integer[i].ToString("000");
		}
		return str;
	}

	public string SimpleString(float digitAfterComa = 2)
	{
		string str = "";
		if(this.isNegative) str += "-";

		str += integer[integer.Count-1].ToString();

		if (integer.Count > 1)
		{
			str += ".";

			// in array = 123.456, 
			// if digit after coma = 2 => 123.45 K
			// if digit after coma = 1 => 123.4 K
			// if digit after coma = 0 => 123 K

			// get number of 0 digit in 1 array (10 = 1, 100 = 2, 1000 = 3) 
			double digitPerArray = Math.Log(threshold,10);
			// CANT BE BIGGER THAN DIGITPERARRAY nor 0
			digitAfterComa = Mathf.Clamp(digitAfterComa,0,(int)digitPerArray);

			// get power number for the number after decimal
			// if the digit after coma is 3 then the divisor should be 1, (because we didnt truncate at all) 123 = 123
			// if the digit after coma is 2 then the divisor should be 10, (because we have to truncate 1 last digit) 123 = 12
			// if the digit after coma is 1 then the divisor should be 100, (because we have to truncate 2 last digit) 123 = 1

			// how we get the number of powermod using the digitAfterComa,
			// we have to subtract digitPerArray with digitAfterComa
			// because we are using 1000 then digitPerArray = 3
			// and digitAfterComa is 2 then the calculation will be like this
			// 3 - 2 = 1
			// the power mod = 10^1 = 10
			// so for the figit after coma 2 then the divisor = 10, and so on..
			// not work perfectly
			double powerMod = digitPerArray - digitAfterComa;
			double divisor = Math.Ceiling(Math.Pow(10,powerMod));

			double rest = integer[integer.Count-2] / divisor;

			str += ((int)rest).ToString("D"+digitAfterComa);
		} 
		return str;
	}
	#endregion

	#region Operator

	#region PLUS/SUBTRACT/MULTIPLY/DIVIDE
	public static BigInt Plus(BigInt c, BigInt a)
	{
		BigInt current = c.Clone();
		BigInt amount = a.Clone();
		
		if(current.Count < amount.Count)
		{
			BigInt temp = current;
			current = amount;
			amount = temp;
		}

		for (int i = 0; i < current.integer.Count; i++) 
		{
			if(i < amount.integer.Count) 
			{	
				if(current.isNegative ^ amount.isNegative)
					current.integer[i] -= amount.integer[i];
				else
					current.integer[i] += amount.integer[i];
			}


			Recalculate(ref current,i);
		}

		RecheckZero(ref current);

		return current;
	}

	public static BigInt Subtract(BigInt c, BigInt a)
	{
		BigInt current = c.Clone();
		BigInt amount = a.Clone();
		
		if(current < amount)
		{
			BigInt temp = current;
			current = amount;
			amount = temp;
			current.isNegative = !current.isNegative;
		}
		
		for (int i = 0; i < current.integer.Count; i++) 
		{
			if(i < amount.integer.Count) 
			{
			 	if(current.isNegative ^ amount.isNegative)
					current.integer[i]+=amount.integer[i];
				else
					current.integer[i]-=amount.integer[i];
			}

			Recalculate(ref current,i);

		}
		
		//we use > instead of >=, because arr idx 0 must still exist, even its 0
		RecheckZero(ref current);

		return current;
	}

//	public static BigInt Multiply(BigInt c, BigInt a)
//	{
//		BigInt current = c.Clone();
//		BigInt amount = a.Clone();
//		
//		if(current < amount)
//		{
//			BigInt temp = current;
//			current = amount;
//			amount = temp;
//		}
//		
//		for (int i = 0; i < current.integer.Count; i++) 
//		{
//			if(i < amount.integer.Count) 
//			{
//				current.isNegative = current.isNegative ^ amount.isNegative;
//				current.integer[i]*=amount.integer[i];
//			}
//			
//			Recalculate(ref current,i);
//			
//		}
//		
//		RecheckZero(ref current);
//		
//		
//		return current;
//	}

	private static void Recalculate(ref BigInt current,int i)
	{
		while(current.integer[i] >= threshold)
		{
			if(i == current.integer.Count -1)
			{
				current.integer.Add(0);
			}
			current.integer[i+1] +=1;
			current.integer[i] -= threshold;
		}


		while(current.integer[i] < 0)
		{
			//if (
			if(i < current.integer.Count-1)
			{
				current.integer[i+1] -=1;
				current.integer[i] += threshold;
			}
			else{
				current.integer[i] *= -1;
				current.isNegative = !current.isNegative;
			}
		}
	}

	
	
	private static void RecheckZero(ref BigInt current)
	{
		int i = current.integer.Count;
		while (i > 0)
		{
			if(i == current.integer.Count -1)
			{
				if(current.integer[i] <= 0)
					current.integer.RemoveAt(i);
			}
			i--;
		}
		
		if(current.IsZero)
			current.isNegative = false;
	}
	


	#endregion





	public static BigInt operator - (BigInt c, BigInt a)
	{
	
		return Subtract(c,a);
	}


	public static BigInt operator + (BigInt c, BigInt a)
	{

//		if(a.isNegative)
//			return Subtract(c,a);
//		if(c.isNegative)
//			return Subtract(a,c);

		return Plus(c,a);
	}









	public static bool operator <= (BigInt current, BigInt amount)
	{
		if(current.isNegative ^ amount.isNegative)
			return current.isNegative ? true : false;


		if (current.integer.Count == amount.integer.Count) {
			
			int len = current.integer.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.integer[i] == amount.integer[i]) continue; 
				return current.integer[i] <= amount.integer[i];
			}	
		}
		
		return current.integer.Count <= amount.integer.Count;
	}

	public static bool operator >= (BigInt current, BigInt amount)
	{
		if(current.isNegative ^ amount.isNegative)
			return amount.isNegative ? true : false;


		if (current.integer.Count == amount.integer.Count) {
			
			int len = current.integer.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.integer[i] == amount.integer[i]) continue; 
				return current.integer[i] >= amount.integer[i];
			}	
		}
		
		return current.integer.Count >= amount.integer.Count;
	}

	public static bool operator < (BigInt current, BigInt amount)
	{
		if(current.isNegative ^ amount.isNegative)
			return current.isNegative ? true : false;

		if (current.integer.Count == amount.integer.Count) {
			
			int len = current.integer.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.integer[i] == amount.integer[i]) continue; 
				return current.integer[i] < amount.integer[i];
			}	
		}
		
		return current.integer.Count < amount.integer.Count;
	}

	public static bool operator > (BigInt current, BigInt amount)
	{
		if(current.isNegative ^ amount.isNegative)
			return amount.isNegative ? true : false;

		if (current.integer.Count == amount.integer.Count) {
			int len = current.integer.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.integer[i] == amount.integer[i]) continue; 
				return current.integer[i] > amount.integer[i];
			}	

		}

		return current.integer.Count > amount.integer.Count;
	}

	#endregion



}
