using UnityEngine;
using System.Collections;
using Artoncode.Core;
using ScottGarland;


public class PlayerStatisticManager : Singleton<PlayerStatisticManager>
{
	BigInteger totalGold;
	BigInteger totalJump;
	BigInteger totalBitten;

	public BigInteger TotalGold{
		get{
			return totalGold;
		}
		set{
			totalGold = value;
			Save();
		}
	}

	public BigInteger TotalJump{
		get{
			return totalJump;
		}
		set{
			totalJump = value;
			Save();
		}
	}

	public BigInteger TotalBitten{
		get{
			return totalBitten;
		}
		set{
			totalBitten = value;
			Save();
		}
	}


	public PlayerStatisticManager()
	{
		Load();
	}

	void Load()
	{
		totalGold = GameDataManager.shared().PlayerTotalGold;
		totalJump = GameDataManager.shared().PlayerTotalJump;
		totalBitten = GameDataManager.shared().PlayerTotalBitten;

		if(totalGold  == null)		totalGold = 0;
		if(totalJump  == null)		totalJump = 0;
		if(totalBitten  == null)	totalBitten = 0;

	}

	void Save()
	{
		GameDataManager.shared().PlayerTotalGold = totalGold;
		GameDataManager.shared().PlayerTotalJump = totalJump;
		GameDataManager.shared().PlayerTotalBitten = totalBitten;
	}
		
	public override string ToString ()
	{
		string str = "";
		str+= "Total gold : " + totalGold.ToString() + "\n";
		str+= "Total jump : " + totalJump.ToString() + "\n";
		str+= "Total bitten : " + totalBitten.ToString() + "\n";

		return str;

	}
}
