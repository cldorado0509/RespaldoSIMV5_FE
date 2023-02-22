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
    public class FLR_SINTOMA_EFController : Controller
    {
        //SIM.Areas.Models.EntitiesSIMOracle db = new SIM.Areas.Models.EntitiesSIMOracle();
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: TalaPoda/FLR_SINTOMA_EF
        public ActionResult Index()
        {
            return View(db.FLR_SINTOMA_EF.ToList());
        }

        // GET: TalaPoda/FLR_SINTOMA_EF/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FLR_SINTOMA_EF fLR_SINTOMA_EF = db.FLR_SINTOMA_EF.Find(id);
            if (fLR_SINTOMA_EF == null)
            {
                return HttpNotFound();
            }
            return View(fLR_SINTOMA_EF);
        }

        // GET: TalaPoda/FLR_SINTOMA_EF/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TalaPoda/FLR_SINTOMA_EF/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OBJECTID,ID_SINTOMA_EF,S_SINTOMA_EF")] FLR_SINTOMA_EF fLR_SINTOMA_EF)
        {
            if (ModelState.IsValid)
            {
                db.FLR_SINTOMA_EF.Add(fLR_SINTOMA_EF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fLR_SINTOMA_EF);
        }

        // GET: TalaPoda/FLR_SINTOMA_EF/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FLR_SINTOMA_EF fLR_SINTOMA_EF = db.FLR_SINTOMA_EF.Find(id);
            if (fLR_SINTOMA_EF == null)
            {
                return HttpNotFound();
            }
            return View(fLR_SINTOMA_EF);
        }

        // POST: TalaPoda/FLR_SINTOMA_EF/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OBJECTID,ID_SINTOMA_EF,S_SINTOMA_EF")] FLR_SINTOMA_EF fLR_SINTOMA_EF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fLR_SINTOMA_EF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fLR_SINTOMA_EF);
        }

        // GET: TalaPoda/FLR_SINTOMA_EF/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FLR_SINTOMA_EF fLR_SINTOMA_EF = db.FLR_SINTOMA_EF.Find(id);
            if (fLR_SINTOMA_EF == null)
            {
                return HttpNotFound();
            }
            return View(fLR_SINTOMA_EF);
        }

        // POST: TalaPoda/FLR_SINTOMA_EF/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            FLR_SINTOMA_EF fLR_SINTOMA_EF = db.FLR_SINTOMA_EF.Find(id);
            db.FLR_SINTOMA_EF.Remove(fLR_SINTOMA_EF);
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
