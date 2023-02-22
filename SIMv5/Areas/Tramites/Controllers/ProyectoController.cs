using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Models;
using SIM.Data;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProyectoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [ValidateInput(false)]
        public ActionResult ProyectosComboGridViewPartial()
        {
            bool lcbolFiltro = false;
            var model = dbSIM.TBPROYECTO;

            for (int lcintCont = 0; lcintCont < 2; lcintCont++)
            {
                if (Request.Params["gvwProyectos$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwProyectos$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
                return PartialView("_ProyectosComboGridViewPartial", model);
            else
                return PartialView("_ProyectosComboGridViewPartial", model.Where(p => p.CODIGO_PROYECTO == -10000).OrderBy(p => p.NOMBRE));
        }

    }
}
