using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class SceneSplasher : MonoBehaviour
{

	public delegate void SceneSplasherDelegate ();

	public event SceneSplasherDelegate OnSceneSplashCompleted;

	public List<Sprite> splashImage;
	public float fadeSpeed = 2.0f;
	public float fadeHoldTime = 3.0f;
	public float fadeTimer;



	private int spriteIdx;
	private Image image;
	private bool fadeIn;
	private bool fadeOut;
	private bool isPlaying;
	private Color clearWhite =  new Color(1.0f,1.0f,1.0f,0.0f);

	public void Awake ()
	{
		image = this.GetComponent<Image> ();
		spriteIdx = 0;
		fadeTimer = 0.0f;
		fadeIn = false;
		fadeOut = false;	
		isPlaying = false;	
		Play();
	}

	public void Play()
	{
		isPlaying = true;
	}


	public void FixedUpdate ()
	{
		if (!isPlaying)
			return;

		//if counter still got the image
		if (spriteIdx < splashImage.Count) {

			image.enabled = true;
			if (image.sprite != splashImage [spriteIdx]) {
				image.sprite = splashImage [spriteIdx];
				image.color = clearWhite;
				fadeIn = true;
				fadeOut = false;

			}

			if (fadeIn && !fadeOut) {
				image.color = Color.Lerp (image.color, Color.white, fadeSpeed * Time.deltaTime);
				if (image.color.a >= 0.97f) {
					image.color = Color.white;
					fadeTimer += Time.deltaTime;
					if (fadeTimer >= fadeHoldTime) {
						fadeIn = false;
						fadeOut = true;
						fadeTimer = 0.0f;
					}
				}
			} else if (fadeOut && !fadeIn) {
				image.color = Color.Lerp (image.color, clearWhite, fadeSpeed * Time.deltaTime);
				if (image.color.a <= 0.03f) {
					image.color = Color.clear;
					fadeTimer += Time.deltaTime;
					if (fadeTimer >= fadeHoldTime / 2.0f) {
						fadeIn = false;
						fadeOut = false;
						fadeTimer = 0.0f;

						spriteIdx++;

					}
				}
			}
		} else {
			if (OnSceneSplashCompleted != null)
				OnSceneSplashCompleted ();
		}

	}




}

