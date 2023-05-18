using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Models;
using SIM.Utilidades;
using System.IO;
using System.Text;
using BaxterSoft.Graphics;
using System.Security.Claims;
using SIM.Data;
using SIM.Data.Tramites;
using System.Configuration;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProyeccionDocumentoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize]
        public ActionResult Index(int? codigo)
        {
            ViewBag.Codigo = codigo ?? 0;
            return View();
        }

        [Authorize]
        public ActionResult IndicesTramites(string t)
        {
            ViewBag.Tramites = t;

            return View(t.Split(',').Select(int.Parse).ToList());
        }


        [Authorize]
        public ActionResult Documento(int? id)
        {
            //var detalleReglaFomularioElaboracion = dbSIM.DETALLE_REGLA.Where(dr => dr.S_FORMULARIO == "20").ToList();
            //ViewBag.te = String.Join(",", detalleReglaFomularioElaboracion.Select(dr => dr.CODTAREA.ToString()));
            bool soloLectura = false;

            if (id > 0)
                soloLectura = !(dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).Select(pd => pd.S_FORMULARIO).FirstOrDefault() == "20");

            ViewBag.id = id ?? 0;
            ViewBag.time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            ViewBag.SoloLectura = (soloLectura ? "S" : "N");
            ViewBag.Popup = true;

            //string indicesSeleccionGrupo = ConfigurationManager.AppSettings["IndicesProyeccionGrupos"] ?? "";
            string indicesSeleccionGrupo = Utilidades.Data.ObtenerValorParametro("PD_IndicesProyeccionGruposPARA");

            ViewBag.IndicesProyeccionGrupos = indicesSeleccionGrupo;

            return View();
        }

        [HttpPost]
        // "file" is the value of the FileUploader's "name" option
        public ActionResult CargarArchivo(HttpPostedFileBase file, int ida, int t)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            if (idUsuario == 0)
            {
                throw new HttpException("El Usuario no está Autenticado");
            }

            // Specifies the target location for the uploaded files
            string targetLocation;

            targetLocation = Server.MapPath("~/App_Data/Temporal/PD");

            if (!Directory.Exists(targetLocation))
            {
                Directory.CreateDirectory(targetLocation);
            }

            // Specifies the maximum size allowed for the uploaded files (20MB)
            int maxFileSize = 1024 * 2014 * 20;

            // Checks whether or not the request contains a file and if this file is empty or not
            if (file == null || file.ContentLength <= 0)
            {
                throw new HttpException("Archivo vacío");
            }

            // Checks that the file size does not exceed the allowed size
            if (file.ContentLength > maxFileSize)
            {
                throw new HttpException("Archivo demasiado grande");
            }

            //if (t == 1 && !file.ContentType.ToUpper().Contains("DOCX"))
            //if (t == 1 && Path.GetExtension(file.FileName).ToUpper() != ".DOCX")
            if (t == 1 && Path.GetExtension(file.FileName).ToUpper() != ".PDF")
            {
                throw new HttpException("Tipo de Archivo Inválido. Para el archivo principal solamente acepta archivos con extensión PDF.");
            }

            // Checks that the file is pdf or docx
            //if (!file.ContentType.ToUpper().Contains("PDF") && !file.ContentType.ToUpper().Contains("DOCX"))
            //if (Path.GetExtension(file.FileName).ToUpper() != ".PDF" && Path.GetExtension(file.FileName).ToUpper() != ".DOCX")
            if (Path.GetExtension(file.FileName).ToUpper() != ".PDF")
            {
                throw new HttpException("Tipo de Archivo Inválido. Para los archivos adjuntos solamente acepta archivos con extensión PDF.");
            }

            try
            {
                //string path = Path.Combine(targetLocation, ida.ToString() + Path.GetExtension(file.FileName));
                string path = Path.Combine(targetLocation, idUsuario.ToString() + "__" + ida + "__" + file.FileName);
                file.SaveAs(path);
            }
            catch (Exception e)
            {
                throw new HttpException("Error Almacenando Archivo");
            }
            return new EmptyResult();
        }

        [Authorize]
        [HttpGet, ActionName("DocumentoPreview")]
        public ActionResult GetDocumentoPreview(int id)
        {
            TramitesLibrary utilidad = new TramitesLibrary();

            //var documento = utilidad.DocumentoRadicado(id, true, 13533, false, "");
            var documento = utilidad.DocumentoRadicado(id, false, 0, true, "");

            return File(documento.documentoBinario, "application/pdf");
        }

        [Authorize]
        [HttpGet, ActionName("DocumentoFinal")]
        public ActionResult GetDocumentoFinal(int id)
        {
            var proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id && pd.S_FORMULARIO == "22").FirstOrDefault();

            if (proyeccionDocumento != null && proyeccionDocumento.ID_RADICADODOC != null)
            {
                var tramite = dbSIM.TRAMITES_PROYECCION.Where(tp => tp.ID_PROYECCION_DOC == id).FirstOrDefault();
                var documento = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == tramite.CODTRAMITE && td.CODDOCUMENTO == tramite.CODDOCUMENTO).FirstOrDefault();

                if (documento != null)
                    return File(System.IO.File.ReadAllBytes(documento.RUTA), "application/" + Path.GetExtension(documento.RUTA).Replace(".", ""));
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        // id: ID_PROYECCION_DOC_ARCHIVOS
        // n: Si el archivo es nuevo
        // a: si el archivo está actualizado
        [Authorize]
        [HttpGet, ActionName("DescargarDocumento")]
        public ActionResult GetDescargarDocumento(int id, string n, string a, string f)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            string rutaArchivo;
            string nombreArchivo = "Archivo";


            if (n == "S" || a == "S")
            {
                string targetLocation;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario > 0)
                {
                    nombreArchivo = SIM.Utilidades.Data.Base64Decode(f);

                    targetLocation = Server.MapPath("~/App_Data/Temporal/PD");
                    rutaArchivo = Path.Combine(targetLocation, idUsuario.ToString() + "__" + id.ToString() + "__" + nombreArchivo);
                }
                else
                {
                    rutaArchivo = null;
                }
            }
            else
            {
                PROYECCION_DOC_DET_ARCH archivo = dbSIM.PROYECCION_DOC_DET_ARCH.Where(da => da.ID_PROYECCION_DOC_ARCHIVOS == id && da.S_ACTIVO == "S").FirstOrDefault();

                if (archivo != null)
                {
                    rutaArchivo = archivo.S_RUTA_ARCHIVO;
                    nombreArchivo = archivo.PROYECCION_DOC_ARCHIVOS.S_DESCRIPCION + Path.GetExtension(rutaArchivo);
                }
                else
                    rutaArchivo = null;
            }

            if (rutaArchivo != null)
            {
                if (System.IO.File.Exists(rutaArchivo))
                    return File(System.IO.File.ReadAllBytes(rutaArchivo), "application/" + Path.GetExtension(rutaArchivo).Replace(".", ""), nombreArchivo);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        // No funciona si el documento fue firmado digitalmente, porque no tenemos la contraseña de la firma digital
        [HttpGet, ActionName("RegenerarDocumento")]
        public ActionResult GetRegenerarDocumento(int id)
        {
            TramitesLibrary utilidad = new TramitesLibrary();
            PROYECCION_DOC documento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();
            string radicadoGenerado = "0";

            DatosRadicado radicado;

            if (documento.ID_RADICADODOC == null)
            {
                return null;
            }
            else
            {
                radicado = new DatosRadicado();

                if ((int)documento.ID_RADICADODOC == -1)
                {
                    radicado.IdRadicado = -1;
                    radicado.Radicado = "SERIE DOCUMENTAL SIN RADICADO";
                    radicado.Fecha = DateTime.Now;
                }
                else
                {
                    RADICADO_DOCUMENTO radicadoCreado = dbSIM.RADICADO_DOCUMENTO.Where(rd => rd.ID_RADICADODOC == (int)documento.ID_RADICADODOC).FirstOrDefault();
                    radicado.IdRadicado = (int)documento.ID_RADICADODOC;
                    radicado.Radicado = radicadoCreado.S_RADICADO;
                    radicado.Fecha = radicadoCreado.D_RADICADO;
                }
            }

            radicadoGenerado = radicado.Radicado;

            var documentoRadicado = utilidad.DocumentoRadicado(documento.ID_PROYECCION_DOC, (radicado.IdRadicado > 0), radicado.IdRadicado, false, "Firmado electrónicamente decreto 491 de 2020");

            if (documentoRadicado != null)
                return File(documentoRadicado.documentoBinario, "application/pdf", "DocumentoProyeccion_" + id.ToString() + ".pdf");
            else
                return null;
        }

        [NonAction]
        protected override void OnActionExecuted(ActionExecutedContext filter)
        {
            var exception = filter.Exception;
            if (exception != null)
            {
                filter.HttpContext.Response.StatusCode = 500;
                filter.Result = new JsonResult
                {
                    Data = exception.Message
                };
                filter.ExceptionHandled = true;
            }
        }
    }
}
