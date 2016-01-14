using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Artoncode.Core.Utility;


public class LivestockController : MonoBehaviour {


	public GameObject livestock;

	private Animator anim;
	private Coroutine jumpCoroutine;

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
			Jump(50);
	}

	void Jump(float height)
	{
		if(jumpCoroutine == null)
			jumpCoroutine = StartCoroutine(JumpUpdate(height));
	}

	IEnumerator JumpUpdate(float height, float speed = 50, float airTime = 0.15f, float jumpDuration = 0.5f)
	{
		anim.SetBool("Jump",true);
		yield return new WaitForSeconds(0.1f);


		float initPos = livestock.GetComponent<RectTransform>().position.y;
		float yPos = initPos;
		// at land, jump to top
		while(yPos < (initPos + height))
		{
			yield return new WaitForEndOfFrame();
			yPos = GameUtility.changeTowards(yPos,(initPos + height),speed/jumpDuration,Time.deltaTime);
			livestock.GetComponent<RectTransform>().position = new Vector3(livestock.GetComponent<RectTransform>().position.x,
			                                                               yPos,
			                                                               livestock.GetComponent<RectTransform>().position.z);
		}
		this.transform.SetAsLastSibling();
		//  at top, now landing
		yield return new WaitForSeconds(airTime);
		while (yPos > initPos)
		{
			yield return new WaitForEndOfFrame();
			yPos = GameUtility.changeTowards(yPos,initPos,speed/jumpDuration,Time.deltaTime);
			livestock.GetComponent<RectTransform>().position = new Vector3(livestock.GetComponent<RectTransform>().position.x,yPos,livestock.GetComponent<RectTransform>().position.z);
		}
		anim.SetBool("Jump",false);
		jumpCoroutine = null;
	}


}
