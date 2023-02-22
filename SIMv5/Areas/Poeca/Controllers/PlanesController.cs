using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Microsoft.AspNet.Identity;
using SIM.Data;
using SIM.Data.General;
using SIM.Data.Poeca;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PdfSharp.Drawing;

namespace SIM.Areas.Poeca.Controllers
{
    public class PlanesController : Controller
    {
        //private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //private ContextoPoeca db = new ContextoPoeca(); 
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        [Authorize(Roles = "VPLANES")]
        // GET: Poeca/Planes
        public ActionResult Index(int? id)
        {

            var userId = int.Parse(User.Identity.GetUserId());
            //var terceroDb = UtilidadesPoeca.AdquirirTercero(dbSIM, userId, id);
            var terceroDb = UtilidadesPoeca.AdquirirTercero(db, userId, id);

            if (User.IsInRole("CEPISODIOS") && id == null)
            {
                var tercerosDb = db.DPOEAIR_MUNICIPIOS_TERCEROS.ToList();
                var terceros = new List<SelectListItem>();

                foreach (var episodioDb in tercerosDb)
                {
                    terceros.Add(new SelectListItem
                    {
                        Text = episodioDb.S_NOMBRE,
                        Value = episodioDb.ID_TERCERO.ToString()
                    });
                }

                ViewBag.TERCEROS = terceros;

                return View("SeleccionarTercero");
            }
            else if (terceroDb != null)
            {
                var planes = db.TPOEAIR_PLAN.Where(x => x.ID_RESPONSABLE == terceroDb.ID_TERCERO).ToList();
                return View(planes);
            }

            return Redirect("/");
        }

        // GET: Poeca/Planes/Detalles/5
        [Authorize(Roles = "VPLANES")]
        public ActionResult Detalles(int? id)
        {
            return RedirectToAction("", "AccionesXPlan", new { plan = id });
        }

        [Authorize(Roles = "VPLANES")]
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

            var datosDePlan = db.TPOEAIR_PLAN.Find(id);

            // User.IsInRole("CEPISODIOS") Valida si es administrador o un usuario que puede acceder a esos datos
            if (User.IsInRole("CEPISODIOS") || datosDePlan.ID_RESPONSABLE == tercero.ID_TERCERO)
            {
                if (!String.IsNullOrEmpty(datosDePlan.S_URL_ANEXO))
                {
                    var nombreArchivo = Path.GetFileName(datosDePlan.S_URL_ANEXO);
                    nombreArchivo = "Plan-" + datosDePlan.N_ANIO + "-" + nombreArchivo;

                    return File(datosDePlan.S_URL_ANEXO, MimeMapping.GetMimeMapping(nombreArchivo), nombreArchivo);
                }
            }
            Response.StatusCode = 404;
            return null;
        }

        [Authorize(Roles = "CPLANES")]
        // GET: Poeca/Planes/Crear
        public ActionResult Crear()
        {
            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");

            return View();
        }

