using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using TMPro;
using ScottGarland;

public class GameOverController : MonoBehaviour {

	public GameObject gameoverPanel;
	public TextMeshProUGUI bestScoreText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI freeCoinText;



	public GameObject bonusCoinButton;
	public GameObject homeButton;

	private int bestScore = 80;

	private BigInteger moneyResult;

	void OnEnable ()
	{
		Init ();

	}

	void Init()
	{
		scoreText.text = "0";	
		SetBonusCoinButton ();
	}


	void SetBonusCoinButton()
	{
		bonusCoinButton.GetComponent<Button> ().interactable = true;
		LivestockSO anyLvs = UpgradeManager.shared ().ownedLivestockList.Random ();
		BigInteger anyPrice  = UpgradeManager.shared().GetLivestockSlideValue(anyLvs);
		BigInteger multi = UpgradeManager.shared ().GetCurrentMultiplier ();
		int randomFactor = Random.Range (20, 30);

		moneyResult = anyPrice * multi * randomFactor;
		bonusCoinButton.GetComponentInChildren<TextMeshProUGUI> ().text = moneyResult.ToStringShort ();

		bonusCoinButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		bonusCoinButton.GetComponent<Button> ().onClick.AddListener (delegate() {

			//TODO: Show ADS HERE
			ShowRewardedAd();
//			CurrencyManager.shared().AddGold(result);
			bonusCoinButton.GetComponent<Button> ().interactable = false;	
		});


	}


	public void SetScore(int currScore,int bScore)
	{
		bestScore = bScore;
		bestScoreText.text = bestScore.ToString();	
		Init ();
		StartCoroutine(ScoreCounter(scoreText,currScore));
	}


	IEnumerator ScoreCounter(TextMeshProUGUI textObject,int toScore)
	{
		float percentage = 0;
		float duration = 1.0f;
		float startTime = Time.time;

		while (percentage < 1.0f) {
			percentage = (Time.time - startTime) / duration;
			percentage = Mathf.Clamp01 (percentage);

			int currScore = Mathf.FloorToInt(toScore * percentage);
			textObject.text = currScore.ToString();

			if (currScore > bestScore) {
				bestScore = currScore;
				bestScoreText.text = currScore.ToString();
			}

			yield return new WaitForEndOfFrame ();
		}


	}

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			ShowOptions options = new ShowOptions{ resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");

			GPGManager.TriggerWatchVideo();

			bonusCoinButton.GetComponent<AudioSource>().Play();
			CurrencyManager.shared().AddGold(moneyResult);
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

}

