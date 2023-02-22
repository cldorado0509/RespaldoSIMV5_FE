using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Oracle;
using Microsoft.Owin.Security;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Models;
using Newtonsoft.Json;
using System.Text;
using System.Web.Hosting;
using System.Security.Cryptography;
using System.IO;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;
using System.Globalization;
using System.Data.Entity;
using SIM.Areas.EncuestaExterna.Reporte;
using System.Data.Entity.SqlServer;
using Xceed.Words.NET;
using Oracle.ManagedDataAccess.Client;
using SIM.Areas.Tramites.Models;
using SIM.Utilidades;
using SIM.Data;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProgramadorActuacionesController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        decimal idTerceroUsuario;

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        public ActionResult Index()
        {
            return View();
        }

        /*[Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionTerceros()
        {
            return View();
        }

        // id: Id Tercero
        // v: Vigencia
        [Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionTercero(int? id, int? ter, string v, int? tra)
        {
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero;

            if (id != null)
            {
                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID == id
                                             select eet).FirstOrDefault();

                if (evaluacionEncuestaTercero == null)
                    return null;

                ter = evaluacionEncuestaTercero.ID_TERCERO;
                v = evaluacionEncuestaTercero.S_VALOR_VIGENCIA;
                tra = evaluacionEncuestaTercero.CODTRAMITE;
            }
            else
            {
                if (ter == null || v == null || tra == null)
                    return null;

                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID_TERCERO == ter && eet.S_VALOR_VIGENCIA == v
                                             select eet).FirstOrDefault();

                decimal? estado;

                estado = (from ge in dbSIM.FRM_GENERICO_ESTADO
                          join vs in dbSIM.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                          where ge.CODTRAMITE == tra
                          orderby ge.ID_ESTADO descending
                          select ge.ID_ESTADO).FirstOrDefault();

                if (estado == null)
                {
                    estado = (from ge in dbSIM.FRM_GENERICO_ESTADO
                              join vs in dbSIM.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                              where ge.ID_TERCERO == id && (vs.ID_VIGENCIA == 721 || vs.ID_VIGENCIA == 781) && vs.VALOR == v
                              orderby ge.ID_ESTADO descending
                              select ge.ID_ESTADO).FirstOrDefault();
                }

                if (estado == null)
                    return null;

                if (evaluacionEncuestaTercero == null) // Aun no existe la evaluacion para el tercero en esa vigencia
                {
                    bool medioEntregaSIM = false;

                    var cm = (from ee in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                              where ee.CODTRAMITE == (int)tra
                              select new
                              {
                                  ee.CM,
                                  ee.N_COD_MUNICIPIO
                              }).FirstOrDefault();

                    datosRadicado radicado = (from ge in dbSIM.FRM_GENERICO_ESTADO
                                              join rd in dbSIM.RADICADO_DOCUMENTO on ge.CODRADICADO equals rd.ID_RADICADODOC
                                              where ge.CODTRAMITE == (int)tra && ge.ID_TERCERO == (int)ter
                                              select new datosRadicado { S_RADICADO = rd.S_RADICADO, D_FECHA = rd.D_RADICADO }).FirstOrDefault();

                    if (radicado == null)
                    {
                        radicado = (from ee in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                                    where ee.CODTRAMITE == (int)tra && ee.ID_TERCERO == (int)ter && ee.S_VALOR_VIGENCIA == v
                                    select new datosRadicado { S_RADICADO = ee.S_RADICADO, D_FECHA = ee.D_RADICADO }).FirstOrDefault();
                    }
                    else
                    {
                        medioEntregaSIM = true;
                    }

                    evaluacionEncuestaTercero = new EVALUACION_ENCUESTA_TERCERO();

                    evaluacionEncuestaTercero.ID_EVALUACION_TIPO = 1; // PMES
                    evaluacionEncuestaTercero.ID_TERCERO = (int)ter;
                    evaluacionEncuestaTercero.S_VALOR_VIGENCIA = v;
                    evaluacionEncuestaTercero.S_ESTADO = "P"; // P: Proceso, G: Generado
                    evaluacionEncuestaTercero.ID_ESTADO = Convert.ToInt32(estado);
                    evaluacionEncuestaTercero.CODTRAMITE = Convert.ToInt32(tra);
                    evaluacionEncuestaTercero.S_MEDIO_ENTREGA = medioEntregaSIM ? "S" : "T";
                    evaluacionEncuestaTercero.S_RADICADO = radicado.S_RADICADO;
                    evaluacionEncuestaTercero.D_FECHA_ENTREGA = radicado.D_FECHA;
                    evaluacionEncuestaTercero.S_CM = (cm == null ? "" : cm.CM ?? "");
                    evaluacionEncuestaTercero.N_COD_MUNICIPIO = (cm == null ? null : cm.N_COD_MUNICIPIO);

                    dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Added;

                    dbSIM.SaveChanges();
                }
            }

            var tercero = (from t in dbSIM.TERCERO
                            where t.ID_TERCERO == ter
                            select t.S_RSOCIAL).FirstOrDefault();

            ViewBag.IdEvaluacionEncuestaTercero = evaluacionEncuestaTercero.ID;
            ViewBag.IdTercero = (int)ter;
            ViewBag.Tercero = tercero;
            ViewBag.ValorVigencia = v;
            ViewBag.SoloLectura = (evaluacionEncuestaTercero.S_ESTADO == "G" ? "S" : "N");

            return View();
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult Evaluacion(int id)
        {
            ViewBag.IdTercero = id;

            return View();
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionEncuesta(int? idee, int? ideet, int? i)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta;

            if (idee != null)
            {
                evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID == idee
                                      select ee).FirstOrDefault();

                if (evaluacionEncuesta == null)
                    return null;
            }
            else
            {
                if (ideet != null && i != null)
                {
                    evaluacionEncuesta = Utilidades.GenerarEvaluacionEncuesta((int)ideet, (int)i, false, false, dbSIM);
                }
                else
                {
                    return null;
                }
            }

            ViewBag.IdEvaluacionEncuesta = evaluacionEncuesta.ID;

            dynamic datosVigencia = Utilidades.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);

            if (datosVigencia != null)
            {
                int idT = Convert.ToInt32(datosVigencia.ID_TERCERO);
                int idI = Convert.ToInt32(datosVigencia.ID_INSTALACION);

                var tercero = (from ter in dbSIM.TERCERO
                               where ter.ID_TERCERO == idT
                               select ter.S_RSOCIAL).FirstOrDefault();

                var instalacion = (from ins in dbSIM.INSTALACION
                                   where ins.ID_INSTALACION == idI
                                   select ins.S_NOMBRE).FirstOrDefault();

                ViewBag.Tercero = tercero;
                ViewBag.ValorVigencia = datosVigencia.VALOR_VIGENCIA;
                ViewBag.Instalacion = instalacion;

                ViewBag.MedioEntrega = (evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                ViewBag.Radicado = evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");
                ViewBag.Coordenada = evaluacionEncuesta.S_COORDENADA;
                ViewBag.Direccion = evaluacionEncuesta.S_DIRECCION;
                ViewBag.TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : "");
                ViewBag.KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : "");
                ViewBag.PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : "");
                ViewBag.PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : "");

                ViewBag.Resultado = evaluacionEncuesta.S_RESULTADO;

                var evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                 where eet.ID == evaluacionEncuesta.ID_EVALUACION_TERCERO
                                                 select eet).FirstOrDefault();

                ViewBag.SoloLectura = (evaluacionEncuestaTercero.S_ESTADO == "G" ? "S" : "N");

                return View();
            }
            else
            {
                return null;
            }
        }

        

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet, ActionName("GenerarDocumentoEvaluacion")]
        public ActionResult GetGenerarDocumentoEvaluacion(int eet)
        {
            var encuestaTercero = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                   where et.ID == eet
                                   select et).FirstOrDefault();

            if (encuestaTercero.S_ESTADO == "G")
                return null;

            int tipoOficio = 0; // 1 Aprobación, 2 Requerimiento

            var datosTerceroEvaluacion = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                join t in dbSIM.TERCERO on et.ID_TERCERO equals t.ID_TERCERO
                                join ee in dbSIM.EVALUACION_ENCUESTA on et.ID equals ee.ID_EVALUACION_TERCERO
                                join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                where et.ID == eet
                                orderby ee.S_PRINCIPAL descending
                                select new
                                {
                                    //S_REPRESENTANTE_LEGAL = t.CONTACTOS.Where(c => c.TIPO == "R").OrderBy(c => c.D_FIN).FirstOrDefault(),
                                    et.ID_TERCERO,
                                    N_DOCUMENTO = t.N_DOCUMENTON,
                                    t.S_RSOCIAL,
                                    i.ID_INSTALACION,
                                    et.S_RADICADO,
                                    et.D_FECHA_ENTREGA,
                                    et.S_CM,
                                    et.S_ESTADO,
                                    et.N_COD_MUNICIPIO
                                }).FirstOrDefault();

            if (datosTerceroEvaluacion == null || datosTerceroEvaluacion.S_ESTADO == "G")
                return null;

            var representanteLegal = (from c in dbSIM.CONTACTOS
                          where c.ID_JURIDICO == datosTerceroEvaluacion.ID_TERCERO
                          orderby c.D_INICIO descending
                          select c.TERCERO.S_RSOCIAL).FirstOrDefault();

            var instalacion = datosTerceroEvaluacion.ID_INSTALACION;

            var datosInstalacionPrincipal = dbSIM.Database.SqlQuery<datosInstalacion>("SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS S_DIRECCION, i.S_TELEFONO AS S_TELEFONO, d.S_NOMBRE AS S_MUNICIPIO FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION = " + instalacion.ToString()).FirstOrDefault();

            string sql = "SELECT ee.ID AS ID_EVALUACION_ENCUESTA, " +
                        "eet.ID AS ID_EVALUACION_TERCERO, " +
                        "t.ID_TERCERO, " +
                        "i.ID_INSTALACION, " +
                        "i.S_NOMBRE INSTALACION, " +
                        "CASE WHEN NVL(ee.S_ESTADO, 'R') = 'R' THEN 'P' ELSE ee.S_RESULTADO END RESULTADO " +
                        "FROM CONTROL.EVALUACION_ENCUESTA_TERCERO eet INNER JOIN " +
                        "  GENERAL.TERCERO t ON eet.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                        "  GENERAL.TERCERO_INSTALACION ti ON t.ID_TERCERO = ti.ID_TERCERO INNER JOIN " +
                        "  GENERAL.INSTALACION i ON ti.ID_INSTALACION = i.ID_INSTALACION LEFT OUTER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND i.ID_INSTALACION = ee.ID_INSTALACION " +
                        "WHERE eet.ID = :eet AND NVL(ee.S_EXCLUIR, 'N') = 'N' " +
                        "ORDER BY i.S_NOMBRE";

            OracleParameter ideetParameter = new OracleParameter("eet", eet);

            var instalacionesEvaluacion = dbSIM.Database.SqlQuery<encuestasInstalaciones>(sql, new object[] { ideetParameter }).ToList();

            {
                encuestasInstalaciones instalacionFaltante;

                instalacionFaltante = instalacionesEvaluacion.Where(i => i.RESULTADO == "P").FirstOrDefault();

                if (instalacionFaltante != null)
                {
                    return null;
                }

                instalacionFaltante = instalacionesEvaluacion.Where(i => i.RESULTADO == "N").FirstOrDefault();

                if (instalacionFaltante == null)
                {
                    tipoOficio = 1;
                }
                else
                {
                    tipoOficio = 2;
                }
            }

            if (tipoOficio == 0)
                return null;

            var stream = new MemoryStream();

            DocX documentoEncabezado = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Encabezado.docx"));

            //if (datosTerceroEvaluacion.S_REPRESENTANTE_LEGAL != null)
            if (representanteLegal != null)
                documentoEncabezado.ReplaceText("{REPRESENTANTE LEGAL}", representanteLegal);
            documentoEncabezado.ReplaceText("{TERCERO}", datosTerceroEvaluacion.S_RSOCIAL.ToString());
            documentoEncabezado.ReplaceText("{DIRECCION}", datosInstalacionPrincipal.S_DIRECCION);
            documentoEncabezado.ReplaceText("{TELEFONO}", datosInstalacionPrincipal.S_TELEFONO);
            documentoEncabezado.ReplaceText("{MUNICIPIO}", datosInstalacionPrincipal.S_MUNICIPIO);

            CultureInfo esCO = new CultureInfo("es-CO");

            documentoEncabezado.ReplaceText("{RADICADO}", datosTerceroEvaluacion.S_RADICADO.ToString() + " del " + Convert.ToDateTime(datosTerceroEvaluacion.D_FECHA_ENTREGA).ToString("dd \\de MMMM \\de yyyy", esCO));

            foreach (encuestasInstalaciones instalacionEvaluacion in instalacionesEvaluacion)
            {
                var encuestaInstalacion = (from ee in dbSIM.EVALUACION_ENCUESTA
                                          where ee.ID == instalacionEvaluacion.ID_EVALUACION_ENCUESTA
                                          select ee).FirstOrDefault();

                DocX documentoEvaluacion = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Evaluacion.docx"));

                documentoEvaluacion.ReplaceText("{INSTALACION}", instalacionEvaluacion.INSTALACION);

                documentoEvaluacion.ReplaceText("{CUMPLE}", encuestaInstalacion.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE");
                documentoEvaluacion.ReplaceText("{CO2_TON}", Convert.ToDecimal(encuestaInstalacion.N_CO2P).ToString("#,##0.0000"));
                documentoEvaluacion.ReplaceText("{CO2_KG}", Convert.ToDecimal(encuestaInstalacion.N_CO2I).ToString("#,##0.0000"));

                documentoEvaluacion.ReplaceText("{OBSERVACIONES}", encuestaInstalacion.S_OBSERVACIONES ?? "--------");

                var preguntasEvaluacion = from pe in dbSIM.EVALUACION_PREGUNTA
                                          join
                                              er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA == instalacionEvaluacion.ID_EVALUACION_ENCUESTA) on pe.ID equals er.ID_PREGUNTA into perj
                                          from per in perj.DefaultIfEmpty()
                                          orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                          select new
                                          {
                                              ID = per == null ? 0 : per.ID,
                                              ID_PREGUNTA = pe.ID,
                                              S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                              S_PREGUNTA = pe.S_DESCRIPCION,
                                              N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                              S_RESPUESTA = (pe.N_TIPO_RESPUESTA == 1 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "SI" : "NO") : (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "CUMPLE" : "NO CUMPLE") : (pe.N_TIPO_RESPUESTA == 3 && per.N_RESPUESTA != null ? per.N_RESPUESTA.ToString() : (pe.N_TIPO_RESPUESTA == 4 && per.N_RESPUESTA != null ? (per.N_RESPUESTA * 100).ToString() + " %" : (pe.N_TIPO_RESPUESTA == 5 && per.N_RESPUESTA != null) ? per.S_RESPUESTA : null))))
                                          };

                for (int idPregunta = 1; idPregunta <= 59; idPregunta++)
                {
                    var respuesta = preguntasEvaluacion.Where(pe => pe.ID_PREGUNTA == idPregunta).FirstOrDefault();

                    if (respuesta != null && respuesta.S_RESPUESTA != null)
                        documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", respuesta.S_RESPUESTA);
                    else
                        documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", "-");
                }

                documentoEncabezado.InsertDocument(documentoEvaluacion, true);

                documentoEvaluacion.Dispose();
            }
            
            DocX documentoFinal = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Final" + (tipoOficio == 1 ? "Aprobacion" : "Requerimiento") + ".docx"));

            //if (datosTerceroEvaluacion.S_REPRESENTANTE_LEGAL != null)
            if (representanteLegal != null)
                documentoFinal.ReplaceText("{REPRESENTANTE LEGAL}", representanteLegal);
            documentoFinal.ReplaceText("{TERCERO}", datosTerceroEvaluacion.S_RSOCIAL.ToString());
            documentoFinal.ReplaceText("{NIT}", datosTerceroEvaluacion.N_DOCUMENTO.ToString());
            documentoFinal.ReplaceText("{DIRECCION}", datosInstalacionPrincipal.S_DIRECCION);
            documentoFinal.ReplaceText("{MUNICIPIO}", datosInstalacionPrincipal.S_MUNICIPIO);
            if (datosTerceroEvaluacion.S_CM != null)
                documentoFinal.ReplaceText("{CM}", datosTerceroEvaluacion.S_CM);
            if (datosTerceroEvaluacion.N_COD_MUNICIPIO != null)
                documentoFinal.ReplaceText("{COD MUNICIPIO}", ((int)datosTerceroEvaluacion.N_COD_MUNICIPIO).ToString("00"));

            int idUsuario = 0;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            if (idUsuario > 0)
            {
                var usuario = (from u in dbSIM.USUARIO
                               where u.ID_USUARIO == idUsuario
                               select u).FirstOrDefault();

                if (usuario != null)
                    documentoFinal.ReplaceText("{USUARIO GENERACION}", usuario.S_NOMBRES + " " + usuario.S_APELLIDOS);
            }
            else
            {
                return null;
            }

            documentoFinal.ReplaceText("{TRAMITE}", encuestaTercero.CODTRAMITE.ToString());

            documentoEncabezado.InsertDocument(documentoFinal, true);

            documentoEncabezado.SaveAs(stream);

            documentoFinal.Dispose();
            documentoEncabezado.Dispose();

            string nombreArchivo;

            if (tipoOficio == 1)
                nombreArchivo = "Oficio Aprobación Plan MES.docx";
            else
                nombreArchivo = "Oficio Requerimiento Plan MES.docx";

            // Se almacena en los documentos temporales
            var tramite = (from tr in dbSIM.TBTRAMITE
                           where tr.CODTRAMITE == encuestaTercero.CODTRAMITE
                           select tr).FirstOrDefault();

            

            
            TBRUTAPROCESO rutaProceso = dbSIM.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == tramite.CODPROCESO).FirstOrDefault();

            string pathDocumentosTemporalesTramite = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(encuestaTercero.CODTRAMITE), 100);
            string ruta = pathDocumentosTemporalesTramite + encuestaTercero.CODTRAMITE.ToString("0") + "_" + nombreArchivo;
            
            if (!Directory.Exists(pathDocumentosTemporalesTramite))
                Directory.CreateDirectory(pathDocumentosTemporalesTramite);

            using (System.IO.FileStream docTemporal = new System.IO.FileStream(ruta, FileMode.Create))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(docTemporal);
                docTemporal.Close();
            }

            codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbSIM, idUsuario);

            decimal versionActual = -1;

            ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
            dbSIM.SP_ASIGNAR_TEMPORAL_TRAMITE(encuestaTercero.CODTRAMITE, 4545, codFuncionario, -1, versionActual, Path.GetFileNameWithoutExtension(nombreArchivo), ruta, rtaResultado);

            encuestaTercero.S_ESTADO = "G";
            encuestaTercero.S_RESULTADO = (tipoOficio == 1 ? "C" : "N");
            encuestaTercero.ID_USUARIO_GENERACION = idUsuario;
            encuestaTercero.D_FECHA_GENERACION = DateTime.Now;
            dbSIM.Entry(encuestaTercero).State = EntityState.Modified;

            try
            {
                dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
                throw error;
            }

            return File(stream.GetBuffer(), "application/docx", nombreArchivo);
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet, ActionName("GenerarDocumentoEvaluacionANT")]
        public ActionResult GetGenerarDocumentoEvaluacionANT(int ee)
        {
            var reporteEvaluacion = new EvaluacionEncuesta();

            var preguntasEvaluacion = from pe in dbSIM.EVALUACION_PREGUNTA
                                      join
                                          er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA == ee) on pe.ID equals er.ID_PREGUNTA into perj
                                      from per in perj.DefaultIfEmpty()
                                      orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                      select new
                                      {
                                          ID = per == null ? 0 : per.ID,
                                          ID_PREGUNTA = pe.ID,
                                          S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                          S_PREGUNTA = pe.S_DESCRIPCION,
                                          N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                          S_RESPUESTA = (pe.N_TIPO_RESPUESTA == 1 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "SI" : "NO") : (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "CUMPLE" : "NO CUMPLE") : (pe.N_TIPO_RESPUESTA == 3 && per.N_RESPUESTA != null ? per.N_RESPUESTA.ToString() : (pe.N_TIPO_RESPUESTA == 4 && per.N_RESPUESTA != null ? (per.N_RESPUESTA * 100).ToString() + " %" : (pe.N_TIPO_RESPUESTA == 5 && per.N_RESPUESTA != null) ? per.S_RESPUESTA : null))))
                                      };


            reporteEvaluacion.CargarDatos(preguntasEvaluacion.ToList());

            MemoryStream ms = new MemoryStream();
            reporteEvaluacion.ExportToRtf(ms);
            //reporteEvaluacion.ExportToPdf(ms);
            return File(ms.GetBuffer(), "application/rtf", "reporte.rtf");
            //return File(ms.GetBuffer(), "application/pdf", "reporte.pdf");
        }*/
    }
}