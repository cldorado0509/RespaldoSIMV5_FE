﻿namespace SIM.Areas.ControlVigilancia.Controllers
{
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Areas.Seguridad.Class;
    using SIM.Areas.Seguridad.Models;
    using SIM.Data;
    using SIM.Models;
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;
    public class DerechoPeticionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        // GET: ControlVigilancia/Registro
        public ActionResult Registro()
        {
            PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete=false, CanInsert=false, CanPrint=false, CanRead= false, CanUpdate = false, IdRol = 0 };

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            decimal codFuncionario = -1;
            if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);

                Permisos permisos = new Permisos();
                permisosRolModel = permisos.ObtenerPermisosRolForma(10565, idUsuario);
            }

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisosRolModel);
        }
    }
}
