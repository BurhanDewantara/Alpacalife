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
	public GameObject dieMark;
	public GameObject sweat;

	public Canvas currCanvas;
	public GameObject smokePrefab;

	public bool isOrdered = false;

	[HideInInspector]
	public ColorSO textSOColor;
	[HideInInspector]
	public ColorSO tintSOColor;


	[Header("Audio")]
	public AudioClip die;

	private Coroutine onFakePanicCoroutine;

	private LivestockSO livestock;
	private SpriteRenderer currSprite{
		get{
			return characterObject.GetComponent<SpriteRenderer>();
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

	public void SetLivestock(LivestockSO so)
	{
		livestock = so;
		currSprite.sprite = livestock.sprite;
	}

	public LivestockSO GetLivestock()
	{
		return livestock;
	}

	protected override void UpdateZOrder ()
	{
		base.UpdateZOrder ();
		if (currCanvas.sortingOrder != -Mathf.CeilToInt (this.transform.localPosition.y * 100)) {
			currCanvas.sortingOrder = -Mathf.CeilToInt (this.transform.localPosition.y * 100);
			dieMark.GetComponent<SpriteRenderer>().sortingOrder = -Mathf.CeilToInt( this.transform.localPosition.y * 100 ) ;
		}
	}

	public void SetLabel(ColorSO label, ColorSO color)
	{
		textSOColor = label;
		tintSOColor = color;
		nameTag.SetActive (true);
		nameTag.GetComponent<TMPro.TextMeshProUGUI> ().text = ColorSO.TintTextWithColor (textSOColor.colorType.ToString ().ToUpper(), tintSOColor.color);
	}

	private void HideLabel()
	{
		nameTag.SetActive(false);
		sweat.SetActive (false);
		dieMark.SetActive (false);
	}

	public void Move(DirectionType direction)
	{
		if (isOrdered)
			return;
		
		isOrdered = true;
		this.tag = "Untagged";
		HideLabel();
		iTween.Stop(this.gameObject);
		if(this.GetComponent<RandomSound>())
			this.GetComponent<RandomSound>().Play();	
		

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
			Destroy (this.gameObject,2);
		}
		if (col.CompareTag ("Wolf")) {
			if(!isOrdered)
				CreateSmoke ();
		}
	}

	void CreateSmoke()
	{
		GameObject obj = Instantiate (smokePrefab) as GameObject;
		obj.transform.position = Vector3.zero;
		obj.transform.SetParent (this.transform, false);
		Destroy (obj, 3);

	}


	public void FakePanic()
	{
		if(onFakePanicCoroutine == null)
			onFakePanicCoroutine = StartCoroutine (FakePanicCoroutine ());

	}

	IEnumerator FakePanicCoroutine()
	{
		AudioSource.PlayClipAtPoint (die, Camera.main.transform.position,0.5f);	
		sweat.SetActive (true);
		dieMark.SetActive (true);
		iTween.ShakePosition (dieMark, Vector3.one * 0.2f, 0.5f);
		iTween.ShakePosition (Camera.main.gameObject, Vector3.one * 0.2f, 1.0f);
		yield return new WaitForSeconds (1.0f);
		sweat.SetActive (false);
		dieMark.SetActive (false);

		onFakePanicCoroutine = null;
	}



	public void Panic()
	{
		AudioSource.PlayClipAtPoint (die, Camera.main.transform.position,0.5f);	
		sweat.SetActive (true);
		dieMark.SetActive (true);
		iTween.ShakePosition (dieMark, Vector3.one * 0.2f, 0.5f);
		iTween.ShakePosition (Camera.main.gameObject, Vector3.one * 0.2f, 1.0f);

	}

}
