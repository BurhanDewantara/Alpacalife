using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class ImageRandomizer : MonoBehaviour {

	public List<Sprite> sprites;

	void Start()
	{
		this.GetComponent<Image>().sprite = sprites.Random();
	}

}
