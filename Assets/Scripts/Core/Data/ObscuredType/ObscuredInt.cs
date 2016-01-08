using System;
using System.Collections;

namespace Artoncode.Core.Data {

	[Serializable]
	public class ObscuredInt {
		private int dec;

		private int key;

		public int Value {
			set {
				dec = value ^ key;
			}
			get {
				return dec ^ key;
			}
		}

		public ObscuredInt() {
			key = UnityEngine.Random.Range (int.MinValue, int.MaxValue);
		}

		public static implicit operator int(ObscuredInt obsInt)
		{
			return obsInt.Value;
		}

		public static implicit operator ObscuredInt(int i)
		{
			ObscuredInt obsInt = new ObscuredInt ();
			obsInt.Value = i;
			return obsInt;
		}

		public override string ToString ()
		{
			return string.Format ("{0}", Value);
		}
	}

}
