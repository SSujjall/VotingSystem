using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using VotingSystem.Common.Extensions.Config;
using VotingSystem.Common.Extensions.ExtensionHelpers;

namespace VotingSystem.Common.Extensions
{
    public static class StringExtension
    {
        public static string Encrypt(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            var options = ServiceLocator.Instance?.GetService<IOptions<EncryptionConfig>>();
            var config = options?.Value ?? throw new Exception("EncryptionConfig not found");

            var keyBytes = Convert.FromBase64String(config.Key);
            var ivBytes = Convert.FromBase64String(config.IV);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);

            sw.Write(plainText);
            sw.Close();

            var encrypted = ms.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(this string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return encryptedText;

            var options = ServiceLocator.Instance?.GetService<IOptions<EncryptionConfig>>();
            var config = options?.Value ?? throw new Exception("EncryptionConfig not found");

            var keyBytes = Convert.FromBase64String(config.Key);
            var ivBytes = Convert.FromBase64String(config.IV);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var cipherBytes = Convert.FromBase64String(encryptedText);

            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
