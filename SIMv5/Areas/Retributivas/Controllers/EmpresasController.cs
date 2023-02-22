using SIM.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExtreme;


namespace SIM.Areas.Retributivas.Controllers
{
    public class EmpresasController : Controller
    {

        EntitiesControlOracle db = new EntitiesControlOracle();

        // GET: Retributivas/Empresas
        public ActionResult Index()
        {
            return View();
        }


    }
}