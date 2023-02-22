using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.TalaPoda.Models;
using SIM.Areas.Models;

namespace SIM.Areas.TalaPoda.Controllers
{
    public class INT_INTERVENCIONController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: TalaPoda/INT_INTERVENCION
        public ActionResult Index()
        {
            //var iNT_INTERVENCION = db.INT_INTERVENCION.Include(i => i.FLR_INTERVENCION).Include(i => i.INT_CARACTERISTICAS);
            //return View(iNT_INTERVENCION.ToList());
            return null;
        }

        // GET: TalaPoda/INT_INTERVENCION/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INT_INTERVENCION iNT_INTERVENCION = db.INT_INTERVENCION.Find(id);
            if (iNT_INTERVENCION == null)
            {
                return HttpNotFound();
            }
            return View(iNT_INTERVENCION);
        }

        // GET: TalaPoda/INT_INTERVENCION/Create
        public ActionResult Create()
        {
            ViewBag.ID_TIPO_INTERVENCION = new SelectList(db.FLR_INTERVENCION, "ID_INTERVENCION", "S_INTERVENCION");
            ViewBag.ID_INTERVENCION = new SelectList(db.INT_CARACTERISTICAS, "ID_INTERVENCION", "ID_INTERVENCION");
            return View();
        }

        // POST: TalaPoda/INT_INTERVENCION/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_INTERVENCION,D_FECHA,ID_TIPO_INTERVENCION,ID_ACTOR,S_OBSERVACIONES,S_FICHA,ID_INDIVIDUO_DISPERSO,ID_OPERADOR,D_ACTUALIZACION,L_PROPUESTO,ID_CONTRATO,ID_ESTADO_INT_CONTRATO,ID_USUARIO,ID_INTERVENTOR,FECHA_INGRESO")] INT_INTERVENCION iNT_INTERVENCION)
        {
            if (ModelState.IsValid)
            {
                db.INT_INTERVENCION.Add(iNT_INTERVENCION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_TIPO_INTERVENCION = new SelectList(db.FLR_INTERVENCION, "ID_INTERVENCION", "S_INTERVENCION", iNT_INTERVENCION.ID_TIPO_INTERVENCION);
            ViewBag.ID_INTERVENCION = new SelectList(db.INT_CARACTERISTICAS, "ID_INTERVENCION", "ID_INTERVENCION", iNT_INTERVENCION.ID_INTERVENCION);
            return View(iNT_INTERVENCION);
        }

        // GET: TalaPoda/INT_INTERVENCION/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INT_INTERVENCION iNT_INTERVENCION = db.INT_INTERVENCION.Find(id);
            if (iNT_INTERVENCION == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_TIPO_INTERVENCION = new SelectList(db.FLR_INTERVENCION, "ID_INTERVENCION", "S_INTERVENCION", iNT_INTERVENCION.ID_TIPO_INTERVENCION);
            ViewBag.ID_INTERVENCION = new SelectList(db.INT_CARACTERISTICAS, "ID_INTERVENCION", "ID_INTERVENCION", iNT_INTERVENCION.ID_INTERVENCION);
            return View(iNT_INTERVENCION);
        }

        // POST: TalaPoda/INT_INTERVENCION/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_INTERVENCION,D_FECHA,ID_TIPO_INTERVENCION,ID_ACTOR,S_OBSERVACIONES,S_FICHA,ID_INDIVIDUO_DISPERSO,ID_OPERADOR,D_ACTUALIZACION,L_PROPUESTO,ID_CONTRATO,ID_ESTADO_INT_CONTRATO,ID_USUARIO,ID_INTERVENTOR,FECHA_INGRESO")] INT_INTERVENCION iNT_INTERVENCION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iNT_INTERVENCION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_TIPO_INTERVENCION = new SelectList(db.FLR_INTERVENCION, "ID_INTERVENCION", "S_INTERVENCION", iNT_INTERVENCION.ID_TIPO_INTERVENCION);
            ViewBag.ID_INTERVENCION = new SelectList(db.INT_CARACTERISTICAS, "ID_INTERVENCION", "ID_INTERVENCION", iNT_INTERVENCION.ID_INTERVENCION);
            return View(iNT_INTERVENCION);
        }

        // GET: TalaPoda/INT_INTERVENCION/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INT_INTERVENCION iNT_INTERVENCION = db.INT_INTERVENCION.Find(id);
            if (iNT_INTERVENCION == null)
            {
                return HttpNotFound();
            }
            return View(iNT_INTERVENCION);
        }

        // POST: TalaPoda/INT_INTERVENCION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            INT_INTERVENCION iNT_INTERVENCION = db.INT_INTERVENCION.Find(id);
            db.INT_INTERVENCION.Remove(iNT_INTERVENCION);
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
