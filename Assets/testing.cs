using UnityEngine;
using System.Collections;

public class testing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
//		Currency x = new Currency (100,CurrencyUnit.V);
//		Currency x = new Currency (12345667890987654321);
		Currency x = new Currency ("123");
		Currency y = new Currency ("123");

		x = x - y; 

//		Debug.Log("x < y " + (x < y));
//		Debug.Log("x > y " + (x > y));
//
//		Debug.Log("x <= y " + (x <= y));
//		Debug.Log("x >= y " + (x >= y));

	}
	

}
