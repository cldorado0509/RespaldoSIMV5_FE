using DevExpress.Utils.Extensions;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;

namespace SIM.Utilidades
{
    public class EmailMK
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

        public static string EnviarEmail(string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, MemoryStream rfstrArchivoAdjunto, string name)
        {
            return EnviarEmail(null, rfstrDestinatarios, null, null, rfstrAsunto, rfstrContenido, null, true, null, null, rfstrArchivoAdjunto, name);
        }
        public static string EnviarEmail(string rfstrRemitente, string rfstrDestinatarios, string rfstrCopia, string rfstrCopiaOculta, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, string rfstrArchivoAdjunto)
        {
            int puertoSMTP = -1;
            string[] lcstrDestinatarios;
            string[] lcstrCopias = null;
            string[] lcstrCopiasOcultas = null;

            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            puertoSMTP = smtpSection.Network.Port;

            if (rfstrRemitente == null)
                rfstrRemitente = smtpSection.From;

            if (rfstrSMTPServer == null)
                rfstrSMTPServer = smtpSection.Network.Host;
            if (rfstrSMTPServer.Contains(":"))
            {
                string[] configSMTP = rfstrSMTPServer.Split(':');
                if (configSMTP.Length > 1)
                {
                    rfstrSMTPServer = configSMTP[0];
                    puertoSMTP = Convert.ToInt32(configSMTP[1]);
                }
            }

            if (rfstrUsuario == null)
                rfstrUsuario = smtpSection.Network.UserName;

            if (rfstrPwd == null)
                rfstrPwd = smtpSection.Network.Password;

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


            using (MimeMessage mm = new MimeMessage())
            {
                mm.From.Add(new MailboxAddress("Sender", rfstrRemitente));

                lcstrDestinatarios.ForEach(i => mm.To.Add(new MailboxAddress("Recipient", i)));
                mm.Subject = rfstrAsunto;
                lcstrCopias.ForEach(c => mm.Cc.Add(new MailboxAddress("Cc", c)));
                lcstrCopiasOcultas.ForEach(o => mm.Bcc.Add(new MailboxAddress("Bcc", o)));
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = rfstrContenido;
                if (rfstrArchivoAdjunto != null)
                {
                    FileInfo _file = new FileInfo(rfstrArchivoAdjunto);
                    string fileName = Path.GetFileName(_file.FullName);
                    builder.Attachments.Add(fileName, File.ReadAllBytes(_file.FullName));
                }
                mm.Body = builder.ToMessageBody();
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Connect(rfstrSMTPServer, puertoSMTP);
                    smtp.Authenticate(rfstrUsuario, rfstrPwd);
                    smtp.Send(mm);
                    smtp.Disconnect(true);
                }
            }
            return string.Join("\r\n", lcstrDestinatarios);
        }

        public static string EnviarEmail(string rfstrRemitente, string rfstrDestinatarios, string rfstrCopia, string rfstrCopiaOculta, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, MemoryStream rfstrArchivoAdjunto, string name)
        {
            int puertoSMTP = -1;
            string[] lcstrDestinatarios;
            string[] lcstrCopias = null;
            string[] lcstrCopiasOcultas = null;

            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            puertoSMTP = smtpSection.Network.Port;

            if (rfstrRemitente == null)
                rfstrRemitente = smtpSection.From;

            if (rfstrSMTPServer == null)
                rfstrSMTPServer = smtpSection.Network.Host;
            if (rfstrSMTPServer.Contains(":"))
            {
                string[] configSMTP = rfstrSMTPServer.Split(':');
                if (configSMTP.Length > 1)
                {
                    rfstrSMTPServer = configSMTP[0];
                    puertoSMTP = Convert.ToInt32(configSMTP[1]);
                }
            }

            if (rfstrUsuario == null)
                rfstrUsuario = smtpSection.Network.UserName;

            if (rfstrPwd == null)
                rfstrPwd = smtpSection.Network.Password;

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


            using (MimeMessage mm = new MimeMessage())
            {
                mm.From.Add(new MailboxAddress(rfstrRemitente, rfstrRemitente));

                lcstrDestinatarios.ForEach(i => mm.To.Add(new MailboxAddress("Recipient", i)));
                mm.Subject = rfstrAsunto;
                if (lcstrCopias != null && lcstrCopias.Length > 0) lcstrCopias.ForEach(c => mm.Cc.Add(new MailboxAddress(c, c)));
                if (lcstrCopiasOcultas != null && lcstrCopiasOcultas.Length > 0) lcstrCopiasOcultas.ForEach(o => mm.Bcc.Add(new MailboxAddress(o, o)));
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = rfstrContenido;
                if (rfstrArchivoAdjunto != null)
                {
                    rfstrArchivoAdjunto.Position = 0;
                    builder.Attachments.Add(name, rfstrArchivoAdjunto.ToArray());
                }
                mm.Body = builder.ToMessageBody();
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Connect(rfstrSMTPServer, puertoSMTP);
                    smtp.Authenticate(rfstrUsuario, rfstrPwd);
                    smtp.Send(mm);
                    smtp.Disconnect(true);
                }
            }
            return string.Join("\r\n", lcstrDestinatarios);
        }
        public static string EnviarEmail2(string rfstrRemitente, string rfstrDestinatarios, string rfstrAsunto, string rfstrContenido, string rfstrSMTPServer, bool vlbolRequiereAutenticacion, string rfstrUsuario, string rfstrPwd, Stream rfstrArchivoAdjunto)
        {
            int puertoSMTP = -1;
            string[] lcstrDestinatarios;

            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            puertoSMTP = smtpSection.Network.Port;

            if (rfstrRemitente == null)
                rfstrRemitente = smtpSection.From;

            if (rfstrSMTPServer == null)
                rfstrSMTPServer = smtpSection.Network.Host;
            if (rfstrSMTPServer.Contains(":"))
            {
                string[] configSMTP = rfstrSMTPServer.Split(':');
                if (configSMTP.Length > 1)
                {
                    rfstrSMTPServer = configSMTP[0];
                    puertoSMTP = Convert.ToInt32(configSMTP[1]);
                }
            }

            if (rfstrUsuario == null)
                rfstrUsuario = smtpSection.Network.UserName;

            if (rfstrPwd == null)
                rfstrPwd = smtpSection.Network.Password;

            if (rfstrUsuario != null && rfstrUsuario != "")
                vlbolRequiereAutenticacion = true;

            if (rfstrDestinatarios != null)
                if (rfstrDestinatarios.Trim() != "")
                    lcstrDestinatarios = rfstrDestinatarios.Split(';');
                else
                    return "[No hay recicipientes de correo configurados]";
            else
                return "[No hay recicipientes de correo configurados]";


            using (MimeMessage mm = new MimeMessage())
            {
                mm.From.Add(new MailboxAddress("Sender", rfstrRemitente));

                lcstrDestinatarios.ForEach(i => mm.To.Add(new MailboxAddress("Recipient", i)));
                mm.Subject = rfstrAsunto;
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = rfstrContenido;
                if (rfstrArchivoAdjunto != null)
                {
                    rfstrArchivoAdjunto.Position = 0;
                    builder.Attachments.Add("Solicitud Trámite.pdf", rfstrArchivoAdjunto);
                }
                mm.Body = builder.ToMessageBody();
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Connect(rfstrSMTPServer, puertoSMTP);
                    smtp.Authenticate(rfstrUsuario, rfstrPwd);
                    smtp.Send(mm);
                    smtp.Disconnect(true);
                }
            }
            return string.Join("\r\n", lcstrDestinatarios);
        }

    }
}