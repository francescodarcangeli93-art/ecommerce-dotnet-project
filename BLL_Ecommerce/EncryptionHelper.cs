using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BLL_Ecommerce
{
    public class EncryptionHelper
    {
        private const int KeySize = 32;
        private const int IvSize = 16;
        private const int Iterations = 100000;
        private const string DefaultEncryptionKey = "CertificaKey_091276";

        private static string EncryptionPassword
        {
            get
            {
                string value = DefaultEncryptionKey;

                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("EncryptionKey non trovata");

                return value;
            }
        }

        private static readonly byte[] FixedSalt = Encoding.UTF8.GetBytes("1234567890ABCDEF");

        private static readonly byte[] FixedIV = Encoding.UTF8.GetBytes("ABCDEF1234567890");

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = DeriveKey(DefaultEncryptionKey, FixedSalt);
                aes.IV = FixedIV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return encryptedText;

            byte[] cipherBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = DeriveKey(DefaultEncryptionKey, FixedSalt);
                aes.IV = FixedIV;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (MemoryStream ms = new MemoryStream(cipherBytes))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            using (var pdb = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                return pdb.GetBytes(KeySize);
            }
        }
    }
}
