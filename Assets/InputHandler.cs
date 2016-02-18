using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			Application.Quit();
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		GameDataManager.shared().save();
	}


	void OnGUI()
	{
		GUILayout.Label(PlayerStatisticManager.shared().ToString(),"box");
	}

}
