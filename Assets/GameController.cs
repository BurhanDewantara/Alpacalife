using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class GameController : MonoBehaviour, IInputManagerDelegate {





	public void touchStateChanged (TouchInput []touches)
	{
		TouchInput touch = touches [0];


		switch (touch.phase) {

		case TouchPhase.Ended : 
			Debug.Log(touch.start);
			Debug.Log(touch.end);
			Debug.Log(touch.deltaPosition);
			break;

		}
	}
}
