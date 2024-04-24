using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Dynamics.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using System.Web.Http;

namespace SIM.Areas.Dynamics.Controllers
{
    public class FacturasApiController : ApiController
    {
        DynamicsContext dbDynamics = new DynamicsContext();
        FacturaController factura = new FacturaController();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customFilters"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ObtenerFacturas")]
        public JArray GetFacturas(string customFilters)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<ListadoFacturas> listadoFacturas = new List<ListadoFacturas>();
            if (customFilters != "" && customFilters != null)
            {
                string[] _Buscar = customFilters.Split(';');
                string _Sql = "SELECT CIT.INVOICEID AS FACTURA,CIT.INVOICEDATE AS FECHAFACTURA,CIT.INVOICEACCOUNT AS DOCUMENTO,DPT.NAME AS TERCERO,COUNTY.NAME AS MUNICIPIO, LEA_PCE.LOCATOR AS EMAIL FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT=CT.ACCOUNTNUM INNER JOIN DIRPARTYTABLE AS DPT ON CT.PARTY=DPT.RECID LEFT OUTER JOIN LOGISTICSPOSTALADDRESS AS LPA ON CIT.POSTALADDRESS=LPA.RECID LEFT OUTER JOIN LOGISTICSELECTRONICADDRESS AS LEA_PCE ON DPT.PRIMARYCONTACTEMAIL=LEA_PCE.RECID LEFT OUTER JOIN LOGISTICSADDRESSCOUNTY AS COUNTY ON COUNTY.COUNTRYREGIONID=LPA.COUNTRYREGIONID AND COUNTY.STATEID=LPA.STATE AND COUNTY.COUNTYID=LPA.COUNTY ";
                string _Where = " WHERE (CIT.POSTED = 1)";
                string _WhereAux = "";
                string _Order = " ORDER BY FECHAFACTURA DESC";
                foreach (string _buscar in _Buscar)
                {
                    string[] _filtro = _buscar.Split(':');
                    if (_filtro.Length > 0)
                    {
                        switch (_filtro[0])
                        {
                            case "D":
                                _WhereAux += $" OR CIT.INVOICEACCOUNT='{_filtro[1]}'";
                                break;
                            case "T":
                                _WhereAux += $" OR DPT.NAME='{_filtro[1]}'";
                                break;
                            case "B":
                                string[] _Facturas = _filtro[1].Split(',');
                                if (_Facturas.Length > 0)
                                {
                                    _WhereAux += $" OR  CIT.INVOICEID IN (";
                                    foreach (string _factura in _Facturas)
                                    {
                                        _WhereAux += "'" + _factura + "',";
                                    }
                                    _WhereAux = _WhereAux.Substring(0, _WhereAux.Length - 1) + ")";
                                }
                                break;
                            case "F":
                                if (_filtro[1].Contains(","))
                                {
                                    string[] _Fechas = _filtro[1].Split(',');
                                    DateTime _dtAuxDesde;
                                    DateTime _dtAuxHasta;
                                    if (DateTime.TryParse(_Fechas[0], out _dtAuxDesde) && DateTime.TryParse(_Fechas[1], out _dtAuxHasta))
                                        _WhereAux += $" OR CIT.INVOICEDATE BETWEEN '{_dtAuxDesde:yyyy-MM-dd}' AND '{_dtAuxHasta:yyyy-MM-dd}'";
                                }
                                else
                                {
                                    DateTime _dtAux;
                                    if (DateTime.TryParse(_filtro[1], out _dtAux))
                                        _WhereAux += $" OR CIT.INVOICEDATE BETWEEN '{_dtAux:yyyy-MM-dd}' AND '{_dtAux:yyyy-MM-dd}'";
                                }
                                break;
                        }
                    }
                }
                if (_WhereAux.Length > 0) _Where += $" AND ({_WhereAux.Substring(3)})";
                _Sql += $"{_Where} {_Order}";
                var Facturas = dbDynamics.Database.SqlQuery<ListadoFacturas>(_Sql).ToList();
                listadoFacturas = Facturas;
            }
            return JArray.FromObject(listadoFacturas, Js);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Terceros")]
        public JArray GetTerceros()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string Sql = "SELECT DISTINCT  RTRIM(LTRIM(DPT.NAME)) AS Name, CIT.INVOICEACCOUNT AS Tercero FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT = CT.ACCOUNTNUM INNER JOIN DIRPARTYTABLE AS DPT ON CT.PARTY = DPT.RECID WHERE (CIT.POSTED = 1) ORDER BY Name";
                var RespBienes = dbDynamics.Database.SqlQuery<TercerosModel>(Sql);
                return JArray.FromObject(RespBienes, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("EnviarFactura")]
        public object PostEnviarFactura(EnvioFactura datos)
        {
            if (!ModelState.IsValid) return new { result = "Error", Mensaje = "No se enviaron todos los datos" };
            MemoryStream pdfFac = factura.GenerarFacturaPdf(datos.IdFact);
            if (pdfFac != null)
            {
                string _Mesanje = "";
                if (datos.Mensaje != "" && datos.Mensaje != null) _Mesanje = datos.Mensaje;
                else _Mesanje = "El Área Metropolitana del Valle de Aburrá remite para lo pertinente.";
                //if (!EnviarMailFactura(datos.Tercero, datos.Mail, datos.IdFact, _Mesanje, pdfFac))
                if (!EnviarMailFacturaMK(datos.Tercero, datos.Mail, datos.IdFact, _Mesanje, pdfFac))
                {
                    return new { result = "Error", Mensaje = "No se pudo enviar el correo electrónico!" };
                }
            }
            else return new { result = "Error", Mensaje = "No se pudo generar la factura " + datos.IdFact };
            return new { result = "Ok", Mensaje = "Correo enviado correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("EnviarFacturasSel")]
        public object PostEnviarFacturasSel(List<EnvioFactura> datos)
        {
            if (!ModelState.IsValid) return new { result = "Error", Mensaje = "No se enviaron todos los datos" };
            string _Mesanje = "";
            if (datos[0].Mensaje != "" && datos[0].Mensaje != null) _Mesanje = datos[0].Mensaje;
            else _Mesanje = "El Área Metropolitana del Valle de Aburrá remite para lo pertinente.";
            MemoryStream pdfFac = new MemoryStream();
            foreach (var item in datos)
            {
                try
                {
                    pdfFac = factura.GenerarFacturaPdf(item.IdFact);
                    if (pdfFac != null)
                    {
                        //EnviarMailFactura(item.Tercero, item.Mail, item.IdFact, _Mesanje, pdfFac);
                        EnviarMailFacturaMK(item.Tercero, item.Mail, item.IdFact, _Mesanje, pdfFac);
                    }
                }
                catch { }
            }
            return new { result = "Ok", Mensaje = "Correo enviado correctamente" };
        }
        private bool EnviarMailFactura(string _Destinatario, string _Para, string _Factura, string _Mensaje, MemoryStream _MsPdf)
        {
            if (_Para.Length == 0) return false;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/Plantillas/PlantillaEnvioFactura.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {
                var hora = DateTime.Now.Hour;
                string _saludo = "";
                if (hora >= 6 && hora < 13) _saludo = "Buenos días";
                if (hora >= 13 && hora < 21) _saludo = "Buenas tardes";
                if (hora >= 21 && hora < 6) _saludo = "Buenas noches";
                body = body.Replace("[destinatario]", _Destinatario);
                body = body.Replace("[saludo]", _saludo);
                body = body.Replace("[mensaje]", _Mensaje);
                MailMessage correo = new MailMessage();

                if (_Para.Contains(";"))
                {
                    string[] _mails = _Para.Split(';');
                    foreach (string _para in _mails) correo.To.Add(new MailAddress(_para));
                }
                else correo.To.Add(new MailAddress(_Para));
                correo.Subject = "Remisión fatura " + _Factura + " para " + _Destinatario;
                correo.Body = body;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.High;
                _MsPdf.Seek(0, SeekOrigin.Begin);
                Attachment _File = new Attachment(_MsPdf, _Factura + ".pdf", "application/pdf");
                correo.Attachments.Add(_File);
                var rfstrRemitente = ConfigurationManager.AppSettings["EmailFrom"];
                if (rfstrRemitente != null)
                {
                    correo.From = new MailAddress(rfstrRemitente, null);
                    var rfstrSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
                    int puertoSMTP;
                    string[] configSMTP = rfstrSMTPServer.Split(':');
                    if (configSMTP.Length > 1)
                    {
                        rfstrSMTPServer = configSMTP[0];
                        puertoSMTP = Convert.ToInt32(configSMTP[1]);
                        var rfstrUsuario = ConfigurationManager.AppSettings["SMTPUser"];
                        var rfstrPwd = ConfigurationManager.AppSettings["SMTPPwd"];
                        SmtpClient smtp = new SmtpClient(rfstrSMTPServer);
                        smtp.Port = puertoSMTP;
                        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(rfstrUsuario, rfstrPwd);
                        try
                        {
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = SMTPUserInfo;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            smtp.Send(correo);
                            return true;
                        }
                        catch (SmtpException ex)
                        {
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        private bool EnviarMailFacturaMK(string _Destinatario, string _Para, string _Factura, string _Mensaje, MemoryStream _MsPdf)
        {
            if (_Para.Length == 0) return false;

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/Plantillas/PlantillaEnvioFactura.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {
                string _Asunto = "Remisión fatura " + _Factura + " para " + _Destinatario;
                var hora = DateTime.Now.Hour;
                string _saludo = "";
                if (hora >= 6 && hora < 13) _saludo = "Buenos días";
                if (hora >= 13 && hora < 21) _saludo = "Buenas tardes";
                if (hora >= 21 && hora < 6) _saludo = "Buenas noches";
                body = body.Replace("[destinatario]", _Destinatario);
                body = body.Replace("[saludo]", _saludo);
                body = body.Replace("[mensaje]", _Mensaje);
                try
                {
                    SIM.Utilidades.EmailMK.EnviarEmail(_Para, _Asunto, body, _MsPdf, _Factura + ".pdf");
                    return true;
                }
                catch { return false; }
            }
            else return false;
        }
    }
}
