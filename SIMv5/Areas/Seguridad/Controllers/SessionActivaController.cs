using SIM.Data;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SIM.Areas.Seguridad.Controllers
{
    public class SessionActivaController
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        //
        // GET: /Seguridad/SessionActiva/
        public byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");
        public byte[] Clave = Encoding.ASCII.GetBytes("12EstaClave34es56dificil489ssswf");
        private IdentityManager _idManager = new IdentityManager();
        private AccountController ac = new AccountController();

        public string crearSessionTabla(string usuario, string claveu)
        {
            string claveEnc = Encripta(claveu);
            //string userpass=Desencripta(claveEnc);
            //  guardarSession(usuario, claveu);
            return null;
        }
        public string guardarSession(string usuario, string claveu)
        {
            //db.SP_GUARDARSESSION(usuario, claveu);
            return null;
        }
        public string limpiarSession()
        {
            return null;
        }
        public string Encripta(string Cadena)
        {

            byte[] inputBytes = Encoding.ASCII.GetBytes(Cadena);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }



        public string Desencripta(string Cadena)
        {
            byte[] inputBytes = Convert.FromBase64String(Cadena);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }
        public string updateLoguin(string usuario, string clave)
        {


            ac.autoLoguin(usuario, clave);

            return null;
        }
    }
}