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
    public class INV_INDIVIDUO_DISPERSOController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: /TalaPoda/INV_INDIVIDUO_DISPERSO/
        public ActionResult Index()
        {
            var inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Include(i => i.FLR_ESPECIE).Include(i => i.INV_ESTADO_INDIVIDUO);
            return View(inv_individuo_disperso.ToList());
        }

        // GET: /TalaPoda/INV_INDIVIDUO_DISPERSO/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INV_INDIVIDUO_DISPERSO inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Find(id);
            if (inv_individuo_disperso == null)
            {
                return HttpNotFound();
            }
            return View(inv_individuo_disperso);
        }

        // GET: /TalaPoda/INV_INDIVIDUO_DISPERSO/Create
        public ActionResult Create()
        {
            ViewBag.ID_ESPECIE = new SelectList(db.FLR_ESPECIE, "ID_ESPECIE", "S_GENERO");
            ViewBag.ID_ESTADO = new SelectList(db.INV_ESTADO_INDIVIDUO, "ID_ESTADO", "S_ESTADO");
            return View();
        }

        // POST: /TalaPoda/INV_INDIVIDUO_DISPERSO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OBJECTID,ID_INDIVIDUO_DISPERSO,ID_PU_INVEN,ID_ESPECIE,ID_LOCALIZACION_ARBOL,ID_EDAD_SIEMBRA,ID_ESTADO,ID_PROCEDENCIA,ID_UBICACION,COD_INDIVIDUO_DISPERSO,L_SETO,N_INDIVIDUOS_SETO,N_LADO_MANZANA,N_ORDEN,ID_INDIVIDUO_DISPERSO_ORIGEN,D_INGRESO,D_ACTUALIZACION,ID_TIPO_ARBOL")] INV_INDIVIDUO_DISPERSO inv_individuo_disperso)
        {
            if (ModelState.IsValid)
            {
                db.INV_INDIVIDUO_DISPERSO.Add(inv_individuo_disperso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ESPECIE = new SelectList(db.FLR_ESPECIE, "ID_ESPECIE", "S_GENERO", inv_individuo_disperso.ID_ESPECIE);
            ViewBag.ID_ESTADO = new SelectList(db.INV_ESTADO_INDIVIDUO, "ID_ESTADO", "S_ESTADO", inv_individuo_disperso.ID_ESTADO);
            return View(inv_individuo_disperso);
        }

        // GET: /TalaPoda/INV_INDIVIDUO_DISPERSO/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INV_INDIVIDUO_DISPERSO inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Find(id);
            if (inv_individuo_disperso == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_ESPECIE = new SelectList(db.FLR_ESPECIE, "ID_ESPECIE", "S_GENERO", inv_individuo_disperso.ID_ESPECIE);
            ViewBag.ID_ESTADO = new SelectList(db.INV_ESTADO_INDIVIDUO, "ID_ESTADO", "S_ESTADO", inv_individuo_disperso.ID_ESTADO);
            return View(inv_individuo_disperso);
        }

				public ActionResult Replica(decimal id)
				{
					if (id == null)
					{
						return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
					}
					INV_INDIVIDUO_DISPERSO inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Find(id);
					if (inv_individuo_disperso == null)
					{
						return HttpNotFound();
					}
					ViewBag.ID_ESPECIE = new SelectList(db.FLR_ESPECIE, "ID_ESPECIE", "S_GENERO", inv_individuo_disperso.ID_ESPECIE);
					ViewBag.ID_ESTADO = new SelectList(db.INV_ESTADO_INDIVIDUO, "ID_ESTADO", "S_ESTADO", inv_individuo_disperso.ID_ESTADO);
					return View(inv_individuo_disperso);
				}

        // POST: /TalaPoda/INV_INDIVIDUO_DISPERSO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OBJECTID,ID_INDIVIDUO_DISPERSO,ID_PU_INVEN,ID_ESPECIE,ID_LOCALIZACION_ARBOL,ID_EDAD_SIEMBRA,ID_ESTADO,ID_PROCEDENCIA,ID_UBICACION,COD_INDIVIDUO_DISPERSO,L_SETO,N_INDIVIDUOS_SETO,N_LADO_MANZANA,N_ORDEN,ID_INDIVIDUO_DISPERSO_ORIGEN,D_INGRESO,D_ACTUALIZACION,ID_TIPO_ARBOL")] INV_INDIVIDUO_DISPERSO inv_individuo_disperso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_individuo_disperso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_ESPECIE = new SelectList(db.FLR_ESPECIE, "ID_ESPECIE", "S_GENERO", inv_individuo_disperso.ID_ESPECIE);
            ViewBag.ID_ESTADO = new SelectList(db.INV_ESTADO_INDIVIDUO, "ID_ESTADO", "S_ESTADO", inv_individuo_disperso.ID_ESTADO);
            return View(inv_individuo_disperso);
        }

        // GET: /TalaPoda/INV_INDIVIDUO_DISPERSO/Delete/5
        public ActionResult Delete(decimal ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INV_INDIVIDUO_DISPERSO inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Find(id);
            if (inv_individuo_disperso == null)
            {
                return HttpNotFound();
            }
            return View(inv_individuo_disperso);
        }

        // POST: /TalaPoda/INV_INDIVIDUO_DISPERSO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal ? id)
        {
            INV_INDIVIDUO_DISPERSO inv_individuo_disperso = db.INV_INDIVIDUO_DISPERSO.Find(id);
            db.INV_INDIVIDUO_DISPERSO.Remove(inv_individuo_disperso);
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
