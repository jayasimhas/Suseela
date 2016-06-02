using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Jabberwocky.Autofac.Attributes;

//Taken and heavily modified from:
//http://stackoverflow.com/a/2791259/3490246

namespace Informa.Library.Utilities.Security
{
    public interface ICrypto
    {
        string EncryptStringsAes(string[] pieces, string sharedSecret);
        string EncryptStringAes(string plainText, string sharedSecret);
        string DecryptStringAes(string cipherText, string sharedSecret);
    }

    [AutowireService]
    public class Crypto : ICrypto
    {
        private byte[] Salt { get; } = HexStringToByteArray("c04eeb397a32992d98e774a2332b98c6");
        private static byte[] HexStringToByteArray(string hex) => Enumerable.Range(0, hex.Length/2)
            .Select(i => Convert.ToByte(hex.Substring(i*2, 2), 16))
            .ToArray();


        public string EncryptStringsAes(string[] pieces, string sharedSecret)
        {
            var joined = string.Join("|", pieces);
            return EncryptStringAes(joined, sharedSecret);
        }
        public string EncryptStringAes(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            using (var algorithm = new RijndaelManaged())
            {
                algorithm.Key = new Rfc2898DeriveBytes(sharedSecret, Salt)
                    .GetBytes(algorithm.KeySize / 8);
                algorithm.IV = Salt;

                ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }


        public string DecryptStringAes(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            using (var algorithm = new RijndaelManaged())
            {
                algorithm.Key = new Rfc2898DeriveBytes(sharedSecret, Salt)
                    .GetBytes(algorithm.KeySize/8);
                algorithm.IV = Salt;

                var cipherBytes = Convert.FromBase64String(cipherText);

                using (var memoryStream = new MemoryStream(cipherBytes))
                {
                    ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}