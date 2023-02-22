using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.GestionDocumental.Models;
using SIM.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Data.Entity;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Threading.Tasks;
using SIM.Data.Documental;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador DigitalizacionApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class DigitalizacionApiController : ApiController
    {
        /// <summary>
        /// Estructura con la configuración para la construcción de los docuemntos asociados.
        /// </summary>
        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public int id;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos.
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [HttpPost, ActionName("Digitalizar")]
        [Authorize(Roles = "VDIGITALIZACION")]
        public async Task<object> PostDigitalizar(string documentoCB)
        {
            int idDocumento;
            DateTime fechaCreacion;
            datosRespuesta resultado;
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsDocumentalPath"];
            string filename;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            fechaCreacion = DateTime.Now;

            // Verifica que el Radicado no haya sido digitalizado aun
            var documentoRadicado = from documento in dbSIM.DOCUMENTOS
                                    join radicados in dbSIM.RADICADOS_ETIQUETAS on documento.ID_RADICADO equals radicados.ID_RADICADO
                                    where radicados.S_ETIQUETA == documentoCB
                                    select documento;

            if (documentoRadicado.Count() > 0)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "El radicado ya ha sido digitalizado.");
                return response;

                // Error, el radicado ya tiene documento digitalizado
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El radicado ya ha sido digitalizado.", id = documentoRadicado.FirstOrDefault().ID_DOCUMENTO };
                return resultado;
            }

            var datosRadicado = (from radicado in dbSIM.RADICADOS_ETIQUETAS
                                 where radicado.S_ETIQUETA == documentoCB
                                 select new
                                 {
                                     radicado.ID_RADICADO,
                                     radicado.RADICADO_UNIDADDOCUMENTAL.ID_UNIDADDOCUMENTAL
                                 }).FirstOrDefault();

            if (datosRadicado == null)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "El radicado no existe .");
                return response;

                // Error, el radicado no existe
                resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "El radicado no existe .", id = 0 };
                return resultado;
            }

            var documentoDigitalizado = new DOCUMENTOS();
            documentoDigitalizado.ID_UNIDADDOCUMENTAL = datosRadicado.ID_UNIDADDOCUMENTAL;
            documentoDigitalizado.ID_UNIDADTIPO = 3;
            documentoDigitalizado.ID_RADICADO = datosRadicado.ID_RADICADO;
            dbSIM.Entry(documentoDigitalizado).State = EntityState.Added;
            dbSIM.SaveChanges();
            
            idDocumento = documentoDigitalizado.ID_DOCUMENTO;

            //idDocumento = 345678;

            pathDocumentos += "\\" + idDocumento.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\");

            if (!Directory.Exists(pathDocumentos))
            {
                Directory.CreateDirectory(pathDocumentos);
            }

            var provider = new DigitalizacionMultipartFormDataStreamProvider(pathDocumentos);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                filename = "documento_" + idDocumento.ToString("D8") + (Path.GetExtension(provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty)) == "" ? "" : Path.GetExtension(provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty)));

                if (System.IO.File.Exists(pathDocumentos + "\\" + filename))
                {
                    System.IO.File.Delete(pathDocumentos + "\\" + filename);
                }

                //System.IO.File.Move(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty), pathDocumentos + "\\Documentos\\" + filename);
                Utilidades.Security.CifrarDocumento(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty), pathDocumentos + "\\" + filename);
                System.IO.File.Delete(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty));

                resultado = new datosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "", id = idDocumento };

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            catch (System.Exception e)
            {
                //resultado = new datosRespuesta() { tipoRespuesta = "Error", detalleRespuesta = "", id = 0 };
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return response;
            }
        }
    }

    public class DigitalizacionMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public DigitalizacionMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }
}