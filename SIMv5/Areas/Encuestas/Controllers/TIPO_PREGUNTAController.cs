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
    public class TIPO_PREGUNTAController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();
        //private EncuestasSIM db = new EncuestasSIM();

        // GET: TIPO_PREGUNTA
        public ActionResult Index()
        { 
            return View(db.TIPO_PREGUNTA.ToList());
        }

        // GET: TIPO_PREGUNTA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PREGUNTA tIPO_PREGUNTA = db.TIPO_PREGUNTA.Find(id);
            if (tIPO_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PREGUNTA);
        }

        // GET: TIPO_PREGUNTA/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TIPO_PREGUNTA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_TIPOPREGUNTA,S_NOMBRE")] TIPO_PREGUNTA tIPO_PREGUNTA)
        {
            if (ModelState.IsValid)
            {
                db.TIPO_PREGUNTA.Add(tIPO_PREGUNTA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tIPO_PREGUNTA);
        }

        // GET: TIPO_PREGUNTA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PREGUNTA tIPO_PREGUNTA = db.TIPO_PREGUNTA.Find(id);
            if (tIPO_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PREGUNTA);
        }

        // POST: TIPO_PREGUNTA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_TIPOPREGUNTA,S_NOMBRE")] TIPO_PREGUNTA tIPO_PREGUNTA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPO_PREGUNTA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tIPO_PREGUNTA);
        }

        // GET: TIPO_PREGUNTA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_PREGUNTA tIPO_PREGUNTA = db.TIPO_PREGUNTA.Find(id);
            if (tIPO_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_PREGUNTA);
        }

        // POST: TIPO_PREGUNTA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIPO_PREGUNTA tIPO_PREGUNTA = db.TIPO_PREGUNTA.Find(id);
            db.TIPO_PREGUNTA.Remove(tIPO_PREGUNTA);
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
