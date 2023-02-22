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

namespace SIM.Areas.Encuestas.Controllers
{
    public class ENCUESTA_PREGUNTAController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: ENCUESTA_PREGUNTA
        public ActionResult Index()
        {
            var eNCUESTA_PREGUNTA = db.ENCUESTA_PREGUNTA.Include(e => e.ENC_ENCUESTA).Include(e => e.ENC_PREGUNTA);
            return View(eNCUESTA_PREGUNTA.ToList());
        }

        // GET: ENCUESTA_PREGUNTA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA = db.ENCUESTA_PREGUNTA.Find(id);
            if (eNCUESTA_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(eNCUESTA_PREGUNTA);
        }

        // GET: ENCUESTA_PREGUNTA/Create
        public ActionResult Create()
        {
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE");
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE");
            return View();
        }

        // POST: ENCUESTA_PREGUNTA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ENCUESTAPREGUNTA,ID_ENCUESTA,ID_PREGUNTA,N_ORDEN,N_FACTOR,S_REQUERIDA")] ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA)
        {
            if (ModelState.IsValid)
            {
                db.ENCUESTA_PREGUNTA.Add(eNCUESTA_PREGUNTA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_ENCUESTA);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_PREGUNTA);
            return View(eNCUESTA_PREGUNTA);
        }

        // GET: ENCUESTA_PREGUNTA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA = db.ENCUESTA_PREGUNTA.Find(id);
            if (eNCUESTA_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_ENCUESTA);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_PREGUNTA);
            return View(eNCUESTA_PREGUNTA);
        }

        // POST: ENCUESTA_PREGUNTA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ENCUESTAPREGUNTA,ID_ENCUESTA,ID_PREGUNTA,N_ORDEN,N_FACTOR,S_REQUERIDA")] ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eNCUESTA_PREGUNTA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_ENCUESTA);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", eNCUESTA_PREGUNTA.ID_PREGUNTA);
            return View(eNCUESTA_PREGUNTA);
        }

        // GET: ENCUESTA_PREGUNTA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA = db.ENCUESTA_PREGUNTA.Find(id);
            if (eNCUESTA_PREGUNTA == null)
            {
                return HttpNotFound();
            }
            return View(eNCUESTA_PREGUNTA);
        }

        // POST: ENCUESTA_PREGUNTA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ENCUESTA_PREGUNTA eNCUESTA_PREGUNTA = db.ENCUESTA_PREGUNTA.Find(id);
            db.ENCUESTA_PREGUNTA.Remove(eNCUESTA_PREGUNTA);
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
