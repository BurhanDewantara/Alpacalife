using System;

namespace Artoncode.Core {
	public class Singleton<T> where T : new() {
		private static T instance = default(T);

		public static T shared () {
			if (instance == null) {
				instance = new T ();
			}
			return instance;
		}
	};
}

