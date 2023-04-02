using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web.Configuration;

namespace ViajesCompartidos.Handlers
{
    public class EncriptadoHandler
    {
        private static byte[] aesKey;
        public static byte[] aesIV;
        const string AESIV = "AxokAf6CYUeRMogrwQE5cQ==";

        public EncriptadoHandler()
        {
            Initialize();
        }

        public static void Initialize()
        {
            aesKey = Convert.FromBase64String(WebConfigurationManager.AppSettings["AESKEY"]);
            aesIV = Convert.FromBase64String(AESIV);
        }

        public static string BytesToString(byte[] bytes) => Convert.ToBase64String(bytes);
        public static byte[] StringToBytes(string texto) => Convert.FromBase64String(texto);

        public static byte[] Encriptar(string texto)
        {
            if (texto == null || texto.Length <= 0)
                throw new ArgumentNullException("texto");
            ValidarParametros();

            byte[] bytes;

            using (ICryptoTransform encryptor = Aes.Create().CreateEncryptor(aesKey, aesIV))
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(texto);
                        }
                        bytes = msEncrypt.ToArray();
                    }
                }
            }

            return bytes;
        }

        public static string DesEncriptar(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
                throw new ArgumentNullException("bytes");
            ValidarParametros();

            string texto = null;

            using (ICryptoTransform decryptor = Aes.Create().CreateDecryptor(aesKey, aesIV))
            {
                using (CryptoStream csDecrypt = new CryptoStream(new MemoryStream(bytes), decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        texto = srDecrypt.ReadToEnd();
                    }
                }
            }

            return texto;
        }

        private static void ValidarParametros()
        {
            if (aesKey == null || aesKey.Length <= 0)
                Initialize();

            if (aesKey == null || aesKey.Length <= 0)
                throw new ArgumentNullException("Key");

            if (aesIV == null || aesIV.Length <= 0)
                Initialize();

            if (aesIV == null || aesIV.Length <= 0)
                throw new ArgumentNullException("IV");
        }
    }
}