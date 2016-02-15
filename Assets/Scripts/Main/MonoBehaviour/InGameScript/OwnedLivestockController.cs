using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OwnedLivestockController : CharacterCanvasController {

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
		}
		get { 
			return isActivated;
		}
	}

	void Start()
	{
		Reset ();
	}

	void Reset()
	{
		isActivated = true;
		isWaiting = true;
		velocity = Vector3.zero;
		waitTime = Random.Range (4, 12);
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
		if(!IsActivated) return;

		base.MovementUpdate ();

		if (!isWaiting) {
			if (Vector3 .Distance (targetPos, this.transform.position) < 0.1) {
				isWaiting = true;
				waitTime = Random.Range (4, 12);
				velocity = Vector3.zero;
				targetPos = Vector3.zero;
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
}
