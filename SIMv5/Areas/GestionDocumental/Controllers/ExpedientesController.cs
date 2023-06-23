using DevExpress.Pdf;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.GestionDocumental.Controllers
{
    [Authorize(Roles = "VEXPEDIENTES")]
    public class ExpedientesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        // GET: Capacitacion/Capacitacion
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdExp"></param>
        /// <returns></returns>
        public ActionResult Tomos(int? IdExp)
        {
            if (IdExp > 0) {
                var Expediente = (from Exp in dbSIM.EXP_EXPEDIENTES where Exp.ID_EXPEDIENTE == IdExp select Exp.S_NOMBRE).FirstOrDefault();
                ViewBag.Expediente = Regex.Replace(Expediente.Trim(), "[^0-9A-Za-z _-]", "");
                ViewBag.IdExpediente = IdExp;
                return View();
            } else return null;
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="IdDocumento">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        [HttpGet, ActionName("LeeDoc")]
        public async Task<ActionResult> GetArchivo(long IdDocumento)
        {
            int idUsuario = 0;
            int funcionario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());

            }
            MemoryStream oStream = await SIM.Utilidades.Archivos.AbrirDocumentoFun(IdDocumento, funcionario);
            if (oStream.Length > 0)
            {
                oStream.Position = 0;
                var Archivo = oStream.GetBuffer();
                return File(Archivo, "application/pdf");
            }
            return null;
        }

        public List<decimal> ObtieneDocumentosExpediente(decimal idExpediente)
        {
            var model = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                         join Tdo in dbSIM.TBTRAMITEDOCUMENTO on Doc.ID_DOCUMENTO equals Tdo.ID_DOCUMENTO
                         join Tom in dbSIM.EXP_TOMOS on Doc.ID_TOMO equals Tom.ID_TOMO
                         where Tom.ID_EXPEDIENTE == idExpediente
                         orderby Doc.N_ORDEN
                         select Doc.ID_DOCUMENTO);
            return model.ToList();
        }

        public List<decimal> ObtieneDocumentosExpediente(decimal idExpediente, decimal idTomo)
        {
            var model = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                         join Tdo in dbSIM.TBTRAMITEDOCUMENTO on Doc.ID_DOCUMENTO equals Tdo.ID_DOCUMENTO
                         join Tom in dbSIM.EXP_TOMOS on Doc.ID_TOMO equals Tom.ID_TOMO
                         where Tom.ID_EXPEDIENTE == idExpediente && Tom.ID_TOMO == idTomo
                         orderby Doc.N_ORDEN
                         select Doc.ID_DOCUMENTO);
            return model.ToList();
        }

        public ActionResult BuscarExpediente()
        {
            return View();
        }

        public ActionResult FlipExpediente(string idExp, string idTomo)
        {
            List<string> listadoSal = new List<string>();

            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                decimal codFuncionario = -1;
                if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    int idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
                    codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                                      join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                                      where uf.ID_USUARIO == idUsuario
                                                      select f.CODFUNCIONARIO).FirstOrDefault());
                }
                int _idExp = 0;
                int _idTomo = 0;
                int.TryParse(idExp, out _idExp);
                int.TryParse(idTomo, out _idTomo);

                ViewBag.CodFuncionario = codFuncionario;

                SIM.Areas.GestionDocumental.Controllers.ExpedientesController expedientesController = new SIM.Areas.GestionDocumental.Controllers.ExpedientesController();

                List<decimal> listado = expedientesController.ObtieneDocumentosExpediente(_idExp, _idTomo);

                string fecha = DateTime.Now.Date.ToFileTimeUtc().ToString();
                string contentPath = "~/Content/ConsultaExpedientesDoc";
                string path = System.IO.Path.Combine(contentPath, fecha);
                if (!System.IO.File.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(path));
                    path = System.IO.Path.Combine($"{contentPath}/{fecha}/{_idExp}/{_idTomo}");
                    if (!System.IO.File.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(path));

                        System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath(path));

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }

                    }
                }
                else
                {
                    path = System.IO.Path.Combine($"{contentPath}/{fecha}/{_idExp}/{_idTomo}");
                    if (!System.IO.File.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(path));
                        System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath(path));

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }

                }

                int doc = 1;
                int largestEdgeLength = 1000;
                foreach (decimal documentoId in listado)
                {
                    System.IO.MemoryStream oStream = SIM.Utilidades.Archivos.AbrirDocumentoFun((long)documentoId, codFuncionario).Result;

                    if (oStream != null)
                    {
                        //byte[] bytes = oStream.ToArray();

                        path = System.IO.Path.Combine($"{contentPath}/{fecha}/{_idExp}/{_idTomo}/doc{doc}.pdf");
                        try
                        {

                            using (PdfDocumentProcessor processor = new PdfDocumentProcessor())
                            {
                                // Load a document.
                                processor.LoadDocument(oStream);

                                for (int i = 1; i <= processor.Document.Pages.Count; i++)
                                {
                                    // Export pages to bitmaps.
                                    Bitmap image = processor.CreateBitmap(i, largestEdgeLength);

                                    System.IO.MemoryStream oStreamSal = new MemoryStream();

                                    // Save the bitmaps.
                                    image.Save(oStreamSal, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    oStream.Seek(0, SeekOrigin.Begin);
                                    SIM.Utilidades.Archivos.GrabaMemoryStream(oStreamSal, Server.MapPath($"~/Content/ConsultaExpedientesDoc/{fecha}/{_idExp}/{_idTomo}/doc{doc}.jpg"));

                                    listadoSal.Add($"/Content/ConsultaExpedientesDoc/{fecha}/{_idExp}/{_idTomo}/doc{doc}.jpg");
                                    doc++;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return View(listadoSal);
        }

        private bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(Server.MapPath(fileName), FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
    }
}