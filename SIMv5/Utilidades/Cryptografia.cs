using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace SIM.Utilidades
{   
    /// <summary>
    /// Clase que permite Encriptar y DesEncritar Archivos.
    /// </summary>
    public class Cryptografia
    {
        private const string initVector = "1wer54dfbbjkiu73";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;

        ///<summary>
        /// Encripta un archivo usando el algoritmo Rijndael. Este es el método utilizado por QUIPUX
        ///</summary>
        ///<param name="inputFile">Archivo sin encriptar</param>
        ///<param name="outputFile">Archivo Encriptado</param>
        ///<param name="sKey"></param>
        public Boolean Encriptar(string inputFile, string outputFile, byte[] key, byte[] iv)
        {

            try
            {
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;


                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
                return true;

            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al realizar el proceso.", er);
            }

        }

        ///<summary>
        /// Encripta un archivo usando el algoritmo Rijndael. Este es el método utilizado por QUIPUX
        ///</summary>
        ///<param name="inputFile">Archivo sin encriptar</param>
        ///<param name="archivoEncriptado">Archivo Encriptado</param>
        ///<param name="sKey"></param>
        public Boolean Encriptar(Stream _StrArch, string outputFile, byte[] key, byte[] iv)
        {
            return Encriptar(_StrArch, outputFile, key, iv, true);
        }

        ///<summary>
        /// Encripta un archivo usando el algoritmo Rijndael. Este es el método utilizado por QUIPUX
        ///</summary>
        ///<param name="inputFile">Archivo sin encriptar</param>
        ///<param name="archivoEncriptado">Archivo Encriptado</param>
        ///<param name="sKey"></param>
        public Boolean Encriptar(Stream _StrArch, string outputFile, byte[] key, byte[] iv, bool cerrarStream)
        {

            try
            {
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;


                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);

                int data;
                while ((data = _StrArch.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                if (cerrarStream)
                    _StrArch.Dispose();
                cs.Close();
                fsCrypt.Close();
                return true;

            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al realizar el proceso.", er);
            }

        }

        public Boolean Encriptar(byte[] archivo, string outputFile, byte[] key, byte[] iv)
        {

            try
            {
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;


                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);

                cs.Write(archivo, 0, archivo.Length);

                cs.Close();
                fsCrypt.Close();
                return true;

            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al realizar el proceso.", er);
            }

        }

        ///<summary>
        /// Desencripta un archivo usando el algoritmo Rijndael.
        ///</summary>
        ///<param name="inputFile">Archivo cifrado</param>
        ///<param name="outputFile">Archivo sin cifrar</param>
        ///<param name="sKey"></param>
        public void DesEncriptar(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            string Punto = "";
            byte[] msCrypt;
            MemoryStream msDecrypt;
            CryptoStream csDecrypt;

            try
            {
                msCrypt = System.IO.File.ReadAllBytes(inputFile);
                msDecrypt = new MemoryStream(msCrypt);
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;

                try
                {
                    csDecrypt = new CryptoStream(msDecrypt, RMCrypto.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                    byte[] _aux = new byte[32768];
                    while (csDecrypt.Read(_aux, 0, _aux.Length) > 0)
                    {
                        try
                        {
                            fsOut.Write(_aux, 0, _aux.Length);
                        }
                        catch
                        {
                            Punto += "<br> No se pudo leer el archivo desencriptado!";
                        }
                    }

                    csDecrypt.Dispose();
                }
                catch
                {
                    Punto += "<br> No se pudo desencriptar el archivo " + inputFile;
                    throw new Exception(Punto);
                }
            }
            catch (Exception er)
            {
                throw new InvalidOperationException("Error al realizar el proceso.", er);
            }

        }
        /// <summary>
        /// Desencripta el archivo y retorna una variable de memoria
        /// </summary>
        /// <param name="_Filename"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public MemoryStream DesEncriptar(string _Filename, byte[] key, byte[] iv)
        {
            string Punto = "";
            byte[] msCrypt;
            MemoryStream msDecrypt;
            CryptoStream csDecrypt;
            MemoryStream _out = new MemoryStream();

            try
            {
                msCrypt = System.IO.File.ReadAllBytes(_Filename);
                msDecrypt = new MemoryStream(msCrypt);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;

                try
                {
                    csDecrypt = new CryptoStream(msDecrypt, RMCrypto.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                    byte[] _aux = new byte[32768];
                    while (csDecrypt.Read(_aux, 0, _aux.Length) > 0)
                    {
                        try
                        {
                            _out.Write(_aux, 0, _aux.Length);
                        }
                        catch
                        {
                            Punto += "<br> No se pudo leer el archivo desencriptado!";
                        }
                    }

                    csDecrypt.Dispose();
                }
                catch
                {
                    Punto += "<br> No se pudo desencriptar el archivo " + _Filename;
                    throw new Exception(Punto);
                }
            }
            catch (Exception er)
            {
                Punto += " <br> No se pudo abrir el archivo " + _Filename;
                throw new Exception("Error al realizar el proceso." + " Punto : " + Punto);
            }                        
            return _out;
        }
        /// <summary>
        /// Desencripta el archivo y retorna una variable de memoria
        /// </summary>
        /// <param name="_Filename"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public MemoryStream DesEncriptarMNP(string _Filename, string password)
        {
            byte[] msCrypt;
            MemoryStream msDecrypt;
            CryptoStream csDecrypt;
            MemoryStream _out = new MemoryStream();

            try
            {
                msCrypt = System.IO.File.ReadAllBytes(_Filename);
                msDecrypt = new MemoryStream(msCrypt);

                // Create cryptography class and key.
                RijndaelManaged RMCrypto = new RijndaelManaged();
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                byte[] key = unicodeEncoding.GetBytes(password);

                try
                {
                    csDecrypt = new CryptoStream(msDecrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
                    byte[] _aux = new byte[32768];
                    while (csDecrypt.Read(_aux, 0, _aux.Length) > 0)
                    {
                        try
                        {
                            _out.Write(_aux, 0, _aux.Length);
                        }
                        catch
                        {
                        }
                    }

                    csDecrypt.Dispose();
                }
                catch
                {
                }
            }
            catch
            {
            }
            return _out;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static MemoryStream DesCifrar(string _Filename, byte[] key, byte[] iv)
        {
            byte[] msCrypt;
            MemoryStream msFile;

            msCrypt = System.IO.File.ReadAllBytes(_Filename);
            msFile = new MemoryStream(msCrypt);

            using (RijndaelManaged RMCrypto = new RijndaelManaged())
            {
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;

                ICryptoTransform decryptor = RMCrypto.CreateDecryptor(key, iv);

                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swDecrypt = new StreamWriter(csDecrypt))
                        {
                            swDecrypt.Write(msCrypt);
                        }
                        return msDecrypt;
                    }
                }
            }
        }
        public static MemoryStream DesCifrar2(string _Filename, byte[] key, byte[] iv)
        {
            string Punto = "";
            byte[] msCrypt;
            MemoryStream msDecrypt;
            CryptoStream csDecrypt;
            MemoryStream _out = new MemoryStream();

            try
            {
                msCrypt = System.IO.File.ReadAllBytes(_Filename);
                msDecrypt = new MemoryStream(msCrypt);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.PKCS7;
                RMCrypto.KeySize = 128;
                RMCrypto.BlockSize = 128;

                try
                {
                    csDecrypt = new CryptoStream(msDecrypt, RMCrypto.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                    long _Veces = 0;
                    int _Final = 0;
                    int _Longitud = 0;
                    if (msDecrypt.Length > 32768)
                    {
                        _Veces = msDecrypt.Length / 32768;
                        _Final = (int)msDecrypt.Length % 32768;
                        _Longitud = 32768;
                    }
                    else
                    {
                        _Veces = 1;
                        _Longitud = (int)msDecrypt.Length;
                    }
                    byte[] _aux = new byte[32768];
                    try
                    {
                        for (int i = 1; i <= _Veces; i++)
                        {
                            csDecrypt.Read(_aux, 0, _Longitud);
                            _out.Write(_aux, 0, _Longitud);
                        }
                        if (_Final > 0)
                        {
                            csDecrypt.Read(_aux, 0, _Final);
                            _out.Write(_aux, 0, _Final);
                        }
                    }
                    catch
                    {
                        Punto += "<br> No se pudo leer el archivo desencriptado!";
                    }
                    csDecrypt.Dispose();
                }
                catch
                {
                    Punto += "<br> No se pudo desencriptar el archivo " + _Filename;
                    throw new Exception(Punto);
                }
            }
            catch (Exception er)
            {
                Punto += " <br> No se pudo abrir el archivo " + _Filename;
                throw new Exception("Error al realizar el proceso." + " Punto : " + Punto);
            }
            return _out;
        }

        public static MemoryStream DesCifrar3(string _Filename, byte[] key, byte[] iv)
        {
            byte[] msCrypt;
            msCrypt = System.IO.File.ReadAllBytes(_Filename);

            RijndaelManaged RMCrypto = new RijndaelManaged();
            RMCrypto.Mode = CipherMode.CBC;
            RMCrypto.Padding = PaddingMode.PKCS7;
            RMCrypto.KeySize = 128;
            RMCrypto.BlockSize = 128;

            ICryptoTransform decryptor = RMCrypto.CreateDecryptor(key, iv);

            MemoryStream msDecrypt = new MemoryStream(msCrypt);

            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            byte[] fromEncrypt = new byte[msCrypt.Length];

            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            MemoryStream _Out = new MemoryStream(fromEncrypt);
            return _Out;
        }
        /// <summary>
        /// Guarda un mmemory stream como archivo de en disco
        /// </summary>
        /// <param name="_Ruta">Ruta y archivo donde se guardara el memorystream</param>
        /// <param name="_stream"></param>
        public static void GuardaMemory(string _Ruta, MemoryStream _stream)
        {
            FileStream fsOut = new FileStream(_Ruta, FileMode.Create);
            try
            {
                _stream.WriteTo(fsOut);
                _stream.Close();
            }
            catch
            { }
        }
        /// <summary>
        /// Retrona un array de bytes de un archivo cifrado a partir de un memorystream
        /// </summary>
        /// <param name="_Arc">Memory stream de entrada</param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static byte[] CifradoMs(MemoryStream _Arc, byte[] key, byte[] IV)
        {
            byte[] cifrado;
            using (RijndaelManaged _Algoritmo = new RijndaelManaged())
            {
                _Algoritmo.Mode = CipherMode.CBC;
                _Algoritmo.Padding = PaddingMode.PKCS7;
                _Algoritmo.Key = key;
                _Algoritmo.IV = IV;
                byte[] _work = _Arc.ToArray();
                ICryptoTransform cifrador = _Algoritmo.CreateEncryptor(_Algoritmo.Key, _Algoritmo.IV);
                using (MemoryStream msCifrar = new MemoryStream())
                using (CryptoStream csCifrar = new CryptoStream(msCifrar, cifrador, CryptoStreamMode.Write))
                {
                    csCifrar.Write(_work, 0, _work.Length);
                    csCifrar.FlushFinalBlock();
                    cifrado = msCifrar.ToArray();
                }
            }
            return cifrado;
        }
        /// <summary>
        /// Rertorna un memorystream descifrado a partir de otro memorystream
        /// </summary>
        /// <param name="_Arc">MemoryStream cifrado</param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static MemoryStream DesCifradoMs(MemoryStream _Arc, byte[] key, byte[] IV)
        {
            using (RijndaelManaged _Algoritmo = new RijndaelManaged())
            {
                _Algoritmo.Mode = CipherMode.CBC;
                _Algoritmo.Padding = PaddingMode.PKCS7;
                _Algoritmo.Key = key;
                _Algoritmo.IV = IV;
                byte[] _work = _Arc.ToArray();
                ICryptoTransform descifrador = _Algoritmo.CreateDecryptor(_Algoritmo.Key, _Algoritmo.IV);
                using (CryptoStream csDescifra = new CryptoStream(_Arc, descifrador, CryptoStreamMode.Read))
                    csDescifra.Read(_work, 0, _work.Length);
                return new MemoryStream(_work);
            }
        }
        /// <summary>
        /// Graba un archivo contenido en un memorystream a disco
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="_FileName"></param>
        public static void GrabaMemoryStream(MemoryStream _input, string _FileName)
        {
            FileStream salida = File.OpenWrite(_FileName);
            _input.WriteTo(salida);
            salida.Flush();
            salida.Close();
        }

        public static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        /// <summary>
        /// Funcion para obtener un string difrado con sha 256 , algortimo mas fuerte que el sha1 y el md5
        /// </summary>
        /// <param name="text">Texto a cifrar</param>
        /// <returns>El texto cifrado en sha256</returns>
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
        /// <summary>
        /// Genera un texto aleatorio con caracteres alfanumericos de la longitud especificada
        /// </summary>
        /// <param name="length">Longitud del texto de salida </param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            Random rand = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[rand.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}
