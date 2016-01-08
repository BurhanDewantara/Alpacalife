using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core.Data;

public static class GameHelper
{
	public static void ResetData()
	{
		DataManager.defaultManager.reset ();
		DataManager.defaultManager.save ();
		
		CurrencyManager.shared ().Reset ();
		PlayerStatisticManager.shared ().Reset ();
		PlayerUpgradableDataManager.shared ().Reset ();

	}

	public static void GetCoins()
	{
		CurrencyManager.shared().AddMoney(new Currency(9999999,999999));
	}

	public static void Upgrade(UpgradableType key)
	{
		PlayerUpgradableDataManager.shared ().Upgrade (key);
	}

	public static string SetColorInText(Color color, string text)
	{
		var rgbString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 
		                              (int)(color.r * 255), 
		                              (int)(color.g * 255), 
		                              (int)(color.b * 255),
		                              (int)(color.a * 255));
		
		string retVal = "<color="+rgbString+">";
		retVal += text;
		return retVal += "</color>";
	}
}
