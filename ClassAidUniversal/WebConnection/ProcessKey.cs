using System;
using System.Security.Cryptography;
using System.Text;

namespace ClassAidUniversal.WebConnection
{
    internal class ProcessKey
    {
        public static string GetKey(string username, string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(username);
            using (MD5CryptoServiceProvider mD5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = mD5.ComputeHash(Encoding.UTF8.GetBytes(password));
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
                    string key = Convert.ToBase64String(result, 0, result.Length);
                    return key;
                }
            }
        }
    }
}
