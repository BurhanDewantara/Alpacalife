using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class WorldManager : SingletonMonoBehaviour<WorldManager> {

	public delegate void WorldManagerDelegate();
	public event WorldManagerDelegate OnAssembleDone;
	public event WorldManagerDelegate OnDisassembleDone;

	public GameObject puffPrefab;
	public GameObject worldLivestockPrefab;

	public GameObject worldLivestockPanel;
	public GameObject worldLivestockAssemblePanel;
	public GameObject worldLivestockDisassemblePanel;

	public List<GameObject> environmentObject;
	public List<GameObject> worldLivestockObject;

	void Awake()
	{
		worldLivestockObject = new List<GameObject>();
	}

	public void Start()
	{
		RefreshEnvironment(true);
		LoadLivestock();
	}

	public void RefreshEnvironment(bool init = false)
	{
		foreach (GameObject item in environmentObject) {
			EnvironmentDrawer envDrawer = item.GetComponent<EnvironmentDrawer>();
			if(envDrawer)
			{
				EnvironmentSO envSO = UpgradeManager.shared().GetLatestEnvironment(envDrawer.type);
				if(envSO !=null)
				{
//					Debug.Log(envDrawer.sprite);
//					Debug.Log(envDrawer.sprite!= envSO.sprite);
//					Debug.Log(envDrawer.sprite!= envSO.sprite);
					if(!init && envDrawer.sprite != envSO.sprite)
					{
						Debug.Log("add");
						GameObject obj = Instantiate(puffPrefab,envDrawer.transform.position,Quaternion.identity) as GameObject;
						Destroy(obj,2);	
					}
					envDrawer.SetSprite(envSO.sprite);

				}
				else
					envDrawer.SetSprite(null);
			}

		}
	}

	public void LoadLivestock()
	{
		foreach (LivestockSO item in  UpgradeManager.shared().ownedLivestockList) {
			GameObject livestock = CreateLivestock(item);
			worldLivestockObject.Add(livestock);

		}

	}

	public GameObject CreateLivestock(LivestockSO lvsSO)
	{
		Vector3 position = Helper.RandomWithinArea(worldLivestockDisassemblePanel.GetComponents<BoxCollider2D>());
		GameObject livestock = Instantiate (worldLivestockPrefab,position,Quaternion.identity) as GameObject;
		livestock.transform.SetParent(worldLivestockPanel.transform,false);
		livestock.GetComponent<OwnedLivestockController>().SetLivestockSO(lvsSO);
		return livestock;

	}


	public void AddLivestock (LivestockSO lvsSO, Vector3 position, bool animate = false)
	{
		GameObject livestock = CreateLivestock(lvsSO);
		livestock.transform.position = position;

		if(animate)//with animation
		{
			iTween.MoveFrom(livestock,
				iTween.Hash(
					"position",new Vector3(5,-1,0)
					,"speed",2.5f
					,"isLocal",true
					,"easeType",iTween.EaseType.linear)
			);
		}
		worldLivestockObject.Add(livestock);

		InputManager.shared().receivers.Add(livestock);
	}



//	public void OnGUI()
//	{
//		if(GUI.Button(new Rect(200,0,100,20),"Livestock Spawn"))
//		{
//			worldLivestockObject.Add(CreateLivestock(UpgradeManager.shared().ownedLivestockList.Random()));
//		}
//
//		if(GUI.Button(new Rect(200,20,100,20),"LVS dateng"))
//		{
//			AddLivestock(UpgradeManager.shared().ownedLivestockList.Random(),new Vector3(0,-1,0),true);
//		}
//
//		if(GUI.Button(new Rect(200,40,100,20),"Assemble"))
//		{
//			LivestockAssemble();
//		}
//		if(GUI.Button(new Rect(200,60,100,20),"Disassemble"))
//		{
//			LivestockDissasemble();
//		}
//	}
//



	public void LivestockAssemble()
	{

		int livestockIdx = 0;
		int randomCounter = 50;
		float time = 0.5f;
		List<Vector3> positions = new List<Vector3>();

		while ( livestockIdx < worldLivestockObject.Count)
		{		
			positions.Clear();
			positions = GenerateAssemblePosition(randomCounter);


			int counter = 0;
			while (counter < randomCounter && livestockIdx < worldLivestockObject.Count) 
			{
				worldLivestockObject[livestockIdx].GetComponent<OwnedLivestockController>().IsActivated = false;
				BoxCollider2D col = worldLivestockObject[livestockIdx].GetComponent<BoxCollider2D>();

				Vector3 nextpos = positions [livestockIdx % randomCounter];
				Vector3 diff = (nextpos - worldLivestockObject [livestockIdx].transform.localPosition);
				worldLivestockObject [livestockIdx].GetComponent<OwnedLivestockController> ().SetDirection (diff.x > 0);

				iTween.MoveTo(worldLivestockObject[livestockIdx],
					iTween.Hash(
						"position",nextpos
						,"time",time
						,"isLocal",true
						,"easeType",iTween.EaseType.linear
						,"oncomplete","LivestockDoneAssemble"
						,"oncompletetarget",this.gameObject
						,"oncompleteparams",worldLivestockObject[livestockIdx]
					));
				counter++;
				livestockIdx++;
			}
		}



	}

	private void LivestockDoneAssemble(GameObject obj)
	{
		if(OnAssembleDone!=null)
			OnAssembleDone();
	}
	private void LivestockDoneDisassemble(GameObject obj)
	{
		obj.GetComponent<OwnedLivestockController>().IsActivated = true;
		if(OnDisassembleDone!=null)
			OnDisassembleDone();
	}


	public void LivestockDissasemble()
	{
		
		for (int i = 0; i < worldLivestockObject.Count; i++) {
			LivestockDoneDisassemble(worldLivestockObject[i]);
		}

//		float time = 1;
//		for (int i = 0; i < worldLivestockObject.Count; i++) {
//			iTween.MoveTo(worldLivestockObject[i],
//				iTween.Hash(
//					"position",Helper.RandomWithinArea(worldLivestockDisassemblePanel.GetComponents<BoxCollider2D>())
//					,"time",time
//					,"isLocal",true
//					,"easeType",iTween.EaseType.linear
//					,"oncomplete","LivestockDoneDisassemble"
//					,"oncompletetarget",this.gameObject
//					,"oncompleteparams",worldLivestockObject[i]
//				));
//		} 
	}




	private List<Vector3> GenerateAssemblePosition(int max = 100)
	{
		List<Vector3> positions = new List<Vector3>();
		for (int i = 0; i < max; i++) {
			positions.Add (Helper.RandomWithinArea(worldLivestockAssemblePanel.GetComponents<BoxCollider2D>()));
		}

		positions.Sort(delegate(Vector3 x, Vector3 y) {
			return x.y.CompareTo(y.y);
		});

		return positions;
	}

}
