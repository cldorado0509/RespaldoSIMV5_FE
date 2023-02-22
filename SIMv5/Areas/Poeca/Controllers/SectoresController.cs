using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevExpress.DashboardCommon;
using DevExtreme.AspNet.Mvc.Builders;
using Independentsoft.Office.Odf;
using Microsoft.AspNet.Identity;
using SIM.Areas.Poeca.Models;
using SIM.Data;
using SIM.Data.Poeca;

namespace SIM.Areas.Poeca.Content
{
    public class SectoresController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Sectores
        [Authorize(Roles = "VSECTORES")]
        public ActionResult Index()
        {
            var elements = db.DPOEAIR_SECTOR.ToList();
            return View(elements);
        }

        // GET: Poeca/Sectores/Details/5
        [Authorize(Roles = "VSECTORES")]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_SECTOR sector = db.DPOEAIR_SECTOR.Find(id);

            var medidasSeleccionadas = new List<DxListItem>();

            foreach (var medidaAccion in sector.TPOEAIR_SECTOR_MEDIDA)
            {
                var medidaSeleccionada = new DxListItem();

                medidaSeleccionada.Text = medidaAccion.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;

                medidaSeleccionada.Key = medidaAccion.ID;
                medidaSeleccionada.Id = medidaAccion.DPOEAIR_MEDIDA.ID;

                medidasSeleccionadas.Add(medidaSeleccionada);
            }

            ViewData["medidasSeleccionadas"] = medidasSeleccionadas;

            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // GET: Poeca/Sectores/Crear
        [Authorize(Roles = "CSECTORES")]
        public ActionResult Crear()
        {

            var medidasBD = db.DPOEAIR_MEDIDA.ToList();
            var medidas = new List<DxListItem>();

            foreach (var medida in medidasBD)
            {
                var listItem = new DxListItem();
                listItem.Text = medida.S_NOMBRE_MEDIDA;
                listItem.Id = medida.ID;

                medidas.Add(listItem);
            }

            ViewData["medidas"] = medidas;
            return View();
        }

        // POST: Poeca/Sectores/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CSECTORES")]
        public ActionResult Crear([Bind(Include = "S_NOMBRE,S_DESCRIPCION")] DPOEAIR_SECTOR nuevoSector, int[] medidas)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                nuevoSector.ID_RESPONSABLE = Int32.Parse(userId);

                var medidasDb = db.DPOEAIR_MEDIDA.Where(x => medidas.Contains(x.ID)).ToList();

                db.DPOEAIR_SECTOR.Add(nuevoSector);

                nuevoSector.TPOEAIR_SECTOR_MEDIDA = new List<TPOEAIR_SECTOR_MEDIDA>();

                foreach (var medida in medidasDb)
                {
                    var relation = new TPOEAIR_SECTOR_MEDIDA();
                    relation.DPOEAIR_MEDIDA = medida;
                    relation.DPOEAIR_SECTOR = nuevoSector;
                    nuevoSector.TPOEAIR_SECTOR_MEDIDA.Add(relation);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(nuevoSector);
        }

        // GET: Poeca/Sectores/Editar/5
        [Authorize(Roles = "ASECTORES")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_SECTOR sectorAModificar = db.DPOEAIR_SECTOR.Find(id);

            var medidasBD = db.DPOEAIR_MEDIDA.ToList();
            var medidas = new List<DxListItem>();

            foreach (var medida in medidasBD)
            {
                var listItem = new DxListItem();
                listItem.Text = medida.S_NOMBRE_MEDIDA;
                listItem.Id = medida.ID;

                medidas.Add(listItem);
            }

            if (sectorAModificar == null)
            {
                return HttpNotFound();
            }

            ViewData["medidas"] = medidas;

            return View(sectorAModificar);
        }

        // POST: Poeca/Sectores/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ASECTORES")]
        public ActionResult Editar([Bind(Include = "ID,S_NOMBRE,S_DESCRIPCION")] DPOEAIR_SECTOR sectorModificado, int[] medidas)
        {

            if (ModelState.IsValid)
            {
                medidas = medidas != null ? medidas : new int[0];
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                // Consulta de acción sin modificar
                var sectorDb = db.DPOEAIR_SECTOR.Find(sectorModificado.ID);
                var medidasDb = db.DPOEAIR_MEDIDA.Where(x => medidas.Contains(x.ID)).ToList();

                // Eliminar las relaciones descartadas para poder ingresar las nuevas
                db.TPOEAIR_SECTOR_MEDIDA.RemoveRange(sectorDb.TPOEAIR_SECTOR_MEDIDA.Where(x => !medidas.Contains(x.ID_MEDIDA)));
                db.Entry(sectorDb).State = System.Data.Entity.EntityState.Detached;

                // El objeto sectorModificado estaba llegando en estado Detached, lo que impedía insertr en la base de datos
                db.DPOEAIR_SECTOR.Attach(sectorModificado);

                foreach (var medida in medidasDb)
                {
                    var relation = sectorModificado.TPOEAIR_SECTOR_MEDIDA.FirstOrDefault(x => x.ID_MEDIDA == medida.ID);

                    if (relation == null)
                    {
                        relation = new TPOEAIR_SECTOR_MEDIDA();
                        sectorModificado.TPOEAIR_SECTOR_MEDIDA.Add(relation);
                    }

                    relation.DPOEAIR_SECTOR = sectorModificado;
                    relation.DPOEAIR_MEDIDA = medida;
                }

                db.Entry(sectorModificado).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(sectorModificado);

        }

        // GET: Poeca/Sectores/Eliminar/5
        [Authorize(Roles = "ESECTORES")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_SECTOR sectoAEliminar = db.DPOEAIR_SECTOR.Find(id);

            var medidasSeleccionadas = new List<DxListItem>();

            foreach (var medidaAccion in sectoAEliminar.TPOEAIR_SECTOR_MEDIDA)
            {
                var medidaSeleccionada = new DxListItem();

                medidaSeleccionada.Text = medidaAccion.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;
                medidaSeleccionada.Id = medidaAccion.DPOEAIR_MEDIDA.ID;

                medidasSeleccionadas.Add(medidaSeleccionada);
            }

            ViewData["medidasSeleccionadas"] = medidasSeleccionadas;

            if (sectoAEliminar == null)
            {
                return HttpNotFound();
            }
            return View(sectoAEliminar);
        }

        // POST: Poeca/Sectores/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ESECTORES")]
        public ActionResult DeleteConfirmed(int id)
        {
            DPOEAIR_SECTOR dPOEAIR_SECTOR = db.DPOEAIR_SECTOR.Find(id);
            db.DPOEAIR_SECTOR.Remove(dPOEAIR_SECTOR);
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
