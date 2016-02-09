using UnityEngine;
using System.Collections;

public class FenceAreaHandler : MonoBehaviour {

	public DirectionType fencePosition;
	private ColorSO colorObject;

	public void SetColor(ColorSO colorObject)
	{
		this.colorObject =  colorObject;
		this.GetComponent<SpriteRenderer>().material.color = colorObject.color;
	}

	public bool IsEqual(ColorSO color)
	{
		return (colorObject.colorType == color.colorType);
	}

}
