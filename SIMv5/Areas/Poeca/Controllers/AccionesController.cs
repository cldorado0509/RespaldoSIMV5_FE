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

namespace SIM.Areas.Poeca.Controllers
{
    public class AccionesController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Acciones
        [Authorize(Roles = "VACCIONES")]
        public ActionResult Index()
        {
            return View(db.DPOEAIR_ACCION.ToList());
        }

        // GET: Poeca/Acciones/Detalles/5
        [Authorize(Roles = "VACCIONES")]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_ACCION accion = db.DPOEAIR_ACCION.Find(id);

            var medidasSeleccionadas = new List<DxListItem>();

            foreach (var medidaAccion in accion.TPOEAIR_MEDIDA_ACCION)
            {
                var medidaSeleccionada = new DxListItem();

                medidaSeleccionada.Text =
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE + " - " +
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;

                medidaSeleccionada.Key = medidaAccion.TPOEAIR_SECTOR_MEDIDA.ID;
                medidaSeleccionada.Id = medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.ID;

                medidasSeleccionadas.Add(medidaSeleccionada);
            }

            ViewData["medidasSeleccionadas"] = medidasSeleccionadas;

            if (accion == null)
            {
                return HttpNotFound();
            }
            return View(accion);
        }

        // GET: Poeca/Acciones/Crear
        [Authorize(Roles = "CACCIONES")]
        public ActionResult Crear()
        {
            var sectorMedidasBd = db.TPOEAIR_SECTOR_MEDIDA.ToList();

            var sectores = new List<DxListItem>();
            var medidas = new Dictionary<string, List<DxListItem>>();

            var sectoresAgregados = new List<int>();

            foreach (var sectorMedida in sectorMedidasBd)
            {

                if (!sectoresAgregados.Contains(sectorMedida.DPOEAIR_SECTOR.ID))
                {
                    var listItemSector = new DxListItem();
                    listItemSector.Text = sectorMedida.DPOEAIR_SECTOR.S_NOMBRE;
                    listItemSector.Id = sectorMedida.DPOEAIR_SECTOR.ID;
                    sectores.Add(listItemSector);

                    sectoresAgregados.Add(listItemSector.Id);
                }

                var listItemMedida = new DxListItem();
                listItemMedida.Text = sectorMedida.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;
                listItemMedida.Key = sectorMedida.ID;
                listItemMedida.Id = sectorMedida.DPOEAIR_MEDIDA.ID;

                if (!medidas.ContainsKey(sectorMedida.DPOEAIR_SECTOR.ID.ToString()))
                {
                    medidas.Add(sectorMedida.DPOEAIR_SECTOR.ID.ToString(), new List<DxListItem>());
                }

                medidas[sectorMedida.DPOEAIR_SECTOR.ID.ToString()].Add(listItemMedida);

            }

            ViewData["medidas"] = medidas;
            ViewData["sectores"] = sectores;
            return View();
        }

        // POST: Poeca/Acciones/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CACCIONES")]
        public ActionResult Crear([Bind(Include = "S_NOMBRE_ACCION,S_DESCRIPCION")] DPOEAIR_ACCION nuevaAccion, int[] sectoresMedidas)
        {

            if (ModelState.IsValid)
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                var userId = User.Identity.GetUserId();
                nuevaAccion.ID_RESPONSABLE = Int32.Parse(userId);

                var sectoresMedidasDb = db.TPOEAIR_SECTOR_MEDIDA.Where(x => sectoresMedidas.Contains(x.ID)).ToList();

                db.DPOEAIR_ACCION.Add(nuevaAccion);

                //db.SaveChanges();

                nuevaAccion.TPOEAIR_MEDIDA_ACCION = new List<TPOEAIR_MEDIDA_ACCION>();

                foreach (var sectorMedida in sectoresMedidasDb)
                {
                    var relation = new TPOEAIR_MEDIDA_ACCION();
                    relation.DPOEAIR_ACCION = nuevaAccion;
                    relation.TPOEAIR_SECTOR_MEDIDA = sectorMedida;
                    nuevaAccion.TPOEAIR_MEDIDA_ACCION.Add(relation);
                }

                //db.Entry(newAction).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(nuevaAccion);
        }

        // GET: Poeca/Acciones/Editar/5
        [Authorize(Roles = "AACCIONES")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accion = db.DPOEAIR_ACCION.Find(id);


            var sectorMedidasBd = db.TPOEAIR_SECTOR_MEDIDA.ToList();

            var sectores = new List<DxListItem>();
            var medidas = new Dictionary<string, List<DxListItem>>();
            var medidasSeleccionadas = new List<DxListItem>();

            var sectoresAgregados = new List<int>();

            foreach (var sectorMedida in sectorMedidasBd)
            {

                if (!sectoresAgregados.Contains(sectorMedida.DPOEAIR_SECTOR.ID))
                {
                    var listItemSector = new DxListItem();
                    listItemSector.Text = sectorMedida.DPOEAIR_SECTOR.S_NOMBRE;
                    listItemSector.Id = sectorMedida.DPOEAIR_SECTOR.ID;
                    sectores.Add(listItemSector);

                    sectoresAgregados.Add(listItemSector.Id);
                }

                var listItemMedida = new DxListItem();
                listItemMedida.Text = sectorMedida.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;
                listItemMedida.Key = sectorMedida.ID;
                listItemMedida.Id = sectorMedida.DPOEAIR_MEDIDA.ID;

                if (!medidas.ContainsKey(sectorMedida.DPOEAIR_SECTOR.ID.ToString()))
                {
                    medidas.Add(sectorMedida.DPOEAIR_SECTOR.ID.ToString(), new List<DxListItem>());
                }

                medidas[sectorMedida.DPOEAIR_SECTOR.ID.ToString()].Add(listItemMedida);

            }

            foreach (var medidaAccion in accion.TPOEAIR_MEDIDA_ACCION)
            {
                var medidaSeleccionada = new DxListItem();

                medidaSeleccionada.Text =
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE + " - " +
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;

                medidaSeleccionada.Key = medidaAccion.TPOEAIR_SECTOR_MEDIDA.ID;
                medidaSeleccionada.Id = medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.ID;

                medidasSeleccionadas.Add(medidaSeleccionada);
            }

            ViewData["sectores"] = sectores;
            ViewData["medidas"] = medidas;
            ViewData["medidasSeleccionadas"] = medidasSeleccionadas;

            if (accion == null)
            {
                return HttpNotFound();
            }
            return View(accion);
        }

        // POST: Poeca/Acciones/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AACCIONES")]
        public ActionResult Editar([Bind(Include = "ID,S_NOMBRE_ACCION,S_DESCRIPCION")] DPOEAIR_ACCION accionModificada, int[] sectoresMedidas)
        {

            if (ModelState.IsValid)
            {
                //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                var userId = User.Identity.GetUserId();

                // Consulta de acción sin modificar
                var accionDb = db.DPOEAIR_ACCION.Find(accionModificada.ID);
                var sectoresMedidasDb = db.TPOEAIR_SECTOR_MEDIDA.Where(x => sectoresMedidas.Contains(x.ID)).ToList();

                // Eliminar las relaciones descartadas para poder ingresar las nuevas
                db.TPOEAIR_MEDIDA_ACCION.RemoveRange(accionDb.TPOEAIR_MEDIDA_ACCION.Where(x => !sectoresMedidas.Contains(x.ID_MEDIDA_SECTOR)));
                db.Entry(accionDb).State = System.Data.Entity.EntityState.Detached;

                // El objeto accionModificada estaba llegando en estado Detached, lo que impedía insertr en la base de datos
                db.DPOEAIR_ACCION.Attach(accionModificada);

                foreach (var sectorMedida in sectoresMedidasDb)
                {
                    var relation = accionModificada.TPOEAIR_MEDIDA_ACCION.FirstOrDefault(x => x.ID_MEDIDA_SECTOR == sectorMedida.ID);

                    if (relation == null)
                    {
                        relation = new TPOEAIR_MEDIDA_ACCION();
                        accionModificada.TPOEAIR_MEDIDA_ACCION.Add(relation);
                    }

                    relation.DPOEAIR_ACCION = accionModificada;
                    relation.TPOEAIR_SECTOR_MEDIDA = sectorMedida;
                }

                accionModificada.ID_RESPONSABLE = Int32.Parse(userId);

                db.Entry(accionModificada).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(accionModificada);

        }

        // GET: Poeca/Acciones/Eliminar/5
        [Authorize(Roles = "EACCIONES")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            DPOEAIR_ACCION accionAEliminar = db.DPOEAIR_ACCION.Find(id);

            var medidasSeleccionadas = new List<DxListItem>();

            foreach (var medidaAccion in accionAEliminar.TPOEAIR_MEDIDA_ACCION)
            {
                var medidaSeleccionada = new DxListItem();

                medidaSeleccionada.Text =
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE + " - " +
                    medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA;

                medidaSeleccionada.Key = medidaAccion.TPOEAIR_SECTOR_MEDIDA.ID;
                medidaSeleccionada.Id = medidaAccion.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.ID;

                medidasSeleccionadas.Add(medidaSeleccionada);
            }

            ViewData["medidasSeleccionadas"] = medidasSeleccionadas;

            if (accionAEliminar == null)
            {
                return HttpNotFound();
            }
            return View(accionAEliminar);
        }

        // POST: Poeca/Acciones/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EACCIONES")]
        public ActionResult EliminarConfirmed(int id)
        {
            DPOEAIR_ACCION dPOEAIR_ACCION = db.DPOEAIR_ACCION.Find(id);
            db.DPOEAIR_ACCION.Remove(dPOEAIR_ACCION);
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
