using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using TMPro;

public class TitleSceneManager : MonoBehaviour {

	public GameObject splashSceneObject;
	public GameObject titleSceneObject;
	public GameObject versionObject;
	public string progressText = "Loading";


	private bool isDone = false;

	public void Start()
	{
		titleSceneObject.SetActive(false);
		splashSceneObject.GetComponent<SceneSplasher>().Play();
		splashSceneObject.GetComponent<SceneSplasher>().OnSceneSplashEndCompleted += SceneSplashCompleteHandler;
		GPGManager.shared().Activate();
		FlurryManager.shared().Activate();
		versionObject.GetComponent<TextMeshProUGUI>().text = "v:"+BundleVersion.GetVersion();
	}

	void SceneSplashCompleteHandler ()
	{
		titleSceneObject.SetActive(true);
		StartCoroutine(LoadingTextAnimation());
	
		progressText = "Authenticate";
		Social.localUser.Authenticate ((bool success) => {
			if(success)
			{
//				progressText = "To the Main";
			}
			else
			{
//				progressText = "gagal";
			}
			isDone = true;
		});	
	}



	IEnumerator LoadingTextAnimation()
	{
		GameObject loadingTextObject = titleSceneObject.transform.FindChild("LoadingText").gameObject;
		int dotCount = 0;

		while (!isDone) {

			string currText = progressText;
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
