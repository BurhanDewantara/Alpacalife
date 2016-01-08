using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Artoncode.Core.Data;

public class ObscuredTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

		for (int i=0; i<10; i++) {
			ObscuredInt a = Random.Range(0,100);
			
			DataManager.defaultManager.setInt ("test"+i, a);
		}

		DataManager.defaultManager.save ();
		DataManager.defaultManager.load ();

		DataManager.defaultManager.debug ();

		ObscuredBool asd = true;
		Debug.Log (asd);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
