using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Models;
using SIM.Utilidades;
using System.IO;
using System.Text;
using BaxterSoft.Graphics;
using System.Security.Claims;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Data.Control;
using SIM.Areas.Seguridad.Models;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class PMESInformativoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize]
        public ActionResult Index(int? codigo)
        {
            ViewBag.Codigo = codigo ?? 0;
            return View();
        }

        [Authorize]
        public ActionResult InformativoTercero(int? codigo, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;
            bool seleccionTercero = false;

            var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

            if (rolesUsuario != null && rolesUsuario.Count() > 0)
            {
                if (t != null)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }

                seleccionTercero = true;
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            var tercero = (from ter in dbSIM.TERCERO where ter.ID_TERCERO == idTercero select ter.S_RSOCIAL).FirstOrDefault();

            ViewBag.idTercero = idTercero;
            ViewBag.NombreTercero = tercero;
            ViewBag.seleccionTercero = seleccionTercero;

            ViewBag.Codigo = codigo ?? 0;

            return View();
        }

        [Authorize]
        public ActionResult Documentos(int? id)
        {
            INFORMATIVO_DOC publicacion = null;
            bool soloLectura = false;
            string tercero = "";

            if ((id ?? 0) > 0)
            {
                publicacion = dbSIM.INFORMATIVO_DOC.Where(idoc => idoc.ID_INFORMATIVO_DOC == id).FirstOrDefault();

                //soloLectura = !(dbSIM.INFORMATIVO_DOC.Where(idoc => idoc.ID_INFORMATIVO_DOC == id).Select(pd => pd.S_FORMULARIO).FirstOrDefault() == "20");
                soloLectura = !(publicacion.S_FORMULARIO == "20");
            }

            if (publicacion != null)
            {
                if (publicacion.ID_TERCERO > 0)
                {
                    tercero = (from ter in dbSIM.TERCERO where ter.ID_TERCERO == publicacion.ID_TERCERO select ter.S_RSOCIAL).FirstOrDefault();

                    ViewBag.idTercero = publicacion.ID_TERCERO;
                    ViewBag.NombreTercero = tercero;
                }
            }

            ViewBag.id = id ?? 0;
            ViewBag.time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            ViewBag.SoloLectura = (soloLectura ? "S" : "N");
            ViewBag.Popup = false;

            return View();
        }

        /*[Authorize]
        [HttpGet, ActionName("DocumentoPreview")]
        public ActionResult GetDocumentoPreview(int id)
        {
            TramitesLibrary utilidad = new TramitesLibrary();

            //var documento = utilidad.DocumentoRadicado(id, true, 13533, false, "");
            var documento = utilidad.DocumentoRadicado(id, false, 0, true, "");

            return File(documento.documentoBinario, "application/pdf");
        }*/

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

                    targetLocation = Server.MapPath("~/App_Data/Temporal/MI");
                    rutaArchivo = Path.Combine(targetLocation, idUsuario.ToString() + "__" + id.ToString() + "__" + nombreArchivo);
                }
                else
                {
                    rutaArchivo = null;
                }
            }
            else
            {
                INFORMATIVO_DOC_DET_ARCH archivo = dbSIM.INFORMATIVO_DOC_DET_ARCH.Where(da => da.ID_INFORMATIVO_DOC_ARCHIVOS == id && da.S_ACTIVO == "S").FirstOrDefault();

                if (archivo != null)
                {
                    rutaArchivo = archivo.S_RUTA_ARCHIVO;
                    nombreArchivo = archivo.INFORMATIVO_DOC_ARCHIVOS.S_DESCRIPCION + Path.GetExtension(rutaArchivo);
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

        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase file, int ida)
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

            targetLocation = Server.MapPath("~/App_Data/Temporal/MI");

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

        // No funciona si el documento fue firmado digitalmente, porque no tenemos la contraseña de la firma digital
        /*[HttpGet, ActionName("RegenerarDocumento")]
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
        }*/

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
