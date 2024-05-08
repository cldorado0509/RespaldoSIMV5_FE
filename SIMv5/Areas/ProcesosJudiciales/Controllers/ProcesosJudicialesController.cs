using SIM.Data;
using System.Web.Mvc;

namespace SIM.Areas.ProcesosJudiciales.Controllers
{
    public class ProcesosJudicialesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //[Authorize(Roles = "VPROCESOSJUDICIALES")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
