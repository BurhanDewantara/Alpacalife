using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannedFoodContent : MonoBehaviour {

	public GameObject canImage;
	public GameObject canLight;
	public CannedFoodItem can;

	public void SetItem(CannedFoodItem can)
	{
		this.can = can;
		if (can.isPowerUp) {
			canLight.SetActive(true);
			this.GetComponent<Button>().enabled = true;
		}
		canImage.GetComponent<Image> ().sprite = can.canSprite;
	}

	public void SetButtonEnable(bool val)
	{
		this.GetComponent<Button>().enabled = val;
	}
}
