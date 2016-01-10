using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

[System.Serializable]
public enum CurrencyUnit
{
	None,
	K,
	M,
	B,
	T,
	Q,
	P,
	V,
}

public class Currency
{	

	private int[] coin;
	private CurrencyUnit unit;
	private bool isPositive;
	private const int threshold = 1000;
	
	#region CONSTRUCTOR

	public Currency(string coinStr)
	{
		if(Regex.IsMatch(coinStr,"[^0-9]"))
		{
			Debug.LogError("Its not a NUMBER");
			return;
		}


		int coinArrayLength = GetCoinArrayLength(coinStr);
		
		if (coinArrayLength > GetCurrencyUnitLength()) {
			Debug.LogError("Unit over measurement ");
			return;
		}
		this.unit = (CurrencyUnit) (coinArrayLength - 1);
		this.coin = new int[coinArrayLength];
		this.isPositive = true;


		ConvertToCoinArray (coinStr, ref this.coin);
		Debug.Log (ToString ());
	}


	public Currency(int coin, CurrencyUnit unit)
	{
		if (coin > threshold) {
			Debug.LogError("the coin must under " + threshold);
			return;
		}

		this.coin = new int[(int)unit+1];
		for (int i = 0; i < this.coin.Length; i++) {
			this.coin[i] = 0;
		}
		this.coin [(int)unit] = coin;
		this.unit = unit;

		Debug.Log (ToString ());
	}


	#endregion


	private int GetCurrencyUnitLength()
	{
		return Enum.GetNames (typeof(CurrencyUnit)).Length;
	}


	private int GetCoinArrayLength(string coin)
	{
		int digitPerArray = Mathf.CeilToInt((float)Math.Log(threshold,10));
		return Mathf.CeilToInt((float)coin.Length/(float)digitPerArray);
	}

	
	private void ConvertToCoinArray(string fromCoin, ref int[] toCoin)
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
	public override string ToString()
	{
		string str = "";

		str += "-CURRENCY-\n";
		str += "Default : " + DefaultString () + "\n";
		str += "Full : " + FullString () + "\n";
		str += "Simple : " + SimpleString () + "\n";


		return str;

	}
	public string DefaultString()
	{
		return FullString ().Replace (".", "");
	}

	public string FullString()
	{
		string str = "";
		for (int i = coin.Length-1; i >=0 ; i--) {
			if(i!=coin.Length-1)
				str+=".";

			if(i == coin.Length-1)
				str+= coin[i].ToString();
			else 
				str+= coin[i].ToString("000");
		}
		return str;
	}

	public string SimpleString()
	{
		string str = "";
		str += coin[coin.Length-1].ToString();

		if (coin.Length > 1)
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

			double rest = coin[coin.Length-2] / divisor;

			str += ((int)rest).ToString("D"+digitAfterComa);

			str += " " + unit;
		} 
		return str;
	}
	#endregion



	#region Operator
	public static Currency operator - (Currency current, Currency amount)
	{
		throw new Exception ("Gabisa");
		return new Currency ("1");
	}


	public static bool operator <= (Currency current, Currency amount)
	{
		if (current.coin.Length == amount.coin.Length) {
			
			int len = current.coin.Length;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] <= amount.coin[i];
			}	
		}
		
		return current.coin.Length <= amount.coin.Length;
	}

	public static bool operator >= (Currency current, Currency amount)
	{
		if (current.coin.Length == amount.coin.Length) {
			
			int len = current.coin.Length;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] >= amount.coin[i];
			}	
		}
		
		return current.coin.Length >= amount.coin.Length;
	}

	public static bool operator < (Currency current, Currency amount)
	{
		if (current.coin.Length == amount.coin.Length) {
			
			int len = current.coin.Length;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] < amount.coin[i];
			}	
		}
		
		return current.coin.Length < amount.coin.Length;
	}

	public static bool operator > (Currency current, Currency amount)
	{
		if (current.coin.Length == amount.coin.Length) {
			int len = current.coin.Length;
			for (int i = len-1; i >=0 ; i--) {
				if(current.coin[i] == amount.coin[i]) continue; 
				return current.coin[i] > amount.coin[i];
			}	

		}

		return current.coin.Length > amount.coin.Length;
	}


	#endregion



	#region Obsolete
	[Obsolete("Use Currency with String parameter instead",true)]
	 public Currency (ulong coin)
	 {
		//		coin = 1.234.567.899;
		//		coin = 1,2345678990 B
		//		coin = 1.234,56789900000 M
		//		coin = 1.234.567,89900000000 K
		//		coin = 1.234.567.899,0000000000 
		
		int coinArrayLength = GetCoinArrayLength(coin);
		
		if (coinArrayLength > GetCurrencyUnitLength()) {
			Debug.LogError("Unit over measurement ");
			return;
		}
		
		this.unit = (CurrencyUnit) (coinArrayLength - 1);
		this.coin = new int[coinArrayLength];
		ConvertToCoinArray (coin,ref this.coin);
		
		Debug.Log (ToString ());
	 }
	 
	 private int GetCoinArrayLength(ulong coin)
	 {
		int coinArray = 0;
		while (coin > 0) {
			coin/=threshold;
			coinArray++;
		}
		return coinArray;
	 }
	 
	 private void ConvertToCoinArray(ulong fromCoin, ref int[] toCoin)
	 {
		for (int i = 0; fromCoin > 0; i++, fromCoin /= threshold) 
		{
			toCoin[i] = (int)(fromCoin % threshold);
		}
	 }
	#endregion



}
