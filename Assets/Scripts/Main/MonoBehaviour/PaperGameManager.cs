using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class PaperGameManager : SingletonMonoBehaviour< PaperGameManager >
{
	public bool isDebug;

	public GamePlayModeType playMode;
	public List<SOColor> paperInGame;
	public List<int> comboLimit;

	public GameObject pauseButton;

	public GameObject tutorialPrefab;
	public GameObject gameOverPrefab;
	public GameObject comboPrefab;


	private GameObject _tutorialGameObject;
	private GameObject _gameOverGameObject;

	private int _comboCounter = 0;
	private int _maxComboCounter = 0;
	private Dictionary<LevelMultiplierType, int> _collectedCannedFood = new Dictionary<LevelMultiplierType, int> ();
	private BonusCannedFoodType currentActivePowerUp; 
	private int comboIdx = 0;

	void Awake ()
	{
		playMode = GamePlayModeType.Say_The_Color;

		_collectedCannedFood [LevelMultiplierType.Negative1] = 0;
		_collectedCannedFood [LevelMultiplierType.Positive05] = 0;
		_collectedCannedFood [LevelMultiplierType.Positive1] = 0;
		_collectedCannedFood [LevelMultiplierType.Positive2] = 0;
		_collectedCannedFood [LevelMultiplierType.Positive4] = 0;
		_collectedCannedFood [LevelMultiplierType.Positive8] = 0;
		_collectedCannedFood [LevelMultiplierType.InstantBonus] = 0;
		_collectedCannedFood [LevelMultiplierType.InstantGem] = 0;

		pauseButton.GetComponent<Button> ().onClick.RemoveAllListeners();
		pauseButton.GetComponent<Button> ().onClick.AddListener (PauseGame);
		TimerController.shared ().OnTimesUp += HandleOnTimesUp;


		comboLimit = new List<int> ();
		comboLimit.Add ((int)(UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ComboShorter) * 1));
		comboLimit.Add ((int)(UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ComboShorter) * 2));
		comboLimit.Add ((int)(UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ComboShorter) * 3));
		comboLimit.Add ((int)(UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ComboShorter) * 4));




	}

	void HandleOnTimesUp (GameObject sender)
	{
		AudioController.shared ().PlayAudio("timesup");
		CannedFoodMachineController.shared ().SetIsAccessible (false);
		PaperController.shared ().SetIsAccessible (false);
		StartCoroutine(ShowGameOver ());
	}

	IEnumerator ShowGameOver()
	{
		yield return new WaitForSeconds (1);
		if (_gameOverGameObject == null) {

			_gameOverGameObject = Instantiate(gameOverPrefab) as GameObject;
			_gameOverGameObject.GetComponent<RectTransform>().SetParent(PaperController.shared().GetComponent<RectTransform>().parent.GetComponent<RectTransform>(),false);
			_gameOverGameObject.GetComponent<GameOverController>().SetCannedFood(_collectedCannedFood);
		}
	}

	void PauseGame()
	{
		if (_tutorialGameObject == null) {
			_tutorialGameObject = Instantiate ( tutorialPrefab) as GameObject;
			_tutorialGameObject.GetComponent<TutorialSwipeController>().SetTutorialColor(paperInGame.GetRange (0, 1),
			                                                                            paperInGame.GetRange (1, 1),
			                                                                            paperInGame.GetRange (2, 1),
			                                                                            paperInGame.GetRange (3, paperInGame.Count - 3));


			_tutorialGameObject.GetComponent<RectTransform>().SetParent(PaperController.shared ().GetComponent<RectTransform>().parent.GetComponent<RectTransform>(),false);
			_tutorialGameObject.GetComponent<Button>().onClick.AddListener(ResumeGame);
		}

		TimerController.shared ().StopTime();
		AudioController.shared ().SetMainAudioSoundVolume(0.3f);
	}

	void ResumeGame()
	{
		Destroy (_tutorialGameObject);
		TimerController.shared ().ResumeTime ();
		AudioController.shared ().SetMainAudioSoundVolume(0.8f);
	}



	void Start()
	{
		paperInGame.Shuffle(5);

		PaperController.shared ().SetPaperDropColor (paperInGame.GetRange (0, 1),
		                                           paperInGame.GetRange (1, 1),
		                                           paperInGame.GetRange (2, 1),
		                                           paperInGame.GetRange (3, paperInGame.Count - 3));

		SetGameTime (UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.PermanentTime));
		InitPowerUpHandler ();
		PauseGame ();
	}

	void SetGameTime(float time)
	{
		TimerController.shared ().SetTime (time);
		TimerController.shared ().StartTime ();
	}

	void InitPowerUpHandler()
	{
		BonusPowerUpController.shared().OnSwitchPlayModeTriggered += HandleOnSwitchPlayModeTriggered;
		BonusPowerUpController.shared().OnBadCanPowerUpTriggered += HandleOnBadCanPowerUpTriggered;
		BonusPowerUpController.shared().OnCanCanPowerUpTriggered += HandleOnCanCanPowerUpTriggered;
		BonusPowerUpController.shared().OnTimeMinusTriggered += HandleOnTimeMinusTriggered;
		BonusPowerUpController.shared().OnTimePlusTriggered += HandleOnTimePlusTriggered;
		BonusPowerUpController.shared().OnInstantCoinTriggered += HandleOnInstantCoinTriggered;
		BonusPowerUpController.shared().OnBonusGemTriggered += HandleOnBonusGemTriggered;
		BonusPowerUpController.shared().OnPowerUpEnded += HandleOnPowerUpEnded;
	}

	
	
	
	#region POWER UPS HANDLER
	
	void HandleOnBonusGemTriggered (GameObject sender, int amount)
	{
		_collectedCannedFood [LevelMultiplierType.InstantGem] += amount;
	}
	
	void HandleOnInstantCoinTriggered (GameObject sender, int amount)
	{
		_collectedCannedFood [LevelMultiplierType.InstantBonus] += amount;
	}
	
	void HandleOnTimePlusTriggered (GameObject sender, int amount)
	{
		TimerController.shared ().EditTime (amount);
	}
	
	void HandleOnTimeMinusTriggered (GameObject sender, int amount)
	{
		TimerController.shared ().EditTime (-amount);
	}


	void HandleOnPowerUpEnded (GameObject sender, float timer)
	{
		currentActivePowerUp = BonusCannedFoodType.None;
	}

	void HandleOnBadCanPowerUpTriggered (GameObject sender, int amount)
	{
		currentActivePowerUp = BonusCannedFoodType.BadCan;
	}
	void HandleOnCanCanPowerUpTriggered (GameObject sender, int amount)
	{
		currentActivePowerUp = BonusCannedFoodType.CanCan;
	}

	void HandleOnSwitchPlayModeTriggered (GameObject sender, int amount)
	{
		switch (playMode) {
		case GamePlayModeType.Say_The_Color:
			playMode = GamePlayModeType.Say_The_Word;
			break;
		case GamePlayModeType.Say_The_Word:
			playMode = GamePlayModeType.Say_The_Color;
			break;
		} 
		PaperController.shared().GetComponentInChildren<BackgroundSwitcher> ().SwitchMode (playMode);
		CannedFoodMachineController.shared ().GetComponentInChildren<BackgroundSwitcher> ().SwitchMode (playMode);
	}

	
	private void CheckBadCan (ref LevelMultiplierType canMultiplier)
	{
		if (currentActivePowerUp == BonusCannedFoodType.BadCan) {
			canMultiplier = LevelMultiplierType.Positive05;
		} 
	}
	
	private IEnumerator CheckCanCan (LevelMultiplierType canMultiplier)
	{
		yield return new WaitForSeconds (0.2f);
		if (currentActivePowerUp == BonusCannedFoodType.CanCan) {
			_collectedCannedFood [canMultiplier]++;
   			
			CannedFoodMachineController.shared ().CreateCan (canMultiplier);
		} 
	}

	#endregion




	void OnGUI ()
	{
		if (isDebug) {
			GUILayout.BeginVertical ("box");
			GUILayout.Label ("PlayMode : " + playMode);
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ("box");
				
			GUILayout.Label ("Canned " + LevelMultiplierType.Negative1.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Negative1]);
			GUILayout.Label ("Canned " + LevelMultiplierType.Positive05.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Positive05]);
			GUILayout.Label ("Canned " + LevelMultiplierType.Positive1.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Positive1]);
			GUILayout.Label ("Canned " + LevelMultiplierType.Positive2.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Positive2]);
			GUILayout.Label ("Canned " + LevelMultiplierType.Positive4.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Positive4]);
			GUILayout.Label ("Canned " + LevelMultiplierType.Positive8.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.Positive8]);
			GUILayout.Label ("Canned " + LevelMultiplierType.InstantBonus.ToString () + " : " + _collectedCannedFood [LevelMultiplierType.InstantBonus]);
			GUILayout.Label ("Combo : " + _comboCounter);
			GUILayout.Label ("Max Combo : " + _maxComboCounter);
			GUILayout.EndVertical ();
		}
	}



	public void DoCorrect ()
	{
		int i = 0;
		for (i = 0; i < comboLimit.Count-1; i++) {
			if (_comboCounter < comboLimit [i])
				break;
		}

		int idx = Mathf.Clamp (i, 0, comboLimit.Count - 1);
		LevelMultiplierType canMultiplier = (LevelMultiplierType)LevelMultiplierType.Positive1 + idx;

		AudioController.shared ().PlayAudio ("correct");


		if (idx != comboIdx) {
			comboIdx = idx;
			string txt = "";
			switch (canMultiplier)
			{
				case LevelMultiplierType.Positive2 : txt = "x2";break;
				case LevelMultiplierType.Positive4 : txt = "x4";break;
				case LevelMultiplierType.Positive8 : txt = "x8";break;
			}
			if(txt!="")
			{
				AudioController.shared ().PlayAudio ("cangem");
				CreatePopUp(txt);

			}

		}


		//POWER UP ------------------------------------------------------------------------------------------------------------------------

		//POWER UP BAD CAN
		CheckBadCan (ref canMultiplier);

		//POWER UP CAN CAN
		StartCoroutine (CheckCanCan (canMultiplier));

		_collectedCannedFood [canMultiplier]++;

		float bonusCanChances = UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ChancesBonusCan);
		float bonusGemchances = UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.ChancesBonusCan);
		CannedFoodMachineController.shared ().CreateCan (canMultiplier,bonusGemchances,bonusCanChances);

		 
		//POWER UP ------------------------------------------------------------------------------------------------------------------------

		_comboCounter++;
		if (_comboCounter > _maxComboCounter) {
			_maxComboCounter = _comboCounter;
		}
	}

	public void DoMistake ()
	{
		AudioController.shared ().PlayAudio ("incorrect");
		_comboCounter = 0;
		_collectedCannedFood [(LevelMultiplierType)LevelMultiplierType.Negative1]++;
		CannedFoodMachineController.shared ().CreateCan (LevelMultiplierType.Negative1);
	}


	public void CreatePopUp(string Text)
	{
		GameObject obj = Instantiate (comboPrefab) as GameObject;
		obj.GetComponent<Text> ().text = Text;
		obj.GetComponent<RectTransform> ().SetParent (PaperController.shared ().GetComponent<RectTransform> ().parent.GetComponent<RectTransform> (),false);
		obj.GetComponent<RectTransform> ().localPosition += new Vector3 (Random.Range (-300, 300), Random.Range (-300, 300), 0);
	}


}
