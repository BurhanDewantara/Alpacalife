using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Currency
{	
	public const int MAX_CURRENCY = 99999;
	public const int MIN_CURRENCY = -10000;
	public const int MAX_GEM_CURRENCY = 9999;
	public const int MIN_GEM_CURRENCY = -1000;
	public int coin;
	public int gem;

	
	public Currency ()
	{
		this.coin = 0;
		this.gem = 0;
	
	}
	
	public Currency (int coin, int gem)
	{
		this.coin = Mathf.Clamp (coin, MIN_CURRENCY, MAX_CURRENCY);
		this.gem = Mathf.Clamp (gem, MIN_GEM_CURRENCY, MAX_GEM_CURRENCY);
	}
	
	public static Currency operator - (Currency pCurrentPrice, Currency pPrice)
	{
		return new Currency (pCurrentPrice.coin - pPrice.coin,
		                     pCurrentPrice.gem - pPrice.gem);
	}
	
	public static Currency operator + (Currency pCurrentPrice, Currency pPrice)
	{
		return new Currency (pCurrentPrice.coin + pPrice.coin,
		                     pCurrentPrice.gem + pPrice.gem
		                     );
	}
	
	public static Currency operator * (Currency pCurrentPrice, int amount)
	{
		return new Currency (pCurrentPrice.coin * amount,
		                     pCurrentPrice.gem * amount
		                     );
	}
	
	public static Currency operator * (Currency pCurrentPrice, float amount)
	{
		return new Currency (Mathf.CeilToInt(pCurrentPrice.coin * amount),
		                     Mathf.CeilToInt(pCurrentPrice.gem * amount)
		                     );
	}


	public static Currency operator / (Currency pCurrentPrice, int amount)
	{
		return new Currency (pCurrentPrice.coin / amount,
		                     pCurrentPrice.gem / amount
		                     );
	}

	public static Currency operator / (Currency pCurrentPrice, float amount)
	{
		return new Currency (Mathf.CeilToInt(pCurrentPrice.coin / amount),
		                     Mathf.CeilToInt(pCurrentPrice.gem / amount)
		                     );
	}

	
	public static bool operator > (Currency pCurrentPrice, Currency pPrice)
	{
		return ((pCurrentPrice.coin > pPrice.coin)&&
		        (pCurrentPrice.gem > pPrice.gem) 
		        );

	}
	
	public static bool operator >= (Currency pCurrentPrice, Currency pPrice)
	{
		return ((pCurrentPrice.coin >= pPrice.coin)&&
		        (pCurrentPrice.gem >= pPrice.gem) 
		        );
	}
	
	public static bool operator < (Currency pCurrentPrice, Currency pPrice)
	{
		return ((pCurrentPrice.coin < pPrice.coin)&&
		        (pCurrentPrice.gem < pPrice.gem) 
		        );
	}
	
	public static bool operator <= (Currency pCurrentPrice, Currency pPrice)
	{
		return ((pCurrentPrice.coin <= pPrice.coin)&&
		        (pCurrentPrice.gem <= pPrice.gem) 
		        );
	}
	
	public override string ToString()
	{
		string str = "";
		str += this.coin + "C / ";
		str += this.gem + "G";
		return str;
	}
	
	
	public KeyValuePair<CurrencyType,int> SplitCurrency()
	{
		if (coin > 0) {
			return new KeyValuePair<CurrencyType,int> (CurrencyType.Coin,coin);
		}
		else if (gem > 0) {
			return new KeyValuePair<CurrencyType,int> (CurrencyType.Gem,gem);
		}
		return new KeyValuePair<CurrencyType,int> (CurrencyType.Coin,0);
	}
	
	
}
