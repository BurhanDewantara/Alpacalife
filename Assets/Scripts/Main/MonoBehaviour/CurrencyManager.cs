using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using Artoncode.Core.Data;
using ScottGarland;

public class CurrencyManager : SingletonMonoBehaviour<CurrencyManager>
{
	public delegate void CurrencyManagerDelegate(BigInteger prev,BigInteger delta);
	public event CurrencyManagerDelegate OnUpdated;

	public BigInteger playerMoney;

	void Awake()
	{
		Load();
	}

	void Load()
	{
		playerMoney = GameDataManager.shared().PlayerCurrency;
		if(playerMoney == null)
			playerMoney = 0;
	}

	void Save()
	{
		GameDataManager.shared().PlayerCurrency = playerMoney;
	}

	public void AddGold(LivestockSO livestock)
	{
		BigInteger gold		  = UpgradeManager.shared ().GetLivestockSlideValue (livestock);
		BigInteger multiplier = UpgradeManager.shared ().GetCurrentMultiplier ();
		AddGold (gold * multiplier);
	}

	public void AddGold(BigInteger value)
	{
		if(OnUpdated !=null)
			OnUpdated(playerMoney,value);

		playerMoney += value;
		Save();
	}

	public void PayGold(BigInteger value)
	{
		if(IsAfforadble(value))
		{
			if(OnUpdated !=null)
				OnUpdated(playerMoney,- value);

			playerMoney -= value;	
			Save();
		}
	}

	public bool IsAfforadble(BigInteger value)
	{
		return playerMoney >= value;
	}



}
