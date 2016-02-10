﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using TMPro;

public enum GameStateType
{
	Initiation,
	Pregame,
	Start,
	Paused,
	End,
	GameOver,
}

public class GameController : MonoBehaviour, IInputManagerDelegate {

	public LivestockManager livestockManager;
	public WolvesManager wolfManager;

	public GameObject mainMenuPanel;

	[Header("Timer")]
	public GameObject timerObject;
	public TimerController timerControllerObject;

	[Header("Counter")]
	public GameObject counterTextObject;
	public TextMeshProUGUI counterText;

	[Header("Gameover")]
	public GameObject gameoverPanelObject;
//	public TextMeshProUGUI counterText;




	int counter;
	private int Counter
	{
		set{
			counter = value;
			if (counter > 0) {
				if(!counterTextObject.activeSelf)
				{
					counterText.text = "";
					PopObject (counterTextObject,true);
				}


				counterText.text = counter.ToString ();
				iTween.ScaleFrom(counterText.gameObject,
					iTween.Hash(
						"scale",Vector3.one * 2.0f
						,"time",0.5f
						,"easetype",iTween.EaseType.easeOutBounce
					));
			}
			else
				counterText.text = "";
		}
		get{ 
			return counter;
		}
	}

	[Header("Environment")]
	public GameObject environment;
	public List<FenceAreaHandler> fences;

	[Header("In Game Settings")]
	public List<ColorSO> availableColors;


	private List<ColorSO> inGameColors;
	private GameStateType state;


	void OnEnable()
	{
		counter = 0;
		state = GameStateType.Initiation;
		inGameColors = new List<ColorSO> ();

		Init ();
		timerObject.SetActive(false);
		counterTextObject.SetActive(false);

		timerControllerObject.OnTimeReachZero += delegate(GameObject sender) {
			StartCoroutine(EndGame());
		};

		WorldManager.shared().LivestockAssemble();
		WorldManager.shared().OnAssembleDone += OnAssembleDoneHandler;
	}

	void OnAssembleDoneHandler()
	{
		environment.GetComponent<Animator>().SetBool("IsPlay",true);
		livestockManager.Spawn();
		PopObject (timerObject,true);
		state = GameStateType.Pregame;
		WorldManager.shared().OnAssembleDone -= OnAssembleDoneHandler;
	}

	void Init()
	{
		inGameColors.Clear ();
		InitInGameColor ();
		InitFenceColor ();
		livestockManager.InitColors (availableColors,inGameColors);
	}

	void PopObject(GameObject obj, bool show,float delay = 0,float time =0.2f)
	{
		obj.SetActive(show);
		if (show) {
			obj.transform.localScale = Vector3.zero;
			iTween.ScaleTo (obj, iTween.Hash (
				"scale", Vector3.one,
				"delay",delay,
				"time", time,
				"easetype", iTween.EaseType.spring
			));
		} else {
			obj.transform.localScale = Vector3.one;
			iTween.ScaleTo (obj, iTween.Hash (
				"scale", Vector3.zero,
				"delay",delay,
				"time", 0.5f,
				"easetype", iTween.EaseType.spring
			));
		}
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
			ColorSO randomedSoColor = availableColors.Random ();
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
						StartCoroutine(EndGame());
					}
				}	
			}

			//			if(touch.deltaPosition)

			break;
		}
	}

	private IEnumerator EndGame()
	{
		livestockManager.ActiveLivestockEaten ();
		state = GameStateType.End;
		yield return new WaitForSeconds(2.0f);
		timerObject.SetActive(false);
		counterTextObject.SetActive(false);
		gameoverPanelObject.SetActive(true);
		gameoverPanelObject.GetComponent<GameOverController>().SetScore(Counter,100);

	}

	public void GoToHome()
	{
		livestockManager.RemoveAllLivestock();
		wolfManager.RemoveAllWolves();
		environment.GetComponent<Animator>().SetBool("IsPlay",false);
		WorldManager.shared().LivestockDissasemble();
		WorldManager.shared().OnDisassembleDone += OnDisassembleDoneHandler;
	}

	private void OnDisassembleDoneHandler()
	{
		WorldManager.shared().OnDisassembleDone -= OnDisassembleDoneHandler;
		this.gameObject.SetActive(false);
		mainMenuPanel.SetActive(true);
	}
//	public void




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
		if (GUILayout.Button ("Add Time")) {
			timerControllerObject.AddTime();
		}
		if (GUILayout.Button ("show Counter")) {
			PopObject (counterTextObject,true);
		}
		if (GUILayout.Button ("hide Counter")) {
			PopObject (counterTextObject,false);
		}


	}
}
