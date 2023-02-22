using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SIM.Areas.Poeca.Models;
using SIM.Data;
using SIM.Data.Poeca;
using SIM.Utilidades;

namespace SIM.Areas.Poeca.Controllers
{
    public class EpisodiosController : Controller
    {
        //private ContextoPoeca db = new ContextoPoeca(); 
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Episodios
        [Authorize(Roles = "VEPISODIOS")]
        public ActionResult Index()
        {
            var dPOEAIR_EPISODIO = db.DPOEAIR_EPISODIO.Include(d => d.DPOEAIR_PERIODO_IMPLEMENTACION);
            return View(dPOEAIR_EPISODIO.ToList());
        }

        // GET: Poeca/Episodios/Details/5
        [Authorize(Roles = "VEPISODIOS")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_EPISODIO dPOEAIR_EPISODIO = db.DPOEAIR_EPISODIO.Find(id);
            if (dPOEAIR_EPISODIO == null)
            {
                return HttpNotFound();
            }
            return View(dPOEAIR_EPISODIO);
        }

        // GET: Poeca/Episodios/Crear
        [Authorize(Roles = "CEPISODIOS")]
        public ActionResult Crear()
        {
            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo");

            return View();
        }


        // POST: Poeca/Episodios/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CEPISODIOS")]
        public ActionResult Crear([Bind(Include = "N_ANIO,ID_PERIODO,S_OBSERVACIONES")] DPOEAIR_EPISODIO nuevoEpisodio)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                nuevoEpisodio.ID_RESPONSABLE = Int32.Parse(userId);
                db.DPOEAIR_EPISODIO.Add(nuevoEpisodio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text", nuevoEpisodio.N_ANIO);
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo", nuevoEpisodio.ID_PERIODO);
            return View(nuevoEpisodio);
        }

        // GET: Poeca/Episodios/Editar/5
        [Authorize(Roles = "AEPISODIOS")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_EPISODIO episodioAEditar = db.DPOEAIR_EPISODIO.Find(id);

            if (episodioAEditar == null)
            {
                return HttpNotFound();
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo", episodioAEditar.ID_PERIODO);

            return View(episodioAEditar);
        }

        // POST: Poeca/Episodios/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AEPISODIOS")]
        public ActionResult Editar([Bind(Include = "ID,N_ANIO,ID_PERIODO,S_OBSERVACIONES")] DPOEAIR_EPISODIO episodioEditado)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                episodioEditado.ID_RESPONSABLE = Int32.Parse(userId);
                db.Entry(episodioEditado).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");
            ViewBag.ID_PERIODO = new SelectList(db.PeriodoImplementacion, "Id", "NombrePeriodo", episodioEditado.ID_PERIODO);
            return View(episodioEditado);
        }

        // GET: Poeca/Episodios/Eliminar/5
        [Authorize(Roles = "EEPISODIOS")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_EPISODIO episodioAEliminar = db.DPOEAIR_EPISODIO.Find(id);
            if (episodioAEliminar == null)
            {
                return HttpNotFound();
            }
            return View(episodioAEliminar);
        }

        // POST: Poeca/Episodios/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EEPISODIOS")]
        public ActionResult EliminarConfirmed(int id)
        {
            DPOEAIR_EPISODIO episodioEliminado = db.DPOEAIR_EPISODIO.Find(id);
            db.DPOEAIR_EPISODIO.Remove(episodioEliminado);
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
