using UnityEngine;
using System.Collections;
using Artoncode.Core;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using ScottGarland;


public class GameManager : Singleton<GameManager>
{
	
	public GameManager ()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()
			.EnableSavedGames ()
			.RequireGooglePlus()
			.Build ();

		PlayGamesPlatform.InitializeInstance (config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate ();
	}

	public void Authenticate (System.Action<bool> callback)
	{
		Social.localUser.Authenticate (callback);
	}

	public void ShowAchievementBoard ()
	{
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
		Social.ReportScore (score, GPGIds.leaderboard_paca_leader, delegate(bool success) {
		});
	}


	public static void TriggerWatchVideo ()
	{
		Social.ReportProgress (GPGIds.achievement_paca_can_watch, 100.0f, delegate(bool success) {
		});
	}

	public static void TriggerTotalEarnMoneyInOneAchievement (BigInteger value)
	{
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

	public static void TriggerTotalJumpInOneAchievement (long value)
	{
		Social.ReportProgress (GPGIds.achievement_jumper_novice, value / 20.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_beginner, value / 50.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_expert, value / 100.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jumper_master, value / 200.0f, delegate(bool success) {
		});
	}


	public static void TriggerTotalJumpAchievement (long value)
	{
		Social.ReportProgress (GPGIds.achievement_jump_1st_time, 100.0f, delegate(bool success) {
		});

		Social.ReportProgress (GPGIds.achievement_can_you_jump, value / 500.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_oh_you_can_jump, value / 1000.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_jump_for_your_health, value / 5000.0f, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_never_skip_leg_day, value / 10000.0f, delegate(bool success) {
		});
	}


	public static void TriggerIncrementalUpgradeEnvironmentAchievement ()
	{
		Social.ReportProgress (GPGIds.achievement_my_pretty_farm, 100.0f, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_farm_sweet_farm, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_farm_is_mine, 1, delegate(bool success) {
		});
	}

	public static void TriggerIncrementalUpgradePacaAchievement ()
	{
		Social.ReportProgress (GPGIds.achievement_my_own_paca, 100.0f, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_10_little_paca, 1, delegate(bool success) {
		});
		PlayGamesPlatform.Instance.IncrementAchievement (GPGIds.achievement_paca_is_love_paca_is_life, 1, delegate(bool success) {
		});
	}

	public static void TriggerIncrementalEatbyWolfAchievement ()
	{
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
