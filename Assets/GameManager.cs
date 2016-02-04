﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public enum GameStateType
{
	Pregame,
	Start,
	Paused,
	End,
}

public class GameManager : MonoBehaviour, IInputManagerDelegate {

	GameStateType state;
	public TimerController timerControllerObject;
	public LivestockManager livestockManager;
	public WolvesManager wolfManager;
	public List<FenceAreaHandler> fences;

	[Header("In Game Settings")]
	public List<SOColor> availableColors;
	public 






	void Awake()
	{
		state = GameStateType.Pregame;
	}


	void Update()
	{
		if(state == GameStateType.Start)
		{
			timerControllerObject.UpdateTime();
		}


	}

	public void touchStateChanged (TouchInput []touches)
	{
		
		if(state != GameStateType.Start) return;


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

	void OnGUI()
	{
		if(GUI.Button(new Rect(0,20,100,20),"Start Game"))
		{
			state = GameStateType.Start;
			livestockManager.Spawn();
		}
		if(GUI.Button(new Rect(0,45,100,20),"Add Time"))
		{
			timerControllerObject.AddTimer();
		}


	}
}
