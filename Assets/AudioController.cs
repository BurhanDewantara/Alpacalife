using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class AudioController : SingletonMonoBehaviour<AudioController>{

	public AudioClip button;

	public void PlayButtonClick()
	{
		AudioSource.PlayClipAtPoint (button, Camera.main.transform.position);	
	}
}
