using UnityEngine;
using System.Collections;
using TMPro;
using GooglePlayGames;

public class TitleSceneManager : MonoBehaviour {

	public GameObject splashSceneObject;
	public GameObject titleSceneObject;

	private bool isDone;

	public void Start()
	{
		titleSceneObject.SetActive(false);
		splashSceneObject.GetComponent<SceneSplasher>().Play();
		splashSceneObject.GetComponent<SceneSplasher>().OnSceneSplashCompleted += SceneSplashCompleteHandler;

	}

	void SceneSplashCompleteHandler ()
	{
		titleSceneObject.SetActive(true);
		StartCoroutine(LoadingTextAnimation());

	}

	IEnumerator LoadingTextAnimation()
	{
		GameObject loadingTextObject = titleSceneObject.transform.FindChild("LoadingText").gameObject;
		string dText = "Loading";
		int dotCount = 0;
		while (!isDone) {

			string currText = dText;
			for (int i = 0; i < dotCount; i++) {
				currText += ".";
			}
			loadingTextObject.GetComponent<TextMeshProUGUI>().text = currText;
			dotCount++;
			dotCount %=4;
			yield return new WaitForSeconds(.5f);
		}
	}
}
