using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BundleVersion 
{
	
	#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string _GetCFBundleVersion();
	
	protected static string GetAppVersion_IOS() {
		return _GetCFBundleVersion();
		
	}
	#endif
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	
	static string GetAppVersion_Android ()
	{
		AndroidJavaClass    unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject   context     = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext");
		AndroidJavaObject   pManager    = context.Call<AndroidJavaObject>("getPackageManager");
		AndroidJavaObject   pInfo       = pManager.Call<AndroidJavaObject>( "getPackageInfo", context.Call<string>("getPackageName"), pManager.GetStatic<int>("GET_ACTIVITIES") );
		string versionName = pInfo.Get<string>( "versionName" );
		return versionName;
	}
	
	#endif //UNITY_ANDROID
	
	
	public static string GetVersion()
	{
		#if UNITY_IOS && !UNITY_EDITOR
		return GetAppVersion_IOS();
		#elif UNITY_ANDROID && !UNITY_EDITOR
		return GetAppVersion_Android();
		#elif UNITY_EDITOR
		return PlayerSettings.bundleVersion;
		#else
		return "";
		#endif
	}
}
