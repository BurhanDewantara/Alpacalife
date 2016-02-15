using UnityEngine;
using System.Collections;

public class WolfContoller : CharacterCanvasController {



	protected override void UpdateRotation ()
	{
//		base.UpdateRotation ();
	}

	public void Charge(Vector2 position,float speed = 200.0f)
	{
		characterObject.transform.eulerAngles = Vector3.up * (this.transform.position.x < 0 ? 180 : 0);

		anim.speed = 1.5f;
		iTween.MoveTo (this.gameObject,
			iTween.Hash("position",new Vector3(position.x,position.y,0)
				,"speed",speed
				,"isLocal",true
				,"easeType",iTween.EaseType.linear
			));

	}
//
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Fence"))
		{
			Jump();				
		}
	}





}
