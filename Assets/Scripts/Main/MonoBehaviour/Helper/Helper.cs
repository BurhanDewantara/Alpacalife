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

	public static bool IsWithinArea(Collider2D obj ,Collider2D[] areas)
	{
		foreach (Collider2D item in areas) {
			if(item.bounds.Contains(obj.bounds.center))
				return true;
		}

		return false;
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


	#if UNITY_EDITOR
	public static T[] LoadAllAssetsOfType<T>(string optionalPath = "") where T : Object
	{
		string[] GUIDs;
		if(optionalPath != "")
		{
			if(optionalPath.EndsWith("/"))
			{
				optionalPath = optionalPath.TrimEnd('/');
			}


			GUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof (T).ToString(),new string[] { optionalPath });
		}
		else
		{
			GUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof (T).ToString());
		}
		T[] objectList = new T[GUIDs.Length];

		for (int index = 0; index < GUIDs.Length; index++)
		{
			string guid = GUIDs[index];
			string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			T asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
			objectList[index] = asset;
		}

		return objectList;
	}
	#endif
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

