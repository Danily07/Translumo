using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Translumo.Utils
{
    public static class StringCipher
    {

        public static string Decrypt(string cipherText, string passPhrase)
        {
            string empty = string.Empty;
            byte[] array = Convert.FromBase64String(cipherText);
            byte[] salt = array.Take(32).ToArray();
            byte[] rgbIV = array.Skip(32).Take(32).ToArray();
            byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray();
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000);
            byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.BlockSize = 256;
            using ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
            using MemoryStream memoryStream = new MemoryStream(array2);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] array3 = new byte[array2.Length];
            int count = cryptoStream.Read(array3, 0, array3.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(array3, 0, count);
        }

    }
}
