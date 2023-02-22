
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data;
using SIM.Data.Control;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Tala.Controllers
{
    public class imgTalaController : Controller
    {
     
        EntitiesFloraOracle modeloArchivos = new EntitiesFloraOracle();
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        //SIM.Areas.Tala.Models.EntitiesTala dbf = new SIM.Areas.Tala.Models.EntitiesTala();

        public JsonResult Index(Decimal id)
        {

            HttpPostedFileBase file = Request.Files[0];
            int fileSize = file.ContentLength;
            string fileName = file.FileName;
            string mimeType = file.ContentType;
            System.IO.Stream fileContent = file.InputStream;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] foto = target.ToArray();

            //dbf.SP_SET_FOTO_TALA(id, foto);
            modeloArchivos.SP_SET_FOTO_TALA(id, foto);

            /*
            AppSettingsReader webConfigReader = new AppSettingsReader();
            FOTOGRAFIA fotografia = new FOTOGRAFIA();
            string url_base = "";
            string path = "";
         
                url_base = (string)webConfigReader.GetValue("url_foto_formulario", typeof(string));
                path = (string)webConfigReader.GetValue("ruta_disco_fotoFormulario", typeof(string)) + "\\tala\\" + idVisita;
            


            string filename = "";

            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload]))
                    continue;

                verificarRutaAlmacenamiento(path, url_base);


                filename = Path.GetFileName(Request.Files[upload].FileName);
                string[] nombreFoto = filename.Split('.');
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                Request.Files[upload].SaveAs(Path.Combine(path, filename));

                fotografia = new FOTOGRAFIA();
                fotografia.S_ARCHIVO = filename;
                fotografia.S_ETIQUETA = nombreFoto[0];
                fotografia.S_RUTA = path;
                fotografia.S_HASH = "";
                fotografia.GPS_LATITUD = 0;
                fotografia.GPS_LONGITUD = 0;
                fotografia.S_USUARIO = "";
                fotografia.D_CREACION = DateTime.Now;
                fotografia.N_ESTADO = 1;

                modeloArchivos.FOTOGRAFIA.Add(fotografia);
                modeloArchivos.SaveChanges();

            }


            String url = url_base + idVisita + "/" + filename;*/


            return Json("");
        }


        public void verificarRutaAlmacenamiento(String ruta_archivos, String url_archivos)
        {


        }

        public JsonResult deleteFile(long id)
        {

            FOTOGRAFIA fotografia = new FOTOGRAFIA();
            fotografia = db.FOTOGRAFIA.Where(f => f.ID_FOTOGRAFIA == id).FirstOrDefault();
            fotografia.N_ESTADO = 2;
            modeloArchivos.SaveChanges();
            return Json("{'fotgrafia':[{'id':'" + fotografia.ID_FOTOGRAFIA + "', 'estado':'" + fotografia.N_ESTADO + "'}]}");
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


    public partial class FotografiaTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string etiqueta { get; set; }
        public string url { get; set; }

        public FotografiaTO(FOTOGRAFIA f, String url)
        {
            this.id = f.ID_FOTOGRAFIA;
            this.nombre = f.S_ARCHIVO;
            this.etiqueta = f.S_ETIQUETA;
            this.url = url;
        }
    }
}