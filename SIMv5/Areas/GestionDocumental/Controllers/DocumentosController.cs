using SIM.Areas.Seguridad.Models;
using SIM.Areas.Seguridad.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.GestionDocumental.Controllers
{
    public class DocumentosController : Controller
    {
        // GET: GestionDocumental/Documentos
        public ActionResult BuscarDocumentos()
        {
            PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete = false, CanInsert = false, CanPrint = false, CanRead = false, CanUpdate = false, IdRol = 0 };

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);

                Permisos permisos = new Permisos();
                permisosRolModel = permisos.ObtenerPermisosRolForma(610, idUsuario);
                ViewBag.Edit = permisosRolModel.CanUpdate ? "Y" : "N";
                ViewBag.CodFuncionario = codFuncionario;

            }
            return View();
        }
    }
}