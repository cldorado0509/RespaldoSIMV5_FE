using DevExpress.XtraReports.UI;
using SIM.Areas.ControlVigilancia.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Net;
using System.Reflection;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using SIM.Data;
using System.Data.SqlClient;
using System.Web.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using SIM.Utilidades;
using System.Text;
using Xceed.Words.NET;
using SIM.Data.Control;
using System.Data.Entity;
using SIM.Areas.Models;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    [Authorize]
    public class InformeVisitaController : Controller
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;

        public ActionResult Index()
        {
            return View("");
        }
        public ActionResult RevisarInfTecnico()
        {
            var indicesSerieDocumental = from i in db.TBINDICESERIE
                                         where i.CODSERIE == 62 && i.MOSTRAR == "1" && i.CODINDICE != 381 && i.CODINDICE != 382 && i.CODINDICE != 2740
                                         orderby i.ORDEN
                                         select i;

            ViewBag.Indices = indicesSerieDocumental.ToList();
            return View();
        }
        public ActionResult cargarDoc()
        {
            return View();
        }
        public ActionResult InformeVisitas()
        {
            return View();
        }
        public ActionResult InformeTecnico()
        {
            return View();
        }
        public ActionResult NuevoInformeTec()
        {
            return View();
        }

        public ActionResult AsignarTramite()
        {
            return View();
        }
        public ActionResult AprobarInfTecnico()
        {
            return View();
        }
        public ActionResult responsableInf()
        {
            return View();
        }
        public ActionResult cargarDocumentoInfTec()
        {
            return View();
        }

        public ActionResult RadicarInformeTecnico()
        {
            return View();
        }

        public ActionResult ConsultarInfVisita(int tramite)
        {
            //if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            //{
            //    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            //    codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
            //}
            String sql = "select distinct v.id_visita,v.s_asunto,v.s_observacion from control.VISITA v inner join control.tramite_visita tv on tv.id_visita=v.id_visita where tv.id_tramite='" + tramite + "'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);

        }
        public ActionResult consultarVisitaFinalizada()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }

            //String sql = "select distinct ID_VISITA,S_ASUNTO,S_OBSERVACION from control.VW_VISITAFINALIZADA";
            String sql = "SELECT DISTINCT v.ID_VISITA, v.S_ASUNTO, v.S_OBSERVACION " +
                        "FROM CONTROL.VISITA v INNER JOIN " +
                        "    CONTROL.VISITAESTADO ve ON v.ID_VISITA = ve.ID_VISITA AND ve.D_FIN IS NULL INNER JOIN " +
                        "    CONTROL.TRAMITE_VISITA tv ON v.ID_VISITA = tv.ID_VISITA INNER JOIN " +
                        "    TRAMITES.TBTRAMITETAREA tt ON tv.ID_TRAMITE = tt.CODTRAMITE AND tt.ESTADO = 0 AND tt.FECHAFIN IS NULL AND tt.COPIA = 0 AND tt.CODFUNCIONARIO = " + codFuncionario.ToString() + " LEFT OUTER JOIN " +
                        "    CONTROL.INFORME_TECNICO it ON v.ID_VISITA = it.ID_VISITA " +
                        "WHERE ve.ID_ESTADOVISITA = 5 AND it.ID_VISITA IS NULL";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarTramitesVisita(decimal idVisita)
        {
            String sql = "SELECT distinct TRAMITE_VISITA.ID_TRAMITE FROM control.TRAMITE_VISITA where TRAMITE_VISITA.ID_VISITA=" + idVisita;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult consultarVisitaDetalle(int id)
        {
            //String sql = "select distinct ID_VISITA,S_ASUNTO,S_OBSERVACION from control.VW_VISITAFINALIZADA where ID_VISITA="+id;

            String sql = "SELECT DISTINCT v.ID_VISITA, v.S_ASUNTO, v.S_OBSERVACION " +
                        "FROM CONTROL.VISITA v INNER JOIN " +
                        "    CONTROL.VISITAESTADO ve ON v.ID_VISITA = ve.ID_VISITA AND ve.D_FIN IS NULL" +
                        "WHERE ve.ID_ESTADOVISITA = 5 AND v.ID_VISITA = " + id.ToString();

            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult consultarTarea(int idTramite)
        {
            String sql = "select distinct CODTAREA from tramites.TBTRAMITETAREA  where ESTADO=0 and CODTRAMITE='" + idTramite + "'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult consultarInformeTecnico()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2 FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF=1 and INFORME_TECNICO.FUNCIONARIO=" + codFuncionario + "";
            String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.FUNCIONARIOACT,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.ID_RADICADO FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF=1 and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult consultarMisInformeTecnico()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2 FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.FUNCIONARIO=" + codFuncionario + "";
            String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2, INFORME_TECNICO.ID_RADICADO FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarInformeTecnicoRevision()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF in(2,3) and INFORME_TECNICO.FUNCIONARIO=" + codFuncionario + "";
            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT, INFORME_TECNICO.ID_RADICADO FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF = 2 and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + "";
            String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT, INFORME_TECNICO.ID_RADICADO, MAX(TBTRAMITETAREA.FECHAINI) AS FECHA_REVISION " +
                "FROM control.INFORME_TECNICO INNER JOIN " +
                "control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF LEFT OUTER JOIN " +
                "control.TRAMITE_VISITA ON INFORME_TECNICO.ID_VISITA = TRAMITE_VISITA.ID_VISITA LEFT OUTER JOIN " +
                "tramites.TBTRAMITETAREA ON TRAMITE_VISITA.ID_TRAMITE = TBTRAMITETAREA.CODTRAMITE AND TBTRAMITETAREA.ESTADO = 0 " +
                "where INFORME_TECNICO.ID_ESTADOINF = 2 and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + " " +
                "GROUP BY INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT, INFORME_TECNICO.ID_RADICADO " +
                "ORDER BY MAX(TBTRAMITETAREA.FECHAINI)";

            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2 FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF in(2,3) and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);


        }

        public ActionResult consultarInformeTecnicoRevisionAprobados()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF in(2,3) and INFORME_TECNICO.FUNCIONARIO=" + codFuncionario + "";
            String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2,INFORME_TECNICO.FUNCIONARIOACT, INFORME_TECNICO.ID_RADICADO, NVL(INFORME_TECNICO.TRAMITE_AVANZADO, 'N') AS TRAMITE_AVANZADO FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF = 3 and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + " ORDER BY NVL(INFORME_TECNICO.TRAMITE_AVANZADO, 'N'), INFORME_TECNICO.ID_INF DESC";

            //String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2 FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF in(2,3) and INFORME_TECNICO.FUNCIONARIOACT=" + codFuncionario + "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult EnviarRevision(string idTramite, decimal codTarea, string copia, decimal CodResponsable, decimal idVisita)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {
                var rutaInforme = (from informeTecnico in db.INFORME_TECNICO
                                   where informeTecnico.ID_VISITA == idVisita
                                   select informeTecnico.URL).FirstOrDefault();

                if (rutaInforme != null && rutaInforme.Trim() != "")
                {
                    foreach (string idTramiteSel in idTramite.Split(','))
                    {
                        if (idTramiteSel != null && idTramiteSel.Trim() != "")
                            Utilidades.Data.AlmacenarDocumentosTemporalesTramite(Convert.ToDecimal(idTramiteSel), codTarea, codFuncionario, rutaInforme, "Informe Técnico (Pendiente Revisión)");
                    }
                }

                dbControl.SP_ADD_TRAMITE_INFORME(idTramite, codFuncionario, copia, CodResponsable, codTarea, 0);
                dbControl.SP_MODIFICAR_INFTECNICO(idVisita, CodResponsable, 2);
            }
            catch (Exception e)
            {
                return Content("0");
            }
            return Content("1");
        }

        public ActionResult previeRTF(decimal id)
        {
            String nombre = "";
            var stream = new MemoryStream();

            string sql = "SELECT distinct VWR_DETALLEINFTECNICO.IDTRAMITE, VWR_DETALLEINFTECNICO.IDVISITA, VWR_DETALLEINFTECNICO.EMPRESA, VWR_DETALLEINFTECNICO.DIRECCION, VWR_DETALLEINFTECNICO.MUNICIPIO, VWR_DETALLEINFTECNICO.NOMBRE_INSTALACION, VWR_DETALLEINFTECNICO.TELEFONO_INSTALACION, VWR_DETALLEINFTECNICO.CM, VWR_DETALLEINFTECNICO.REPRESENTANTE_LEGAL, VWR_DETALLEINFTECNICO.NRO_DOCUMENTO, VWR_DETALLEINFTECNICO.ID_TERCERO, VWR_DETALLEINFTECNICO.ID_INSTALACION, VWR_DETALLEINFTECNICO.QUEJA, VWR_DETALLEINFTECNICO.ANO, VWR_DETALLEINFTECNICO.ABOGADO FROM control.VWR_DETALLEINFTECNICO WHERE VWR_DETALLEINFTECNICO.IDVISITA = " + id.ToString();

            DatosVisita consulta = ((DbContext)db).Database.SqlQuery<DatosVisita>(sql).FirstOrDefault();

            if (consulta == null)
                consulta = new DatosVisita();

            DocX document = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/InformeTecnico.docx"));

            document.ReplaceText("{INSTALACION}", consulta.NOMBRE_INSTALACION ?? "");
            document.ReplaceText("{EMPRESA}", consulta.EMPRESA ?? "");
            document.ReplaceText("{REPRESENTANTE_LEGAL}", consulta.REPRESENTANTE_LEGAL ?? "");
            document.ReplaceText("{NIT}", consulta.NRO_DOCUMENTO == null ? "" : consulta.NRO_DOCUMENTO.ToString());
            document.ReplaceText("{DIRECCION}", consulta.DIRECCION ?? "");
            document.ReplaceText("{TELEFONO}", consulta.TELEFONO_INSTALACION ?? "");
            document.ReplaceText("{MUNICIPIO}", consulta.MUNICIPIO ?? "");
            document.ReplaceText("{CM}", consulta.CM ?? "");
            document.ReplaceText("{QUEJA}", consulta.QUEJA ?? "");
            document.ReplaceText("{ANO}", consulta.ANO == null ? "" : consulta.ANO.ToString());
            document.ReplaceText("{ABOGADO}", consulta.ABOGADO ?? "");
            document.ReplaceText("{TRAMITE}", consulta.IDTRAMITE.ToString());

            document.SaveAs(stream);




            /*var report10 = new SIM.Areas.ControlVigilancia.reportInfoTec.infTecnico();
            DevExpress.XtraReports.Parameters.Parameter idVisita = report10.Parameters["prm_idVisita"];
            idVisita.Value = id;
            report10.ExportToRtf(stream);*/
            nombre = "InformeTecnico.docx";
            return File(stream.GetBuffer(), "application/docx", nombre);
        }

        private static bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }

        [HttpGet, ActionName("DocumentoActuacion")]
        public ActionResult DocumentoActuacion(int id)
        {
            INFORME_TECNICO informeTecnico = db.INFORME_TECNICO.Where(it => it.ID_VISITA == id).FirstOrDefault();

            if (informeTecnico != null)
            {
                //var response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new StreamContent(stream);
                //response.Content = new ByteArrayContent(System.IO.File.ReadAllBytes(informeTecnico.URL));
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

                if (System.IO.File.Exists(informeTecnico.URL))
                    return File(System.IO.File.ReadAllBytes(informeTecnico.URL), "application/" + Path.GetExtension(informeTecnico.URL).Replace(".", ""), "InformeTecnico_" + id.ToString() + Path.GetExtension(informeTecnico.URL));
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public ActionResult guardarUrl(decimal idVisita, String fecha)
        {
            string url = "";
            string filename = "";
            string rutaLeer = ConfigurationManager.AppSettings["dir_doc_inf_salidadGuardar"];
            string url2 = ConfigurationManager.AppSettings["dir_doc_inf_leer"];

            fecha = DateTime.Now.ToString("yyyyMMddHHmmss");

            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload]))
                    continue;

                filename = Path.GetFileName(Request.Files[upload].FileName);

                if (!Directory.Exists(rutaLeer + "\\" + idVisita))
                {
                    Directory.CreateDirectory(rutaLeer + "\\" + idVisita);
                }

                Request.Files[upload].SaveAs(Path.Combine(rutaLeer + "\\" + idVisita, idVisita + "-" + fecha + "Radicado" + Path.GetExtension(filename)));
            }
            dbControl.SP_GUADAR_INF_URL(idVisita, rutaLeer + "\\" + idVisita + "\\" + idVisita + "-" + fecha + "Radicado" + Path.GetExtension(filename), url2 + idVisita + "/" + idVisita + "-" + fecha + "Radicado" + Path.GetExtension(filename));
            //try
            //{
            //    if (!Directory.Exists(rutaLeer+"\\"+ idVisita))
            //    {
            //        Directory.CreateDirectory(rutaLeer + "\\" + idVisita);
            //    }
            //    WebClient carpeta = new WebClient();
            //   // carpeta.Credentials = new NetworkCredential("jorge.mora", "junio2015");
            //   //carpeta.DownloadFile(new Uri(url), "D:\\informeTecnico.rtf");

            //    NetworkCredential myCredentials = new NetworkCredential("jorge.mora", "junio2015");


            //    carpeta.Credentials = CredentialCache.DefaultCredentials; 

            //    carpeta.DownloadFile(url, rutaLeer + "\\" + idVisita + "\\" + idVisita + "nuevo.rtf");
            //   db.SP_GUADAR_INF_URL(idVisita, rutaLeer + "\\" + idVisita + "\\" + idVisita + "nuevo.rtf", url2 + idVisita + "/" + idVisita + "nuevo.rtf");
            //}
            //catch(Exception ex)
            //{
            //    return new HttpStatusCodeResult(404,ex.Message);
            //}

            return Content("1");
        }

        public ActionResult converRTFaPDF(string idRadicado, string url, decimal id)
        {

            WebClient carpeta = new WebClient();

            string rutaLeer = ConfigurationManager.AppSettings["dir_doc_inf_leer"];
            WebClient webClient = new WebClient();
            string path = ConfigurationManager.AppSettings["url_radicador"];
            string path2 = ConfigurationManager.AppSettings["dir_doc_inf_salidadGuardar"];

            string url2 = url.Replace(".rtf", ".pdf");
            String urlRad = path + idRadicado + "&formatoRetorno=png";
            RichEditDocumentServer server = new RichEditDocumentServer();

            server.LoadDocument(@url, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
            carpeta.DownloadFile(urlRad, path2 + "\\" + id + "\\" + id + "radicado.png");

            FileStream fsOut = System.IO.File.Open(@url2, FileMode.Create);
            server.ExportToPdf(fsOut);
            server.Dispose();
            fsOut.Close();
            Stream inputPdfStream = new FileStream(url2, FileMode.Open, FileAccess.Read, FileShare.Read);

            //Stream inputPdfStream = new FileStream(path2 + "\\" + id + "\\" + id + "nuevo.pdf", FileMode.Open, FileAccess.Read, FileShare.Read);
            Stream inputImageStream = new FileStream(path2 + "\\" + id + "\\" + id + "radicado.png", FileMode.Open, FileAccess.Read, FileShare.Read);
            Stream outputPdfStream = new FileStream(path2 + "\\" + id + "\\" + id + "nuevo2.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, outputPdfStream);
            var pdfContentByte = stamper.GetOverContent(1);

            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
            image.SetAbsolutePosition(100, 600);
            pdfContentByte.AddImage(image);
            stamper.Close();
            inputPdfStream.Close();
            inputImageStream.Close();
            outputPdfStream.Close();
            dbControl.SP_GUADAR_INF_URL(id, rutaLeer + "//" + id + "//" + id + "nuevo2.pdf", rutaLeer + "//" + id + "//" + id + "nuevo2.pdf");
            return Content("1");
        }

        public ActionResult guardarTramiteInf(string tramites, decimal tareaSiguiente, decimal idVisita)
        {


            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {

                dbControl.SP_ADD_VISITA_INF(tramites, codFuncionario, idVisita, tareaSiguiente);

            }

            catch
            {
                return Content("0");
            }
            return Content("1");
        }


        public ActionResult guardarInfTecnico(string asunto, string observacion, decimal estado, decimal idVisita)
        {


            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {
                dbControl.SP_GUADAR_INFORMETECNICO(asunto, observacion, estado, idVisita, codFuncionario);

            }

            catch
            {
                return Content("0");
            }
            return Content("1");
        }

        public ActionResult ConsultarTareaSiguiente(decimal idVisita)
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_CONSULTARTAREASIG_VISITA(idVisita, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult ConsultarInformeTecnicoRadicado(int idVisita)
        {
            INFORME_TECNICO informeTecnico = db.INFORME_TECNICO.Where(it => it.ID_VISITA == idVisita).FirstOrDefault();

            if (informeTecnico != null)
            {
                if (System.IO.File.Exists(informeTecnico.URL2))
                {
                    Cryptografia crypt = new Cryptografia();

                    MemoryStream ms = crypt.DesEncriptar(informeTecnico.URL2, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
                    return File(ms.GetBuffer(), "application/" + Path.GetExtension(informeTecnico.URL2).Replace(".", ""), "InformeTecnico_" + idVisita.ToString() + Path.GetExtension(informeTecnico.URL2));
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
    }
}