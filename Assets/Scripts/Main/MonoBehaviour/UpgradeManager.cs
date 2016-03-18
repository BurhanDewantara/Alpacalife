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

	public void Awake ()
	{
		Load ();
	}

	public int LivestockLevel
	{
		get{return livestockLevel;}
	}

	public int EnvironmentLevel
	{
		get{return environmentLevel;}
	}

	public void Reset()
	{
		Load();		
	}


	void Load ()
	{
		environmentLevel = GameDataManager.shared ().EnvironmentLevel;
		livestockLevel = GameDataManager.shared ().LivestockLevel;

		ownedEnvironmentList = new List<EnvironmentSO> (defaultEnvironmentList);
		ownedLivestockList = new List<LivestockSO> (defaultLivestockList);

		if (environmentLevel <= environmentList.Count) {
			for (int i = 0; i < environmentLevel; i++) {
				ownedEnvironmentList.Add (environmentList [i]);
			}
		}

		if (livestockLevel <= livestockList.Count) {
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


	public BigInteger CalculateTotalSpent()
	{
		BigInteger totalCostEnv = 0;
		BigInteger totalCostLvs = 0;

		for (int i = 0; i < environmentLevel; i++) {
			totalCostEnv += GetEnvironmentUpgradePrice(environmentList[i]);
		}

		for (int i = 0; i < livestockLevel; i++) {
			totalCostLvs += GetLivestockUpgradePrice(livestockList[i]);
		}

		return (totalCostEnv + totalCostLvs);
	}

	#region Helper


	private int GetCalculateModPriceBasedOnLevel (int level)
	{
		float mod =1.25f;
		float baseVal = 10.0f;
		int result = Mathf.CeilToInt(baseVal);

		for (int i = 0; i < level-1; i++) {
			result = Mathf.CeilToInt(result * mod);
		}

		return result;

//		return Mathf.Min (level + 9, 100);
	}

	private BigInteger GetCalculateSlideValue(int level)
	{
		BigInteger baseVal = 1;
		BigInteger result = baseVal;

		for (int i = 0; i < level; i++) {
			result += i+1;
		}
		return result;
	}

	private BigInteger GetCalculatedMultiplierValue(int level)
	{
		return level+1;
	}

	private BigInteger GetEnvironmentUpgradePrice(int level)
	{
		int modPower = 2;
		BigInteger modPrice = GetCalculateModPriceBasedOnLevel(level);
		BigInteger levelMod = Mathf.CeilToInt(Mathf.Pow(level,modPower));

		BigInteger result = level * levelMod * modPrice;
		return result;
	}

	private BigInteger GetLivestockUpgradePrice(int level)
	{
		BigInteger modPrice = GetCalculateModPriceBasedOnLevel(level);
		BigInteger slideValue = GetCalculateSlideValue(level-1);

		BigInteger result = level * slideValue * modPrice;
		return result;
	}



//
//	private int GetMultiplier (int basevalue, int level)
//	{
//		float multi = 1.4f;
//		float curr = 1;
//		float next = curr;
//		float n = 0;
//
//		for (int i = 0; i < level; i++) {
//			next = curr * multi;
//			curr = next;
//		}
//
//		return Mathf.RoundToInt (next * basevalue);
//	}
//
//	private BigInteger GetFibonaciIndex (int level)
//	{
//		BigInteger o = 1;
//		BigInteger p = 1;
//		BigInteger v = 1;
//
//
//		for (int i = 0; i < level; i++) {
//			v = o + p;
//			o = p;
//			p = v;
//		}
//
//		return v;
//	}

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
			Save();
			return true;
		}
		return false;
	}

	public BigInteger GetLivestockSlideValue (LivestockSO livestock)
	{
		int level = livestockList.IndexOf (livestock) + 1;
		return GetCalculateSlideValue(level);
	}

	public BigInteger GetLivestockUpgradePrice (LivestockSO livestock)
	{
		if (livestock == null)
			return -1;
		int level = livestockList.IndexOf (livestock) + 1;
		return GetLivestockUpgradePrice(level);

	}

	public string GetLivestockProgress ()
	{
		return livestockLevel + "/" + livestockList.Count;
	}

	public bool IsLivestockMaxedOut()
	{
		return livestockLevel == livestockList.Count;
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
			Save();
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

		int level = environmentList.IndexOf (environment) + 1;
		return GetCalculatedMultiplierValue(level);

	}


	public BigInteger GetEnvironmentUpgradePrice (EnvironmentSO environment)
	{
		if (environment == null)
			return -1;

		int level = environmentList.IndexOf (environment) + 1;
		return GetEnvironmentUpgradePrice(level);
	}

	public string GetEnvironmentProgress ()
	{
		return environmentLevel + "/" + environmentList.Count;
	}

	public bool IsEnvironmmentMaxedOut()
	{
		return environmentLevel == environmentList.Count;
	}

	#endregion




//	public void OnGUI ()
//	{
//		GUILayout.BeginVertical ("Box");
//
//		GUILayout.Label ("Env Level : " + environmentLevel);
//		GUILayout.Label ("Lvs Level : " + livestockLevel);
//
//		EnvironmentSO env = GetNextEnvironmentUpgrade ();
//		if (env != null) {
//			if (GUILayout.Button ("env")) {
//				UpgradeEnvironment ();
//				WorldManager.shared ().RefreshEnvironment ();
//
//			}
//		}
//		LivestockSO lvs = GetNextLivestockUpgrade ();
//		if (lvs != null) {
//			if (GUILayout.Button ("lvs")) {
//				UpgradeLivestock ();
//			}
//		}
//
//		GUILayout.EndVertical ();
//	}
//


	#region Custom Method

//	[MenuItem ("CONTEXT/UpgradeManager/Calculate")]
//	private static void Calc ()
//	{
//		LivestockSO so = UpgradeManager.shared ().livestockList.Random ();
//
//		print (so + " : " + UpgradeManager.shared ().GetLivestockUpgradePrice (so).ToStringShort () + " " + UpgradeManager.shared ().GetLivestockSlideValue (so).ToStringShort ());
//	}
//

	#if UNITY_EDITOR

	[UnityEditor.MenuItem ("CONTEXT/UpgradeManager/CheckModLevel")]
	private static void CalculateLevel(){

		for (int i = 0; i < 10; i++) {
			Debug.Log(UpgradeManager.shared().GetCalculateModPriceBasedOnLevel(i+1));
		}
	}


	[UnityEditor.MenuItem ("CONTEXT/UpgradeManager/Calculate")]
	private static void Calculate(){
		BigInteger totalCostEnv = 0;
		BigInteger totalCostLvs = 0;

		foreach (EnvironmentSO item in UpgradeManager.shared().environmentList) {
			totalCostEnv += UpgradeManager.shared().GetEnvironmentUpgradePrice(item);
		}

		foreach (LivestockSO item in UpgradeManager.shared().livestockList) {
			Debug.Log(UpgradeManager.shared().GetLivestockUpgradePrice(item));
			totalCostLvs += UpgradeManager.shared().GetLivestockUpgradePrice(item);
		}

		Debug.Log(totalCostEnv + " " + totalCostEnv.ToStringShort());
		Debug.Log(totalCostLvs + " " + totalCostLvs.ToStringShort());
		Debug.Log(totalCostEnv+totalCostLvs);
		Debug.Log((totalCostEnv+totalCostLvs).ToStringShort());

	}

	[UnityEditor.MenuItem ("CONTEXT/UpgradeManager/Generate Upgrades")]
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

	#endif





	#endregion

}
