using UnityEngine;
using System.Collections;
using TMPro;

public class PopAndFade : MonoBehaviour {

	float startTime;
	float currTime;
	bool isUpdate;

	public void SetText(string text)
	{
		this.GetComponent<TextMeshProUGUI> ().text = text;
	}

	public void PopUp()
	{
		iTween.MoveAdd(this.gameObject,
			iTween.Hash(
				"y",100,
				"time",1,
				"easeType",iTween.EaseType.easeOutQuad
			)
		);

		StartCoroutine (fade(0.5f));
	}

	IEnumerator fade(float delay)
	{
		yield return new WaitForSeconds (delay);
		float percentage = 0;
		float duration = 1.0f;
		float startTime = Time.time;


		while (percentage < 1.0f) {
			percentage = (Time.time - startTime) / duration;
			percentage = Mathf.Clamp01 (percentage);

			this.GetComponent<TextMeshProUGUI> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f - (1.0f * percentage));

			yield return new WaitForEndOfFrame ();
		}
		Destroy (this.gameObject,0.5f);
	}


}
