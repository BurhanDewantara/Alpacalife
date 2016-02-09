using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour {

	public List<GameObject> environmentObject;

	public void Start()
	{
		
	}

	public void DrawEnvironment()
	{
		foreach (GameObject item in environmentObject) {
//			item.GetComponent<EnvironmentDrawer>().SetSprite(
		}
	}
}
