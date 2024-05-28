using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuestSystem.Application.Common.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace QuestSystem.Infrastructure.Security;

public class SecureDataService : ISecureDataService
{
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;


    public SecureDataService(IConfiguration configuration)
    {
        _configuration = configuration;
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new ObjectiveJsonConverter() }
        };
    }
    
    
    private byte[] GetKeyFromEnvironmentVariable()
    {
        string? key = _configuration.GetValue<string>("AppSettings:SecureDataEncryptionKey");
        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException($"Configuration key 'AppSettings:SecureDataEncryptionKey' not found.");
        }
        return CreateValidKey(Encoding.UTF8.GetBytes(key));
    }

    
    private byte[] CreateValidKey(byte[] key)
    {
        int validKeyLength = 32; // 256 bits (32 bytes)
        if (key.Length >= validKeyLength)
        {
            Array.Resize(ref key, validKeyLength);
        }
        else
        {
            Array.Resize(ref key, validKeyLength);
            for (int i = key.Length; i < validKeyLength; i++)
            {
                key[i] = 0;
            }
        }
        return key;
    }

    public string Encrypt<T>(T plainObject)
    {
        string plainText = JsonConvert.SerializeObject(plainObject);
        byte[] key = GetKeyFromEnvironmentVariable();

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    byte[] iv = aesAlg.IV;
                    byte[] encryptedContent = msEncrypt.ToArray();
                    byte[] result = new byte[iv.Length + encryptedContent.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }
    }

    public T? Decrypt<T>(string cipherText)
    {
        byte[] key = GetKeyFromEnvironmentVariable();
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            byte[] iv = new byte[aesAlg.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipher))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string plainText = srDecrypt.ReadToEnd();
                        return JsonSerializer.Deserialize<T>(plainText, _jsonOptions);
                    }
                }
            }
        }
    }
}
