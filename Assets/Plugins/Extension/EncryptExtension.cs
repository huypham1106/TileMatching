﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class EncryptExtension
{
    public const int BlockSize = 16;
    public const int KeyLen = 32;
    public const int IvLen = 16;

    private static byte[] key;
    private static byte[] iv;
    private const string passphrase = "lAhdSHH3242HSnah32hnAHZZpo9940ahNYCA";//"0123456789abcdef";
    //private const string passphrase = "0123456789abcdef";//"0123456789abcdef";


    /// <summary>
    /// Encrypt input text with the password using random salt.
    /// Returns base64 decoded encrypted string.
    /// </summary>
    /// <param name="text">Input text to encrypt</param>
    /// <param name="passphrase">Passphrase</param>
    public static string Encrypt(this string text)
    {
        using (var random = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[8];
            random.GetBytes(salt);

            DeriveKeyAndIv(passphrase, salt);

            byte[] encrypted;
            using (var aes = new RijndaelManaged())
            {
                aes.BlockSize = BlockSize * 8;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return System.Convert.ToBase64String(Concat(Concat("Salted__", salt), encrypted));
        }
    }

    /// <summary>
    /// Derypt encrypted text with the password using random salt.
    /// Returns the decrypted string.
    /// </summary>
    /// <param name="encrypted">Encrypted text to decrypt</param>
    /// <param name="passphrase">Passphrase</param>
    public static string Decrypt(this string encrypted)
    {
        byte[] ct = System.Convert.FromBase64String(encrypted);
        if (ct == null || ct.Length <= 0)
        {
            return "";
        }

        byte[] salted = new byte[8];
        Array.Copy(ct, 0, salted, 0, 8);

        string se = Encoding.UTF8.GetString(salted);
        if (se != "Salted__")
        {
            return "";
        }

        byte[] salt = new byte[8];
        Array.Copy(ct, 8, salt, 0, 8);

        byte[] cipherText = new byte[ct.Length - 16];
        Array.Copy(ct, 16, cipherText, 0, ct.Length - 16);

        DeriveKeyAndIv(passphrase, salt);

        string decrypted;
        using (var aes = new RijndaelManaged())
        {
            aes.BlockSize = BlockSize * 8;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msDecrypt = new MemoryStream(cipherText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        decrypted = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        key = null;
        iv = null;
        return decrypted;
    }

    /// <summary>
    /// Derive key and iv.
    /// </summary>
    /// <param name="passphrase">Passphrase</param>
    /// <param name="salt">Salt</param>
    private static void DeriveKeyAndIv(string passphrase, byte[] salt)
    {
        MD5 md5 = MD5.Create();

        key = new byte[KeyLen];
        iv = new byte[IvLen];

        byte[] dx = new byte[] { };
        byte[] salted = new byte[] { };
        byte[] pass = Encoding.UTF8.GetBytes(passphrase);

        for (int i = 0; i < (KeyLen + IvLen / 16); i++)
        {
            dx = Concat(Concat(dx, pass), salt);
            dx = md5.ComputeHash(dx);
            salted = Concat(salted, dx);
        }

        Array.Copy(salted, 0, key, 0, KeyLen);
        Array.Copy(salted, KeyLen, iv, 0, IvLen);
    }

    private static byte[] Concat(byte[] a, byte[] b)
    {
        byte[] output = new byte[a.Length + b.Length];

        for (int i = 0; i < a.Length; i++)
            output[i] = a[i];
        for (int j = 0; j < b.Length; j++)
            output[a.Length + j] = b[j];

        return output;
    }

    private static byte[] Concat(string a, byte[] b)
    {
        return Concat(Encoding.UTF8.GetBytes(a), b);
    }
}
