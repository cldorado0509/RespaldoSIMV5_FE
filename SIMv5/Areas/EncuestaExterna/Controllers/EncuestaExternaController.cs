using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.ServiceModel.Channels;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using SIM.Areas.Seguridad.Models;
using System.Web.Script.Serialization;
using System.Web.Http;
using SIM.Areas.ControlVigilancia.Controllers;
using System.Data;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Sql;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SIM.Areas.Tramites.Models;
using SIM.Utilidades;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Drawing.Imaging;
using SIM.Data;
using SIM.Models;
using SIM.Data.Tramites;
using DevExpress.Pdf;
using System.Web.Hosting;
using SIM.Utilidades;
using SIM.Data.Control;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using ActionNameAttribute = System.Web.Mvc.ActionNameAttribute;
using SIM.Utilidades.SLExcelUtility;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class VigenciasRelacionadas
    {
        public int ID_VIGENCIA { get; set; }
        public string VIGENCIA { get; set; }
        public int? ID_INSTALACION { get; set; }
        public int NUM_SOLUCIONES { get; set; }
        public int NUM_ENVIADOS { get; set; }
        public int ENVIADO { get; set; }
    }

    public class DatosBaseEncuesta
    {
        public string ENCUESTA_TITULO { get; set; }
        public int? ID_PREGUNTA_DESC { get; set; }
    }

    public class DatosEstadoVigencia
    {
        public int ID_VIGENCIA { get; set; }
        public int ID_TERCERO { get; set; }
        public int ID_INSTALACION { get; set; }
        public string VALOR_VIGENCIA { get; set; }
        public int RADICADO { get; set; }
    }

    public class DatosCargaMasiva
    {
        public int ins { get; set; }
        public int ter { get; set; }
        public int vig { get; set; }
        public string val { get; set; }
        public string datos { get; set; }
    }

    public class GrupoVigencia
    {
        public int? ID_VIGENCIA_GRUPO { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_NOMBRE_ORIGINAL { get; set; }
    }

    public class EncuestaExternaController : Controller
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();


        //SIM.Areas.Tala.Models.EntitiesTala dbf = new SIM.Areas.Tala.Models.EntitiesTala();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        decimal idTerceroUsuario;
        AppSettingsReader webConfigReader = new AppSettingsReader();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IngresoEncuesta()
        {
            return View();
        }

        /*public ActionResult EncuestaCardinalidad()
        {
            int vigencia = Convert.ToInt32(Request.Params["vigencia"]);
            ViewBag.vigencia = vigencia;
            int valor = Convert.ToInt32(Request.Params["valor"]);
            ViewBag.valor = valor;
            int instalacion = Convert.ToInt32(Request.Params["instalacion"]);
            ViewBag.instalacion = instalacion;
            return View();
        }*/

        public ActionResult EncuestaInvalida()
        {
            return View();
        }

        [System.Web.Mvc.Authorize]
        public ActionResult EncuestaCardinalidad()
        {
            var idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            int valor = Convert.ToInt32(Request.Params["valor"]);
            ViewBag.valor = valor;
            int tercero = Convert.ToInt32(Request.Params["tercero"]);
            ViewBag.tercero = tercero;
            int instalacion = Convert.ToInt32(Request.Params["instalacion"]);
            ViewBag.instalacion = instalacion;
            int vigencia = Convert.ToInt32(Request.Params["vigencia"]);
            ViewBag.vigencia = vigencia;

            var idPrimeraEncuesta = db.FRM_GENERICO_ESTADO.Where(ge => ge.ID_TERCERO == tercero && ge.ID_INSTALACION == instalacion && ge.ID_VIGENCIA == vigencia && ge.VALOR == valor).FirstOrDefault();

            int codigoEncuesta = (idPrimeraEncuesta == null ? 0 : Convert.ToInt32(idPrimeraEncuesta.ID_ESTADO));
            ViewBag.codigoEncuesta = codigoEncuesta;

            var vigenciaArchivo = db.VIGENCIA.Where(v => v.ID_VIGENCIA == vigencia).FirstOrDefault();

            if (vigenciaArchivo != null && vigenciaArchivo.S_NOMBRE_ARCHIVO != null && vigenciaArchivo.S_NOMBRE_ARCHIVO.Trim() != "")
            {
                ViewBag.archivoVigencia = true;
            }
            else
            {
                ViewBag.archivoVigencia = false;
            }

            ViewBag.OcultarEnvio = (idTercero != tercero);

            return View();
        }

        [HttpPost, ActionName("CargarPlantillaDiligenciada")]
        //public ActionResult PostCargarPlantillaDiligenciada(int i, int t, int vg, string v)
        //public ActionResult PostCargarPlantillaDiligenciada(HttpPostedFileBase file)
        public string PostCargarPlantillaDiligenciada(int t, int i, int vg, string v)
        {
            string resultado = "Archivo Procesado Satisfactoriamente !!!";
            try
            {
                HttpPostedFileBase file = Request.Files[0];

                var cargaMasiva = new CargaMasiva();
                resultado = cargaMasiva.CargarPlantillaDiligenciada(t, i, vg, v, file);
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = "Error Cargando Archivo.";
            }

            return resultado;
        }

        private string ReadExcelasJSON(string filePath)
        {
            try
            {
                DataTable dtTable = new DataTable();
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
                    {
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                        SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();



                        for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
                        {
                            List<string> rowList = new List<string>();
                            for (int rCnt1 = 0; rCnt1
                                < thesheetdata.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
                            {

                                Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
                                //statement to take the integer value  
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                //first row will provide the column name.
                                                if (rCnt == 0)
                                                {
                                                    dtTable.Columns.Add(item.Text.Text);
                                                }
                                                else
                                                {
                                                    rowList.Add(item.Text.Text);
                                                }
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (rCnt != 0)//reserved for column values
                                    {
                                        rowList.Add(thecurrentcell.InnerText);
                                    }
                                }
                            }
                            if (rCnt != 0)//reserved for column values
                                dtTable.Rows.Add(rowList.ToArray());

                        }

                    }

                    return JsonConvert.SerializeObject(dtTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ActionResult TerceroEncuestaUserExter()
        {
            return View();
        }
        public ActionResult geocodificador()
        {
            return View();
        }
        public ActionResult tipoHora()
        {
            return View();
        }

        public ActionResult vigencia()
        {
            string sql = "SELECT NVL(PERMITE_COPIA, 'N') || '-' || NVL(TIPO_TERMINOS, 'V') || '-' || NVL(TIPO_FORMULARIO, '1') || '-' || NVL(URL_PERSONALIZADA, '') FROM CONTROL.VIGENCIA WHERE ID_VIGENCIA = " + Request.Params["id"];

            string datosVigencia = db.Database.SqlQuery<string>(sql).FirstOrDefault();

            int id = Convert.ToInt32(Request.Params["id"]);
            ViewBag.idVigen = id;
            int tipo = Convert.ToInt32(Request.Params["tipo"]);
            ViewBag.tipo = tipo;
            ViewBag.permiteCopia = (datosVigencia.Split('-')[0] == "S");
            ViewBag.TipoTerminos = datosVigencia.Split('-')[1];
            ViewBag.TipoFormulario = datosVigencia.Split('-')[2];
            ViewBag.urlFormulario = datosVigencia.Split('-')[3];
            return View();
        }
        public ActionResult ModificaEncuesta()
        {
            int id = Convert.ToInt32(Request.Params["idestado"]);
            ViewBag.idEstado = id;

            return View();
        }

        public ActionResult AdminEncuestaExterna()
        {
            return View();
        }
        public ActionResult GestionarEncuesta()
        {
            int idestado = Convert.ToInt32(Request.Params["idestado"]);
            ViewBag.idestado = idestado;
            return View();
        }

        public ActionResult GestionarEncUsExterno(int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            int idTerceroActual = 0;
            bool terceroUsuario = true;
            bool seleccionTercero = false;

            idTerceroActual = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

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
                idTercero = idTerceroActual;

            var tercero = (from ter in db.TERCERO where ter.ID_TERCERO == idTercero select ter.S_RSOCIAL).FirstOrDefault();

            int idestado = Convert.ToInt32(Request.Params["idestado"]);
            ViewBag.idestado = idestado;
            ViewBag.idTercero = idTercero;
            ViewBag.NombreTercero = tercero;
            ViewBag.seleccionTercero = seleccionTercero;

            ViewBag.Habilitar = (idTercero != idTerceroActual);

            return View();
        }

        public string ValidarEncuestas(int instalacion, int vigencia, int valor)
        {
            string sql = "SELECT SUM(CASE nvl(S.Id_Solucion,0) WHEN 0 THEN 1 ELSE 0 END) + SUM(CASE ge.TIPO_GUARDADO WHEN 0 THEN 1 ELSE 0 END) NODILIGENCIADAS FROM control.FRM_GENERICO_ESTADO ge INNER JOIN CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO LEFT JOIN control.Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO WHERE ge.Activo=0 and ge.NOMBRE is not null and vs.ID_VIGENCIA = " + vigencia.ToString() + " and vs.valor='" + valor.ToString() + "' and ge.ID_INSTALACION=" + instalacion.ToString();

            var encuestasNoDiligenciadas = db.Database.SqlQuery<int?>(sql).FirstOrDefault();

            if (encuestasNoDiligenciadas == null) // No hay encuestas
            {
                return "SE";
            }
            else if (encuestasNoDiligenciadas > 0) // Hay encuestas sin diligenciar
            {
                return "NO";
            }
            else
            {
                return "SI";
            }
        }

        [System.Web.Mvc.Authorize]
        public ActionResult CargaMasivaEncuestas(DatosCargaMasiva datosCarga)
        {
            string lineasError = "";
            string lineasRepetidos = "";
            int contLinea = 1;
            int contLineaOK = 0;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            using (StringReader reader = new StringReader(datosCarga.datos))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {
                        var datosLinea = line.Trim().Split(',', ';');

                        if (datosLinea.Length == 2)
                        {
                            try
                            {
                                dbControl.SP_SET_ESTADO_CARDINALIDAD2(0, idUsuario, datosCarga.ter, datosCarga.ins, 1, datosCarga.vig, datosCarga.val, datosLinea[0].Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim(), datosLinea[1].Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim());
                                contLineaOK++;
                            }
                            catch
                            {
                                if (lineasError == "")
                                    lineasError = contLinea.ToString();
                                else
                                    lineasError += ", " + contLinea.ToString();
                            }
                        }
                        else
                        {
                            if (lineasError == "")
                                lineasError = contLinea.ToString();
                            else
                                lineasError += ", " + contLinea.ToString();
                        }
                    }

                    contLinea++;
                }
            }

            return Json(new { respuesta = "OK:" + contLineaOK.ToString() + ":" + lineasError }, JsonRequestBehavior.AllowGet);
        }

        public string EnviarEncuestas(int instalacion, int vigencia, int valor)
        {
            //string sql = "UPDATE CONTROL.FRM_GENERICO_ESTADO SET TIPO_GUARDADO = 1 WHERE Activo=0 and NOMBRE is not null and ID_VIGENCIA = " + vigencia.ToString() + " and valor=" + valor.ToString() + " and ID_INSTALACION=" + instalacion.ToString();
            string sql = "UPDATE CONTROL.FRM_GENERICO_ESTADO SET TIPO_GUARDADO = 1 WHERE ID_ESTADO IN (SELECT ge.ID_ESTADO FROM CONTROL.FRM_GENERICO_ESTADO ge INNER JOIN CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO WHERE ge.ACTIVO = 0 AND vs.ID_VIGENCIA = " + vigencia.ToString() + " AND vs.VALOR = '" + valor.ToString() + "' AND ge.ID_INSTALACION = " + instalacion.ToString() + ")";

            try
            {
                var numActualizadas = db.Database.ExecuteSqlCommand(sql);
            } catch (Exception error)
            {
                return "Error";
            }

            return "OK";
        }

        public ActionResult validarGenerarRadicado(String ID_ESTADO, String idinstalacion)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            }

            //String sql = "select distinct RADICADO,CODRADICADO from control.FRM_GENERICO_ESTADO where ID_TERCERO=" + idTerceroUsuario + " and ID_INSTALACION=" + idinstalacion + " and ID_ESTADO=" + ID_ESTADO;
            String sql = "SELECT NVL(v.RADICAR, 0) AS RADICADO, ge.CODRADICADO FROM CONTROL.FRM_GENERICO_ESTADO ge LEFT OUTER JOIN CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO LEFT OUTER JOIN CONTROL.VIGENCIA v ON vs.ID_VIGENCIA = v.ID_VIGENCIA WHERE ge.ID_TERCERO = " + idTerceroUsuario + " AND ge.ID_INSTALACION = " + idinstalacion + " AND ge.ID_ESTADO = " + ID_ESTADO;

            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult guardarradicadocod(int idestado, String codradicado)
        {
            dbControl.SP_SET_GUARDARRADICADO(idestado, codradicado);
            return Content("ok");
        }

        public ActionResult encuestaUsuaroExterno()
        {
            int idVigen = Convert.ToInt32(Request.Params["idVigencia"]);
            int tipo = Convert.ToInt32(Request.Params["tipo"]);
            int estado = Convert.ToInt32(Request.Params["estado"]);
            int card = Convert.ToInt32(Request.Params["card"]);
            string old = Request.Params["old"] ?? "";

            ViewBag.TipoInvocacion = "I";

            ViewBag.idVigen = idVigen;
            ViewBag.estado = estado;
            ViewBag.tipo = tipo;
            ViewBag.card = card;
            ViewBag.old = old;

            var vigencia = db.VIGENCIA.Where(v => v.ID_VIGENCIA == idVigen).FirstOrDefault();

            ViewBag.TipoTerminos = (vigencia.TIPO_TERMINOS ?? "V");
            ViewBag.TipoFormulario = (vigencia.TIPO_FORMULARIO ?? 1);
            ViewBag.urlFormulario = (vigencia.URL_PERSONALIZADA ?? "");

            if (vigencia.TIPO_TERMINOS == "E")
            {
                ViewBag.TerminosCondiciones = vigencia.TERMINO;
            }

            int numDependencias = db.Database.SqlQuery<int>("SELECT COUNT(0) FROM CONTROL.ENC_VIGENCIA_DEPENDENCIA WHERE ID_VIGENCIA = " + idVigen.ToString()).FirstOrDefault();

            if (numDependencias > 0)
            {
                ViewBag.Dependencias = 1;
            }
            else
            {
                ViewBag.Dependencias = 0;
            }

            return View("EncuestaUsuarioExterno");
        }

        public ActionResult EncuestaUsuarioExterno(string d)
        {
            if (d != null)
            {
                ViewBag.TipoInvocacion = d.Substring(0, 1);
                try
                {
                    var data = Cryptografia.DecryptString(d.Substring(1), "*&&%tyU23a2").Split('|');

                    /*int idVigen = Convert.ToInt32(Request.Params["idVigencia"]);
                    int tipo = Convert.ToInt32(Request.Params["tipo"]);
                    int estado = Convert.ToInt32(Request.Params["estado"]);
                    int card = Convert.ToInt32(Request.Params["card"]);
                    string old = Request.Params["old"] ?? "";

                    ViewBag.idVigen = idVigen;
                    ViewBag.estado = estado;
                    ViewBag.tipo = tipo;
                    ViewBag.card = card;
                    ViewBag.old = old;
                    ViewBag.no = Request.Params["no"];*/

                    int idVigen = Convert.ToInt32(data[0]);

                    ViewBag.idVigen = Convert.ToInt32(data[0]);
                    ViewBag.estado = Convert.ToInt32(data[1]);
                    ViewBag.tipo = Convert.ToInt32(data[2]);
                    ViewBag.card = Convert.ToInt32(data[3]);
                    ViewBag.old = "";
                    ViewBag.no = data[4];

                    var tipoTerminos = db.VIGENCIA.Where(v => v.ID_VIGENCIA == idVigen).FirstOrDefault();

                    ViewBag.TipoTerminos = (tipoTerminos.TIPO_TERMINOS ?? "V");

                    if (tipoTerminos.TIPO_TERMINOS == "E")
                    {
                        ViewBag.TerminosCondiciones = tipoTerminos.TERMINO;
                    }

                    int numDependencias = db.Database.SqlQuery<int>("SELECT COUNT(0) FROM CONTROL.ENC_VIGENCIA_DEPENDENCIA WHERE ID_VIGENCIA = " + idVigen.ToString()).FirstOrDefault();

                    if (numDependencias > 0)
                    {
                        ViewBag.Dependencias = 1;
                    }
                    else
                    {
                        ViewBag.Dependencias = 0;
                    }

                    return View();
                }
                catch
                {
                    ViewBag.Respuesta = "Encuesta Inválida";
                    return View("RespuestaEncuesta");
                }
            }
            else
            {
                ViewBag.Respuesta = "Encuesta Inválida";
                return View("RespuestaEncuesta");
            }
        }

        public ActionResult EncuestaClave(int idCE, string n, string c) // idV - Id Vigencia, n - Nombre Encuesta (email), c - Clave, cr - Cardinalidad, t - Tipo
        {
            return ProcesarEncuestaClave(idCE, n, c, false);
        }

        //public ActionResult EncuestaClave(int idV, string n, string c, int cr, int t) // idV - Id Vigencia, n - Nombre Encuesta (email), c - Clave, cr - Cardinalidad, t - Tipo
        public ActionResult ProcesarEncuestaClave(int idCE, string n, string c, bool segundoIntento) // idV - Id Vigencia, n - Nombre Encuesta (email), c - Clave, cr - Cardinalidad, t - Tipo
        {
            decimal? idV;
            string valorVigencia;

            if (c != null && c.Trim() != "")
            {
                var registroBaseEncuesta = db.FRM_GENERICO_ESTADO.Where(ge => ge.ID_ESTADO == idCE).FirstOrDefault();

                if (registroBaseEncuesta != null)
                {
                    idV = registroBaseEncuesta.ID_VIGENCIA;
                    valorVigencia = registroBaseEncuesta.VALOR.ToString();

                    var estado = (from ge in db.FRM_GENERICO_ESTADO
                                  join vs in db.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                                  where ge.NOMBRE.Trim().ToUpper() == n.Trim().ToUpper() && vs.ID_VIGENCIA == idV && vs.VALOR == valorVigencia && ge.ACTIVO == "0"
                                  select ge).FirstOrDefault();

                    if (estado != null)
                    {
                        if (estado.S_CLAVE.Trim() == c.Trim())
                        {
                            if (estado.TIPO_GUARDADO == 0)
                            {
                                //string data = Cryptografia.EncryptString(idV.ToString() + "|" + estado.ID_ESTADO.ToString() + "|" + t.ToString() + "|" + cr.ToString() + "|" + n.Replace("|", " "), "*&&%tyU23a2");
                                string data = Cryptografia.EncryptString(idV.ToString() + "|" + estado.ID_ESTADO.ToString() + "|1|2|" + n.Replace("|", " "), "*&&%tyU23a2");

                                return RedirectToAction("EncuestaUsuarioExterno", "EncuestaExterna", new { d = "E" + data });
                            }
                            else
                            {
                                ViewBag.Respuesta = "La Encuesta ya fue diligenciada";
                                return View("RespuestaEncuesta");
                            }
                        }
                        else
                        {
                            ViewBag.Respuesta = "Clave Inválida";
                            return View("RespuestaEncuesta");
                        }
                    }
                    else // Crea la encuesta
                    {
                        if (!segundoIntento)
                        {
                            dbControl.SP_SET_ESTADO_CARDINALIDAD2(0, 0, Convert.ToInt32(registroBaseEncuesta.ID_TERCERO), Convert.ToInt32(registroBaseEncuesta.ID_INSTALACION), 1, Convert.ToInt32(registroBaseEncuesta.ID_VIGENCIA), registroBaseEncuesta.VALOR.ToString().Replace("-", ""), n, c);
                            return ProcesarEncuestaClave(idCE, n, c, true);
                        }
                        else
                        {
                            ViewBag.Respuesta = "Error Creando Encuesta";
                            return View("RespuestaEncuesta");
                        }
                    }
                }
                else
                {
                    ViewBag.Respuesta = "Código de Encuesta Inválido";
                    return View("RespuestaEncuesta");
                }
            }
            else
            {
                ViewBag.Respuesta = "Encuesta Inválida";
                return View("RespuestaEncuesta");
            }
        }

        [System.Web.Mvc.Authorize]
        public ActionResult EncuestaEstado(int e, int cr, int t)
        {
            var encuesta = (from ge in db.FRM_GENERICO_ESTADO
                            join vs in db.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                            where ge.ID_ESTADO == e
                            select new
                            {
                                vs.ID_VIGENCIA,
                                ge.NOMBRE

                            }).FirstOrDefault();

            if (encuesta != null)
            {
                string data = Cryptografia.EncryptString(encuesta.ID_VIGENCIA.ToString() + "|" + e.ToString() + "|" + t.ToString() + "|" + cr.ToString() + "|" + (encuesta.NOMBRE ?? "").Replace("|", " "), "*&&%tyU23a2");

                return RedirectToAction("EncuestaUsuarioExterno", "EncuestaExterna", new { d = "I" + data });
            }
            else
            {
                ViewBag.Respuesta = "Encuesta Inválida";
                return View("RespuestaEncuesta");
            }
        }

        public ActionResult consultarEncuestasAsociadas()
        {
            String sql = "SELECT e.ID_ENCUESTA ID, e.S_NOMBRE NOMBRE FROM control.ENC_ENCUESTA e inner join control.FORMULARIO_ENCUESTA fe on fe.ID_ENCUESTA=e.ID_ENCUESTA where fe.ID_FORMULARIO=14";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        //public ActionResult consultarcombohijo(int idpre, int fil)
        //{

        //    ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));

        //    db.SP_GET_COMBOHIJO(idpre,fil, jSONOUT);
        //    return Json(jSONOUT.Value);
        //}
        /*public ActionResult terceroinstalacionunico(int id)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);


            }
            String sql = "SELECT ID_ESTADO FROM control.FRM_GENERICO_ESTADO  where ID_TERCERO=" + idTerceroUsuario + " and ID_INSTALACION="+id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }*/

        public ActionResult terceroinstalacionunico(int id, int id_v)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);


            }
            String sql = "SELECT ID_ESTADO FROM control.FRM_GENERICO_ESTADO  where ID_TERCERO=" + idTerceroUsuario + " and ID_INSTALACION=" + id + " and ID_VIGENCIA=" + id_v;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarEstadoV(String idesatdo)
        {
            String sql = "SELECT valor,ID_TERCERO,ID_INSTALACION from control.FRM_GENERICO_ESTADO where ID_ESTADO=" + idesatdo;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarVigenciaEnc(int id)
        {
            //String sql = "SELECT ID_VIGENCIA,TIPOVIGENCIA,FECHA_INICIO,FECHA_FIN,VIGENCIA,CARDINALIDAD FROM control.VIGENCIA where ID_VIGENCIA=" + id;
            String sql = "SELECT ID_VIGENCIA,TIPOVIGENCIA,FECHA_INICIO,FECHA_FIN,VIGENCIA,CARDINALIDAD,NOMBRE,ITEM FROM control.VIGENCIA where ID_VIGENCIA=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarEstado(int idEnc)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);


            }
            String sql = "SELECT ID_ESTADO, ID_ENCUESTA, COD_USURARIO FROM control.FRM_GENERICO_ESTADO where ID_ENCUESTA=" + idEnc + " and COD_USURARIO=" + idTerceroUsuario;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarVigencia()
        {
            String sql = "";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarJsonEncuestas()
        {
            Decimal idEstado = Decimal.Parse(Request.Params["idEstado"]);
            Decimal idvigen = Decimal.Parse(Request.Params["idvigen"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            string anterior = Request.Params["anterior"] ?? "";

            if (anterior != "1")
            {
                string datosEstado = db.Database.SqlQuery<string>("SELECT ID_TERCERO || ',' || ID_INSTALACION FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();

                var datosEncuesta = (new EncuestaDatos()).ObtenerJsonEncuesta(Convert.ToInt32(idEstado), Convert.ToInt32(idform), Convert.ToInt32(idvigen), Convert.ToInt32(datosEstado.Split(',')[0]), Convert.ToInt32(datosEstado.Split(',')[1]));
                var resultado = datosEncuesta.json;

                return Json(resultado);
            }
            else
            {
                ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                dbControl.SP_GET_ENCUESTAS2(idEstado, idform, idvigen, jSONOUT);
                return Json(jSONOUT.Value);
            }
        }
        public ActionResult validarenviarenc(int idEstado)
        {
            //String sql = " select RADICADO from FRM_GENERICO_ESTADO where ID_ESTADO=" + idEstado;
            String sql = "SELECT NVL(v.RADICAR, 0) AS RADICADO FROM CONTROL.FRM_GENERICO_ESTADO ge LEFT OUTER JOIN CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO LEFT OUTER JOIN CONTROL.VIGENCIA v ON vs.ID_VIGENCIA = v.ID_VIGENCIA WHERE ge.ID_ESTADO = " + idEstado.ToString();

            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);

        }

        //public ActionResult guardarUsuarioExterno(int vig, String nombre, int idEncuesta, String fechaini, String fechaFin, String arrEncuesta, int rol, String terminos, int tipoInstalacion, int cardinalidad, int radicar)
        public ActionResult guardarUsuarioExterno(int vig, String nombre, int idEncuesta, String fechaini, String fechaFin, String arrEncuesta, int rol, String terminos, int tipoInstalacion, int cardinalidad, int radicar, String nombreItem)
        {

            try
            {
                //db.SP_SET_ENCUESTA_ROL2(vig, nombre, fechaini, idEncuesta, fechaFin, arrEncuesta, rol, terminos, tipoInstalacion, cardinalidad, radicar);
                dbControl.SP_SET_ENCUESTA_ROL2(vig, nombre, fechaini, idEncuesta, fechaFin, arrEncuesta, rol, terminos, tipoInstalacion, cardinalidad, radicar, nombreItem);
            }
            catch (Exception x)
            {
                var res = x.Message;
                throw;
            }

            return Content("ok");
        }
        public ActionResult agregarVigencia(int idvig, String vigen, int estado)
        {

            dbControl.SP_SET_MODIFICARVIGENCIA(idvig, vigen, estado);



            return Content("ok");
        }
        public ActionResult crearEstado(int idEncu, int idtercero, int idInstalacion, int rad)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            dbControl.SP_SET_CREAR_ESTADO_GENERICO2(idEncu, idUsuario, rta, idtercero, idInstalacion, rad);
            return Json(rta.Value);
        }
        public ActionResult crearEstadoCardinalidad(int idEncu, int idtercero, int idInstalacion, int card, int vigencia, String vige)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            dbControl.SP_SET_ESTADO_CARDINALIDAD2(idEncu, idUsuario, idtercero, idInstalacion, card, vigencia, vige.Replace("-", ""));
            return Content("ok");
        }

        /*public ActionResult CrearEstadosCardinalidad(int idEncu, int idtercero, int idInstalacion, int card, int vigencia, String vige)
        {

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }


            dbControl.SP_SET_ESTADO_CARDINALIDAD2(idEncu, idUsuario, idtercero, idInstalacion, card, vigencia, vige.Replace("-", ""));
            return Content("ok");
        }*/

        public ActionResult GuardarInformacionEncuesta()
        {
            string jsonInfo = Request.Params["jsonInfo"];
            ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
            Decimal idCapEs = Convert.ToDecimal(Request.Params["idCapEstado"]);
            Decimal strEstado = Convert.ToDecimal(Request.Params["idform"]);
            string marcarModificados = Request.Params["mm"] ?? "1";
            dbControl.SP_SET_ENCUESTAS(idCapEs, strEstado, jsonInfo, rTA);

            try
            {
                if (marcarModificados == "1")
                    dbControl.SP_SET_DATOS_MODIFICADOS(idCapEs);
            }
            catch (Exception error)
            {
                string mensaje;

                mensaje = error.Message;
            }

            return Content("" + rTA.Value);
        }

        public ActionResult consultarEncVigen(int id, int instalacion)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                //idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            String sql = "SELECT distinct vs.ID_VIGENCIA,vs.VALOR FROM control.VIGENCIA_SOLUCION vs left JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO =ge.ID_ESTADO where Ge.Activo=0  and vs.ID_VIGENCIA=" + id + " and ge.ID_TERCERO=" + idTerceroUsuario + "  and Ge.Id_Instalacion=" + instalacion;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarTermino(int id)
        {
            String sql = "SELECT TERMINO,TIPOINSTALACION,RADICAR FROM VIGENCIA where ID_VIGENCIA=" + id;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarTerceroEncuesta()
        {

            List<terc> terceroList = new List<terc>();
            var tercero = (from t in db.VW_TERCERO



                           select new
                           {
                               t.ID,
                               t.NOMBRE
                           }).ToList();
            foreach (var item in tercero)
            {
                terc terc = new terc();
                terc.id = item.ID.ToString();
                terc.nombre = item.NOMBRE;


                terceroList.Add(terc);
            }


            var serializer = new JavaScriptSerializer();


            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new { Value = "foo", Text = "var" };
            var result = new ContentResult
            {
                Content = serializer.Serialize(terceroList),
                ContentType = "application/json"
            };
            return result;

        }

        public ActionResult modificarEstado(int idEStado)
        {

            dbControl.SP_SET_MODIFICAR_ESTADO(idEStado);
            return Content("ok");
        }
        public ActionResult enviarEncuesta(int idestado) {
            var cardinalidad = (from ge in db.FRM_GENERICO_ESTADO
                                join vs in db.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                                join v in db.VIGENCIA on vs.ID_VIGENCIA equals v.ID_VIGENCIA
                                where ge.ID_ESTADO == idestado
                                select v.CARDINALIDAD).FirstOrDefault();

            var encuesta = (from ge in db.FRM_GENERICO_ESTADO
                            where ge.ID_ESTADO == idestado
                            select ge).FirstOrDefault();



            if (cardinalidad == 1)
            {
                encuesta.TIPO_GUARDADO = 1;
            }
            else
            {
                encuesta.TIPO_GUARDADO = 2;
            }

            db.Entry(encuesta).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //dbControl.SP_SET_MODIFICAR_ESTADO(idestado);
            return Content("ok");
        }
        public ActionResult EliminarEncuesta(int idestado)
        {
            dbControl.SP_SET_ELIMINAR_ENCUESTA(idestado);

            try
            {
                dbControl.SP_SET_DATOS_MODIFICADOS(idestado);
            }
            catch (Exception error)
            {
                string mensaje;

                mensaje = error.Message;
            }

            return Content("ok");
        }
        public ActionResult EliminarEncuestaCardi(int idv, int val, int inst)
        {
            dbControl.SP_SET_ELI_ENC_CARDINALIAD(idv, val, inst);
            return Content("ok");
        }
        public ActionResult clonarEncuesta(int idvigencia, String valor, int idestado, int idtercero, int instalacion)
        {

            dbControl.SP_SET_CLONAR_ENCUESTA_EXTERNO(idvigencia, valor, idtercero, idestado, instalacion);
            return Content("ok");
        }
        public ActionResult modificarEncuestaCardinalidad(String json)
        {

            dbControl.SP_SET_MODIFICARENCUESTACARD(json);
            return Content("ok");
        }
        /*public ActionResult ConsultarEncuestaUsuarioFormulario()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                //codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
            }
            //String sql = "SELECT distinct ge.TIPO_GUARDADO,v.ID_VIGENCIA,ge.ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,ge.COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION where fe.ID_FORMULARIO=14 and ge.ID_TERCERO=" + idTerceroUsuario + " union all SELECT DISTINCT -1 TIPO_GUARDADO,v.ID_VIGENCIA,0 ID_ESTADO,v.NOMBRE encuesta,'' vigencia,'' tipovigencia,'Cardinalidad N' estadoencuesta,'0' COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON v.ID_VIGENCIA =ge.ID_VIGENCIA INNER JOIN GENERAL.TERCERO t ON t.ID_TERCERO=ge.ID_TERCERO INNER JOIN GENERAL.INSTALACION i ON i.ID_INSTALACION   =ge.ID_INSTALACION WHERE fe.ID_FORMULARIO=14 AND ge.ID_TERCERO=" + idTerceroUsuario;
            //String sql = "SELECT distinct ge.ID_TERCERO, ge.ID_INSTALACION , ge.TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,ge.COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION where fe.ID_FORMULARIO=14 and ge.activo=0 and ge.ID_TERCERO=" + idTerceroUsuario;
            String sql = "SELECT distinct ge.ID_TERCERO, ge.ID_INSTALACION , ge.TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,ge.COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card,  TO_CHAR(SOL.D_EDICION,'DD/MM/YYYY hh:mi:ss') D_EDICION FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION  left join (CONTROL.VW_ULTFECHA_ENCUESTA) SOL ON SOL.ID_VIGENCIA=v.ID_VIGENCIA AND SOL.ID_INSTALACION   =ge.ID_INSTALACION AND vs.VALOR=SOL.VALOR where fe.ID_FORMULARIO=14 and ge.activo=0 and ge.ID_TERCERO=" + idTerceroUsuario;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }*/

        /*public ActionResult ConsultarEncuestaUsuarioFormulario()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                //codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
            }
            String sql = "SELECT distinct ge.ID_TERCERO ,ge.ID_INSTALACION , ge.TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,ge.COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card,  TO_CHAR(SOL.D_EDICION,'DD/MM/YYYY hh:mi:ss')D_EDICION FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION  left join (CONTROL.VW_ULTFECHA_ENCUESTA) SOL ON SOL.ID_VIGENCIA=v.ID_VIGENCIA AND SOL.ID_INSTALACION   =ge.ID_INSTALACION AND vs.VALOR=SOL.VALOR where fe.ID_FORMULARIO=14 and ge.activo=0 and ge.ID_TERCERO=" + idTerceroUsuario;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }*/

        public ActionResult ConsultarEncuestaUsuarioFormulario(int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            //String sql = "SELECT distinct ge.ID_TERCERO ,ge.ID_INSTALACION , ge.TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card,  TO_CHAR(SOL.D_EDICION,'DD/MM/YYYY hh:mi:ss')D_EDICION FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION  left join (CONTROL.VW_ULTFECHA_ENCUESTA) SOL ON SOL.ID_VIGENCIA=v.ID_VIGENCIA AND SOL.ID_INSTALACION = ge.ID_INSTALACION AND vs.VALOR=SOL.VALOR AND SOL.ID_TERCERO = ge.ID_TERCERO WHERE fe.ID_FORMULARIO=14 and ge.activo=0 and ge.ID_TERCERO=" + idTerceroUsuario;
            //String sql = "SELECT distinct ge.ID_TERCERO ,ge.ID_INSTALACION , CASE WHEN ge.TIPO_GUARDADO <> 1 THEN 0 ELSE 1 END AS TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' else 'Guardado Temp'end estadoencuesta,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card,  TO_CHAR(SOL.D_EDICION,'DD/MM/YYYY hh:mi:ss')D_EDICION FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION  left join (CONTROL.VW_ULTFECHA_ENCUESTA) SOL ON SOL.ID_VIGENCIA=v.ID_VIGENCIA AND SOL.ID_INSTALACION   =ge.ID_INSTALACION AND vs.VALOR=SOL.VALOR where fe.ID_FORMULARIO=14 and ge.activo=0 and ge.ID_TERCERO= " + idTercero.ToString();
            String sql = "SELECT distinct ge.ID_TERCERO ,ge.ID_INSTALACION, vg.ID_VIGENCIA_GRUPO, NVL(vg.S_NOMBRE, 'OTROS') AS VIGENCIA_GRUPO, CASE WHEN NVL(v.TIPO_FORMULARIO, 1) = 1 THEN 'EncuestaExterna/EncuestaEstado' ELSE v.URL_PERSONALIZADA END AS URL_ENCUESTA, CASE WHEN ge.TIPO_GUARDADO <> 1 THEN 0 ELSE 1 END AS TIPO_GUARDADO,v.ID_VIGENCIA, case v.CARDINALIDAD when 1 then ge.ID_ESTADO  when 2 then  0 end as ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' else 'Guardado Temp'end estadoencuesta,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion,v.CARDINALIDAD,case v.CARDINALIDAD when 1 then 'Uno' when 2 then 'N' end card,  TO_CHAR(SOL.D_EDICION,'DD/MM/YYYY hh:mi:ss') D_EDICION, tv.TOTAL, tv.BORRADOR, tv.ENVIADAS, NVL(v.TIPO_FORMULARIO, 1) AS TIPO_FORMULARIO " +
                "FROM control.ENC_ENCUESTA e INNER JOIN " +
                "   control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA AND fe.ID_FORMULARIO=14 INNER JOIN " +
                "   control.ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN " +
                "   control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA LEFT OUTER JOIN " +
                "   control.VIGENCIA_GRUPO vg ON v.ID_VIGENCIA_GRUPO = vg.ID_VIGENCIA_GRUPO INNER JOIN " +
                "   control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN " +
                "   control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO and ge.activo=0 and ge.ID_TERCERO = " + idTercero.ToString() + " inner join " +
                "   GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join " +
                "   GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION  left join " +
                "   (CONTROL.VW_ULTFECHA_ENCUESTA) SOL ON SOL.ID_TERCERO = " + idTercero.ToString() + " AND SOL.ID_VIGENCIA=v.ID_VIGENCIA AND SOL.ID_INSTALACION   =ge.ID_INSTALACION AND vs.VALOR=SOL.VALOR LEFT OUTER JOIN " +
                "   (" +
                "       SELECT gei.ID_TERCERO, gei.ID_INSTALACION, vsi.ID_VIGENCIA, vsi.VALOR AS VIGENCIA, COUNT(0) AS TOTAL, SUM(CASE WHEN gei.TIPO_GUARDADO = 0 THEN 1 ELSE 0 END) AS BORRADOR, SUM(CASE WHEN gei.TIPO_GUARDADO <> 0 THEN 1 ELSE 0 END) AS ENVIADAS " +
                "       FROM CONTROL.FRM_GENERICO_ESTADO gei INNER JOIN " +
                "           CONTROL.VIGENCIA_SOLUCION vsi ON gei.ID_ESTADO = vsi.ID_ESTADO " +
                "       WHERE gei.ID_TERCERO = " + idTercero.ToString() + " AND ACTIVO = '0' " +
                "       GROUP BY gei.ID_TERCERO, gei.ID_INSTALACION, vsi.ID_VIGENCIA, vsi.VALOR" +
                "   ) tv ON ge.ID_TERCERO = tv.ID_TERCERO AND ge.ID_INSTALACION = tv.ID_INSTALACION AND vs.ID_VIGENCIA = tv.ID_VIGENCIA AND vs.VALOR = tv.VIGENCIA";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarConsultarEncuestaAdmin()
        {

            String sql = "SELECT distinct ge.ID_INSTALACION,t.ID_TERCERO,ge.TIPO_GUARDADO,v.ID_VIGENCIA,ge.ID_ESTADO,v.NOMBRE||'-'||vs.VALOR  encuesta,case vs.VALOR when '1' then 'Inicio' when '2' then 'Fin' else vs.VALOR end  vigencia,CASE v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'periodo' end tipovigencia,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 0 then 'Guardado Temporal'end estadoencuesta,ge.COD_USURARIO,t.S_RSOCIAL empresa,i.S_NOMBRE instalacion FROM control.ENC_ENCUESTA e INNER JOIN control.FORMULARIO_ENCUESTA fe ON fe.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN ENCUESTA_VIGENCIA ev ON ev.ID_ENCUESTA=e.ID_ENCUESTA INNER JOIN control.VIGENCIA v ON v.ID_VIGENCIA=ev.ID_VIGENCIA INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO       =ge.ID_ESTADO inner join GENERAL.TERCERO t on t.ID_TERCERO=ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION=ge.ID_INSTALACION where fe.ID_FORMULARIO=14 AND GE.TIPO_GUARDADO=1";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        /*public ActionResult consultarEncuestaCardinalidad(String valor,String idinstalacion)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            }
            String sql = "select distinct ge.ID_ESTADO,ge.NOMBRE,ge.ID_TERCERO,ge.ID_INSTALACION,ge.ID_VIGENCIA,ge.TIPO_GUARDADO,case nvl(S.Id_Solucion,0) when 0 then 'No diligenciada' else 'Diligenciada'end estado from control.FRM_GENERICO_ESTADO ge left join Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO where ge.Activo=0 and ge.NOMBRE is not null and ge.ID_TERCERO=" + idTerceroUsuario + " and ge.valor=" + valor + " and ge.ID_INSTALACION=" + idinstalacion + " order by ge.ID_ESTADO";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }*/

        /*public ActionResult consultarEncuestaCardinalidad(String valor, String idinstalacion, String vig)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            }
            //String sql = "select distinct ge.ID_ESTADO,ge.NOMBRE,ge.ID_TERCERO,ge.ID_INSTALACION,ge.ID_VIGENCIA,ge.TIPO_GUARDADO,case nvl(S.Id_Solucion,0) when 0 then 'No diligenciada' else 'Diligenciada'end estado from control.FRM_GENERICO_ESTADO ge left join Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_ESTADO=ge.ID_ESTADO where ge.Activo=0 and ge.NOMBRE is not null and ge.ID_TERCERO=" + idTerceroUsuario + " and ge.valor=" + valor + " and ge.ID_INSTALACION=" + idinstalacion + "  and vs.ID_VIGENCIA=" + vig + " order by ge.ID_ESTADO";
            String sql = "select distinct ge.ID_ESTADO,ge.NOMBRE,ge.ID_TERCERO,ge.ID_INSTALACION,ge.ID_VIGENCIA,ge.TIPO_GUARDADO,case nvl(S.Id_Solucion,0) when 0 then 'No diligenciada' else 'Diligenciada'end estado, TO_CHAR(S.D_EDICION,'DD/MM/YYYY hh:mi:ss') D_EDICION from control.FRM_GENERICO_ESTADO ge left join Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_ESTADO=ge.ID_ESTADO where ge.Activo=0 and ge.NOMBRE is not null and ge.ID_TERCERO=" + idTerceroUsuario + " and ge.valor=" + valor + " and ge.ID_INSTALACION=" + idinstalacion + "  and vs.ID_VIGENCIA=" + vig + " order by ge.ID_ESTADO";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }*/

        public ActionResult consultarEncuestaCardinalidad(String valor, String idinstalacion, String vig, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            /*if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) ! null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            }*/

            //String sql = "select distinct ge.ID_ESTADO,ge.NOMBRE,ge.S_CLAVE,ge.ID_TERCERO,ge.ID_INSTALACION,ge.ID_VIGENCIA,ge.TIPO_GUARDADO,case nvl(S.Id_Solucion,0) when 0 then 'No diligenciada' else 'Diligenciada'end estado, TO_CHAR(ff.D_EDICION,'DD/MM/YYYY hh:mi:ss') D_EDICION from control.FRM_GENERICO_ESTADO ge left join Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO  left join (select max(D_EDICION) D_EDICION , id_estado from control.Enc_Solucion group by id_estado) ff ON ff.Id_Estado=ge.ID_ESTADO  INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_ESTADO=ge.ID_ESTADO where ge.Activo=0 and ge.NOMBRE is not null and ge.ID_TERCERO=" + idTerceroUsuario + " and ge.valor=" + valor + " and ge.ID_INSTALACION=" + idinstalacion + "  and vs.ID_VIGENCIA=" + vig + " order by ge.NOMBRE";
            String sql = "select distinct ge.ID_ESTADO,ge.NOMBRE,ge.S_CLAVE,ge.ID_TERCERO,ge.ID_INSTALACION,ge.ID_VIGENCIA,ge.TIPO_GUARDADO,case ge.TIPO_GUARDADO when 1 then 'Enviada' when 2 then 'Diligenciada' else case nvl(S.Id_Solucion,0) when 0 then 'No Diligenciada' else 'Borrador' end end estado, TO_CHAR(ff.D_EDICION,'DD/MM/YYYY hh:mi:ss') D_EDICION from control.FRM_GENERICO_ESTADO ge left join Enc_Solucion s on S.Id_Estado=ge.ID_ESTADO  left join (select max(D_EDICION) D_EDICION , id_estado from control.Enc_Solucion group by id_estado) ff ON ff.Id_Estado=ge.ID_ESTADO  INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_ESTADO=ge.ID_ESTADO where ge.Activo=0 and ge.NOMBRE is not null and ge.ID_TERCERO=" + idTercero.ToString() + " and ge.valor=" + valor + " and ge.ID_INSTALACION=" + idinstalacion + "  and vs.ID_VIGENCIA=" + vig + " order by ge.NOMBRE";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            try
            {
                dbControl.SP_GET_DATOS(sql, jSONOUT);
            }
            catch
            {
                return null;
            }
            return Json(jSONOUT.Value);
        }

        public ActionResult HabilitarEstado(int id)
        {
            var estado = db.FRM_GENERICO_ESTADO.Where(ge => ge.ID_ESTADO == id).FirstOrDefault();

            if (estado == null)
                return Json(new { respuesta = "ERROR:Encuesta Inválida" }, JsonRequestBehavior.AllowGet);
            else
            {
                try
                {
                    estado.TIPO_GUARDADO = 0;
                    db.Entry(estado).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { respuesta = "OK" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception error)
                {
                    SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "EncuestaExterna [HabilitarEstado - " + id.ToString() + " ] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
                    return Json(new { respuesta = "ERROR:Error Realizando la Operación." }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult HabilitarEncuesta(int idInstalacion, int idVigencia, string valor)
        {
            var sql =
                "UPDATE CONTROL.FRM_GENERICO_ESTADO " +
                "SET TIPO_GUARDADO = 0 " +
                "WHERE ID_ESTADO IN ( " +
                "    SELECT ge.ID_ESTADO " +
                "    FROM CONTROL.FRM_GENERICO_ESTADO ge INNER JOIN " +
                "        CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO " +
                "    WHERE ge.ID_INSTALACION = " + idInstalacion.ToString() + " AND vs.ID_VIGENCIA = " + idVigencia.ToString() + " AND vs.VALOR = '" + valor + "' AND ACTIVO = 0 " +
                ")";

            var resultado = db.Database.ExecuteSqlCommand(sql);

            return Json(new { respuesta = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult reportEncuesta(int idestado, int idRadicado)
        {
            byte[] memStream = ReporteEncuesta(idestado);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "encuestaSitio.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(memStream, "application/pdf");
        }

        private byte[] ReporteEncuesta(int idestado)
        {
            var url_rencabezado = (string)webConfigReader.GetValue("url_rps_encabesado", typeof(string));
            var url_piepagina = (string)webConfigReader.GetValue("url_rps_piepagina", typeof(string));

            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            DateTime localDate = DateTime.Now;
            string newstrDate = localDate.ToString("M/d/yyyy");
            String sql3 = "select * from control.VWR_ENCUESTA_SITIO where id_estado=" + idestado;
            ObjectParameter jSONOUT3 = new ObjectParameter("jSONOUT", typeof(string));
            ObjectParameter jSONOUT5 = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql3, jSONOUT3);

            var emp = JsonConvert.DeserializeObject<List<empresa>>(jSONOUT3.Value.ToString());
            String sqlencuesta = "select * from control.VWR_ENCUESTASUSUARIOEXTERNO where id_estado=" + idestado;
            ObjectParameter jSONOUT4 = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlencuesta, jSONOUT4);
            var encJson = JsonConvert.DeserializeObject<List<encuesta>>(jSONOUT4.Value.ToString());

            String sqlPregunta = "select * from control.VWR_ENCUESTAS_PREGUNTAS where id_estado=" + idestado + " order by N_ORDEN";
            ObjectParameter jSONP = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlPregunta, jSONP);
            var pregJson = JsonConvert.DeserializeObject<List<pregunta>>(jSONP.Value.ToString());
            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            doc.Open();
            Image imgEncabezado = Image.GetInstance(new Uri(url_rencabezado));
            imgEncabezado.ScalePercent(90f);

            doc.Add(imgEncabezado);
            Image imgPiePagina = Image.GetInstance(new Uri(url_piepagina));
            imgPiePagina.ScalePercent(25f);
            imgPiePagina.SetAbsolutePosition(0f, 0f);
            doc.Add(imgPiePagina);

            //iTextSharp.text.Image bannerImage = iTextSharp.text.Image.GetInstance("~/Content/imagenes/logoTala.png");

            var parr = new Paragraph(newstrDate.ToString());
            parr.Alignment = Element.ALIGN_CENTER;
            doc.Add(parr);
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEmpresa = new PdfPTable(1);
            tblEmpresa.WidthPercentage = 100;
            String vigencia = "";
            String tercero = "";
            String instalacion = "";
            if (emp.Count > 0)
            {
                vigencia = emp[0].VIGENCIA;
                tercero = emp[0].TERCERO;
                instalacion = emp[0].S_NOMBRE;
            }

            // Configuramos el título de las columnas de la tabla
            PdfPCell clVigencia = new PdfPCell(new Phrase("VIGENCIA:       " + vigencia));
            clVigencia.BorderWidth = 0;


            tblEmpresa.AddCell(clVigencia);
            PdfPCell clTercero = new PdfPCell(new Phrase("TERCERO:       " + tercero));
            clTercero.BorderWidth = 0;
            tblEmpresa.AddCell(clTercero);
            PdfPCell clInstalacion = new PdfPCell(new Phrase("INSTALACIÓN: " + instalacion));
            clInstalacion.BorderWidth = 0;
            tblEmpresa.AddCell(clInstalacion);

            doc.Add(tblEmpresa);
            var parr2 = new Paragraph("Encuesta");
            doc.Add(parr2);

            for (int i = 0; i < encJson.Count; i++)
            {
                var parrafo = new Paragraph(encJson[i].NOMBRE, boldFont);
                doc.Add(parrafo);
                var saltolinea = new Paragraph("       ");
                doc.Add(saltolinea);
                PdfPTable tblPregunta = new PdfPTable(3);
                tblPregunta.WidthPercentage = 100;
                PdfPCell clpregunta = new PdfPCell(new Phrase("PREGUNTA"));
                PdfPCell clrespuesta = new PdfPCell(new Phrase("RESPUESTA"));
                PdfPCell clobservacion = new PdfPCell(new Phrase("OBSERVACION"));
                clpregunta.BackgroundColor = BaseColor.LIGHT_GRAY;
                clrespuesta.BackgroundColor = BaseColor.LIGHT_GRAY;
                clobservacion.BackgroundColor = BaseColor.LIGHT_GRAY;
                tblPregunta.AddCell(clpregunta);
                tblPregunta.AddCell(clrespuesta);
                tblPregunta.AddCell(clobservacion);
                for (int p = 0; p < pregJson.Count; p++)
                {
                    if (pregJson[p].ID_ENCUESTA == encJson[i].ID_ENCUESTA)
                    {
                        PdfPCell clpreguntar;

                        if (pregJson[p].ID_TIPOPREGUNTA == "11")
                            clpreguntar = new PdfPCell(new Phrase(pregJson[p].PREGUNTA, boldFont));
                        else
                            clpreguntar = new PdfPCell(new Phrase(pregJson[p].PREGUNTA));
                        PdfPCell clrespuestar = new PdfPCell(new Phrase(pregJson[p].RESPUESTA));
                        PdfPCell clobservacionr = new PdfPCell(new Phrase(pregJson[p].OBSERVACION));
                        tblPregunta.AddCell(clpreguntar);
                        tblPregunta.AddCell(clrespuestar);
                        tblPregunta.AddCell(clobservacionr);
                    }

                }
                doc.Add(tblPregunta);

            }
            doc.Close();

            /*var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "encuestaSitio.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());*/

            return memStream.GetBuffer();
        }

        private void DrawImage2(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }
        public ActionResult consultarLimiteFecha()
        {
            String sql = " SELECT VALOR from general.PARAMETROS where ID_PARAMETRO in(89,90)";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
     
        public ActionResult guardarArchivoEncuesta(int id, int idEE)
        {
            string path;
            string url;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            }

            HttpPostedFileBase file = Request.Files[0];
            int fileSize = file.ContentLength;
            string fileName = (idEE == -1 ? "" : idEE.ToString() + "_" + id.ToString() + "_") + file.FileName;
            string mimeType = file.ContentType;

            if (idEE != -1)
            {
                var datosEstado = (from vs in db.VIGENCIA_SOLUCION
                                  join ge in db.FRM_GENERICO_ESTADO on vs.ID_ESTADO equals ge.ID_ESTADO
                                  where ge.ID_ESTADO == idEE
                                  select new
                                  {
                                      ID_TERCERO = ge.ID_TERCERO,
                                      ID_VIGENCIA = vs.ID_VIGENCIA,
                                      VALOR = vs.VALOR
                                  }).FirstOrDefault();


                path = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string)) + "\\ArchivosEncuestas\\" + datosEstado.ID_VIGENCIA.ToString() + "\\" + datosEstado.VALOR + "\\" + datosEstado.ID_TERCERO.ToString();
                url = (string)webConfigReader.GetValue("url_doc_form", typeof(string)) + "/Tercero/" + idTerceroUsuario + "/Pregunta/" + id + "/" + fileName;
            }
            else
            {
                path = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string)) + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + id;
                url = (string)webConfigReader.GetValue("url_doc_form", typeof(string)) + "/Tercero/" + idTerceroUsuario + "/Pregunta/" + id + "/" + fileName;
            }

            //Directory.Delete(path, true);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (string upload in Request.Files)
            {
                Request.Files[upload].SaveAs(Path.Combine(path, fileName));
            }
            doc docum= new doc();
            docum.idPregunta = id.ToString();
            docum.nombreDoc = file.FileName; // fileName;
            docum.ruta = file.FileName; // fileName;
            return Json(docum);
        }
        private void DrawImage(XGraphics gfx, Stream imageEtiqueta, int x, int y, int width, int height)
        {
            XImage image = XImage.FromStream(imageEtiqueta);

            gfx.DrawImage(image, new System.Drawing.Point(x, y));
        }
       
        public ActionResult consultarRol()
        {
            String sql = "SELECT ID_ROL ID, S_NOMBRE NOMBRE FROM seguridad.ROL order by S_NOMBRE";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarIntalacionTercero(int idTercero)
        {
            String sql = "SELECT ID_INSTALACION ID,S_NOMBRE NOMBRE FROM control.VW_INSTALACION where ID_TERCERO=" + idTercero;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarTerceroUsuario()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
            }
            var resultData = new { id = idTerceroUsuario };
            return Json(resultData);
        }
        public ActionResult geocodificadorArea(String valor)
        {
            /*ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            coordenadas coorP = new coordenadas();
            coordenadas coorP2 = new coordenadas();
            var client = new System.Net.WebClient();
            
            var dato = client.DownloadString("https://www.medellin.gov.co/MapGIS/Geocod/AjaxGeocod?accion=2&valor=" + valor + "&f=json");
            if (dato != "0")
            {
                coorP = JsonConvert.DeserializeObject<coordenadas>(dato);
                String xp = coorP.x;
                String yp = coorP.y;
                var dato2 = client.DownloadString("https://www.medellin.gov.co/mapas/rest/services/Utilities/Geometry/GeometryServer/project?inSR=6257&outSR=4326&geometries=" + xp + "," + yp + "&transformation=15738&transformForward=true&f=json");
                JObject o = JObject.Parse(dato2);
                Decimal x = (Decimal)o.SelectToken("geometries[0].x");
                Decimal y = (Decimal)o.SelectToken("geometries[0].y");
                var coordenadas = new { cx = x, cy = y };
                return Json(coordenadas);
            }
            else
            {
                var coordenadas = new { cx = 0, cy = 0 };
                return Json(coordenadas);
            }*/

            coordenadas resul = Utilidades.Geocoding.ObtenerCoordenadas(valor);

            var coordenadas = new { cx = Convert.ToDecimal(resul.x), cy = Convert.ToDecimal(resul.y) };
            return Json(coordenadas);
        }

        private List<Data> CreateData()
        {
            List<Data> data = new List<Data>();

            Data item1 = new Data();
           
            item1.Id = 0;
            item1.Name = "First";
            data.Add(item1);

            Data item2 = new Data();
          
            item2.Id = 1;
            item2.Name = "Second";
            data.Add(item2);

            Data item3 = new Data();
        
            item3.Id = 2;
            item3.Name = "Third";
            data.Add(item3);

            return data;
        }
        public ActionResult reportEncuestaCard(int idinstalacion,int valor, int idRadicado)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

            }
            
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            DateTime localDate = DateTime.Now;
            string newstrDate = localDate.ToString("M/d/yyyy");
            String sql3 = "select  DISTINCT i.ID_INSTALACION,ge.valor, case v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'inicio_fin' end vigencia,i.S_NOMBRE, t.S_RSOCIAL tercero from control.VIGENCIA v inner join control.VIGENCIA_SOLUCION vs on vs.ID_VIGENCIA = v.ID_VIGENCIA inner join control.FRM_GENERICO_ESTADO ge on ge.ID_ESTADO = vs.ID_ESTADO inner join general.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION where i.ID_INSTALACION= " + idinstalacion + " and ge.valor=" + valor;
            ObjectParameter jSONOUT3 = new ObjectParameter("jSONOUT", typeof(string));
            
            dbControl.SP_GET_DATOS(sql3, jSONOUT3);
          
            var emp = JsonConvert.DeserializeObject<List<empresa>>(jSONOUT3.Value.ToString());
            String sql4 = "select distinct Ge.Id_Estado, Ge.Nombre, Ge.Valor, Ge.Id_Instalacion from control.FRM_GENERICO_ESTADO ge where Ge.Nombre is not null and Ge.Valor=" + valor + " and Ge.Id_Instalacion=" + idinstalacion + " order by Ge.Id_Estado";
            ObjectParameter jSONOUT4 = new ObjectParameter("jSONOUT", typeof(string));
            
            dbControl.SP_GET_DATOS(sql4, jSONOUT4);
          
            var trabjson = JsonConvert.DeserializeObject<List<trabajador>>(jSONOUT4.Value.ToString());

            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
            doc.Open();
            /*
            Image imgEncabezado = Image.GetInstance(new Uri("//localhost/SIM/Content/imagenes/logoTala.png"));
            imgEncabezado.ScalePercent(90f);
            doc.Add(imgEncabezado);

            Image imgPiePagina = Image.GetInstance(new Uri("//localhost/SIM/Content/imagenes/pata.jpg"));
            imgPiePagina.ScalePercent(25f);
            imgPiePagina.SetAbsolutePosition(0f, 0f);
            doc.Add(imgPiePagina);
            */

            //iTextSharp.text.Image bannerImage = iTextSharp.text.Image.GetInstance("~/Content/imagenes/logoTala.png");

            var parr = new Paragraph(newstrDate.ToString());
            parr.Alignment = Element.ALIGN_CENTER;
            doc.Add(parr);
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEmpresa = new PdfPTable(1);
            tblEmpresa.WidthPercentage = 100;
            String vigencia = "";
            String tercero = "";
            String instalacion = "";
            if (emp.Count > 0)
            {
                vigencia = emp[0].VIGENCIA;
                tercero = emp[0].TERCERO;
                instalacion = emp[0].S_NOMBRE;
            }

            // Configuramos el título de las columnas de la tabla
            PdfPCell clVigencia = new PdfPCell(new Phrase("VIGENCIA:       " + vigencia));
            clVigencia.BorderWidth = 0;


            tblEmpresa.AddCell(clVigencia);
            PdfPCell clTercero = new PdfPCell(new Phrase("TERCERO:       " + tercero));
            clTercero.BorderWidth = 0;
            tblEmpresa.AddCell(clTercero);
            PdfPCell clInstalacion = new PdfPCell(new Phrase("INSTALACIÓN: " + instalacion));
            clInstalacion.BorderWidth = 0;
            tblEmpresa.AddCell(clInstalacion);

            doc.Add(tblEmpresa);
            var saltolinea = new Paragraph("       ");
            doc.Add(saltolinea);
            for (int i = 0; i < trabjson.Count;i++ )
            {
                var parra = new Paragraph(trabjson[i].NOMBRE);

                doc.Add(parra);
              
            }
                doc.Close();
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = "encuestaTrabajadores.pdf",
                    Inline = true,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(memStream.GetBuffer(), "application/pdf");

           // return File(memStream.GetBuffer(), "application/pdf", "encuestaTrabajadores.pdf");
        }

        public ActionResult reportEncuestaCardRadicado(int idinstalacion, string valor, int idRadicado, int ID_ESTADO)
        {
            return reportEncuestaCardRadicadoTercero(idinstalacion, valor, idRadicado, ID_ESTADO, null, null);
        }

        public ActionResult reportEncuestaCardRadicadoTercero(int idinstalacion, string valor, int idRadicado, int ID_ESTADO, int? idUsuarioGestion, int? idTercero)
        {
            return reportEncuestaCardRadicadoTercero(idinstalacion, valor, idRadicado, ID_ESTADO, idUsuarioGestion, idTercero, null);
        }

        public ActionResult reportEncuestaCardRadicadoTercero(int idinstalacion, string valor, int idRadicado, int ID_ESTADO, int? idUsuarioGestion, int? idTercero, tramite tramite, bool registrarDocumento = true)
        {
            //var sql = "SELECT distinct v.NOMBRE||'-'||vs.VALOR encuesta FROM control.VIGENCIA v INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE ge.ID_ESTADO=" + ID_ESTADO.ToString();
            var sql = "SELECT distinct v.NOMBRE||'-'||vs.VALOR AS ENCUESTA_TITULO, v.ID_PREGUNTA_DESC FROM control.VIGENCIA v INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE ge.ID_ESTADO=" + ID_ESTADO.ToString();

            var datosEncuesta = db.Database.SqlQuery<DatosBaseEncuesta>(sql).FirstOrDefault();

            string tituloEncuesta = "";

            if (datosEncuesta != null)
            {
                tituloEncuesta = datosEncuesta.ENCUESTA_TITULO;
            }

            tramite tramitePMES;

            if (tramite == null)
                tramitePMES = guardarTramite(tituloEncuesta, ID_ESTADO); // guardarTramite("tramite PMES");
            else
                tramitePMES = tramite;

            String strTitulo = tituloEncuesta; // "Documento Plan MES";
            var url_rencabezado = (string)webConfigReader.GetValue("url_rps_encabesado", typeof(string));
            var url_piepagina = (string)webConfigReader.GetValue("url_rps_piepagina", typeof(string));

            if (idUsuarioGestion == null || idTercero == null)
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }
            }
            else
            {
                idTerceroUsuario = Convert.ToDecimal(idTercero);
                idUsuario = (int)idUsuarioGestion;
            }

            var usuario = (from Rdocumt in db.USUARIO
                           where (Rdocumt.ID_USUARIO == idUsuario)
                           select new { Rdocumt.S_NOMBRES, Rdocumt.S_APELLIDOS }).FirstOrDefault();

            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            DateTime localDate = DateTime.Now;
            string newstrDate = localDate.ToString("M/d/yyyy");
            //String sql3 = "select  DISTINCT i.ID_INSTALACION,ge.valor, case v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'inicio_fin' end vigencia,i.S_NOMBRE, t.S_RSOCIAL tercero from control.VIGENCIA v inner join control.VIGENCIA_SOLUCION vs on vs.ID_VIGENCIA = v.ID_VIGENCIA inner join control.FRM_GENERICO_ESTADO ge on ge.ID_ESTADO = vs.ID_ESTADO inner join general.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION where i.ID_INSTALACION= " + idinstalacion + " and vs.valor='" + valor + "'";
            String sql3 = "select  DISTINCT i.ID_INSTALACION,ge.valor, case v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'inicio_fin' end vigencia,i.S_NOMBRE, t.S_RSOCIAL tercero from control.VIGENCIA v inner join control.VIGENCIA_SOLUCION vs on vs.ID_VIGENCIA = v.ID_VIGENCIA inner join control.FRM_GENERICO_ESTADO ge on ge.ID_ESTADO = vs.ID_ESTADO inner join general.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION where i.ID_INSTALACION= " + idinstalacion + " and vs.valor='" + valor + "' AND ge.ID_TERCERO = " + idTerceroUsuario.ToString();
            ObjectParameter jSONOUT3 = new ObjectParameter("jSONOUT", typeof(string));

            dbControl.SP_GET_DATOS(sql3, jSONOUT3);

            var emp = JsonConvert.DeserializeObject<List<empresa>>(jSONOUT3.Value.ToString());
            //String sql4 = "select DIRECCION,TELEFONO_INSTALACION,MUNICIPIO,EMAIL_ORGANIZACION  from general.QRYINSTALACION where ID_INSTALACION= " + idinstalacion;
            //String sql4 = "SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS DIRECCION, i.S_TELEFONO AS TELEFONO_INSTALACION, d.S_NOMBRE AS MUNICIPIO, t.S_CORREO AS EMAIL_ORGANIZACION, i.S_NOMBRE AS NOMBRE_INSTALACION, t.S_RSOCIAL AS RAZON_SOCIAL FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION= " + idinstalacion;
            String sql4 = "SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS DIRECCION, i.S_TELEFONO AS TELEFONO_INSTALACION, d.S_NOMBRE AS MUNICIPIO, t.S_CORREO AS EMAIL_ORGANIZACION, i.S_NOMBRE AS NOMBRE_INSTALACION, t.S_RSOCIAL AS RAZON_SOCIAL FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION= " + idinstalacion + " AND ti.ID_TERCERO = " + idTerceroUsuario.ToString();
            ObjectParameter jSONOUT4 = new ObjectParameter("jSONOUT", typeof(string));

            dbControl.SP_GET_DATOS(sql4, jSONOUT4);

            var inst = JsonConvert.DeserializeObject<List<instalacion>>(jSONOUT4.Value.ToString());

            List<doc> observ = null;

            if (datosEncuesta != null && datosEncuesta.ID_PREGUNTA_DESC != null && datosEncuesta.ID_PREGUNTA_DESC > 0)
            {
                //String sqlobser = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.Id_Pregunta=645 and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO;
                String sqlobser = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.Id_Pregunta = " + datosEncuesta.ID_PREGUNTA_DESC.ToString() + " and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO;
                ObjectParameter jSONobser = new ObjectParameter("jSONOUT", typeof(string));
                dbControl.SP_GET_DATOS(sqlobser, jSONobser);
                observ = JsonConvert.DeserializeObject<List<doc>>(jSONobser.Value.ToString());
            }

            var radicadodocumento = (from Rdocumt in db.RADICADO_DOCUMENTO
                                     where (Rdocumt.ID_RADICADODOC == idRadicado)
                                     select new { Rdocumt.D_RADICADO, Rdocumt.S_RADICADO }).FirstOrDefault();
            Cryptografia crypt = new Cryptografia();
            MemoryStream ms = new MemoryStream();

            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("C://Temp/plantilla.pdf", FileMode.Create));
            PdfWriter writer2 = PdfWriter.GetInstance(doc, memStream);
            int pageNumber = writer.PageNumber - 1;
            doc.Open();

            /*Image imgEncabezado = Image.GetInstance(new Uri(url_rencabezado));
            imgEncabezado.ScalePercent(90f);

            doc.Add(imgEncabezado);*/

            var parr = new Paragraph(newstrDate.ToString());
            parr.Alignment = Element.ALIGN_LEFT;
            doc.Add(parr);
            doc.Add(Chunk.NEWLINE);


            //Radicado01Report etiqueta = new Radicado01Report();
            //MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(idRadicado, "png");

            MemoryStream imagenEtiqueta = new MemoryStream();

            Radicador radicador = new Radicador();
            var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
            imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);


            //System.Drawing.Image sdi = System.Drawing.Image.FromStream(imagenEtiqueta);
            System.Drawing.Image sdi = System.Drawing.Image.FromStream(imagenEtiqueta);

            Image imgRadicado = Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Bmp);
            //Image imgRadicado = Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Png);
            imgRadicado.ScalePercent(50f);
            imgRadicado.Alignment = Element.ALIGN_RIGHT;
            imgRadicado.SetAbsolutePosition(Convert.ToSingle(293), Convert.ToSingle(690));

            doc.Add(imgRadicado);


            /*Image imgPiePagina = Image.GetInstance(new Uri(url_piepagina));
            imgPiePagina.ScalePercent(25f);
            imgPiePagina.SetAbsolutePosition(0f, 0f);
            doc.Add(imgPiePagina);*/



            //var Titulo = new Paragraph(strTitulo);
            //Titulo.Alignment = Element.ALIGN_CENTER;
            //var titleFont = FontFactory.GetFont("Courier", 20, BaseColor.BLACK);
            //Titulo.Font = titleFont;
            //doc.Add(Titulo);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            var enc1 = new Paragraph("Señores");
            doc.Add(enc1);
            var enc2 = new Paragraph("AREA METROPOLITANA DEL VALLE DE ABURRA");
            doc.Add(enc2);
            var enc3 = new Paragraph("ATENCIÓN AL USUARIO");
            doc.Add(enc3);
            var enc4 = new Paragraph("Medellín, Antioquia");
            doc.Add(enc4);
            var saltolinea1 = new Paragraph("       ");
            doc.Add(saltolinea1);
            String cuerpo = "";
            try
            {
                if (observ != null)
                    cuerpo = observ[0].nombreDoc;
            }
            catch (Exception e)
            {
            }
            //var asunto = new Paragraph("Asunto " + strTitulo + " Vigencia " + valor);
            var asunto = new Paragraph("Asunto " + strTitulo);
            doc.Add(asunto);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            var obs = new Paragraph(cuerpo);
            doc.Add(obs);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            var atentamente = new Paragraph("Atentamente,");
            doc.Add(atentamente);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEmpresa = new PdfPTable(1);
            tblEmpresa.WidthPercentage = 100;
            String vigencia = "";
            String tercero = "";
            String instalacion = "";
            if (emp.Count > 0)
            {
                vigencia = emp[0].VIGENCIA;
                tercero = emp[0].TERCERO;
                instalacion = emp[0].S_NOMBRE;
            }

            String razonSocila = "";
            try
            {
                razonSocila = inst[0].RAZON_SOCIAL;
            }
            catch (Exception e)
            {
            }
            String nombreInstalacion = "";
            try
            {
                nombreInstalacion = inst[0].NOMBRE_INSTALACION;
            }
            catch (Exception e)
            {
            }
            String municipio = "";
            try
            {
                municipio = inst[0].MUNICIPIO;
            }
            catch (Exception e)
            {
            }
            String direcion = "";
            try
            {
                direcion = inst[0].DIRECCION;
            }
            catch (Exception e)
            {
            }
            String email = "";
            try
            {
                email = inst[0].EMAIL_ORGANIZACION;
            }
            catch (Exception e)
            {
            }
            String telefono = "";
            try
            {
                telefono = inst[0].TELEFONO_INSTALACION;
            }
            catch (Exception e)
            {
            }


            PdfPCell clTercero = new PdfPCell(new Phrase(tercero));
            clTercero.BorderWidth = 0;
            tblEmpresa.AddCell(clTercero);
            PdfPCell clInstalacion = new PdfPCell(new Phrase("INSTALACIÓN: " + instalacion));
            clInstalacion.BorderWidth = 0;
            tblEmpresa.AddCell(clInstalacion);
            PdfPCell clmunicipio = new PdfPCell(new Phrase("MUNICIPIO: " + municipio));
            clmunicipio.BorderWidth = 0;
            tblEmpresa.AddCell(clmunicipio);
            PdfPCell cldirecion = new PdfPCell(new Phrase("DIRECCIÓN: " + direcion));
            cldirecion.BorderWidth = 0;
            tblEmpresa.AddCell(cldirecion);
            PdfPCell clcorreo = new PdfPCell(new Phrase("CORREO ELECTRÓNICO: " + email));
            clcorreo.BorderWidth = 0;
            tblEmpresa.AddCell(clcorreo);
            PdfPCell clTELEFONO = new PdfPCell(new Phrase("TELÉFONO: " + telefono));
            clTELEFONO.BorderWidth = 0;
            tblEmpresa.AddCell(clTELEFONO);


            doc.Add(tblEmpresa);



            doc.Close();
            documento docrtp = new documento();


            var cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = "encuestaTrabajadores.pdf",
                FileName = "encuesta.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            byte[] content = memStream.ToArray();
            PdfSharp.Pdf.PdfDocument docCOD = new PdfSharp.Pdf.PdfDocument();
            docCOD = PdfSharp.Pdf.IO.PdfReader.Open(@"C://Temp/plantilla.pdf", PdfDocumentOpenMode.Import);

            int countPlant = docCOD.PageCount;
            int numPaginas = 0;
            String sqlDoc = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta, NVL(ep.N_VALOR, 0) AS tipoRuta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.ID_TIPOPREGUNTA=8 and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO + " ORDER BY P.Id_Pregunta";
            ObjectParameter jSONDoc = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlDoc, jSONDoc);
            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();
            for (int idx = 0; idx < countPlant; idx++)
            {
                PdfSharp.Pdf.PdfPage page = docCOD.Pages[idx];
                outputDocument.AddPage(page);
                numPaginas++;

            }
            var anexo = JsonConvert.DeserializeObject<List<doc>>(jSONDoc.Value.ToString());
            string path = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string));

            var datosEstado = (from vs in db.VIGENCIA_SOLUCION
                               join v in db.VIGENCIA on vs.ID_VIGENCIA equals v.ID_VIGENCIA
                               join ge in db.FRM_GENERICO_ESTADO on vs.ID_ESTADO equals ge.ID_ESTADO
                               where ge.ID_ESTADO == ID_ESTADO
                               select new
                               {
                                   ID_TERCERO = ge.ID_TERCERO,
                                   ID_VIGENCIA = vs.ID_VIGENCIA,
                                   VALOR = vs.VALOR,
                                   ID_VIGENCIA_RELACIOANDA_DOC = v.ID_VIGENCIA_RELACIOANDA_DOC ?? 0
                               }).FirstOrDefault();

            if (datosEstado.ID_VIGENCIA_RELACIOANDA_DOC > 0)
            {
                var estadosEncuestaRelacionadas = from vs in db.VIGENCIA_SOLUCION
                                             join ge in db.FRM_GENERICO_ESTADO on vs.ID_ESTADO equals ge.ID_ESTADO
                                             where vs.ID_VIGENCIA == datosEstado.ID_VIGENCIA_RELACIOANDA_DOC && ge.ID_TERCERO == datosEstado.ID_TERCERO && vs.VALOR == datosEstado.VALOR
                                             select ge.ID_ESTADO;

                foreach (var estadoEncuesta in estadosEncuestaRelacionadas)
                {
                    var reporteEncuestaRelacionada = ReporteEncuesta(Convert.ToInt32(estadoEncuesta));
                    var memStreamEncuestaRelacionada = new MemoryStream(reporteEncuestaRelacionada);

                    PdfSharp.Pdf.PdfDocument docAjt = new PdfSharp.Pdf.PdfDocument();
                    docAjt = PdfSharp.Pdf.IO.PdfReader.Open(memStreamEncuestaRelacionada, PdfDocumentOpenMode.Import);
                    int countDocAj = docAjt.PageCount;
                    for (int idx = 0; idx < countDocAj; idx++)
                    {
                        PdfSharp.Pdf.PdfPage page = docAjt.Pages[idx];
                        outputDocument.AddPage(page);
                        numPaginas++;
                    }
                }
            }

            foreach (var item in anexo)
            {
                PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();

                if (item.tipoRuta == 1 && System.IO.File.Exists(path + "\\ArchivosEncuestas\\" + datosEstado.ID_VIGENCIA.ToString() + "\\" + datosEstado.VALOR + "\\" + datosEstado.ID_TERCERO.ToString() + "\\" + ID_ESTADO.ToString() + "_" + item.idPregunta + "_" + item.nombreDoc))
                {
                    documentoDE.LoadDocument(path + "\\ArchivosEncuestas\\" + datosEstado.ID_VIGENCIA.ToString() + "\\" + datosEstado.VALOR + "\\" + datosEstado.ID_TERCERO.ToString() + "\\" + ID_ESTADO.ToString() + "_" + item.idPregunta + "_" + item.nombreDoc, true);
                }
                else
                {
                    if (System.IO.Path.GetExtension(item.nombreDoc).ToUpper().Trim() != ".PDF")
                        continue;
                    else
                        documentoDE.LoadDocument(path + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + item.idPregunta + "\\" + item.nombreDoc, true);
                }

                MemoryStream documentoDEPDF = new MemoryStream();
                documentoDE.SaveDocument(documentoDEPDF);
                documentoDE.CloseDocument();

                PdfSharp.Pdf.PdfDocument docAjt = new PdfSharp.Pdf.PdfDocument();
                //docAjt = PdfSharp.Pdf.IO.PdfReader.Open(path + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + item.idPregunta+"\\"+item.nombreDoc, PdfDocumentOpenMode.Import);
                docAjt = PdfSharp.Pdf.IO.PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);
                int countDocAj = docAjt.PageCount;
                for (int idx = 0; idx < countDocAj; idx++)
                {
                    PdfSharp.Pdf.PdfPage page = docAjt.Pages[idx];
                    outputDocument.AddPage(page);
                    numPaginas++;
                }
            }

            if (registrarDocumento)
            {
                docrtp = RegistrarDocumento(Int32.Parse(tramitePMES.codFuncionario), idRadicado, numPaginas, Int32.Parse(tramitePMES.codTramite), Int32.Parse(tramitePMES.codProceso), 10);
                String rut = docrtp.file.ToString().Substring(0, docrtp.file.ToString().Length - 1);
                System.IO.Directory.CreateDirectory(rut);
                dbControl.SP_SET_URLRADICADO(ID_ESTADO, docrtp.RUTA.ToString());
                outputDocument.Save(docrtp.RUTA.ToString());
                outputDocument.Save(ms);
                ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
                String strindice = Convert.ToDecimal(docrtp.IDDOC) + "-" + radicadodocumento.D_RADICADO.ToString() + "-" + radicadodocumento.S_RADICADO + "-" + Int32.Parse(tramitePMES.codTramite) + "-" + strTitulo + "-" + usuario.S_NOMBRES + " " + usuario.S_APELLIDOS + "-" + tercero + "-" + idTerceroUsuario + "-" + idinstalacion;
                try
                {
                    dbControl.SP_SET_INDICE_COR(Convert.ToDecimal(docrtp.IDDOC), radicadodocumento.D_RADICADO.ToString(), radicadodocumento.S_RADICADO, Int32.Parse(tramitePMES.codTramite), strTitulo, usuario.S_NOMBRES + " " + usuario.S_APELLIDOS, tercero, idTerceroUsuario, idinstalacion, rta);

                }
                catch (Exception e)
                {

                }

                crypt.Encriptar(ms, docrtp.RUTA.ToString(), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
            }
            else
            {
                outputDocument.Save(@"C:\Temp\ArchivoEncuestas\" + idRadicado.ToString() + ".pdf");
                outputDocument.Save(ms);
                crypt.Encriptar(ms, @"C:\Temp\ArchivoEncuestas\" + idRadicado.ToString() + "_CRYPT.pdf", UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
                ms = new MemoryStream();
                outputDocument.Save(ms);
            }

            return File(ms.GetBuffer(), "application/pdf");
        }

        public ActionResult reportEncuestaCardRadicadoTerceroTest(int idEstado, int encriptado, int registrarDocumento, int crearTramite, int x, int y)
        {
            int idinstalacion;
            string valor;
            int idRadicado;

            //var sql = "SELECT distinct v.NOMBRE||'-'||vs.VALOR encuesta FROM control.VIGENCIA v INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE ge.ID_ESTADO=" + ID_ESTADO.ToString();
            var sql = "SELECT distinct v.NOMBRE||'-'||vs.VALOR AS ENCUESTA_TITULO, v.ID_PREGUNTA_DESC FROM control.VIGENCIA v INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE ge.ID_ESTADO=" + idEstado.ToString();

            var datosEncuesta = db.Database.SqlQuery<DatosBaseEncuesta>(sql).FirstOrDefault();

            idinstalacion = db.Database.SqlQuery<int>("SELECT ID_INSTALACION FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();
            idRadicado = db.Database.SqlQuery<int>("SELECT CODRADICADO FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();
            valor = db.Database.SqlQuery<string>("SELECT vs.VALOR FROM CONTROL.FRM_GENERICO_ESTADO ge INNER JOIN CONTROL.VIGENCIA_SOLUCION vs ON ge.ID_ESTADO = vs.ID_ESTADO WHERE ge.ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();

            string tituloEncuesta = "";

            if (datosEncuesta != null)
            {
                tituloEncuesta = datosEncuesta.ENCUESTA_TITULO;
            }

            String strTitulo;
            if (crearTramite == 1)
            {
                tramite tramitePMES = guardarTramite(tituloEncuesta, idEstado); // guardarTramite("tramite PMES");
            }

            strTitulo = tituloEncuesta; // "Documento Plan MES";

            var url_rencabezado = (string)webConfigReader.GetValue("url_rps_encabesado", typeof(string));
            var url_piepagina = (string)webConfigReader.GetValue("url_rps_piepagina", typeof(string));

            /*if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }*/

            idTerceroUsuario = db.Database.SqlQuery<int>("SELECT ID_TERCERO FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();
            idUsuario = Convert.ToInt32(db.Database.SqlQuery<string>("SELECT COD_USURARIO FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault());

            var usuario = (from Rdocumt in db.USUARIO
                           where (Rdocumt.ID_USUARIO == idUsuario)
                           select new { Rdocumt.S_NOMBRES, Rdocumt.S_APELLIDOS }).FirstOrDefault();

            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            DateTime localDate = DateTime.Now;
            string newstrDate = localDate.ToString("M/d/yyyy");
            //String sql3 = "select  DISTINCT i.ID_INSTALACION,ge.valor, case v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'inicio_fin' end vigencia,i.S_NOMBRE, t.S_RSOCIAL tercero from control.VIGENCIA v inner join control.VIGENCIA_SOLUCION vs on vs.ID_VIGENCIA = v.ID_VIGENCIA inner join control.FRM_GENERICO_ESTADO ge on ge.ID_ESTADO = vs.ID_ESTADO inner join general.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION where i.ID_INSTALACION= " + idinstalacion + " and vs.valor='" + valor + "'";
            String sql3 = "select  DISTINCT i.ID_INSTALACION,ge.valor, case v.TIPOVIGENCIA when 1 then 'Anual' when 2 then 'Mensual' when 3 then 'Unica' when 4 then 'inicio_fin' end vigencia,i.S_NOMBRE, t.S_RSOCIAL tercero from control.VIGENCIA v inner join control.VIGENCIA_SOLUCION vs on vs.ID_VIGENCIA = v.ID_VIGENCIA inner join control.FRM_GENERICO_ESTADO ge on ge.ID_ESTADO = vs.ID_ESTADO inner join general.TERCERO t on t.ID_TERCERO = ge.ID_TERCERO inner join GENERAL.INSTALACION i on i.ID_INSTALACION = ge.ID_INSTALACION where i.ID_INSTALACION= " + idinstalacion + " and vs.valor='" + valor + "' AND ge.ID_TERCERO = " + idTerceroUsuario.ToString();
            ObjectParameter jSONOUT3 = new ObjectParameter("jSONOUT", typeof(string));

            dbControl.SP_GET_DATOS(sql3, jSONOUT3);

            var emp = JsonConvert.DeserializeObject<List<empresa>>(jSONOUT3.Value.ToString());
            //String sql4 = "select DIRECCION,TELEFONO_INSTALACION,MUNICIPIO,EMAIL_ORGANIZACION  from general.QRYINSTALACION where ID_INSTALACION= " + idinstalacion;
            //String sql4 = "SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS DIRECCION, i.S_TELEFONO AS TELEFONO_INSTALACION, d.S_NOMBRE AS MUNICIPIO, t.S_CORREO AS EMAIL_ORGANIZACION, i.S_NOMBRE AS NOMBRE_INSTALACION, t.S_RSOCIAL AS RAZON_SOCIAL FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION= " + idinstalacion;
            String sql4 = "SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS DIRECCION, i.S_TELEFONO AS TELEFONO_INSTALACION, d.S_NOMBRE AS MUNICIPIO, t.S_CORREO AS EMAIL_ORGANIZACION, i.S_NOMBRE AS NOMBRE_INSTALACION, t.S_RSOCIAL AS RAZON_SOCIAL FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION= " + idinstalacion + " AND ti.ID_TERCERO = " + idTerceroUsuario.ToString();
            ObjectParameter jSONOUT4 = new ObjectParameter("jSONOUT", typeof(string));

            dbControl.SP_GET_DATOS(sql4, jSONOUT4);

            var inst = JsonConvert.DeserializeObject<List<instalacion>>(jSONOUT4.Value.ToString());

            List<doc> observ = null;

            if (datosEncuesta != null && datosEncuesta.ID_PREGUNTA_DESC != null && datosEncuesta.ID_PREGUNTA_DESC > 0)
            {
                //String sqlobser = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.Id_Pregunta=645 and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO;
                String sqlobser = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.Id_Pregunta = " + datosEncuesta.ID_PREGUNTA_DESC.ToString() + " and ep.S_VALOR is not null and s.ID_ESTADO=" + idEstado;
                ObjectParameter jSONobser = new ObjectParameter("jSONOUT", typeof(string));
                dbControl.SP_GET_DATOS(sqlobser, jSONobser);
                observ = JsonConvert.DeserializeObject<List<doc>>(jSONobser.Value.ToString());
            }

            var radicadodocumento = (from Rdocumt in db.RADICADO_DOCUMENTO
                                     where (Rdocumt.ID_RADICADODOC == idRadicado)
                                     select new { Rdocumt.D_RADICADO, Rdocumt.S_RADICADO }).FirstOrDefault();
            Cryptografia crypt = new Cryptografia();
            MemoryStream ms = new MemoryStream();

            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("C://Temp/plantilla.pdf", FileMode.Create));
            PdfWriter writer2 = PdfWriter.GetInstance(doc, memStream);
            int pageNumber = writer.PageNumber - 1;
            doc.Open();

            /*if (encabezado == 1)
            {
                Image imgEncabezado = Image.GetInstance(new Uri(url_rencabezado));
                imgEncabezado.ScalePercent(90f);
                doc.Add(imgEncabezado);
            }*/

            var parr = new Paragraph(newstrDate.ToString());
            parr.Alignment = Element.ALIGN_LEFT;
            doc.Add(parr);
            doc.Add(Chunk.NEWLINE);


            //Radicado01Report etiqueta = new Radicado01Report();
            //MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(idRadicado, "png");

            MemoryStream imagenEtiqueta = new MemoryStream();

            Radicador radicador = new Radicador();
            var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
            imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);


            //System.Drawing.Image sdi = System.Drawing.Image.FromStream(imagenEtiqueta);
            System.Drawing.Image sdi = System.Drawing.Image.FromStream(imagenEtiqueta);

            Image imgRadicado = Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Bmp);
            //Image imgRadicado = Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Png);
            imgRadicado.ScalePercent(50f);
            imgRadicado.Alignment = Element.ALIGN_RIGHT;
            if (x == 0)
            {
                imgRadicado.SetAbsolutePosition(Convert.ToSingle(293), Convert.ToSingle(690));
            }
            else
            {
                imgRadicado.SetAbsolutePosition(Convert.ToSingle(x), Convert.ToSingle(y));
            }

            doc.Add(imgRadicado);


            /*if (encabezado == 1)
            {
                Image imgPiePagina = Image.GetInstance(new Uri(url_piepagina));
                imgPiePagina.ScalePercent(25f);
                imgPiePagina.SetAbsolutePosition(0f, 0f);
                doc.Add(imgPiePagina);
            }*/


            //var Titulo = new Paragraph(strTitulo);
            //Titulo.Alignment = Element.ALIGN_CENTER;
            //var titleFont = FontFactory.GetFont("Courier", 20, BaseColor.BLACK);
            //Titulo.Font = titleFont;
            //doc.Add(Titulo);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            var enc1 = new Paragraph("Señores");
            doc.Add(enc1);
            var enc2 = new Paragraph("AREA METROPOLITANA DEL VALLE DE ABURRA");
            doc.Add(enc2);
            var enc3 = new Paragraph("ATENCIÓN AL USUARIO");
            doc.Add(enc3);
            var enc4 = new Paragraph("Medellín, Antioquia");
            doc.Add(enc4);
            var saltolinea1 = new Paragraph("       ");
            doc.Add(saltolinea1);
            String cuerpo = "";
            try
            {
                if (observ != null)
                    cuerpo = observ[0].nombreDoc;
            }
            catch (Exception e)
            {
            }
            //var asunto = new Paragraph("Asunto " + strTitulo + " Vigencia " + valor);
            var asunto = new Paragraph("Asunto " + strTitulo);
            doc.Add(asunto);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            var obs = new Paragraph(cuerpo);
            doc.Add(obs);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            var atentamente = new Paragraph("Atentamente,");
            doc.Add(atentamente);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEmpresa = new PdfPTable(1);
            tblEmpresa.WidthPercentage = 100;
            String vigencia = "";
            String tercero = "";
            String instalacion = "";
            if (emp.Count > 0)
            {
                vigencia = emp[0].VIGENCIA;
                tercero = emp[0].TERCERO;
                instalacion = emp[0].S_NOMBRE;
            }

            String razonSocila = "";
            try
            {
                razonSocila = inst[0].RAZON_SOCIAL;
            }
            catch (Exception e)
            {
            }
            String nombreInstalacion = "";
            try
            {
                nombreInstalacion = inst[0].NOMBRE_INSTALACION;
            }
            catch (Exception e)
            {
            }
            String municipio = "";
            try
            {
                municipio = inst[0].MUNICIPIO;
            }
            catch (Exception e)
            {
            }
            String direcion = "";
            try
            {
                direcion = inst[0].DIRECCION;
            }
            catch (Exception e)
            {
            }
            String email = "";
            try
            {
                email = inst[0].EMAIL_ORGANIZACION;
            }
            catch (Exception e)
            {
            }
            String telefono = "";
            try
            {
                telefono = inst[0].TELEFONO_INSTALACION;
            }
            catch (Exception e)
            {
            }


            PdfPCell clTercero = new PdfPCell(new Phrase(tercero));
            clTercero.BorderWidth = 0;
            tblEmpresa.AddCell(clTercero);
            PdfPCell clInstalacion = new PdfPCell(new Phrase("INSTALACIÓN: " + instalacion));
            clInstalacion.BorderWidth = 0;
            tblEmpresa.AddCell(clInstalacion);
            PdfPCell clmunicipio = new PdfPCell(new Phrase("MUNICIPIO: " + municipio));
            clmunicipio.BorderWidth = 0;
            tblEmpresa.AddCell(clmunicipio);
            PdfPCell cldirecion = new PdfPCell(new Phrase("DIRECCIÓN: " + direcion));
            cldirecion.BorderWidth = 0;
            tblEmpresa.AddCell(cldirecion);
            PdfPCell clcorreo = new PdfPCell(new Phrase("CORREO ELECTRÓNICO: " + email));
            clcorreo.BorderWidth = 0;
            tblEmpresa.AddCell(clcorreo);
            PdfPCell clTELEFONO = new PdfPCell(new Phrase("TELÉFONO: " + telefono));
            clTELEFONO.BorderWidth = 0;
            tblEmpresa.AddCell(clTELEFONO);


            doc.Add(tblEmpresa);



            doc.Close();
            documento docrtp = new documento();


            var cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = "encuestaTrabajadores.pdf",
                FileName = "encuesta.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            byte[] content = memStream.ToArray();
            PdfSharp.Pdf.PdfDocument docCOD = new PdfSharp.Pdf.PdfDocument();
            docCOD = PdfSharp.Pdf.IO.PdfReader.Open(@"C://Temp/plantilla.pdf", PdfDocumentOpenMode.Import);

            int countPlant = docCOD.PageCount;
            int numPaginas = 0;
            String sqlDoc = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta, NVL(ep.N_VALOR, 0) AS tipoRuta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.ID_TIPOPREGUNTA=8 and ep.S_VALOR is not null and s.ID_ESTADO=" + idEstado + " ORDER BY P.Id_Pregunta";
            ObjectParameter jSONDoc = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlDoc, jSONDoc);
            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();
            for (int idx = 0; idx < countPlant; idx++)
            {
                PdfSharp.Pdf.PdfPage page = docCOD.Pages[idx];
                outputDocument.AddPage(page);
                numPaginas++;

            }
            var anexo = JsonConvert.DeserializeObject<List<doc>>(jSONDoc.Value.ToString());
            string path = (string)webConfigReader.GetValue("ruta_base_Documentos", typeof(string));

            var datosEstado = (from vs in db.VIGENCIA_SOLUCION
                               join ge in db.FRM_GENERICO_ESTADO on vs.ID_ESTADO equals ge.ID_ESTADO
                               where ge.ID_ESTADO == idEstado
                               select new
                               {
                                   ID_TERCERO = ge.ID_TERCERO,
                                   ID_VIGENCIA = vs.ID_VIGENCIA,
                                   VALOR = vs.VALOR
                               }).FirstOrDefault();

            foreach (var item in anexo)
            {
                bool fallaDE = false;
                PdfSharp.Pdf.PdfDocument docAjt;

                PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
                //documentoDE.LoadDocument(path + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + item.idPregunta + "\\" + item.nombreDoc, true);
                if (item.tipoRuta == 1 && System.IO.File.Exists(path + "\\ArchivosEncuestas\\" + datosEstado.ID_VIGENCIA.ToString() + "\\" + datosEstado.VALOR + "\\" + datosEstado.ID_TERCERO.ToString() + "\\" + idEstado.ToString() + "_" + item.idPregunta + "_" + item.nombreDoc))
                {
                    documentoDE.LoadDocument(path + "\\ArchivosEncuestas\\" + datosEstado.ID_VIGENCIA.ToString() + "\\" + datosEstado.VALOR + "\\" + datosEstado.ID_TERCERO.ToString() + "\\" + idEstado.ToString() + "_" + item.idPregunta + "_" + item.nombreDoc, true);
                }
                else
                {
                    documentoDE.LoadDocument(path + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + item.idPregunta + "\\" + item.nombreDoc, true);
                }

                MemoryStream documentoDEPDF = new MemoryStream();
                try
                {
                    documentoDE.SaveDocument(documentoDEPDF);
                    documentoDE.CloseDocument();
                }
                catch
                {
                    fallaDE = true;
                }

                docAjt = new PdfSharp.Pdf.PdfDocument();

                if (!fallaDE)
                    docAjt = PdfSharp.Pdf.IO.PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);
                else
                    docAjt = PdfSharp.Pdf.IO.PdfReader.Open(path + "\\Tercero\\" + idTerceroUsuario + "\\Pregunta\\" + item.idPregunta + "\\" + item.nombreDoc, PdfDocumentOpenMode.Import);


                int countDocAj = docAjt.PageCount;
                for (int idx = 0; idx < countDocAj; idx++)
                {
                    PdfSharp.Pdf.PdfPage page = docAjt.Pages[idx];
                    outputDocument.AddPage(page);
                    numPaginas++;
                }
            }

            if (registrarDocumento == 1)
            {
                int codFuncionario = Convert.ToInt32(SIM.Utilidades.Data.ObtenerValorParametro("FuncionarioPMES"));
                int codProceso = Convert.ToInt32(SIM.Utilidades.Data.ObtenerValorParametro("IdProcesoPMES"));

                int codTramite = db.Database.SqlQuery<int>("SELECT CODTRAMITE FROM CONTROL.FRM_GENERICO_ESTADO WHERE ID_ESTADO = " + idEstado.ToString()).FirstOrDefault();

                docrtp = RegistrarDocumento(codFuncionario, idRadicado, numPaginas, codTramite, codProceso, 10);
                String rut = docrtp.file.ToString().Substring(0, docrtp.file.ToString().Length - 1);
                System.IO.Directory.CreateDirectory(rut);
                dbControl.SP_SET_URLRADICADO(idEstado, docrtp.RUTA.ToString());

                outputDocument.Save(ms);
                crypt.Encriptar(ms, docrtp.RUTA.ToString(), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
                //outputDocument.Save(docrtp.RUTA.ToString());

                ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
                String strindice = Convert.ToDecimal(docrtp.IDDOC) + "-" + radicadodocumento.D_RADICADO.ToString() + "-" + radicadodocumento.S_RADICADO + "-" + codTramite + "-" + strTitulo + "-" + usuario.S_NOMBRES + " " + usuario.S_APELLIDOS + "-" + tercero + "-" + idTerceroUsuario + "-" + idinstalacion;
                try
                {
                    dbControl.SP_SET_INDICE_COR(Convert.ToDecimal(docrtp.IDDOC), radicadodocumento.D_RADICADO.ToString(), radicadodocumento.S_RADICADO, codTramite, strTitulo, usuario.S_NOMBRES + " " + usuario.S_APELLIDOS, tercero, idTerceroUsuario, idinstalacion, rta);
                }
                catch (Exception e)
                {

                }
            }

            outputDocument.Save(ms);
            

            if (encriptado == 1)
            {
                string archivo = "ReporteEncuestaRadicado" + DateTime.Now.Minute.ToString("00") + "_" + DateTime.Now.Second.ToString("00") + ".pdf";

                crypt.Encriptar(ms, "C:\\Temp\\" + archivo, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));

                return File(System.IO.File.ReadAllBytes("C:\\Temp\\" + archivo), "application/pdf", archivo);
            }
            else
            {
                return File(ms.GetBuffer(), "application/pdf");
            }
        }

        public ActionResult conaultardocradicado(int ID_ESTADO)
        {
            String sqlUrl = "select URL from control.FRM_GENERICO_ESTADO where ID_ESTADO=" + ID_ESTADO;
            ObjectParameter jSONDoc = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlUrl, jSONDoc);
            var docum = JsonConvert.DeserializeObject<List<doc>>(jSONDoc.Value.ToString());
            Cryptografia crypt = new Cryptografia();
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "documentoPMES.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            MemoryStream ms = crypt.DesEncriptar(docum[0].url, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));
            return File(ms.GetBuffer(), "application/pdf");
        }
        public ActionResult reportEncuestasinradicado(int idinstalacion,int ID_ESTADO)
        {
            var sql = "SELECT distinct v.NOMBRE||'-'||vs.VALOR AS ENCUESTA_TITULO, v.ID_PREGUNTA_DESC FROM control.VIGENCIA v INNER JOIN control.VIGENCIA_SOLUCION vs ON vs.ID_VIGENCIA=v.ID_VIGENCIA INNER JOIN control.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE ge.ID_ESTADO=" + ID_ESTADO.ToString();

            var datosEncuesta = db.Database.SqlQuery<DatosBaseEncuesta>(sql).FirstOrDefault();

            string tituloEncuesta = "";

            if (datosEncuesta != null)
            {
                tituloEncuesta = datosEncuesta.ENCUESTA_TITULO;
            }

            String strTitulo = tituloEncuesta; //"Documento Plan MES";
            var url_rencabezado = (string)webConfigReader.GetValue("url_rps_encabesado", typeof(string));
            var url_piepagina = (string)webConfigReader.GetValue("url_rps_piepagina", typeof(string));
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
            {
                idTerceroUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);


            }
            var usuario = (from Rdocumt in db.USUARIO
                           where (Rdocumt.ID_USUARIO == idUsuario)
                           select new { Rdocumt.S_NOMBRES, Rdocumt.S_APELLIDOS }).FirstOrDefault();

            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            DateTime localDate = DateTime.Now;
            string newstrDate = localDate.ToString("M/d/yyyy");
          

            //String sql4 = "select DIRECCION,TELEFONO_INSTALACION,MUNICIPIO,EMAIL_ORGANIZACION,NOMBRE_INSTALACION,RAZON_SOCIAL  from general.QRYINSTALACION where ID_INSTALACION= " + idinstalacion;
            String sql4 = "SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS DIRECCION, i.S_TELEFONO AS TELEFONO_INSTALACION, d.S_NOMBRE AS MUNICIPIO, t.S_CORREO AS EMAIL_ORGANIZACION, i.S_NOMBRE AS NOMBRE_INSTALACION, t.S_RSOCIAL AS RAZON_SOCIAL FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION= " + idinstalacion + " AND ti.ID_TERCERO = " + idTerceroUsuario.ToString();
            ObjectParameter jSONOUT4 = new ObjectParameter("jSONOUT", typeof(string));

            dbControl.SP_GET_DATOS(sql4, jSONOUT4);

            var inst = JsonConvert.DeserializeObject<List<instalacion>>(jSONOUT4.Value.ToString());

            /*String sqlDoc = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where   P.Id_Pregunta=645 and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO;
            ObjectParameter jSONDoc = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sqlDoc, jSONDoc);
            var obser = JsonConvert.DeserializeObject<List<doc>>(jSONDoc.Value.ToString());*/

            List<doc> obser = null;

            if (datosEncuesta != null && datosEncuesta.ID_PREGUNTA_DESC != null && datosEncuesta.ID_PREGUNTA_DESC > 0)
            {
                String sqlobser = "select distinct  ep.S_VALOR nombreDoc,P.Id_Pregunta idPregunta from ENC_SOLUCION s inner join ENC_SOLUCION_PREGUNTAS ep on ep.ID_SOLUCION=s.ID_SOLUCION inner join Enc_Pregunta p on P.Id_Pregunta=Ep.Id_Pregunta where  p.Id_Pregunta = " + datosEncuesta.ID_PREGUNTA_DESC.ToString() + " and ep.S_VALOR is not null and s.ID_ESTADO=" + ID_ESTADO;
                ObjectParameter jSONobser = new ObjectParameter("jSONOUT", typeof(string));
                dbControl.SP_GET_DATOS(sqlobser, jSONobser);
                obser = JsonConvert.DeserializeObject<List<doc>>(jSONobser.Value.ToString());
            }

            MemoryStream ms = new MemoryStream();

            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            
            PdfWriter writer2 = PdfWriter.GetInstance(doc, memStream);
          
            doc.Open();
            Image imgEncabezado = Image.GetInstance(new Uri(url_rencabezado));
            imgEncabezado.ScalePercent(90f);

            doc.Add(imgEncabezado);

           
            Image imgPiePagina = Image.GetInstance(new Uri(url_piepagina));
            imgPiePagina.ScalePercent(25f);
            imgPiePagina.SetAbsolutePosition(0f, 0f);
            doc.Add(imgPiePagina);


            var parr = new Paragraph(newstrDate.ToString());
            parr.Alignment = Element.ALIGN_CENTER;
            doc.Add(parr);
            doc.Add(Chunk.NEWLINE);

            var Titulo = new Paragraph(strTitulo);
            Titulo.Alignment = Element.ALIGN_CENTER;
            var titleFont = FontFactory.GetFont("Courier", 20, BaseColor.BLACK);
            Titulo.Font = titleFont;
            doc.Add(Titulo);
            doc.Add(Chunk.NEWLINE);
            var enc1 = new Paragraph("Señores");
            doc.Add(enc1);
            var enc2 = new Paragraph("AREA METROPOLITANA DEL VALLE DE ABURRA");
            doc.Add(enc2);
            var enc3 = new Paragraph("ATENCIÓN AL USUARIO");
            doc.Add(enc3);
            var enc4 = new Paragraph("Medellín, Antioquia");
            doc.Add(enc4);
            var saltolinea1 = new Paragraph("       ");
            doc.Add(saltolinea1);

            var asunto = new Paragraph("Asunto " + strTitulo);
            doc.Add(asunto);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            String observa="";
            try
            {
                observa = obser[0].nombreDoc;
            }
            catch (Exception e)
            {
            }

            String razonSocila = "";
            try
            {
                razonSocila = inst[0].RAZON_SOCIAL;
            }
            catch (Exception e)
            {
            }
            String nombreInstalacion = "";
            try
            {
                nombreInstalacion = inst[0].NOMBRE_INSTALACION;
            }
            catch (Exception e)
            {
            }
                 String municipio = "";
            try
            {
                municipio = inst[0].MUNICIPIO;
            }
            catch (Exception e)
            {
            }
                 String direcion = "";
            try
            {
                direcion = inst[0].DIRECCION;
            }
            catch (Exception e)
            {
            }
                 String email = "";
            try
            {
                email = inst[0].EMAIL_ORGANIZACION;
            }
            catch (Exception e)
            {
            }
                    String telefono = "";
            try
            {
                telefono = inst[0].TELEFONO_INSTALACION;
            }
            catch (Exception e)
            {
            }
            
            var obs = new Paragraph(observa);
            doc.Add(obs);
           doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            var atentamente = new Paragraph("Atentamente,");
            doc.Add(atentamente);
            doc.Add(saltolinea1);
            doc.Add(saltolinea1);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblEmpresa = new PdfPTable(1);
            tblEmpresa.WidthPercentage = 100;
            PdfPCell clTercero = new PdfPCell(new Phrase(razonSocila));
            clTercero.BorderWidth = 0;
            tblEmpresa.AddCell(clTercero);
            PdfPCell clInstalacion = new PdfPCell(new Phrase("INSTALACIÓN: " + nombreInstalacion));
            clInstalacion.BorderWidth = 0;
            tblEmpresa.AddCell(clInstalacion);
            PdfPCell clmunicipio = new PdfPCell(new Phrase("MUNICIPIO: " + municipio));
            clmunicipio.BorderWidth = 0;
            tblEmpresa.AddCell(clmunicipio);
            PdfPCell cldirecion = new PdfPCell(new Phrase("DIRECCIÓN: " + direcion));
            cldirecion.BorderWidth = 0;
            tblEmpresa.AddCell(cldirecion);
            PdfPCell clcorreo = new PdfPCell(new Phrase("CORREO ELECTRÓNICO: " + email));
            clcorreo.BorderWidth = 0;
            tblEmpresa.AddCell(clcorreo);
            PdfPCell clTELEFONO = new PdfPCell(new Phrase("TELÉFONO: " + telefono));
            clTELEFONO.BorderWidth = 0;
            tblEmpresa.AddCell(clTELEFONO);
            doc.Add(tblEmpresa);
            doc.Close();
            var cd = new System.Net.Mime.ContentDisposition
            {
                //FileName = "encuestaTrabajadores.pdf",
                FileName = "encuesta.pdf",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(memStream.GetBuffer(), "application/pdf");

        }
        private documento RegistrarDocumento(int codFuncionario, int idRadicado, int numPaginas, int idTramit, int idproceso, int serieCod)
        {
            string rutaDocumento;
            int idCodDocumento;


            TBRUTAPROCESO rutaProceso = db.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == idproceso).FirstOrDefault();
            TBTRAMITEDOCUMENTO ultimoDocumento = db.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == idTramit).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();
            RADICADO_DOCUMENTO radicadoDocumento = db.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicado).FirstOrDefault();

            if (ultimoDocumento == null)
                idCodDocumento = 1;
            else
                idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

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
            documento.CIFRADO = "1";
            //Convert.ToInt32(radicado.CODSERIE);

            db.Entry(documento).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            relDocTra.CODTRAMITE = idTramit;
            relDocTra.CODDOCUMENTO = idCodDocumento;
            relDocTra.ID_DOCUMENTO = documento.ID_DOCUMENTO;
            db.Entry(relDocTra).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            documento doc = new documento();
            doc.RUTA = rutaDocumento;
            doc.IDDOC = idCodDocumento.ToString();
            doc.file = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(idTramit), 100);

            /*RADICADOS radicado = (from r in db.RADICADOS
                                 where r.ID_RADICADO == idRadicado
                                 select r).FirstOrDefault();*/

            if (radicadoDocumento != null)
            {
                radicadoDocumento.CODTRAMITE = idTramit;
                radicadoDocumento.CODDOCUMENTO = idCodDocumento;

                db.Entry(radicadoDocumento).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return doc;
        } 

        // Radica y genera documento de una encuesta
        public ActionResult RadicarGenerarDocumento(int idUsuario, int idEstado, int unidadDocumental, int idTercero, int idInstalacion, string valor, string claveFuncionario)
        {
            // Generar Radicado y asignarselo al Estado
            var fechaCreacion = DateTime.Now;
            Radicador radicador = new Radicador();
            DatosRadicado radicadoGenerado = radicador.GenerarRadicado(unidadDocumental, 0, fechaCreacion, claveFuncionario);

            guardarradicadocod(idEstado, radicadoGenerado.IdRadicado.ToString());

            return reportEncuestaCardRadicadoTercero(idInstalacion, valor, radicadoGenerado.IdRadicado, idEstado, idUsuario, idTercero);
        }

        // Radica y genera documento de una encuesta
        public ActionResult RadicarEncuestaEstado(int idEstado)
        {
            int unidadDocumental = 10;
            int idTercero;
            int idInstalacion;
            string valor;
            string claveFuncionario = "FuncionarioPMES";

            // Generar Radicado y asignarselo al Estado
            var fechaCreacion = DateTime.Now;
            Radicador radicador = new Radicador();
            DatosRadicado radicadoGenerado = radicador.GenerarRadicado(unidadDocumental, 0, fechaCreacion, claveFuncionario);

            guardarradicadocod(idEstado, radicadoGenerado.IdRadicado.ToString());

            var sql = "SELECT vs.ID_VIGENCIA, ge.ID_TERCERO, ge.ID_INSTALACION, vs.VALOR AS VALOR_VIGENCIA FROM CONTROL.VIGENCIA_SOLUCION vs INNER JOIN CONTROL.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE vs.ID_ESTADO = :idEstado";

            OracleParameter idEstadoParameter = new OracleParameter("idEstado", idEstado);

            DatosEstadoVigencia datosEstado = db.Database.SqlQuery<DatosEstadoVigencia>(sql, new object[] { idEstadoParameter }).FirstOrDefault();

            if (datosEstado != null)
                return reportEncuestaCardRadicadoTercero(datosEstado.ID_INSTALACION, datosEstado.VALOR_VIGENCIA, radicadoGenerado.IdRadicado, idEstado, null, datosEstado.ID_TERCERO);
            else
                return null;
        }

        // &&&
        // registrarDocumento: 0 No registrar el documento en el sistema, solamente generar el PDF - 1 Generar el PDF y registrar el documento en el sistema
        public ActionResult RadicarEncuestaEstadoRadicadoGenerado(int idEstado, int registrarDocumento)
        {
            var datosRadicado = db.FRM_GENERICO_ESTADO.Where(e => e.ID_ESTADO == idEstado).FirstOrDefault();

            if (datosRadicado != null && datosRadicado.CODRADICADO != null && datosRadicado.CODRADICADO > 0 && datosRadicado.CODTRAMITE != null && datosRadicado.CODTRAMITE > 0)
            {
                tramite tramite = new tramite();

                var funcionario = db.PARAMETROS.Where(p => p.CLAVE == "FuncionarioPMES").Select(p => p.VALOR).FirstOrDefault();
                var proceso = db.PARAMETROS.Where(p => p.CLAVE == "IdProcesoPMES").Select(p => p.VALOR).FirstOrDefault();

                tramite.codTramite = datosRadicado.CODTRAMITE.ToString();
                tramite.codProceso = proceso;
                tramite.codFuncionario = funcionario;

                return RadicarEncuestaEstadoRadicado(idEstado, Convert.ToInt32(datosRadicado.CODRADICADO), tramite, registrarDocumento);
            }
            else
            {
                return null;
            }
        }

        public ActionResult RadicarEncuestaEstadoRadicado(int idEstado, int idRadicado, tramite tramite, int registrarDocumento)
        {
            int unidadDocumental = 10;
            int idTercero;
            int idInstalacion;
            string valor;
            string claveFuncionario = "FuncionarioPMES";

            DatosRadicado radicadoGenerado = new DatosRadicado() { IdRadicado = idRadicado, Radicado = null, Etiqueta = null, Fecha = DateTime.Today };

            guardarradicadocod(idEstado, radicadoGenerado.IdRadicado.ToString());

            var sql = "SELECT vs.ID_VIGENCIA, ge.ID_TERCERO, ge.ID_INSTALACION, vs.VALOR AS VALOR_VIGENCIA FROM CONTROL.VIGENCIA_SOLUCION vs INNER JOIN CONTROL.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO WHERE vs.ID_ESTADO = :idEstado";

            OracleParameter idEstadoParameter = new OracleParameter("idEstado", idEstado);

            DatosEstadoVigencia datosEstado = db.Database.SqlQuery<DatosEstadoVigencia>(sql, new object[] { idEstadoParameter }).FirstOrDefault();

            if (datosEstado != null)
                return reportEncuestaCardRadicadoTercero(datosEstado.ID_INSTALACION, datosEstado.VALOR_VIGENCIA, radicadoGenerado.IdRadicado, idEstado, null, datosEstado.ID_TERCERO, tramite, (registrarDocumento == 1));
            else
                return null;
        }

        public tramite guardarTramite(String mensaje, int idEstado)
        {
            tramite tramite = new tramite();
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            ObjectParameter rta2 = new ObjectParameter("rTA2", typeof(string));
            ObjectParameter rta3 = new ObjectParameter("rTA3", typeof(string));
            ObjectParameter rta4 = new ObjectParameter("rTA4", typeof(string));
            dbControl.SP_SET_TRAMITE_PMES(mensaje, "FuncionarioPMES", "TareaInicialPMES", "IdProcesoPMES", idEstado, rta, rta2, rta3, rta4);
            tramite.codTramite = rta2.Value.ToString();
            tramite.codProceso = rta3.Value.ToString();
            tramite.codFuncionario = rta4.Value.ToString();
            return tramite;
        }

        public string DatosEstadoVigencia(int idEstado)
        {
            var sql = "SELECT vs.ID_VIGENCIA, ge.ID_TERCERO, ge.ID_INSTALACION, vs.VALOR AS VALOR_VIGENCIA, NVL(v.RADICAR, 0) AS RADICADO FROM CONTROL.VIGENCIA_SOLUCION vs INNER JOIN CONTROL.FRM_GENERICO_ESTADO ge ON vs.ID_ESTADO = ge.ID_ESTADO LEFT OUTER JOIN CONTROL.VIGENCIA v ON vs.ID_VIGENCIA = v.ID_VIGENCIA  WHERE vs.ID_ESTADO = :idEstado";

            OracleParameter idEstadoParameter = new OracleParameter("idEstado", idEstado);

            DatosEstadoVigencia datosEstado = db.Database.SqlQuery<DatosEstadoVigencia>(sql, new object[] { idEstadoParameter }).FirstOrDefault();

            if (datosEstado != null)
                return JsonConvert.SerializeObject(datosEstado);
            else
                return null;
        }

        public string ValidarDependienciasVigencia(int idVigencia, int idInstalacion, string valorVigencia)
        {
            List<VigenciasRelacionadas> vigenciasValidacion = null;

            // Se obtienen los grupos asociados a la vigencia en las dependencias
            var sqlGrupos = "SELECT DISTINCT S_GRUPO " +
                "FROM CONTROL.ENC_VIGENCIA_DEPENDENCIA " +
                "WHERE ID_VIGENCIA = " + idVigencia.ToString();

            var grupos = db.Database.SqlQuery<string>(sqlGrupos).ToList();

            foreach (var grupo in grupos)
            {
                var sql = "SELECT v.ID_VIGENCIA, " +
                            "v.NOMBRE AS VIGENCIA, " +
                            "s.ID_INSTALACION, " +
                            "COUNT(s.ID_ESTADO) AS NUM_SOLUCIONES,  " +
                            "SUM(CASE WHEN NVL(s.TIPO_GUARDADO, 0) = 0 THEN 0 ELSE 1 END) AS NUM_ENVIADOS, " +
                            "CASE WHEN COUNT(s.ID_ESTADO) = SUM(CASE WHEN NVL(s.TIPO_GUARDADO, 0) = 0 THEN 0 ELSE 1 END) AND SUM(CASE WHEN NVL(s.TIPO_GUARDADO, 0) = 0 THEN 0 ELSE 1 END) > 0 THEN 1 ELSE 0 END AS ENVIADO " +
                            "FROM CONTROL.ENC_VIGENCIA_DEPENDENCIA vd INNER JOIN " +
                            "CONTROL.VIGENCIA v ON vd.ID_VIGENCIA_DEPENDE = v.ID_VIGENCIA " + (grupo != null && grupo.Trim() != "" ? " AND vd.S_GRUPO = '" + grupo + "'" : "") + "LEFT OUTER JOIN " +
                            "( " +
                            "SELECT gei.ID_INSTALACION, vsi.ID_VIGENCIA, gei.ID_ESTADO, gei.TIPO_GUARDADO  " +
                            "FROM CONTROL.ENC_VIGENCIA_DEPENDENCIA vdi INNER JOIN " +
                            "CONTROL.VIGENCIA vi ON vdi.ID_VIGENCIA = :idVigencia AND vdi.ID_VIGENCIA_DEPENDE = vi.ID_VIGENCIA " + (grupo != null && grupo.Trim() != "" ? " AND vdi.S_GRUPO = '" + grupo + "'" : "") + "INNER JOIN " +
                            "CONTROL.VIGENCIA_SOLUCION vsi ON vi.ID_VIGENCIA = vsi.ID_VIGENCIA AND vsi.VALOR = :valorVigencia INNER JOIN " +
                            "CONTROL.FRM_GENERICO_ESTADO gei ON vsi.ID_ESTADO = gei.ID_ESTADO AND gei.ID_INSTALACION = :instalacion AND gei.ACTIVO = 0 " +
                            ") s ON v.ID_VIGENCIA = s.ID_VIGENCIA " +
                            "WHERE vd.ID_VIGENCIA = :idVigencia " +
                            "GROUP BY v.ID_VIGENCIA, v.NOMBRE, s.ID_INSTALACION " +
                            "HAVING (CASE WHEN COUNT(s.ID_ESTADO) = SUM(CASE WHEN NVL(s.TIPO_GUARDADO, 0) = 0 THEN 0 ELSE 1 END) AND SUM(CASE WHEN NVL(s.TIPO_GUARDADO, 0) = 0 THEN 0 ELSE 1 END) > 0 THEN 1 ELSE 0 END) = 0 ";

                OracleParameter idVigenciaParameter = new OracleParameter("idVigencia", idVigencia);
                OracleParameter valorVigenciaParameter = new OracleParameter("valorVigencia", valorVigencia);
                OracleParameter instalacionParameter = new OracleParameter("instalacion", idInstalacion);

                var dependencias = db.Database.SqlQuery<VigenciasRelacionadas>(sql, new object[] { idVigenciaParameter, valorVigenciaParameter, instalacionParameter }).ToList();

                if (vigenciasValidacion == null)
                    vigenciasValidacion = dependencias;
                else
                {
                    if (dependencias.Count < vigenciasValidacion.Count)
                        vigenciasValidacion = dependencias;
                }
            }

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = (vigenciasValidacion == null ? 0 : vigenciasValidacion.Count());
            if (resultado.numRegistros == 0)
                resultado.datos = null;
            else
                resultado.datos = (IEnumerable<dynamic>)vigenciasValidacion;

            return JsonConvert.SerializeObject(resultado);
        }

        // e: ID_ESTADO
        // n: Nombre Encuesta
        public void ActualizarNombreEncuesta(int e, string n)
        {
            var genericoEstado = (from ge in db.FRM_GENERICO_ESTADO
                              where ge.ID_ESTADO == e
                              select ge).FirstOrDefault();

            genericoEstado.NOMBRE = n.Trim();

            db.Entry(genericoEstado).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        // e: ID_ESTADO
        // c: Clave Encuesta
        public void ActualizarClaveEncuesta(int e, string c)
        {
            var genericoEstado = (from ge in db.FRM_GENERICO_ESTADO
                                  where ge.ID_ESTADO == e
                                  select ge).FirstOrDefault();

            genericoEstado.S_CLAVE = c.Trim();

            db.Entry(genericoEstado).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public ActionResult PlantillaCargaMasiva(int v, string valor, bool inicial = true)
        {
            string path = ConfigurationManager.AppSettings["RutaPlantillasEncuestas"];

            var vigencia = db.VIGENCIA.Where(r => r.ID_VIGENCIA == v).FirstOrDefault();
            string fileName = Path.Combine(path + "\\" + v.ToString(), vigencia.S_NOMBRE_ARCHIVO + ".xlsx");

            if (System.IO.File.Exists(fileName))
            {
                return File(System.IO.File.ReadAllBytes(fileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + " " + valor + ".xlsx");
            }
            else
            {
                if (inicial)
                {
                    CargaMasiva cargaMasiva = new CargaMasiva();

                    cargaMasiva.GenerarPlantillaCargaMasiva(path, v);

                    return PlantillaCargaMasiva(v, valor, false);
                }
                else
                {
                    return null;
                }
            }
        }

        public string GenerarPlantillaCargaMasiva(int v)
        {
            string path = ConfigurationManager.AppSettings["RutaPlantillasEncuestas"];

            if (path == null || path.Trim() == "")
            {
                return "Error: Configuración Inválida.";
            }
            else
            {
                CargaMasiva cargaMasiva = new CargaMasiva();

                try
                {
                    cargaMasiva.GenerarPlantillaCargaMasiva(path, v);
                    return "OK";
                }
                catch (Exception error)
                {
                    return "Error: " + error.Message;
                }
            }

            /*
            string path = HostingEnvironment.MapPath("~/App_Data/Plantillas/Encuestas/" + v.ToString());

            var vigencia = db.VIGENCIA.Where(r => r.ID_VIGENCIA == v).FirstOrDefault();
            string fileName = Path.Combine(path, vigencia.S_NOMBRE_ARCHIVO + ".xlsx");

            if (vigencia != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                MemoryStream archivoPlantilla = new MemoryStream();

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(archivoPlantilla, SpreadsheetDocumentType.Workbook))
                //using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    {
                        // Hoja de Datos
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet();

                        Sheet sheetDatos = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Datos" };
                        sheets.Append(sheetDatos);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        Row titulo = new Row();
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(vigencia.NOMBRE);
                        titulo.AppendChild(cell);
                        sheetData.AppendChild(titulo);

                        sheetData.AppendChild(new Row());

                        string sql = "SELECT p.S_NOMBRE " +
                            "FROM CONTROL.VIGENCIA v INNER JOIN " +
                            "CONTROL.ENCUESTA_VIGENCIA ev ON v.ID_VIGENCIA = ev.ID_VIGENCIA INNER JOIN " +
                            "CONTROL.FORMULARIO_ENCUESTA fe ON ev.ID_ENCUESTA = fe.ID_ENCUESTA AND fe.ID_FORMULARIO = 14 INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON ev.ID_ENCUESTA = ep.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA " +
                            "WHERE v.ID_VIGENCIA = " + v.ToString() + " " +
                            "ORDER BY fe.N_ORDEN, ep.N_ORDEN";

                        var preguntas = db.Database.SqlQuery<string>(sql).ToList();

                        List<OpenXmlElement> colIDPreguntas = new List<OpenXmlElement>();
                        List<OpenXmlElement> colPreguntas = new List<OpenXmlElement>();

                        foreach (var pregunta in preguntas)
                        {
                            colIDPreguntas.Add(new Cell()
                            {
                                CellValue = new CellValue(pregunta),
                                DataType = new EnumValue<CellValues>(CellValues.String),
                            });

                            colPreguntas.Add(new Cell()
                            {
                                CellValue = new CellValue(pregunta),
                                DataType = new EnumValue<CellValues>(CellValues.String),
                            });
                        }

                        Row row = new Row();

                        row.Append(colPreguntas);
                        sheetData.AppendChild(row);

                        //workbookPart.Workbook.Save();
                    }

                    {
                        // Hoja de Opciones
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet();

                        Sheet sheetDatos = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 2, Name = "Opciones" };
                        sheets.Append(sheetDatos);

                        SheetData sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        Row titulo = new Row();
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue("TITULO 2");
                        titulo.AppendChild(cell);
                        sheetData.AppendChild(titulo);

                        sheetData.AppendChild(new Row());

                        string sql = "SELECT p.S_NOMBRE " +
                            "FROM CONTROL.VIGENCIA v INNER JOIN " +
                            "CONTROL.ENCUESTA_VIGENCIA ev ON v.ID_VIGENCIA = ev.ID_VIGENCIA INNER JOIN " +
                            "CONTROL.FORMULARIO_ENCUESTA fe ON ev.ID_ENCUESTA = fe.ID_ENCUESTA AND fe.ID_FORMULARIO = 14 INNER JOIN " +
                            "CONTROL.ENC_ENCUESTA_PREGUNTA ep ON ev.ID_ENCUESTA = ep.ID_ENCUESTA INNER JOIN " +
                            "CONTROL.ENC_PREGUNTA p ON ep.ID_PREGUNTA = p.ID_PREGUNTA " +
                            "WHERE v.ID_VIGENCIA = " + v.ToString() + " " +
                            "ORDER BY fe.N_ORDEN, ep.N_ORDEN";

                        var preguntas = db.Database.SqlQuery<string>(sql).ToList();

                        List<OpenXmlElement> colPreguntas = new List<OpenXmlElement>();
                        foreach (var pregunta in preguntas)
                        {
                            colPreguntas.Add(new Cell()
                            {
                                CellValue = new CellValue(pregunta + " 2"),
                                DataType = new EnumValue<CellValues>(CellValues.String),
                            });
                        }

                        Row row = new Row();

                        row.Append(colPreguntas);
                        sheetData.AppendChild(row);
                    }
                }

                var result = File(archivoPlantilla.GetBuffer(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + ".xlsx");
                //archivoPlantilla.Dispose();

                return result;

                //return File(System.IO.File.ReadAllBytes(fileName), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", vigencia.S_NOMBRE_ARCHIVO + ".xlsx");
            } else
            {
                return null;
            }*/
        }

        public string GruposVigencia()
        {
            var sql = "SELECT -1 AS ID_VIGENCIA_GRUPO, '(TODOS)' AS S_NOMBRE, '' AS S_NOMBRE_ORIGINAL " +
                "FROM DUAL " +
                "UNION ALL " +
                "SELECT ID_VIGENCIA_GRUPO, S_NOMBRE, S_NOMBRE AS S_NOMBRE_ORIGINAL " +
                "FROM CONTROL.VIGENCIA_GRUPO " +
                "UNION ALL " +
                "SELECT -2 ID_VIGENCIA_GRUPO, '[OTROS]' AS S_NOMBRE, '[OTROS]' AS S_NOMBRE_ORIGINAL " +
                "FROM DUAL " +
                "ORDER BY S_NOMBRE_ORIGINAL";

            var datosGruposVigencia = db.Database.SqlQuery<GrupoVigencia>(sql, new object[0]);

            if (datosGruposVigencia != null)
                return JsonConvert.SerializeObject(datosGruposVigencia.OrderBy(g => g.S_NOMBRE_ORIGINAL).ToList());
               else
                return null;
        }

        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        } 

        public class doc
        {
            public String nombreDoc { get; set; }
            public String idPregunta { get; set; }
            public String ruta { get; set; }
            public String valor { get; set; }
            public String url { get; set; }
            public int tipoRuta { get; set; } // 0 Anterior, 1 Nueva

        }
        public class documento
        {
            public byte[] DOC { get; set; }
            public String IDARBOL { get; set; }
            public String TIPO { get; set; }
            public String RUTA { get; set; }
            public String IDDOC { get; set; }
            public String file { get; set; }
        }
          public class instalacion
        {
            
            public String DIRECCION { get; set; }
            public String TELEFONO_INSTALACION { get; set; }
            public String MUNICIPIO { get; set; }
            public String EMAIL_ORGANIZACION { get; set; }
            public String NOMBRE_INSTALACION { get; set; }
            public String RAZON_SOCIAL { get; set; }
              
              
              
       
        }
        
        public class terc
        {
            public String id { get; set; }
            public String nombre { get; set; }
      
        }
        public class solucion
        {
            public String ID_ENCUESTA { get; set; }
            public String ID_PREGUNTA { get; set; }
            public String S_VALOR { get; set; }

        }
        public class pregunta
        {
            public String ID_ENCUESTA { get; set; }
            public String ID_PREGUNTA { get; set; }
            public String ID_ESTADO { get; set; }
            public String PREGUNTA { get; set; }
            public String RESPUESTA { get; set; }
            public String OBSERVACION { get; set; }
            public String ID_TIPOPREGUNTA { get; set; }
        }
        public class tramite
        {
            public String codTramite { get; set; }
            public String codProceso { get; set; }
            public String codFuncionario { get; set; }

        }
 
        public class trabajador
        {
            public String NOMBRE { get; set; }
           

        }
        public class Data
        {
            public Data()
            {
            }
            private int _id;
            private string _name;
            private DateTime _date;
            public int Id
            {
                get { return this._id; }
                set { this._id = value; }
            }
            public string Name
            {
                get { return this._name; }
                set { this._name = value; }
            }
            public DateTime Date
            {
                get { return this._date; }
                set { this._date = value; }
            }
        }
    }
}