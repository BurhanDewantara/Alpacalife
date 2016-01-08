using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PaperContent : MonoBehaviour {

	public GameObject PaperImage;
	public GameObject PaperText;

	public PaperItem paper;


	public void SetItem(PaperItem paper)
	{
		this.paper = paper;
		PaperImage.GetComponent<Image> ().sprite = this.paper.paperImage;
		PaperText.GetComponent<Text> ().text = this.paper.paperTitle;
	}
}
