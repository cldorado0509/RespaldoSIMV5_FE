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

namespace SIM.Areas.Poeca
{
    public class ProductosController : Controller
    {
        //private ContextoPoeca db = new ContextoPoeca(); 
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/Productos
        [Authorize(Roles = "VPRODUCTOS")]
        public ActionResult Index()
        {
            return View(db.DPOEAIR_PRODUCTO.ToList());
        }

        // GET: Poeca/Productos/Details/5
        [Authorize(Roles = "VPRODUCTOS")]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_PRODUCTO producto = db.DPOEAIR_PRODUCTO.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Poeca/Productos/Create
        [Authorize(Roles = "CPRODUCTOS")]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Poeca/Productos/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CPRODUCTOS")]
        public ActionResult Crear([Bind(Include = "S_NOMBRE_PRODUCTO,S_DESCRIPCION")] DPOEAIR_PRODUCTO producto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                producto.ID_RESPONSABLE = Int32.Parse(userId);
                db.DPOEAIR_PRODUCTO.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Poeca/Productos/Edit/5
        [Authorize(Roles = "APRODUCTOS")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_PRODUCTO producto = db.DPOEAIR_PRODUCTO.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Poeca/Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "APRODUCTOS")]
        public ActionResult Editar([Bind(Include = "ID,S_NOMBRE_PRODUCTO,S_DESCRIPCION")] DPOEAIR_PRODUCTO producto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                producto.ID_RESPONSABLE = Int32.Parse(userId);
                db.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Poeca/Productos/Delete/5
        [Authorize(Roles = "EPRODUCTOS")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DPOEAIR_PRODUCTO producto = db.DPOEAIR_PRODUCTO.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Poeca/Productos/Delete/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EPRODUCTOS")]
        public ActionResult DeleteConfirmed(int id)
        {
            DPOEAIR_PRODUCTO producto = db.DPOEAIR_PRODUCTO.Find(id);
            db.DPOEAIR_PRODUCTO.Remove(producto);
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
