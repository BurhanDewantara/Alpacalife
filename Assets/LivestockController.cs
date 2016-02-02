using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : CharacterCanvasController {

	LivestockColorHandler colorHandler;

	public bool isReady = false;
	public bool isOrdered = false;


	void Awake()
	{
		
	}



	void MoveLeft()
	{
		velocity = Vector2.left * moveSpeed;
	}

	void MoveRight()
	{
		velocity = Vector2.right * moveSpeed;
	}

	void MoveDown()
	{
		velocity = Vector2.down * moveSpeed;

	}

	void MoveUp()
	{
		velocity = Vector2.up * moveSpeed ;

	}


	void InputUpdate()
	{
		if (isInteractable) {
			////			velocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * moveSpeed;
			if (Input.GetAxisRaw ("Horizontal") == 1) {
				MoveRight ();
			}
			if (Input.GetAxisRaw ("Horizontal") == -1) {
				MoveLeft ();
			}
			if (Input.GetAxisRaw ("Vertical") == -1) {
				MoveDown ();
			}
			if (Input.GetAxisRaw ("Vertical") == 1) {
				MoveUp ();
			}
			if (Input.GetButton("Jump")) {
				velocity = Vector2.zero;
			}
		}
	}

	void Update()
	{
		InputUpdate();
		MovementUpdate();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Fence"))
		{
			Jump();				
		}

	}


}
