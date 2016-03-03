using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace wizz.Class
{
    public class Encryption
    {
        public static string Encrypt(string input, string passphrase = null)
        {
            byte[] key, iv;

            // byte[] base64data = Convert.FromBase64String(input);
            byte[] passphrasedata = RawBytesFromString(passphrase);
            byte[] currentHash = new byte[0];
            SHA256Managed hash = new SHA256Managed();
            currentHash = hash.ComputeHash(passphrasedata);

            iv = new byte[16];
            // DeriveKeyAndIV(RawBytesFromString(passphrase), null, 1, out key, out iv);

            return Convert.ToBase64String(EncryptStringToBytes(input, currentHash, iv));
        }

        public static string Decrypt(string inputBase64, string passphrase = null)
        {
            byte[] key, iv = new byte[0];
            byte[] base64data = Convert.FromBase64String(inputBase64);
            byte[] passphrasedata = RawBytesFromString(passphrase);
            byte[] currentHash = new byte[0];
            SHA256Managed hash = new SHA256Managed();
            currentHash = hash.ComputeHash(passphrasedata);

            // DeriveKeyAndIV(RawBytesFromString(passphrase), null, 1, out key, out iv);

            return DecryptStringFromBytes(base64data, currentHash, null);
        }

        private static byte[] RawBytesFromString(string input)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(input);
            //var ret = new List<Byte>();

            //foreach (char x in input)
            //{
            //  var c = (byte)((ulong)x & 0xFF);
            //  ret.Add(c);
            //}

            //return  ret.ToArray();
        }

        private static void DeriveKeyAndIV(byte[] data, byte[] salt, int count, out byte[] key, out byte[] iv)
        {
            List<byte> hashList = new List<byte>();
            byte[] currentHash = new byte[0];

            int preHashLength = data.Length + ((salt != null) ? salt.Length : 0);
            byte[] preHash = new byte[preHashLength];

            System.Buffer.BlockCopy(data, 0, preHash, 0, data.Length);
            if (salt != null)
                System.Buffer.BlockCopy(salt, 0, preHash, data.Length, salt.Length);

            SHA256Managed hash = new SHA256Managed();
            currentHash = hash.ComputeHash(preHash);

            for (int i = 1; i < count; i++)
            {
                currentHash = hash.ComputeHash(currentHash);
            }

            hashList.AddRange(currentHash);

            while (hashList.Count < 48) // for 32-byte key and 16-byte iv
            {
                preHashLength = currentHash.Length + data.Length + ((salt != null) ? salt.Length : 0);
                preHash = new byte[preHashLength];

                System.Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                System.Buffer.BlockCopy(data, 0, preHash, currentHash.Length, data.Length);
                if (salt != null)
                    System.Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + data.Length, salt.Length);

                currentHash = hash.ComputeHash(preHash);

                for (int i = 1; i < count; i++)
                {
                    currentHash = hash.ComputeHash(currentHash);
                }

                hashList.AddRange(currentHash);
            }
            hash.Clear();
            key = new byte[32];
            iv = new byte[16];
            hashList.CopyTo(0, key, 0, 32);
            hashList.CopyTo(32, iv, 0, 16);
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged cipher = new RijndaelManaged())
            {
                cipher.Key = Key;
                cipher.IV = IV;
                //cipher.Mode = CipherMode.CBC;
                //cipher.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            //if (IV == null || IV.Length <= 0)
            //  throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var cipher = new RijndaelManaged())
            {
                cipher.Key = Key;
                cipher.IV = new byte[16];
                //cipher.Mode = CipherMode.CBC;
                //cipher.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = cipher.CreateDecryptor(Key, cipher.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var bytes = default(byte[]);
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            bytes = srDecrypt.CurrentEncoding.GetBytes(srDecrypt.ReadToEnd());

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            //aintext = srDecrypt.ReadToEnd();
                        }
                        plaintext = ASCIIEncoding.UTF8.GetString(bytes, 0, bytes.Count());
                    }
                }

            }

            return plaintext;

        }

        public static string EncryptURL(string clearText)
        {
            string EncryptionKey = "STUDEBUDDY03092014"; //"MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    //   clearText = System.Web.HttpServerUtility.UrlTokenEncode(ms.ToArray());
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptURL(string cipherText)
        {
            string EncryptionKey = "STUDEBUDDY03092014";// "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    //cipherText = System.Web.HttpServerUtility.UrlTokenDecode(cipherText);
                    // cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}