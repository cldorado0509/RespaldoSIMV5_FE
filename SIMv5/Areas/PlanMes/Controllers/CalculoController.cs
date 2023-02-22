using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ControlVigilancia.Controllers;

using System.Configuration;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Security.Claims;
using SIM.Areas.Seguridad.Controllers;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.PlanMes.Controllers
{
    public class CalculoController : Controller
    {
        EntitiesControlOracle db = new EntitiesControlOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        // GET: planMes/calculo
        public ActionResult Index()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }

            //String sql = "Select REPLACE(S_NOMBRE, '\"', '') AS S_NOMBRE from VW_INSTALACION  where ID_TERCERO=" + idUsuario;
            String sql = "SELECT S_NOMBRE FROM VW_INSTALACION WHERE ID_TERCERO="+ idUsuario;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            ViewBag.json =jSONOUT.Value.ToString();
           
            return View();
        }

 
    }
}
