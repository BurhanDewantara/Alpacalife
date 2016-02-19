using UnityEngine;
using System.Collections;
using Artoncode.Core;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using ScottGarland;


public class GPGManager : Singleton<GPGManager>
{

	public void Activate()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
			.EnableSavedGames ()
			.Build ();

		PlayGamesPlatform.InitializeInstance (config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate ();
	}


	public void ShowAchievementBoard ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ShowAchievementsUI ();
	}

	public void ShowLeaderboardUI ()
	{
		if (Social.localUser.authenticated) {
			Social.ShowLeaderboardUI ();		
		} else {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ShowLeaderboardUI ();		
				}
			});			
		}


	}


	public static void PostLeaderBoard (long score)
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportScore (score, GPGIds.leaderboard_paca_leader, delegate(bool success) {
			
		});
	}


	public static void TriggerWatchVideo ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_paca_can_watch, 100.0f, delegate(bool success) {
		});
	}

	public static void TriggerTotalEarnMoneyInOneAchievement (BigInteger value)
	{
		if(!Social.localUser.authenticated)return;

		float fiftM = float.Parse ((value / 50000000).ToString ());
		float fivhK = float.Parse ((value / 500000).ToString ());
		float fiveK = float.Parse ((value / 5000).ToString ());
		float fifty = float.Parse ((value / 50).ToString ());

		Social.ReportProgress (GPGIds.achievement_novice_billionaire, fifty, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_beginner_billionaire, fiveK, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_expert_billionaire, fivhK, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_master_billionaire, fiftM, delegate(bool success) {
		});
	}


	public static void TriggerTotalEarnMoneyAchievement (BigInteger value)
	{
		if(!Social.localUser.authenticated)return;

		float oneB = float.Parse ((value / 1000000000).ToString ());
		float tenM = float.Parse ((value / 10000000).ToString ());
		float hunK = float.Parse ((value / 100000).ToString ());
		float oneK = float.Parse ((value / 1000).ToString ());

		Social.ReportProgress (GPGIds.achievement_novice_billionaire, oneK, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_beginner_billionaire, hunK, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_expert_billionaire, tenM, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_master_billionaire, oneB, delegate(bool success) {
		});

	}

	public static void TriggerTotalJumpInOneAchievement (BigInteger value)
	{

		if(!Social.localUser.authenticated)return;

		float _20 = float.Parse ((value / 20).ToString ());
		float _50 = float.Parse ((value / 50).ToString ());
		float _100 = float.Parse ((value / 100).ToString ());
		float _200 = float.Parse ((value / 200).ToString ());

		Social.ReportProgress (GPGIds.achievement_jumper_novice, _20, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_beginner, _50, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_expert, _100, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_master, _200, delegate(bool success) {
		});
	}

	public static void Trigger1stJumpAchievement ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_jump_1st_time, 100.0f, delegate(bool success) {
		});

	}


	public static void TriggerTotalJumpAchievement (BigInteger value)
	{
		if(!Social.localUser.authenticated)return;

		float _500 = float.Parse ((value / 500).ToString ());
		float _1000 = float.Parse ((value / 1000).ToString ());
		float _5000 = float.Parse ((value / 5000).ToString ());
		float _10000 = float.Parse ((value / 10000).ToString ());

		Social.ReportProgress (GPGIds.achievement_can_you_jump, _500, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_oh_you_can_jump, _1000, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jump_for_your_health, _5000, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_never_skip_leg_day, _10000, delegate(bool success) {
		});
	}


	public static void TriggerIncrementalUpgradeEnvironmentAchievement ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_my_pretty_farm, 100.0f, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_farm_sweet_farm, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_farm_is_mine, 1, delegate(bool success) {
		});
	}

	public static void TriggerIncrementalUpgradePacaAchievement ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_my_own_paca, 100.0f, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_10_little_paca, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_paca_is_love_paca_is_life, 1, delegate(bool success) {
		});
	}

	public static void TriggerIncrementalEatbyWolfAchievement ()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_noob, 100.0f, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_wolf_attack, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_raaaaawr, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_wolf_dinner, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_is_this_wolfs_farm, 1, delegate(bool success) {
		});

	}

}
