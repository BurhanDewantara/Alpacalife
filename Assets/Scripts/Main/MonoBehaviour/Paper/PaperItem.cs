using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PaperItem 
{
	public string paperTitle;
	public int paperValue;

	public SOColor colorTint;
	public SOColor colorText;
	public Sprite paperImage;

	public PaperItem(SOColor soColorTint, SOColor soColorText, Sprite sprite, int value, string title)
	{
		this.colorTint = soColorTint;
		this.colorText = soColorText;
		this.paperValue = value;

		this.paperImage = sprite;
		this.paperTitle = GeneratePaperNameColor(colorTint,title,colorText.colorType.ToString());
	}

	string GeneratePaperNameColor(SOColor soColor, string aTitle, string aColorText)
	{
		string colorToText = soColor.ColorToRichText (aColorText);
		return  aTitle.Replace ("[XXX]", colorToText);
	}


}
