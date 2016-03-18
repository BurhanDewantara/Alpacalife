using UnityEngine;
using System.Collections;

public class WorldTimer : MonoBehaviour {

	float time = 0;
		
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if(time > 1.0f)
		{
			time -= 1.0f;
			PlayerStatisticManager.shared().TotalPlayTime +=1;
		}

	}
}
