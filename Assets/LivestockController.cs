using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : CharacterCanvasController {

	public delegate void LivestockControllerDelegate (GameObject sender);
	public event LivestockControllerDelegate OnLivestockReceivedOrder;
	public GameObject nameTag;

	LivestockColorHandler colorHandler
	{
		get{
			return this.GetComponent<LivestockColorHandler>();
		}
	}

	public bool isOrdered = false;
	public enum DirectionType
	{
		Left,Right,Down,Up
	};


	public void MoveToReadyPosition(Vector2 position,float speed = 200.0f)
	{
		iTween.MoveTo (this.gameObject,
			iTween.Hash("position",new Vector3(position.x,position.y,0)
				,"speed",speed
				,"isLocal",true
				,"easeType",iTween.EaseType.linear
			));
	}

	public void SetLabel(string text,SOColor color)
	{
		
	}

	private void RemoveLabel()
	{
		nameTag.SetActive(false);
	}

	public void Move(DirectionType direction)
	{
		if (isOrdered)
			return;
		
		isOrdered = true;
		RemoveLabel();
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
