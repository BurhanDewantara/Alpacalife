using UnityEngine;
using System.Collections;
using ScottGarland;

public class ShopNotificationHandler : MonoBehaviour {

	public GameObject notificationObject;

	void OnEnable(){
		CurrencyManager.shared().OnUpdated += OnCurrencyUpdate;
		OnCurrencyUpdate(0,0);
	}

	public void OnCurrencyUpdate(BigInteger prev, BigInteger delta)
	{
		notificationObject.SetActive(CheckAnyUpgradeable());
	}

	public bool CheckAnyUpgradeable()
	{
		EnvironmentSO envSo = UpgradeManager.shared ().GetNextEnvironmentUpgrade();
		LivestockSO lvsSo = UpgradeManager.shared ().GetNextLivestockUpgrade ();

		BigInteger envSoPrice = UpgradeManager.shared ().GetEnvironmentUpgradePrice (envSo);
		BigInteger lvsSoPrice = UpgradeManager.shared ().GetLivestockUpgradePrice (lvsSo);

		return CurrencyManager.shared ().IsAfforadble (envSoPrice) || CurrencyManager.shared ().IsAfforadble (lvsSoPrice);
	}




}
