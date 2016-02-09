using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorSpeedRandomizer : MonoBehaviour {

	private float minAnimationSpeed = 0.7f;
	private float maxAnimationSpeed = 1.0f;

	void Start () {
		this.GetComponent<Animator>().speed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
	}
}
