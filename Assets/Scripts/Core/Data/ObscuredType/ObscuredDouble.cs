using System;
using System.Collections;

namespace Artoncode.Core.Data {
	
	[Serializable]
	public class ObscuredDouble {
		private int wholePart;
		private double fraction;
		
		private int key;
		
		public double Value {
			set {
				wholePart = (int)value;
				fraction = value - wholePart;
				wholePart = wholePart ^ key;
			}
			get {
				return (wholePart ^ key) + fraction;
			}
		}
		
		public ObscuredDouble() {
			key = UnityEngine.Random.Range (int.MinValue, int.MaxValue);
		}
		
		public static implicit operator double(ObscuredDouble obsDouble)
		{
			return obsDouble.Value;
		}
		
		public static implicit operator ObscuredDouble(double i)
		{
			ObscuredDouble obsDouble = new ObscuredDouble ();
			obsDouble.Value = i;
			return obsDouble;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}", Value);
		}
	}
	
}
