﻿using UnityEngine;
using System.Collections;
using Artoncode.Core;
using Artoncode.Core.Data;
using ScottGarland;

public class GameDataManager : Singleton<GameDataManager>
{
	public const string PLAYER_ENV_UPGRADE_LEVEL_KEY = "PLAYER_ENV_UPGRADE_LEVEL_KEY";
	public const string PLAYER_LVS_UPGRADE_LEVEL_KEY = "PLAYER_LVS_UPGRADE_LEVEL_KEY";
	public const string PLAYER_CURRENCY_KEY = "PLAYER_CURRENCY_KEY";
	public const string PLAYER_TOTAL_GOLD_KEY = "PLAYER_TOTAL_GOLD_KEY";
	public const string PLAYER_TOTAL_JUMP_KEY = "PLAYER_TOTAL_JUMP_KEY";
	public const string PLAYER_TOTAL_BITTEN_KEY = "PLAYER_TOTAL_BITTEN_KEY";

	public const string PLAYER_BEST_KEY = "PLAYER_BEST_KEY";
	public const string PLAYER_TUTORIAL_KEY = "PLAYER_HAS_TUTORIAL_KEY";


	public DataManager data;

	public GameDataManager()
	{
		data = DataManager.create ();
		data.setDefaultPath ("data.dat");
		Load();
	}

	public void save()
	{
		data.save ();
	}
	public void Load()
	{
		data.load ();
	}


	public int EnvironmentLevel
	{
		get
		{
			return ((data.getInt (PLAYER_ENV_UPGRADE_LEVEL_KEY) != null) ? data.getInt (PLAYER_ENV_UPGRADE_LEVEL_KEY).Value : 0);
		}
		set{ 
			data.setInt (PLAYER_ENV_UPGRADE_LEVEL_KEY, value);	
		}
	}

	public int LivestockLevel
	{
		get
		{
			return ((data.getInt (PLAYER_LVS_UPGRADE_LEVEL_KEY) != null) ? data.getInt (PLAYER_LVS_UPGRADE_LEVEL_KEY).Value : 0);
		}
		set{ 
			data.setInt (PLAYER_LVS_UPGRADE_LEVEL_KEY, value);	
		}
	}

	public int PlayerBestScore
	{
		get
		{
			return ((data.getInt (PLAYER_BEST_KEY) != null) ? data.getInt (PLAYER_BEST_KEY).Value : 0);
		}
		set{ 
			data.setInt (PLAYER_BEST_KEY, value);	
		}
	}


	public bool PlayerHasTakenTutorial
	{
		get
		{
			return ((data.getBool (PLAYER_TUTORIAL_KEY) != null) ? data.getBool (PLAYER_TUTORIAL_KEY).Value : false);
		}
		set{ 
			data.setBool (PLAYER_TUTORIAL_KEY, value);	
		}
	}




	public BigInteger PlayerCurrency {
		get {
			return (BigInteger)data.getObject (PLAYER_CURRENCY_KEY);
		}
		set {
			data.setObject (PLAYER_CURRENCY_KEY, value);
		}
	}	

	public BigInteger PlayerTotalGold{
		get {
			return (BigInteger)data.getObject (PLAYER_TOTAL_GOLD_KEY);
		}
		set {
			data.setObject (PLAYER_TOTAL_GOLD_KEY, value);
		}
	}

	public BigInteger PlayerTotalJump{
		get {
			return (BigInteger)data.getObject (PLAYER_TOTAL_JUMP_KEY);
		}
		set {
			data.setObject (PLAYER_TOTAL_JUMP_KEY, value);
		}
	}

	public BigInteger PlayerTotalBitten{
		get {
			return (BigInteger)data.getObject (PLAYER_TOTAL_BITTEN_KEY);
		}
		set {
			data.setObject (PLAYER_TOTAL_BITTEN_KEY, value);
		}
	}

}
