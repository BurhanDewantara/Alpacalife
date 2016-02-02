using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : CharacterCanvasController {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Livestock"))
		{
			
			Jump();				
		}

	}


}
