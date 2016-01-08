using UnityEngine;
using System.Collections;

	[RequireComponent(typeof(GUITexture))]
	public class SceneFader : MonoBehaviour
	{
		public delegate void SceneFaderDelegate ();

		public event SceneFaderDelegate OnFadeOutCompleted;
		public event SceneFaderDelegate OnFadeInCompleted;

		public float fadeSpeed = 5.0f;
		public bool isFadingOut;
		public bool isFadingIn;

		void Awake ()
		{
			GetComponent<GUITexture>().texture = new Texture2D (Screen.width, Screen.height);
			GetComponent<GUITexture>().pixelInset = new Rect (0, 0, Screen.width, Screen.height);
			GetComponent<GUITexture>().color = Color.clear;
			GetComponent<GUITexture>().enabled = false;
		}

		void FadeToClear ()
		{
			GetComponent<GUITexture>().color = Color.Lerp (GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
		}

		void FadeToBlack ()
		{
			GetComponent<GUITexture>().color = Color.Lerp (GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
		}

		void Update ()
		{
			if (isFadingIn) {
				FadeIn ();
			}


			if (isFadingOut) {
				FadeOut ();
			}
		}
		 
		public void FadeIn ()
		{
			GetComponent<GUITexture>().enabled = true;
			isFadingIn = true;
			FadeToClear ();
			if (GetComponent<GUITexture>().color.a <= .05f) {
			
				GetComponent<GUITexture>().color = Color.clear;
				GetComponent<GUITexture>().enabled = false;
				isFadingIn = false;
				if (OnFadeInCompleted != null)
					OnFadeInCompleted ();
			}

		}

		public void FadeOut ()
		{
			GetComponent<GUITexture>().enabled = true;
			isFadingOut = true;
			FadeToBlack ();

			if (GetComponent<GUITexture>().color.a >= 0.95f) {
				GetComponent<GUITexture>().color = Color.black;
				isFadingOut = false;
				if (OnFadeOutCompleted != null)
					OnFadeOutCompleted ();
			}


		}
}

