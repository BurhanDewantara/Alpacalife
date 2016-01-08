using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using Artoncode.Core.Data;

public class GameManager : Singleton<GameManager>
{
	private const string PLAYER_CURRENCY_KEY = "PLAYER_CURRENCY_KEY";
	private const string PLAYER_UPGRADABLE_DATA_KEY = "PLAYER_UPGRADABLE_KEY";
	private const string PLAYER_STATISTIC_KEY = "PLAYER_STATISTIC_KEY";

	public GameManager ()
	{
		DataManager.defaultManager.load ();
		SaveLog ("Load", "Load All data");
	}


	public Currency GameCurrency{
		get {
			return (Currency)DataManager.defaultManager.getObject (PLAYER_CURRENCY_KEY);
		}
		set {
			DataManager.defaultManager.setObject (PLAYER_CURRENCY_KEY, value);
			DataManager.defaultManager.save ();
			SaveLog ("Save", "Game Currency");
		} 

	}

	public Hashtable GameUpgradableData { 
		get {
			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_UPGRADABLE_DATA_KEY);
		}
		set {
			DataManager.defaultManager.setObject (PLAYER_UPGRADABLE_DATA_KEY, value);
			DataManager.defaultManager.save ();
			SaveLog ("Save", "Game Upgradeable");
		} 
	}

	public Hashtable GameStatisticData { 
		get {
			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_STATISTIC_KEY);
		}
		set {
			DataManager.defaultManager.setObject (PLAYER_STATISTIC_KEY, value);
			DataManager.defaultManager.save ();
			SaveLog ("Save", "Game Statistic");
		} 
	}


//	
//	public Hashtable GameAchievement { 
//		get {
//			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_GAME_ACHIEVEMENT_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_ACHIEVEMENT_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "Game Achievement");
//		} 
//	}
//	
//	
//	public Hashtable GameInventory { 
//		get {
//			
//			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_GAME_INVENTORY_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_INVENTORY_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "GameInventory");
//		} 
//	}
//	
//	public Currency GameCurrency { 
//		get {
//			return (Currency)DataManager.defaultManager.getObject (PLAYER_GAME_CURRENCY_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_CURRENCY_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "GameCurrency");
//		} 
//	}
//	
//	public Hashtable GameParty {
//		get {
//			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_GAME_CHAR_STAT_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_CHAR_STAT_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "GamePlayerStatistic");
//		}
//		
//	}
//	
//	public List<PetticleItem>  GamePlayerPetticles {
//		get {
//			return (List<PetticleItem>)DataManager.defaultManager.getObject (PLAYER_GAME_PETTICLE_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_PETTICLE_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "PlayerPetticle");
//		}
//	}
//	
//	public List<PetticleItem>  GamePlayerEquippedPetticles {
//		get {
//			return (List<PetticleItem>)DataManager.defaultManager.getObject (PLAYER_GAME_EQUIPPED_PETTICLE_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_EQUIPPED_PETTICLE_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "PlayerEquippedPetticle");
//		}
//	}
//	
//	
//	
//	public Hashtable GameStoryGameState {
//		get {
//			return (Hashtable)DataManager.defaultManager.getObject (PLAYER_GAME_STATE_KEY);
//		}
//		set {
//			DataManager.defaultManager.setObject (PLAYER_GAME_STATE_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "player Game State");
//		}
//	}
//	
//	
//	public GameDataModel GamePlayerMiscGameData
//	{ 
//		get{
//			return (GameDataModel)DataManager.defaultManager.getObject (PLAYER_GAME_MISC_DATA_KEY);
//		}
//		set{
//			DataManager.defaultManager.setObject (PLAYER_GAME_MISC_DATA_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "PLAYER_GAME_MISC_DATA_KEY");
//		}
//	}
//	
//	public int GamePlayerPetticlePoint
//	{ 
//		get{
//			return (int) (DataManager.defaultManager.getInt (PLAYER_GAME_PETTICLE_POINT_KEY) == null ? 0 : (int) DataManager.defaultManager.getInt (PLAYER_GAME_PETTICLE_POINT_KEY) );
//		}
//		set{
//			DataManager.defaultManager.setInt (PLAYER_GAME_PETTICLE_POINT_KEY, value);
//			DataManager.defaultManager.save ();
//			SaveLog ("Save", "PLAYER_GAME_PETTICLE_POINT_KEY");
//		}
//	}
//	
	
	
	private void SaveLog (string pEvent, string pLogName)
	{
		//			Debug.Log("GAMEMANAGER : "+pEvent+" : "+pLogName);
	}
	
	
	
	
}
