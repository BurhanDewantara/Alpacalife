using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class CharacterCanvasController : MonoBehaviour {


	public GameObject characterObject;

	protected Animator anim;

	private bool isJump;
	private Vector2 velocity;

	[Header("Movement Speed")]
	protected float minMoveSpeed = 4;
	protected float maxMoveSpeed = 5;
	private float moveSpeed;

	[Header("Jump Power")]
	protected float minJumpPower = 10;
	protected float maxJumpPower = 15;
	private float jumpPower;



	private float drag = 0.95f;
	private float gravity = 1;


	private bool isInteractable;
	public bool IsInteractable
	{
		get {
			return isInteractable;
		}
	}




	void Awake()
	{
		this.anim = this.GetComponent<Animator>();
	}

	void Start()
	{
		isInteractable = true;
		isJump = false;
		moveSpeed = Random.Range (minMoveSpeed, maxMoveSpeed);
	}


	public void Move(Vector2 targetPos)
	{
		if (isInteractable) 
		{
			this.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(this.GetComponent<RectTransform>().anchoredPosition,targetPos,3);
			isInteractable = false;
		}

//		velocity = 
	}

	public void Jump()
	{
		if (!isJump) {
			isJump = true;
			anim.SetBool("Jump",isJump);
			jumpPower = Random.Range (minJumpPower, maxJumpPower);
		}
	}

	protected virtual void MoveLeft()
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


//				Vector2 targetPos = this.GetComponent<RectTransform> ().anchoredPosition;
//				targetPos.x += 200;
//				Move (targetPos);
//			velocity
//			}

		}
//		if(Input.GetButton("Jump"))
//			Jump(maxspeed);
	}

	void UpdateSizeBasedOnPosition()
	{
		float currY = this.GetComponent<RectTransform> ().anchoredPosition.y;
		float deltaSize = -currY / 1000;
		this.GetComponent<RectTransform> ().localScale = new Vector2 (1 + deltaSize, 1 + deltaSize);

	}

	void MovementUpdate()
	{

//		velocity.y *= this.GetComponent<RectTransform> ().localScale.y;

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
		InputUpdate();

		//movement & jump
		MovementUpdate();
	
	}

}
