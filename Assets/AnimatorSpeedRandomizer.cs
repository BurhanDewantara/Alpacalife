using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorSpeedRandomizer : MonoBehaviour {

	public float minAnimationSpeed =0.8f;
	public float maxAnimationSpeed =1.2f;

	void Start () {
		this.GetComponent<Animator>().speed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
	}
}
