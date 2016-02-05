using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;


public class LivestockManager : SingletonMonoBehaviour<LivestockManager> {

	public delegate void LivestockManagerDelegate(GameObject sender);
	public event LivestockManagerDelegate OnCorrectDelivery;
	public event LivestockManagerDelegate OnIncorrectDelivery;

	public GameObject spawnPoint;
	public GameObject gatheringPoint;

	public GameObject livestockPrefab;
	public int totalLivestock;

	private List<GameObject> queueLivestock;
	private int livestockCounter;

	public LivestockController activeLivestock{
		get{
			if(queueLivestock != null)
				if(queueLivestock.Count > 0)
					return queueLivestock[0].GetComponent<LivestockController> ();
			return null;
		}
	}

	private List<SOColor> availableColors;
	private List<SOColor> inGameColors;

	void Awake()
	{
		livestockCounter = 0;
		queueLivestock = new List<GameObject> ();
	}

	public void InitColors(List<SOColor> availableColors, List<SOColor> inGameColors)
	{
		this.availableColors = availableColors;
		this.inGameColors = inGameColors;
	}


	private GameObject SpawnLivestock()
	{
		Vector3 newPos = Helper.RandomWithinArea (spawnPoint.GetComponents<BoxCollider2D>());
		GameObject newLivestock = Instantiate (livestockPrefab, newPos, Quaternion.identity) as GameObject;
		newLivestock.name = livestockCounter.ToString();
		newLivestock.transform.SetParent (this.transform, false);
		newLivestock.GetComponent<LivestockController>().OnLivestockReceivedOrder += delegate(GameObject sender) {
			queueLivestock.Remove(sender);
		};
		return newLivestock;
	}

	public void Spawn()
	{
		 
		Vector3 newPos = Helper.RandomWithinArea (gatheringPoint.GetComponents<BoxCollider2D>());
		float speed = 2.5f;

		GameObject currLivestock = SpawnLivestock ();

		currLivestock.GetComponent<LivestockController>().SetLabel(inGameColors.Random (),availableColors.Random ());
		currLivestock.GetComponent<LivestockController>().MoveToReadyPosition(newPos,speed);			

		queueLivestock.Add (currLivestock);
		livestockCounter++;

	}

	public void ActiveLivestockGo(DirectionType dir)
	{
		if (activeLivestock == null) {
			return;
		}

		activeLivestock.Move (dir);

//		if(true) //swipe nya bener
//		{
//			Spawn();			
//		}
//		else
//		{

//		}
	}

	public void ActiveLivestockEaten()
	{
		WolvesManager.shared ().Charge (queueLivestock.ToArray());
		iTween.Stop(activeLivestock.gameObject);
	}


//	void Update()
//	{
//		InputUpdate ();
//	}
//
//	void InputUpdate()
//	{
//		//change this input with swipe
//		if (activeLivestock == null) {
//			return;
//		}
//
//		if (Input.GetAxisRaw ("Horizontal") == 1) {
//			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Right);
//		} else if (Input.GetAxisRaw ("Horizontal") == -1) {
//			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Left);
//		} else if (Input.GetAxisRaw ("Vertical") == -1) {
//			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Down);
//		} else if (Input.GetAxisRaw ("Vertical") == 1) {
//			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Up);
//		} else if (Input.GetButtonUp ("Jump")) {
//			WolvesManager.shared ().Charge (activeLivestock);
//		}
//	}


}
