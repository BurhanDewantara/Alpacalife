using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;


[System.Serializable]
public enum InfCurrencyUnit
{
	None,
	K,
	M,
	B,
	T,
	Q,
	P,
	V,
	aa,
	bb,
	cc,
	dd,
	ee,
	ff,
	gg,
	hh,
	ii,
	jj,
	kk,
	ll,
	mm,
	nn,
	oo,
	pp,
	qq,
	rr,
	ss,
	tt,
	uu,
	vv,
	ww,
	xx,
	yy,
	zz,
	aaa,
	bbb,
	ccc,
	ddd,
	eee,
	fff,
	ggg,
	hhh,
	iii,
	jjj,
	kkk,
	lll,
	mmm,
	nnn,
	ooo,
	ppp,
	qqq,
	rrr,
	sss,
	ttt,
	uuu,
	vvv,
	www,
	xxx,
	yyy,
	zzz,
	aaaa,
	bbbb,
	cccc,
	dddd,
	eeee,
	ffff,
	gggg,
	hhhh,
	iiii,
	jjjj,
	kkkk,
	llll,
	mmmm,
	nnnn,
	oooo,
	pppp,
	qqqq,
	rrrr,
	ssss,
	tttt,
	uuuu,
	vvvv,
	wwww,
	xxxx,
	yyyy,
	zzzz
}

public class InfCurrency
{	

	private List<int> coin;
	private bool isNegative;
	private InfCurrencyUnit unit;
	public InfCurrencyUnit Unit{
		set{
			unit = value;
		}
		get{ 

			if(coin != null && (unit != (InfCurrencyUnit) coin.Count-1))
					return (InfCurrencyUnit) coin.Count-1;

			return unit;
		}
	}
	private const int threshold = 1000;
	
	#region CONSTRUCTOR
	public InfCurrency(InfCurrency copyCons)
	{
		this.coin = new List<int>(copyCons.coin);
		this.Unit = copyCons.Unit;
		this.isNegative = copyCons.isNegative;
	}


	public InfCurrency(string coinStr)
	{
		if(Regex.IsMatch(coinStr,"[^0-9]"))
		{
			Debug.LogError("Its not a NUMBER");
			return;
		}

		int coinArrayLength = GetCoinArrayLength(coinStr);
		if (coinArrayLength > GetCurrencyUnitLength()) {
			Debug.LogError("Unit over measurement");
			return;
		}


		this.Unit = (InfCurrencyUnit) (coinArrayLength - 1);
		this.coin = new List<int>();
		this.isNegative = false;
		for (int i = 0; i < coinArrayLength; i++) {
			this.coin.Add(0);
		}

		ConvertToCoinArray (coinStr, ref this.coin);
		Debug.Log (DebugString ());
	}


	public InfCurrency(int coin, InfCurrencyUnit unit = InfCurrencyUnit.None)
	{
		if (coin > threshold) {
			Debug.LogError("the coin must under " + threshold);
			return;
		}

		this.coin = new List<int>();
		for (int i = 0; i <= (int)unit; i++) {
			this.coin.Add(0);
		}

		this.Unit = unit;
		this.coin [(int)unit] = coin;
		this.isNegative = false;

		Debug.Log (DebugString ());
	}

	#endregion

	public InfCurrency Clone()
	{
		return new InfCurrency(this);
	}


	private int GetCurrencyUnitLength()
	{
		return Enum.GetNames (typeof(InfCurrencyUnit)).Length;
	}


