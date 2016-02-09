﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EnvironmentDrawer : MonoBehaviour {

	public EnvironmentType type;
	public void SetSprite(Sprite sprite)
	{
		this.GetComponent<SpriteRenderer> ().sprite = sprite;
		this.GetComponent<SpriteRenderer> ().color = (sprite == null) ? Color.clear : Color.white;
	}


}
