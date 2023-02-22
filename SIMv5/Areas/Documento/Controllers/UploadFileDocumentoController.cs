using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using SIM.Data;
using SIM.Data.Control;
using SIM.Models;

namespace SIM.Areas.DocumentoAdjunto.Controllers
{
    public class UploadFileDocumentoController : Controller
    {
        //
        // GET: /FileUpload/
        //[HttpPost]
        //SIM.Areas.Fotografia.Models.EntitiesFotografia modeloArchivos = new SIM.Areas.Fotografia.Models.EntitiesFotografia();
        EntitiesSIMOracle modeloArchivos = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        // EntitiesArchivos modeloArchivos = new EntitiesArchivos();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        public JsonResult Index(String etiqueta)
        {
            Int32 idUsuario;
            decimal codFuncionario;

            AppSettingsReader webConfigReader = new AppSettingsReader();
            DOCUMENTO documento = new DOCUMENTO();
           // FOTOGRAFIA fotografia = new FOTOGRAFIA();

            string url_base = (string)webConfigReader.GetValue("url_doc_form", typeof(string));
            string path = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string));
            string tipoArchivo = (string)webConfigReader.GetValue("tipo_archivo", typeof(string));
            string filename = "";

            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload]))
                    continue;

                verificarRutaAlmacenamiento(path, url_base);


                filename = Path.GetFileName(Request.Files[upload].FileName);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                Request.Files[upload].SaveAs(Path.Combine(path, filename));

                documento = new DOCUMENTO();
                documento.S_ARCHIVO = filename;
                documento.S_RUTA = path;
                documento.S_ETIQUETA = etiqueta;
                documento.S_HASH = "";
                documento.GPS_LATITUD = 0;
                documento.GPS_LONGITUD = 0;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                    documento.S_USUARIO = Convert.ToString(codFuncionario);
                }
                documento.D_CREACION = DateTime.Now;
                documento.N_ESTADO = 1;

                modeloArchivos.DOCUMENTO.Add(documento);
                modeloArchivos.SaveChanges();

            }


            String url = url_base + filename;

            DocumentoTO ft = new DocumentoTO(documento, url);

            return Json(ft);
        }


        public void verificarRutaAlmacenamiento(String ruta_archivos, String url_archivos)
        {


        }

        public JsonResult deleteFile(long id)
        {

            DOCUMENTO documento = new DOCUMENTO();
         //   documento = modeloArchivos.documento.Where(f => f.ID_DOCUMENTO == id).FirstOrDefault();
            documento.N_ESTADO = 2;
            modeloArchivos.SaveChanges();
            return Json("{'fotgrafia':[{'id':'" + documento.ID_DOCUMENTO + "', 'estado':'" + documento.N_ESTADO + "'}]}");
        }

        public JsonResult consultarArchivosEncuesta(String idEncuesta)
        {
            //Consultar los archivos de una encuesta
            return Json(
            ""//{"fotografias":[ {"id":"111",      "url":"http://servidor/imagen.extension",     "etiqueta":"http://servidor/imagen.extension"    },    {"id":"111",      "url":"http://servidor/imagen.extension",     "etiqueta":"http://servidor/imagen.extension"    },    {"id":"111",      "url":"http://servidor/imagen.extension",     "etiqueta":"http://servidor/imagen.extension"}]}
             );
        }
        private static bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }


    public partial class DocumentoTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string etiqueta { get; set; }
        public string url { get; set; }

        public DocumentoTO(DOCUMENTO f, String url)
        {
            this.id = f.ID_DOCUMENTO;
            this.nombre = f.S_ARCHIVO;
            this.etiqueta = f.S_ETIQUETA;
            this.url = url;
        }


  

    
	}
}