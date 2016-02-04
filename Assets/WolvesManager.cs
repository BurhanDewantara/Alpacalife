using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class WolvesManager : SingletonMonoBehaviour<WolvesManager> {

	public GameObject WolfPrefab;
	public GameObject WolfSpawnPoints;

	private int wolvesCount = 10;
	private List<GameObject> wolvesList;

	void Awake()
	{
		wolvesList = new List<GameObject> ();
	}


	public void Charge(GameObject livestock)
	{
		StartCoroutine(SpawnWolves(livestock));
	}


	IEnumerator SpawnWolves(GameObject livestockTransfrom,float delay = 0.1f)
	{
		float speed = 6;
		while (wolvesList.Count < wolvesCount) {
			Vector3 newPos = Helper.RandomWithinArea (WolfSpawnPoints.GetComponents<BoxCollider2D>());
			
			GameObject wolf = Instantiate (WolfPrefab,newPos,Quaternion.identity) as GameObject;

			Vector3 targetPos = livestockTransfrom.transform.localPosition + Helper.RandomWithinArea (livestockTransfrom.GetComponents<BoxCollider2D>());

			wolf.GetComponent<WolfContoller> ().Charge (targetPos, speed);
			wolf.transform.SetParent (this.transform,false);
			wolvesList.Add (wolf);

			yield return new WaitForSeconds (delay);
		}


	}





}
