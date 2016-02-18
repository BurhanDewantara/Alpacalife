using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using TMPro;

public class TitleSceneManager : MonoBehaviour {

	public GameObject splashSceneObject;
	public GameObject titleSceneObject;

	private bool isDone = false;

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
		StartCoroutine(Login());
	}

	IEnumerator Login()
	{
		yield return new WaitForSeconds(Random.Range(4.0f,5.0f));
		GameManager.shared().Authenticate((bool success) => {
			if(success)
			{
				Debug.Log("berhasil");
			}
			else
			{
				Debug.Log("gagal");
			}
			isDone = true;
		});
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
			dotCount%=4;
			yield return new WaitForSeconds(0.5f);
		}

		SceneManager.LoadScene("Scene.Game");
	}
}
