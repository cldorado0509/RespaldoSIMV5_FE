namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    using DevExpress.Pdf;
    using SIM.Areas.Seguridad.Models;
    using SIM.Data;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    ///Controller Expedientes
    /// </summary>
    public class ExpedientesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// GET: ExpedienteAmbiental/Expedientes
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var _IdUsuario = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("nameidentifier")).FirstOrDefault();
            decimal IdUsuario = 0;
            decimal.TryParse(_IdUsuario.Value, out IdUsuario);

            decimal codFuncionario = -1;
            codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                              join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                              where uf.ID_USUARIO == IdUsuario
                                              select f.CODFUNCIONARIO).FirstOrDefault());
            string actionName = RouteData.Values["action"].ToString();
            string controllerName = RouteData.Values["controller"].ToString();
            string areaName = RouteData.DataTokens["area"].ToString();
            PermisosRolModel permisos = SIM.Utilidades.Security.PermisosFormulario(areaName, controllerName, actionName, IdUsuario);

            ViewBag.CodFuncionario = codFuncionario;
            ViewBag.CodigoUnidadDocumental = SIM.Utilidades.Data.ObtenerValorParametro("IdCodSerieHistoriasAmbientales").ToString();

            //var idForma = 0;
            //int.TryParse(SIM.Utilidades.Data.ObtenerValorParametro("IdFormaExpedientesAmbientales").ToString(), out idForma);


            //PermisosRolModel permisosRolModel = new PermisosRolModel { CanDelete=false, CanInsert=false, CanPrint=false, CanRead= false, CanUpdate = false, IdRol = 0 };

            //Permisos permisos = new Permisos();
            //permisosRolModel = permisos.ObtenerPermisosRolForma(idForma, idUsuario);

            ViewBag.CodFuncionario = codFuncionario;
            return View(permisos);

        }

        /// <summary>
        /// GET: ExpedienteAmbiental/Expedientes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FlipExpediente(string id)
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
                int _id = 0;

                int.TryParse(id, out _id);

                ViewBag.CodFuncionario = codFuncionario;

                SIM.Areas.GestionDocumental.Controllers.ExpedientesController expedientesController = new SIM.Areas.GestionDocumental.Controllers.ExpedientesController();

                List<decimal> listado = expedientesController.ObtieneDocumentosExpediente(_id);

                string fecha = DateTime.Now.Date.ToFileTimeUtc().ToString();
                string contentPath = "~/Content/ConsultaExpedientesDoc";
                string path = System.IO.Path.Combine(contentPath, fecha);
                if (!System.IO.File.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(path));
                    path = System.IO.Path.Combine($"{contentPath}/{fecha}/{id}");
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
                    path = System.IO.Path.Combine($"{contentPath}/{fecha}/{id}");
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

                        path = System.IO.Path.Combine($"{contentPath}/{fecha}/{id}/doc{doc}.pdf");
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
                                    SIM.Utilidades.Archivos.GrabaMemoryStream(oStreamSal, Server.MapPath($"~/Content/ConsultaExpedientesDoc/{fecha}/{id}/doc{doc}.jpg"));

                                    listadoSal.Add($"/Content/ConsultaExpedientesDoc/{fecha}/{id}/doc{doc}.jpg");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ResolveUrl(string relativeUrl)
        {
            if (relativeUrl == null) throw new ArgumentNullException("relativeUrl");

            if (relativeUrl.Length == 0 || relativeUrl[0] == '/' || relativeUrl[0] == '\\')
                return relativeUrl;

            int idxOfScheme = relativeUrl.IndexOf(@"://", StringComparison.Ordinal);
            if (idxOfScheme != -1)
            {
                int idxOfQM = relativeUrl.IndexOf('?');
                if (idxOfQM == -1 || idxOfQM > idxOfScheme) return relativeUrl;
            }

            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(HttpRuntime.AppDomainAppVirtualPath);
            if (sbUrl.Length == 0 || sbUrl[sbUrl.Length - 1] != '/') sbUrl.Append('/');

            // found question mark already? query string, do not touch!
            bool foundQM = false;
            bool foundSlash; // the latest char was a slash?
            if (relativeUrl.Length > 1
                && relativeUrl[0] == '~'
                && (relativeUrl[1] == '/' || relativeUrl[1] == '\\'))
            {
                relativeUrl = relativeUrl.Substring(2);
                foundSlash = true;
            }
            else foundSlash = false;
            foreach (char c in relativeUrl)
            {
                if (!foundQM)
                {
                    if (c == '?') foundQM = true;
                    else
                    {
                        if (c == '/' || c == '\\')
                        {
                            if (foundSlash) continue;
                            else
                            {
                                sbUrl.Append('/');
                                foundSlash = true;
                                continue;
                            }
                        }
                        else if (foundSlash) foundSlash = false;
                    }
                }
                sbUrl.Append(c);
            }

            return sbUrl.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
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