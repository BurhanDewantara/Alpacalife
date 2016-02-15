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

		BigInteger result = anyPrice * multi * randomFactor;
		bonusCoinButton.GetComponentInChildren<TextMeshProUGUI> ().text = result.ToStringShort ();

		bonusCoinButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		bonusCoinButton.GetComponent<Button> ().onClick.AddListener (delegate() {
			CurrencyManager.shared().AddGold(result);
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




}