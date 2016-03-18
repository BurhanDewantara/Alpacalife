using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum infoDataType
{
	Statistic,
	Cheat,
}

public class GameDataHelper : MonoBehaviour {

	public bool isVisible;

	List<bool> infoData = new List<bool>();
	List<Vector2> infoDataScroll = new List<Vector2>();


	public void Awake ()
	{
		for (int i = 0; i < System.Enum.GetNames(typeof(infoDataType)).Length; i++) {
			infoData.Add (true);
			infoDataScroll.Add (new Vector2());
		}
	}
	void Reset()
	{
		GameDataManager.shared().Reset();
		UpgradeManager.shared().Reset();
		CurrencyManager.shared().Reset();
		PlayerStatisticManager.shared().Reset();

	}



	void OnGUI()
	{
		if(!isVisible) return;

			GUILayout.BeginHorizontal ("box");
			for (int i = 0; i < System.Enum.GetNames(typeof(infoDataType)).Length; i++) {
				infoData [i] = GUILayout.Toggle (infoData [i], ((infoDataType)i).ToString());
			}
			GUILayout.EndHorizontal ();


		if(infoData[(int)infoDataType.Statistic])
		{
			infoDataScroll[(int)infoDataType.Statistic] = GUILayout.BeginScrollView(infoDataScroll[(int)infoDataType.Statistic],"box");
			GUILayout.Label("Total Time Played : " + PlayerStatisticManager.shared().TotalPlayTime + " s");
			GUILayout.Label("Total Game Played : " + PlayerStatisticManager.shared().TotalBitten);
			GUILayout.Label("Animal Level : " + UpgradeManager.shared().LivestockLevel);
			GUILayout.Label("Environment Level : " + UpgradeManager.shared().EnvironmentLevel);
			GUILayout.Label("Highest Score : " + GameDataManager.shared().PlayerBestScore);
			GUILayout.Label("Total animal jump : " + PlayerStatisticManager.shared().TotalJump);
			GUILayout.Label("Total earned gold : " + PlayerStatisticManager.shared().TotalGold);
			GUILayout.Label("Max earned gold in 1 game : " + PlayerStatisticManager.shared().TotalGoldEarn1Game);
			GUILayout.Label("Total money spent : " + PlayerStatisticManager.shared().TotalGoldSpent);
			GUILayout.EndScrollView();
		}
		if(infoData[(int)infoDataType.Cheat])
		{
			infoDataScroll[(int)infoDataType.Cheat] = GUILayout.BeginScrollView(infoDataScroll[(int)infoDataType.Cheat],"box");
			if(GUILayout.Button("Reset Data")){
				Reset();
			}

			GUILayout.BeginHorizontal ();
			{
				if(GUILayout.Button("Lvs Up")){
					UpgradeManager.shared().UpgradeLivestock();
				}

				if(GUILayout.Button("Ënv Up")){
					UpgradeManager.shared().UpgradeEnvironment();
					WorldManager.shared ().RefreshEnvironment ();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal ();
			{
				if(GUILayout.Button("Coin Up 100")){
					CurrencyManager.shared().AddGold(100);
				}
				if(GUILayout.Button("Coin Up 100K")){
					CurrencyManager.shared().AddGold(100000);
				}
				if(GUILayout.Button("Coin Up 100M")){
					CurrencyManager.shared().AddGold(100000000);
				}
				if(GUILayout.Button("Coin x 2 ")){
					CurrencyManager.shared().AddGold(CurrencyManager.shared().playerMoney * 2);
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.EndScrollView();
		}


	}

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




}


