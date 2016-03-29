using UnityEngine;
using System.Collections;
using TMPro;

public class BarnAreaHandler : CorralAreaHandler {


	override public void SetColor(ColorSO colorObject)
	{
		this.colorObject = colorObject;
		this.GetComponentInChildren<TextMeshProUGUI>().text = colorObject.colorType.ToString();
	}

	override public bool IsEqual(ColorSO color)
	{
		return (colorObject.colorType == color.colorType);
	}

}
