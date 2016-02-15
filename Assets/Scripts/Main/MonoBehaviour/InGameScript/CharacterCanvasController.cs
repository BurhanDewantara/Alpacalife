using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class CharacterCanvasController : MonoBehaviour {


	public GameObject characterObject;

	protected Animator anim
	{
		get{
			return this.GetComponent<Animator>();
		}
	}


	protected bool isJump;
	public Vector3 velocity;

	[Header("Movement Speed")]
	protected float minMoveSpeed = 0.05f;
	protected float maxMoveSpeed = 0.06f;
	protected float moveSpeed;

	[Header("Jump Power")]
	protected float minJumpPower = 0.20f;
	protected float maxJumpPower = 0.22f;
	protected float jumpPower;



	private float drag = 0.98f;
	private float gravity = 0.1f;


	protected bool isInteractable;
	public bool IsInteractable
	{
		get {
			return isInteractable;
		}
	}

	void Awake(){
		isInteractable = true;
		isJump = false;
		moveSpeed = Random.Range (minMoveSpeed, maxMoveSpeed);
	}

	public void Jump()
	{
		if (!isJump) {
			isJump = true;
			anim.SetBool("Jump",isJump);
			jumpPower = Random.Range (minJumpPower, maxJumpPower);
		}
	}


	protected virtual void UpdateRotation()
	{
		//Rotation / Facing
		characterObject.transform.eulerAngles = Vector3.up * (velocity.x > 0 ? 180 : 0);
	}

	protected virtual void MovementUpdate()
	{

		//MOVE
		this.transform.localPosition += velocity;

		if(isJump)
		{
			jumpPower *= drag;
			gravity /= drag;
			characterObject.transform.localPosition += new Vector3(0,jumpPower,0);
			characterObject.transform.localPosition -= new Vector3(0,gravity,0);
		}

		//Landing
		if(characterObject.transform.localPosition.y < 0)
		{
			isJump = false;
			anim.SetBool("Jump",isJump);
			characterObject.transform.localPosition = Vector3.zero;
			gravity = 0.1f;
		}

	}

	public virtual void Update()
	{
		//movement & jump
		MovementUpdate ();
		UpdateRotation ();
		UpdateZOrder ();
	}

	protected virtual void UpdateZOrder()
	{
		if(characterObject.GetComponent<SpriteRenderer>().sortingOrder != -Mathf.CeilToInt( this.transform.localPosition.y * 100 ) )
			characterObject.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.CeilToInt( this.transform.localPosition.y * 100 ) ;
	}
}
