namespace SIM.Controllers
{
    using SIM.Data;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;
    using System.IO;
    using System.Security.Claims;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Net.Http;
    using DevExtreme.AspNet.Mvc;
    using DevExpress.CodeParser;
    using DocumentFormat.OpenXml.EMMA;
    using Antlr.Runtime.Misc;
    using DevExpress.Web.Internal;
    using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

    [Authorize]
    public class UtilidadesController : Controller
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }


        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;


        public ActionResult Documento(string url)
        {
            ViewBag.url = url;
            return View();
        }

        public ActionResult BuscarDocumento(bool popup)
        {
            if (popup) ViewBag.Popup = true;
            else ViewBag.Popup = false;
            //ViewBag.Parametro = Parametro != null ? Parametro.Length > 0 ? Parametro :"" : "";
            return View();
        }

        public ActionResult BuscarExpediente(bool popup)
        {
            if (popup) ViewBag.Popup = true;
            else ViewBag.Popup = false;
            return View();
        }

        public ActionResult BuscarTramite(bool popup)
        {
            if (popup) ViewBag.Popup = true;
            else ViewBag.Popup = false;
            return View();
        }
        [Authorize]
        public ActionResult DetalleTramite(bool popup, int CodTramite, int? Orden)
        {
            if (popup) ViewBag.Popup = true;
            else ViewBag.Popup = false;
            int idUsuario = 0;
            decimal funcionario = 0;
            decimal UltFun = -1;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());

            }

            if (Orden != null)
            {
                UltFun = (from Tta in dbSIM.TBTRAMITETAREA
                          where Tta.CODTRAMITE == CodTramite && Tta.ORDEN == Orden && Tta.ESTADO == 0
                          select Tta.CODFUNCIONARIO).FirstOrDefault();
                if (UltFun <= 0) UltFun = -1;
            }
            else
            {
                var Ord = dbSIM.TBTRAMITETAREA.Where(w => w.CODTRAMITE.Equals(CodTramite) && w.COPIA == 0).Select(s => s.ORDEN).ToList();
                if (Ord.Count > 0) Orden = (int)Ord.Max();
                else Orden = 1;           
            }

            var TraOp = dbSIM.TBTRAMITE.Where(w => w.CODTRAMITE == CodTramite).Select(s => s.ESTADO).FirstOrDefault();
            var TramiteTarea = (from TR in dbSIM.TBTRAMITE
                                join PRO in dbSIM.TBPROCESO on TR.CODPROCESO equals PRO.CODPROCESO
                                join TT in dbSIM.TBTRAMITETAREA on TR.CODTRAMITE equals TT.CODTRAMITE
                                join FUN in dbSIM.QRY_FUNCIONARIO_ALL on TT.CODFUNCIONARIO equals FUN.CODFUNCIONARIO
                                join TAR in dbSIM.TBTAREA on TT.CODTAREA equals TAR.CODTAREA
                                where TT.CODTRAMITE == CodTramite && TT.ORDEN == Orden
                                select new SIM.Models.TramiteTarea
                                {
                                    CodTramite = (int)TR.CODTRAMITE,
                                    FechaIncioTramite = TR.FECHAINI,
                                    Proceso = PRO.NOMBRE,
                                    Tarea = TAR.NOMBRE,
                                    FechaIniciaTarea = TT.FECHAINI,
                                    TipoTarea = TT.COPIA == 0 ? "Tarea de mi responsabilidad" : "Para mi conocimiento",
                                    Funcionario = FUN.NOMBRES,
                                    QueDeboHacer = TAR.OBJETIVO != null ? TAR.OBJETIVO : "No documentado. (Solicite la documentación al administrador del sistema!)",
                                    Vital = TR.NUMERO_VITAL,
                                    Orden = TT.ORDEN,
                                    CodFuncionario = funcionario,
                                    Propietario = UltFun == funcionario,
                                    TramiteAbierto = TraOp != 0
                                }).FirstOrDefault();
            if (TramiteTarea == null) 
            {
                TramiteTarea = (from TR in dbSIM.TBTRAMITE
                                join PRO in dbSIM.TBPROCESO on TR.CODPROCESO equals PRO.CODPROCESO
                                where TR.CODTRAMITE == CodTramite 
                                select new SIM.Models.TramiteTarea
                                {
                                    CodTramite = (int)TR.CODTRAMITE,
                                    FechaIncioTramite = TR.FECHAINI,
                                    Proceso = PRO.NOMBRE,
                                    Tarea = "Sin tarea asociada",
                                    FechaIniciaTarea = TR.FECHAINI.Value,
                                    TipoTarea = "Sin tarea asociada",
                                    Funcionario =  "Sin fumcionario asignado",
                                    QueDeboHacer = "Sin tarea asociada",
                                    Vital = TR.NUMERO_VITAL,
                                    Orden = 1,
                                    CodFuncionario = funcionario,
                                    Propietario = false
                                }).FirstOrDefault(); 
            }
            TramiteTarea.Vital = TramiteTarea.Vital != null ? TramiteTarea.Vital : "-1";
            return View(TramiteTarea);
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="IdDocumento">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        [ActionName("LeeDoc")]
        [Authorize]
        public async Task<FileContentResult> GetArchivo(long IdDocumento)
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
            var UniDoc = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.ID_DOCUMENTO == IdDocumento).Select(s => s.CODSERIE).FirstOrDefault();
            if (UniDoc > 0)
            {
                if (FuncionarioPoseePermiso(funcionario, (int)UniDoc))
                {
                    MemoryStream oStream = await SIM.Utilidades.Archivos.AbrirDocumentoFun(IdDocumento, funcionario);
                    if (oStream.Length > 0)
                    {
                        oStream.Position = 0;
                        var Archivo = oStream.ToArray();
                        return File(Archivo, "application/pdf");
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="IdDocumento">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        [ActionName("LeeTemporal")]
        public async Task<FileContentResult> GetTemporal(long IdDocumento)
        {
            Temporal oStream = await SIM.Utilidades.Archivos.AbrirTemporal(IdDocumento);
            string _Mime = "";
            if (oStream.dataFile.Length > 0)
            {
                oStream.dataFile.Position = 0;
                FileInfo _Archivo = new FileInfo(oStream.filName);

                var Archivo = oStream.dataFile.GetBuffer();
                switch (oStream.fileType.ToLower())
                {
                    case ".pdf":
                        _Mime = "application/pdf";
                        break;
                    case ".doc":
                        _Mime = "application/msword";
                        break;
                    case ".docx":
                        _Mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                }
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = _Archivo.Name,
                    Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");
                return File(Archivo, _Mime);
            }
            return null;
        }

        /// <summary>
        /// Elimina un documento 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        [ActionName("EliminaTemporal")]
        [Authorize]
        public ActionResult GetEliminaTemporal(long IdDocumento)
        {
            if (IdDocumento <= 0) return Json(new { returnvalue = false }, JsonRequestBehavior.AllowGet);
            int idUsuario = 0;
            bool resp = false;
            decimal funcionario = 0;
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
            try
            {
                var Temp = (from Tem in dbSIM.DOCUMENTO_TEMPORAL where Tem.ID_DOCUMENTO == IdDocumento && Tem.CODFUNCIONARIO == funcionario select Tem).FirstOrDefault();
                if (Temp != null)
                {
                    dbSIM.DOCUMENTO_TEMPORAL.Remove(Temp);  
                    dbSIM.SaveChanges();
                    System.IO.File.Delete(Temp.S_RUTA);
                    resp = true;
                }
                else resp = false;
            }
            catch
            {
                resp=false;
            }
            return Json(new { returnvalue = resp }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica si el funcionario puede editar los indices del tramite
        /// </summary>
        /// <returns></returns>
        [ActionName("PuedeEditarIndicesTra")]
        [Authorize]
        public ActionResult GetPuedeEditarIndicesTra()
        {
            int idUsuario = 0;
            bool _resp = false;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            if (idUsuario > 0)
            {
                var Permiso = (from P in dbSIM.USUARIO_FORMA
                               where P.ID_USUARIO == idUsuario && P.ID_FORMA == 1191
                               select P).FirstOrDefault();
                if (Permiso != null) _resp = true;
            }
            return Json(new { returnvalue = _resp }, JsonRequestBehavior.AllowGet);
        }

        [ActionName("PuedeEditarIndicesDoc")]
        [Authorize]
        public ActionResult GetPuedeEditarIndicesDoc(decimal IdDoc)
        {
            int idUsuario = 0;
            bool _resp = false;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            if (idUsuario > 0 && IdDoc > 0)
            {
                var UniDoc = (from D in dbSIM.TBTRAMITEDOCUMENTO
                              where D.ID_DOCUMENTO == IdDoc
                              select D.CODSERIE).FirstOrDefault();
                if (UniDoc > 0)
                {
                    var Permiso = (from U in dbSIM.USUARIO
                                   join UF in dbSIM.USUARIO_FUNCIONARIO on U.ID_USUARIO equals UF.ID_USUARIO
                                   join FUD in dbSIM.PERMISOSSERIE on UF.CODFUNCIONARIO equals FUD.CODFUNCIONARIO
                                   where U.ID_USUARIO == idUsuario && FUD.PM == "1" && FUD.CODSERIE == UniDoc
                                   select U).FirstOrDefault();
                    if (Permiso != null) _resp = true;
                }
            }
            return Json(new { returnvalue = _resp }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene un documento desde Vital
        /// </summary>
        /// <param name="IdRadicadoVital">Radicado Vital</param>
        /// <param name="NombreArchivo">Nombre del archivo en Vital</param>
        /// <returns></returns>
        [ActionName("LeeDocVital")]
        public FileContentResult GetLeeDocVital(long IdRadicadoVital, string NombreArchivo)
        {
            if (IdRadicadoVital <= 0 || string.IsNullOrEmpty(NombreArchivo)) return null;
            WSPQ03 ws = new WSPQ03();
            Byte[] _Documento = ws.ObtenerDocumentoRadicacion(IdRadicadoVital, NombreArchivo);
            return File(_Documento, "application/octetstream", NombreArchivo);
        }

        [System.Web.Http.HttpPost]
        public ActionResult CargarArchivoTemp(int Tra, int Version)
        {
            int idUsuario = 0;
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
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
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(_Ruta)) Directory.CreateDirectory(_Ruta);
            var httpRequest = context.Request;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                string filePath = _Ruta + @"\" + Tra.ToString() + "-" + Version.ToString() + "-" + postedFile.FileName;
                postedFile.SaveAs(filePath);
            }
            return new EmptyResult();
        }

        public JArray GetListaUnidades()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Uni in dbSIM.TBSERIE
                             where Uni.S_DEFINEEXPEDIENTE != "1"
                             orderby Uni.NOMBRE
                             select new
                             {
                                 Uni.CODSERIE,
                                 Uni.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public JArray GetListaUnidadesExp()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var vigencia = (from vig in dbSIM.TBVIGENCIA_TRD orderby vig.D_INICIOVIGENCIA descending select vig.COD_VIGENCIA_TRD).FirstOrDefault();
                if (vigencia <= 0)
                {
                    throw new Exception("No se encontró una vigencia de la TRD!");
                }
                var model = (from Uni in dbSIM.TBSERIE
                             join Vud in dbSIM.TBUNIDADESDOC_VIGENCIATRD on Uni.CODSERIE equals Vud.CODUNIDAD_DOCUMENTAL
                             where Uni.S_DEFINEEXPEDIENTE == "1" && Uni.ACTIVO == "1" && Vud.CODVIGENCIA_TRD == vigencia
                             orderby Uni.NOMBRE
                             select new
                             {
                                 Uni.CODSERIE,
                                 Uni.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public JArray GetFields(decimal UniDoc)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Ind in dbSIM.TBINDICESERIE
                             where Ind.CODSERIE == UniDoc
                             select new Fields {
                                 dataField = Ind.INDICE,
                                 dataType = Ind.TIPO == 0 ? "string" :
                                            Ind.TIPO == 5 ? "string" :
                                            Ind.TIPO == 8 ? "string" :
                                            Ind.TIPO == 3 ? "string" :
                                            Ind.TIPO == 1 ? "number" :
                                            Ind.TIPO == 2 ? "date" :
                                            Ind.TIPO == 4 ? "string" :
                                            Ind.TIPO == 6 ? "number" :
                                            Ind.TIPO == 7 ? "number" : ""
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        [ActionName("MotivoDevolucion")]
        public JObject GetMotivoDevolucion(long IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var Docu = (from Tra in dbSIM.TBTRAMITEDOCUMENTO
                            where Tra.ID_DOCUMENTO == IdDocumento
                            select new
                            {
                                Tra.CODTRAMITE,
                                Tra.CODDOCUMENTO
                            }).FirstOrDefault();
                var model = (from Anu in dbSIM.ANULACION_DOC
                             join Mot in dbSIM.MOTIVO_ANULACION on Anu.ID_MOTIVO_ANULACION equals Mot.ID_MOTIVO_ANULACION
                             join Tra in dbSIM.TRAMITES_PROYECCION on Anu.ID_PROYECCION_DOC equals Tra.ID_PROYECCION_DOC
                             where Tra.CODTRAMITE == Docu.CODTRAMITE && Tra.CODDOCUMENTO == Docu.CODDOCUMENTO
                             select new
                             {
                                 Motivo = Mot.S_DESCRIPCION,
                                 Causa = Anu.S_SOLICITUD,
                                 Fecha = Anu.D_FECHA_APROBACION,
                                 TraAnula = Anu.CODTRAMITE_ANULACION
                             }).FirstOrDefault();
                if (model == null)
                {
                    var Error = new { Motivo = "Problemas con la proyección del documento", Causa = "No se encontró una proyeccion para esta anulación", Fecha = "N", TraAnula = 0 };
                    return JObject.FromObject(Error, Js);
                }
                return JObject.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public JArray GetListaProcesos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Pro in dbSIM.TBPROCESO
                             where Pro.ACTIVO == "1"
                             orderby Pro.NOMBRE
                             select new
                             {
                                 Pro.CODPROCESO,
                                 Pro.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public JArray GetIndicesProc(decimal CodProceso)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Ind in dbSIM.TBINDICEPROCESO
                             where Ind.CODPROCESO == CodProceso
                             select new Fields
                             {
                                 dataField = Ind.INDICE,
                                 dataType = Ind.TIPO == 0 ? "string" :
                                            Ind.TIPO == 5 ? "string" :
                                            Ind.TIPO == 8 ? "string" :
                                            Ind.TIPO == 3 ? "string" :
                                            Ind.TIPO == 1 ? "number" :
                                            Ind.TIPO == 2 ? "date" :
                                            Ind.TIPO == 4 ? "string" :
                                            Ind.TIPO == 6 ? "number" :
                                            Ind.TIPO == 7 ? "number" : ""
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public JArray GetListaFuncionarios()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Func in dbSIM.QRY_FUNCIONARIO_ALL
                             where Func.ACTIVO == "1"
                             select new
                             {
                                 Func.CODFUNCIONARIO,
                                 NOMBRE = Func.NOMBRES 
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Determina si un funcionario tiene permisos para ver este tipo de documento
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        [ActionName("FuncionarioPoseePermiso")]
        [Authorize]
        public ActionResult FuncionarioPoseePermiso(int IdDocumento)
        {
            int idUsuario = 0;
            decimal funcionario = 0;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            }
            var IdUniDoc = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.ID_DOCUMENTO == IdDocumento).Select(s => s.CODSERIE).FirstOrDefault();
            bool _resp = FuncionarioPoseePermiso((int)funcionario, (int)IdUniDoc);
            return Json(new { returnvalue = _resp }, JsonRequestBehavior.AllowGet);
        }

        private bool FuncionarioPoseePermiso(int IdFuncionario, int IdUniDoc)
        {
            if (IdFuncionario <= 0) return false;
            if (IdUniDoc <= 0) return false;
            try
            {
                var Permiso = dbSIM.PERMISOSSERIE.Where(w => w.CODFUNCIONARIO == IdFuncionario && w.CODSERIE == IdUniDoc).FirstOrDefault();
                if (Permiso == null) return false;
                else if (Permiso.PC == "1") return true;
                else return false;
            }
            catch { return false; }
        }
    }

    public class Temporal
    {
        public MemoryStream dataFile { get; set; }
        public string fileType { get; set; }
        public string filName { get; set; }
    }
    public class Fields
    {
        public string dataField { get; set; }
        public string dataType { get; set; }
    }
}