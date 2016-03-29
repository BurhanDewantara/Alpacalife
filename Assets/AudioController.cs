using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class AudioController : SingletonMonoBehaviour<AudioController>{

	public AudioClip button;
	public AudioClip error;

	public void PlayButtonError()
	{
		AudioSource.PlayClipAtPoint (error, Camera.main.transform.position);	
	}
	public void PlayButtonClick()
	{
		AudioSource.PlayClipAtPoint (button, Camera.main.transform.position);	
	}
}
