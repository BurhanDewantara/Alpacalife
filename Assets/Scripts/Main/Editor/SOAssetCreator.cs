using UnityEngine;
using UnityEditor;
using System.Collections;

public class SOAssetCreator : Editor {

	/// <summary>
	/// Creates the object.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected static void CreateObject<T> (string itemName)where T: ScriptableObject
	{
		string path = EditorUtility.SaveFilePanel ("Save", "Assets/Resources/", itemName, "asset");
		if (path == "")
			return;
		
		T asset = ScriptableObject.CreateInstance<T> ();
		path = FileUtil.GetProjectRelativePath (path);
		AssetDatabase.CreateAsset (asset, path);
		AssetDatabase.SaveAssets ();
	}
}
