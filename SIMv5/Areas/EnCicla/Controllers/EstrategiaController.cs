using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Data;
using SIM.Data.EnCicla;

namespace SIM.Areas.EnCicla.Controllers
{
    public class EstrategiaController : Controller
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();

        //
        // GET: /EnCicla/Estrategia/
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult gvwAdministrarEstrategia()
        {
            var model = db.ESTRATEGIA;
            return PartialView("_gvwAdministrarEstrategia", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarEstrategiaAddNew(ESTRATEGIA item)
        {
            var model = db.ESTRATEGIA;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwAdministrarEstrategia", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarEstrategiaUpdate(ESTRATEGIA item)
        {
            var model = db.ESTRATEGIA;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_ESTRATEGIA == item.ID_ESTRATEGIA);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwAdministrarEstrategia", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarEstrategiaDelete(System.Decimal ID_ESTRATEGIA)
        {
            var model = db.ESTRATEGIA;
            if (ID_ESTRATEGIA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_ESTRATEGIA == ID_ESTRATEGIA);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwAdministrarEstrategia", model.ToList());
        }
    }
}