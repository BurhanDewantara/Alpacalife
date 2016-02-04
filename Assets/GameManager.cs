﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using TMPro;

public enum GameStateType
{
	Pregame,
	Start,
	Paused,
	End,
	GameOver,
}

public class GameManager : MonoBehaviour, IInputManagerDelegate {

	GameStateType state;
	public TimerController timerControllerObject;
	public LivestockManager livestockManager;
	public WolvesManager wolfManager;
	public GameObject counterText;

	int counter;
	private int Counter
	{
		set{
			counter = value;
			if (counter > 0) {
				counterText.GetComponent<TextMeshProUGUI>().text = counter.ToString ();
				iTween.ScaleFrom(counterText,
					iTween.Hash(
						"scale",Vector3.one * 2.0f
						,"time",0.5f
						,"easetype",iTween.EaseType.easeOutBounce
					));
				
			}

			else
				counterText.GetComponent<TextMeshProUGUI>().text = "";
		}
		get{ 
			return counter;
		}
	}

	public List<FenceAreaHandler> fences;
	[Header("In Game Settings")]
	public List<SOColor> availableColors;
	private List<SOColor> inGameColors;


	void Awake()
	{
		counter = 0;
		state = GameStateType.Pregame;
		inGameColors = new List<SOColor> ();
	}


	void Init()
	{
		inGameColors.Clear ();
		InitInGameColor ();
		InitFenceColor ();
		livestockManager.InitColors (availableColors,inGameColors);
	}

	void Start()
	{
		Init ();
		timerControllerObject.OnTimeReachZero += delegate(GameObject sender) {
			livestockManager.ActiveLivestockEaten();
			state = GameStateType.End;
		};
		livestockManager.Spawn();

	}

	void InitFenceColor()
	{
		for (int i = 0; i < inGameColors.Count; i++) {
			fences[i].SetColor(inGameColors[i]);
		}
	}

	void InitInGameColor()
	{
		if (availableColors.Count < 3) {
			Debug.LogError ("COLOR MUST AT LEAST 3 TYPES");
			return;
		}


		while (inGameColors.Count < 3) 
		{
			SOColor randomedSoColor = availableColors.Random ();
			if (!inGameColors.Contains (randomedSoColor)) {
				inGameColors.Add (randomedSoColor);
			}
		}
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
		if(state != GameStateType.Start && state != GameStateType.Pregame) return;

		TouchInput touch = touches [0];

		switch (touch.phase) 
		{
		case TouchPhase.Began: break;
		case TouchPhase.Moved: break;
		case TouchPhase.Ended: 

			DirectionType direction = SwipeDirection (touch.deltaPosition);

			if (direction == DirectionType.Up)
				return;

			if (state == GameStateType.Pregame)
				state = GameStateType.Start;



			foreach (FenceAreaHandler fence in fences) {
				if (fence.fencePosition == direction) {
					if (fence.IsEqual (livestockManager.activeLivestock.textSOColor)) {
						livestockManager.ActiveLivestockGo(SwipeDirection(touch.deltaPosition));
						UpdateDifficulty (Counter);

						livestockManager.Spawn ();
						timerControllerObject.AddTime ();
						Counter++;
					} else {
						livestockManager.ActiveLivestockEaten ();
						state = GameStateType.End;
					}
				}	
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

	void UpdateDifficulty(int counter)
	{
		if (counter > 20) {
			if (counter % 5 == 0) {
				//RE INIT
				Init ();	
			}
		}

	}

	void OnGUI()
	{
		if(GUILayout.Button("Start Game"))
		{
			if (state == GameStateType.Pregame) {
				state = GameStateType.Start;
			}
		}
		if (GUILayout.Button ("Re-Init")) {
			Init ();
		}

	}
}
