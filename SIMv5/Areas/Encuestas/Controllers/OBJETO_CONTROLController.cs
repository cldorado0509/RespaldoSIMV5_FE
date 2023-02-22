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
    public class OBJETO_CONTROLController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();
        

        // GET: OBJETO_CONTROL
        public ActionResult Index()
        {
            return View(db.OBJETO_CONTROL.ToList());
        }

        // GET: OBJETO_CONTROL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBJETO_CONTROL oBJETO_CONTROL = db.OBJETO_CONTROL.Find(id);
            if (oBJETO_CONTROL == null)
            {
                return HttpNotFound();
            }
            return View(oBJETO_CONTROL);
        }

        // GET: OBJETO_CONTROL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OBJETO_CONTROL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_OBJETOCONTROL,S_NOMBRETABLA,S_NOMBRECAMPO,S_CODIGOOBJETO,ID_VISITA")] OBJETO_CONTROL oBJETO_CONTROL)
        {
            if (ModelState.IsValid)
            {
                db.OBJETO_CONTROL.Add(oBJETO_CONTROL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oBJETO_CONTROL);
        }

        // GET: OBJETO_CONTROL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBJETO_CONTROL oBJETO_CONTROL = db.OBJETO_CONTROL.Find(id);
            if (oBJETO_CONTROL == null)
            {
                return HttpNotFound();
            }
            return View(oBJETO_CONTROL);
        }

        // POST: OBJETO_CONTROL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_OBJETOCONTROL,S_NOMBRETABLA,S_NOMBRECAMPO,S_CODIGOOBJETO,ID_VISITA")] OBJETO_CONTROL oBJETO_CONTROL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oBJETO_CONTROL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oBJETO_CONTROL);
        }

        // GET: OBJETO_CONTROL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OBJETO_CONTROL oBJETO_CONTROL = db.OBJETO_CONTROL.Find(id);
            if (oBJETO_CONTROL == null)
            {
                return HttpNotFound();
            }
            return View(oBJETO_CONTROL);
        }

        // POST: OBJETO_CONTROL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OBJETO_CONTROL oBJETO_CONTROL = db.OBJETO_CONTROL.Find(id);
            db.OBJETO_CONTROL.Remove(oBJETO_CONTROL);
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
