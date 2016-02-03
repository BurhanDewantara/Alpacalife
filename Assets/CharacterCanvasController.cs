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
	protected Vector2 velocity;

	[Header("Movement Speed")]
	protected float minMoveSpeed = 4;
	protected float maxMoveSpeed = 5;
	protected float moveSpeed;

	[Header("Jump Power")]
	protected float minJumpPower = 10;
	protected float maxJumpPower = 15;
	protected float jumpPower;



	private float drag = 0.95f;
	private float gravity = 1;


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


	void UpdateSizeBasedOnPosition()
	{
		float currY = this.GetComponent<RectTransform> ().anchoredPosition.y;
		float deltaSize = -currY / 1000;
		this.GetComponent<RectTransform> ().localScale = new Vector2 (1 + deltaSize, 1 + deltaSize);

	}

	protected virtual void MovementUpdate()
	{

		//MOVE
		this.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity.x,velocity.y);

		//Rotation / Facing
		characterObject.GetComponent<RectTransform> ().eulerAngles = Vector3.up * (velocity.x > 0 ? 180 : 0);


		if(isJump)
		{
			jumpPower *= drag;
			gravity /= drag;
			characterObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,jumpPower);
			characterObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,gravity);
		}

		//Landing
		if(characterObject.GetComponent<RectTransform>().anchoredPosition.y < 0)
		{
			isJump = false;
			anim.SetBool("Jump",isJump);
			characterObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			gravity = 1;
		}
//		UpdateSizeBasedOnPosition ();s
	}



	void Update()
	{
		//movement & jump
		MovementUpdate();
	
	}

}
