using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Jabberwocky.Autofac.Attributes;

//Taken and modified from:
//http://stackoverflow.com/a/2791259/3490246

namespace Informa.Library.Utilities.Security
{
    public interface ICrypto
    {
        string EncryptStringAes(string plainText, string sharedSecret);
        string DecryptStringAes(string cipherText, string sharedSecret);
    }

    [AutowireService]
    public class Crypto : ICrypto
    {
        private readonly byte[] _salt = Encoding.ASCII.GetBytes("dxk39f6glkw8xotl");

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public string EncryptStringAes(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            using (var algorithm = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                algorithm.Key = key.GetBytes(algorithm.KeySize/8);

                ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(BitConverter.GetBytes(algorithm.IV.Length), 0, sizeof(int));
                    memoryStream.Write(algorithm.IV, 0, algorithm.IV.Length);

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

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public string DecryptStringAes(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException(nameof(sharedSecret));

            using (var algorithm = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                var bytes = Convert.FromBase64String(cipherText);
                using (var memoryStream = new MemoryStream(bytes))
                {
                    algorithm.Key = key.GetBytes(algorithm.KeySize/8);
                    algorithm.IV = ReadByteArray(memoryStream);

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

        private static byte[] ReadByteArray(Stream stream)
        {
            var rawLength = new byte[sizeof(int)];
            if (stream.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}