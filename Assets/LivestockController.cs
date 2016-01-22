using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : MonoBehaviour {


	public GameObject livestock;

	private Animator anim;

	private bool isJump = false;
	private Vector2 velocity;
	private float jumpSpeed;
	private float drag = 0.95f;
	private float gravity = 1;


	[SerializeField] private bool isInteractable;
	public bool IsInteractable
	{
		get {
			return isInteractable;
		}
	}

	private float minSpeed = 5;
	private float maxSpeed = 7;
	private float minJumpPower = 10;
	private float maxJumpPower = 15;



	void Awake()
	{
		this.anim = this.GetComponent<Animator>();
	}



	void Start()
	{
		isInteractable = true;
	}


	void Move(Vector2 position)
	{
		if (isInteractable) {
			isInteractable = false;
		}
	}

	void Jump(float speed)
	{
		if (!isJump) {
			isJump = true;
			anim.SetBool("Jump",isJump);
			jumpSpeed = speed;
		}
	}

	void InputUpdate()
	{
		if (isInteractable) {
			velocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * minSpeed;
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

	void Update()
	{
		InputUpdate();

		//movement & jump
		this.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity.x,velocity.y);
		jumpSpeed *= drag;
		gravity /= drag;
		livestock.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,jumpSpeed);
		livestock.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,gravity);

		//Rotation
		livestock.GetComponent<RectTransform> ().eulerAngles = Vector3.up * (velocity.x > 0 ? 180 : 0);

		//Landing
		if(livestock.GetComponent<RectTransform>().anchoredPosition.y < 0)
		{
			isJump = false;
			anim.SetBool("Jump",isJump);
			livestock.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			gravity = 1;
		}
		UpdateSizeBasedOnPosition ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Jump(minJumpPower);
	}


}
