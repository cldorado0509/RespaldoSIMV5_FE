using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.Models;
using SIM.Areas.Encuestas.Models;

namespace SIM.Areas.Encuestas.Controllers
{
    public class SOLUCIONsController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: SOLUCIONs
        public ActionResult Index()
        {
            var sOLUCIONs = db.SOLUCIONs.Include(s => s.ENCUESTA).Include(s => s.OBJETO_CONTROL).Include(s => s.PREGUNTA).Include(s => s.RESPUESTA);
            return View(sOLUCIONs.ToList());
        }

        // GET: SOLUCIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOLUCION sOLUCION = db.SOLUCIONs.Find(id);
            if (sOLUCION == null)
            {
                return HttpNotFound();
            }
            return View(sOLUCION);
        }

        // GET: SOLUCIONs/Create
        public ActionResult Create()
        {
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE");
            ViewBag.ID_OBJETOCONTROL = new SelectList(db.OBJETO_CONTROL, "ID_OBJETOCONTROL", "S_NOMBRETABLA");
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE");
            ViewBag.ID_RESPUESTA = new SelectList(db.RESPUESTAs, "ID_RESPUESTA", "S_VALOR");
            return View();
        }

        // POST: SOLUCIONs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_SOLUCION,ID_OBJETOCONTROL,ID_ENCUESTA,ID_PREGUNTA,ID_RESPUESTA,S_TIPOTEXTO,N_TIPONUMERO,D_TIPOFECHA,S_OBSERVACION,D_SOLUCION")] SOLUCION sOLUCION)
        {
            if (ModelState.IsValid)
            {
                db.SOLUCIONs.Add(sOLUCION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", sOLUCION.ID_ENCUESTA);
            ViewBag.ID_OBJETOCONTROL = new SelectList(db.OBJETO_CONTROL, "ID_OBJETOCONTROL", "S_NOMBRETABLA", sOLUCION.ID_OBJETOCONTROL);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", sOLUCION.ID_PREGUNTA);
            ViewBag.ID_RESPUESTA = new SelectList(db.RESPUESTAs, "ID_RESPUESTA", "S_VALOR", sOLUCION.ID_RESPUESTA);
            return View(sOLUCION);
        }

        // GET: SOLUCIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOLUCION sOLUCION = db.SOLUCIONs.Find(id);
            if (sOLUCION == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", sOLUCION.ID_ENCUESTA);
            ViewBag.ID_OBJETOCONTROL = new SelectList(db.OBJETO_CONTROL, "ID_OBJETOCONTROL", "S_NOMBRETABLA", sOLUCION.ID_OBJETOCONTROL);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", sOLUCION.ID_PREGUNTA);
            ViewBag.ID_RESPUESTA = new SelectList(db.RESPUESTAs, "ID_RESPUESTA", "S_VALOR", sOLUCION.ID_RESPUESTA);
            return View(sOLUCION);
        }

        // POST: SOLUCIONs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_SOLUCION,ID_OBJETOCONTROL,ID_ENCUESTA,ID_PREGUNTA,ID_RESPUESTA,S_TIPOTEXTO,N_TIPONUMERO,D_TIPOFECHA,S_OBSERVACION,D_SOLUCION")] SOLUCION sOLUCION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sOLUCION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_ENCUESTA = new SelectList(db.ENCUESTAs, "ID_ENCUESTA", "S_NOMBRE", sOLUCION.ID_ENCUESTA);
            ViewBag.ID_OBJETOCONTROL = new SelectList(db.OBJETO_CONTROL, "ID_OBJETOCONTROL", "S_NOMBRETABLA", sOLUCION.ID_OBJETOCONTROL);
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTAs, "ID_PREGUNTA", "S_NOMBRE", sOLUCION.ID_PREGUNTA);
            ViewBag.ID_RESPUESTA = new SelectList(db.RESPUESTAs, "ID_RESPUESTA", "S_VALOR", sOLUCION.ID_RESPUESTA);
            return View(sOLUCION);
        }

        // GET: SOLUCIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOLUCION sOLUCION = db.SOLUCIONs.Find(id);
            if (sOLUCION == null)
            {
                return HttpNotFound();
            }
            return View(sOLUCION);
        }

        // POST: SOLUCIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SOLUCION sOLUCION = db.SOLUCIONs.Find(id);
            db.SOLUCIONs.Remove(sOLUCION);
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
