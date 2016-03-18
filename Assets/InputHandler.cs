using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	void OnApplicationPause(bool pauseStatus)
	{
		GameDataManager.shared().save();
		if(WorldManager.shared())
			WorldManager.shared().LivestockStopMove();

	}

}
