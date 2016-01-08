using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Artoncode.Core.Utility {
	
	public class AESEncryption {
		
		public static byte[] encrypt(byte[] plainData, string password, string salt, int keySize = 256) {
			Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes (password, Encoding.UTF8.GetBytes (salt));

			RijndaelManaged symmetricKey = new RijndaelManaged ();
			symmetricKey.Mode = CipherMode.CBC;
			symmetricKey.Key = derivedKey.GetBytes (keySize / 8);
			symmetricKey.GenerateIV ();
			
			byte[] cipherData;
			
			using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(symmetricKey.Key, symmetricKey.IV)) {
				using (MemoryStream ms = new MemoryStream()){
					using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
						cs.Write (plainData, 0, plainData.Length);
						cs.FlushFinalBlock();
						cipherData = ms.ToArray();
						ms.Close();
						cs.Close();
					}
				}
			}
			return cipherData;
		}
		
		public static byte[] decrypt(byte[] cipherData, string password, string salt, int keySize = 256) {
			Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes (password, Encoding.UTF8.GetBytes (salt));

			RijndaelManaged symmetricKey = new RijndaelManaged ();
			symmetricKey.Mode = CipherMode.CBC;
			symmetricKey.Key = derivedKey.GetBytes (keySize / 8);
			
			byte[] plainData = new byte[cipherData.Length];
			using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(symmetricKey.Key, symmetricKey.IV)) {
				using (MemoryStream ms = new MemoryStream(cipherData)){
					using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
						cs.Read(plainData, 0, plainData.Length);
						ms.Close();
						cs.Close();
					}
				}
			}

			return plainData;
		}
	}

}