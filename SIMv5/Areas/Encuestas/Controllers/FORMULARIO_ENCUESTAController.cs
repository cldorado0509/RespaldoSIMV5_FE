using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Data;
using SIM.Areas.Encuestas.Models;
using SIM.Data.Control;

namespace SIM.Areas.Encuestas.Controllers
{
    public class FORMULARIO_ENCUESTAController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: FORMULARIO_ENCUESTA
        public ActionResult Index()
        {
            //var fORMULARIO_ENCUESTA = db.FORMULARIO_ENCUESTA.Include(f => f.ENC_ENCUESTA).Include(f => f.FORMULARIO);
            //return View(fORMULARIO_ENCUESTA.ToList());
            return null;
        }

        // GET: FORMULARIO_ENCUESTA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA = db.FORMULARIO_ENCUESTA.Find(id);
            if (fORMULARIO_ENCUESTA == null)
            {
                return HttpNotFound();
            }
            return View(fORMULARIO_ENCUESTA);
        }

        // GET: FORMULARIO_ENCUESTA/Create
        public ActionResult Create()
        {
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE");
            ViewBag.ID_FORMULARIO = new SelectList(db.FORMULARIO, "ID_FORMULARIO", "S_NOMBRE");
            return View();
        }

        // POST: FORMULARIO_ENCUESTA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_FORMULARIOENCUESTA,ID_FORMULARIO,ID_ENCUESTA,D_INICIO,D_FIN")] FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA)
        {
            if (ModelState.IsValid)
            {
                db.FORMULARIO_ENCUESTA.Add(fORMULARIO_ENCUESTA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_ENCUESTA);
            ViewBag.ID_FORMULARIO = new SelectList(db.FORMULARIO, "ID_FORMULARIO", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_FORMULARIO);
            return View(fORMULARIO_ENCUESTA);
        }

        // GET: FORMULARIO_ENCUESTA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA = db.FORMULARIO_ENCUESTA.Find(id);
            if (fORMULARIO_ENCUESTA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_ENCUESTA);
            ViewBag.ID_FORMULARIO = new SelectList(db.FORMULARIO, "ID_FORMULARIO", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_FORMULARIO);
            return View(fORMULARIO_ENCUESTA);
        }

        // POST: FORMULARIO_ENCUESTA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_FORMULARIOENCUESTA,ID_FORMULARIO,ID_ENCUESTA,D_INICIO,D_FIN")] FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fORMULARIO_ENCUESTA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_ENCUESTA);
            ViewBag.ID_FORMULARIO = new SelectList(db.FORMULARIO, "ID_FORMULARIO", "S_NOMBRE", fORMULARIO_ENCUESTA.ID_FORMULARIO);
            return View(fORMULARIO_ENCUESTA);
        }

        // GET: FORMULARIO_ENCUESTA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA = db.FORMULARIO_ENCUESTA.Find(id);
            if (fORMULARIO_ENCUESTA == null)
            {
                return HttpNotFound();
            }
            return View(fORMULARIO_ENCUESTA);
        }

        // POST: FORMULARIO_ENCUESTA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FORMULARIO_ENCUESTA fORMULARIO_ENCUESTA = db.FORMULARIO_ENCUESTA.Find(id);
            db.FORMULARIO_ENCUESTA.Remove(fORMULARIO_ENCUESTA);
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
