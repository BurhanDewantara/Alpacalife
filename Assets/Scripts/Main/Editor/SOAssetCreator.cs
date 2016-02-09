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

		T asset = ScriptableObject.CreateInstance<T> ();
		string path = EditorUtility.GetAssetPath (Selection.activeObject) + "/"+ itemName+".asset";
		AssetDatabase.CreateAsset (asset, path);
		AssetDatabase.SaveAssets ();
	}
}
