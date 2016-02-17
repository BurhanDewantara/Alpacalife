using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using ScottGarland;

public class ShopController : MonoBehaviour {

	public GameObject environmentBuyButton;
	public GameObject livestockBuyButton;
	public GameObject displayObject;

	public GameObject buyPanel;

	public GameObject mainCanvas;
	public GameObject popUpPrefab;

	private GameObject currentSelectedObject = null;


	//0 envi
	//1 livestock
	private int buyState = 0;

	List<string> noMoney = 
		new List<string>()
	{
		"Insufficient funds",
		"You don't have enough gold",
		"mbeee, not nuff gold",
		"yo! you aint no money",
		"Doh!",
		"Sir, look at your money now",
		"Not enough gold",
	};

	void OnEnable()
	{
		RefreshEnvironmentButton();
		RefreshLivestockButton();
		RefreshDisplay();
	}

	public void RefreshDisplay(Sprite sprite = null)
	{
		displayObject.GetComponent<Image>().sprite = sprite;
		displayObject.GetComponent<Image>().color = sprite == null ? Color.clear : Color.black;
	}

	#region environment
	public void RefreshEnvironmentButton()
	{
		EnvironmentSO item = UpgradeManager.shared().GetNextEnvironmentUpgrade();
		Sprite sprite = null;
		if(item!=null) sprite = item.sprite;


		ChangeButtonDetail(environmentBuyButton.transform,
			sprite,
			UpgradeManager.shared().GetEnvironmentUpgradePrice(item).ToStringShort(),
			UpgradeManager.shared().GetEnvironmentProgress(),
			UpgradeManager.shared().IsEnvironmmentMaxedOut()
		);

	}


	public void ShowEnvironment()
	{
		if(currentSelectedObject != environmentBuyButton)
		{
			EnvironmentSO item = UpgradeManager.shared().GetNextEnvironmentUpgrade();
			if(item !=null )
				RefreshDisplay(item.sprite);
			currentSelectedObject = environmentBuyButton;
		}
		else
		{
			EnvironmentSO item = UpgradeManager.shared().GetNextEnvironmentUpgrade();
			if(item !=null )
			{
				BigInteger price = UpgradeManager.shared ().GetEnvironmentUpgradePrice (item);
				if (CurrencyManager.shared ().IsAfforadble (price)) {
					CurrencyManager.shared ().PayGold (price);
					buyState = 0;
					string detail = "multiplier x" + UpgradeManager.shared ().GetEnvironmentMultiplyValue (item).ToStringShort ();
					currentSelectedObject.GetComponent<AudioSource>().Play();
					StartCoroutine (DisplayBoughtAnimation (item.sprite, item.name, detail));
				} else {
					InsufficientFundPopUp (environmentBuyButton);
				}
			}
		}
	}
	public void EnvironmentBought()
	{
		UpgradeManager.shared().UpgradeEnvironment();
		WorldManager.shared().RefreshEnvironment();
		currentSelectedObject = null;
		RefreshEnvironmentButton ();
		RefreshDisplay();
	}
	#endregion

	#region livestock
	public void RefreshLivestockButton()
	{
		LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
		Sprite sprite = null;

		if(item!=null) sprite = item.sprite;

		ChangeButtonDetail(livestockBuyButton.transform,
			sprite,
			UpgradeManager.shared().GetLivestockUpgradePrice(item).ToStringShort(),
			UpgradeManager.shared().GetLivestockProgress(),
			UpgradeManager.shared().IsLivestockMaxedOut()

		);
	}

	public void ShowLivestock()
	{
		if(currentSelectedObject != livestockBuyButton)
		{
			LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
			if(item !=null )
				RefreshDisplay(item.sprite);
			currentSelectedObject = livestockBuyButton;
		}
		else
		{
			LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
			if(item !=null )
			{
				BigInteger price = UpgradeManager.shared ().GetLivestockUpgradePrice (item);

				//HACK
				if (CurrencyManager.shared ().IsAfforadble (price) || true) {
//					CurrencyManager.shared ().PayGold (price);

					buyState = 1;
					string detail = "Value : " + UpgradeManager.shared().GetLivestockSlideValue(item).ToStringShort();
					currentSelectedObject.GetComponent<AudioSource>().Play();
					StartCoroutine(DisplayBoughtAnimation(item.sprite,item.name,detail ));
				}
				else
				{
					InsufficientFundPopUp (livestockBuyButton);

				}
			}
				
			
		}
	}

