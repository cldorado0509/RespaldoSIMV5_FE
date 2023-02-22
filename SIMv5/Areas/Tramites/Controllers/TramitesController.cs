//using DevExpress.Pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Areas.ControlVigilancia.Controllers;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SIM.Areas.Tramites.Models;
using PdfSharp.Drawing;
using System.Web.Script.Serialization;
using SIM.Data;

namespace SIM.Areas.Tramites.Controllers
{
    [Authorize]
    public class TramitesController : Controller
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        Int32 idUsuario;
        decimal codFuncionario;

        // Equivale a Mis Tramites
        public ActionResult Index()
        {
            return MisTramites();
        }

        public ActionResult MisTramites()
        {
            return View();
        }

        public ActionResult BuscarTramites()
        {
            return View();
        }

        /*public ActionResult AvanzaTareaTramite(int codTramite, int? codTarea, int? tipo)
        {
            int? codTareaActual = codTarea ?? 0;

            if (codTareaActual == 0)
            {
                var datos = dbSIM.Database.SqlQuery<int>(
                    "SELECT CODTAREA " +
                    "FROM TRAMITES.TBTRAMITETAREA " +
                    "WHERE CODTRAMITE = " + codTramite.ToString() + " AND ESTADO = 0");

                codTareaActual = datos.FirstOrDefault();
            }

            if (codTareaActual == null)
                return null;
            else
            {
                ViewBag.CodTramites = "";
                ViewBag.CodTramite = codTramite;
                ViewBag.CodTarea = codTareaActual;
                ViewBag.Tipo = tipo ?? 0;

                return View();
            }
        }*/

        // tipo 0: TBDETALLEREGLA, 1: DETALLEREGLA
        public ActionResult AvanzaTareaTramite(string codTramites, int? codTarea, int? tipo, int? restringirResponsable, string origen, string copiaDefecto, int? multiTramites, int? idGrupo)
        {
            multiTramites = (multiTramites ?? 0);
            int? codTareaActual = codTarea ?? 0;

            if (codTramites.Trim().Length > 0)
            {
                //string tramites = String.Join(",", codTramites.Select(x => x.ToString()).ToArray());

                if (codTareaActual == 0 && multiTramites == 0)
                {
                    var datos = dbSIM.Database.SqlQuery<int>(
                        "SELECT DISTINCT CODTAREA " +
                        "FROM TRAMITES.TBTRAMITETAREA " +
                        "WHERE CODTRAMITE IN (" + codTramites + ") AND ESTADO = 0 AND COPIA = 0");

                    //if (datos.Count() != 1) // && ((tipo ?? 0) == 0)) // Quitar comentario para evitar que devuelva una ventana en blanco cuando hay mas de una tarea en los trámites
                    if ((datos.Count() != 1) && ((tipo ?? 0) == 0))
                    {
                        return null;
                    }
                    else
                    {
                        codTareaActual = datos.FirstOrDefault();
                    }
                }

                if (codTareaActual == null)
                    return null;
                else
                {
                    ViewBag.CodTramites = codTramites;
                    ViewBag.CodTarea = codTareaActual;
                    ViewBag.Tipo = tipo ?? 0;
                    ViewBag.RestringirResponsable = restringirResponsable ?? 1;
                    ViewBag.Origen = origen;
                    ViewBag.CopiaDefecto = copiaDefecto;
                    ViewBag.MultiTramites = (multiTramites ?? 0);
                    ViewBag.IdGrupo = (idGrupo ?? -1);

                    return View();
                }
            }
            else
            {
                return null;
            }
        }
    }
}