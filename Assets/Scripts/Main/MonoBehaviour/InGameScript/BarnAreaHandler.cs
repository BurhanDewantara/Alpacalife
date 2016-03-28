using UnityEngine;
using System.Collections;
using TMPro;

public class BarnAreaHandler : MonoBehaviour {

	public DirectionType fencePosition;
	private ColorSO colorObject;

	public void SetColor(ColorSO colorObject)
	{
		this.colorObject =  colorObject;
		this.GetComponentInChildren<TextMeshProUGUI>().text = colorObject.color.ToString();
//		this.GetComponent<SpriteRenderer>().material.color = colorObject.color;
	}

	public bool IsEqual(ColorSO color)
	{
		return (colorObject.colorType == color.colorType);
	}

}
