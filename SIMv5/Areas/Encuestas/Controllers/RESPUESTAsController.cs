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
    public class RESPUESTAsController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: RESPUESTAs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtenerRespuestasxPregunta(int CodigoPregunta)
        {
            System.Collections.Generic.List<RESPUESTA> Respuestas = db.RESPUESTA.Where(R => R.ID_PREGUNTA == CodigoPregunta).ToList();

            return Json((from obj in Respuestas select new { Orden = obj.N_ORDEN, Codigo = obj.S_CODIGO, Valor = obj.S_VALOR, Consecutivo = obj.ID_RESPUESTA }), System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        // GET: RESPUESTAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESPUESTA rESPUESTA = db.RESPUESTA.Find(id);
            if (rESPUESTA == null)
            {
                return HttpNotFound();
            }
            return View(rESPUESTA);
        }

        // GET: RESPUESTAs/Create
        public ActionResult Create(int CodigoPregunta)
        {
            ViewBag.CodigoPregunta = CodigoPregunta;
            return View();
        }

        // POST: RESPUESTAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_RESPUESTA,ID_PREGUNTA,S_VALOR,S_CODIGO,N_ORDEN")] RESPUESTA rESPUESTA)
        {
            bool Error = false;
            int CodigoPregunta = -1;

            try
            {
                CodigoPregunta = int.Parse(Request["CodigoPregunta"].ToString());
            }
            catch { }

            if (CodigoPregunta == -1)
            {
                ModelState.AddModelError("", "No se hallo el codigo de la pregunta.");
                Error = true;
            }

            if (ModelState.IsValid)
            {
                if (rESPUESTA.S_VALOR == null)
                {
                    ModelState.AddModelError("S_VALOR", "El valor es requerido.");
                    Error = true;
                }

                if (rESPUESTA.S_CODIGO == null)
                {
                    ModelState.AddModelError("S_CODIGO", "El codigo es requerido.");
                    Error = true;
                }

                if (rESPUESTA.N_ORDEN == null)
                {
                    ModelState.AddModelError("N_ORDEN", "El orden es requerido.");
                    Error = true;
                }

                ViewBag.Error = Error;

                if(!Error)
                {
                    int Cantidad = db.RESPUESTA.Where(R => R.ID_PREGUNTA == CodigoPregunta).Count();
                    if (Cantidad < 2)
                    {
                        rESPUESTA.ID_PREGUNTA = CodigoPregunta;
                        db.RESPUESTA.Add(rESPUESTA);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Error = true;
                        ModelState.AddModelError("", "Una pregunta binaria solo puede tener dos opciones de respuesta.");
                    }

                    return View(rESPUESTA);
                }
            }

            return View(rESPUESTA);
        }

        // GET: RESPUESTAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESPUESTA rESPUESTA = db.RESPUESTA.Find(id);
            if (rESPUESTA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTA, "ID_PREGUNTA", "S_NOMBRE", rESPUESTA.ID_PREGUNTA);
            return View(rESPUESTA);
        }

        // POST: RESPUESTAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_RESPUESTA,ID_PREGUNTA,S_VALOR,S_CODIGO,N_ORDEN")] RESPUESTA rESPUESTA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rESPUESTA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_PREGUNTA = new SelectList(db.PREGUNTA, "ID_PREGUNTA", "S_NOMBRE", rESPUESTA.ID_PREGUNTA);
            return View(rESPUESTA);
        }

        // GET: RESPUESTAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESPUESTA rESPUESTA = db.RESPUESTA.Find(id);
            if (rESPUESTA == null)
            {
                return HttpNotFound();
            }
            return View(rESPUESTA);
        }

        // POST: RESPUESTAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RESPUESTA rESPUESTA = db.RESPUESTA.Find(id);
            db.RESPUESTA.Remove(rESPUESTA);
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
