using UnityEngine;
using System.Collections;
using Artoncode.Core;
public class PlayerStatisticManager : Singleton<PlayerStatisticManager> {

	private Hashtable playerStatistic;

	public PlayerStatisticManager ()
	{
		Reset ();
	}
	
	public void Reset ()
	{
		Load ();
	}
	
	private void Load ()
	{
		playerStatistic = GameManager.shared ().GameStatisticData;
		if (playerStatistic == null) {
			playerStatistic = new Hashtable ();
		}
	}
	
	private void Save ()
	{
		GameManager.shared ().GameStatisticData = playerStatistic;
	}
	
	public override string ToString()
	{
		string str = "";
		return str;
	}
}
