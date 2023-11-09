using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.GestionDocumental.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MasivosController : Controller
    {
        // GET: GestionDocumental/Masivos
        public ActionResult Index()
        {
            decimal codFuncionario = -1;
            string _PuedeRadicar = "0";
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
                    if (codFuncionario > 0)
                    {
                        var _FuncionariosRadicanMasivos = SIM.Utilidades.Data.ObtenerValorParametro("FuncionariosRadicanMasivos");
                        if (_FuncionariosRadicanMasivos != "")
                        {
                            string[] _Funcionarios = _FuncionariosRadicanMasivos.Split(',');
                            _PuedeRadicar = _Funcionarios.Contains(codFuncionario.ToString()) ? "1" : "0";
                        }
                    }
                }
            }
            catch { }
            ViewBag.CodFuncionario = codFuncionario;
            ViewBag.PuedeRadicar = _PuedeRadicar;
            return View();
        }

        public ActionResult EditarMasivo()
        {
            return View();
        }
    }
}