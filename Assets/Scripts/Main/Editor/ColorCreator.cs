using UnityEngine;
using UnityEditor;
using System.Collections;

public class ColorCreator : SOAssetCreator {

	[MenuItem("Assets/Create/Color")]
	public static void createColor ()
	{
		CreateObject<ColorSO> ("Color");
	}

	[MenuItem("Assets/Create/Environment")]
	public static void createEnvi ()
	{
		CreateObject<EnvironmentSO> ("Environment");
	}

	[MenuItem("Assets/Create/Livestock")]
	public static void createAnimal ()
	{
		CreateObject<LivestockSO> ("Livestock");
	}

}
