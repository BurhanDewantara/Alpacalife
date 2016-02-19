using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class SceneSplasher : MonoBehaviour
{

	public delegate void SceneSplasherDelegate ();

	public event SceneSplasherDelegate OnSceneSplashEndCompleted;

	public List<Sprite> splashImage;
	public float fadeSpeed = 2.0f;
	public float fadeHoldTime = 3.0f;

	private int spriteIdx;
	private Image image;
	private Color clearWhite =  new Color(1.0f,1.0f,1.0f,0.0f);

	public void Awake ()
	{
		image = this.GetComponent<Image> ();
		spriteIdx = 0;
	
	}

	public void Play()
	{
		StartCoroutine(Splash());
	}

	private IEnumerator Splash()
	{
		while (spriteIdx < splashImage.Count) {
			image.enabled = true;
			if (image.sprite != splashImage [spriteIdx]) {
				image.sprite = splashImage [spriteIdx];
				image.color = clearWhite;
			}	

			while(image.color.a <= 0.97f) {
				yield return new WaitForEndOfFrame();
				image.color = Color.Lerp (image.color, Color.white, Time.deltaTime * fadeSpeed);				
			}
			yield return new WaitForSeconds(fadeHoldTime);

			while(image.color.a >= 0.03f) {
				yield return new WaitForEndOfFrame();
				image.color = Color.Lerp (image.color, clearWhite, Time.deltaTime * fadeSpeed);				
			}
			spriteIdx++;
		}

		if(OnSceneSplashEndCompleted!=null)
			OnSceneSplashEndCompleted();

	}

}

