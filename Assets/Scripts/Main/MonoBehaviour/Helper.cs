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


	public static Vector3 RandomWithinArea(Collider2D[] areas)
	{
		
		Collider2D area = areas.Random();

		Vector3 newPosition = new Vector3(
			Random.Range(area.offset.x - area.bounds.size.x/2, area.offset.x + area.bounds.size.x/2),
			Random.Range(area.offset.y - area.bounds.size.y/2, area.offset.y + area.bounds.size.y/2),
			0);

		return newPosition;
	}

}

//	public static Vector3 RandomWithinArea(RectTransform point)
//	{
//		Vector2 newPos = new Vector2(Random.Range(
//			point.anchoredPosition.x - point.sizeDelta.x/2,
//			point.anchoredPosition.x + point.sizeDelta.x/2),
//			Random.Range(
//				point.anchoredPosition.y - point.sizeDelta.y/2,
//				point.anchoredPosition.y + point.sizeDelta.y/2));
//		return newPos;
//	}

