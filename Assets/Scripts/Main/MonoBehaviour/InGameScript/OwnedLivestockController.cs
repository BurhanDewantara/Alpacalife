using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OwnedLivestockController : CharacterCanvasController {

	LivestockSO livestockSO;

	public void SetLivestockSO(LivestockSO livestock)
	{
		print(livestock);
		print(livestock.sprite);
		print(characterObject.GetComponent<SpriteRenderer>());
		livestockSO = livestock;
		characterObject.GetComponent<SpriteRenderer>().sprite = livestockSO.sprite;		
	}

}
