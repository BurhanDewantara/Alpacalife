using UnityEngine;
using System.Collections;
using Artoncode.Core;

public class CurrencyManager : Singleton<CurrencyManager> {

	private Currency playerCurrency;
	public Currency PlayerCurrency {
		get{ return playerCurrency;}
	}
	
	public CurrencyManager ()
	{
		Reset ();
	}
	
	public void Reset ()
	{
		Load ();
	}
	
	private void Load ()
	{
		playerCurrency = GameManager.shared ().GameCurrency;
		if (playerCurrency == null) {
			playerCurrency = new Currency ();
		}
	}
	
	private void Save ()
	{
		GameManager.shared ().GameCurrency = playerCurrency;
	}
	
	public override string ToString ()
	{
		string str = "";
		str += playerCurrency.ToString () + "\n";
		return str;
	}

	#region Add / Pay
	
	public void AddMoney (Currency pCurrency, bool autoSave = false)
	{
		playerCurrency += pCurrency;
		if(autoSave)
			this.Save ();
	}
	
	public void AddMoney (int coin = 0, int gem = 0)
	{
		this.AddMoney (new Currency (coin, gem));
	}
	
	public bool PayMoney (Currency pCurrency)
	{
		if (this.IsAffordable (pCurrency)) {
			playerCurrency -= pCurrency;

			this.Save ();
			return true;
		}
		return false;
	}
	
	public bool PayMoney (int coin = 0, int gem = 0)
	{
		return this.PayMoney (new Currency (coin, gem));
	}
	

	public bool IsAffordable (Currency Price)
	{
		return (playerCurrency >= Price);
	}
	public bool IsAffordable (int coin = 0, int gem = 0)
	{
		return IsAffordable(new Currency (coin, gem));
	}

	
	
	#endregion 
}
