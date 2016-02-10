using UnityEngine;
using UnityEditor;
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


	public static T[] LoadAllAssetsOfType<T>(string optionalPath = "") where T : Object
	{
		string[] GUIDs;
		if(optionalPath != "")
		{
			if(optionalPath.EndsWith("/"))
			{
				optionalPath = optionalPath.TrimEnd('/');
			}
			GUIDs = AssetDatabase.FindAssets("t:" + typeof (T).ToString(),new string[] { optionalPath });
		}
		else
		{
			GUIDs = AssetDatabase.FindAssets("t:" + typeof (T).ToString());
		}
		T[] objectList = new T[GUIDs.Length];

		for (int index = 0; index < GUIDs.Length; index++)
		{
			string guid = GUIDs[index];
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			T asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
			objectList[index] = asset;
		}

		return objectList;
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

