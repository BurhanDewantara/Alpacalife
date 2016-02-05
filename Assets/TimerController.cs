using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour {


	public delegate void TimerControllerDelegate(GameObject sender);
	public event TimerControllerDelegate OnTimeReachZero;

	public GameObject frameObject;
	public Color frameBlinkColor;
	public GameObject fluidObject;
	public Color fluidBlinkColor;

	private float maxValue = 10.0f;
	private float minValue = 2.0f;

	private Slider timerSlider {
		get{
			return this.GetComponent<Slider>();
		}
	}

	void Awake()
	{
		timerSlider.minValue = 0;
		timerSlider.maxValue = maxValue;
		timerSlider.value = maxValue;
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

	public void AddTime(float second = 1.0f)
	{
		timerSlider.value += second;
		ShortenMaxTimer();
		StartCoroutine(Blink());
	}

	private void ShortenMaxTimer()
	{
		timerSlider.maxValue -= timerSlider.maxValue/10;
		timerSlider.maxValue = Mathf.Clamp(timerSlider.maxValue,minValue,maxValue);
	}

	public void UpdateTime()
	{

		timerSlider.value -= Time.deltaTime;
		if(timerSlider.value <=0)
		{
			if (OnTimeReachZero !=null) {
				OnTimeReachZero (this.gameObject);
			}
		}
	}


}