	public void LivestockBought()
	{
		LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
		if(item!= null)
		{

			UpgradeManager.shared().UpgradeLivestock();

			Vector3 pos = Camera.main.ScreenToWorldPoint (displayObject.GetComponent<RectTransform> ().position);
			//remove Z position
			pos = new Vector3 (pos.x, pos.y, 0);
			WorldManager.shared ().AddLivestock (item,pos, true );
			currentSelectedObject = null;

			RefreshLivestockButton ();
			RefreshDisplay();


			//ShowLivestock();
		}
	}
	#endregion


	IEnumerator DisplayBoughtAnimation(Sprite img, string name, string detail)
	{
		float delay = 0.5f;

		displayObject.SetActive(false);
		buyPanel.SetActive(true);
		buyPanel.GetComponent<Button>().interactable = false;

		GameObject light 	= buyPanel.transform.FindChild("Shine").gameObject;
		GameObject display 	= buyPanel.transform.FindChild("Image").gameObject;
		GameObject dName 	= buyPanel.transform.FindChild("NameText").gameObject;
		GameObject dDetail	= buyPanel.transform.FindChild("DetailText").gameObject;

		light.transform.localScale = Vector3.zero;
		display.GetComponent<Image>().sprite = img;
		display.GetComponent<Image>().color= Color.black;
		dName.GetComponent<TextMeshProUGUI>().text = "";
		dDetail.GetComponent<TextMeshProUGUI>().text = "";



		iTween.RotateTo(light,
			iTween.Hash(
				"z",720,
				"time",5.0f,
				"looptype",iTween.LoopType.loop,
				"easetype",iTween.EaseType.linear
			));

		iTween.ScaleTo(light,
			iTween.Hash(
				"scale",Vector3.one,
				"time",1.0f,
				"easetype",iTween.EaseType.easeOutQuint
			));



		yield return new WaitForSeconds(delay);

//		iTween.ColorTo (display, Color.white, delay);
		display.GetComponent<Image>().color= Color.white;

		yield return new WaitForSeconds(delay);
		dName.GetComponent<TextMeshProUGUI>().text = name;

		yield return new WaitForSeconds(delay);
		dDetail.GetComponent<TextMeshProUGUI>().text = detail;

		buyPanel.GetComponent<Button>().interactable = true;
		yield return new WaitForEndOfFrame();


	}

	public void CloseBuyPanel()
	{
		buyPanel.SetActive(false);
		displayObject.SetActive(true);
		if(buyState == 1)
			LivestockBought();
		else if (buyState == 0)
			EnvironmentBought();
			
	}


	public void ChangeButtonDetail(Transform trans, Sprite sprite, string price, string availableText, bool isMaxedOut)
	{
		trans.FindChild("SoldImage").gameObject.SetActive(isMaxedOut);
		trans.FindChild("Image").gameObject.SetActive(!isMaxedOut);
		trans.FindChild("Currency").gameObject.SetActive(!isMaxedOut);


		trans.FindChild("Image").GetComponent<Image>().sprite = sprite;
		trans.FindChild("Image").GetComponent<Image>().color = sprite == null ? Color.clear : Color.black;

		trans.FindChild("Currency").GetComponentInChildren<TextMeshProUGUI>().text = price;
		trans.FindChild("AvailableText").GetComponent<TextMeshProUGUI>().text = availableText;
	}


	private void InsufficientFundPopUp(GameObject targetLoc)
	{
		string popText = noMoney.Random ();
		GameObject obj = Instantiate (popUpPrefab, targetLoc.transform.position, Quaternion.identity) as GameObject;
		obj.GetComponent<PopAndFade> ().SetText (popText);
		obj.GetComponent<PopAndFade> ().PopUp ();
		obj.transform.SetParent (mainCanvas.transform, true);

	}

}
