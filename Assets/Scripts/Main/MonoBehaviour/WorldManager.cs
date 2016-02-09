using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artoncode.Core;

public class WorldManager : SingletonMonoBehaviour<WorldManager> {

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
		RefreshEnvironment();
		LoadLivestock();
	}

	public void RefreshEnvironment()
	{
		foreach (GameObject item in environmentObject) {
			EnvironmentDrawer envDrawer = item.GetComponent<EnvironmentDrawer>();
			if(envDrawer)
			{
				EnvironmentSO envSO = UpgradeManager.shared().GetLatestEnvironment(envDrawer.type);
				if(envSO !=null)
					envDrawer.SetSprite(envSO.sprite);
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
	}



	public void OnGUI()
	{
		if(GUI.Button(new Rect(200,0,100,20),"Livestock Spawn"))
		{
			worldLivestockObject.Add(CreateLivestock(UpgradeManager.shared().ownedLivestockList.Random()));
		}

		if(GUI.Button(new Rect(200,20,100,20),"LVS dateng"))
		{
			AddLivestock(UpgradeManager.shared().ownedLivestockList.Random(),new Vector3(0,-1,0),true);
		}

		if(GUI.Button(new Rect(200,40,100,20),"Assemble"))
		{
			LivestockAssemble();
		}
		if(GUI.Button(new Rect(200,60,100,20),"Disassemble"))
		{
			LivestockDissasemble();
		}
	}




	public void LivestockAssemble()
	{
		int randomCounter = 50;
		float speed = 2.5f;
		List<Vector3> positions = GenerateAssemblePosition(randomCounter);

		for (int i = 0; i < worldLivestockObject.Count; i++) {

			iTween.MoveTo(worldLivestockObject[i],
				iTween.Hash(
					"position",positions[i]
					,"speed",speed
					,"isLocal",true
					,"easeType",iTween.EaseType.linear
				));
		} 
	}


	public void LivestockDissasemble()
	{
		float speed = 2.5f;
		for (int i = 0; i < worldLivestockObject.Count; i++) {
			iTween.MoveTo(worldLivestockObject[i],
				iTween.Hash(
					"position",Helper.RandomWithinArea(worldLivestockDisassemblePanel.GetComponents<BoxCollider2D>())
					,"speed",speed
					,"isLocal",true
					,"easeType",iTween.EaseType.linear
				));
		} 
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
