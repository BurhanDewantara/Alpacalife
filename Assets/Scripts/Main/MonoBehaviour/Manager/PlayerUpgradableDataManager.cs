using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class PlayerUpgradableDataManager : Singleton<PlayerUpgradableDataManager> {

	//it only store LEVEL!
	public Hashtable playerUpgradableData;

	public PlayerUpgradableDataManager ()
	{
		Reset ();
	}

	public void Reset ()
	{
		Load ();
	}
	public void Save()
	{
		GameManager.shared ().GameUpgradableData = playerUpgradableData;
	}

 	public void Load()
	{
		playerUpgradableData = GameManager.shared ().GameUpgradableData;
		if (playerUpgradableData == null) {
			playerUpgradableData = new Hashtable();

			playerUpgradableData.Add(UpgradableType.CanMultiplier,0);
			playerUpgradableData.Add(UpgradableType.ChancesBonusCan,0);
			playerUpgradableData.Add(UpgradableType.ChancesBonusGem,0);
			playerUpgradableData.Add(UpgradableType.ComboShorter,0);
			playerUpgradableData.Add(UpgradableType.PaperSlideSpeed,0);
			playerUpgradableData.Add(UpgradableType.PermanentTime,0);
			playerUpgradableData.Add(UpgradableType.ReduceMistakePaperCost,0);
			playerUpgradableData.Add(UpgradableType.ReduceUpgradeCost,0);

		}
	}

	public void Upgrade(UpgradableType key, int up = 1)
	{
		if (playerUpgradableData.ContainsKey(key))
		{	
			int currentValue =(int) playerUpgradableData[key];
			currentValue +=up;
			playerUpgradableData[key] = currentValue;
			this.Save ();
		}
	}

	public int GetUpgradeDataLevel(UpgradableType key)
	{
		if (playerUpgradableData.ContainsKey(key))
		{	
			return (int)playerUpgradableData[key];
		}
		return 0;
	}


	public override string ToString ()
	{
		string str = "";
		foreach (DictionaryEntry upgrade in playerUpgradableData) {
			str +=  upgrade.Key.ToString() + " " + upgrade.Value.ToString () + "\n" ;
		}
		return str;
	}
}
