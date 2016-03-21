using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class WolvesManager : SingletonMonoBehaviour<WolvesManager> {

	public GameObject WolfPrefab;
	public GameObject WolfSpawnPoints;
	public AudioClip WolfAudio;

	private int wolvesCount = 10;
	private List<GameObject> wolvesList;

	void Awake()
	{
		wolvesList = new List<GameObject> ();
	}


	public void Charge(GameObject[] livestocks)
	{
		StartCoroutine(SpawnWolves(livestocks));
		StartCoroutine(SpawnWolvesSound());
	}



	public void RemoveAllWolves()
	{
		foreach (GameObject item in wolvesList) {
			Destroy(item);
		}
		wolvesList.Clear();
	}

	IEnumerator SpawnWolvesSound(float delay = 0.1f)
	{
		yield return new WaitForSeconds (delay);
		AudioSource.PlayClipAtPoint(WolfAudio,Camera.main.transform.position,0.5f);
	}

	IEnumerator SpawnWolves(GameObject[] livestocks,float delay = 0.1f)
	{
		float speed = 6;

		while (wolvesList.Count < wolvesCount) {
			Vector3 newPos = Helper.RandomWithinArea (WolfSpawnPoints.GetComponents<BoxCollider2D>());
			
			GameObject wolf = Instantiate (WolfPrefab,newPos,Quaternion.identity) as GameObject;

			GameObject livestockTransform = livestocks.Random ();
			Vector3 targetPos = livestockTransform.transform.localPosition + Helper.RandomWithinArea (livestockTransform.GetComponents<BoxCollider2D>());
			
			wolf.GetComponent<WolfContoller> ().Charge (targetPos, speed);
			wolf.transform.SetParent (this.transform,false);
			wolvesList.Add (wolf);

			yield return new WaitForSeconds (delay);
		}
	}





}
