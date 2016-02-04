using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;


public class LivestockManager : SingletonMonoBehaviour<LivestockManager> {

	public GameObject spawnPoint;
	public GameObject gatheringPoint;

	public GameObject livestockPrefab;
	public int totalLivestock;

	private List<GameObject> queueLivestock;
	private int livestockCounter;



	private GameObject activeLivestock{
		get{
			if(queueLivestock != null)
				if(queueLivestock.Count > 0)
					return queueLivestock[0];
			return null;
		}
	}

	void Start()
	{
		Init ();
	}

	void Init()
	{
		livestockCounter = 0;
		queueLivestock = new List<GameObject> ();
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


	void OnGUI()
	{
		if (GUILayout.Button ("spawn"))
		{
			Spawn ();
		}
	}

	void Spawn()
	{
		 
		Vector3 newPos = Helper.RandomWithinArea (gatheringPoint.GetComponents<BoxCollider2D>());
		float speed = 2.5f;

		GameObject currLivestock = SpawnLivestock ();

		currLivestock.GetComponent<LivestockController>().MoveToReadyPosition(newPos,speed);			
		queueLivestock.Add (currLivestock);
		livestockCounter++;

	}

	void Update()
	{
		InputUpdate ();
	}

	void InputUpdate()
	{
		//change this input with swipe
		if (activeLivestock == null) {
			return;
		}

		if (Input.GetAxisRaw ("Horizontal") == 1) {
			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Right);
		} else if (Input.GetAxisRaw ("Horizontal") == -1) {
			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Left);
		} else if (Input.GetAxisRaw ("Vertical") == -1) {
			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Down);
		} else if (Input.GetAxisRaw ("Vertical") == 1) {
			activeLivestock.GetComponent<LivestockController> ().Move (DirectionType.Up);
		} else if (Input.GetButtonUp ("Jump")) {
			WolvesManager.shared ().Charge (activeLivestock);
		}
	}

	public void Go(DirectionType dir)
	{
		if (activeLivestock == null) {
			return;
		}

		activeLivestock.GetComponent<LivestockController> ().Move (dir);

		if(true) //swipe nya bener
		{
			Spawn();			
		}
		else
		{
			WolvesManager.shared ().Charge (activeLivestock);
		}
	}

}
