using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevExpress.Utils.Extensions;
using SIM.Data;
using SIM.Data.Poeca;

namespace SIM.Areas.Poeca.Controllers
{
    public class AccionesXPlanController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();
        //private ContextoPoeca db = new ContextoPoeca();


        // GET: Poeca/AccionesXPlan
        public ActionResult Index(int? plan)
        {
            ViewBag.MensajeError = TempData["ErrorMessage"];

            var planDB = db.TPOEAIR_PLAN.Find(plan);

            planDB.TPOEAIR_ACCIONES_PLAN = planDB.TPOEAIR_ACCIONES_PLAN
                .OrderBy(x => x.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE)
                .ToList();

            return View(planDB);
        }

        // GET: Poeca/AccionesXPlan/Detalles/5
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_ACCIONES_PLAN tPOEAIR_ACCIONES_PLAN = db.TPOEAIR_ACCIONES_PLAN.Find(id);
            if (tPOEAIR_ACCIONES_PLAN == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // GET: Poeca/AccionesXPlan/Crear
        public ActionResult Crear(int? plan)
        {
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // TODO: Validar que el usuario tenga acceso a ese plan

            ViewBag.ID_NIVEL = new SelectList(db.DPOEAIR_NIVEL, "ID", "S_NOMBRE_NIVEL");
            ViewBag.ID_SECTOR = new SelectList(db.DPOEAIR_SECTOR, "ID", "S_NOMBRE");
            //ViewBag.ID_MEDIDA_ACCION = new SelectList(db.TPOEAIR_MEDIDA_ACCION, "ID", "ID");
            //ViewBag.ID_MEDIDA = new SelectList(db.DPOEAIR_MEDIDA, "ID", "S_NOMBRE_MEDIDA");
            //ViewBag.ID_ACCION = new SelectList(db.DPOEAIR_ACCION, "ID", "S_NOMBRE_ACCION");
            ViewBag.ID_PRODUCTO = new SelectList(db.DPOEAIR_PRODUCTO, "ID", "S_NOMBRE_PRODUCTO");
            ViewBag.ID_PERIODICIDAD = new SelectList(db.TPOEAIR_PERIODICIDAD, "ID", "S_NOMBRE");
            ViewBag.ID_PLAN = plan;
            ViewBag.ANIO = db.TPOEAIR_PLAN.Find(plan).N_ANIO;

            return View();
        }

        // POST: Poeca/AccionesXPlan/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "ID_PLAN,ID_MEDIDA_ACCION,ID_NIVEL,ID_PRODUCTO,ID_PERIODICIDAD,N_META_PROPUESTA,S_RESPONSABLE,S_RECURSOS,N_VALORACION_ECONOMICA,S_OBSERVACIONES")] TPOEAIR_ACCIONES_PLAN nuevaAccionDePlan)
        {
            if (ModelState.IsValid)
            {
                nuevaAccionDePlan.D_FECHA_CREACION = DateTime.Now;
                //nuevaAccionDePlan.
                db.TPOEAIR_ACCIONES_PLAN.Add(nuevaAccionDePlan);
                db.SaveChanges();
                return RedirectToAction("Index", new { plan = nuevaAccionDePlan.ID_PLAN });
            }

            ViewBag.ID_NIVEL = new SelectList(db.DPOEAIR_NIVEL, "ID", "S_NOMBRE_NIVEL");
            //ViewBag.ID_MEDIDA_ACCION = new SelectList(db.TPOEAIR_MEDIDA_ACCION, "ID", "ID");
            ViewBag.ID_SECTOR = new SelectList(db.DPOEAIR_SECTOR, "ID", "S_NOMBRE");
            //ViewBag.ID_MEDIDA = new SelectList(db.DPOEAIR_MEDIDA, "ID", "S_NOMBRE_MEDIDA");
            //ViewBag.ID_ACCION = new SelectList(db.DPOEAIR_ACCION, "ID", "S_NOMBRE_ACCION");
            ViewBag.ID_PRODUCTO = new SelectList(db.DPOEAIR_PRODUCTO, "ID", "S_NOMBRE_PRODUCTO");
            ViewBag.ID_PERIODICIDAD = new SelectList(db.TPOEAIR_PERIODICIDAD, "ID", "S_NOMBRE");
            ViewBag.ID_PLAN = nuevaAccionDePlan.ID_PLAN;
            ViewBag.ANIO = db.TPOEAIR_PLAN.Find(nuevaAccionDePlan.ID_PLAN).N_ANIO;

            return View(nuevaAccionDePlan);
        }

        // GET: Poeca/AccionesXPlan/Editar/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_ACCIONES_PLAN accionAEditar = db.TPOEAIR_ACCIONES_PLAN.Find(id);
            if (accionAEditar == null)
            {
                return HttpNotFound();
            }

            var medidas = ListarMedidasPorSector(accionAEditar.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.ID_SECTOR);
            var acciones = ListarAccionesPorSectorMedida(accionAEditar.TPOEAIR_MEDIDA_ACCION.ID_MEDIDA_SECTOR);

            ViewBag.ID_NIVEL = new SelectList(db.DPOEAIR_NIVEL, "ID", "S_NOMBRE_NIVEL", accionAEditar.ID_NIVEL);
            ViewBag.ID_PRODUCTO = new SelectList(db.DPOEAIR_PRODUCTO, "ID", "S_NOMBRE_PRODUCTO", accionAEditar.ID_PRODUCTO);
            ViewBag.ID_SECTOR = new SelectList(db.DPOEAIR_SECTOR, "ID", "S_NOMBRE", accionAEditar.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.ID_SECTOR);
            ViewBag.ID_MEDIDA = new SelectList(medidas, "Value", "Text", accionAEditar.TPOEAIR_MEDIDA_ACCION.ID_MEDIDA_SECTOR);
            ViewBag.ID_MEDIDA_ACCION = new SelectList(acciones, "Value", "Text", accionAEditar.ID_MEDIDA_ACCION);
            ViewBag.ID_PERIODICIDAD = new SelectList(db.TPOEAIR_PERIODICIDAD, "ID", "S_NOMBRE", accionAEditar.ID_PERIODICIDAD);
            ViewBag.ID_PLAN = accionAEditar.ID_PLAN;
            return View(accionAEditar);
        }

