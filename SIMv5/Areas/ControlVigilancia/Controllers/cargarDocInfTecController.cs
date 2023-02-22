using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class cargarDocInfTecController : Controller
    {
        //
        public JsonResult Index(int Id)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();



            string path = (string)webConfigReader.GetValue("ruta_base_documentoInfTec", typeof(string));
            string filename = "";
            path += "\\" + Id + "\\";
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