using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Tala.Controllers
{
    public class guardarFotoController : Controller
    {
        // GET: Tala/guardarFoto
        public ActionResult Index()
        {
            return View();
        }
        private static bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
       public JsonResult getFoto(String intervencion,String codtramite)
        {

           
            AppSettingsReader webConfigReader = new AppSettingsReader();
           
            string url_base = "";
            string path = "";
       
                url_base = (string)webConfigReader.GetValue("url_base_archivos", typeof(string));
                path = (string)webConfigReader.GetValue("ruta_base_Fotovisitas", typeof(string)) + "\\INT\\" + codtramite + "\\" + intervencion;
            
          

           

            string filename = "";
            imagen foto = new imagen();
            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload]))
                    continue;

                filename = Path.GetFileName(Request.Files[upload].FileName);

                string[] nombreFoto = filename.Split('.');
                string filenameTMP = nombreFoto[0] + "_TMP." + nombreFoto[1];
                foto.nombre = filename;
                foto.url = url_base + "/INT/" + codtramite + "/" + intervencion + "/" + filenameTMP;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //Request.Files[upload].SaveAs(Path.Combine(path, filename));
                Request.Files[upload].SaveAs(Path.Combine(path, filenameTMP));

            }
            return Json(foto);
       
        }

    }
    public class imagen
    {
        public String nombre { get; set; }
        public String url { get; set; }


    }
}