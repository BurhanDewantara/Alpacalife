using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : CharacterCanvasController {

	public delegate void LivestockControllerDelegate (GameObject sender);
	public event LivestockControllerDelegate OnLivestockReceivedOrder;

	public GameObject nameTag;
	public LivestockColorHandler colorHandler;

	public bool isReady = false;
	public bool isOrdered = false;
	public enum DirectionType
	{
		Left,Right,Down,Up
	};


	public void MoveToReadyPosition(Vector2 position,float speed = 200.0f)
	{
		isReady = true;
		iTween.MoveTo (this.gameObject,
			iTween.Hash("position",new Vector3(position.x,position.y,0)
				,"speed",speed
				,"isLocal",true
				,"easeType",iTween.EaseType.linear
			));
	}

	private void RemoveNameLabel()
	{
		this.GetComponent<live
	}

	public void Move(DirectionType direction)
	{
		if (!isReady)
			return;
		if (isOrdered)
			return;



		isOrdered = true;
		iTween.Stop(this.gameObject);

		switch (direction) {
		case DirectionType.Left:
			velocity = Vector2.left * moveSpeed;
			break;
		case DirectionType.Right:
			velocity = Vector2.right * moveSpeed;
			break;
		case DirectionType.Down:
			velocity = Vector2.down * moveSpeed;
			break;
		case DirectionType.Up:
			velocity = Vector2.up * moveSpeed;
			break;
		}

		if (OnLivestockReceivedOrder != null) {
			OnLivestockReceivedOrder (this.gameObject);
		}
	}


	void Update()
	{
//		InputUpdate();
		MovementUpdate();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Fence"))
		{
			Jump();				
		}

		if (col.CompareTag ("Destroyer")) {
			
			Destroy (this.gameObject);
		}

	}


}
