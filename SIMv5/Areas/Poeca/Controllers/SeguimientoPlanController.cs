using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SIM.Data;
using SIM.Data.Poeca;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SIM.Areas.Poeca.Controllers
{
    public class SeguimientoPlanController : Controller
    {
        //private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //private ContextoPoeca db = new ContextoPoeca();
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Seguimiento
        public ActionResult Index(int? episodio, int? tercero)
        {
            if (episodio != null)
            {
                var userId = int.Parse(User.Identity.GetUserId());
                //var terceroDb = UtilidadesPoeca.AdquirirTercero(dbSIM, userId, tercero);
                var terceroDb = UtilidadesPoeca.AdquirirTercero(db, userId, tercero);

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

                var episodioDb = db.DPOEAIR_EPISODIO.Find(episodio);
                var plan = db.TPOEAIR_PLAN.Where(x => (x.ID_RESPONSABLE == terceroDb.ID_TERCERO && x.N_ANIO == episodioDb.N_ANIO)).FirstOrDefault();

                if (plan != null)
                {
                    var seguimientos = db.TPOEAIR_SEGUIMIENTO_META
                                       .Include(t => t.DPOEAIR_EPISODIO)
                                       .Include(t => t.TPOEAIR_ACCIONES_PLAN)
                                       .Where(x => x.ID_EPISODIO == episodio && x.ID_RESPONSABLE == terceroDb.ID_TERCERO);

                    var seguimientosDTO = seguimientos.OrderBy(x => x.TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE)
                        .Select(x => new
                        {
                            x.ID,
                            x.N_SEGUIMIENTO_META,
                            x.N_VALORACION_ECONOMICA,
                            x.S_OBSERVACIONES,
                            x.ID_INFO_ACCION,
                            x.ID_EPISODIO,
                            ANIO_EPISODIO = x.DPOEAIR_EPISODIO.N_ANIO,
                            x.D_FECHA_ACTUALIZACION
                        })
                        .ToList();

                    var accionesDTO = plan.TPOEAIR_ACCIONES_PLAN
                        .Select(x => new
                        {
                            id = x.ID,
                            nivel = x.DPOEAIR_NIVEL.S_NOMBRE_NIVEL,
                            producto = x.DPOEAIR_PRODUCTO.S_NOMBRE_PRODUCTO,
                            periodicidad = x.TPOEAIR_PERIODICIDAD.S_NOMBRE,
                            metaPropuesta = x.N_META_PROPUESTA,
                            cargoResponsable = x.S_RESPONSABLE,
                            valoracionEconomica = x.N_VALORACION_ECONOMICA,
                            otrosRecursos = x.S_RECURSOS,
                            accion = x.TPOEAIR_MEDIDA_ACCION.DPOEAIR_ACCION.S_NOMBRE_ACCION,
                            sector = x.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE,
                            medida = x.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA
                        })
                        .OrderBy(x => x.sector)
                        .ToList();

                    var accionesDisponibles = accionesDTO.Where(x => !seguimientos.Any(s => s.ID_INFO_ACCION == x.id));

                    ViewBag.acciones = JsonConvert.SerializeObject(accionesDTO);
                    ViewBag.accionesDisponibles = JsonConvert.SerializeObject(accionesDisponibles);
                    ViewBag.seguimientos = JsonConvert.SerializeObject(seguimientosDTO);
                    ViewBag.episodio = episodio;

                    return View(seguimientos.ToList());
                }
                return RedirectToAction("Index", "Planes");
            }
            else
            {
                var episodiosDb = db.DPOEAIR_EPISODIO.ToList();
                var episodios = new List<SelectListItem>();

                foreach (var episodioDb in episodiosDb)
                {
                    episodios.Add(new SelectListItem
                    {
                        Text = episodioDb.DPOEAIR_PERIODO_IMPLEMENTACION.NombrePeriodo + " - " + episodioDb.N_ANIO,
                        Value = episodioDb.ID.ToString()
                    });
                }

                ViewBag.EPISODIOS = episodios;

                return View("SeleccionarEpisodio");
            }
        }

        // GET: Poeca/Seguimiento/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_SEGUIMIENTO_META tPOEAIR_SEGUIMIENTO_META = db.TPOEAIR_SEGUIMIENTO_META.Find(id);
            if (tPOEAIR_SEGUIMIENTO_META == null)
            {
                return HttpNotFound();
            }
            return View(tPOEAIR_SEGUIMIENTO_META);
        }

        // GET: Poeca/Seguimiento/Crear
        public ActionResult Crear()
        {
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo");
            ViewBag.ID_INFO_ACCION = new SelectList(db.TPOEAIR_ACCIONES_PLAN, "ID", "S_RESPONSABLE");
            return View();
        }

        // POST: Poeca/Seguimiento/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Crear([Bind(Include = "ID_INFO_ACCION,ID_EPISODIO,N_SEGUIMIENTO_META,N_VALORACION_ECONOMICA,S_OBSERVACIONES")] TPOEAIR_SEGUIMIENTO_META seguimientoCreado)
        {
            if (ModelState.IsValid)
            {
                var userId = Int32.Parse(User.Identity.GetUserId());
                //var tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, userId);
                var tercero = UtilidadesPoeca.AdquirirTercero(db, userId);

                seguimientoCreado.D_FECHA_ACTUALIZACION = DateTime.Now;
                seguimientoCreado.ID_RESPONSABLE = tercero.ID_TERCERO;
                db.TPOEAIR_SEGUIMIENTO_META.Add(seguimientoCreado);
                db.SaveChanges();
                return Json(seguimientoCreado);
            }
            Response.StatusCode = 400;
            return Json(null);
        }

        // GET: Poeca/Seguimiento/Editar/5
        public ActionResult Editar(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_SEGUIMIENTO_META seguimientoAEditar = db.TPOEAIR_SEGUIMIENTO_META.Find(id);
            if (seguimientoAEditar == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo", seguimientoAEditar.ID_EPISODIO);
            ViewBag.ID_INFO_ACCION = new SelectList(db.TPOEAIR_ACCIONES_PLAN, "ID", "S_RESPONSABLE", seguimientoAEditar.ID_INFO_ACCION);
            return View(seguimientoAEditar);
        }

        // POST: Poeca/Seguimiento/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ID,ID_INFO_ACCION,ID_EPISODIO,N_SEGUIMIENTO_META,N_VALORACION_ECONOMICA,S_OBSERVACIONES")] TPOEAIR_SEGUIMIENTO_META seguimientoEditado)
        {
            if (ModelState.IsValid)
            {
                seguimientoEditado.D_FECHA_ACTUALIZACION = DateTime.Now;
                db.Entry(seguimientoEditado).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(seguimientoEditado);
            }
            Response.StatusCode = 400;
            return Json(null);
        }

        // GET: Poeca/Seguimiento/Editar/5
        public ActionResult Eliminar(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_SEGUIMIENTO_META tPOEAIR_SEGUIMIENTO_META = db.TPOEAIR_SEGUIMIENTO_META.Find(id);
            if (tPOEAIR_SEGUIMIENTO_META == null)
            {
                return HttpNotFound();
            }
            return View(tPOEAIR_SEGUIMIENTO_META);
        }

        // POST: Poeca/Seguimiento/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        //[ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmed(decimal id)
        {
            TPOEAIR_SEGUIMIENTO_META tPOEAIR_SEGUIMIENTO_META = db.TPOEAIR_SEGUIMIENTO_META.Find(id);
            db.TPOEAIR_SEGUIMIENTO_META.Remove(tPOEAIR_SEGUIMIENTO_META);
            db.SaveChanges();
            return Json(true);
        }

        public JsonResult Informe(int? id, int? idTercero)
        {
            var userId = Int32.Parse(User.Identity.GetUserId());
            //var tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, userId, idTercero);
            var tercero = UtilidadesPoeca.AdquirirTercero(db, userId, idTercero);

            var episodioDb = db.DPOEAIR_EPISODIO.Find(id);
            var plan = db.TPOEAIR_PLAN.Where(x => (x.ID_RESPONSABLE == tercero.ID_TERCERO && x.N_ANIO == episodioDb.N_ANIO)).FirstOrDefault();

            var acciones = plan.TPOEAIR_ACCIONES_PLAN;
            var idsAcciones = acciones.Select(x => x.ID);

            var seguimientos = db.TPOEAIR_SEGUIMIENTO_META
                .Where(x => x.ID_EPISODIO == id && idsAcciones.Contains(x.TPOEAIR_ACCIONES_PLAN.ID))
                .GroupBy(x => x.ID_INFO_ACCION).ToList()
                .Select(x => new
                {
                    sector = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE,
                    medida = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA,
                    accion = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.DPOEAIR_ACCION.S_NOMBRE_ACCION,
                    meta = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.N_META_PROPUESTA,
                    seguimiento = x.Sum(sm => sm.N_SEGUIMIENTO_META),
                    fechas = x.Min(f => f.D_FECHA_ACTUALIZACION) + " - " + x.Max(f => f.D_FECHA_ACTUALIZACION),
                    valoracionEconomica = x.Sum(ve => ve.N_VALORACION_ECONOMICA),
                    observaciones = string.Join("\r\n\r\n", x.Select(o => o.S_OBSERVACIONES)),
                });

            return Json(seguimientos.ToList(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
