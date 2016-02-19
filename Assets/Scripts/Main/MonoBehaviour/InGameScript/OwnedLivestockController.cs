using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core;

public class OwnedLivestockController : CharacterCanvasController,IInputManagerDelegate {

	LivestockSO livestockSO;

	public Vector3 targetPos;
	public float waitTime;
	public bool isWaiting;

	bool isActivated;
	public bool IsActivated
	{
		set{ 
			isActivated = value;
			if (isActivated)
				Reset ();
			else
			{
				velocity = Vector3.zero;
				targetPos = Vector3.zero;
			}
		}
		get { 
			return isActivated;
		}
	}

	void Start()
	{
		drag = 0.99f;
		maxJumpPower = 0.18f;
		minJumpPower = 0.15f;

		InputManager.shared().receivers.Add(this.gameObject);
		Reset ();
	}

	public void Reset()
	{
		isActivated = true;
		isWaiting = true;
		waitTime = Random.Range (4, 12);
		velocity = Vector3.zero;
		targetPos = Vector3.zero;
	}

	public void SetLivestockSO(LivestockSO livestock)
	{
		livestockSO = livestock;
		characterObject.GetComponent<SpriteRenderer>().sprite = livestockSO.sprite;		
	}

	public void SetDirection(bool isFlipped)
	{
		characterObject.transform.eulerAngles = Vector3.up * (isFlipped ? 180 : 0);

	}
	protected override void UpdateRotation()
	{
		if(!IsActivated) return;
		base.UpdateRotation ();
	}

	protected override void MovementUpdate ()
	{
		base.MovementUpdate ();

		if(!IsActivated) return;

		if (!isWaiting) {
			if (Vector3 .Distance (targetPos, this.transform.position) < 0.1) {
				Wait();
			}
		} else {
			waitTime -= Time.deltaTime;
			if (waitTime < 0) {
				GameObject obj = this.transform.parent.FindChild ("DisassemblePoint").gameObject;
				targetPos = Helper.RandomWithinArea (obj.GetComponents<BoxCollider2D> ());
				velocity = targetPos - this.transform.position;
//				velocity *= Time.deltaTime;
				velocity = velocity.normalized * Time.deltaTime;
				isWaiting = false;
			} 
		}
	}

	private void Wait()
	{
		isWaiting = true;
		waitTime = Random.Range (4, 12);
		velocity = Vector3.zero;
		targetPos = Vector3.zero;

	}


	public void touchStateChanged (TouchInput []touches)
	{
		if(!IsActivated) return;

		TouchInput touch = touches [0];

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (-1) || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject (touch.fingerId)) return;

		switch (touch.phase) 
		{
		case TouchPhase.Began: break;
		case TouchPhase.Moved: break;
		case TouchPhase.Ended: 

			Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(touch.end);
			worldTouchPos = new Vector3(worldTouchPos.x,worldTouchPos .y,0);

			if(this.GetComponent<BoxCollider2D>().bounds.Contains(worldTouchPos))
			{
				Wait();
				Jump();
				if(this.GetComponent<RandomSound>())
					this.GetComponent<RandomSound>().Play();	
				
			}
				
			break;
		}
	}
}