        // POST: Poeca/Planes/Crear
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CPLANES")]
        public ActionResult Crear([Bind(Include = "N_ANIO,S_OBSERVACIONES,S_REMITENTE,S_CARGO_REMITENTE")] TPOEAIR_PLAN nuevoPlan, int? id)
        {

            if (ModelState.IsValid && !User.IsInRole("CEPISODIOS"))
            {
                var idUsuario = int.Parse(User.Identity.GetUserId());
                //var tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, idUsuario);
                var tercero = UtilidadesPoeca.AdquirirTercero(db, idUsuario);

                var planExistente = db.TPOEAIR_PLAN.Where(x => x.N_ANIO == nuevoPlan.N_ANIO && x.ID_RESPONSABLE == tercero.ID_TERCERO).FirstOrDefault();
                if (planExistente != null)
                {
                    ViewBag.MensajeError = "Ya existe un plan para el año " + planExistente.N_ANIO;
                }
                else
                {
                    var rutaAnexo = "";
                    var rutaBase = ConfigurationManager.AppSettings["DocumentsDocumentalPath"] + "\\" + idUsuario;

                    Directory.CreateDirectory(rutaBase);

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        if (file.ContentLength > 0)
                        {
                            rutaAnexo = Path.GetFileName(file.FileName);
                            rutaAnexo = rutaBase + "\\" + rutaAnexo;
                            file.SaveAs(rutaAnexo);
                        }
                    }

                    var idTramite = UtilidadesPoeca.crearTramite(idUsuario);

                    //var tramite = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                    //var user = dbSIM.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();
                    var tramite = db.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                    var user = db.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();

                    //Archivos.GetRutaDocumento();
                    var pdfStream = Utilidades.Reportes.crearCartaCreacion(nuevoPlan, tercero, tramite);
                    var rutaCarta = rutaBase + "\\carta creación.pdf";
                    Archivos.GrabaMemoryStream(pdfStream, rutaCarta);

                    nuevoPlan.S_URL_ANEXO = rutaBase + "\\Anexo Plan  " + nuevoPlan.N_ANIO + "-" + tramite.CODTRAMITE + ".pdf";

                    if (!String.IsNullOrEmpty(rutaAnexo))
                    {
                        Archivos.CombinarPDF(rutaCarta, rutaAnexo, nuevoPlan.S_URL_ANEXO);
                    }
                    else
                    {
                        using (PdfDocument cartaPdf = PdfReader.Open(rutaCarta, PdfDocumentOpenMode.Import))
                        using (PdfDocument outPdf = new PdfDocument())
                        {
                            Archivos.CopiarPaginasPDF(cartaPdf, outPdf);

                            outPdf.Save(nuevoPlan.S_URL_ANEXO);
                        }
                    }

                    nuevoPlan.ID_TRAMITE = (decimal)idTramite;

                    nuevoPlan.ID_RESPONSABLE = tercero.ID_TERCERO;

                    db.TPOEAIR_PLAN.Add(nuevoPlan);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");

            return View(nuevoPlan);
        }

        // GET: Poeca/Planes/Editar/5
        [Authorize(Roles = "APLANES")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_PLAN planAEditar = db.TPOEAIR_PLAN.Find(id);

            if (planAEditar == null)
            {
                return HttpNotFound();
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");

            return View(planAEditar);
        }

        // POST: Poeca/Planes/Editar/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "APLANES")]
        public ActionResult Editar([Bind(Include = "ID,N_ANIO,S_OBSERVACIONES,S_REMITENTE,S_CARGO_REMITENTE")] TPOEAIR_PLAN planAEditar)
        {
            if (ModelState.IsValid && !User.IsInRole("CEPISODIOS"))
            {
                var idUsuario = int.Parse(User.Identity.GetUserId());
                //var tercero = UtilidadesPoeca.AdquirirTercero(dbSIM, idUsuario);
                var tercero = UtilidadesPoeca.AdquirirTercero(db, idUsuario);

                var planExistente = db.TPOEAIR_PLAN.Where(x => x.ID_RESPONSABLE == tercero.ID_TERCERO && x.N_ANIO == planAEditar.N_ANIO && x.ID != planAEditar.ID).FirstOrDefault();
                if (planExistente != null)
                {
                    ViewBag.MensajeError = "Ya existe un plan para el año " + planExistente.N_ANIO;
                }
                else
                {
                    var rutaAnexo = "";
                    var rutaBase = ConfigurationManager.AppSettings["DocumentsDocumentalPath"] + "\\" + idUsuario;

                    Directory.CreateDirectory(rutaBase);

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        if (file.ContentLength > 0)
                        {
                            rutaAnexo = Path.GetFileName(file.FileName);
                            rutaAnexo = rutaBase + "\\" + rutaAnexo;
                            file.SaveAs(rutaAnexo);
                        }
                    }

                    var idTramite = db.TPOEAIR_PLAN.Where(x => x.ID == planAEditar.ID).Select(x => x.ID_TRAMITE).FirstOrDefault();

                    //var tramite = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                    //var user = dbSIM.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();
                    var tramite = db.TBTRAMITE.Where(t => t.CODTRAMITE == idTramite).FirstOrDefault();
                    var user = db.USUARIO.Where(t => t.ID_USUARIO == idUsuario).FirstOrDefault();

                    //Archivos.GetRutaDocumento();
                    var pdfStream = Utilidades.Reportes.crearCartaCreacion(planAEditar, tercero, tramite);
                    var rutaCarta = rutaBase + "\\carta creación.pdf";
                    Archivos.GrabaMemoryStream(pdfStream, rutaCarta);

                    planAEditar.S_URL_ANEXO = rutaBase + "\\Anexo Plan  " + planAEditar.N_ANIO + "-" + tramite.CODTRAMITE + ".pdf";

                    if (!String.IsNullOrEmpty(rutaAnexo))
                    {
                        Archivos.CombinarPDF(rutaCarta, rutaAnexo, planAEditar.S_URL_ANEXO);
                    }
                    else
                    {
                        using (PdfDocument cartaPdf = PdfReader.Open(rutaCarta, PdfDocumentOpenMode.Import))
                        using (PdfDocument outPdf = new PdfDocument())
                        {
                            Archivos.CopiarPaginasPDF(cartaPdf, outPdf);

                            outPdf.Save(planAEditar.S_URL_ANEXO);
                        }
                    }

                    planAEditar.ID_TRAMITE = (decimal)idTramite;

                    planAEditar.ID_RESPONSABLE = tercero.ID_TERCERO;

                    db.Entry(planAEditar).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            var aniosDisponibles = UtilidadesPoeca.crearListaDeAnios();

            ViewBag.N_ANIO = new SelectList(aniosDisponibles, "Id", "Text");

            return View(planAEditar);
        }

        // GET: Poeca/Planes/Eliminar/5
        [Authorize(Roles = "EPLANES")]
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPOEAIR_PLAN tPOEAIR_PLAN = db.TPOEAIR_PLAN.Find(id);
            if (tPOEAIR_PLAN == null)
            {
                return HttpNotFound();
            }
            return View(tPOEAIR_PLAN);
        }

        // POST: Poeca/Planes/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EPLANES")]
        public ActionResult EliminarConfirmado(int id)
        {
            TPOEAIR_PLAN tPOEAIR_PLAN = db.TPOEAIR_PLAN.Find(id);
            db.TPOEAIR_PLAN.Remove(tPOEAIR_PLAN);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "APLANES")]
        public ActionResult Radicar(int id)
        {
            var userId = Int32.Parse(User.Identity.GetUserId());
            TPOEAIR_PLAN planCompletado = db.TPOEAIR_PLAN.Find(id);

            if (planCompletado.TPOEAIR_ACCIONES_PLAN.Count != 0)
            {
                var medidasObligatorias = db.DPOEAIR_MEDIDA.Where(x => x.S_ES_OBLIGATORIA == "1");
                var faltanMedidas = false;
                foreach (var medidaObligatoria in medidasObligatorias)
                {
                    var existe = false;
                    foreach (var accion in planCompletado.TPOEAIR_ACCIONES_PLAN)
                    {
                        if (accion.TPOEAIR_MEDIDA_ACCION.TPOEAIR_SECTOR_MEDIDA.ID_MEDIDA == medidaObligatoria.ID)
                        {
                            existe = true;
                            break;
                        }
                    }

                    if (!existe)
                    {
                        if (!faltanMedidas)
                        {
                            faltanMedidas = true;
                            TempData["ErrorMessage"] = "Faltan las siguientes medidas Obligatorias:\n";
                        }
                        TempData["ErrorMessage"] += $" - {medidaObligatoria.S_NOMBRE_MEDIDA}\n";
                    }
                }

                if (faltanMedidas)
                {
                    return RedirectToAction("Index", "AccionesXPlan", new { plan = id });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se ha agregado ninguna acción";
                return RedirectToAction("Index", "AccionesXPlan", new { plan = id });
            }

            //var tramite = dbSIM.TBTRAMITE.Where(x => x.CODTRAMITE == planCompletado.ID_TRAMITE).FirstOrDefault();
            var tramite = db.TBTRAMITE.Where(x => x.CODTRAMITE == planCompletado.ID_TRAMITE).FirstOrDefault();


            // Radicación
            var radicador = new Radicador();

            var unidadDocumentalOSerie = int.Parse(SIM.Utilidades.Data.ObtenerValorParametro("UnidadDocCOR"));
            //var resultadoRadicado = radicador.GenerarRadicado(dbSIM, unidadDocumentalOSerie, userId, DateTime.Now, "FuncionarioTL");
            var resultadoRadicado = radicador.GenerarRadicado(db, unidadDocumentalOSerie, userId, DateTime.Now, "FuncionarioTL");
            planCompletado.N_RADICADO = resultadoRadicado.IdRadicado;
            planCompletado.S_RADICADO = resultadoRadicado.Radicado;
            planCompletado.N_RADICADO_ANIO = DateTime.Today.Year;


            // Registro de documento
            var pdfDocument = PdfReader.Open(planCompletado.S_URL_ANEXO, PdfDocumentOpenMode.Import);
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
            documentoSalida.Save(planCompletado.S_URL_ANEXO);

            var documento = memStream.ToArray();

            UtilidadesPoeca.RegistrarDocumento(userId, (int)tramite.CODTRAMITE, unidadDocumentalOSerie, planCompletado.N_RADICADO, documento, pdfDocument.PageCount);

            db.Entry(planCompletado).State = System.Data.Entity.EntityState.Modified;
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