        // POST: Poeca/AccionesXPlan/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ID,ID_PLAN,ID_TERCEROS,ID_MEDIDA_ACCION,ID_NIVEL,ID_PRODUCTO,ID_PERIODICIDAD,N_META_PROPUESTA,S_RESPONSABLE,S_RECURSOS,N_VALORACION_ECONOMICA,S_OBSERVACIONES")] TPOEAIR_ACCIONES_PLAN accionEditada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accionEditada).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { plan = accionEditada.ID_PLAN });
            }

            accionEditada = db.TPOEAIR_ACCIONES_PLAN.Find(accionEditada.ID);

            var medidas = ListarMedidasPorSector(accionEditada.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.ID_SECTOR);
            var acciones = ListarAccionesPorSectorMedida(accionEditada.TPOEAIR_MEDIDA_ACCION.ID_MEDIDA_SECTOR);

            ViewBag.ID_NIVEL = new SelectList(db.DPOEAIR_NIVEL, "ID", "S_NOMBRE_NIVEL", accionEditada.ID_NIVEL);
            ViewBag.ID_PRODUCTO = new SelectList(db.DPOEAIR_PRODUCTO, "ID", "S_NOMBRE_PRODUCTO", accionEditada.ID_PRODUCTO);
            ViewBag.ID_SECTOR = new SelectList(db.DPOEAIR_SECTOR, "ID", "S_NOMBRE", accionEditada.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.ID_SECTOR);
            ViewBag.ID_MEDIDA = new SelectList(medidas, "Value", "Text", accionEditada.TPOEAIR_MEDIDA_ACCION.ID_MEDIDA_SECTOR);
            ViewBag.ID_MEDIDA_ACCION = new SelectList(acciones, "Value", "Text", accionEditada.ID_MEDIDA_ACCION);
            ViewBag.ID_PERIODICIDAD = new SelectList(db.TPOEAIR_PERIODICIDAD, "ID", "S_NOMBRE", accionEditada.ID_PERIODICIDAD);
            ViewBag.ID_PLAN = accionEditada.ID_PLAN;

            return View(accionEditada);
        }

        // GET: Poeca/AccionesXPlan/Eliminar/5
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_ACCIONES_PLAN accionAEliminar = db.TPOEAIR_ACCIONES_PLAN.Find(id);
            if (accionAEliminar == null)
            {
                return HttpNotFound();
            }
            return View(accionAEliminar);
        }

        // POST: Poeca/AccionesXPlan/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            TPOEAIR_ACCIONES_PLAN accionEliminada = db.TPOEAIR_ACCIONES_PLAN.Find(id);

            var idPlan = accionEliminada.ID_PLAN;

            db.TPOEAIR_ACCIONES_PLAN.Remove(accionEliminada);
            db.SaveChanges();

            return RedirectToAction("Index", new { plan = idPlan });
        }
        

        



        [HttpGet]
        public ActionResult MedidasPorSector(int? id)
        {
            var sectorId = id;
            List<SelectListItem> medidas = ListarMedidasPorSector(sectorId);
            return Json(medidas, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> ListarMedidasPorSector(int? sectorId)
        {
            List<SelectListItem> medidas = new List<SelectListItem>();
            if (sectorId != null && sectorId != 0)
            {
                var medidasDB = db.TPOEAIR_SECTOR_MEDIDA.Where(x => x.ID_SECTOR == sectorId).ToList();
                medidasDB.ForEach(x =>
                {
                    medidas.Add(new SelectListItem
                    {
                        Text = x.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA,
                        Value = x.ID.ToString()
                    });
                });
            }

            return medidas;
        }



        [HttpGet]
        public ActionResult AccionesPorSectorMedida(int? id)
        {
            var sectorMedidaId = id;
            List<SelectListItem> acciones = ListarAccionesPorSectorMedida(sectorMedidaId);
            return Json(acciones, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> ListarAccionesPorSectorMedida(int? sectorMedidaId)
        {
            List<SelectListItem> acciones = new List<SelectListItem>();
            if (sectorMedidaId != null && sectorMedidaId != 0)
            {
                var accionesDB = db.TPOEAIR_MEDIDA_ACCION.Where(x => x.ID_MEDIDA_SECTOR == sectorMedidaId).ToList();
                //var accionesDB = db.TPOEAIR_MEDIDA_ACCION.ToList();
                accionesDB.ForEach(x =>
                {
                    acciones.Add(new SelectListItem
                    {
                        Text = x.DPOEAIR_ACCION.S_NOMBRE_ACCION,
                        Value = x.ID.ToString()
                    });
                });
            }

            return acciones;
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
