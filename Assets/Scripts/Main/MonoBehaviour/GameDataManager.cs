using UnityEngine;
using System.Collections;
using Artoncode.Core;
using Artoncode.Core.Data;
using ScottGarland;

public class GameDataManager : Singleton<GameDataManager>
{
	public const string PLAYER_ENV_UPGRADE_LEVEL_KEY = "PLAYER_ENV_UPGRADE_LEVEL_KEY";
	public const string PLAYER_LVS_UPGRADE_LEVEL_KEY = "PLAYER_LVS_UPGRADE_LEVEL_KEY";
	public const string PLAYER_BigInteger_KEY = "PLAYER_ENVIRONMENT_LEVEL_KEY";

	public DataManager data;

	public GameDataManager()
	{
		data = DataManager.create ();
		data.setDefaultPath ("data.dat");
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

	public BigInteger GameBigInteger {
		get {
			return (BigInteger)data.getObject (PLAYER_BigInteger_KEY);
		}
		set {
			data.setObject (PLAYER_BigInteger_KEY, value);
		}
	}



}
