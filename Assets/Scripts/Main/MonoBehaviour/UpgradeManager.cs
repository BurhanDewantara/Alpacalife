using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using Artoncode.Core.Data;
using ScottGarland;

public class UpgradeManager : SingletonMonoBehaviour<UpgradeManager>
{
	public delegate void UpgradeManagerDelegate (BigInteger prev,BigInteger delta);

	public event UpgradeManagerDelegate OnMultiplierUpdated;


	private int environmentLevel;
	private int livestockLevel;

	public List<EnvironmentSO> defaultEnvironmentList;
	public List<LivestockSO> defaultLivestockList;

	public List<EnvironmentSO> environmentList;
	public List<LivestockSO> livestockList;

	public List<EnvironmentSO> ownedEnvironmentList;
	public List<LivestockSO> ownedLivestockList;

	public void Start ()
	{
		Load ();
	}

	void Load ()
	{
		environmentLevel = GameDataManager.shared ().EnvironmentLevel;
		livestockLevel = GameDataManager.shared ().LivestockLevel;

		ownedEnvironmentList = new List<EnvironmentSO> (defaultEnvironmentList);
		ownedLivestockList = new List<LivestockSO> (defaultLivestockList);

		if (environmentLevel < environmentList.Count) {
			for (int i = 0; i < environmentLevel; i++) {
				ownedEnvironmentList.Add (environmentList [i]);
			}
		}

		if (livestockLevel < livestockList.Count) {
			for (int i = 0; i < livestockLevel; i++) {
				ownedLivestockList.Add (livestockList [i]);
			}
		}
	}


	void Save ()
	{
		GameDataManager.shared ().EnvironmentLevel = environmentLevel;
		GameDataManager.shared ().LivestockLevel = livestockLevel;
	}


	#region Helper

	private int GetModPriceBasedOnLevel (int level)
	{
		return Mathf.Min (level + 9, 100);
	}


	private int GetMultiplier (int basevalue, int level)
	{
		float multi = 1.4f;
		float curr = 1;
		float next = curr;
		float n = 0;

		for (int i = 0; i < level; i++) {
			next = curr * multi;
			curr = next;
		}

		return Mathf.RoundToInt (next * basevalue);
	}

	private BigInteger GetFibonaciIndex (int level)
	{
		BigInteger o = 1;
		BigInteger p = 1;
		BigInteger v = 1;


		for (int i = 0; i < level; i++) {
			v = o + p;
			o = p;
			p = v;
		}

		return v;
	}

	#endregion

	#region Livestock

	public LivestockSO GetNextLivestockUpgrade ()
	{
		if (livestockLevel < livestockList.Count) {
			return livestockList [livestockLevel];
		}

		return null;
	}

	public bool UpgradeLivestock ()
	{
		if (livestockLevel < livestockList.Count) {
			ownedLivestockList.Add (livestockList [livestockLevel]);
			livestockLevel++;
			return true;
		}
		return false;
	}

	public BigInteger GetLivestockSlideValue (LivestockSO livestock)
	{
		if (livestock.slideValue == 0) {
			//LEVEL START FROM 1 not 0
			int level = livestockList.IndexOf (livestock) + 1;
			livestock.slideValue = GetFibonaciIndex (level);
		}

		return livestock.slideValue;
	}

	public BigInteger GetLivestockUpgradePrice (LivestockSO livestock)
	{
		if (livestock == null)
			return -1;

		if (livestock.upgradePrice == 0) {
			//LEVEL START FROM 1 not 0
			int level = livestockList.IndexOf (livestock) + 1;
//			BigInteger bIntPrice = GetFibonaciIndex(level);
			BigInteger bIntPrice = GetMultiplier (GetModPriceBasedOnLevel (level), level);

			bIntPrice = bIntPrice * level;
			livestock.upgradePrice = bIntPrice;	
		}
		return livestock.upgradePrice;
	}

	public string GetLivestockProgress ()
	{
		return livestockLevel + "/" + livestockList.Count;
	}

	#endregion

	#region Environment

	public EnvironmentSO GetLatestEnvironment (EnvironmentType type)
	{
		EnvironmentSO selected = null;
		foreach (EnvironmentSO item in ownedEnvironmentList) {
			if (item.type == type) {
				if (selected == null) {
					selected = item;
				}
				else if (selected.level < item.level) {
					selected = item;
				}
			}
		}
		return selected;
	}


	public EnvironmentSO GetNextEnvironmentUpgrade ()
	{
		if (environmentLevel < environmentList.Count) {
			return environmentList [environmentLevel];
		}
			
		return null;
	}

