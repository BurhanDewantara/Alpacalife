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

public enum GameModeType
{
	ColorToText,
	TextToColor,
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
	protected int tutorialCounter;
	public GameObject tutorialPanel;
	public GameObject tutorialOkButton;
	public GameObject showTutorialButton;
	protected GameObject tutorialAnimation{
		get{
			if(tutorialPanel!=null)
				return tutorialPanel.transform.FindChild("Animation").gameObject;
			return null;
		}
	}


	protected int counter;
	protected BigInteger inGameEarnedMoney;


	protected int Counter
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
	public List<GameObject> fences;

	[Header("In Game Settings")]
	public List<ColorSO> availableColors;


	protected List<ColorSO> inGameColors;
	protected GameStateType state;
	protected DirectionType swipeDirection;
	protected Vector2 _deltaMovement;
	protected GameModeType gameMode;

	public void StartGame()
	{
		counter = 0;
		tutorialCounter = 0;
		inGameEarnedMoney = 0;
		state = GameStateType.Initiation;
		inGameColors = new List<ColorSO> ();

		Init ();
		timerGameObject.SetActive(false);
		showTutorialButton.SetActive (false);
		counterTextObject.SetActive(false);


		timerControllerObject.Init();
		timerControllerObject.OnTimeReachZero += delegate(GameObject sender) {
			StartCoroutine(GameOver());
		};

		WorldManager.shared().LivestockAssemble();
		WorldManager.shared().OnAssembleDone += OnAssembleDoneHandler;
	}

	#region Init
	void Init()
	{
		inGameColors.Clear ();
		InitInGameColor ();
		InitFence ();
		livestockManager.InitColors (availableColors,inGameColors);
	}

