using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Data;
using SIM.Data.General;
using SIM.Data.Poeca;
using SIM.Utilidades;

namespace SIM.Areas.Poeca.Controllers
{
    public class SeguimientoFinalEpisodioController : Controller
    {
        //private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //private ContextoPoeca db = new ContextoPoeca();
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        // GET: Poeca/SeguimientoFinalEpisodio/
        [Authorize]
        public ActionResult Index(int? plan, int? tercero)
        {
            var userId = int.Parse(User.Identity.GetUserId());
            //var terceroDb = UtilidadesPoeca.AdquirirTercero(dbSIM, userId, tercero);
            var terceroDb = UtilidadesPoeca.AdquirirTercero(db, userId, tercero);

            if (terceroDb == null)
            {
                if (User.IsInRole("CEPISODIOS"))
                {
                    var tercerosDb = db.DPOEAIR_MUNICIPIOS_TERCEROS.ToList();
                    var terceros = new List<SelectListItem>();

                    foreach (var terceroDb1 in tercerosDb)
                    {
                        terceros.Add(new SelectListItem
                        {
                            Text = terceroDb1.S_NOMBRE,
                            Value = terceroDb1.ID_TERCERO.ToString()
                        });
                    }

                    ViewBag.TERCEROS = terceros;

                    return View("SeleccionarTercero");
                }
                else
                {
                    return HttpNotFound();
                }
            }

            if (plan != null)
            {
                var tPOEAIR_SEGUIMIENTO_GLOBAL = db.TPOEAIR_SEGUIMIENTO_GLOBAL
                .Include(t => t.DPOEAIR_EPISODIO)
                .Include(t => t.TPOEAIR_PLAN)
                .Where(t => t.ID_PLAN == plan);

                return View(tPOEAIR_SEGUIMIENTO_GLOBAL.ToList());
            }
            else
            {
                var planesDb = db.TPOEAIR_PLAN.Where(x => x.ID_RESPONSABLE == terceroDb.ID_TERCERO).ToList();
                var planes = new List<SelectListItem>();

                foreach (var planDb in planesDb)
                {
                    planes.Add(new SelectListItem
                    {
                        Text = "Plan del " + planDb.N_ANIO,
                        Value = planDb.ID.ToString()
                    });
                }

                ViewBag.PLANES = planes;

                return View("SeleccionarPlan");
            }

        }

