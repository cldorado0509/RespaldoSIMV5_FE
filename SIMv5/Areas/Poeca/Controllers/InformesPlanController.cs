using Microsoft.AspNet.Identity;
using SIM.Data;
using SIM.Data.General;
using SIM.Data.Poeca;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Poeca.Controllers
{
    public class InformesPlanController : Controller
    {
        //private ContextoPoeca db = new ContextoPoeca();
        //private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/InformesPlan
        [Authorize]
        public ActionResult Index(int? id, int? tercero)
        {
            if (id == null)
            {
                var episodiosDb = db.DPOEAIR_EPISODIO.ToList();
                var episodios = new List<SelectListItem>();

                foreach (var episodioDb1 in episodiosDb)
                {
                    episodios.Add(new SelectListItem
                    {
                        Text = episodioDb1.DPOEAIR_PERIODO_IMPLEMENTACION.NombrePeriodo + " - " + episodioDb1.N_ANIO,
                        Value = episodioDb1.ID.ToString()
                    });
                }

                ViewBag.EPISODIOS = episodios;
                return View("SeleccionarEpisodio");
            }

            TERCERO terceroDb;
            int userId = int.Parse(User.Identity.GetUserId());
            //terceroDb = UtilidadesPoeca.AdquirirTercero(dbSIM, userId, tercero);
            terceroDb = UtilidadesPoeca.AdquirirTercero(db, userId, tercero);

            if (terceroDb == null)
            {
                if (User.IsInRole("CEPISODIOS"))
                {
                    var tercerosDb = db.DPOEAIR_MUNICIPIOS_TERCEROS.ToList();
                    var terceros = new List<SelectListItem>();

                    foreach (var terceroDb1 in tercerosDb)
                    {
                        terceros.Add(new SelectListItem
                        {
                            Text = terceroDb1.S_NOMBRE,
                            Value = terceroDb1.ID_TERCERO.ToString()
                        });
                    }

                    ViewBag.TERCEROS = terceros;

                    return View("SeleccionarTercero");
                }
                else
                {
                    return HttpNotFound();
                }
            }

            var episodioDb = db.DPOEAIR_EPISODIO.Find(id);
            var plan = db.TPOEAIR_PLAN.Where(x => (x.ID_RESPONSABLE == terceroDb.ID_TERCERO && x.N_ANIO == episodioDb.N_ANIO)).FirstOrDefault();

            var acciones = plan.TPOEAIR_ACCIONES_PLAN;
            var idsAcciones = acciones.Select(x => x.ID);

            var seguimientos = db.TPOEAIR_SEGUIMIENTO_META
                .Where(x => x.ID_EPISODIO == id && idsAcciones.Contains(x.TPOEAIR_ACCIONES_PLAN.ID))
                .GroupBy(x => x.ID_INFO_ACCION).ToList()
                .Select(x => new
                {
                    sector = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE,
                    porcentajeCumplimiento = (double)x.Sum(pc => pc.N_SEGUIMIENTO_META) / x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.N_META_PROPUESTA,
                })
                .GroupBy(x => x.sector)
                .Select(x => new
                {
                    sector = x.FirstOrDefault().sector,
                    porcentajeCumplimiento = x.Sum(pc => pc.porcentajeCumplimiento) * 100/ x.Count(),
                });

            ViewBag.seguimientos = seguimientos;
            return View();
        }
    }
}