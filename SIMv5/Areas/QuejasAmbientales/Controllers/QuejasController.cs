namespace SIM.Areas.QuejasAmbientales.Controllers
{
    using DevExpress.XtraRichEdit;
    using SIM.Areas.QuejasAmbientales.Models;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Mvc;

    [Authorize]
    public class QuejasController : Controller
    {
        private string urlApiSecurity = "https://sim.metropol.gov.co/seguridad/";
        //private string urlApiQuejasAmbientales = "https://localhost:7286/";
        private string urlApiQuejasAmbientales = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioQuejasAmbientales").ToString();
        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public ActionResult Quejas()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

        [HttpGet, ActionName("ReporteQueja")]
        public async Task<ActionResult> ReporteQueja(decimal id)
        {
            ApiService apiService = new ApiService();


            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return null;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return null;

            response = await apiService.GetListAsync<ReporteQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoReporteQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return null;
            var list = (List<ReporteQuejaDTO>)response.Result;

            var oStream = GenerarReporte(list);

            //SIM.Utilidades.Archivos.GrabaMemoryStream(oStream, @"d:\temp\prueba.pdf"); //GUARDA EL PDF EN LOCAL
            oStream.Position = 0;
            var Archivo = oStream.GetBuffer();
            return File(Archivo, "application/pdf");
        }


        private MemoryStream GenerarReporte(List<ReporteQuejaDTO> listaReporte)
        {
            MemoryStream archivo = new MemoryStream();
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Areas/QuejasAmbientales/Recursos/Plantillas/PlantillaReporteQueja.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {

                TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                body = body.Replace("[NumeroQueja]", listaReporte[0].NroQueja.Value.ToString("0"));
                body = body.Replace("[AnoQueja]", listaReporte[0].Anno);
                body = body.Replace("[FormaRecibe]", listaReporte[0].NombreForma);
                body = body.Replace("[Recurso]", listaReporte[0].NombreRecurso);
                body = body.Replace("[Municipio]", listaReporte[0].Municipio);
                body = body.Replace("[Radicado]", listaReporte[0].Radicado);
                body = body.Replace("[Recibe]", listaReporte[0].Recibe);
                body = body.Replace("[Abogado]", listaReporte[0].NombreAbogado);
                body = body.Replace("[Direccion]", listaReporte[0].Direccion);
                body = body.Replace("[Asunto]", listaReporte[0].Asunto);
                body = body.Replace("[Comentarios]", listaReporte[0].Comentarios);
                body = body.Replace("[Afectacion]", listaReporte[0].NombreAfectacion);
                body = body.Replace("[FechaRad]", listaReporte[0].FechaRecepcion.Value.ToString("dd/MM/yyyy"));

                string _TablaPersona = " <table cellspacing=0 cellpadding=0 width=100% style='width:100%;border-collapse:collapse;'>";
                _TablaPersona += " <tr>" +
                    "<td colspan='6' width=100% style='background-color: gray;border:solid 1.0pt;padding-left:5.4pt;'><center><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif;font-weight: bold;'>Personas relacionadas con la queja</span></center></td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td width=20% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Nombre</span></td>" +
                    "<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Documento</span></td>" +
                    "<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Tipo</span></td>" +
                    "<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Fecha</span></td>" +
                    "<td width=15% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Descripcion</span></td>" +
                    "<td width=35% style='border:solid #000000 1.0pt;padding-left:5.4pt;'><span lang=ES style='font-size:11.0pt;font-family:Arial,sans-serif'>Dirección</span></td>" +
                    "</tr>";
                if (listaReporte[0].RSocial != null && listaReporte[0].RSocial != "")
                {

                    foreach (var item in listaReporte)
                    {
                        _TablaPersona += $"  <tr>" +
                            $"<td width=20% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.RSocial}</td>" +
                            $"<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.DocumentoN.Value.ToString("0")}</td>" +
                            $"<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.NombreTipoTercero}</td>" +
                            $"<td width=10% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.FechaTercero.Value.ToString("dd-MM-yyyy")}</td>" +
                            $"<td width=15% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.NombreInstalacion}</td>" +
                            $"<td width=35% style='border:solid #000000 1.0pt;padding-left:5.4pt;font-size:9.0pt;font-family:Arial,sans-serif'>{item.Descripcion}</td>" +
                            $"</tr>";
                    }

                }
                _TablaPersona += "</table>";

                body = body.Replace("[TablaPersonas]", _TablaPersona);



                using (RichEditDocumentServer doc = new RichEditDocumentServer())
                {
                    doc.HtmlText = body;
                    // doc.LoadDocument(body, DocumentFormat.Html);
                    doc.ExportToPdf(archivo);
                }
            }
            else return null;
            return archivo;
        }
    }
}