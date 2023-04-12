using DevExpress.Web;
using Independentsoft.Office.Odf.Fields;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.PDFFile;
using SIM.Areas.Pqrsd.Models;
using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SIM.Areas.GestionDocumental.Controllers
{
    [Authorize]
    public class DocumentosApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Retorna los documentos de un tramite
        /// </summary>
        /// <param name="CodTramite">Codigo del trámite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocumento")]
        public JArray GetObtieneDocumento(int IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int idUsuario = 0;
                decimal funcionario = 0;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                    funcionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
                }


                var model = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                             join Ser in dbSIM.TBSERIE on Doc.CODSERIE equals Ser.CODSERIE
                             where Doc.ID_DOCUMENTO == IdDocumento
                             orderby Doc.CODDOCUMENTO descending
                             select new Documento
                             {
                                 ID_DOCUMENTO = Doc.ID_DOCUMENTO,
                                 CODDOC = Doc.CODDOCUMENTO,
                                 SERIE = Ser.NOMBRE,
                                 FECHA = Doc.FECHACREACION.Value,
                                 ESTADO = Doc.S_ESTADO == "N" ? "Anulado" : "",
                                 ADJUNTO = Doc.S_ADJUNTO != "1" ? "No" : "Si",
                                 CODTRAMITE = Doc.CODTRAMITE,
                                 EDIT_INDICES = (from PER in dbSIM.PERMISOSSERIE where PER.CODFUNCIONARIO == funcionario &&
                                                 PER.CODSERIE == Doc.CODSERIE select PER.PM).FirstOrDefault() == "1" ? true : false
                             }).ToList();
                if (model != null)
                {
                    foreach (var doc in model)
                    {
                        if (doc.ESTADO != "Anulado")
                        {
                            var estDoc = (from A in dbSIM.ANULACION_DOC
                                          join D in dbSIM.TRAMITES_PROYECCION on A.ID_PROYECCION_DOC equals D.ID_PROYECCION_DOC
                                          where D.CODTRAMITE == doc.CODTRAMITE && D.CODDOCUMENTO == doc.CODDOC && A.S_ESTADO == "P"
                                          select A.ID_ANULACION_DOC).FirstOrDefault();
                            if (estDoc > 0) doc.ESTADO = "En proceso";
                        }
                    }
                }
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("RecibeArch")]
        public string RecibeArchAsync()
        {
            string _resp = "";
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            PDFFile oSrcPDF = null;
            var httpRequest = context.Request;
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(_Ruta)) Directory.CreateDirectory(_Ruta);
            var File = httpRequest.Files[0];
            if (File != null && File.ContentLength > 0)
            {
                string[] fileExtensions = { ".pdf" };
                var fileName = File.FileName.ToLower();
                var isValidExtenstion = fileExtensions.Any(ext =>
                {
                    return fileName.LastIndexOf(ext) > -1;
                });
                if (isValidExtenstion) {
                    string filePath = _Ruta + @"\" + fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);    
                    }
                    File.SaveAs(filePath);
                    try
                    {
                        oSrcPDF = PDFFile.FromFile(filePath);
                        _resp = "Ok;" + fileName;
                    }
                    catch (Exception ex)
                    {
                        _resp = "Error;" + ex.Message;
                    }
                    oSrcPDF.Dispose();
                }
                else
                {
                    _resp = "Error;Tipo de documento no permitido";
                }
            }
            return _resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <param name="Doc"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ReemplazaDoc")]
        public object PostReemplazaDoc(decimal IdDocumento, string Doc)
        {
            PDFFile oSrcPDF = null;
            if (IdDocumento <= 0) return new { resp = "Error", mensaje = "No se ha ingresado un identificador de documento" };
            if (Doc == null || Doc.Length == 0) return new { resp = "Error", mensaje = "No se ha ingresado un documento de reemplazo" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            string filePath = _Ruta + @"\" + Doc;
            FileInfo DocNuevo = new FileInfo(filePath);   
            if (!DocNuevo.Exists) return new { resp = "Error", mensaje = "No se ha podido encontrar el documento subido al sistema" };
            var Docu = (from D in dbSIM.TBTRAMITEDOCUMENTO
                        where D.ID_DOCUMENTO == IdDocumento
                        select D).FirstOrDefault();
            if (Docu == null || Docu.RUTA.Length == 0) return new { resp = "Error", mensaje = "Hay un problema con el documento destino, no se encontró la ruta" };
            FileInfo DocAnte = new FileInfo(Docu.RUTA);
            if (!DocAnte.Exists) return new { resp = "Error", mensaje = "No se ha podido encontrar el documento cod identificador " + IdDocumento };
            try
            {
                oSrcPDF = PDFFile.FromFile(DocNuevo.FullName);
                var _bkFile = DocAnte.DirectoryName + @"\"+ Path.GetFileNameWithoutExtension(DocAnte.FullName) + "_old" + DocAnte.Extension;
                DocAnte.CopyTo(_bkFile);
                if (DocAnte.Extension.ToLower() != DocNuevo.Extension.ToLower()) {
                    DocAnte.Delete();
                    Docu.RUTA = DocAnte.DirectoryName + @"\" + Path.GetFileNameWithoutExtension(DocAnte.FullName) + DocNuevo.Extension;
                }
                
                DocNuevo.CopyTo(Docu.RUTA);
                int _Pag = oSrcPDF.PagesCount;
                oSrcPDF.Dispose();
                Docu.PAGINAS = _Pag;
                Docu.CIFRADO = "0";
                dbSIM.Entry(Docu).State = System.Data.Entity.EntityState.Modified;
                dbSIM.SaveChanges();
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = ex.Message };
            }
            return new { resp = "Ok", mensaje = "Documento reemplazado correctamente" };
        }
    }

        public class Documento
    {
        public decimal ID_DOCUMENTO { get; set; }
        public decimal CODDOC { get; set; }
        public string SERIE { get; set; }
        public DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string ADJUNTO { get; set; }
        public decimal CODTRAMITE { get; set; }
        public bool EDIT_INDICES { get; set; }   
    }
}
