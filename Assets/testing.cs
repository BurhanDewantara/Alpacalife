using UnityEngine;
using System.Collections;
using ScottGarland;

public class testing : MonoBehaviour {

	BigInteger aa;
	BigInteger bb;
	InfCurrency x;
	InfCurrency a;
	InfCurrency b;

	// Use this for initialization
	void Start () {
	
//		Currency x = new Currency (100,CurrencyUnit.V);
		a = new InfCurrency ("99999999999999999999999999999999999999999999999999999999999999999999999999999999999");
		b = new InfCurrency ("1");
//		InfCurrency y = new InfCurrency (1);
//		x = new InfCurrency ("10");

		Debug.Log(b-a);

//		x = x + x; 
//		Debug.Log(x);
//
//		Debug.Log("x < y " + (x < y));
//		Debug.Log("x > y " + (x > y));
//
//		Debug.Log("x <= y " + (x <= y));
//		Debug.Log("x >= y " + (x >= y));

		aa = new BigInteger("11");
		bb = new BigInteger("22");
		BigInteger cc = aa - bb;
		Debug.Log(cc);
		cc+=bb;
		Debug.Log(cc);




	}


	void OnGUI()
	{
//		a-=b;
//		GUI.Label(new Rect(0,20,Screen.width,Screen.height),a.DefaultString());

//		for (int i=0; i<100; i++) {
//			aa += aa;
//		}
//		GUI.Label(new Rect(0,20,Screen.width,Screen.height),aa.ToString());

//		for (int i=0; i<100; i++) {
//		x = x + x ;
//		}
//		GUI.Label(new Rect(0,20,Screen.width,Screen.height),x.DefaultString());
	}

		
	

}
