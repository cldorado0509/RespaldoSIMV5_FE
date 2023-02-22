using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
namespace SIM.Areas.Sau.Controllers
{
    public class cargarArchivoController : Controller
    {
        //
        public JsonResult Index(String etiqueta)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();
           

           
            string path = (string)webConfigReader.GetValue("ruta_base_archivosExcel", typeof(string));
            string filename = "";

            foreach (string upload in Request.Files)
            {
             


                filename = Path.GetFileName(Request.Files[upload].FileName);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                Request.Files[upload].SaveAs(Path.Combine(path, filename));

        

            }





            return Json(path + "\\" + filename);
        }

        
	}
}