	protected void InitFence()
	{
		for (int i = 0; i < inGameColors.Count; i++) {
			fences[i].GetComponent<FenceAreaHandler>().SetColor(inGameColors[i]);
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


	#endregion

	public void SetGameMode(GameModeType mode)
	{
		gameMode = mode;
	}



	#region Tutorial
	public void TriggerTutorial (int tCounter = 3)
	{
		tutorialCounter = tCounter;
		isTutorial = true;
		timerGameObject.SetActive (false);
		tutorialPanel.SetActive (true);
		tutorialAnimation.SetActive(true);
		tutorialOkButton.SetActive(tutorialCounter <= 0);

		tutorialPanel.GetComponent<RectTransform> ().localScale = Vector3.one;
		iTween.ScaleFrom (tutorialPanel,
			iTween.Hash(
				"scale",Vector3.zero
				,"time", 0.4f
				,"easetype",iTween.EaseType.easeOutBack
			)
		);
		showTutorialButton.SetActive (false);
		GetTutorialDirection();

	}


	public void HideTutorial()
	{
		timerGameObject.SetActive (false);
		showTutorialButton.SetActive (false);
		tutorialAnimation.SetActive(false);
		iTween.ScaleTo(tutorialPanel,
			iTween.Hash(
				"scale",Vector3.zero 
				,"time", 0.4f
				,"easetype",iTween.EaseType.easeInBack
				,"oncomplete","HideTutorialComplete"
				,"oncompletetarget",this.gameObject
			)
		);
		GPGManager.TriggerTutorial();
	}

	public void HideTutorialComplete()
	{
		tutorialPanel.GetComponent<RectTransform> ().localScale = Vector3.one;
		tutorialPanel.SetActive (false);
		isTutorial = false;
		PopObject(timerGameObject,true);
		PopObject(showTutorialButton,true);
		GameDataManager.shared ().PlayerHasTakenTutorial = true;
	}

	private void GetTutorialDirection()
	{
		if(isTutorial)
		{
			if(livestockManager.activeLivestock ==null) return;

			if(tutorialAnimation !=null)
			{
				foreach (GameObject obj in fences) {
					FenceAreaHandler fence = obj.GetComponent<FenceAreaHandler>();
					if (fence.IsEqual (livestockManager.activeLivestock.textSOColor)) {
						DirectionType direction = fence.fencePosition;
						tutorialAnimation.GetComponent<Animator>().SetTrigger(direction.ToString());
						break;
					}
				}
			}
		}
	}

	#endregion







	void OnAssembleDoneHandler()
	{
		environment.GetComponent<Animator>().speed = 1;
		environment.GetComponent<Animator>().SetBool("IsPlay",true);
		livestockManager.Spawn();
		PopObject (timerGameObject,true);
		PopObject (showTutorialButton,!isTutorial);
		tutorialAnimation.SetActive(true);
		GetTutorialDirection();
		state = GameStateType.Pregame;
		WorldManager.shared().OnAssembleDone -= OnAssembleDoneHandler;
		if (!GameDataManager.shared ().PlayerHasTakenTutorial ) {
			TriggerTutorial (5);
		}
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
		float range = 50;

		switch (touch.phase) 
		{
		case TouchPhase.Began: 
			_deltaMovement = Vector2.zero;
			break;
		case TouchPhase.Moved: 
//			_deltaMovement += touch.deltaPosition;
			break;
		case TouchPhase.Ended: 

			_deltaMovement = touch.end - touch.start;
			if (Mathf.Abs (_deltaMovement.x) > Mathf.Abs (_deltaMovement.y) && _deltaMovement.x > range ) {
				ExecuteSwipe(DirectionType.Right);
			} else if (Mathf.Abs (_deltaMovement.x) > Mathf.Abs (_deltaMovement.y) && _deltaMovement.x < -range ) {
				ExecuteSwipe(DirectionType.Left);
			} else if (Mathf.Abs (_deltaMovement.y) > Mathf.Abs (_deltaMovement.x) && _deltaMovement.y < -range ) {
				ExecuteSwipe(DirectionType.Down);
			}

			break;
		}
	}

	private void ExecuteSwipe(DirectionType direction)
	{

		if (direction == DirectionType.Up)
			return;

		if (state == GameStateType.Pregame && !isTutorial) {
			state = GameStateType.Start;
			showTutorialButton.SetActive (false);
		}

		foreach (GameObject obj in fences) {
			FenceAreaHandler fence = obj.GetComponent<FenceAreaHandler>();

			if (fence.fencePosition == direction) {
				if (fence.IsEqual (livestockManager.activeLivestock.textSOColor)) {

					if (!isTutorial) {
						BigInteger earn = CalculateGold();

						CurrencyManager.shared ().AddGold (earn);
						Popuptext(earn.ToStringShort(), livestockManager.activeLivestock.transform.position );
						inGameEarnedMoney += earn;


						UpdateDifficulty (Counter);
						timerControllerObject.AddTime ();
						Counter++;
					}
					else if (isTutorial)
					{
						tutorialCounter--;
						tutorialOkButton.SetActive(tutorialCounter <= 0);
					}
					GPGManager.Trigger1stJumpAchievement ();

					livestockManager.ActiveLivestockGo(direction);
					livestockManager.Spawn ();
					GetTutorialDirection();

				} else {
					if (!isTutorial) {


						PlayerStatisticManager.shared().TotalBitten+=1;
						PlayerStatisticManager.shared().TotalJump+=counter;
						PlayerStatisticManager.shared().TotalGoldEarn1Game=inGameEarnedMoney;


						GPGManager.TriggerTotalEarnMoneyInOneAchievement(inGameEarnedMoney);
						GPGManager.TriggerTotalJumpInOneAchievement(Counter);

						GPGManager.TriggerIncrementalEatbyWolfAchievement();
						GPGManager.TriggerTotalJumpAchievement(PlayerStatisticManager.shared().TotalJump);


						StartCoroutine (GameOver ());
					} else {
						livestockManager.activeLivestock.FakePanic();
					}
				}
			}	
		}
	}



	private BigInteger CalculateGold()
	{
		GameObject lvsObj = livestockManager.activeLivestock.gameObject;
		LivestockSO so = livestockManager.activeLivestock.GetLivestock ();

		BigInteger gold		  = UpgradeManager.shared ().GetLivestockSlideValue (so);
		BigInteger multiplier = UpgradeManager.shared ().GetCurrentMultiplier ();
		BigInteger result = gold * multiplier;
		return result;

	}


	private void Popuptext (string popText,Vector3 popUpPosition)
	{
		Vector3 position = Camera.main.WorldToScreenPoint(popUpPosition);

		GameObject obj = Instantiate (popUpPrefab) as GameObject;
		obj.GetComponent<PopAndFade> ().SetText (popText);
		obj.GetComponent<PopAndFade> ().PopUp ();
		obj.transform.SetParent (mainCanvas.transform, false);
		obj.GetComponent<RectTransform>().position = position;
	}

	private IEnumerator GameOver()
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
		gameoverPanelObject.GetComponent<GameOverController>().OnDisabled += EndGame;

		int bestscore = GameDataManager.shared ().PlayerBestScore;
		gameoverPanelObject.GetComponent<GameOverController>().SetScore(Counter,bestscore);

		if (bestscore < counter)
			GameDataManager.shared ().PlayerBestScore = counter;

		GPGManager.PostLeaderBoard(bestscore);
	}

	public void EndGame()
	{
		livestockManager.RemoveAllLivestock();
		wolfManager.RemoveAllWolves();
		WorldManager.shared().OnDisassembleDone += OnDisassembleDoneHandler;
		WorldManager.shared().LivestockDissasemble();
		GameDataManager.shared().save();

		GPGManager.TriggerTotalEarnMoneyAchievement(PlayerStatisticManager.shared().TotalGold);
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
		
		if (counter >= 50) {
			Init ();	
		}
		else if (counter >= 35 && counter % 3 == 0) {
			Init ();	
		}
		else if (counter >= 20 && counter % 5 == 0) {
			Init ();	
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
