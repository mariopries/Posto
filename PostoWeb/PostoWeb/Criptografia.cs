namespace PostoWeb
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Criptografia
    {

        public static string Encrypt(string text, string secretKey)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);

            var symmetricKey = new RijndaelManaged()
            {
                Key = Encoding.UTF8.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126
            };
            var encryptor = symmetricKey.CreateEncryptor();

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText, string secretKey)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            var symmetricKey = new RijndaelManaged()
            {
                Key = Encoding.UTF8.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.ISO10126
            };

            var decryptor = symmetricKey.CreateDecryptor();
            int decryptedByteCount = 0;
            byte[] plainTextBytes;

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    plainTextBytes = new byte[cipherTextBytes.Length];
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                }
            }
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    }
}




    
