using UnityEngine;
using UnityEngine;
using System.Collections.Generic;
using Artoncode.Core;
using Analytics;



public class FlurryManager : Singleton<FlurryManager>
{
	private string _iosApiKey = "";
	private string _androidApiKey = "TD3MM8GJNP78YHF2K2FN";

	public void Activate()
	{
		IAnalytics service = Flurry.Instance;

		service.SetLogLevel(LogLevel.All);
		service.StartSession(_iosApiKey, _androidApiKey);
	}


	
}
