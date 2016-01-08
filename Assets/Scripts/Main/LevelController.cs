using UnityEngine;
using System.Collections;
using Artoncode.Core;
using Artoncode.Core.Data;

public class LevelController : SingletonMonoBehaviour<LevelController>
{
	public delegate void LoadDelegate (LevelController sender);

	public event LoadDelegate onLoadStarted;

	private Material m_Material = null;
	private string m_LevelName = "";
	private int m_LevelIndex = 0;
	private bool m_Fading = false;
		
	private void Awake ()
	{
		m_Material = new Material ("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
		StartCoroutine (FadeIn ());

	}
		
	private void DrawQuad (Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		m_Material.SetPass (0);
		GL.PushMatrix ();
		GL.LoadOrtho ();
		GL.Begin (GL.QUADS);
			GL.Color (aColor);
			GL.Vertex3 (0, 0, -1);
			GL.Vertex3 (0, 1, -1);
			GL.Vertex3 (1, 1, -1);
			GL.Vertex3 (1, 0, -1);
		GL.End ();
		GL.PopMatrix ();
	}
		
	IEnumerator FadeIn () {
		float t = 1f;
		while (t > 0.0f) {
			yield return new WaitForEndOfFrame ();
			t = changeTowards (t, 0, 1/0.3f, Time.deltaTime);
			DrawQuad (Color.black, t);
		}
	}

	public float changeTowards (float current, float target, float acceleration, float timeStep) {
		if (current == target) {
			return current;	
		} else {
			float dir = Mathf.Sign (target - current); // must n be increased or decreased to get closer to target
			current += acceleration * timeStep * dir;
			return (dir == Mathf.Sign (target - current)) ? current : target; // if n has now passed target then return target, otherwise return n
		}
	}




	private IEnumerator Fade (float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 0.0f;
		while (t<1.0f) {
			yield return new WaitForEndOfFrame ();
			t = Mathf.Clamp01 (t + Time.deltaTime / aFadeOutTime);
			DrawQuad (aColor, t);
		}
		if (m_LevelName != "") {
			if (onLoadStarted != null)
				onLoadStarted (this);
			Application.LoadLevelAsync (m_LevelName);
		} else {
			if (onLoadStarted != null)
				onLoadStarted (this);
			Application.LoadLevelAsync (m_LevelIndex);
		}

		while (true) {
			yield return new WaitForEndOfFrame ();
			DrawQuad (aColor, t);
		}
//		while (t>0.0f) {
//			yield return new WaitForEndOfFrame ();
//			t = Mathf.Clamp01 (t - Time.deltaTime / aFadeInTime);
//			DrawQuad (aColor, t);
//		}
//		m_Fading = false;
	}

	private void StartFade (float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine (Fade (aFadeOutTime, aFadeInTime, aColor));
	}

	public void LoadLevel (string aLevelName)
	{

		LoadLevel (aLevelName, 1, 1, Color.black);
	}
		
	public void LoadLevel (string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (m_Fading)
			return;
		m_LevelName = aLevelName;

		StartFade (aFadeOutTime, aFadeInTime, aColor);
	}
}

