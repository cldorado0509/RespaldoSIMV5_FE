using System.Linq;
using System.Web.Mvc;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class MemorandosController : Controller
    {

        // GET: GestionDocumental/Memorandos
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}