	private int GetCoinArrayLength(string coin)
	{
		int digitPerArray = Mathf.CeilToInt((float)Math.Log(threshold,10));
		return Mathf.CeilToInt((float)coin.Length/(float)digitPerArray);
	}

	
	private void ConvertToCoinArray(string fromCoin, ref List<int> toCoin)
	{
		int idx = 0;
		while (fromCoin.Length > 0) {
			int start = Math.Max(0,fromCoin.Length - 3);
			int len  =  Math.Min(fromCoin.Length,3);
			string last3 = fromCoin.Substring(start, len );

			toCoin[idx] = int.Parse(last3);
			fromCoin = fromCoin.Substring(0,start);
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
		for (int i = coin.Count-1; i >=0 ; i--) {
			if(i!=coin.Count-1)
				str+=".";

			if(i == coin.Count-1)
				str+= coin[i].ToString();
			else 
				str+= coin[i].ToString("000");
		}
		return str;
	}

	public string SimpleString()
	{
		string str = "";
		str += coin[coin.Count-1].ToString();

		if (coin.Count > 1)
		{
			str += ".";

			// in array = 123.456, 
			// if digit after coma = 2 => 123.45 K
			// if digit after coma = 1 => 123.4 K
			// if digit after coma = 0 => 123 K

			// get number of 0 digit in 1 array (10 = 1, 100 = 2, 1000 = 3) 
			double digitPerArray = Math.Log(threshold,10);
			// CANT BE BIGGER THAN DIGITPERARRAY nor 0
			double digitAfterComa = Mathf.Clamp(2,0,(int)digitPerArray);

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

			double rest = coin[coin.Count-2] / divisor;

			str += ((int)rest).ToString("D"+digitAfterComa);

			str += " " + Unit;
		} 
		return str;
	}
	#endregion



	#region Operator
	public static InfCurrency operator - (InfCurrency c, InfCurrency a)
	{
//		if(amount > current)
//		{
//			Debug.LogError("the value can not below than 0");
//		}
//		return new InfCurrency ("1");

		InfCurrency current = c.Clone();
		InfCurrency amount = a.Clone();

		if(current.coin.Count < amount.coin.Count)
		{
			InfCurrency temp = current;
			current = amount;
			amount = temp;
		}

		for (int i = 0; i < current.coin.Count; i++) 
		{
			if(i < amount.coin.Count) 
			{
				current.coin[i]-=amount.coin[i];
			}

			while(current.coin[i] < 0)
			{
				current.coin[i+1] -=1;
				current.coin[i] += threshold;
			}


		}

		int j = current.coin.Count;
		while (j >= 0)
		{
			if(j == current.coin.Count -1)
			{
				if(current.coin[j] <= 0)
					current.coin.RemoveAt(j);
			}
			j--;
		}



		
		return current;

	}

	public static InfCurrency operator + (InfCurrency c, InfCurrency a)
	{
		InfCurrency current = c.Clone();
		InfCurrency amount = a.Clone();
	
		if(current.coin.Count < amount.coin.Count)
		{
			InfCurrency temp = current;
			current = amount;
			amount = temp;
		}

		for (int i = 0; i < current.coin.Count; i++) 
		{
			if(i < amount.coin.Count) 
			{
				current.coin[i]+=amount.coin[i];
			}

			while(current.coin[i] >= threshold)
			{
				if(i == current.coin.Count -1)
				{
					current.coin.Add(0);
				}
				current.coin[i+1] +=1;
				current.coin[i] -= threshold;
			}
		}

		return current;
	}


	public static bool operator <= (InfCurrency current, InfCurrency amount)
	{
		if (current.coin.Count == amount.coin.Count) {
			
			int len = current.coin.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] <= amount.coin[i];
			}	
		}
		
		return current.coin.Count <= amount.coin.Count;
	}

	public static bool operator >= (InfCurrency current, InfCurrency amount)
	{
		if (current.coin.Count == amount.coin.Count) {
			
			int len = current.coin.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] >= amount.coin[i];
			}	
		}
		
		return current.coin.Count >= amount.coin.Count;
	}

	public static bool operator < (InfCurrency current, InfCurrency amount)
	{
		if (current.coin.Count == amount.coin.Count) {
			
			int len = current.coin.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] < amount.coin[i];
			}	
		}
		
		return current.coin.Count < amount.coin.Count;
	}

	public static bool operator > (InfCurrency current, InfCurrency amount)
	{
		if (current.coin.Count == amount.coin.Count) {
			int len = current.coin.Count;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] > amount.coin[i];
			}	

		}

		return current.coin.Count > amount.coin.Count;
	}

	#endregion



}
