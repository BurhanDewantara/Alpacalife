using UnityEngine;
using System.Collections;

namespace Artoncode.Core
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		
		public static T shared ()
		{
			if (_instance == null) {
				T[] objs = (T[])FindObjectsOfType (typeof(T));
				if (objs.Length > 0) {
					_instance = objs [0];
				} else {
					if (_instance == null)
					{
//						_instance = (new GameObject(typeof(T).ToString())).AddComponent<T>();
					}
					return _instance;
				}
			}
			return _instance;
		}

	}
	
}