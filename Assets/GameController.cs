﻿using UnityEngine;
using System.Collections;
using Artoncode.Core;

public enum GameStateType
{
	Pregame,
	Start,
	Paused,
	End,
}

public class GameController : MonoBehaviour, IInputManagerDelegate {

	GameStateType state;
	public GameObject TimerObject;



	void Awake()
	{
		state = GameStateType.Pregame;
	}


	public void touchStateChanged (TouchInput []touches)
	{
		TouchInput touch = touches [0];

		switch (touch.phase) {

		case TouchPhase.Began: 
			
			break;
			
		case TouchPhase.Moved: 
			
			break;

		case TouchPhase.Ended : 
			
			switch (SwipeDirection(touch.deltaPosition)) 
			{
			case DirectionType.Left 	: 
			case DirectionType.Down		: 
			case DirectionType.Right 	: 
				LivestockManager.shared().Go(SwipeDirection(touch.deltaPosition));
				break;
			}

			//			if(touch.deltaPosition)

			break;

		}
	}




	public DirectionType SwipeDirection(Vector2 delta)
	{
		if(Mathf.Abs (delta.x) > Mathf.Abs (delta.y) && delta.x > 0) {
			return DirectionType.Right;
		} else if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y) && delta.x < 0) {
			return DirectionType.Left;

		} else if (Mathf.Abs (delta.y) > Mathf.Abs (delta.x) && delta.y < 0) {
			return DirectionType.Down;
//		} else if (Mathf.Abs (delta.y) > Mathf.Abs (delta.x) && delta.y > 0) {
//			return DirectionType.Up;
//
		} 

		return DirectionType.Up;
	}


}
