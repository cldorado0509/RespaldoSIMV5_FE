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
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data.Control;

namespace SIM.Areas.Encuestas.Controllers
{
    public class FORMULARIOsController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: FORMULARIOs
        public ActionResult Index()
        {
            return View(db.FORMULARIO.ToList());
        }

        // GET: FORMULARIOs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO fORMULARIO = db.FORMULARIO.Find(id);
            if (fORMULARIO == null)
            {
                return HttpNotFound();
            }
            return View(fORMULARIO);
        }

        // GET: FORMULARIOs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FORMULARIOs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_FORMULARIO,S_NOMBRE")] FORMULARIO fORMULARIO)
        {
            if (ModelState.IsValid)
            {
                db.FORMULARIO.Add(fORMULARIO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fORMULARIO);
        }

        // GET: FORMULARIOs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO fORMULARIO = db.FORMULARIO.Find(id);
            if (fORMULARIO == null)
            {
                return HttpNotFound();
            }
            return View(fORMULARIO);
        }

        // POST: FORMULARIOs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_FORMULARIO,S_NOMBRE")] FORMULARIO fORMULARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fORMULARIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fORMULARIO);
        }

        // GET: FORMULARIOs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FORMULARIO fORMULARIO = db.FORMULARIO.Find(id);
            if (fORMULARIO == null)
            {
                return HttpNotFound();
            }
            return View(fORMULARIO);
        }

        // POST: FORMULARIOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FORMULARIO fORMULARIO = db.FORMULARIO.Find(id);
            db.FORMULARIO.Remove(fORMULARIO);
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
