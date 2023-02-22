using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SIM.Data;
using SIM.Data.Poeca;

namespace SIM.Areas.Poeca.Controllers
{
    public class MedidasController : Controller
    {
        //private ContextoPoeca db = new ContextoPoeca(); 
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Medidas
        [Authorize(Roles = "VMEDIDAS")]
        public ActionResult Index()
        {
            return View(db.DPOEAIR_MEDIDA.ToList());
        }

        [Authorize(Roles = "VMEDIDAS")]
        // GET: Poeca/Medidas/Detalles/5
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_MEDIDA medida = db.DPOEAIR_MEDIDA.Find(id);
            if (medida == null)
            {
                return HttpNotFound();
            }
            return View(medida);
        }

        // GET: Poeca/Medidas/Create
        [Authorize(Roles = "CMEDIDAS")]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Poeca/Medidas/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CMEDIDAS")]
        public ActionResult Crear([Bind(Include = "S_NOMBRE_MEDIDA,S_DESCRIPCION,S_ES_OBLIGATORIA")] DPOEAIR_MEDIDA nuevaMedida)
        {
            if (ModelState.IsValid)
            {
                var medidaExistente = db.DPOEAIR_MEDIDA
                   .Where(x => x.S_NOMBRE_MEDIDA == nuevaMedida.S_NOMBRE_MEDIDA || (!string.IsNullOrEmpty(x.S_DESCRIPCION) && x.S_DESCRIPCION == nuevaMedida.S_DESCRIPCION))
                   .FirstOrDefault();
                if (medidaExistente != null)
                {
                    ViewBag.MensajeError = "Ya existe una medida con este nombre o descripción";
                    return View(nuevaMedida);
                }

                var userId = User.Identity.GetUserId();
                nuevaMedida.ID_RESPONSABLE = Int32.Parse(userId);
                db.DPOEAIR_MEDIDA.Add(nuevaMedida);
                //if (dPOEAIR_MEDIDA.S_ES_OBLIGATORIA.Equals("1") || dPOEAIR_MEDIDA.S_ES_OBLIGATORIA.Equals("0"))

                if (!(new[] { "0", "1" }).Contains(nuevaMedida.S_ES_OBLIGATORIA)) // Se compara con una lista de valores válidos
                {
                    nuevaMedida.S_ES_OBLIGATORIA = "0";
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nuevaMedida);
        }

        // GET: Poeca/Medidas/Editar/5
        [Authorize(Roles = "AMEDIDAS")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_MEDIDA medidaAEditar = db.DPOEAIR_MEDIDA.Find(id);
            if (medidaAEditar == null)
            {
                return HttpNotFound();
            }
            return View(medidaAEditar);
        }

        // POST: Poeca/Medidas/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AMEDIDAS")]
        public ActionResult Editar([Bind(Include = "ID,S_NOMBRE_MEDIDA,S_DESCRIPCION,S_ES_OBLIGATORIA")] DPOEAIR_MEDIDA medidaEditada)
        {
            if (ModelState.IsValid)
            {
                var medidaExistente = db.DPOEAIR_MEDIDA
                    .Where(x => x.ID != medidaEditada.ID && (x.S_NOMBRE_MEDIDA == medidaEditada.S_NOMBRE_MEDIDA || (!string.IsNullOrEmpty(x.S_DESCRIPCION) && x.S_DESCRIPCION == medidaEditada.S_DESCRIPCION)))
                    .FirstOrDefault();
                if (medidaExistente != null)
                {
                    ViewBag.MensajeError = "Ya existe una medida con este nombre o descripción";
                    return View(medidaEditada);
                }

                var userId = User.Identity.GetUserId();
                medidaEditada.ID_RESPONSABLE = Int32.Parse(userId);

                db.Entry(medidaEditada).State = System.Data.Entity.EntityState.Modified;

                if (!(new[] { "0", "1" }).Contains(medidaEditada.S_ES_OBLIGATORIA)) // Se compara con una lista de valores válidos
                {
                    medidaEditada.S_ES_OBLIGATORIA = "0";
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medidaEditada);
        }

        // GET: Poeca/Medidas/Eliminar/5
        [Authorize(Roles = "EMEDIDAS")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_MEDIDA medidaAEliminar = db.DPOEAIR_MEDIDA.Find(id);
            if (medidaAEliminar == null)
            {
                return HttpNotFound();
            }
            return View(medidaAEliminar);
        }

        // POST: Poeca/Medidas/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EMEDIDAS")]
        public ActionResult DeleteConfirmed(int id)
        {
            DPOEAIR_MEDIDA medidaEliminada = db.DPOEAIR_MEDIDA.Find(id);
            db.DPOEAIR_MEDIDA.Remove(medidaEliminada);
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