	public bool UpgradeEnvironment ()
	{
		if (environmentLevel < environmentList.Count) {
			ownedEnvironmentList.Add (environmentList [environmentLevel]);
			if (OnMultiplierUpdated != null) {
				BigInteger prev = environmentLevel == 0 ? 1 : GetEnvironmentMultiplyValue (environmentList [environmentLevel - 1]);
				BigInteger target = GetEnvironmentMultiplyValue (environmentList [environmentLevel]);
				BigInteger delta = target - prev;

				OnMultiplierUpdated (prev, delta);
			}

			environmentLevel++;


			return true;
		}
		return false;
	}

	public BigInteger GetCurrentMultiplier ()
	{
		BigInteger prev = environmentLevel == 0 ? 1 : GetEnvironmentMultiplyValue (environmentList [environmentLevel - 1]);
		return prev; 
	}

	public BigInteger GetEnvironmentMultiplyValue (EnvironmentSO environment)
	{
		if (environment == null)
			return -1;

		if (environment.multiplier == 0) {
			//LEVEL START FROM 1 not 0
			int level = environmentList.IndexOf (environment) + 1;
			environment.multiplier = 1 + GetMultiplier (1, level);
		}
		return environment.multiplier;

	}


	public BigInteger GetEnvironmentUpgradePrice (EnvironmentSO environment)
	{
		if (environment == null)
			return -1;

		if (environment.upgradePrice == 0) {
			//LEVEL START FROM 1 not 0
			int level = environmentList.IndexOf (environment) + 1;
			BigInteger bIntPrice = GetFibonaciIndex (level);
			bIntPrice = bIntPrice * level * GetModPriceBasedOnLevel (level);
			environment.upgradePrice = bIntPrice;	
		}
		return environment.upgradePrice;
	}

	public string GetEnvironmentProgress ()
	{
		return environmentLevel + "/" + environmentList.Count;
	}

	#endregion




	public void OnGUI ()
	{
		GUILayout.BeginVertical ("Box");

		GUILayout.Label ("Env Level : " + environmentLevel);
		GUILayout.Label ("Lvs Level : " + livestockLevel);

		EnvironmentSO env = GetNextEnvironmentUpgrade ();
		if (env != null) {
//			if (GUILayout.Button (env.ToString () + " : " + GetEnvironmentUpgradePrice(env).ToStringShort())) {
			if (GUILayout.Button ("env")) {
				UpgradeEnvironment ();
				WorldManager.shared ().RefreshEnvironment ();

			}
		}
		LivestockSO lvs = GetNextLivestockUpgrade ();
		if (lvs != null) {
//			if (GUILayout.Button (lvs.ToString () + " : " + GetLivestockUpgradePrice(lvs).ToStringShort())) {
			if (GUILayout.Button ("lvs")) {
				UpgradeLivestock ();
			}
		}

		GUILayout.EndVertical ();
//		BuyNewEnvironment ();
	}



	#region Custom Method

	[MenuItem ("CONTEXT/UpgradeManager/Calculate")]
	private static void Calc ()
	{
		LivestockSO so = UpgradeManager.shared ().livestockList.Random ();

		print (so + " : " + UpgradeManager.shared ().GetLivestockUpgradePrice (so).ToStringShort () + " " + UpgradeManager.shared ().GetLivestockSlideValue (so).ToStringShort ());
	}


	[MenuItem ("CONTEXT/UpgradeManager/Generate Upgrades")]
	private static void GenerateUpgrade ()
	{	
		// Environment List
		UpgradeManager.shared ().environmentList = new List<EnvironmentSO> ();
		//  Get Environment in Assets Folder
		foreach (EnvironmentSO item in Helper.LoadAllAssetsOfType<EnvironmentSO> ()) {
			//dont insert the one that default
			if (!UpgradeManager.shared ().defaultEnvironmentList.Contains (item)) {
				UpgradeManager.shared ().environmentList.Add (item);
			}

		}

//		// SORT Environment
//		UpgradeManager.shared().environmentList.Sort (delegate(EnvironmentSO x, EnvironmentSO y) {
//			if (x.level == y.level)
//				return x.type.CompareTo (y.type);
//
//			return x.level.CompareTo (y.level);
//		});
//

		// LIVESTOCK
		UpgradeManager.shared ().livestockList = new List<LivestockSO> ();
		// Get Livestock in the Assets Folder
		foreach (LivestockSO item in Helper.LoadAllAssetsOfType<LivestockSO> ()) {
			//dont insert the one that default
			if (!UpgradeManager.shared ().defaultLivestockList.Contains (item))
				UpgradeManager.shared ().livestockList.Add (item);
		}
		//NO NEED TO SORT, SORT BASED ON THE DIRECTORY (by name)
//		// Sort Livestock
//		UpgradeManager.shared().livestockList.Sort (delegate(LivestockSO x, LivestockSO y) {
//			return x.name.CompareTo (y.name);
//		});
//		// Reverse order to get ascending order
//		UpgradeManager.shared ().livestockList.Reverse ();
	}






	#endregion

}
