using UnityEngine;
using System.Collections;
using ScottGarland;
using TMPro;

public class MultiplierHandler : MonoBehaviour {

	public TextMeshProUGUI textObject;
	// Use this for initialization
	void Start () {
		SetValue(UpgradeManager.shared().GetCurrentMultiplier(),0);
		UpgradeManager.shared().OnMultiplierUpdated+= HandleOnMultiplierUpdated;
	}

	void SetValue(BigInteger curr, BigInteger delta)
	{
		StopAllCoroutines();
		StartCoroutine(ValueUpdate(curr,delta));
	}

	IEnumerator	ValueUpdate(BigInteger curr, BigInteger delta)
	{
		float percentage = 0;
		float duration = 0.1f;
		float startTime = Time.time;

		while (percentage < 1.0f) {
			percentage = (Time.time - startTime) / duration;
			percentage = Mathf.Clamp01 (percentage);
			int bigpercent =  Mathf.CeilToInt(percentage * 1000);

			BigInteger inText = curr + (delta * bigpercent / 1000);
			textObject.text = "x"+inText.ToStringShort();
			yield return new WaitForEndOfFrame ();
		}
	}


	void HandleOnMultiplierUpdated (BigInteger curr, BigInteger delta)
	{
		StopAllCoroutines();
		StartCoroutine(ValueUpdate(curr,delta));
	}

//	void OnGUI()
//	{
//		if(GUILayout.Button("up"))
//		{
//			UpgradeManager.shared().UpgradeEnvironment();
//		}
//	}
}
