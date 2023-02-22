using O2S.Components.PDF4NET;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Areas.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Contratos.Controllers
{
    [Authorize]
    public class PropuestaController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        decimal codFuncionario = -1;

        // GET: Contratos/Propuesta
        public ActionResult Index()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            ViewBag.CodFuncionario = codFuncionario;
            return View();
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="Propuesa">Codigo de la propuesta en el sistema</param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeDoc")]
        public async Task<ActionResult> GetArchivo(long Propuesta)
        {
            if (Propuesta == 0) return null;
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            MemoryStream oStream = new MemoryStream();
            var DatPropuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                             join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                             where Ppta.ID_PROPUESTA == Propuesta && Ppta.S_ABIERTA_PPTA == "1"
                             select new
                             {
                                 Ppta.ID_RADICADO,
                                 Ppta.ARCHIVOPROPUESTA,
                                 Pro.B_SOBRESELLADO
                             }).FirstOrDefault();
            if (DatPropuesta != null)
            {
                if (DatPropuesta.B_SOBRESELLADO == "1")
                {
                    if (DatPropuesta.ARCHIVOPROPUESTA.Length > 0)
                    {
                        PDFDocument Origen = new PDFDocument(new MemoryStream(DatPropuesta.ARCHIVOPROPUESTA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                        Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                        Origen.SecurityManager = null;
                        MemoryStream imagenEtiqueta = new MemoryStream();
                        var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea((int)DatPropuesta.ID_RADICADO);
                        if (imagenRadicado != null)
                        {
                            PDFPage _pag = Origen.Pages[0];
                            _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                            Origen.Save(oStream);
                            oStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                    else return null;
                }
                else
                {
                    var Radicado = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == DatPropuesta.ID_RADICADO select new { Rad.CODTRAMITE, Rad.CODDOCUMENTO }).FirstOrDefault();
                    if (Radicado != null) oStream = await SIM.Utilidades.Archivos.AbrirDocumento((long)Radicado.CODTRAMITE, (long)Radicado.CODDOCUMENTO);
                    oStream.Seek(0, SeekOrigin.Begin);
                }
            }
            if (oStream != null && oStream.Length > 0)
            {
                var Descarga = new FileStreamResult(oStream, "application/pdf");
                return Descarga;
            }
            return null;
        }

        [HttpGet, ActionName("DescargaPropuesta")]
        public async Task<FileContentResult> Download(long Propuesta)
        {
            if (Propuesta == 0) return null;
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            MemoryStream oStream = new MemoryStream();
            var DatPropuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                                where Ppta.ID_PROPUESTA == Propuesta && Ppta.S_ABIERTA_PPTA == "1"
                                select new
                                {
                                    Ppta.ID_RADICADO,
                                    Ppta.ARCHIVOPROPUESTA,
                                    Pro.N_FUNCIONARIO_CONTRATO,
                                    Pro.B_SOBRESELLADO                                   
                                }).FirstOrDefault();
            if (DatPropuesta != null)
            {
                if (DatPropuesta.N_FUNCIONARIO_CONTRATO == codFuncionario)
                {
                    if (DatPropuesta.B_SOBRESELLADO == "1")
                    {
                        if (DatPropuesta.ARCHIVOPROPUESTA.Length > 0)
                        {
                            PDFDocument Origen = new PDFDocument(new MemoryStream(DatPropuesta.ARCHIVOPROPUESTA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                            Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                            Origen.SecurityManager = null;
                            MemoryStream imagenEtiqueta = new MemoryStream();
                            var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea((int)DatPropuesta.ID_RADICADO);
                            if (imagenRadicado != null)
                            {
                                PDFPage _pag = Origen.Pages[0];
                                _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                                Origen.Save(oStream);
                                oStream.Seek(0, SeekOrigin.Begin);
                            }
                        }
                        else return File(new byte[0], "application/pdf", "Propuesta.pdf");
                    }else
                    {
                        var Radicado = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == DatPropuesta.ID_RADICADO select new { Rad.CODTRAMITE, Rad.CODDOCUMENTO }).FirstOrDefault();
                        if (Radicado != null) oStream = await SIM.Utilidades.Archivos.AbrirDocumento((long)Radicado.CODTRAMITE, (long)Radicado.CODDOCUMENTO);
                        oStream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            if (oStream != null && oStream.Length > 0)
            {
                oStream.Position = 0;
                var Archivo = oStream.ToArray();
                return  File(Archivo, "application/pdf", "Propuesta.pdf");
            }
            return File(new byte[0], "application/pdf", "Propuesta.pdf");
        }
        /// <summary>
        /// Consulta un archivo del sistema de la propuesta economica y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="Propuesa">Codigo de la propuesta en el sistema</param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeDocEco")]
        public ActionResult GetArchivoEco(long Propuesta)
        {
            if (Propuesta == 0) return null;
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            MemoryStream oStream = new MemoryStream();
            var DatPropuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                                where Ppta.ID_PROPUESTA == Propuesta && Ppta.S_ABIERTA_ECO == "1"
                                select new
                                {
                                    Ppta.ID_RADECO,
                                    Ppta.ARCHICOECONOMICA
                                }).FirstOrDefault();
            if (DatPropuesta != null)
            {
                if (DatPropuesta.ARCHICOECONOMICA.Length > 0)
                {
                    PDFDocument Origen = new PDFDocument(new MemoryStream(DatPropuesta.ARCHICOECONOMICA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                    Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                    Origen.SecurityManager = null;
                    MemoryStream imagenEtiqueta = new MemoryStream();
                    var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea((int)DatPropuesta.ID_RADECO);
                    if (imagenRadicado != null)
                    {
                        PDFPage _pag = Origen.Pages[0];
                        _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                        Origen.Save(oStream);
                        oStream.Seek(0, SeekOrigin.Begin);
                    }
                }
                else return null;
            }
            if (oStream != null && oStream.Length > 0)
            {
                var Descarga = new FileStreamResult(oStream, "application/pdf");
                return Descarga;
            }
            return null;
        }

        [HttpGet, ActionName("DescargaEconomica")]
        public FileContentResult DownloadEco(long Propuesta)
        {
            if (Propuesta == 0) return null;
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            MemoryStream oStream = new MemoryStream();
            var DatPropuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                                where Ppta.ID_PROPUESTA == Propuesta && Ppta.S_ABIERTA_ECO == "1"
                                select new
                                {
                                    Ppta.ID_RADECO,
                                    Ppta.ARCHICOECONOMICA,
                                    Pro.N_FUNCIONARIO_CONTRATO
                                }).FirstOrDefault();
            if (DatPropuesta != null)
            {
                if (DatPropuesta.N_FUNCIONARIO_CONTRATO == codFuncionario)
                {

                    if (DatPropuesta.ARCHICOECONOMICA.Length > 0)
                    {
                        PDFDocument Origen = new PDFDocument(new MemoryStream(DatPropuesta.ARCHICOECONOMICA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                        Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                        Origen.SecurityManager = null;
                        MemoryStream imagenEtiqueta = new MemoryStream();
                        var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea((int)DatPropuesta.ID_RADECO);
                        if (imagenRadicado != null)
                        {
                            PDFPage _pag = Origen.Pages[0];
                            _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                            Origen.Save(oStream);
                            oStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                    else return null;
                }
            }
            if (oStream != null && oStream.Length > 0)
            {
                oStream.Position = 0;
                var Archivo = oStream.ToArray();
                return File(Archivo, "application/pdf", "PropuestaEconomica.pdf");
            }
            return File(new byte[0], "application/pdf", "PropuestaEconomica.pdf");
        }
    }
}