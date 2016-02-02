using UnityEngine;
using System.Collections;
using TMPro;

public class LivestockColorHandler : MonoBehaviour {

	public TextMeshProUGUI textObject;

	private SOColor color;

	public void SetColor(SOColor color, string word)
	{
		this.color = color;
		textObject.text = color.TintText (word);
	}

}
