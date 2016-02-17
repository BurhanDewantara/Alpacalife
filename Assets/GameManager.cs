using UnityEngine;
using System.Collections;
using Artoncode.Core;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class GameManager : Singleton<GameManager>
{
	public GameManager()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();

//		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//			// enables saving game progress.
//			.EnableSavedGames()
//			// registers a callback to handle game invitations received while the game is not running.
//			.WithInvitationDelegate(<callback method>)
//			// registers a callback for turn based match notifications received while the
//			// game is not running.
//			.WithMatchDelegate(<callback method>)
//			// require access to a player's Google+ social graph to sign in
//			.RequireGooglePlus()
//			.Build();


	}

	public void Authenticate()
	{
		Social.localUser.Authenticate((bool success) => {
			if(success)
			{}
			else
			{}
		});
	}

//	public static void ()
//	{
//		Social.ReportProgress(GPGIds.
//	}

}
