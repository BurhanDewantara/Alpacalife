using UnityEngine;
using System.Collections;
using ScottGarland;
using TMPro;

public class CurrencyHandler : MonoBehaviour {

	public TextMeshProUGUI textObject;
	// Use this for initialization
	void Start () {
		SetCurrency(CurrencyManager.shared().playerMoney,0);
		CurrencyManager.shared().OnUpdated += HandleCurrencyManagerDelegate;
	}

	void SetCurrency(BigInteger curr, BigInteger delta)
	{
		StopAllCoroutines();
		StartCoroutine(CurrencyUpdate(curr,delta));
	}

	IEnumerator	CurrencyUpdate(BigInteger curr, BigInteger delta)
	{
		float percentage = 0;
		float duration = 0.1f;
		float startTime = Time.time;

		while (percentage < 1.0f) {
			percentage = (Time.time - startTime) / duration;
			percentage = Mathf.Clamp01 (percentage);
			int bigpercent =  Mathf.CeilToInt(percentage * 1000);

			BigInteger inText = curr + (delta * bigpercent / 1000);
			textObject.text = inText.ToStringShort();
			yield return new WaitForEndOfFrame ();
		}
	}


	void HandleCurrencyManagerDelegate (BigInteger curr, BigInteger delta)
	{
		StopAllCoroutines();
		StartCoroutine(CurrencyUpdate(curr,delta));
	}

	void OnGUI()
	{
		if(GUILayout.Button("+100000"))
		{
			CurrencyManager.shared().AddGold(100000);
		}
		if(GUILayout.Button("-777"))
		{
			CurrencyManager.shared().PayGold(777);
		}
	}
}
