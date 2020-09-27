using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace TalkaTIPSerwer
{
    class Security
    {
        ECDiffieHellmanCng owner = new ECDiffieHellmanCng();//Initializes a new instance of the ECDiffieHellmanCng class with a random key pair.
        byte[] iv = { 126, 122, 93, 86, 153, 51, 216, 230, 93, 82, 240, 192, 201, 239, 119, 120 };

        public ECDiffieHellmanPublicKey GetOwnerPublicKey()
        {

            return owner.PublicKey;
        }
        public void CreatePublicKey()
        {
            owner.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash; //Gets or sets the key derivation function for the ECDiffieHellmanCng class.
            owner.HashAlgorithm = CngAlgorithm.Sha256;
        }
        public byte[] SetSessionKey(byte[] clientPublicKey) //set session key
        {
            //CngKey k = CngKey.Import(clientPublicKey, CngKeyBlobFormat.EccPublicBlob); //firs argument public key from client
            byte[] sessionKey = owner.DeriveKeyMaterial(CngKey.Import(clientPublicKey, CngKeyBlobFormat.EccPublicBlob));//firs argument public key from client           
            //byte[] sessionKey = owner.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKey, CngKeyBlobFormat.EccPublicBlob));
            return sessionKey;
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
