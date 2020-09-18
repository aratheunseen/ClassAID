using System;
using System.Security.Cryptography;
using System.Text;

namespace ClassAid.Models.Engines
{
    internal class Cryptography
    {
        public static string EncryptString(string text, string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            using (MD5CryptoServiceProvider mD5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = mD5.ComputeHash(Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripleDES =
                    new TripleDESCryptoServiceProvider()
                    {
                        Key = keys,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    ICryptoTransform cryptoTransform = tripleDES.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        public static string DecryptString(string text, string key)
        {
            byte[] data = Convert.FromBase64String(text);
            using (MD5CryptoServiceProvider mD5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = mD5.ComputeHash(Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripleDES =
                    new TripleDESCryptoServiceProvider()
                    {
                        Key = keys,
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    })
                {
                    ICryptoTransform cryptoTransform = tripleDES.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

    }
}
