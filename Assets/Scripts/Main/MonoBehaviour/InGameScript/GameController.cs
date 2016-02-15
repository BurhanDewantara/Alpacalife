﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;
using TMPro;
using ScottGarland;


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

	public GameObject mainCanvas;
	public GameObject popUpPrefab;

	[Header("Timer")]
	public GameObject timerGameObject;
	public TimerController timerControllerObject;

	[Header("Counter")]
	public GameObject counterTextObject;
	public TextMeshProUGUI counterText;

	[Header("Gameover")]
	public GameObject gameoverPanelObject;
//	public TextMeshProUGUI counterText;


	[Header("Tutorial")]
	public bool isTutorial = false;
	public GameObject tutorialPanel;
	public GameObject tutorialButton;


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
		timerGameObject.SetActive(false);
		tutorialButton.SetActive (false);
		counterTextObject.SetActive(false);


		timerControllerObject.Init();
		timerControllerObject.OnTimeReachZero += delegate(GameObject sender) {
			StartCoroutine(EndGame());
		};

		WorldManager.shared().LivestockAssemble();
		WorldManager.shared().OnAssembleDone += OnAssembleDoneHandler;


		if (!GameDataManager.shared ().PlayerHasTakenTutorial ) {
			TriggerTutorial ();
		}
	}

	public void TriggerTutorial ()
	{
		isTutorial = true;
		timerGameObject.SetActive (false);
		tutorialPanel.SetActive (true);
		tutorialPanel.GetComponent<RectTransform> ().localScale = Vector3.one;
		iTween.ScaleFrom (tutorialPanel,
			iTween.Hash(
				"scale",Vector3.zero
				,"time", 0.4f
				,"easetype",iTween.EaseType.easeOutBack
			)
			);
		tutorialButton.SetActive (false);
	}


	public void HideTutorial()
	{
		timerGameObject.SetActive (false);
		tutorialButton.SetActive (false);

		iTween.ScaleTo(tutorialPanel,
			iTween.Hash(
				"scale",Vector3.zero 
				,"time", 0.4f
				,"easetype",iTween.EaseType.easeInBack
				,"oncomplete","HideTutorialComplete"
				,"oncompletetarget",this.gameObject
			)
		);
	}

	public void HideTutorialComplete()
	{
		tutorialPanel.GetComponent<RectTransform> ().localScale = Vector3.one;
		tutorialPanel.SetActive (false);
		isTutorial = false;
		PopObject(timerGameObject,true);
		PopObject(tutorialButton,true);
		GameDataManager.shared ().PlayerHasTakenTutorial = true;
	}






	void OnAssembleDoneHandler()
	{
		environment.GetComponent<Animator>().speed = 1;
		environment.GetComponent<Animator>().SetBool("IsPlay",true);
		livestockManager.Spawn();
		PopObject (timerGameObject,true);
		PopObject (tutorialButton,true);
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

			if (state == GameStateType.Pregame && !isTutorial) {
				state = GameStateType.Start;
				tutorialButton.SetActive (false);
			}

			foreach (FenceAreaHandler fence in fences) {
				if (fence.fencePosition == direction) {
					if (fence.IsEqual (livestockManager.activeLivestock.textSOColor)) {
						

						if (!isTutorial) {
							Popuptext();
							CurrencyManager.shared ().AddGold (livestockManager.activeLivestock.GetLivestock ());
							UpdateDifficulty (Counter);
							timerControllerObject.AddTime ();
							Counter++;
						}


						livestockManager.ActiveLivestockGo(SwipeDirection(touch.deltaPosition));
						livestockManager.Spawn ();

					} else {
						if (!isTutorial) {
							StartCoroutine (EndGame ());
						} else {
							livestockManager.activeLivestock.FakePanic();
						}
					}
				}	
			}

			//			if(touch.deltaPosition)

			break;
		}
	}

	private void Popuptext ()
	{
		GameObject lvsObj = livestockManager.activeLivestock.gameObject;
		LivestockSO so = livestockManager.activeLivestock.GetLivestock ();

		BigInteger gold		  = UpgradeManager.shared ().GetLivestockSlideValue (so);
		BigInteger multiplier = UpgradeManager.shared ().GetCurrentMultiplier ();
		BigInteger result = gold * multiplier;

		string popText = result.ToStringShort ();

		Vector3 position = Camera.main.WorldToScreenPoint(lvsObj.transform.position);

		GameObject obj = Instantiate (popUpPrefab, position, Quaternion.identity) as GameObject;
		obj.GetComponent<PopAndFade> ().SetText (popText);
		obj.GetComponent<PopAndFade> ().PopUp ();
		obj.transform.SetParent (mainCanvas.transform, true);
	}

	private IEnumerator EndGame()
	{
		livestockManager.activeLivestock.Panic ();
		livestockManager.ActiveLivestockEaten ();
		state = GameStateType.End;
		environment.GetComponent<Animator>().speed = 2;
		environment.GetComponent<Animator>().SetBool("IsPlay",false);
		yield return new WaitForSeconds(2.0f);
		timerGameObject.SetActive(false);
		counterTextObject.SetActive(false);
		gameoverPanelObject.SetActive(true);
		int bestscore = GameDataManager.shared ().PlayerBestScore;
		gameoverPanelObject.GetComponent<GameOverController>().SetScore(Counter,bestscore);

		if (bestscore < counter)
			GameDataManager.shared ().PlayerBestScore = counter;

	}

	public void GoToHome()
	{
		livestockManager.RemoveAllLivestock();
		wolfManager.RemoveAllWolves();
		WorldManager.shared().OnDisassembleDone += OnDisassembleDoneHandler;
		WorldManager.shared().LivestockDissasemble();
	}

	private void OnDisassembleDoneHandler()
	{
		this.gameObject.SetActive(false);
		this.mainMenuPanel.SetActive(true);
		WorldManager.shared().OnDisassembleDone -= OnDisassembleDoneHandler;
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
		if (counter >= 20) {
			if (counter % 5 == 0) {
				//RE INIT
				Init ();	
			}
		}

	}

//	void OnGUI()
//	{
//		if(GUILayout.Button("Start Game"))
//		{
//			if (state == GameStateType.Pregame) {
//				state = GameStateType.Start;
//			}
//		}
//		if (GUILayout.Button ("Re-Init")) {
//			Init ();
//		}
//		if (GUILayout.Button ("Add Time")) {
//			timerControllerObject.AddTime();
//		}
//		if (GUILayout.Button ("show Counter")) {
//			PopObject (counterTextObject,true);
//		}
//		if (GUILayout.Button ("hide Counter")) {
//			PopObject (counterTextObject,false);
//		}
//	}
}
