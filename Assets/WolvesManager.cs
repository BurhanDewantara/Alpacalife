using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class WolvesManager : SingletonMonoBehaviour<WolvesManager> {

	public GameObject WolfPrefab;
	public List<RectTransform> WolfSpawnPoints;

	private int wolvesCount = 10;
	private List<GameObject> wolvesList;

	void Awake()
	{
		wolvesList = new List<GameObject> ();
	}

	public void Charge(GameObject livestock)
	{
		StartCoroutine(SpawnWolves(livestock.GetComponent<RectTransform>()));
	}


	IEnumerator SpawnWolves(RectTransform livestockTransfrom,float delay = 0.1f)
	{
		float speed = 500;
		while (wolvesList.Count < wolvesCount) {
			Vector2 newPos = Helper.RandomWithinArea (WolfSpawnPoints.Random());
			
			GameObject wolf = Instantiate (WolfPrefab,newPos,Quaternion.identity) as GameObject;

			Vector2 targetPos = Helper.RandomWithinArea (livestockTransfrom);
			wolf.GetComponent<WolfContoller> ().Charge (targetPos, speed);
			wolf.transform.SetAsFirstSibling ();
			wolf.transform.SetParent (this.transform,false);
			wolvesList.Add (wolf);

			yield return new WaitForSeconds (delay);
		}
//		currLivestock.GetComponent<LivestockController>().MoveToReadyPosition(newPos,speed);			


	}





}
