using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;

public enum DirectionType
{
	Left,Right,Down,Up
};

public class LivestockController : CharacterCanvasController {

	public delegate void LivestockControllerDelegate (GameObject sender);
	public event LivestockControllerDelegate OnLivestockReceivedOrder;
	public GameObject nameTag;
	public Canvas currCanvas;
	public bool isOrdered = false;

	private SpriteRenderer currSprite{
		get{
			return characterObject.GetComponent<SpriteRenderer>();
		}
	}


	LivestockColorHandler colorHandler
	{
		get{
			return this.GetComponent<LivestockColorHandler>();
		}
	}

	public void MoveToReadyPosition(Vector3 position,float speed)
	{
		iTween.MoveTo (this.gameObject,
			iTween.Hash("position",position
				,"speed",speed
				,"isLocal",true
				,"easeType",iTween.EaseType.linear
			));
	}

	protected override void UpdateZOrder ()
	{
		base.UpdateZOrder ();
		if(currCanvas.sortingOrder != -Mathf.CeilToInt(this.transform.localPosition.y * 100 ))
			currCanvas.sortingOrder = -Mathf.CeilToInt(this.transform.localPosition.y * 100 );
		
	
	}

	public void SetLabel(string text,SOColor color)
	{
		
	}

	private void HideLabel()
	{
		nameTag.SetActive(false);
	}

	public void Move(DirectionType direction)
	{
		if (isOrdered)
			return;
		
		isOrdered = true;
		HideLabel();
		iTween.Stop(this.gameObject);

		switch (direction) {
		case DirectionType.Left:
			velocity = Vector3.left * moveSpeed;
			break;
		case DirectionType.Right:
			velocity = Vector3.right * moveSpeed;
			break;
		case DirectionType.Down:
			velocity = Vector3.down * moveSpeed;
			break;
		case DirectionType.Up:
			velocity = Vector3.up * moveSpeed;
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
