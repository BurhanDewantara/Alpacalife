using UnityEngine;
using System.Collections;

public class Helper {

	public static Vector2 RandomWithinArea(RectTransform point)
	{
		Vector2 newPos = new Vector2(Random.Range(
			point.anchoredPosition.x - point.sizeDelta.x/2,
			point.anchoredPosition.x + point.sizeDelta.x/2),
			Random.Range(
				point.anchoredPosition.y - point.sizeDelta.y/2,
				point.anchoredPosition.y + point.sizeDelta.y/2));
		return newPos;
	}

}
