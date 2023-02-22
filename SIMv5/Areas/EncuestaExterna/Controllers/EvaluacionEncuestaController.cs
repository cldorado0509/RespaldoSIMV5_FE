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
using SIM.Areas.EncuestaExterna.Models;
using System.Data.Entity;
using SIM.Areas.EncuestaExterna.Reporte;
using System.Data.Entity.SqlServer;
using Novacode;
using Oracle.ManagedDataAccess.Client;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class EvaluacionEncuesta : Controller
    {
        SIM.Areas.Models.EntitiesSIMOracle dbSIM = new SIM.Areas.Models.EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        decimal idTerceroUsuario;

        //[Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionEncuestaTerceros()
        {
            return View();
        }

        // id: Id Tercero
        // v: Vigencia
        //[Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionEncuestaTercero(int? id, int? ter, string v, int? tra)
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
                    datosRadicado radicado = (from ee in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                                              where ee.CODTRAMITE == (int)tra && ee.ID_TERCERO == (int)ter && ee.S_VALOR_VIGENCIA == v
                                              select new datosRadicado { S_RADICADO = ee.S_RADICADO, D_FECHA = ee.D_RADICADO }).FirstOrDefault();

                    evaluacionEncuestaTercero = new EVALUACION_ENCUESTA_TERCERO();

                    evaluacionEncuestaTercero.ID_EVALUACION_TIPO = 1; // PMES
                    evaluacionEncuestaTercero.ID_TERCERO = (int)ter;
                    evaluacionEncuestaTercero.S_VALOR_VIGENCIA = v;
                    evaluacionEncuestaTercero.S_ESTADO = "P"; // P: Proceso, G: Generado
                    evaluacionEncuestaTercero.ID_ESTADO = Convert.ToInt32(estado);
                    evaluacionEncuestaTercero.CODTRAMITE = Convert.ToInt32(tra);
                    evaluacionEncuestaTercero.S_MEDIO_ENTREGA = "S";
                    evaluacionEncuestaTercero.S_RADICADO = radicado.S_RADICADO;
                    evaluacionEncuestaTercero.D_FECHA_ENTREGA = radicado.D_FECHA;

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

            return View();
        }

        //[Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionEncuesta(int id)
        {
            ViewBag.IdTercero = id;

            return View();
        }

        //[Authorize(Roles = "VPMESEVALUACION")]
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

                /*ViewBag.MedioEntrega = (evaluacionEncuesta.S_MEDIO_ENTREGA == "S" ? "SIM" : "TAQUILLA");
                ViewBag.Radicado = evaluacionEncuesta.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuesta.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuesta.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");*/
                ViewBag.Coordenada = evaluacionEncuesta.S_COORDENADA;
                ViewBag.Direccion = evaluacionEncuesta.S_DIRECCION;
                ViewBag.TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : "");
                ViewBag.KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : "");
                ViewBag.PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : "");
                ViewBag.PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : "");

                ViewBag.Resultado = evaluacionEncuesta.S_RESULTADO;

                return View();
            }
            else
            {
                return null;
            }
        }

        

        //[Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet, ActionName("GenerarDocumentoEvaluacionEncuesta")]
        public ActionResult GetGenerarDocumentoEvaluacion(int eet)
        {
            int tipoOficio = 0; // 1 Aprobación, 2 Requerimiento

            var datosTerceroEvaluacion = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                join t in dbSIM.TERCERO on et.ID_TERCERO equals t.ID_TERCERO
                                where et.ID == eet
                                select new
                                {
                                    S_REPRESENTANTE_LEGAL = t.CONTACTOS.Where(c => c.D_FIN == null && c.TIPO == "R"),
                                    t.S_RSOCIAL
                                }).FirstOrDefault();

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

            foreach (encuestasInstalaciones instalacionEvaluacion in instalacionesEvaluacion)
            {
                var encuestaInstalacion = (from ee in dbSIM.EVALUACION_ENCUESTA
                                          where ee.ID == instalacionEvaluacion.ID_EVALUACION_ENCUESTA
                                          select ee).FirstOrDefault();

                DocX documentoEvaluacion = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Evaluacion.docx"));

                documentoEvaluacion.ReplaceText("{CUMPLE}", encuestaInstalacion.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE");
                documentoEvaluacion.ReplaceText("{CO2_TON}", Convert.ToDecimal(encuestaInstalacion.N_CO2P).ToString("#,##0.0000"));
                documentoEvaluacion.ReplaceText("{CO2_KG}", Convert.ToDecimal(encuestaInstalacion.N_CO2I).ToString("#,##0.0000"));

                documentoEvaluacion.ReplaceText("{OBSERVACIONES}", encuestaInstalacion.S_OBSERVACIONES ?? "[Sin Observaciones]");

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

            documentoEncabezado.InsertDocument(documentoFinal, true);

            documentoEncabezado.SaveAs(stream);

            documentoFinal.Dispose();
            documentoEncabezado.Dispose();

            string nombreArchivo;

            if (tipoOficio == 1)
                nombreArchivo = "Oficio Aprobación Plan MES.docx";
            else
                nombreArchivo = "Oficio Requerimiento Plan MES.docx";

            return File(stream.GetBuffer(), "application/docx", nombreArchivo);
        }
    }
}