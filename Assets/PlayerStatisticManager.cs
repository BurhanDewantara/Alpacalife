using UnityEngine;
using System.Collections;
using Artoncode.Core;
using ScottGarland;


public class PlayerStatisticManager : Singleton<PlayerStatisticManager>
{
	BigInteger totalGold;
	BigInteger totalJump;
	BigInteger totalBitten;

	BigInteger totalGoldSpent;
	BigInteger totalGoldEarn1Game;
	BigInteger totalGameTime;
	BigInteger totalPlayTime;


	public BigInteger TotalPlayTime
	{
		get{
			return totalPlayTime ;
		}
		set{
			totalPlayTime = value;
			Save();
		}
	}

	public BigInteger TotalGoldSpent {
		get{
			return totalGoldSpent ;
		}
		set{
			totalGoldSpent = value;
			Save();
		}
	}


	public BigInteger TotalGoldEarn1Game{
		get{
			return totalGoldEarn1Game;
		}
		set{
			if(value > totalGoldEarn1Game){
				totalGoldEarn1Game = value;
				Save();
			}
		}
	}
		
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

	public void Reset()
	{
		Load();
	}

	void Load()
	{
		totalGold = GameDataManager.shared().PlayerTotalGold;
		totalJump = GameDataManager.shared().PlayerTotalJump;
		totalBitten = GameDataManager.shared().PlayerTotalBitten;
		totalGoldEarn1Game = GameDataManager.shared().PlayerTotalGoldEarn1Game;
		totalPlayTime = GameDataManager.shared().PlayerTotalGameTime;
		totalGoldSpent = GameDataManager.shared().PlayerTotalGoldSpent;
		if(totalGoldSpent == 0 )
		{
			totalGoldSpent = UpgradeManager.shared().CalculateTotalSpent();
		}


	}

	void Save()
	{
		GameDataManager.shared().PlayerTotalGold = totalGold;
		GameDataManager.shared().PlayerTotalJump = totalJump;
		GameDataManager.shared().PlayerTotalBitten = totalBitten;
		GameDataManager.shared().PlayerTotalGoldEarn1Game = totalGoldEarn1Game;
		GameDataManager.shared().PlayerTotalGameTime = totalPlayTime;
		GameDataManager.shared().PlayerTotalGoldSpent = totalGoldSpent;

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