        [Authorize]
        // GET: Poeca/SeguimientoFinalEpisodio/Detalles/5
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_SEGUIMIENTO_GLOBAL tPOEAIR_SEGUIMIENTO_GLOBAL = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);
            if (tPOEAIR_SEGUIMIENTO_GLOBAL == null)
            {
                return HttpNotFound();
            }
            return View(tPOEAIR_SEGUIMIENTO_GLOBAL);
        }

        // GET: Poeca/SeguimientoFinalEpisodio/Radicar
        public ActionResult Radicar(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.episodio = id;

                return View();
            }

            return HttpNotFound();
        }

        // POST: Poeca/SeguimientoFinalEpisodio/Radicar
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Radicar([Bind(Include = "ID_PLAN,ID_EPISODIO,S_OBSERVACIONES,S_REMITENTE,S_CARGO_REMITENTE")] TPOEAIR_SEGUIMIENTO_GLOBAL seguimientoARadicar)
        {
            if (ModelState.IsValid && !User.IsInRole("CEPISODIOS"))
            {
                var rutaEvidencia = "";
                var idUsuario = int.Parse(User.Identity.GetUserId());
                var rutaBase = ConfigurationManager.AppSettings["DocumentsDocumentalPath"] + "\\" + idUsuario;

                Directory.CreateDirectory(rutaBase);

                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        rutaEvidencia = Path.GetFileName(file.FileName);
                        rutaEvidencia = rutaBase + "\\" + rutaEvidencia;
                        file.SaveAs(rutaEvidencia);
                    }
                }

                var idTramite = UtilidadesPoeca.crearTramite(idUsuario);

                //var tramite = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                //var user = dbSIM.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();
                var tramite = db.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                var user = db.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();
                var episodio = db.DPOEAIR_EPISODIO.Find(seguimientoARadicar.ID_EPISODIO);

                //var tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, idUsuario);
                var tercero = UtilidadesPoeca.AdquirirTercero(db, idUsuario);
                var plan = db.TPOEAIR_PLAN.Where(t => t.N_ANIO == episodio.N_ANIO && t.ID_RESPONSABLE == tercero.ID_TERCERO).FirstOrDefault();

                //Archivos.GetRutaDocumento();
                var pdfStream = Utilidades.Reportes.crearCartaFinEpisodio(episodio, tercero, tramite);
                var rutaCarta = rutaBase + "\\carta evidencia.pdf";
                Archivos.GrabaMemoryStream(pdfStream, rutaCarta);

                seguimientoARadicar.S_URL_EVIDENCIA = rutaBase + "\\Evidencias " + episodio.DPOEAIR_PERIODO_IMPLEMENTACION.NombrePeriodo + " de " + episodio.N_ANIO + "-" + tramite.CODTRAMITE + ".pdf";

                Archivos.CombinarPDF(rutaCarta, rutaEvidencia, seguimientoARadicar.S_URL_EVIDENCIA);

                // Radicación
                var radicador = new Radicador();

                // Unidad documental y serie son lo mismo
                var unidadDocumentalOSerie = int.Parse(SIM.Utilidades.Data.ObtenerValorParametro("UnidadDocCOR"));
                var resultadoRadicado = radicador.GenerarRadicado(unidadDocumentalOSerie, idUsuario, DateTime.Now, "FuncionarioTL");
                seguimientoARadicar.N_RADICADO = resultadoRadicado.IdRadicado;
                seguimientoARadicar.S_RADICADO = resultadoRadicado.Radicado;


                // Registro de documento
                var pdfDocument = PdfReader.Open(seguimientoARadicar.S_URL_EVIDENCIA, PdfDocumentOpenMode.Import);
                var memStream = new MemoryStream();
                var documentoSalida = new PdfDocument();

                for (int i = 0; i < pdfDocument.PageCount; i++)
                {
                    var page = pdfDocument.Pages[i];
                    documentoSalida.AddPage(page);

                    if (i == 0)
                    {
                        Radicado01Report etiqueta = new Radicado01Report();
                        MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(resultadoRadicado.IdRadicado, "png");

                        XGraphics gfx = XGraphics.FromPdfPage(documentoSalida.Pages[i]);
                        XImage image = XImage.FromStream(imagenEtiqueta);
                        gfx.DrawImage(image, new System.Drawing.Point(300, 60));
                    }
                }
                pdfDocument.Close();

                documentoSalida.Save(memStream);
                documentoSalida.Save(seguimientoARadicar.S_URL_EVIDENCIA);

                var documento = memStream.ToArray();

                UtilidadesPoeca.RegistrarDocumento(idUsuario, (int)tramite.CODTRAMITE, unidadDocumentalOSerie, seguimientoARadicar.N_RADICADO, documento, pdfDocument.PageCount);


                seguimientoARadicar.ID_TRAMITE = (int)idTramite;
                seguimientoARadicar.ID_RESPONSABLE = tercero.ID_TERCERO;
                seguimientoARadicar.ID_PLAN = plan.ID;

                db.TPOEAIR_SEGUIMIENTO_GLOBAL.Add(seguimientoARadicar);
                db.SaveChanges();

                return RedirectToAction("Index", new { plan = seguimientoARadicar.ID_PLAN });
            }

            ViewBag.ID_EPISODIO = new SelectList(db.DPOEAIR_EPISODIO, "ID", "S_OBSERVACIONES", seguimientoARadicar.ID_EPISODIO);
            ViewBag.ID_PLAN = new SelectList(db.TPOEAIR_PLAN, "ID", "S_RADICADO", seguimientoARadicar.ID_PLAN);
            return View(seguimientoARadicar);
        }

        public FileResult DescargarAnexo(int id)
        {
            var userId = Int32.Parse(User.Identity.GetUserId());
            TERCERO tercero;
            try
            {
                //tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, userId);
                tercero = UtilidadesPoeca.AdquirirTercero(db, userId);
            }
            catch (Exception)
            {
                tercero = null;
            }

            var datosDeSeguimiento = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);

            // User.IsInRole("CEPISODIOS") Valida si es administrador o un usuario que puede acceder a esos datos
            if (User.IsInRole("CEPISODIOS") || datosDeSeguimiento.ID_RESPONSABLE == tercero.ID_TERCERO)
            {
                if (!String.IsNullOrEmpty(datosDeSeguimiento.S_URL_EVIDENCIA))
                {
                    var nombreArchivo = Path.GetFileName(datosDeSeguimiento.S_URL_EVIDENCIA);

                    return File(datosDeSeguimiento.S_URL_EVIDENCIA, MimeMapping.GetMimeMapping(nombreArchivo), nombreArchivo);
                }
            }
            Response.StatusCode = 404;
            return null;
        }

        // GET: Poeca/SeguimientoFinalEpisodio/Editar/5
        //public ActionResult Editar(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TPOEAIR_SEGUIMIENTO_GLOBAL tPOEAIR_SEGUIMIENTO_GLOBAL = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);
        //    if (tPOEAIR_SEGUIMIENTO_GLOBAL == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ID_EPISODIO = new SelectList(db.DPOEAIR_EPISODIO, "ID", "S_OBSERVACIONES", tPOEAIR_SEGUIMIENTO_GLOBAL.ID_EPISODIO);
        //    ViewBag.ID_PLAN = new SelectList(db.TPOEAIR_PLAN, "ID", "S_RADICADO", tPOEAIR_SEGUIMIENTO_GLOBAL.ID_PLAN);
        //    return View(tPOEAIR_SEGUIMIENTO_GLOBAL);
        //}

        //// POST: Poeca/SeguimientoFinalEpisodio/Editar/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Editar([Bind(Include = "ID,ID_PLAN,ID_EPISODIO,N_RADICADO,ID_RESPONSABLE,S_OBSERVACIONES,S_REMITENTE,S_CARGO_REMITENTE,S_URL_EVIDENCIA")] TPOEAIR_SEGUIMIENTO_GLOBAL tPOEAIR_SEGUIMIENTO_GLOBAL)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tPOEAIR_SEGUIMIENTO_GLOBAL).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ID_EPISODIO = new SelectList(db.DPOEAIR_EPISODIO, "ID", "S_OBSERVACIONES", tPOEAIR_SEGUIMIENTO_GLOBAL.ID_EPISODIO);
        //    ViewBag.ID_PLAN = new SelectList(db.TPOEAIR_PLAN, "ID", "S_RADICADO", tPOEAIR_SEGUIMIENTO_GLOBAL.ID_PLAN);
        //    return View(tPOEAIR_SEGUIMIENTO_GLOBAL);
        //}

        //// GET: Poeca/SeguimientoFinalEpisodio/Eliminar/5
        //public ActionResult Eliminar(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TPOEAIR_SEGUIMIENTO_GLOBAL tPOEAIR_SEGUIMIENTO_GLOBAL = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);
        //    if (tPOEAIR_SEGUIMIENTO_GLOBAL == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tPOEAIR_SEGUIMIENTO_GLOBAL);
        //}

        //// POST: Poeca/SeguimientoFinalEpisodio/Eliminar/5
        //[HttpPost, ActionName("Eliminar")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EliminarConfirmed(int id)
        //{
        //    TPOEAIR_SEGUIMIENTO_GLOBAL tPOEAIR_SEGUIMIENTO_GLOBAL = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);
        //    db.TPOEAIR_SEGUIMIENTO_GLOBAL.Remove(tPOEAIR_SEGUIMIENTO_GLOBAL);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public JsonResult Informe(int? id)
        {

            var seguimientoFinal = db.TPOEAIR_SEGUIMIENTO_GLOBAL.Find(id);
            var acciones = seguimientoFinal.TPOEAIR_PLAN.TPOEAIR_ACCIONES_PLAN;
            var idsAcciones = acciones.Select(x => x.ID);
            var seguimientos = db.TPOEAIR_SEGUIMIENTO_META
                .Where(x => x.ID_EPISODIO == seguimientoFinal.ID_EPISODIO && idsAcciones.Contains(x.TPOEAIR_ACCIONES_PLAN.ID))
                .GroupBy(x => x.ID_INFO_ACCION).ToList()
                .Select(x => new
                {
                    ector = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_SECTOR.S_NOMBRE,
                    medida = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.DPOEAIR_MEDIDA.S_NOMBRE_MEDIDA,
                    accion = x.FirstOrDefault().TPOEAIR_ACCIONES_PLAN.TPOEAIR_MEDIDA_ACCION.DPOEAIR_ACCION.S_NOMBRE_ACCION,
                    valoracionEconomica = x.Sum(ve => ve.N_VALORACION_ECONOMICA),
                    seguimiento = x.Sum(sm => sm.N_SEGUIMIENTO_META),
                    observaciones = string.Join("\n", x.Select(o => o.S_OBSERVACIONES)),
                });

            return Json(seguimientos.ToList(), JsonRequestBehavior.AllowGet);
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
