﻿using UnityEngine;
using System.Collections;

public class FenceAreaHandler : MonoBehaviour {

	public DirectionType fencePosition;
	private SOColor colorObject;

	public void SetColor(SOColor colorObject)
	{
		this.colorObject =  colorObject;
		this.GetComponent<SpriteRenderer>().material.color = colorObject.color;
	}

	public bool IsEqual(SOColor color)
	{
		return (colorObject.colorType == color.colorType);
	}

}