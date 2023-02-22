using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SIM.Data;
using SIM.Data.Poeca;

namespace SIM.Areas.Poeca
{
    public class PeriodoImplementacionController : Controller
    {
        //private ContextoPoeca db = new ContextoPoeca(); 
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/PeriodosImplementacion
        [Authorize(Roles = "VPERIODOIMPLEMENTACION")]
        public ActionResult Index()
        {
            return View(db.PeriodoImplementacion.ToList());
        }

        // GET: Poeca/PeriodosImplementacion/Details/5
        [Authorize(Roles = "VPERIODOIMPLEMENTACION")]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodoImplementacion periodoImplementacion = db.PeriodoImplementacion.Find(id);
            if (periodoImplementacion == null)
            {
                return HttpNotFound();
            }
            return View(periodoImplementacion);
        }

        // GET: Poeca/PeriodosImplementacion/Create
        [Authorize(Roles = "CPERIODOIMPLEMENTACION")]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Poeca/PeriodosImplementacion/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CPERIODOIMPLEMENTACION")]
        public ActionResult Crear([Bind(Include = "Id,NombrePeriodo,Descripcion,IdResponsable")] PeriodoImplementacion periodoImplementacion)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();
                periodoImplementacion.IdResponsable = Int32.Parse(userId);

                db.PeriodoImplementacion.Add(periodoImplementacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(periodoImplementacion);
        }

        // GET: Poeca/PeriodosImplementacion/Edit/5
        [Authorize(Roles = "APERIODOIMPLEMENTACION")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodoImplementacion periodoImplementacion = db.PeriodoImplementacion.Find(id);
            if (periodoImplementacion == null)
            {
                return HttpNotFound();
            }
            return View(periodoImplementacion);
        }

        // POST: Poeca/PeriodosImplementacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "APERIODOIMPLEMENTACION")]
        public ActionResult Editar([Bind(Include = "Id,NombrePeriodo,Descripcion,IdResponsable")] PeriodoImplementacion periodoImplementacion)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                periodoImplementacion.IdResponsable = Int32.Parse(userId);

                db.Entry(periodoImplementacion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(periodoImplementacion);
        }

        // GET: Poeca/PeriodosImplementacion/Delete/5
        [Authorize(Roles = "EPERIODOIMPLEMENTACION")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodoImplementacion periodoImplementacion = db.PeriodoImplementacion.Find(id);
            if (periodoImplementacion == null)
            {
                return HttpNotFound();
            }
            return View(periodoImplementacion);
        }

        // POST: Poeca/PeriodosImplementacion/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EPERIODOIMPLEMENTACION")]
        public ActionResult DeleteConfirmed(int id)
        {
            PeriodoImplementacion periodoImplementacion = db.PeriodoImplementacion.Find(id);
            db.PeriodoImplementacion.Remove(periodoImplementacion);
            db.SaveChanges();
            return RedirectToAction("Index");
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
