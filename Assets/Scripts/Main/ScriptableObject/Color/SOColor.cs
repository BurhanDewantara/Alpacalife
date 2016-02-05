using UnityEngine;
using System.Collections;

public enum ColorType
{
	Red		= 1, 
	Green	= 2, 
	Blue	= 3, 
	Yellow 	= 4, 
	Purple	= 5,
	Brown 	= 6,
}

public class SOColor : ScriptableObject {

	public ColorType colorType;
	public Color color;

	public string TintText(string PaperColorText)
	{
		return TintTextWithColor(PaperColorText,color);
	}

	public static string TintTextWithColor(string text,Color color)
	{
		var rgbString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", 
		                              (int)(color.r * 255), 
		                              (int)(color.g * 255), 
		                              (int)(color.b * 255),
		                              (int)(color.a * 255));
		
		string retVal = "<color="+rgbString+">";
		retVal += text;
		return retVal += "</color>";
	}
}
