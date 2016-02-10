using UnityEngine;
using System.Collections;
using TMPro;

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
		bestScoreText.text = bestScore.ToString();	
		scoreText.text = "";	
	}

	public void SetScore(int currScore,int bScore)
	{
		Init ();
		StartCoroutine(ScoreCounter(scoreText,currScore));
	}


	IEnumerator ScoreCounter(TextMeshProUGUI textObject,int toScore)
	{

		float percentage = 0;
		float duration = 2.0f;
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