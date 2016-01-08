using UnityEngine;
using System.Collections;

namespace Artoncode.Core.UI
{
	public class Fader : MonoBehaviour
	{
		public delegate void FaderDelegate (GameObject sender);

		public event FaderDelegate OnFadeInStart;
		public event FaderDelegate OnFadeInEnd;
		public event FaderDelegate OnFadeOutStart;
		public event FaderDelegate OnFadeOutEnd;


		public float fadeTime = 1;
		public float fadeSpeed = 2;
		public Color fadeColor = Color.black;

		private Material m_Material = null;
		private bool m_FadingIn = false;
		private bool m_FadingOut = false;


		private void Awake ()
		{
			m_Material = new Material ("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
		}
	
		private void DrawQuad (Color aColor, float aAlpha)
		{
			aColor.a = aAlpha;
			m_Material.SetPass (0);
			GL.Color (aColor);
			GL.PushMatrix ();
			GL.LoadOrtho ();
			GL.Begin (GL.QUADS);
			GL.Vertex3 (0, 0, -1);
			GL.Vertex3 (0, 1, -1);
			GL.Vertex3 (1, 1, -1);
			GL.Vertex3 (1, 0, -1);
			GL.End ();
			GL.PopMatrix ();
		}

		private IEnumerator FadeOutCoroutine (float aFadeOutTime, Color aColor)
		{
			float t = 0.0f;
			if (OnFadeOutStart != null)
				OnFadeOutStart (this.gameObject);		
			while (t<1.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t + Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeOutEnd != null)
				OnFadeOutEnd (this.gameObject);		

			float time = 0;
			while(m_FadingOut || time < aFadeOutTime)
			{
				time+= Time.deltaTime;
				yield return new WaitForEndOfFrame ();
			}

		}

		private IEnumerator FadeInCoroutine (float aFadeInTime, Color aColor)
		{
			float t = 1.0f;
			if (OnFadeInStart != null)
				OnFadeInStart (this.gameObject);		
			while (t>0.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t - Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeInEnd != null)
				OnFadeInEnd (this.gameObject);		

			float time = 0;
			while(m_FadingIn || time < aFadeInTime)
			{
				time+= Time.deltaTime;
				yield return new WaitForEndOfFrame ();
			}
		}



		private IEnumerator FadeCoroutine (float fadeDuration, Color aColor)
		{

			//FADE OUT AND FADE IN  in 1 time
			float t = 0.0f;
			if (OnFadeOutStart != null)
				OnFadeOutStart (this.gameObject);		
			while (t<1.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t + Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeOutEnd != null)
				OnFadeOutEnd (this.gameObject);		

			float time = 0;
			while(m_FadingOut && time < fadeDuration)
			{
				yield return new WaitForEndOfFrame ();
				DrawQuad (aColor, t);
				time+= Time.deltaTime;
			}


			if (OnFadeInStart != null)
				OnFadeInStart (this.gameObject);		
			while (t>0.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t - Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeInEnd != null)
				OnFadeInEnd (this.gameObject);		

			m_FadingOut = false;
			m_FadingIn = false;
		}

		public void FadeStart( float aFadeDuration =-1 )
		{
			if (m_FadingIn) return;
			if (m_FadingOut) return;
			m_FadingOut = true;
			m_FadingIn = true;

			if (aFadeDuration == -1)
				aFadeDuration = 999;
			StartCoroutine (FadeCoroutine (aFadeDuration, Color.black));
		}

		public void FadeStop()
		{
			m_FadingOut = false;
			m_FadingIn = false;
		}

		public void FadeIn ()
		{
			FadeIn (fadeTime, fadeColor);
		}

		public void FadeOut ()
		{
			FadeOut (fadeTime, fadeColor);
		}

		public void FadeIn (float aFadeInTime,Color aColor)
		{
			if (m_FadingIn)
				return;

			m_FadingOut = false;
			m_FadingIn = true;
			StartCoroutine (FadeInCoroutine (aFadeInTime, aColor));
			Debug.Log("in");
		}

		public void FadeOut (float aFadeOutTime, Color aColor)
		{
			if (m_FadingOut)
				return;

			m_FadingIn = false;
			m_FadingOut = true;
			StartCoroutine (FadeOutCoroutine (aFadeOutTime, aColor));
			Debug.Log("out");

		}

	
	}
}
