using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace TalkaTIPClientV2
{
    class Security
    {
        // Initializes a new instance of the ECDiffieHellmanCng class with a random key pair
        ECDiffieHellmanCng owner = new ECDiffieHellmanCng();
        byte[] iv = { 126, 122, 93, 86, 153, 51, 216, 230, 93, 82, 240, 192, 201, 239, 119, 120 };

        public ECDiffieHellmanPublicKey GetOwnerPublicKey()
        {
            return owner.PublicKey;
        }

        public void CreatePublicKey()
        {
            // Gets or sets the key derivation function for the ECDiffieHellmanCng class
            owner.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            owner.HashAlgorithm = CngAlgorithm.Sha256;
        }

        // Set session key
        public byte[] SetSessionKey(byte[] clientPublicKey) 
        {
            // The first argument is public key from the client
            byte[] SessionKey = owner.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKey, CngKeyBlobFormat.EccPublicBlob));           

            return SessionKey;
        }

        public byte[] EncryptMessage(byte[] key, string secretMessage)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;

                // Encrypt the message
                using (MemoryStream ciphertext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextMessage = Encoding.UTF8.GetBytes(secretMessage);
                    cs.Write(plaintextMessage, 0, plaintextMessage.Length);
                    cs.Close();
                    byte[] encryptedMessage = ciphertext.ToArray();
                    return encryptedMessage;
                }
            }
        }

        public string DecryptMessage(byte[] encryptedMessage, byte[] sessionKey)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = sessionKey;
                aes.IV = iv;
                // Decrypt the message
                using (MemoryStream plaintext = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedMessage, 0, encryptedMessage.Length);
                        cs.Close();
                        string message = Encoding.UTF8.GetString(plaintext.ToArray());
                        return message;
                    }
                }
            }
        }
    }
}
