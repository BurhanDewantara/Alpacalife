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


	public static void TriggerTutorial()
	{
		if(!Social.localUser.authenticated)return;

		Social.ReportProgress (GPGIds.achievement_practice_makes_perfect, 100.0f, delegate(bool success) {
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

		float _50M = value.DivideWith(new BigInteger(50000000))*100.0f;
		float _500K = value.DivideWith(new BigInteger(500000))*100.0f;
		float _5K = value.DivideWith(new BigInteger(5000))*100.0f;
		float _50 = value.DivideWith(new BigInteger(50))*100.0f;


		Social.ReportProgress (GPGIds.achievement_novice_billionaire, _50, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_beginner_billionaire, _5K, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_expert_billionaire, _500K, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_master_billionaire, _50M, delegate(bool success) {
		});
	}


	public static void TriggerTotalEarnMoneyAchievement (BigInteger value)
	{
		if(!Social.localUser.authenticated)return;

		float _1B = value.DivideWith(new BigInteger(1000000000))*100.0f;
		float _10M = value.DivideWith(new BigInteger(10000000))*100.0f;
		float _100K = value.DivideWith(new BigInteger(100000))*100.0f;
		float _1K = value.DivideWith(new BigInteger(1000))*100.0f;

		Social.ReportProgress (GPGIds.achievement_novice_billionaire, _1K, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_beginner_billionaire, _100K, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_expert_billionaire, _10M, delegate(bool success) {
		});
		Social.ReportProgress (GPGIds.achievement_master_billionaire, _1B, delegate(bool success) {
		});

	}

	public static void TriggerTotalJumpInOneAchievement (BigInteger value)
	{

		if(!Social.localUser.authenticated)return;

		float _20 = value.DivideWith(new BigInteger(20))*100.0f;
		float _50 = value.DivideWith(new BigInteger(50))*100.0f;
		float _100 = value.DivideWith(new BigInteger(100))*100.0f;
		float _200 = value.DivideWith(new BigInteger(200))*100.0f;

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

		float _500 = value.DivideWith(new BigInteger(500))*100.0f;
		float _1000 = value.DivideWith(new BigInteger(1000))*100.0f;
		float _5000 = value.DivideWith(new BigInteger(5000))*100.0f;
		float _10000 = value.DivideWith(new BigInteger(10000))*100.0f;

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
