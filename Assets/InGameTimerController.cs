using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameTimerController : MonoBehaviour {

	public GameObject frameObject;
	public Color frameBlinkColor;
	public GameObject fluidObject;
	public Color fluidBlinkColor;

	public bool isStart = false;

	private float maxTimer = 5.0f;
	private float minTimer = 1.0f;

	private Slider timerSlider {
		get{
			return this.GetComponent<Slider>();
		}
	}



	private IEnumerator Blink()
	{
		frameObject.GetComponent<Image>().color = frameBlinkColor;
		fluidObject.GetComponent<Image>().color = fluidBlinkColor;

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		frameObject.GetComponent<Image>().color = Color.white;
		fluidObject.GetComponent<Image>().color = Color.white;
	}

	public void AddTimer(float second = 1.0f)
	{
		timerSlider.value += second;
		ShortenMaxTimer();
		StartCoroutine(Blink());
	}

	private void ShortenMaxTimer()
	{
		timerSlider.maxValue -= timerSlider.maxValue/10;
		timerSlider.maxValue = Mathf.Clamp(timerSlider.maxValue,minTimer,maxTimer);
	}

	void Update()
	{
		timerSlider.value -= Time.deltaTime;
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(0,20,100,20),"Add Time"))
		{
			AddTimer();
		}


	}
}
