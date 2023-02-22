using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SIM.Utilidades
{
    public class Email
    {
        public static string EnviarEmail(string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido)
        {
            return EnviarEmail(null, rfstrDestinatarios, rfstrAsunto, rfstrContenido, null, true, null, null, null);
        }

        public static string EnviarEmail(string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, string rfstrArchivoAdjunto)
        {
            return EnviarEmail(null, rfstrDestinatarios, rfstrAsunto, rfstrContenido, null, true, null, null, rfstrArchivoAdjunto);
        }

        public static string EnviarEmail(string rfstrRemitente, string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd)
        {
            return EnviarEmail(rfstrRemitente, rfstrDestinatarios, rfstrAsunto, rfstrContenido, rfstrSMTPServer, vlbolRequiereAutenticacion, rfstrUsuario, rfstrPwd, null);
        }

        public static string EnviarEmail(string rfstrRemitente, string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, string rfstrArchivoAdjunto)
        {
            return EnviarEmail(rfstrRemitente, rfstrDestinatarios, "", "", rfstrAsunto, rfstrContenido, rfstrSMTPServer, vlbolRequiereAutenticacion, rfstrUsuario, rfstrPwd, rfstrArchivoAdjunto);
        }

        public static string EnviarEmail(string rfstrRemitente, string rfstrDestinatarios, string rfstrCopia, string rfstrCopiaOculta, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, string rfstrArchivoAdjunto)
        {
            int puertoSMTP = -1;
            string[] lcstrDestinatarios;
            string[] lcstrCopias = null;
            string[] lcstrCopiasOcultas = null;

            if (rfstrRemitente == null)
                rfstrRemitente = ConfigurationManager.AppSettings["EmailFrom"];

            if (rfstrSMTPServer == null)
                rfstrSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];

            string[] configSMTP = rfstrSMTPServer.Split(':');
            if (configSMTP.Length > 1)
            {
                rfstrSMTPServer = configSMTP[0];
                puertoSMTP = Convert.ToInt32(configSMTP[1]);
            }

            if (rfstrUsuario == null)
                rfstrUsuario = ConfigurationManager.AppSettings["SMTPUser"];

            if (rfstrPwd == null)
                rfstrPwd = ConfigurationManager.AppSettings["SMTPPwd"];

            if (rfstrUsuario != null && rfstrUsuario != "")
                vlbolRequiereAutenticacion = true;

            if (rfstrDestinatarios != null)
                if (rfstrDestinatarios.Trim() != "")
                    lcstrDestinatarios = rfstrDestinatarios.Split(';');
                else
                    return "[No hay recicipientes de correo configurados]";
            else
                return "[No hay recicipientes de correo configurados]";

            if (rfstrCopia != null && rfstrCopia.Trim() != "")
                lcstrCopias = rfstrCopia.Split(';');

            if (rfstrCopiaOculta != null && rfstrCopiaOculta.Trim() != "")
                lcstrCopiasOcultas = rfstrCopiaOculta.Split(';');

            MailMessage mail = new MailMessage();

            if (rfstrArchivoAdjunto != null)
            {
                Attachment att = new Attachment(rfstrArchivoAdjunto);
                mail.Attachments.Add(att);
            }

            // Specify sender and recipient options for the e-mail message.
            mail.From = new MailAddress(rfstrRemitente, null);

            foreach (string lcstrDestinatario in lcstrDestinatarios)
            {
                if (lcstrDestinatario.Trim() != "")
                    mail.To.Add(new MailAddress(lcstrDestinatario.Trim()));
            }

            if (rfstrCopia != null && rfstrCopia.Trim() != "")
            {
                foreach (string lcstrCopia in lcstrCopias)
                {
                    if (lcstrCopia.Trim() != "")
                        mail.CC.Add(new MailAddress(lcstrCopia.Trim()));
                }
            }

            if (rfstrCopiaOculta != null && rfstrCopiaOculta.Trim() != "")
            {
                foreach (string lcstrCopiaOculta in lcstrCopiasOcultas)
                {
                    if (lcstrCopiaOculta.Trim() != "")
                        mail.Bcc.Add(new MailAddress(lcstrCopiaOculta.Trim()));
                }
            }

            // Asignación de los datos del correo
            mail.Subject = rfstrAsunto;
            mail.IsBodyHtml = true;
            mail.Body = rfstrContenido;

            // Send the e-mail message via the specified SMTP server.
            SmtpClient smtp = new SmtpClient(rfstrSMTPServer);

            if (puertoSMTP != -1)
            {
                smtp.Port = puertoSMTP;
                if (puertoSMTP == 587)
                    smtp.EnableSsl = true;
            }

            if (vlbolRequiereAutenticacion)
            {
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rfstrUsuario, rfstrPwd);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;
            }

            smtp.Send(mail);

            return string.Join("\r\n", lcstrDestinatarios);
        }

        public static string EnviarEmail2(string rfstrRemitente, string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, Stream rfstrArchivoAdjunto)
        {
            string[] lcstrDestinatarios;

            if (rfstrDestinatarios != null)
                if (rfstrDestinatarios.Trim() != "")
                    lcstrDestinatarios = rfstrDestinatarios.Split(';');
                else
                    return "[No hay recicipientes de correo configurados]";
            else
                return "[No hay recicipientes de correo configurados]";

            MailMessage mail = new MailMessage();


            if (rfstrArchivoAdjunto != null)
            {
                Attachment att = new Attachment(rfstrArchivoAdjunto, "Solicitud Trámite.pdf");
                mail.Attachments.Add(att);
            }

            // Specify sender and recipient options for the e-mail message.
            mail.From = new MailAddress(rfstrRemitente, null);

            foreach (string lcstrDestinatario in lcstrDestinatarios)
            {
                if (lcstrDestinatario.Trim() != "")
                    mail.To.Add(new MailAddress(lcstrDestinatario.Trim()));
            }

            // Asignación de los datos del correo
            mail.Subject = rfstrAsunto;
            mail.IsBodyHtml = true;
            mail.Body = rfstrContenido;

            // Send the e-mail message via the specified SMTP server.
            SmtpClient smtp = new SmtpClient(rfstrSMTPServer);

            if (rfstrSMTPServer == "smtp.gmail.com")
            {
                smtp.Port = 587;
                smtp.EnableSsl = true;
            }

            if (vlbolRequiereAutenticacion)
            {
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rfstrUsuario, rfstrPwd);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;
            }

            smtp.Send(mail);

            return string.Join("\r\n", lcstrDestinatarios);
        }
    }
}