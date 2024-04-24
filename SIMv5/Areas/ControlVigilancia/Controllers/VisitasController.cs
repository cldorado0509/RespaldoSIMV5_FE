using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Areas.Seguridad.Controllers;
using SIM.Data;
using SIM.Data.Control;
using SIM.Models;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SIM.Areas.ControlVigilancia.Controllers
{

    public class VisitasController : Controller
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        private SessionActivaController session = new SessionActivaController();
        private AccountController ac = new AccountController();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;

        public ActionResult Index()
        {

            ViewBag.Departments = new SelectList(db.TIPO_VISITA, "ID_TIPOVISITA", "S_NOMBRE");
            return View();

        }
        public ActionResult validarUser()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

            }
            else
            {
                // session.autoLogin();

            }

            return Content("1");
        }

        public ActionResult rvisita(int id, string contenido, string mail, int idRadicado)
        {
            //string emailFrom = "amonsalve@hyg.com.co";
            //string emailSMTPServer = "mail.hyg.com.co";
            //string emailSMTPUser = "amonsalve@hyg.com.co";
            //string emailSMTPPwd = "amonsalve@!123";
            string path = ConfigurationManager.AppSettings["url_radicador"];
            String urlRad = path + idRadicado + "&formatoRetorno=png";
            string asunto = "Constancia de visita";

            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            string emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            string emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];

            VISITA visita = db.VISITA.Where(v => v.ID_VISITA == id).FirstOrDefault();

            if (visita != null)
            {
                visita.D_FECHA = DateTime.Now;
                db.Entry(visita).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }


            string rutaCertificado = ConfigurationManager.AppSettings["rutacerticadoV"] + id;
            /*if (System.IO.File.Exists(rutaCertificado))
            {
                System.IO.File.Delete(rutaCertificado);
                System.IO.FileStream f = System.IO.File.Create(rutaCertificado);
                f.Close();
            }*/
            if (!Directory.Exists(rutaCertificado))
            {

                Directory.CreateDirectory(rutaCertificado);
            }

            rutaCertificado += "\\certificado" + id + ".pdf";
            var stream = new MemoryStream();

            var report = new SIM.Areas.ControlVigilancia.reporte.visita.contanciaVisita();
            DevExpress.XtraReports.Parameters.Parameter param = report.Parameters["idVisita"];
            DevExpress.XtraReports.Parameters.Parameter url = report.Parameters["prm_radicador"];
            // url.Value = urlRad;
            param.Value = id;

            report.ExportToPdf(rutaCertificado);
            report.ExportToPdf(stream);
            report.Dispose();

            /*try
            {
                Utilidades.Email.EnviarEmail(emailFrom, mail, asunto, contenido, emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, rutaCertificado);

            }
            catch
            {
            }*/

            return File(stream.GetBuffer(), "application/pdf", "certificadoVisita.pdf");
        }
        public ActionResult DocumentViewerPartial()
        {
            ViewData["Report"] = new SIM.Areas.ControlVigilancia.reporte.visita.contanciaVisita();
            return PartialView("DocumentViewerPartial");
        }

        public ActionResult ExportDocumentViewer()
        {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new SIM.Areas.ControlVigilancia.reporte.visita.contanciaVisita());
        }

        public ActionResult Visitas()
        {
            ViewBag.TipoVisita = new SelectList(db.TIPO_VISITA, "ID_TIPOVISITA", "S_NOMBRE");
            return View();
        }
        public ActionResult RealizarVisita()
        {
            Int32 IdVisita = Convert.ToInt32(Request.Params["id_visita"]);
            Int32 IdEstdo = Convert.ToInt32(Request.Params["p_ESTADOVISITA"]);

            ViewBag.IdVisita = IdVisita;
            ViewBag.txtEstadoVisita = IdEstdo;


            return View();
        }


        public ActionResult cargarImagenForm()
        {
            return View();
        }
        public ActionResult NuevaVisita()
        {
            return View();
        }
        public ActionResult reporteVisitas()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }
        public ActionResult arbol()
        {
            return View();
        }

        [HttpGet, ActionName("GenerarDocumentoRadicadoPreview")]
        public ActionResult GetGenerarDocumentoRadicadoPreview(int idVisita)
        {
            int codFuncionario1;

            var visitaEstado = db.VISITAESTADO.Where(visitaFunc => visitaFunc.ID_VISITA == idVisita && visitaFunc.ID_ESTADOVISITA == 2).FirstOrDefault();
            codFuncionario1 = (visitaEstado == null ? -1 : (int)visitaEstado.ID_TERCERO);
            int codFuncionario2 = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value)));

            var documento = Utilidades.Data.GenerarDocumentoRadicado(idVisita, 0, false, codFuncionario1, codFuncionario2);

            return File(documento.documentoBinario, "application/pdf");
        }

        public ActionResult previePDF(int id, int tipo, int idRadicado)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();
            var stream = new MemoryStream();
            String nombre = "";
            string path = ConfigurationManager.AppSettings["url_radicador"];
            String urlRad = path + idRadicado + "&formatoRetorno=png";

            switch (tipo)
            {
                case 15://general
                    var report1 = new SIM.Areas.ControlVigilancia.reporte.aguas.visitas();
                    DevExpress.XtraReports.Parameters.Parameter param1 = report1.Parameters["prm_id_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url = report1.Parameters["prm_radicador"];
                    url.Value = urlRad;
                    param1.Value = id;
                    report1.ExportToPdf(stream);
                    report1.Dispose();
                    nombre = "visitas.pdf";
                    break;
                case 20://Arbol Urbano
                    /*var report20 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                    DevExpress.XtraReports.Parameters.Parameter param20 = report20.Parameters["prm_idvisita"];
                    DevExpress.XtraReports.Parameters.Parameter url20 = report20.Parameters["prm_radicador"];
                    url20.Value = urlRad;
                    param20.Value = id;
                    report20.ExportToPdf(stream);
                    report20.Dispose();
                    nombre = "ArbolUrbano.pdf";*/
                    break;
                case 13://vertimiento
                    var report2 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.reportVertimiento();
                    DevExpress.XtraReports.Parameters.Parameter param2 = report2.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url2 = report2.Parameters["prm_radicador"];
                    url2.Value = urlRad;
                    param2.Value = id;
                    report2.ExportToPdf(stream);
                    report2.Dispose();
                    nombre = "vertimiento.pdf";
                    break;
                case 5://subterranea
                    var report3 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuterranea();
                    DevExpress.XtraReports.Parameters.Parameter param3 = report3.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url3 = report3.Parameters["prm_radicador"];
                    url3.Value = urlRad;
                    param3.Value = id;
                    report3.ExportToPdf(stream);
                    report3.Dispose();
                    nombre = "subterranea.pdf";
                    break;
                case 4://aguas Superficiales
                    var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial();
                    DevExpress.XtraReports.Parameters.Parameter param4 = report4.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url4 = report4.Parameters["prm_radicador"];
                    url4.Value = urlRad;
                    param4.Value = id;
                    report4.ExportToPdf(stream);
                    report4.Dispose();
                    nombre = "Superficiales.pdf";
                    break;
                case 12://ocupacion de cause
                    var report5 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NocupacionCause();
                    DevExpress.XtraReports.Parameters.Parameter param5 = report5.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url5 = report5.Parameters["prm_radicador"];
                    url5.Value = urlRad;
                    param5.Value = id;
                    report5.ExportToPdf(stream);
                    report5.Dispose();
                    nombre = "ocupaciondecause.pdf";
                    break;
                case 9://fuente fijas
                    {
                        var report6 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NfuentesFijas();
                        DevExpress.XtraReports.Parameters.Parameter param6 = report6.Parameters["prm_idVisita"];
                        DevExpress.XtraReports.Parameters.Parameter url6 = report6.Parameters["prm_radicador"];
                        url6.Value = urlRad;
                        param6.Value = id;

                        var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report6.DataSource;
                        var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                        //query.Sql = "SELECT * FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + id.ToString() + " AND id_formulario=9";
                        query.Sql = "SELECT CAST(ID_FUENTE_FIJA AS NUMBER(10,0)) ID_FUENTE_FIJA, NOMBRE, ID_FUENTE_FIJA_ESTADO, ID_VISITA, ID_INSTALACION, ID_TERCERO, ID_FORMULARIO, X, Y, URLMAPA FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + id.ToString() + " AND id_formulario=9 ORDER BY ID_FUENTE_FIJA";
                        dataSource.RebuildResultSchema();

                        report6.ExportToPdf(stream);
                        report6.Dispose();
                        nombre = "fuentesFijas.pdf";
                    }
                    break;
                case 10://CDA
                    var report7 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NCDA();
                    DevExpress.XtraReports.Parameters.Parameter param7 = report7.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url7 = report7.Parameters["prm_radicador"];
                    url7.Value = urlRad;
                    param7.Value = id;
                    report7.ExportToPdf(stream);
                    report7.Dispose();
                    nombre = "cda.pdf";
                    break;
                case 11://quejas
                    var report11 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.Nquejas();
                    DevExpress.XtraReports.Parameters.Parameter param11 = report11.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url11 = report11.Parameters["prm_radicador"];
                    url11.Value = urlRad;
                    param11.Value = id;
                    report11.ExportToPdf(stream);
                    report11.Dispose();
                    nombre = "quejas.pdf";
                    break;
                case 8://arbol urbano
                    var report8 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                    DevExpress.XtraReports.Parameters.Parameter param8 = report8.Parameters["prm_idvisita"];
                    DevExpress.XtraReports.Parameters.Parameter url8 = report8.Parameters["prm_radicador"];
                    url8.Value = urlRad;
                    param8.Value = id;
                    report8.ExportToPdf(stream);
                    report8.Dispose();
                    nombre = "arbolUrbano.pdf";
                    break;
                case 6://residuos peligrosos
                    {
                        var report9 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.ResiduoPeligroso();
                        DevExpress.XtraReports.Parameters.Parameter param9 = report9.Parameters["prm_idVisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url9 = report9.Parameters["prm_radicador"];
                        //url9.Value = urlRad;
                        param9.Value = id;

                        /*var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report9.DataSource;
                        var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                        //query.Sql = "SELECT * FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + id.ToString();
                        query.Sql = "SELECT NOMBRE, CAST(ID_RESIDUO AS NUMBER(10,0)) AS ID_RESIDUO, ID_VISITA, ID_RESIDUO_ESTADO FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + id.ToString();
                        dataSource.RebuildResultSchema();*/

                        report9.ExportToPdf(stream);
                        report9.Dispose();
                        nombre = "residuosPeligrosos.pdf";
                    }
                    break;
                case 7://proyectos constructivos
                    var report10 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.nproyectosContructivos();
                    DevExpress.XtraReports.Parameters.Parameter param10 = report10.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url10 = report10.Parameters["prm_radicador"];
                    url10.Value = urlRad;
                    param10.Value = id;
                    report10.ExportToPdf(stream);
                    report10.Dispose();
                    nombre = "proyectosContructivos.pdf";
                    break;
                case 1://INDUSTRIA FORESTAL
                    var report12 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NindustriaForestal();
                    DevExpress.XtraReports.Parameters.Parameter param12 = report12.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url12 = report12.Parameters["prm_radicador"];
                    url12.Value = urlRad;
                    param12.Value = id;
                    report12.ExportToPdf(stream);
                    report12.Dispose();
                    nombre = "industriaForestal.pdf";
                    break;

            }
            return File(stream.GetBuffer(), "application/pdf", nombre);
        }

        public ActionResult previePDFTipos(int id, string tipos)
        {
            int tipo;
            PdfDocument inputDocument;
            AppSettingsReader webConfigReader = new AppSettingsReader();
            var stream = new MemoryStream();
            String nombre = "";
            //string path = ConfigurationManager.AppSettings["url_radicador"];
            //String urlRad = path + idRadicado + "&formatoRetorno=png";

            PdfDocument outputDocument = new PdfDocument();

            foreach (string tipoSel in tipos.Split(','))
            {

                stream = new MemoryStream();

                tipo = Convert.ToInt32(tipoSel);

                switch (tipo)
                {
                    case 15://general
                        var report1 = new SIM.Areas.ControlVigilancia.reporte.aguas.visitas();
                        DevExpress.XtraReports.Parameters.Parameter param1 = report1.Parameters["prm_id_visita"];
                        //DevExpress.XtraReports.Parameters.Parameter url = report1.Parameters["prm_radicador"];
                        //url.Value = urlRad;
                        param1.Value = id;
                        report1.ExportToPdf(stream);
                        report1.Dispose();
                        nombre = "visitas.pdf";
                        break;
                    case 20://Arbol Urbano
                        /*var report20 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                        DevExpress.XtraReports.Parameters.Parameter param20 = report20.Parameters["prm_idvisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url20 = report20.Parameters["prm_radicador"];
                        //url20.Value = urlRad;
                        param20.Value = id;
                        report20.ExportToPdf(stream);
                        report20.Dispose();
                        nombre = "ArbolUrbano.pdf";*/
                        break;
                    case 13://vertimiento
                        var report2 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.reportVertimiento();
                        DevExpress.XtraReports.Parameters.Parameter param2 = report2.Parameters["prm_idVisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url2 = report2.Parameters["prm_radicador"];
                        //url2.Value = urlRad;
                        param2.Value = id;
                        report2.ExportToPdf(stream);
                        report2.Dispose();
                        nombre = "vertimiento.pdf";
                        break;
                    case 5://subterranea
                        var report3 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuterranea();
                        DevExpress.XtraReports.Parameters.Parameter param3 = report3.Parameters["prm_visita"];
                        //DevExpress.XtraReports.Parameters.Parameter url3 = report3.Parameters["prm_radicador"];
                        //url3.Value = urlRad;
                        param3.Value = id;
                        report3.ExportToPdf(stream);
                        report3.Dispose();
                        nombre = "subterranea.pdf";
                        break;
                    case 4://aguas Superficiales
                        var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial();
                        DevExpress.XtraReports.Parameters.Parameter param4 = report4.Parameters["prm_visita"];
                        //DevExpress.XtraReports.Parameters.Parameter url4 = report4.Parameters["prm_radicador"];
                        //url4.Value = urlRad;
                        param4.Value = id;
                        report4.ExportToPdf(stream);
                        report4.Dispose();
                        nombre = "Superficiales.pdf";
                        break;
                    case 12://ocupacion de cause
                        var report5 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NocupacionCause();
                        DevExpress.XtraReports.Parameters.Parameter param5 = report5.Parameters["prm_idVisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url5 = report5.Parameters["prm_radicador"];
                        //url5.Value = urlRad;
                        param5.Value = id;
                        report5.ExportToPdf(stream);
                        report5.Dispose();
                        nombre = "ocupaciondecause.pdf";
                        break;
                    case 9://fuente fijas
                        {
                            var report6 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NfuentesFijas();
                            DevExpress.XtraReports.Parameters.Parameter param6 = report6.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url6 = report6.Parameters["prm_radicador"];
                            //url6.Value = urlRad;
                            param6.Value = id;

                            var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report6.DataSource;
                            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                            // Lo siguiente corrige el error de Valor Fuera del Intervalo
                            //query.Sql = "SELECT * FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + id.ToString() + " AND id_formulario=9";
                            query.Sql = "SELECT CAST(ID_FUENTE_FIJA AS NUMBER(10,0)) ID_FUENTE_FIJA, NOMBRE, ID_FUENTE_FIJA_ESTADO, ID_VISITA, ID_INSTALACION, ID_TERCERO, ID_FORMULARIO, X, Y, URLMAPA FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + id.ToString() + " AND id_formulario=9 ORDER BY ID_FUENTE_FIJA";
                            dataSource.RebuildResultSchema();

                            report6.ExportToPdf(stream);
                            report6.Dispose();
                            nombre = "fuentesFijas.pdf";
                        }
                        break;
                    case 10://CDA
                        var report7 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NCDA();
                        DevExpress.XtraReports.Parameters.Parameter param7 = report7.Parameters["prm_visita"];
                        //DevExpress.XtraReports.Parameters.Parameter url7 = report7.Parameters["prm_radicador"];
                        //url7.Value = urlRad;
                        param7.Value = id;
                        report7.ExportToPdf(stream);
                        report7.Dispose();
                        nombre = "cda.pdf";
                        break;
                    case 11://quejas
                        var report11 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.Nquejas();
                        DevExpress.XtraReports.Parameters.Parameter param11 = report11.Parameters["prm_visita"];
                        //DevExpress.XtraReports.Parameters.Parameter url11 = report11.Parameters["prm_radicador"];
                        //url11.Value = urlRad;
                        param11.Value = id;
                        report11.ExportToPdf(stream);
                        report11.Dispose();
                        nombre = "quejas.pdf";
                        break;
                    case 8://arbol urbano
                        var report8 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                        DevExpress.XtraReports.Parameters.Parameter param8 = report8.Parameters["prm_idvisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url8 = report8.Parameters["prm_radicador"];
                        //url8.Value = urlRad;
                        param8.Value = id;
                        report8.ExportToPdf(stream);
                        report8.Dispose();
                        nombre = "arbolUrbano.pdf";
                        break;
                    case 6://residuos peligrosos
                        {
                            var report9 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.ResiduoPeligroso();
                            DevExpress.XtraReports.Parameters.Parameter param9 = report9.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url9 = report9.Parameters["prm_radicador"];
                            //url9.Value = urlRad;
                            param9.Value = id;

                            /*var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report9.DataSource;
                            var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                            query.Sql = "SELECT NOMBRE, CAST(ID_RESIDUO AS NUMBER(10,0)) AS ID_RESIDUO, ID_VISITA, ID_RESIDUO_ESTADO FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + id.ToString();
                            //query.Sql = "select 'Residuo(PERFOREX )' AS NOMBRE, 13933 AS ID_RESIDUO, 12872 AS ID_VISITA, 14329 AS ID_RESIDUO_ESTADO FROM dual";

                            //byte[] byt = System.Text.Encoding.UTF8.GetBytes("<DataSet Name=\"sqlDataSource1\"><View Name=\"CustomSqlQuery\"><Field Name=\"NOMBRE\" Type=\"String\" /><Field Name=\"ID_RESIDUO\" Type=\"Int64\" /><Field Name=\"ID_VISITA\" Type=\"Int64\" /><Field Name=\"ID_RESIDUO_ESTADO\" Type=\"Int64\" /></View></DataSet>");
                            //var customSchema = Convert.ToBase64String(byt);
                            //dataSource.ResultSchemaSerializable = customSchema;

                            dataSource.RebuildResultSchema();*/

                            report9.ExportToPdf(stream);
                            report9.Dispose();
                            nombre = "residuosPeligrosos.pdf";
                        }
                        break;
                    case 7://proyectos constructivos
                        var report10 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.nproyectosContructivos();
                        DevExpress.XtraReports.Parameters.Parameter param10 = report10.Parameters["prm_idVisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url10 = report10.Parameters["prm_radicador"];
                        //url10.Value = urlRad;
                        param10.Value = id;
                        report10.ExportToPdf(stream);
                        report10.Dispose();
                        nombre = "proyectosContructivos.pdf";
                        break;
                    case 1://proyectos constructivos
                        var report12 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NindustriaForestal();
                        DevExpress.XtraReports.Parameters.Parameter param12 = report12.Parameters["prm_idVisita"];
                        //DevExpress.XtraReports.Parameters.Parameter url12 = report12.Parameters["prm_radicador"];
                        //url12.Value = urlRad;
                        param12.Value = id;
                        report12.ExportToPdf(stream);
                        report12.Dispose();
                        nombre = "industriaForestal.pdf";
                        break;
                }

                inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfPage page = inputDocument.Pages[idx];
                    outputDocument.AddPage(page);
                }

                stream.Close();
            }

            MemoryStream ms = new MemoryStream();
            outputDocument.Save(ms);
            return File(ms.GetBuffer(), "application/pdf", "reporte.pdf");
        }

        public ActionResult previeRTF(int id, int tipo, int idRadicado)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();
            string path = ConfigurationManager.AppSettings["url_radicador"];
            String nombre = "";
            var stream = new MemoryStream();
            String urlRad = path + idRadicado + "&formatoRetorno=png";

            switch (tipo)
            {
                case 15://general
                    var report1 = new SIM.Areas.ControlVigilancia.reporte.aguas.visitas();
                    DevExpress.XtraReports.Parameters.Parameter param1 = report1.Parameters["prm_id_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url = report1.Parameters["prm_radicador"];
                    url.Value = urlRad;
                    param1.Value = id;
                    report1.ExportToRtf(stream);
                    report1.Dispose();
                    nombre = "visitas.rtf";
                    break;
                case 20://Arbol Urbano
                    /*var report20 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                    DevExpress.XtraReports.Parameters.Parameter param20 = report20.Parameters["prm_idvisita"];
                    DevExpress.XtraReports.Parameters.Parameter url20 = report20.Parameters["prm_radicador"];
                    url20.Value = urlRad;
                    param20.Value = id;
                    report20.ExportToRtf(stream);
                    report20.Dispose();
                    nombre = "ArbolUrbano.rtf";*/
                    break;
                case 13://vertimiento
                    var report2 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.reportVertimiento();
                    DevExpress.XtraReports.Parameters.Parameter param2 = report2.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url2 = report2.Parameters["prm_radicador"];
                    url2.Value = urlRad;
                    param2.Value = id;
                    report2.ExportToRtf(stream);
                    report2.Dispose();
                    nombre = "vertimiento.rtf";
                    break;
                case 5://subterranea
                    var report3 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuterranea();
                    DevExpress.XtraReports.Parameters.Parameter param3 = report3.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url3 = report3.Parameters["prm_radicador"];
                    url3.Value = urlRad;
                    param3.Value = id;
                    report3.ExportToRtf(stream);
                    report3.Dispose();
                    nombre = "subterranea.rtf";
                    break;
                case 4://aguas Superficiales
                    var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial();
                    DevExpress.XtraReports.Parameters.Parameter param4 = report4.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url4 = report4.Parameters["prm_radicador"];
                    url4.Value = urlRad;
                    param4.Value = id;
                    report4.ExportToRtf(stream);
                    report4.Dispose();
                    nombre = "Superficiales.rtf";
                    break;
                case 12://ocupacion de cause
                    var report5 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NocupacionCause();
                    DevExpress.XtraReports.Parameters.Parameter param5 = report5.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url5 = report5.Parameters["prm_radicador"];
                    url5.Value = urlRad;
                    param5.Value = id;
                    report5.ExportToRtf(stream);
                    report5.Dispose();
                    nombre = "ocupaciondecause.rtf";
                    break;
                case 9://fuente fijas
                    var report6 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NfuentesFijas();
                    DevExpress.XtraReports.Parameters.Parameter param6 = report6.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url6 = report6.Parameters["prm_radicador"];
                    url6.Value = urlRad;
                    param6.Value = id;
                    report6.ExportToRtf(stream);
                    report6.Dispose();
                    nombre = "fuentesFija.rtf";
                    break;
                case 10://CDA
                    var report7 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NCDA();
                    DevExpress.XtraReports.Parameters.Parameter param7 = report7.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url7 = report7.Parameters["prm_radicador"];
                    url7.Value = urlRad;
                    param7.Value = id;
                    report7.ExportToRtf(stream);
                    report7.Dispose();
                    nombre = "cda.rtf";
                    break;
                case 11://quejas
                    var report11 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.Nquejas();
                    DevExpress.XtraReports.Parameters.Parameter param11 = report11.Parameters["prm_visita"];
                    DevExpress.XtraReports.Parameters.Parameter url11 = report11.Parameters["prm_radicador"];
                    url11.Value = urlRad;
                    param11.Value = id;
                    report11.ExportToRtf(stream);
                    report11.Dispose();
                    nombre = "quejas.rtf";
                    break;
                case 8://arbol urbano
                    var report8 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                    DevExpress.XtraReports.Parameters.Parameter param8 = report8.Parameters["prm_idvisita"];
                    DevExpress.XtraReports.Parameters.Parameter url8 = report8.Parameters["prm_radicador"];
                    url8.Value = urlRad;
                    param8.Value = id;
                    report8.ExportToRtf(stream);
                    report8.Dispose();
                    nombre = "arbolUrbano.rtf";
                    break;
                case 6://residuos peligrosos
                    var report9 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.ResiduoPeligroso();
                    DevExpress.XtraReports.Parameters.Parameter param9 = report9.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url9 = report9.Parameters["prm_radicador"];
                    url9.Value = urlRad;
                    param9.Value = id;
                    report9.ExportToRtf(stream);
                    report9.Dispose();
                    nombre = "residuosPeligrosos.rtf";
                    break;
                case 7://proyectos constructivos
                    var report10 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.nproyectosContructivos();
                    DevExpress.XtraReports.Parameters.Parameter param10 = report10.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url10 = report10.Parameters["prm_radicador"];
                    url10.Value = urlRad;
                    param10.Value = id;
                    report10.ExportToRtf(stream);
                    report10.Dispose();
                    nombre = "proyectosContructivos.rtf";
                    break;
                case 1://proyectos constructivos
                    var report12 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NindustriaForestal();
                    DevExpress.XtraReports.Parameters.Parameter param12 = report12.Parameters["prm_idVisita"];
                    DevExpress.XtraReports.Parameters.Parameter url12 = report12.Parameters["prm_radicador"];
                    url12.Value = urlRad;
                    param12.Value = id;
                    report12.ExportToRtf(stream);
                    report12.Dispose();
                    nombre = "industriaForestal.rtf";
                    break;
            }
            return File(stream.GetBuffer(), "application/rtf", nombre);
        }

        [HttpGet, ActionName("GenerarPDFActuacion")]
        public ActionResult GenerarPDFActuacion(int idActuacion)
        {
            bool estadosVisitaOK;
            int numPaginas = 0;
            string tipos = "15,20";
            int tipo;
            string nombre = "";
            PdfSharp.Pdf.PdfDocument inputDocument;
            MemoryStream stream = null;

            VISITA visita = db.VISITA.Where(v => v.ID_VISITA == idActuacion).FirstOrDefault();

            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

            if (visita != null)
            {
                Decimal idInstalacion = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_INSTALACION);
                Decimal idTercero = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_TERCERO);
                /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                db.SP_GET_FORMULARIOS(idInstalacion, idTercero, Convert.ToDecimal(idActuacion), jSONOUT);

                dynamic data = JsonConvert.DeserializeObject(jSONOUT.Value.ToString());*/

                var datosFormulario = (new FormularioDatos()).ObtenerJsonFormularios(Convert.ToInt32(idInstalacion), Convert.ToInt32(idTercero), idActuacion);
                dynamic data = JsonConvert.DeserializeObject(datosFormulario.json);

                foreach (dynamic nodos in data)
                {
                    foreach (dynamic formulario in nodos.FORMULARIOS)
                    {
                        if (formulario.ITEMS != null && formulario.ITEMS.Count > 0)
                        {
                            estadosVisitaOK = false;

                            foreach (dynamic item in formulario.ITEMS)
                            {
                                if (item.ESTADO != 0)
                                {
                                    estadosVisitaOK = true;
                                    break;
                                }
                            }

                            //if (tipos == "")
                            //    tipos = formulario.ID.Value.ToString();
                            //else
                            if (estadosVisitaOK)
                                tipos += "," + formulario.ID.Value.ToString();
                        }
                    }
                }
            }

            foreach (string tipoSel in tipos.Split(','))
            {
                tipo = Convert.ToInt32(tipoSel);

                if (tipo != 8 && tipo != 20) // Excluimos Arbol Urbano mientras tanto.
                {
                    stream = new MemoryStream();

                    switch (tipo)
                    {
                        case 15://general
                            var report1 = new SIM.Areas.ControlVigilancia.reporte.aguas.visitas();
                            DevExpress.XtraReports.Parameters.Parameter param1 = report1.Parameters["prm_id_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url = report1.Parameters["prm_radicador"];
                            //url.Value = urlRad;
                            param1.Value = idActuacion;
                            report1.ExportToPdf(stream);
                            report1.Dispose();
                            nombre = "visitas.pdf";
                            break;
                        case 20://Arbol Urbano
                            /*var report20 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                            DevExpress.XtraReports.Parameters.Parameter param20 = report20.Parameters["prm_idvisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url20 = report20.Parameters["prm_radicador"];
                            //url20.Value = urlRad;
                            param20.Value = idActuacion;
                            report20.ExportToPdf(stream);
                            report20.Dispose();
                            nombre = "ArbolUrbano.pdf";*/
                            break;
                        case 13://vertimiento
                            var report2 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.reportVertimiento();
                            DevExpress.XtraReports.Parameters.Parameter param2 = report2.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url2 = report2.Parameters["prm_radicador"];
                            //url2.Value = urlRad;
                            param2.Value = idActuacion;
                            report2.ExportToPdf(stream);
                            report2.Dispose();
                            nombre = "vertimiento.pdf";
                            break;
                        case 5://subterranea
                            var report3 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuterranea();
                            DevExpress.XtraReports.Parameters.Parameter param3 = report3.Parameters["prm_visita"];
                            param3.Value = idActuacion;

                            report3.ExportToPdf(stream);
                            report3.Dispose();
                            nombre = "subterranea.pdf";
                            break;
                        case 4://aguas Superficiales
                            var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial();
                            //var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial(idActuacion);
                            DevExpress.XtraReports.Parameters.Parameter param4 = report4.Parameters["prm_visita"];
                            param4.Value = idActuacion;
                            report4.ExportToPdf(stream);
                            report4.Dispose();
                            nombre = "Superficiales.pdf";
                            break;
                        case 12://ocupacion de cause
                            var report5 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NocupacionCause();
                            DevExpress.XtraReports.Parameters.Parameter param5 = report5.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url5 = report5.Parameters["prm_radicador"];
                            //url5.Value = urlRad;
                            param5.Value = idActuacion;
                            report5.ExportToPdf(stream);
                            report5.Dispose();
                            nombre = "ocupaciondecause.pdf";
                            break;
                        case 9://fuente fijas
                            {
                                var report6 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NfuentesFijas();
                                DevExpress.XtraReports.Parameters.Parameter param6 = report6.Parameters["prm_idVisita"];
                                //DevExpress.XtraReports.Parameters.Parameter url6 = report6.Parameters["prm_radicador"];
                                //url6.Value = urlRad;
                                param6.Value = idActuacion;

                                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report6.DataSource;
                                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                                //query.Sql = "SELECT * FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + idActuacion.ToString() + " AND id_formulario=9";
                                query.Sql = "SELECT CAST(ID_FUENTE_FIJA AS NUMBER(10,0)) ID_FUENTE_FIJA, NOMBRE, ID_FUENTE_FIJA_ESTADO, ID_VISITA, ID_INSTALACION, ID_TERCERO, ID_FORMULARIO, X, Y, URLMAPA FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + idActuacion.ToString() + " AND id_formulario = 9 ORDER BY ID_FUENTE_FIJA";
                                dataSource.RebuildResultSchema();

                                report6.ExportToPdf(stream);
                                report6.Dispose();
                                nombre = "fuentesFijas.pdf";
                            }
                            break;
                        case 10://CDA
                            var report7 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NCDA();
                            DevExpress.XtraReports.Parameters.Parameter param7 = report7.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url7 = report7.Parameters["prm_radicador"];
                            //url7.Value = urlRad;
                            param7.Value = idActuacion;
                            report7.ExportToPdf(stream);
                            report7.Dispose();
                            nombre = "cda.pdf";
                            break;
                        case 11://quejas
                            var report11 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.Nquejas();
                            DevExpress.XtraReports.Parameters.Parameter param11 = report11.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url11 = report11.Parameters["prm_radicador"];
                            //url11.Value = urlRad;
                            param11.Value = idActuacion;
                            report11.ExportToPdf(stream);
                            report11.Dispose();
                            nombre = "quejas.pdf";
                            break;
                        case 8://arbol urbano
                            var report8 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                            DevExpress.XtraReports.Parameters.Parameter param8 = report8.Parameters["prm_idvisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url8 = report8.Parameters["prm_radicador"];
                            //url8.Value = urlRad;
                            param8.Value = idActuacion;
                            report8.ExportToPdf(stream);
                            report8.Dispose();
                            nombre = "arbolUrbano.pdf";
                            break;
                        case 6://residuos peligrosos
                            {
                                var report9 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.ResiduoPeligroso();
                                DevExpress.XtraReports.Parameters.Parameter param9 = report9.Parameters["prm_idVisita"];
                                //DevExpress.XtraReports.Parameters.Parameter url9 = report9.Parameters["prm_radicador"];
                                //url9.Value = urlRad;
                                param9.Value = idActuacion;

                                /*var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report9.DataSource;
                                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                                //query.Sql = "SELECT * FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + idActuacion.ToString();
                                query.Sql = "SELECT NOMBRE, CAST(ID_RESIDUO AS NUMBER(10,0)) AS ID_RESIDUO, ID_VISITA, ID_RESIDUO_ESTADO FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + idActuacion.ToString();
                                dataSource.RebuildResultSchema();*/

                                report9.ExportToPdf(stream);
                                report9.Dispose();
                                nombre = "residuosPeligrosos.pdf";
                            }
                            break;
                        case 7://proyectos constructivos
                            var report10 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.nproyectosContructivos();
                            DevExpress.XtraReports.Parameters.Parameter param10 = report10.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url10 = report10.Parameters["prm_radicador"];
                            //url10.Value = urlRad;
                            param10.Value = idActuacion;
                            report10.ExportToPdf(stream);
                            report10.Dispose();
                            nombre = "proyectosContructivos.pdf";
                            break;
                        case 1://proyectos constructivos
                            var report12 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NindustriaForestal();
                            DevExpress.XtraReports.Parameters.Parameter param12 = report12.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url12 = report12.Parameters["prm_radicador"];
                            //url12.Value = urlRad;
                            param12.Value = idActuacion;
                            report12.ExportToPdf(stream);
                            report12.Dispose();
                            nombre = "industriaForestal.pdf";
                            break;
                    }

                    inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];
                        outputDocument.AddPage(page);
                    }

                    if (stream != null)
                        stream.Close();
                }
            }

            MemoryStream ms = new MemoryStream();
            outputDocument.Save(ms);
            return File(ms.GetBuffer(), "application/pdf", "reporte.pdf");
        }

        public ActionResult reportAguas()
        {

            var report = new SIM.Areas.ControlVigilancia.reporte.aguas.encuesta();
            DevExpress.XtraReports.Parameters.Parameter param_id_encuesta = report.Parameters["prm_id_encuesta"];
            param_id_encuesta.Value = 125;
            DevExpress.XtraReports.Parameters.Parameter param_id_estado = report.Parameters["prm_id_estado"];
            param_id_estado.Value = 38;

            var stream = new MemoryStream();
            report.ExportToPdf(stream);
            report.Dispose();

            return File(stream.GetBuffer(), "application/pdf");

        }
        public ActionResult RealizarVisitaAtiende()
        {
            return View();
        }
        public ActionResult EnviarCertificado()
        {
            return View();
        }
        public ActionResult GridAtiendeTabla()
        {
            return View();
        }
        public ActionResult RegistrarVisitasAsignacionTramites()
        {
            return View();
        }
        public ActionResult Guardar()
        {

            String Asunto = Request.Params["p_Asunto"];
            Decimal Cx = Decimal.Parse(Request.Params["p_cx"].Replace(".", ","));
            Decimal Cy = Decimal.Parse(Request.Params["p_cy"].Replace(".", ","));
            String TipoUbicacion = Request.Params["p_TipoUbicacion"];
            String Tramites = Request.Params["p_IdsTramite"];
            if (Tramites == "")
                Tramites = "0";

            String Cometario = Request.Params["p_Cometario"];
            String IdsCopias = Request.Params["p_IdsCopias"];
            if (IdsCopias == "")
                Tramites = "0";

            Int32 IdTipoVisita = Int32.Parse(Request.Params["p_IdTipoVisita"]);
            Int32 Id_visita_actual = Int32.Parse(Request.Params["p_IdVisita"]);

            dbControl.SP_NEW_VISITA(Asunto, Cx, Cy, TipoUbicacion, Tramites, 1111115377, Cometario, IdsCopias, IdTipoVisita, Id_visita_actual);
            return Content("1");
        }

        public ActionResult GrdEncargados()
        {
            System.Collections.Generic.List<VW_FUNCIONARIO> t = db.VW_FUNCIONARIO.ToList();
            return PartialView("_GrdEncargados", t);
        }

        [ValidateInput(false)]
        public ActionResult GrdVisitas()
        {

            System.Collections.Generic.List<VW_VISITAS> t = db.VW_VISITAS.ToList();
            return PartialView("_GrdVisitas", t);
        }


        public ActionResult GrdTramiteVisita()
        {
            System.Collections.Generic.List<VW_TRAMITE_VISITA> t = db.VW_TRAMITE_VISITA.ToList();
            return PartialView("_GrdTramiteVisita", t);
        }

        public ActionResult GrdTramiteVisita_E()
        {
            int Codigo = -1;
            System.Collections.Generic.List<VW_TRAMITE_VISITA> s = new List<VW_TRAMITE_VISITA>();

            try
            {
                Codigo = int.Parse(Request["Codigo"].ToString());
            }
            catch { }

            if (Codigo == -1)
            {
                s = db.VW_TRAMITE_VISITA.ToList();
            }
            else
            {
                s = db.VW_TRAMITE_VISITA.Where(p => p.CODTRAMITE == Codigo && p.CODFUNCIONARIO == 1111115377).ToList();
            }

            return this.PartialView("_GrdTramiteVisita", s);


        }

        [ValidateInput(false)]
        public ActionResult GrdTramitesVisitas_E()
        {

            System.Collections.Generic.List<VW_TRAMITE_A_VISITAR> t = db.VW_TRAMITE_A_VISITAR.Where(p => p.CODFUNCIONARIO == 1111115377).ToList();
            return PartialView("_GrdTramitesVisitas_E", t);
        }

        [ValidateInput(false)]
        public ActionResult GrdTramitesVisitas()
        {
            System.Collections.Generic.List<VW_TRAMITE_A_VISITAR> t = db.VW_TRAMITE_A_VISITAR.Where(p => p.CODFUNCIONARIO == 1111115377).ToList();
            return PartialView("_GrdTramitesVisitas", t);
        }


        [ValidateInput(false)]
        public ActionResult GrdEncargados_E()
        {
            int Codigo = -1;
            System.Collections.Generic.List<VW_FUNCIONARIO> s = new List<VW_FUNCIONARIO>();

            try
            {
                Codigo = int.Parse(Request["CodFuncionario"].ToString());
            }
            catch { }

            if (Codigo == -1)
            {
                s = db.VW_FUNCIONARIO.ToList();
            }
            else
            {
                s = db.VW_FUNCIONARIO.Where(p => p.CODFUNCIONARIO != Codigo).ToList();
            }

            return this.PartialView("_GrdEncargados_E", s);
        }

        [ValidateInput(false)]
        public ActionResult GrdDetalleEncargados_E()
        {

            int Codigo = -1;
            System.Collections.Generic.List<VW_FUNCIONARIO> s = new List<VW_FUNCIONARIO>();

            try
            {
                Codigo = int.Parse(Request["CodFuncionario"].ToString());
            }
            catch { }

            if (Codigo == -1)
            {
                s = db.VW_FUNCIONARIO.ToList();
            }
            else
            {
                s = db.VW_FUNCIONARIO.Where(p => p.CODFUNCIONARIO == Codigo).ToList();
            }

            return this.PartialView("_GrdDetalleEncargados_E", s);
        }

        [ValidateInput(false)]
        public ActionResult GrdDetalleTramitesVisitas()
        {

            int Codigo = -1;
            System.Collections.Generic.List<VW_TRAMITE_A_VISITAR> s = new List<VW_TRAMITE_A_VISITAR>();

            try
            {
                Codigo = int.Parse(Request["Codigo"].ToString());
            }
            catch { }

            if (Codigo == -1)
            {
                s = null;
            }
            else
            {
                s = db.VW_TRAMITE_A_VISITAR.Where(p => p.CODTRAMITE == Codigo && p.CODFUNCIONARIO == 1111115377).ToList();

            }

            return this.PartialView("_GrdDetalleTramitesVisitas", s);
        }

        [ValidateInput(false)]
        public ActionResult GrdAcompananteXVisita()
        {

            System.Collections.Generic.List<VW_VISITAS> s = new List<VW_VISITAS>();
            s = db.VW_VISITAS.Where(p => p.COPIAS.Contains("141")).ToList();
            return PartialView("_GrdAcompananteXVisita", s);
        }


        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            var path = Server.MapPath("~/test.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
            return Content("1");

        }



        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
        public ActionResult Observacion()
        {
            return View();
        }

        public ActionResult cargarImagen()
        {
            return View();
        }
        public ActionResult EliminarFoto()
        {
            return View();
        }
        public ActionResult EliminarFotoForm()
        {
            return View();
        }

        public ActionResult ConsultarInformacionFormulariosANT()
        {

            Decimal idInstalacion = Decimal.Parse(Request.Params["ins"]);
            Decimal idTercero = Decimal.Parse(Request.Params["tercero"]);
            Decimal idVisista = Decimal.Parse(Request.Params["visita"]);
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_FORMULARIOS(idInstalacion, idTercero, idVisista, jSONOUT);
            return Json(jSONOUT.Value);


        }

        // &&&
        public ActionResult ConsultarInformacionFormularios(int ins, int tercero, int visita)
        {
            var datosFormulario = (new FormularioDatos()).ObtenerJsonFormularios(ins, tercero, visita);
            var resultado = datosFormulario.json;

            return Json(resultado);
        }

        [ValidateInput(false)]
        public ActionResult TreeListPartial()
        {
            var model = new object[0];
            return PartialView("_TreeListPartial", model);
        }

        public ActionResult DetalleVisitas()
        {

            return View();
        }
        public ActionResult ConsultarPalabra(int idF)
        {

            var foto = from f in db.FOTOGRAFIA
                       where f.ID_FOTOGRAFIA == idF
                       select new { f.PALABRA_CLAVE };
            return Json(foto);
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }
        public ActionResult DetalleTramitesVisitasInfTecn()
        {
            String sql = "select CODTRAMITE,FECHAINI,COMENTARIO,ESTADO,URL,ORDEN,S_FORMULARIO from control.VW_TRAMITE_INF_TEC";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult DetalleTramitesVisitasInfTecnAprobado()
        {
            String sql = "select CODTRAMITE,FECHAINI,COMENTARIO,ESTADO,URL,ORDEN,S_FORMULARIO,RUTA from control.VW_TRAMITE_INF_TEC_APROB";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult DetalleTramitesVisitas(int id_visita)
        {
            String sql = "SELECT  p.ID_TRAMITE, p.FECHAINI, p.DIRECCION, p.MUNICIPIO, p.ASUNTO  FROM VW_DETALLE_TRAMITE_V p  where  p.ID_VISITA ='" + id_visita + "' order by p.ID_TRAMITE desc";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarVisitaTramite()
        {
            var model = from p in db.VW_TRAMITE_A_VISITAR
                        let Vincular = false
                        orderby p.CODTRAMITE descending
                        select new { p.CODTRAMITE, p.FECHAINICIOTRAMITE, p.DIRECCION, p.MUNICIPIO, p.X, p.Y, p.ASUNTO, p.STR_CODTRAMITE, Vincular };
            return Json(model);
        }
        public ActionResult consultarTramitesVisita(int idVisita)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            String sql = "SELECT distinct tv.id_tramite,tv.asunto,tv.fechaini,to_char(tv.direccion) direccion,to_char(tv.municipio) municipio FROM VW_DETALLE_TRAMITE_V tv where tv.ID_VISITA ='" + idVisita + "' union SELECT distinct tv.codtramite,tv.asunto,tv.fechaini,to_char(tv.direccion) direccion,to_char(tv.municipio) municipio FROM VW_TRAMITE_A_VISITAR tv where tv.codtramite not in(SELECT distinct id_tramite FROM TRAMITE_VISITA) and tv.CODFUNCIONARIO='" + codFuncionario + "'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarResponsableVisita(String tramite)
        {
            String sql = "select f.codfuncionario CODFUNCIONARIO,f.nombres NOMBRECOMPLETO from VW_FUNCIONARIO f where f.CODFUNCIONARIO in(select distinct CODFUNCIONARIO from TRAMITES.tbtramitetarea where CODTRAMITE in(" + tramite + "))";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult enviarMail()
        {
            //hyg

            //string emailFrom="amonsalve@hyg.com.co";
            //string emailSMTPServer = "mail.hyg.com.co"; 
            //string emailSMTPUser="amonsalve@hyg.com.co";
            //string emailSMTPPwd="amonsalve@!123";

            //area

            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            string emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            string emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];
            string asunto;
            string contenido;
            asunto = "pruebas";
            contenido = "certificado de la visita";

            try
            {
                Utilidades.EmailMK.EnviarEmail(emailFrom, "", asunto, contenido, emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, "d:\\visitas.pdf");

            }
            catch
            {
            }
            return Content("1");
        }
        public ActionResult asociarTramiteVisita(String tramite, int idVisita)
        {

            dbControl.SP_ASOCIARTRAMITE_VISITA(tramite, idVisita);
            return Content("1");
        }
        public ActionResult consultarTramitesVisitaNuevo()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            //String sql = "SELECT distinct tv.codtramite CODTRAMITE,tv.fechaini FECHAINICIOTRAMITE,to_char(tv.direccion) DIRECCION,to_char(tv.municipio) MUNICIPIO,tv.X,tv.Y,tv.ASUNTO,tv.STR_CODTRAMITE FROM VW_TRAMITE_A_VISITAR tv where tv.codtramite not in(SELECT distinct id_tramite FROM TRAMITE_VISITA) and tv.CODFUNCIONARIO='" + codFuncionario + "'";
            String sql = "SELECT distinct tv.codtramite CODTRAMITE,tv.fechaini FECHAINICIOTRAMITE,to_char(tv.direccion) DIRECCION,to_char(tv.municipio) MUNICIPIO,tv.X,tv.Y,tv.ASUNTO,tv.STR_CODTRAMITE FROM VW_TRAMITE_A_VISITAR tv where tv.codtramite not in (SELECT id_tramite FROM TRAMITE_VISITA tv INNER JOIN VISITAESTADO ve ON tv.ID_VISITA = ve.ID_VISITA WHERE ve.D_FIN IS NULL AND ve.ID_ESTADOVISITA < 5) and tv.CODFUNCIONARIO='" + codFuncionario + "'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);

        }


        public ActionResult modificarVisita(string p_Asunto, string cx, string cy, string p_TipoUbicacion, string p_IdsTramite, string p_Cometario, string p_IdsCopias, int p_IdTipoVisita, int p_IdVisita)
        {
            decimal p_cx = Convert.ToDecimal(cx.Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            decimal p_cy = Convert.ToDecimal(cy.Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

            if (p_IdsTramite == "")
                p_IdsTramite = "0";

            if (p_IdsCopias == "")
                p_IdsCopias = "0";

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }

            try
            {
                dbControl.SP_NEW_VISITA(p_Asunto, p_cx, p_cy, p_TipoUbicacion, p_IdsTramite, codFuncionario, p_Cometario, p_IdsCopias, p_IdTipoVisita, p_IdVisita);
            }

            catch (Exception e)
            {
                return Content("0");
            }
            return Content("1");
        }

        public ActionResult desasociarTramite(string idTramites, string comentarios, string arrCopias, decimal tipo)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {
                dbControl.SP_DESASOCIARTRAMITE(idTramites, codFuncionario, comentarios, arrCopias, tipo);
            }

            catch (Exception e)
            {
                return Content("0");
            }
            return Content("1");
        }

        public ActionResult desasociarTramiteAprobacion(string idTramites, string arrCopias, decimal CodResponsable, decimal codTarea, decimal tareaSig, decimal idVisita, decimal funAnt)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {

                dbControl.SP_ADD_TRAMITE_INFORME(idTramites, codFuncionario, arrCopias, CodResponsable, codTarea, tareaSig);
                //db.SP_MODIFICAR_INFTECNICO(idVisita, funAnt, 1);
                dbControl.SP_MODIFICAR_INFTECNICO(idVisita, CodResponsable, 1);
            }

            catch (Exception e)
            {
                return Content("0");
            }
            return Content("1");
        }

        public ActionResult aprobarTramite(string idTramites, string arrCopias, decimal codTarea, decimal idVisita)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            try
            {

                dbControl.SP_ADD_TRAMITE_INFORME(idTramites, codFuncionario, arrCopias, codFuncionario, codTarea, 0);
                dbControl.SP_MODIFICAR_INFTECNICO(idVisita, codFuncionario, 3);
            }

            catch (Exception e)
            {
                return Content("0");
            }
            return Content("1");
        }
        public ActionResult consultarTareaAnt(string idTramite)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            String sql = "SELECT distinct codtarea,codtarea_anterior from tramites.tbtramitetarea where codtramite in(" + idTramite + ") and ESTADO=0 and orden=(SELECT max(orden) from tramites.tbtramitetarea where codtramite in(" + idTramite + ") and ESTADO=0 ) ";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);

        }
        public ActionResult consultarTramiteVisita(string idVisita)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            String sql = "SELECT distinct ID_TRAMITE from control.TRAMITE_VISITA where ID_VISITA=" + idVisita + "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);

        }
        public ActionResult validar(string usuario, string clav)
        {
            ac.validarLogueo(usuario, clav);
            return Content("1");
        }

        public string Prueba(int idVisita)
        {
            bool estadosVisitaOK;
            string tipos = "15,20";

            VISITA visita = db.VISITA.Where(v => v.ID_VISITA == idVisita).FirstOrDefault();

            if (visita != null)
            {
                Decimal idInstalacion = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_INSTALACION);
                Decimal idTercero = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_TERCERO);
                /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                db.SP_GET_FORMULARIOS(idInstalacion, idTercero, Convert.ToDecimal(idVisita), jSONOUT);*/
                //dynamic data = JsonConvert.DeserializeObject(jSONOUT.Value.ToString());

                var datosFormulario = (new FormularioDatos()).ObtenerJsonFormularios(Convert.ToInt32(idInstalacion), Convert.ToInt32(idTercero), idVisita);
                dynamic data = JsonConvert.DeserializeObject(datosFormulario.json);

                foreach (dynamic nodos in data)
                {
                    foreach (dynamic formulario in nodos.FORMULARIOS)
                    {
                        if (formulario.ITEMS != null && formulario.ITEMS.Count > 0)
                        {
                            estadosVisitaOK = false;

                            foreach (dynamic item in formulario.ITEMS)
                            {
                                if (item.ESTADO != 0)
                                {
                                    estadosVisitaOK = true;
                                    break;
                                }
                            }

                            //if (tipos == "")
                            //    tipos = formulario.ID.Value.ToString();
                            //else

                            if (estadosVisitaOK)
                                tipos += "," + formulario.ID.Value.ToString();
                        }
                    }
                }
            }

            return tipos;
        }
    }
}