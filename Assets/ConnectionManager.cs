using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class ConnectionManager : SingletonMonoBehaviour<ConnectionManager> {

	public bool isConnected = false;
	private WWW www;
	// Use this for initialization
	void Start () {
		www = new WWW("Google.com");
		StartCoroutine(CheckConnection());
	}

	IEnumerator CheckConnection()
	{
		yield return www;
		if(www.error !=null)
		{
			isConnected = false;
			yield return new WaitForSeconds(2);
			StartCoroutine(CheckConnection());
		}
		else
		{
			isConnected = true;
			yield return new WaitForSeconds(2);
			StartCoroutine(CheckConnection());
		}
	}

//	void OnGUI()
//	{
//		GUILayout.Label(isConnected.ToString(),"box");
//	}

}
