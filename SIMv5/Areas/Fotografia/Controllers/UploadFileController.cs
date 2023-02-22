using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Utilidades;
using SIM.Data.Control;
using SIM.Data;

namespace SIM.Areas.Fotografia.Controllers
{
    public class UploadFileController : Controller
    {
        //
        // GET: /FileUpload/
        //[HttpPost]
        //SIM.Areas.Fotografia.Models.EntitiesFotografia modeloArchivos = new SIM.Areas.Fotografia.Models.EntitiesFotografia();
        EntitiesSIMOracle modeloArchivos = new EntitiesSIMOracle();
       // EntitiesArchivos modeloArchivos = new EntitiesArchivos();
        public JsonResult Index(String idVisita,String tipo)
        {
            int anchoFotografia;
            AppSettingsReader webConfigReader = new AppSettingsReader();
            FOTOGRAFIA fotografia = new FOTOGRAFIA();
            string url_base="";
            string path ="";

            if(tipo=="1")//visitas
            {
                url_base = (string)webConfigReader.GetValue("url_base_archivos", typeof(string));
                path = (string)webConfigReader.GetValue("ruta_base_Fotovisitas", typeof(string)) + "\\" + Convert.ToInt32(idVisita).ToString("00-00-00").Replace('-', '\\');
            }
            else //formularios
            {
                url_base = (string)webConfigReader.GetValue("url_foto_formulario", typeof(string));

                if (idVisita.Contains("|"))
                {
                    path = (string)webConfigReader.GetValue("ruta_disco_fotoFormulario", typeof(string)) + "\\" + idVisita.Split('|')[0] + "\\" + Convert.ToInt32(idVisita.Split('|')[1]).ToString("00-00-00").Replace('-', '\\');
                } else {
                    path = (string)webConfigReader.GetValue("ruta_disco_fotoFormulario", typeof(string)) + "\\" + Convert.ToInt32(idVisita).ToString("00-00-00").Replace('-', '\\');
                }
            }

            try
            {
                anchoFotografia = Convert.ToInt32(webConfigReader.GetValue("ancho_maximo_fotos", typeof(int)));
            }
            catch
            {
                anchoFotografia = 800;
            }
            
            string filename = "";
             
            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload]))
                    continue;

                verificarRutaAlmacenamiento(path, url_base);


                filename = Path.GetFileName(Request.Files[upload].FileName);
                string[] nombreFoto = filename.Split('.');
                string filenameTMP = nombreFoto[0] + "_TMP." + nombreFoto[1];

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //Request.Files[upload].SaveAs(Path.Combine(path, filename));
                Request.Files[upload].SaveAs(Path.Combine(path, filenameTMP));

                Archivos.ResizeImage(Path.Combine(path, filenameTMP), Path.Combine(path, filename), anchoFotografia, true);

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


            //String url = url_base + idVisita+"/"+ filename;

            String url = Url.Content("~/ControlVigilancia/api/VisitasWebAPI/ObtenerFotografia") + "/" + fotografia.ID_FOTOGRAFIA;

            FotografiaTO ft = new FotografiaTO(fotografia, url);

            return Json(ft);
        }


        public void verificarRutaAlmacenamiento(String ruta_archivos, String url_archivos)
        {


        }

        public JsonResult deleteFile(long id)
        {

            FOTOGRAFIA fotografia = new FOTOGRAFIA();
            fotografia = modeloArchivos.FOTOGRAFIA.Where(f => f.ID_FOTOGRAFIA == id).FirstOrDefault();
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