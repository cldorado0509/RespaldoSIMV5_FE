using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Data;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    public class ActividadEconomicaController : Controller
    {
        //
        // GET: /ActividadEconomica/

        public ActionResult Index()
        {
            return View();
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [ValidateInput(false)]
        public ActionResult gvwAdministrarActividadEconomica()
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA;
            return PartialView("_gvwAdministrarActividadEconomica", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarActividadEconomicaAddNew(ACTIVIDAD_ECONOMICA item)
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwActividadEconomicaPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarActividadEconomicaUpdate(ACTIVIDAD_ECONOMICA item)
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_ACTIVIDADECONOMICA == item.ID_ACTIVIDADECONOMICA);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwActividadEconomicaPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarActividadEconomicaDelete(System.Int32 ID_ACTIVIDADECONOMICA)
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ID_ACTIVIDADECONOMICA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_ACTIVIDADECONOMICA == ID_ACTIVIDADECONOMICA);
                    if (item != null)
                        model.Remove(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwActividadEconomicaPartial", model.ToList());
        }

        [ValidateInput(false)]
        public ActionResult gvwSeleccionarActividadEconomica()
        {
            bool lcbolFiltro = false;
            var model = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.S_VERSION == "4");

            for (int lcintCont = 0; lcintCont < 2; lcintCont++)
            {
                if (Request.Params["gvwActividadesEconomicas$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwActividadesEconomicas$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
                return PartialView("_gvwSeleccionarActividadEconomica", model);
            else
                return PartialView("_gvwSeleccionarActividadEconomica", model.Where(p => p.ID_ACTIVIDADECONOMICA == -10000));
        }

        [ValidateInput(false)]
        public ActionResult tvwSeleccionarActividadEconomica()
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA.Where(ae => ae.S_VERSION == "4");

            return PartialView("_tvwSeleccionarActividadEconomica", model);
        }
    }
}
