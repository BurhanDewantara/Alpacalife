using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Artoncode.Core.Utility;

namespace Artoncode.Core.Data {
	public class DataManager {
		private static DataManager manager;
		private string defaultPath;
		private string aesPassword;
		private string aesSalt;
		private string aesIV;
		private Hashtable data;
		
		private DataManager () {
			#if UNITY_IOS
			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
			#endif

			reset ();
			defaultPath = "userData.dat";
			aesPassword = "zXyIGVM6yt3izxR35WLFYLR917PAmgZ2HsJ42OalchxqW50SH86EhjIpHrE5jiY";
			aesSalt = "puHm0exu59nc7VXXuQ9VUMRvWx9RtvNRGho9TJgknbhgdBHYQZWmCwkcc9sFm8f";
		}
		
		public static DataManager defaultManager {
			get {
				if (manager == null) {
					manager = new DataManager ();
				}
				return manager;
			}
		}

		public DataManager create () {
			return new DataManager ();	
		}

		public void setDefaultPath (string path) {
			defaultPath = path;
		}

		public void setEncryptionSetting (string password, string salt) {
			aesPassword = password;
			aesSalt = salt;
		}

		public void reset () {
			data = new Hashtable ();
		}

		#region Accessor 
		
		public void setInt (string key, ObscuredInt value) {
			data [key] = value;
		}
		
		public ObscuredInt getInt (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			object value = data [key];
			if (value is ObscuredInt)
				return (ObscuredInt)value;
			return null;
		}

		public void setFloat (string key, ObscuredFloat value) {
			data [key] = value;
		}
		
		public ObscuredFloat getFloat (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			object value = data [key];
			if (value is ObscuredFloat)
				return (ObscuredFloat)value;
			return null;
		}
		
		public void setDouble (string key, ObscuredDouble value) {
			data [key] = value;
		}
		
		public ObscuredDouble getDouble (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			object value = data [key];
			if (value is ObscuredDouble)
				return (ObscuredDouble)value;
			return null;
		}
		
		public void setBool (string key, ObscuredBool value) {
			data [key] = value;
		}
		
		public ObscuredBool getBool (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			object value = data [key];
			if (value is ObscuredBool)
				return (ObscuredBool)value;
			return null;
		}
		
		public void setString (string key, string value) {
			data [key] = value;
		}
		
		public string getString (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			return data [key].ToString ();
		}

		public void setObject (string key, object value) {
			data [key] = value;	
		}

		public object getObject (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			return data [key];
		}

		public Type getObjectType (string key) {
			if (!data.ContainsKey (key)) {
				return null;
			}
			return data [key].GetType ();
		}

		public void deleteKey (string key) {
			data.Remove (key);
		}

		#endregion

		public void save (string relativePath=null) {
			if (relativePath == null) {
				relativePath = defaultPath;			
			}

			// Serializing data
			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream ms = new MemoryStream ();
			bf.Serialize (ms, data);

			// Encrypt serialized data
			byte[] cipherData = AESEncryption.encrypt (ms.ToArray (), aesPassword, aesSalt);

			// Write encrypted data
			FileStream fs = File.Create (Application.persistentDataPath + "/" + relativePath);
			fs.Write (cipherData, 0, cipherData.Length);
			fs.Close ();

			// Compute hmac to lock data on specific device
			string hmac = Digest.computeHMACSHA1 (cipherData, SystemInfo.deviceUniqueIdentifier);
			PlayerPrefs.SetString ("hash-" + relativePath, hmac);
		}
		
		public void load (string relativePath=null) {
			if (relativePath == null) {
				relativePath = defaultPath;			
			}
			
			// Read encrypted data
			string path = Application.persistentDataPath + "/" + relativePath;
			FileStream fs = File.Open (path, FileMode.OpenOrCreate);
			byte[] cipherData = new byte[fs.Length];
			fs.Read (cipherData, 0, (int)fs.Length);
			fs.Close ();

			// Verify that loaded data is from valid device
			string hmac = Digest.computeHMACSHA1 (cipherData, SystemInfo.deviceUniqueIdentifier);
			if (hmac == PlayerPrefs.GetString ("hash-" + relativePath)) {
				// Decrypt encrypted data
				byte[] plainData = AESEncryption.decrypt (cipherData, aesPassword, aesSalt);
				
				// Deserialized decrypted data
				try {
					BinaryFormatter bf = new BinaryFormatter ();
					MemoryStream ms = new MemoryStream (plainData);
					data = (Hashtable)bf.Deserialize (ms);
				} catch (SerializationException) {
					reset ();
				}
			}
			else {
				// Invalid device, erase the data
				reset ();
			}
		}

		public void debug () {
			foreach (DictionaryEntry kv in data) {
				Debug.Log (kv.Key + ": " + kv.Value);
			}
		}
	}

}