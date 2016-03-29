using UnityEngine;
using System.Collections;
using TMPro;

public class CorralAreaHandler : MonoBehaviour {

	public DirectionType fencePosition;
	protected ColorSO colorObject;

	virtual public void SetColor(ColorSO colorObject)
	{
		this.colorObject = colorObject;
		this.GetComponent<SpriteRenderer>().material.color = colorObject.color;
	}

	virtual public bool IsEqual(ColorSO color)
	{
		
		return (colorObject.colorType == color.colorType);
	}

}
