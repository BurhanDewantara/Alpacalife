using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			Application.Quit();
		}


	}
}
