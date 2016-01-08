using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Artoncode.Core.Utility {

	public class Digest  {
		public static string computeHMACSHA1(byte[] data, string key) {
			HMACSHA1 hmacDigest = new HMACSHA1 (Encoding.UTF8.GetBytes (key));
			byte[] hmac = hmacDigest.ComputeHash (data);
			return BitConverter.ToString (hmac).Replace ("-", "");
		}
	}

}