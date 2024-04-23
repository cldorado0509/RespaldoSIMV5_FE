using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SIM.Areas.ControlVigilancia.Controllers;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;
using SIM.Utilidades;
using System;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Tala.Controllers
{

    public class TalaController : Controller
    {


        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesFloraOracle dbFlora = new EntitiesFloraOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        //SIM.Areas.Tala.Models.EntitiesTala fldb = new SIM.Areas.Tala.Models.EntitiesTala();
        // SIM.Areas.Tala.Models.other.Model1Container fldb = new SIM.Areas.Tala.Models.other.Model1Container();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        public ActionResult Tala()
        {
            return View();
        }
        public ActionResult imagenArbol()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            ViewBag.idArbol = id;
            return View();
        }
        public ActionResult TerceroIntervencion()
        {
            return View();
        }
        public ActionResult TerminoCondiciones()
        {

            return View();
        }
        public ActionResult agregarDireccion()
        {

            return View();

        }
        public ActionResult agregarDireccion1()
        {

            return View();
        }
        public ActionResult ArbolExistente()
        {


            return View();
        }

        public ActionResult Arbol()
        {


            return View();
        }

        public ActionResult cargaPopUp()
        {


            return View();
        }

        public ActionResult FotoIntervencion()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            ViewBag.id = id;
            int tipo = Convert.ToInt32(Request.Params["tipo"]);
            ViewBag.tipo = tipo;
            return View();
        }
        public ActionResult Justificacion()
        {


            return View();
        }
        public ActionResult fototerceroint()
        {
            return View();
        }
        public ActionResult cargarImagen()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            ViewBag.id = id;

            return View();
        }

        public ActionResult caracteristica()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            ViewBag.id = id;

            return View();
        }
        public ActionResult NuevoArbol()
        {


            return View();
        }

        private void DrawImage(XGraphics gfx, Stream imageEtiqueta, int x, int y, int width, int height)
        {
            XImage image = XImage.FromStream(imageEtiqueta);

            gfx.DrawImage(image, new System.Drawing.Point(x, y));
        }
        [HttpPost]
        public ActionResult reportTalaPodada(String json, String jsonSolicitante, String mensaje, int idRadicado, String direccionArbol, String municipioArbol)// String arrId, String idRadicadoCOD, String fechavisi)
        {


            String arrId = "0";
            String idRadicadoCOD = "100";
            String fechavisi = "";
            tramitePoda tramiteTala = guardarTramiteTala("tamite tala y poda");
            String strArbol = guardarArbolTala(json, Int32.Parse(tramiteTala.codTramite), Int32.Parse(tramiteTala.codFuncionario));

            solicitante solicit = new solicitante();
            solicit = JsonConvert.DeserializeObject<solicitante>(jsonSolicitante);
            String municipio = solicit.mumicipio;
            String tipoSolicitante = solicit.tipoSolicitante;
            String medioRespuesta = solicit.medioRespuesta;
            String mail = solicit.mail;
            String direcion = solicit.direcion;
            String nombreSolicitante = solicit.nombreSolicitante;
            String barrio = solicit.barrio;
            String telefono = solicit.telefono;
            String pais = solicit.pais;
            String tipoIdentificacion = solicit.tipoIdentificacion;
            String departamento = solicit.departamento;
            String identificacion = solicit.identificacion;
            String IDREPORT = solicit.IDREPORT;
            String idArbolDoc = solicit.idArbolDoc;

            //comunicacion oficial despachada
            var radicadodocumento = (from Rdocumt in db.RADICADO_DOCUMENTO
                                     where (Rdocumt.ID_RADICADODOC == idRadicado)
                                     select new { Rdocumt.D_RADICADO, Rdocumt.S_RADICADO }).FirstOrDefault();
            var streamCod = new MemoryStream();
            var reportcod = new SIM.Areas.Tala.reporte.cod();
            DevExpress.XtraReports.Parameters.Parameter nombreCod = reportcod.Parameters["prm_nombre"];
            nombreCod.Value = nombreSolicitante;
            DevExpress.XtraReports.Parameters.Parameter dirCod = reportcod.Parameters["prm_direcion"];
            dirCod.Value = direccionArbol;
            DevExpress.XtraReports.Parameters.Parameter telCod = reportcod.Parameters["prm_telefono"];
            telCod.Value = telefono;
            DevExpress.XtraReports.Parameters.Parameter correoCod = reportcod.Parameters["prm_mail"];
            correoCod.Value = mail;
            DevExpress.XtraReports.Parameters.Parameter depCod = reportcod.Parameters["prm_departamento"];
            depCod.Value = departamento;
            DevExpress.XtraReports.Parameters.Parameter ciudadCod = reportcod.Parameters["prm_ciudad"];
            ciudadCod.Value = municipioArbol;
            DevExpress.XtraReports.Parameters.Parameter tramiteCod = reportcod.Parameters["PRM_IDTRAMITE"];
            tramiteCod.Value = tramiteTala.codTramite;
            DevExpress.XtraReports.Parameters.Parameter radicadoCod = reportcod.Parameters["prm_radicado"];
            radicadoCod.Value = radicadodocumento.S_RADICADO;
            DevExpress.XtraReports.Parameters.Parameter fechaRadicadoCod = reportcod.Parameters["prm_fechaRadicado"];
            fechaRadicadoCod.Value = radicadodocumento.D_RADICADO.ToString();
            DevExpress.XtraReports.Parameters.Parameter semanavisit = reportcod.Parameters["prmfecha"];
            semanavisit.Value = fechavisi;




            reportcod.ExportToPdf(streamCod);


            PdfDocument outpuCOD = new PdfDocument();
            MemoryStream streamCOD = new MemoryStream();
            PdfDocument docCOD = new PdfDocument();
            docCOD = PdfReader.Open(streamCod, PdfDocumentOpenMode.Import);
            int numPaginasCOD = 0;
            int countPlantCOD = docCOD.PageCount;
            for (int idx = 0; idx < countPlantCOD; idx++)
            {
                PdfPage page = docCOD.Pages[idx];
                outpuCOD.AddPage(page);
                numPaginasCOD++;
                if (idx == 0) // Primera Página, se inserta el radicado
                {
                    PdfPage pageRadicado = outpuCOD.Pages[idx];
                    Radicado01Report etiqueta = new Radicado01Report();
                    MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(Int32.Parse(idRadicadoCOD), "png");

                    XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);//Security.ObtenerFirmaElectronicaFuncionario(codigoFuncionario)
                    DrawImage(gfx, imagenEtiqueta, 300, 130, 250, 90);

                }
                if (idx == countPlantCOD - 1) // ultima Página, se inserta firma  
                {
                    //  PdfPage pageRadicado = outpuCOD.Pages[idx];
                    //  Radicado01Report etiqueta = new Radicado01Report();

                    //  long codigoFuncionario = 71763413;
                    //System.Drawing.Image imageFirma = Utilidades.Security.ObtenerFirmaElectronicaFuncionario(codigoFuncionario);
                    // XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);
                    // DrawImage2(gfx, imageFirma, 300, 600, 250, 90);

                }
            }
            outpuCOD.Save(streamCOD);



            //comunicacion oficial Recividad
            if (municipio == "-1")
                municipio = "";
            doc objDoc = new doc();
            Decimal idr = Convert.ToDecimal(idArbolDoc);

            //var documento = (from documt in fldb.TEMP_DOC
            var documento = (from documt in db.TEMP_DOC
                             where arrId.Contains(documt.IDARBOL.ToString())


                             select new
                             {
                                 documt.DOC,
                                 documt.TIPO,
                                 documt.IDARBOL
                             }).ToList();

            var stream = new MemoryStream();
            var report = new SIM.Areas.Tala.reporte.talapoda();
            DevExpress.XtraReports.Parameters.Parameter nombre = report.Parameters["prm_nombre"];
            nombre.Value = nombreSolicitante;
            DevExpress.XtraReports.Parameters.Parameter tipoS = report.Parameters["prm_tipo_solicitante"];
            tipoS.Value = tipoSolicitante;
            DevExpress.XtraReports.Parameters.Parameter correo = report.Parameters["prm_mail"];
            correo.Value = mail;
            DevExpress.XtraReports.Parameters.Parameter barr = report.Parameters["prm_barrio"];
            barr.Value = barrio;
            DevExpress.XtraReports.Parameters.Parameter pai = report.Parameters["prm_pais"];
            pai.Value = pais;
            DevExpress.XtraReports.Parameters.Parameter dep = report.Parameters["prm_departamento"];
            dep.Value = departamento;
            DevExpress.XtraReports.Parameters.Parameter tel = report.Parameters["prm_telefono"];
            tel.Value = telefono;
            DevExpress.XtraReports.Parameters.Parameter tipoI = report.Parameters["prm_tipoIdentificacion"];
            tipoI.Value = tipoIdentificacion;
            DevExpress.XtraReports.Parameters.Parameter ident = report.Parameters["prm_identificacion"];
            ident.Value = identificacion;
            DevExpress.XtraReports.Parameters.Parameter dir = report.Parameters["prm_direcion"];
            dir.Value = direcion;
            DevExpress.XtraReports.Parameters.Parameter ident2 = report.Parameters["PRM_IDTRAMITE"];
            ident2.Value = Int32.Parse(tramiteTala.codTramite);

            report.ExportToPdf(stream);


            PdfDocument outputDocument = new PdfDocument();
            MemoryStream stream2 = new MemoryStream();
            PdfDocument docPlanti = new PdfDocument();
            docPlanti = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            int numPaginas = 0;
            int countPlant = docPlanti.PageCount;
            for (int idx = 0; idx < countPlant; idx++)
            {
                PdfPage page = docPlanti.Pages[idx];
                outputDocument.AddPage(page);
                numPaginas++;
                if (idx == 0) // Primera Página, se inserta el radicado
                {
                    PdfPage pageRadicado = outputDocument.Pages[idx];
                    Radicado01Report etiqueta = new Radicado01Report();
                    MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(idRadicado, "png");

                    XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);
                    DrawImage(gfx, imagenEtiqueta, 300, 80, 250, 61);
                }
            }
            foreach (var item in documento)
            {
                MemoryStream docum = new MemoryStream(item.DOC);
                PdfDocument docAjt = new PdfDocument();
                docAjt = PdfReader.Open(docum, PdfDocumentOpenMode.Import);
                int countDocAj = docAjt.PageCount;
                for (int idx = 0; idx < countDocAj; idx++)
                {
                    PdfPage page = docAjt.Pages[idx];
                    outputDocument.AddPage(page);
                    numPaginas++;
                }
            }


            String[] arrDOC = arrId.Split(',');
            for (var i = 0; i < arrDOC.Length; i++)
            {
                //fldb.SP_ELIMINAR_DOC_TALA(Int32.Parse(arrDOC[i]));
                dbFlora.SP_ELIMINAR_DOC_TALA(Int32.Parse(arrDOC[i]));
            }
            ObjectParameter rtaTerminos = new ObjectParameter("rTA", typeof(string));

            doc docrtp = new doc();
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            //registrar documento comunicacion oficial recividad
            docrtp = RegistrarDocumento(Int32.Parse(tramiteTala.codFuncionario), idRadicado, numPaginas, Int32.Parse(tramiteTala.codTramite), Int32.Parse(tramiteTala.codProceso), 10);
            //fldb.SP_SET_INDICE_TALA(Convert.ToDecimal(docrtp.IDDOC), medioRespuesta, mail, tipoIdentificacion, tipoSolicitante, nombreSolicitante, Int32.Parse(tramiteTala.codTramite), municipioArbol, direccionArbol,radicadodocumento.D_RADICADO.ToString(),radicadodocumento.S_RADICADO, rta);
            dbFlora.SP_SET_INDICE_TALA(Convert.ToDecimal(docrtp.IDDOC), medioRespuesta, mail, tipoIdentificacion, tipoSolicitante, nombreSolicitante, Int32.Parse(tramiteTala.codTramite), municipioArbol, direccionArbol, radicadodocumento.D_RADICADO.ToString(), radicadodocumento.S_RADICADO, rta);
            Cryptografia crypt = new Cryptografia();
            MemoryStream ms = new MemoryStream();

            String rut = docrtp.file.ToString().Substring(0, docrtp.file.ToString().Length - 1);
            System.IO.Directory.CreateDirectory(rut);

            outputDocument.Save(docrtp.RUTA.ToString());
            outputDocument.Save(ms);



            crypt.Encriptar(ms, docrtp.RUTA.ToString(), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));


            //registrar documento comunicacion oficial despachada
            string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            string emailSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            string emailSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
            string emailSMTPPwd = ConfigurationManager.AppSettings["SMTPPwd"];
            string asunto = "Respuesta a comunicación oficial recibida con radicado N° " + radicadodocumento.S_RADICADO.ToString();
            string contenido = "Respuesta a comunicación oficial recibida con radicado N° " + radicadodocumento.S_RADICADO.ToString();
            string rutaCOD = @"D:\" + tramiteTala.codTramite;//ConfigurationManager.AppSettings["rutacerticadoV"] + tramiteTala.codTramite;
            if (!Directory.Exists(rutaCOD))
            {

                Directory.CreateDirectory(rutaCOD);
            }

            rutaCOD += "\\comunicacioOficialDespachada" + tramiteTala.codTramite + ".pdf";
            reportcod.ExportToPdf(rutaCOD);
            try
            {
                Utilidades.EmailMK.EnviarEmail(emailFrom, mail, asunto, contenido, emailSMTPServer, true, emailSMTPUser, emailSMTPPwd, rutaCOD);

            }
            catch
            {
            }
            doc docrtpCOD = new doc();
            docrtpCOD = RegistrarDocumento(Int32.Parse(tramiteTala.codFuncionario), Int32.Parse(idRadicadoCOD), numPaginasCOD, Int32.Parse(tramiteTala.codTramite), Int32.Parse(tramiteTala.codProceso), 12);
            ObjectParameter rta2 = new ObjectParameter("rTA", typeof(string));
            //fldb.SP_SET_INDICE_TALACOD(Convert.ToDecimal(docrtpCOD.IDDOC), radicadodocumento.D_RADICADO.ToString(), radicadodocumento.S_RADICADO, nombreSolicitante, Int32.Parse(tramiteTala.codTramite), municipioArbol, direccionArbol, rta2);
            dbFlora.SP_SET_INDICE_TALACOD(Convert.ToDecimal(docrtpCOD.IDDOC), radicadodocumento.D_RADICADO.ToString(), radicadodocumento.S_RADICADO, nombreSolicitante, Int32.Parse(tramiteTala.codTramite), municipioArbol, direccionArbol, rta2);
            String rut2 = docrtpCOD.file.ToString().Substring(0, docrtpCOD.file.ToString().Length - 1);
            System.IO.Directory.CreateDirectory(rut2);

            outpuCOD.Save(docrtpCOD.RUTA.ToString());


            crypt.Encriptar(streamCOD, docrtpCOD.RUTA.ToString(), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
            //return File(streamCOD.GetBuffer(), "application/pdf", "comunicacionoficialdespachada.pdf");
            //return Content("");
            return File(streamCOD.GetBuffer(), "application/pdf", "comunicacionoficialdespachada.pdf");

        }

        private void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int p1, int p2, int p3, int p4)
        {
            throw new NotImplementedException();
        }
        private void DrawImage2(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }
        private doc RegistrarDocumento(int codFuncionario, int idRadicado, int numPaginas, int idTramit, int idproceso, int serieCod)
        {
            string rutaDocumento;
            int idCodDocumento;


            TBRUTAPROCESO rutaProceso = db.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == idproceso).FirstOrDefault();
            TBTRAMITEDOCUMENTO ultimoDocumento = db.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == idTramit).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();
            RADICADO_DOCUMENTO radicado = db.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicado).FirstOrDefault();

            if (ultimoDocumento == null)
                idCodDocumento = 1;
            else
                idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

            //rutaDocumento = rutaProceso.PATH +  "\\" + idTramit.ToString() + "-" + idCodDocumento.ToString() + ".pdf";
            rutaDocumento = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(idTramit), 100) + idTramit.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";



            TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
            TBTRAMITE_DOC relDocTra = new TBTRAMITE_DOC();

            documento.CODDOCUMENTO = idCodDocumento;
            documento.CODTRAMITE = idTramit;
            documento.TIPODOCUMENTO = 1;
            documento.FECHACREACION = DateTime.Now;
            documento.CODFUNCIONARIO = codFuncionario;
            documento.ID_USUARIO = codFuncionario;
            documento.RUTA = rutaDocumento;
            documento.MAPAARCHIVO = "M";
            documento.MAPABD = "M";
            documento.PAGINAS = numPaginas;
            documento.CODSERIE = serieCod;
            //Convert.ToInt32(radicado.CODSERIE);

            db.Entry(documento).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            relDocTra.CODTRAMITE = idTramit;
            relDocTra.CODDOCUMENTO = idCodDocumento;
            relDocTra.ID_DOCUMENTO = documento.ID_DOCUMENTO;
            db.Entry(relDocTra).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            doc doc = new doc();
            doc.RUTA = rutaDocumento;
            doc.IDDOC = idCodDocumento.ToString();
            doc.file = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(idTramit), 100);
            return doc;
        }
        public ActionResult consultarDepartamento()
        {
            String sql = " SELECT ID_DEPTO , CODIGO, NOMBRE FROM GENERAL.DEPARTAMENTOS";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarTamanoFoto()
        {
            String sql = " SELECT VALOR from general.PARAMETROS where CLAVE='TamanoFotoTala'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarDocFoto()
        {
            String sql = " SELECT VALOR from general.PARAMETROS where CLAVE='TamanoDocTala'";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarId()
        {
            String sql = " SELECT flora.SEQ_IDTEMP_INT_INDIVIDUO.NEXTVAL id  FROM dual";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarNombreComun()
        {
            String sql = "SELECT distinct ID_ESPECIE ID, S_NOMBRE_COMUN nombre FROM flora.FLR_ESPECIE where S_NOMBRE_COMUN!='_' ORDER BY S_NOMBRE_COMUN";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarIdReport()
        {
            String sql = " SELECT flora.SEQ_REPORT.NEXTVAL id  FROM dual";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarMunicipio(int idDepart)
        {
            String sql = "SELECT ID_MUNI, ID_DEPTO, CODIGO, NOMBRE FROM GENERAL.MUNICIPIOS where ID_DEPTO=" + idDepart;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarMunicipioArea()
        {
            String sql = "select CODIGO_MUNICIPIO CODIGO,NOMBRECOMPLETO NOMBRE from tramites.QRYMUNICIPIO where CODIGO_MUNICIPIO not in(0)";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarEstadoArbol()
        {
            String sql = "SELECT ID_ESTADO_ARBOL, S_ESTADO_ARBOL FROM flora.FLR_ESTADO_ARBOL";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarIndividuo(int id)
        {
            String sql = "select i.ID_INDIVIDUO_DISPERSO,i.COD_INDIVIDUO_DISPERSO_AMVA COD_INDIVIDUO_DISPERSO,i.NOMBRE_COMUN S_NOMBRE_COMUN,NOMBRE_CIENTIFICO,e.ID_ESPECIE,i.SHAPE.SDO_POINT.X GEOX,i.SHAPE.SDO_POINT.Y GEOY from SIARBURB.VWM_IND_DISP i left  join  siarburb.inv_individuo_disperso e on   i.ID_INDIVIDUO_DISPERSO=e.ID_INDIVIDUO_DISPERSO where i.ID_INDIVIDUO_DISPERSO=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }


        public ActionResult consultarImagSau(int id)
        {
            var client = new System.Net.WebClient();
            var dato = client.DownloadString("http://192.168.1.12/sau/consultarArbolGeneral.hyg?id=" + id);
            //  var dato = client.DownloadString("http://172.16.105.198/sau/consultarArbolGeneral.hyg?id=" + id);


            return Json(dato);
        }
        public ActionResult consultarIntervencion()
        {
            String sql = "SELECT ID_INTERVENCION, S_INTERVENCION FROM flora.FLR_INTERVENCION where ID_INTERVENCION in(1,8,25,9,14,21,19,20,3,4,5,6,7,18,24) order by S_INTERVENCION";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public String guardarArbolTala(String json, int idTramite, int codFuncionario)
        {

            ObjectParameter rta = new ObjectParameter("rta", typeof(string));
            try
            {
                //fldb.SP_SET_TALA_PODA(json, idTramite, codFuncionario, rta);
                dbFlora.SP_SET_TALA_PODA(json, idTramite, codFuncionario, rta);
            }
            catch (Exception e)
            {
            }


            return rta.Value.ToString();
        }
        public tramitePoda guardarTramiteTala(String mensaje)
        {
            tramitePoda tramite = new tramitePoda();
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            ObjectParameter rta2 = new ObjectParameter("rTA2", typeof(string));
            ObjectParameter rta3 = new ObjectParameter("rTA3", typeof(string));
            ObjectParameter rta4 = new ObjectParameter("rTA4", typeof(string));
            //fldb.SP_SET_TRAMITE_TALA_PODA(mensaje, rta, rta2, rta3, rta4);
            dbFlora.SP_SET_TRAMITE_TALA_PODA(mensaje, rta, rta2, rta3, rta4);
            tramite.codTramite = rta2.Value.ToString();
            tramite.codProceso = rta3.Value.ToString();
            tramite.codFuncionario = rta4.Value.ToString();
            return tramite;
        }
        [HttpPost]
        public ActionResult UploadArchivo1()
        {



            Decimal id = Convert.ToDecimal(Request.Params["id"]);
            Decimal tipo = Convert.ToDecimal(Request.Params["tipo"]);
            Decimal idReport = Convert.ToDecimal(Request.Params["idReport"]);
            HttpPostedFileBase file = Request.Files[0];
            int fileSize = file.ContentLength;
            string fileName = file.FileName;
            string mimeType = file.ContentType;
            System.IO.Stream fileContent = file.InputStream;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] doc = target.ToArray();

            //fldb.SP_SET_DOC_TALA(id, doc, tipo);
            dbFlora.SP_SET_DOC_TALA(id, doc, tipo);

            return Content("ok");
        }
        [HttpPost]
        public ActionResult guardarImagen()
        {



            Decimal id = Convert.ToDecimal(Request.Params["id"]);
            HttpPostedFileBase file = Request.Files[0];
            int fileSize = file.ContentLength;
            string fileName = file.FileName;
            string mimeType = file.ContentType;
            System.IO.Stream fileContent = file.InputStream;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] img = target.ToArray();

            //fldb.SP_SET_FOTO_TALA(id, img);
            dbFlora.SP_SET_FOTO_TALA(id, img);

            return Content("ok");
        }

        public ActionResult consultarMensaje()
        {
            String sql = "SELECT MENSAJE MENSAJE FROM TRAMITES.TERMINO_CONDICIONES where TIPOSOLICITUD=8 and  ((sysdate BETWEEN FECHAINI AND FECHAFIN) or (FECHAFIN is null))";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult eliminarFoto(int idFoto)
        {

            //fldb.SP_ELIMINAR_FOTO_TALA(idFoto);
            dbFlora.SP_ELIMINAR_FOTO_TALA(idFoto);
            return Content("ok");
        }
        public ActionResult guardarfotointv(String json, String observacio, int id)
        {
            //fldb.SP_SET_FOTO_INTV(id, observacio, json);
            dbFlora.SP_SET_FOTO_INTV(id, observacio, json);
            return Content("ok");
        }
        public ActionResult guardarUrbano(String cx, String cy, int municipio, int id, String dir, String ubicacion)
        {
            //fldb.SP_SET_USO_URBANO(id, cx, cy, dir, municipio, ubicacion);
            dbFlora.SP_SET_USO_URBANO(id, cx, cy, dir, municipio, ubicacion);
            return Content("ok");
        }
        public ActionResult consultarFotoIntv(String id)
        {
            String sql = "SELECT distinct ID_FOTO_INTV,URL from flora.TEMP_FOTO_INTV where ID_TERCERO_INT=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarobservacionTercero(String id)
        {
            String sql = "SELECT distinct OBSERVACION from flora.TEMP_TERCERO_INT where ID_TERCERO_INT=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }


        public ActionResult consultarDepartamentosSol()
        {
            String sql = "SELECT distinct ID_DEPTO,NOMBRE FROM GENERAL.DEPARTAMENTOS";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarmunicipioxDepartamento(String id)
        {
            String sql = "SELECT ID_MUNI,NOMBRE FROM GENERAL.MUNICIPIOS where ID_DEPTO=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarTerceroIntervencion(String tipo)
        {
            String sql = "SELECT DISTINCT Tint.ID_TERCERO_INT,Tint.Id_Individuo,Ti.S_Intervencion,TO_CHAR(Tint.Fecha_Int,'DD/MM/YYYY') fechain,Tint.Codtramite,Tint.Id_Intervencion,Ti.Id_Intervencion id_tipo,CASE tint.tipo WHEN 0 THEN 'POR EJECUTAR' else 'EJECUTADA' END ESTADO,Tint.Tipo FROM flora.TEMP_TERCERO_INT tint INNER JOIN SIARBURB.INT_ARBOLADO_RECOMENDACION ar ON ar.Id_Recomendacion=Tint.Id_Recomendada INNER JOIN siarburb.FLR_INTERVENCION ti ON Ti.Id_Intervencion=Ar.Id_Recomendacion WHERE tint.tipo=" + tipo;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public String guardarFotoTala(String json, int idTramite, int codFuncionario)
        {

            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            try
            {
                //fldb.SP_SET_TALA_PODA(json, idTramite, codFuncionario, rta);
                dbFlora.SP_SET_TALA_PODA(json, idTramite, codFuncionario, rta);
            }
            catch (Exception e)
            {
            }


            return rta.Value.ToString();
        }

        public ActionResult consultarMensajeszona(String id)
        {
            String sql = "select i.ID_INDIVIDUO_DISPERSO,i.COD_INDIVIDUO_DISPERSO_AMVA COD_INDIVIDUO_DISPERSO,i.NOMBRE_COMUN S_NOMBRE_COMUN,NOMBRE_CIENTIFICO,e.ID_ESPECIE,i.SHAPE.SDO_POINT.X GEOX,i.SHAPE.SDO_POINT.Y GEOY from SIARBURB.VWM_IND_DISP i left  join  siarburb.inv_individuo_disperso e on   i.ID_INDIVIDUO_DISPERSO=e.ID_INDIVIDUO_DISPERSO where i.ID_INDIVIDUO_DISPERSO=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

    }
    public class doc
    {
        public byte[] DOC { get; set; }
        public String IDARBOL { get; set; }
        public String TIPO { get; set; }
        public String RUTA { get; set; }
        public String IDDOC { get; set; }
        public String file { get; set; }
    }
    public class tramitePoda
    {
        public String codTramite { get; set; }
        public String codProceso { get; set; }
        public String codFuncionario { get; set; }

    }




}
