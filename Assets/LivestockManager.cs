﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LivestockManager : MonoBehaviour {

	public GameObject gatheringPoint;

	public GameObject livestockPrefab;
	public int totalLivestock;

	private List<GameObject> queueLivestock;

	private GameObject activeLivestock{
		get{
			if(queueLivestock != null )
				if(queueLivestock.Count > 0)
					return queueLivestock[0];
			return null;
		}
	}





	public void SpawnLivestock()
	{


	}





}