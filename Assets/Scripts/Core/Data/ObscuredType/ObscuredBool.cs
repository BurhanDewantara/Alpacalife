using System;
using System.Collections;

namespace Artoncode.Core.Data {
	
	[Serializable]
	public class ObscuredBool {
		private bool val;
		
		private bool key;
		
		public bool Value {
			set {
				val = value ^ key;
			}
			get {
				return val ^ key;
			}
		}
		
		public ObscuredBool() {
			key = UnityEngine.Random.value > 0.5f;
		}
		
		public static implicit operator bool(ObscuredBool obsBool)
		{
			return obsBool.Value;
		}
		
		public static implicit operator ObscuredBool(bool i)
		{
			ObscuredBool obsBool = new ObscuredBool ();
			obsBool.Value = i;
			return obsBool;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}", Value);
		}
	}
	
}
