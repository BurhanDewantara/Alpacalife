using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ShopController : MonoBehaviour {

	public GameObject environmentBuyButton;
	public GameObject livestockBuyButton;
	public GameObject displayObject;

	private GameObject currentSelectedObject = null;

	void OnEnable()
	{
		RefreshEnvironmentButton();
		RefreshLivestockButton();
		RefreshDisplay();
	}

	public void RefreshDisplay(Sprite sprite = null)
	{
		displayObject.GetComponent<Image>().sprite = sprite;
		displayObject.GetComponent<Image>().color = sprite == null ? Color.clear : Color.white;
		displayObject.GetComponent<Image> ().SetNativeSize ();
	}

	public void RefreshEnvironmentButton()
	{
		EnvironmentSO item = UpgradeManager.shared().GetNextEnvironmentUpgrade();
		Sprite sprite = null;
		if(item!=null) sprite = item.sprite;

		ChangeButtonDetail(environmentBuyButton.transform,
			sprite,
			UpgradeManager.shared().GetEnvironmentUpgradePrice(item).ToStringShort(),
			UpgradeManager.shared().GetEnvironmentProgress()
		);
	}

	public void RefreshLivestockButton()
	{
		LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
		Sprite sprite = null;

		if(item!=null) sprite = item.sprite;
			ChangeButtonDetail(livestockBuyButton.transform,
			sprite,
			UpgradeManager.shared().GetLivestockUpgradePrice(item).ToStringShort(),
			UpgradeManager.shared().GetLivestockProgress()
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
			currentSelectedObject = null;
			UpgradeManager.shared().UpgradeEnvironment();
			WorldManager.shared().RefreshEnvironment();
			RefreshEnvironmentButton ();
			ShowEnvironment();
		}
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
			currentSelectedObject = null;
			LivestockSO item = UpgradeManager.shared().GetNextLivestockUpgrade();
			UpgradeManager.shared().UpgradeLivestock();

			Vector3 pos = Camera.main.ScreenToWorldPoint (displayObject.GetComponent<RectTransform> ().position);
			//remove Z position
			pos = new Vector3 (pos.x, pos.y, 0);

			WorldManager.shared ().AddLivestock (item,Vector3.zero, true );
			RefreshLivestockButton ();
			ShowLivestock();
		}
	}



	public void ChangeButtonDetail(Transform trans, Sprite sprite,string price,string availableText)
	{
		trans.FindChild("Image").GetComponent<Image>().sprite = sprite;
		trans.FindChild("Image").GetComponent<Image>().color = sprite == null ? Color.clear : Color.white;

		trans.FindChild("Currency").GetComponentInChildren<TextMeshProUGUI>().text = price;
		trans.FindChild("AvailableText").GetComponent<TextMeshProUGUI>().text = availableText;

	}



}
