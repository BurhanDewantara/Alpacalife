using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRandomizer : MonoBehaviour {

	public List<Sprite> sprites;

	void Start () {
		this.GetComponent<SpriteRenderer>().sprite = sprites.Random();
	}
}

