using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PaperContentViewer : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

	public delegate void PaperContentViewerDelegate (GameObject sender);

	public event PaperContentViewerDelegate OnDropAtRightPanel;
	public event PaperContentViewerDelegate OnDropAtLeftPanel;
	public event PaperContentViewerDelegate OnDropAtTopPanel;
	public event PaperContentViewerDelegate OnDropAtBottomPanel;

	private Vector3 _imageOriginalPosition;
	private Vector2 _deltaMovement;

	public bool IsAccessible;

	public void OnBeginDrag (PointerEventData eventData)
	{
//		Debug.Log (eventData);
		if (!IsAccessible)
			return;

		_imageOriginalPosition = this.GetComponent<RectTransform> ().position;
		_deltaMovement = Vector2.zero;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (!IsAccessible)
			return;

//		if(!eventData.delta.Equals(Vector2.zero))
//			_deltaMovement = eventData.delta;
		
		if (Mathf.Abs (_deltaMovement.x) > Mathf.Abs (_deltaMovement.y) && _deltaMovement.x > 0) {
			if (OnDropAtRightPanel != null)
				OnDropAtRightPanel (this.gameObject);

		} else if (Mathf.Abs (_deltaMovement.y) > Mathf.Abs (_deltaMovement.x) && _deltaMovement.y > 0) {
			if (OnDropAtTopPanel != null)
				OnDropAtTopPanel (this.gameObject);

		} else if (Mathf.Abs (_deltaMovement.x) > Mathf.Abs (_deltaMovement.y) && _deltaMovement.x < 0) {
			if (OnDropAtLeftPanel != null)
				OnDropAtLeftPanel (this.gameObject);

		} else if (Mathf.Abs (_deltaMovement.y) > Mathf.Abs (_deltaMovement.x) && _deltaMovement.y < 0) {
			if (OnDropAtBottomPanel != null)
				OnDropAtBottomPanel (this.gameObject);
		}

	}

	public void OnDrag (PointerEventData eventData)
	{
//		Debug.Log (eventData);
		if (!IsAccessible)
			return;

		_deltaMovement += new Vector2 (eventData.delta.x, eventData.delta.y) * 2; 
		this.GetComponent<RectTransform> ().position = _imageOriginalPosition + new Vector3(_deltaMovement.x,_deltaMovement.y);

	}


}
