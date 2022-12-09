using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Evon
{
	// Token: 0x02000008 RID: 8
	public class Encryption
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002828 File Offset: 0x00000A28
		public static byte[] EncryptBytes(byte[] inputBytes)
		{
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay")), Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay"))), "SHA-256", 2);
			byte[] result;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC
			})
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)), CryptoStreamMode.Write))
					{
						cryptoStream.Write(inputBytes, 0, inputBytes.Length);
						cryptoStream.FlushFinalBlock();
						result = memoryStream.ToArray();
					}
				}
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002914 File Offset: 0x00000B14
		public static byte[] DecryptBytes(byte[] inputBytes)
		{
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay")), Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay"))), "SHA-256", 2);
			byte[] result;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC
			})
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)), CryptoStreamMode.Write))
					{
						cryptoStream.Write(inputBytes, 0, inputBytes.Length);
						cryptoStream.FlushFinalBlock();
						result = memoryStream.ToArray();
					}
				}
			}
			return result;
		}
	}
}
