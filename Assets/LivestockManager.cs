﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LivestockManager : MonoBehaviour {

	public RectTransform spawnPoint;
	public RectTransform gatheringPoint;


	public GameObject livestockPrefab;
	public int totalLivestock;

	private List<GameObject> queueLivestock;
	private float counter;

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
		counter = 0;
		queueLivestock = new List<GameObject> ();
	}


	private GameObject SpawnLivestock()
	{
		Vector2 newPos = RandomWithinArea (spawnPoint);
		GameObject newLivestock = Instantiate (livestockPrefab, newPos, Quaternion.identity) as GameObject;
		newLivestock.name = counter.ToString();
		newLivestock.transform.SetParent (this.transform, false);
		newLivestock.transform.SetAsFirstSibling ();
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


	void Update()
	{
		InputUpdate ();
	}
	void InputUpdate()
	{
		//change this input with swipe
		if (activeLivestock == null) {
			print (activeLivestock);
			print (queueLivestock.Count);
			return;
		}

		if (Input.GetAxisRaw ("Horizontal") == 1) {
			print (activeLivestock);
			activeLivestock.GetComponent<LivestockController>().Move (LivestockController.DirectionType.Right);
		}
		if (Input.GetAxisRaw ("Horizontal") == -1) {
			activeLivestock.GetComponent<LivestockController>().Move (LivestockController.DirectionType.Left);
		}
		if (Input.GetAxisRaw ("Vertical") == -1) {
			activeLivestock.GetComponent<LivestockController>().Move (LivestockController.DirectionType.Down);
		}
		if (Input.GetAxisRaw ("Vertical") == 1) {
			activeLivestock.GetComponent<LivestockController>().Move (LivestockController.DirectionType.Up);
		}
	}


	void Spawn()
	{
		 
		Vector2 newPos = RandomWithinArea (gatheringPoint);
		float baseSpeed = 100;
		float speed = Mathf.Clamp(counter * 10 + baseSpeed, baseSpeed , baseSpeed + 10 * 50);
		GameObject currLivestock = SpawnLivestock ();
		currLivestock.GetComponent<LivestockController>().MoveToReadyPosition(newPos,speed);			
		queueLivestock.Add (currLivestock);
		counter++;
		print (queueLivestock.Count);
	}


	Vector2 RandomWithinArea(RectTransform point)
	{
		Vector2 newPos = new Vector2(Random.Range(
				point.anchoredPosition.x - point.sizeDelta.x/2,
				point.anchoredPosition.x + point.sizeDelta.x/2),
			Random.Range(
				point.anchoredPosition.y - point.sizeDelta.y/2,
				point.anchoredPosition.y + point.sizeDelta.y/2));
		return newPos;
	}
}
