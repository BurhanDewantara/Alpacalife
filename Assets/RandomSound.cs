using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour {

	public List<AudioClip> clips;


	private AudioSource audio{
		get{
			return this.GetComponent<AudioSource>();
		}
	}

	public void Play()
	{
		this.audio.clip = clips.Random();
		this.audio.Play();
	}

}
