using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class PaperController : SingletonMonoBehaviour<PaperController> {

	public GameObject leftPanel;
	public GameObject topPanel;
	public GameObject rightPanel;
	public GameObject bottomPanel;

	public GameObject centerPanel;

	public GameObject paperPrefab;
	public GameObject paperContainerPanel;

	public SOFish fishData;

	private List<SOColor> _paperList;
	private int _maxPaperCount = 10;
	private int _counter = 0;

	private bool isAutoMode;


	
	public void SetIsAccessible(bool val =false)
	{
		foreach (GameObject paper in paperContainerPanel.GetComponent<PaperContainer> ().Papers) {
			paper.GetComponent<PaperContentViewer>().IsAccessible = val;
			
		}
	}

	void Awake()
	{
		_paperList = PaperGameManager.shared ().paperInGame;

		float power = UpgradableDataController.shared ().GetPlayerUpgradeDataValue (UpgradableType.PaperSlideSpeed);
		leftPanel.GetComponent<Magnet> ().magnetPower = power;
		topPanel.GetComponent<Magnet> ().magnetPower = power;
		rightPanel.GetComponent<Magnet> ().magnetPower = power;
		bottomPanel.GetComponent<Magnet> ().magnetPower = power;

		leftPanel.GetComponent<Magnet>().OnObjectMagnetized += HandleOnObjectMagnetized;
		rightPanel.GetComponent<Magnet>().OnObjectMagnetized += HandleOnObjectMagnetized;
		topPanel.GetComponent<Magnet>().OnObjectMagnetized += HandleOnObjectMagnetized;
		bottomPanel.GetComponent<Magnet>().OnObjectMagnetized += HandleOnObjectMagnetized;

		centerPanel.GetComponent<Magnet>().OnObjectMagnetized += HandleOnCenterPanelObjectMagnetized;

		isAutoMode = false;

		BonusPowerUpController.shared().OnSlidePowerUpTriggered += HandleOnSlidePowerUpTriggered;
		BonusPowerUpController.shared().OnPowerUpEnded += HandleOnPowerUpEnded;;
	}


	void Start()
	{
		StartCoroutine( InitCreatePaper ());
	}

	public void SetPaperDropColor(List<SOColor> left, List<SOColor> top, List<SOColor> right, List<SOColor> bottom)
	{
		leftPanel.GetComponent<PaperDropPanel> ().AddColorTarget(left);
		topPanel.GetComponent<PaperDropPanel> ().AddColorTarget(top);
		rightPanel.GetComponent<PaperDropPanel> ().AddColorTarget(right);
		bottomPanel.GetComponent<PaperDropPanel> ().AddColorTarget(bottom);   
		bottomPanel.GetComponent<PaperDropPanel> ().isTrashPanel = true;
	}




	void HandleOnCenterPanelObjectMagnetized (GameObject sender, GameObject magnetObject)
	{
		sender.GetComponent<Magnet> ().RemoveMagnetObject (magnetObject);
	}

	void HandleOnPowerUpEnded (GameObject sender, float timer)
	{
		isAutoMode = false;
	}

	void HandleOnSlidePowerUpTriggered (GameObject sender, int amount)
	{
		isAutoMode = true;
	}



	void CreatePaper()
	{
		while (paperContainerPanel.GetComponent<PaperContainer>().Papers.Count < _maxPaperCount) {
			GameObject obj = GeneratePaperObject(paperPrefab);
			paperContainerPanel.GetComponent<PaperContainer>().AddPaper(obj);
			obj.GetComponent<PaperContent>().SetItem (GeneratePaperItem());
			obj.GetComponent<RectTransform>().position = centerPanel.GetComponent<RectTransform>().position;
			centerPanel.GetComponent<Magnet>().AddMagnetObject(obj);
			_counter++;
		}
	}

	IEnumerator InitCreatePaper()
	{
		while (paperContainerPanel.GetComponent<PaperContainer>().Papers.Count < _maxPaperCount) {
			GameObject obj = GeneratePaperObject(paperPrefab);
			paperContainerPanel.GetComponent<PaperContainer>().AddPaper(obj);
			obj.GetComponent<PaperContent>().SetItem (GeneratePaperItem());
			centerPanel.GetComponent<Magnet>().AddMagnetObject(obj);
			_counter++;
			yield return new WaitForSeconds(0.1f);
		}

//		paperContainerPanel.GetComponent<PaperContainer> ().Papers [0].GetComponent<PaperContentViewer> ().IsAccessible = true;
	}

	GameObject GeneratePaperObject(GameObject paperPrefab)
	{
		GameObject obj = Instantiate(paperPrefab) as GameObject;
		obj.name = "paper - " + _counter.ToString("000");
		obj.transform.Rotate (new Vector3 (0,0,Random.Range(-10,10)));
		obj.GetComponent<PaperContentViewer>().OnDropAtBottomPanel += HandleOnDropAtBottomPanel;
		obj.GetComponent<PaperContentViewer>().OnDropAtLeftPanel += HandleOnDropAtLeftPanel;
		obj.GetComponent<PaperContentViewer>().OnDropAtTopPanel += HandleOnDropAtTopPanel;
		obj.GetComponent<PaperContentViewer>().OnDropAtRightPanel += HandleOnDropAtRightPanel;;
		obj.GetComponent<PaperContentViewer>().IsAccessible = true;

		return obj;
	}

	PaperItem GeneratePaperItem()
	{
		SOColor randomColorTint 	= _paperList [Random.Range (0, _paperList.Count)];
		SOColor randomColorText 	= _paperList [Random.Range (0, _paperList.Count)];
		string randomName 			= fishData.GetRandomName ();
		Sprite randomImage 			= fishData.GetRandomImage ();

		PaperItem pItem = new PaperItem (randomColorTint, randomColorText, randomImage, 1/* #### value from upgradeable*/, randomName);
		return pItem;
	}


	#region Paper Handler

	void AutoMagnet(GameObject paper)
	{
		SOColor countedObject = null; 
		switch (PaperGameManager.shared ().playMode) 
		{
		case GamePlayModeType.Say_The_Color : 
			countedObject = paper.GetComponent<PaperContent>().paper.colorTint;
			break;
		case GamePlayModeType.Say_The_Word : 
			countedObject = paper.GetComponent<PaperContent>().paper.colorText;
			break;
		}
			
		if (topPanel.GetComponent<PaperDropPanel> ().IsColorExist (countedObject)) {
			topPanel.GetComponent<Magnet> ().AddMagnetObject (paper);
		}

		else if (rightPanel.GetComponent<PaperDropPanel> ().IsColorExist (countedObject)) {
			rightPanel.GetComponent<Magnet> ().AddMagnetObject (paper);
		}

		else if (leftPanel.GetComponent<PaperDropPanel> ().IsColorExist (countedObject)) {
			leftPanel.GetComponent<Magnet> ().AddMagnetObject (paper);
		}

		else if (bottomPanel.GetComponent<PaperDropPanel> ().IsColorExist (countedObject)) {
			bottomPanel.GetComponent<Magnet> ().AddMagnetObject (paper);
		}

	}


	void HandleOnDropAtTopPanel (GameObject sender)
	{
		sender.GetComponent<PaperContentViewer> ().IsAccessible = false;	
		if (isAutoMode) {
			AutoMagnet(sender);
		} else {
			topPanel.GetComponent<Magnet> ().AddMagnetObject (sender);
		}

	}

	void HandleOnDropAtRightPanel (GameObject sender)
	{
		sender.GetComponent<PaperContentViewer> ().IsAccessible = false;
		if (isAutoMode) {
			AutoMagnet (sender);
		} else {
			rightPanel.GetComponent<Magnet> ().AddMagnetObject (sender);
		}
	}

	void HandleOnDropAtLeftPanel (GameObject sender)
	{
		sender.GetComponent<PaperContentViewer> ().IsAccessible = false;	
		if (isAutoMode) {
			AutoMagnet (sender);
		} else {
			leftPanel.GetComponent<Magnet> ().AddMagnetObject (sender);
		}
	}

	void HandleOnDropAtBottomPanel (GameObject sender)
	{
		sender.GetComponent<PaperContentViewer> ().IsAccessible = false;	
		if (isAutoMode) {
			AutoMagnet (sender);
		} else {
			bottomPanel.GetComponent<Magnet> ().AddMagnetObject (sender);
		}
	}


	/// <summary>
	/// Handles the on object magnetized.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="magnetObject">Magnet object.</param>
	void HandleOnObjectMagnetized (GameObject sender, GameObject magnetObject)
	{
		//Calculate
		CalculateScore (sender, magnetObject);


		//REMOVE
		sender.GetComponent<Magnet> ().RemoveMagnetObject (magnetObject);
		paperContainerPanel.GetComponent<PaperContainer> ().RemovePaper (magnetObject);
		Destroy (magnetObject);

		CreatePaper ();
	}


	void CalculateScore(GameObject panel, GameObject pObject)
	{
		PaperItem paperObject = pObject.GetComponent<PaperContent> ().paper;
		SOColor colorObject = paperObject.colorTint;

		if (PaperGameManager.shared().playMode == GamePlayModeType.Say_The_Color) 
		{
			colorObject = paperObject.colorTint;
		} 
		else if ( PaperGameManager.shared().playMode == GamePlayModeType.Say_The_Word) 
		{
			colorObject = paperObject.colorText;
		}
		
		
		if (panel.GetComponent<PaperDropPanel> ().IsColorExist (colorObject)) {
			PaperGameManager.shared().DoCorrect();
		}
		else{
			PaperGameManager.shared().DoMistake();
		}
	}
	#endregion



}
