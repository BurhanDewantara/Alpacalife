using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannedFoodContentViewer : MonoBehaviour {

	public delegate void CanContentViewerDelegate(GameObject sender);
	public event CanContentViewerDelegate OnCanClicked;
	public event CanContentViewerDelegate OnCanDestroyed;

	public int state;
	private Animator anim;

	void Awake()
	{
		anim = this.GetComponent<Animator> ();
		this.GetComponent<Button> ().onClick.AddListener (OnClick);
		state = 1;
	}

	public void OnClick()
	{
		if (OnCanClicked != null)
			OnCanClicked (this.gameObject);
	}

	public void Slide()
	{
		state++;
		state = Mathf.Clamp (state, 1, 6);
		anim.SetInteger ("State",state);

	}
	public void Drop()
	{
		if(OnCanDestroyed!=null) OnCanDestroyed(this.gameObject);	
	}




}
