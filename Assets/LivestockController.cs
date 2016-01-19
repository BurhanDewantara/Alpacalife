using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : MonoBehaviour {


	public GameObject livestock;

	private Animator anim;
	private Coroutine jumpCoroutine;

	private bool isGrounded = true;
	private bool isJump = false;
	private Vector2 velocity;
	private float jumpSpeed;
	private float drag = 0.89f;
	private float gravity = 1;
	private float speed = 30;

	void Awake()
	{
		this.anim = this.GetComponent<Animator>();
	}


	void Move(Vector2 position)
	{

	}


	void OnGUI()
	{
		if(GUILayout.Button("Jump"))
		{
//			Jump(150);
			isJump = true;
			jumpSpeed = speed;
		}
			
	}



	void Jump(float height)
	{
		if(jumpCoroutine == null)
			jumpCoroutine = StartCoroutine(JumpUpdate(height));
	}



	void Update()
	{
		velocity =  new Vector2 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * 10;
		if(Input.GetButton("Jump"))
		{
			isJump = true;
			jumpSpeed = speed;
		}


		this.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity.x,velocity.y);

		jumpSpeed *= drag;
		gravity /= drag;
		livestock.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,jumpSpeed);
		livestock.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,gravity);
		if(livestock.GetComponent<RectTransform>().anchoredPosition.y < 0)
		{
			livestock.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			gravity =1;
		}

//		livestock.GetComponent<RectTransform>().localPosition -= Vector3.down*;

	

	}



	IEnumerator JumpUpdate(float height, float speed = 50, float airTime = 0.05f, float jumpDuration = 0.2f)
	{
		anim.SetBool("Jump",true);
		yield return new WaitForSeconds(0.1f);


		float initPos = livestock.GetComponent<RectTransform>().localPosition.y;
		float yPos = initPos;
		float velocity = 1;
		// at land, jump to top

		do
		{
			yield return new WaitForEndOfFrame();
//			yPos = GameUtility.changeTowards(yPos,(initPos + height),speed/jumpDuration,Time.deltaTime);
//			yPos = Mathf.Lerp (yPos,initPos+height,Time.deltaTime);
			yPos = Mathf.SmoothDamp(yPos,initPos+height,ref velocity, jumpDuration);
			Debug.Log(velocity);
			livestock.GetComponent<RectTransform>().localPosition = new Vector3(livestock.GetComponent<RectTransform>().localPosition.x,
			                                                             	 	yPos,
			                                                                    livestock.GetComponent<RectTransform>().localPosition.z);
		}while(velocity > 5.0f);


		this.transform.SetAsLastSibling();
		//  at top, now landing
		airTime = jumpDuration / 2.0f;
//		yield return new WaitForSeconds(airTime);

		do
		{
			yield return new WaitForEndOfFrame();
//			yPos = GameUtility.changeTowards(yPos,initPos,speed/jumpDuration,Time.deltaTime);
//			yPos = Mathf.Lerp(yPos,initPos,Time.deltaTime);
			yPos = Mathf.SmoothDamp(yPos,initPos,ref velocity, jumpDuration);
			Debug.Log(velocity);
			livestock.GetComponent<RectTransform>().localPosition = new Vector3(livestock.GetComponent<RectTransform>().localPosition.x,
			                                                                    yPos,
			                                                                    livestock.GetComponent<RectTransform>().localPosition.z);
		}while(velocity < -5.0f);

		anim.SetBool("Jump",false);
		jumpCoroutine = null;
	}


}
