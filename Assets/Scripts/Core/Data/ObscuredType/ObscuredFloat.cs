using System;
using System.Collections;

namespace Artoncode.Core.Data {
	
	[Serializable]
	public class ObscuredFloat {
		private int wholePart;
		private float fraction;

		private int key;
		
		public float Value {
			set {
				wholePart = (int)value;
				fraction = value - wholePart;
				wholePart = wholePart ^ key;
			}
			get {
				return (wholePart ^ key) + fraction;
			}
		}
		
		public ObscuredFloat() {
			key = UnityEngine.Random.Range (int.MinValue, int.MaxValue);
		}
		
		public static implicit operator float(ObscuredFloat obsFloat)
		{
			return obsFloat.Value;
		}
		
		public static implicit operator ObscuredFloat(float i)
		{
			ObscuredFloat obsFloat = new ObscuredFloat ();
			obsFloat.Value = i;
			return obsFloat;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}", Value);
		}
	}
	
}
