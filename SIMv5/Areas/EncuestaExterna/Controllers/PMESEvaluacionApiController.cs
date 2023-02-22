using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Models;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;
using System.Security.Claims;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SIM.Utilidades;
using System.Globalization;
using System.Text;
using System.Web.Hosting;
using Oracle.ManagedDataAccess.Client;
using SIM.Data;
using SIM.Data.Control;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class PMESEvaluacionApiController : ApiController
    {
        public partial class ESTRATEGIA
        {
            public int ID { get; set; }
            public int ID_EVALUACION_TERCERO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public int ID_GRUPO { get; set; }
            public string S_GRUPO { get; set; }
            public string S_ESTRATEGIA { get; set; }
            public string S_OTRO { get; set; }
            public string S_INDICADOR_MEDICION { get; set; }
            public string S_UNIDADES_META { get; set; }
            public string S_UNIDADES_META_NOMBRE { get; set; }
            public decimal N_VALOR_META { get; set; }
            public decimal N_PRESUPUESTO { get; set; }
            public string S_TIPO { get; set; }
            public decimal? N_VALOR_META_ALCANZAR { get; set; }
        }

        public class DATOSENCUESTADIRECCION
        {
            public string S_DIRECCION { get; set; }
            public string X_VALOR { get; set; }
            public string Y_VALOR { get; set; }
        }

        public class DATOSRADICADO
        {
            public string S_RADICADO { get; set; }
            public DateTime? D_FECHA { get; set; }
        }

        public struct ACTUALIZACIONPMES
        {
            public int ID { get; set; }
            public int ID_ENCUESTA { get; set; }
            public int ID_TERCERO { get; set; }
            public int ID_INSTALACION { get; set; }
            public string S_VALOR_VIGENCIA { get; set; }
        }

        public class ENCUESTASINSTALACIONES
        {
            public int? ID_EVALUACION_ENCUESTA { get; set; }
            public int? ID_EVALUACION_TERCERO { get; set; }
            public int ID_TERCERO { get; set; }
            public int ID_INSTALACION { get; set; }
            public string INSTALACION { get; set; }
            public string ENC_SITIO { get; set; }
            public string ENC_TRABAJADORES { get; set; }
            public string RESULTADO { get; set; }
            public string S_EXCLUIR { get; set; }
            public string S_PRINCIPAL { get; set; }
        }

        public class OPCIONESDETALLERESPUESTA
        {
            public int ID { get; set; }
            public string S_DESCRIPCION { get; set; }
            public int N_ORDEN { get; set; }
            public int SELECCIONADO { get; set; }
        }

        public class OPCIONESDETALLERESPUESTAINGRESADO
        {
            public int Id { get; set; }
            public string Opciones { get; set; }
            public string Texto { get; set; }
        }

        public struct DATOSCONSULTA
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct DATOSRESPUESTA
        {
            public int codigoRespuesta;
            public string tipoRespuesta; // OK, Error, Ya Tiene Tercero, 
            public string detalleRespuesta;
        }

        public struct DATOSREGISTRO
        {
            public int idEncuestaEvaluacion;
            public int id;
            public int idPregunta;
            public int tipoRespuesta;
            public string valor;
        }

        public struct DATOSOBSERVACIONES
        {
            public int idEncuestaEvaluacion;
            public string observaciones;
        }

        public class DATOSREGISTRORESPUESTA
        {
            public int ID { get; set; }
            public int ID_EVALUACION_ENCUESTA { get; set; }
            public int N_ORDENGRUPO { get; set; }
            public int? N_ORDEN { get; set; }
            public int ID_PREGUNTA { get; set; }
            public string S_GRUPO { get; set; }
            public string S_GRUPO_INSTALACION { get; set; }
            public string S_TIPO { get; set; }
            public string S_PREGUNTA { get; set; }
            public int N_TIPO_RESPUESTA { get; set; }
            public decimal? N_RESPUESTA { get; set; }
            public string S_RESPUESTA { get; set; }
            public string S_CALCULADO { get; set; }
            public int? N_VALOR_COMPLEMENTO { get; set; }
            public int? ID_GRUPO_COMPLEMENTO { get; set; }
            public int COLOR { get; set; }
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("EvaluacionTerceros")]
        public DATOSCONSULTA GetEvaluacionTerceros(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, string version, int ?tipo, int? copia)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            dynamic modelData;

            tipo = tipo ?? 1;
            copia = copia ?? 0;

            string esCopia = (copia == 1 ? "S" : "N");

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XPMESEVALUACION");
            }

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                if (administrador)
                {
                    var model = (from PMESEvaluacion in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                                 where PMESEvaluacion.ID_USUARIO == idUsuario && (PMESEvaluacion.S_VERSION == "0" || PMESEvaluacion.S_VERSION == version) && PMESEvaluacion.ID_EVALUACION_TIPO == tipo && PMESEvaluacion.S_COPIA == esCopia
                                 orderby PMESEvaluacion.S_TERCERO, PMESEvaluacion.S_VALOR_VIGENCIA
                                 select PMESEvaluacion);

                    modelData = model;
                }
                else
                {
                    var model = (from PMESEvaluacion in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                                 where PMESEvaluacion.ID_USUARIO == idUsuario && (PMESEvaluacion.S_VERSION == "0" || PMESEvaluacion.S_VERSION == version) && PMESEvaluacion.ID_EVALUACION_TIPO == tipo && PMESEvaluacion.S_COPIA == esCopia
                                 orderby PMESEvaluacion.S_TERCERO, PMESEvaluacion.S_VALOR_VIGENCIA
                                 select PMESEvaluacion);

                    modelData = model;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PendientesAvanzar")]
        public DATOSCONSULTA GetPendientesAvanzar(string sort, int skip, int take)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            dynamic modelData;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XPMESEVALUACION");
            }

            if (administrador)
            {
                var model = (from pe in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                             join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on pe.CODTRAMITE equals eet.CODTRAMITE
                             join t in dbSIM.TERCERO on eet.ID_TERCERO equals t.ID_TERCERO
                             join tr in dbSIM.TBTRAMITE on eet.CODTRAMITE equals tr.CODTRAMITE
                             where eet.S_ESTADO == "G"
                             orderby t.S_RSOCIAL, eet.S_VALOR_VIGENCIA
                             select new
                             {
                                 ID_EET = eet.ID,
                                 eet.ID_TERCERO,
                                 CM = eet.S_CM,
                                 eet.CODTRAMITE,
                                 N_DOCUMENTO = t.N_DOCUMENTON,
                                 S_TERCERO = t.S_RSOCIAL,
                                 S_VALOR_VIGENCIA = eet.S_VALOR_VIGENCIA,
                                 S_ASUNTO = tr.MENSAJE,
                                 eet.D_FECHA_GENERACION,
                                 S_RESULTADO = (eet.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE")
                             });


                modelData = model;
            }
            else
            {
                var model = (from pe in dbSIM.VW_PMES_EVALUACION_EMPRESAS
                             join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on pe.CODTRAMITE equals eet.CODTRAMITE
                             join t in dbSIM.TERCERO on eet.ID_TERCERO equals t.ID_TERCERO
                             join tr in dbSIM.TBTRAMITE on eet.CODTRAMITE equals tr.CODTRAMITE
                             where eet.ID_USUARIO_GENERACION == idUsuario
                             where eet.S_ESTADO == "G"
                             orderby t.S_RSOCIAL, eet.S_VALOR_VIGENCIA
                             select new
                             {
                                 ID_EET = eet.ID,
                                 eet.ID_TERCERO,
                                 CM = eet.S_CM,
                                 eet.CODTRAMITE,
                                 N_DOCUMENTO = t.N_DOCUMENTON,
                                 S_TERCERO = t.S_RSOCIAL,
                                 S_VALOR_VIGENCIA = eet.S_VALOR_VIGENCIA,
                                 S_ASUNTO = tr.MENSAJE,
                                 eet.D_FECHA_GENERACION,
                                 S_RESULTADO = (eet.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE")
                             });


                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, "", sort, "");

            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PlanesHistoricoEvaluacion")]
        public DATOSCONSULTA GetPlanesHistoricoEvaluacion(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            dynamic modelData;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XPMESEVALUACION");
            }

            if (administrador)
            {
                var model = (from pe in dbSIM.VW_PMES_EVALUACION_EMPRESAS_H
                             join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on pe.ID_EET equals eet.ID
                             where eet.S_ESTADO == "G"
                             orderby pe.S_TERCERO, pe.S_VALOR_VIGENCIA
                             select new
                             {
                                 pe.ID_EET,
                                 pe.ID_TERCERO,
                                 pe.CM,
                                 pe.CODTRAMITE,
                                 pe.N_DOCUMENTO,
                                 pe.S_TERCERO,
                                 pe.S_VALOR_VIGENCIA,
                                 pe.S_ASUNTO,
                                 eet.D_FECHA_GENERACION,
                                 S_RESULTADO = (eet.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE"),
                                 N_ANO = ((DateTime)eet.D_FECHA_GENERACION).Year,
                                 N_MES = ((DateTime)eet.D_FECHA_GENERACION).Month,
                                 N_DIA = ((DateTime)eet.D_FECHA_GENERACION).Day
                             });


                modelData = model;
            }
            else
            {
                var model = (from pe in dbSIM.VW_PMES_EVALUACION_EMPRESAS_H
                             join eet in dbSIM.EVALUACION_ENCUESTA_TERCERO on pe.ID_EET equals eet.ID
                             where eet.S_ESTADO == "G" && eet.ID_USUARIO_GENERACION == idUsuario
                             orderby pe.S_TERCERO, pe.S_VALOR_VIGENCIA
                             select new
                             {
                                 pe.ID_EET,
                                 pe.ID_TERCERO,
                                 pe.CM,
                                 pe.CODTRAMITE,
                                 pe.N_DOCUMENTO,
                                 pe.S_TERCERO,
                                 pe.S_VALOR_VIGENCIA,
                                 pe.S_ASUNTO,
                                 eet.D_FECHA_GENERACION,
                                 S_RESULTADO = (eet.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE"),
                                 N_ANO = ((DateTime)eet.D_FECHA_GENERACION).Year,
                                 N_MES = ((DateTime)eet.D_FECHA_GENERACION).Month,
                                 N_DIA = ((DateTime)eet.D_FECHA_GENERACION).Day
                             });


                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("EncuestasInstalacionesTercero")]
        public DATOSCONSULTA GetEncuestasInstalacionesTercero(int idTercero, string valorVigencia, string v, string t)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            dynamic modelData;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            string sql = "SELECT ee.ID AS ID_EVALUACION_ENCUESTA, " +
                        "eet.ID AS ID_EVALUACION_TERCERO, " +
                        "t.ID_TERCERO, " +
                        "i.ID_INSTALACION, " +
                        "i.S_NOMBRE INSTALACION, " +
                        "( " +
                        "  SELECT CASE  " +
                        "      WHEN COUNT(gei.ID_ESTADO) = 0 " +
                        "      THEN 'NO EXISTE' " +
                        "      WHEN COUNT(gei.ID_ESTADO) = SUM(NVL(gei.TIPO_GUARDADO, 0)) " +
                        "      THEN 'OK' " +
                        "      ELSE 'SIN ENVIAR' " +
                        "    END AS ENVIADO " +
                        "  FROM CONTROL.VIGENCIA_SOLUCION vsi INNER JOIN  " +
                        "    CONTROL.FRM_GENERICO_ESTADO gei ON vsi.ID_ESTADO = gei.ID_ESTADO AND gei.ID_INSTALACION = i.ID_INSTALACION AND gei.ACTIVO = 0 " +
                        "  WHERE vsi.ID_VIGENCIA IN (701, 1081) AND vsi.VALOR = :valorVigencia " +
                        ") ENC_SITIO, " +
                        "( " +
                        "  SELECT CASE  " +
                        "      WHEN COUNT(gei.ID_ESTADO) = 0 " +
                        "      THEN 'NO EXISTE' " +
                        "      WHEN COUNT(gei.ID_ESTADO) = SUM(NVL(gei.TIPO_GUARDADO, 0)) " +
                        "      THEN 'OK' " +
                        "      ELSE 'SIN ENVIAR' " +
                        "    END AS ENVIADO " +
                        "  FROM CONTROL.VIGENCIA_SOLUCION vsi INNER JOIN  " +
                        "    CONTROL.FRM_GENERICO_ESTADO gei ON vsi.ID_ESTADO = gei.ID_ESTADO AND gei.ID_INSTALACION = i.ID_INSTALACION AND gei.ACTIVO = 0 " +
                        "  WHERE vsi.ID_VIGENCIA IN (681, 1141, 1320) AND vsi.VALOR = :valorVigencia " +
                        ") ENC_TRABAJADORES, " +
                        "CASE WHEN NVL(ee.S_ESTADO, 'R') = 'R' THEN 'PENDIENTE' ELSE CASE WHEN ee.S_RESULTADO = 'C' THEN 'CUMPLE' ELSE 'NO CUMPLE' END END RESULTADO, " +
                        "NVL(ee.S_EXCLUIR, 'N') S_EXCLUIR, " +
                        "NVL(ee.S_PRINCIPAL, 'N') S_PRINCIPAL " +
                        "FROM GENERAL.TERCERO t INNER JOIN " +
                        "  GENERAL.TERCERO_INSTALACION ti ON t.ID_TERCERO = ti.ID_TERCERO INNER JOIN " +
                        "  GENERAL.INSTALACION i ON ti.ID_INSTALACION = i.ID_INSTALACION LEFT OUTER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA_TERCERO eet ON eet.ID_EVALUACION_TIPO = " + (t == "2" ? "2" : "1") + " AND t.ID_TERCERO = eet.ID_TERCERO AND eet.S_VALOR_VIGENCIA = :valorVigencia LEFT OUTER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND i.ID_INSTALACION = ee.ID_INSTALACION " +
                        "WHERE t.ID_TERCERO = :idTercero ";

            if (v == "2" || t == "2")
            {
                sql += "UNION ALL " +
                    "SELECT -1 AS ID_EVALUACION_ENCUESTA, " +
                    "   eet.ID AS ID_EVALUACION_TERCERO, " +
                    "   -1 AS ID_TERCERO, " +
                    "   -1 AS ID_INSTALACION, " +
                    "   'EVALUACIÓN EMPRESA' INSTALACION, " +
                    "   '' AS ENC_SITIO, " +
                    "   '' AS ENC_TRABAJADORES, " +
                    "   CASE eet.S_RESULTADO WHEN 'P' THEN 'PENDIENTE' WHEN 'C' THEN 'CUMPLE' ELSE 'NO CUMPLE' END RESULTADO, " +
                    "   '' AS S_EXCLUIR, " +
                    "   '' AS S_PRINCIPAL " +
                    "FROM GENERAL.TERCERO t INNER JOIN " +
                    "  CONTROL.EVALUACION_ENCUESTA_TERCERO eet ON eet.ID_EVALUACION_TIPO = " + (t == "2" ? "2" : "1") + " AND t.ID_TERCERO = eet.ID_TERCERO AND eet.S_VALOR_VIGENCIA = :valorVigencia " +
                    "WHERE t.ID_TERCERO = :idTercero ";
            }

            sql += "ORDER BY ID_TERCERO DESC, INSTALACION ASC";

            OracleParameter idTerceroParameter = new OracleParameter("idTercero", idTercero);
            OracleParameter valorVigenciaParameter = new OracleParameter("valorVigencia", valorVigencia);

            var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { valorVigenciaParameter, valorVigenciaParameter, valorVigenciaParameter, idTerceroParameter }).ToList();

            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = instalacionesEvaluacion.Count();
                if (resultado.numRegistros == 0)
                    resultado.datos = null;
                else
                    resultado.datos = (IEnumerable<dynamic>)instalacionesEvaluacion.AsEnumerable();

                return resultado;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("ExcluirInstalacion")]
        public void GetExcluirInstalacion(int? idee, int? ideet, int? i, bool valor)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta = null;

            var evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID == ideet
                                             select eet).FirstOrDefault();

            if (evaluacionEncuestaTercero != null && evaluacionEncuestaTercero.S_ESTADO == "G")
                return;

            if (idee != null)
            {
                evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID == idee
                                      select ee).FirstOrDefault();

                evaluacionEncuesta.S_EXCLUIR = valor ? "S" : "N";

                if (valor)
                {
                    evaluacionEncuesta.S_PRINCIPAL = "N";
                }

                dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }
            else
            {
                if (ideet != null && i != null)
                {
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, (int)i, valor, false, dbSIM);
                }
            }

            EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);
        }

        /*
        private EVALUACION_ENCUESTA GenerarEvaluacionEncuesta(int ideet, int i)
        {
            //dbSIM.EVALUACION_ENCUESTA_TERCERO

            var poblacion = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                             join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                             where vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                             select vpmes.N_POBLACION).FirstOrDefault();

            var co2 = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                       join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                       where vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                       select vpmes.N_CO2P).Sum();

            var pm25 = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                        join vpmes in dbSIM.VWM_PMES on ee.ID_TERCERO equals vpmes.ID_TERCERO
                        where vpmes.ID_INSTALACION == i && vpmes.VIGENCIA == ee.S_VALOR_VIGENCIA
                        select vpmes.N_PM25P).Sum();

            var valorVigencia = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                 where ee.ID == ideet
                                 select ee.S_VALOR_VIGENCIA).FirstOrDefault();

            OracleParameter idInstalacionParameter = new OracleParameter("idInstalacion", i);
            OracleParameter valorVigenciaParameter = new OracleParameter("valorVigencia", valorVigencia);

            datosEncuestaDireccion direccion = dbSIM.Database.SqlQuery<datosEncuestaDireccion>("SELECT sp.S_VALOR AS S_DIRECCION, sp.X_VALOR, sp.Y_VALOR " +
                                                                "FROM CONTROL.ENC_SOLUCION_PREGUNTAS sp INNER JOIN " +
                                                                "  CONTROL.ENC_SOLUCION s ON sp.ID_SOLUCION = s.ID_SOLUCION INNER JOIN " +
                                                                "  CONTROL.VIGENCIA_SOLUCION vs ON s.ID_ESTADO = vs.ID_ESTADO INNER JOIN " +
                                                                "  CONTROL.FRM_GENERICO_ESTADO ge ON s.ID_ESTADO = ge.ID_ESTADO " +
                                                                "WHERE sp.ID_PREGUNTA = 552 AND ge.ID_INSTALACION = :idInstalacion AND vs.VALOR = :valorVigencia", new OracleParameter[] { idInstalacionParameter, valorVigenciaParameter }).FirstOrDefault();

            var evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID_EVALUACION_TERCERO == ideet && ee.ID_INSTALACION == i
                                      select ee).FirstOrDefault();

            if (evaluacionEncuesta == null) // No existe la evaluación de la encuesta, por lo tanto se crea
            {
                evaluacionEncuesta = new EVALUACION_ENCUESTA();
                evaluacionEncuesta.ID_EVALUACION_TERCERO = (int)ideet;
                evaluacionEncuesta.ID_INSTALACION = (int)i;
                evaluacionEncuesta.S_ESTADO = "R"; // R: Registrado, G: Generado
                evaluacionEncuesta.S_RESULTADO = "P"; // P-Pendiente, C - Cumplió, N - No Cumplió

                if (direccion != null)
                {
                    if (direccion.X_VALOR != null && direccion.Y_VALOR != null)
                        evaluacionEncuesta.S_COORDENADA = direccion.X_VALOR.ToString().Replace(",", ".") + " | " + direccion.Y_VALOR.ToString().Replace(",", ".");

                    evaluacionEncuesta.S_DIRECCION = direccion.S_DIRECCION;
                }

                if (co2 != null)
                {
                    evaluacionEncuesta.N_CO2P = co2;

                    if (poblacion != null && poblacion > 0)
                        evaluacionEncuesta.N_CO2I = (co2 / poblacion) * 1000;
                }

                if (pm25 != null)
                {
                    evaluacionEncuesta.N_PM25P = pm25;

                    if (poblacion != null && poblacion > 0)
                        evaluacionEncuesta.N_PM25I = (pm25 / poblacion);
                }

                dbSIM.Entry(evaluacionEncuesta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }

            return evaluacionEncuesta;
        }*/

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("InstalacionPrincipal")]
        public void GetInstalacionPrincipal(int? idee, int? ideet, int? i, bool valor)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta = null;

            var evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID == ideet
                                             select eet).FirstOrDefault();

            if (evaluacionEncuestaTercero != null && evaluacionEncuestaTercero.S_ESTADO == "G")
                return;

            if (idee != null)
            {
                evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID == idee
                                      select ee).FirstOrDefault();

                evaluacionEncuesta.S_PRINCIPAL = valor ? "S" : "N";

                if (valor)
                {
                    var evaluacionEncuestaPrincipal = (from ee in dbSIM.EVALUACION_ENCUESTA
                                                       where ee.ID_EVALUACION_TERCERO == ideet && ee.S_PRINCIPAL == "S"
                                                       select ee).FirstOrDefault();

                    if (evaluacionEncuestaPrincipal != null)
                    {
                        evaluacionEncuestaPrincipal.S_PRINCIPAL = "N";

                        dbSIM.Entry(evaluacionEncuestaPrincipal).State = EntityState.Modified;

                        dbSIM.SaveChanges();
                    }
                }

                dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }
            else
            {
                if (ideet != null && i != null)
                {
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, (int)i, false, valor, dbSIM);
                }
            }

            EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("CumplimientoContenido")]
        public string GetCumplimientoContenido(int idee, string v)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return null;
            }

            var resultado = ResultadoEncuesta(idee, (v == null || v.Trim() == "" ? "1" : v));

            EVALUACION_ENCUESTA evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                                      where ee.ID == idee
                                                      select ee).FirstOrDefault();

            if (evaluacionEncuesta != null)
            {
                evaluacionEncuesta.S_RESULTADO = resultado;
                evaluacionEncuesta.S_ESTADO = "G";
                evaluacionEncuesta.ID_USUARIO_GENERACION = idUsuario;
                evaluacionEncuesta.D_FECHA_GENERACION = DateTime.Now;

                dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();

                return resultado;
            }
            else
            {
                return "P";
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("CumplimientoContenidoT")]
        public string GetCumplimientoContenidoT(int ideet)
        {
            DateTime fechaActual = DateTime.Now;
            string resultado;
            string resultadoFinal = "C";
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return null;
            }

            resultadoFinal = ResultadoEncuestaT(ideet, "2");

            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                                     where eet.ID == ideet
                                                                     select eet).FirstOrDefault();

            evaluacionEncuestaTercero.S_RESULTADO = resultadoFinal;

            dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Modified;

            dbSIM.SaveChanges();

            return resultadoFinal;
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("CumplimientoContenidoSeguimiento")]
        public string GetCumplimientoContenidoSeguimiento(int ideet)
        {
            DateTime fechaActual = DateTime.Now;
            string resultadoFinal = "C";
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return null;
            }

            resultadoFinal = ResultadoEncuestaSeguimiento(ideet);

            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                                     where eet.ID == ideet
                                                                     select eet).FirstOrDefault();

            evaluacionEncuestaTercero.S_RESULTADO = resultadoFinal;

            dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Modified;

            dbSIM.SaveChanges();

            return resultadoFinal;
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("DevolverEvaluacion")]
        public string GetDevolverEvaluacion(int ideet)
        {
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                                     where ee.ID == ideet
                                                                     select ee).FirstOrDefault();

            if (evaluacionEncuestaTercero != null)
            {
                try
                {
                    evaluacionEncuestaTercero.S_ESTADO = "P";
                    evaluacionEncuestaTercero.S_RESULTADO = null;
                    evaluacionEncuestaTercero.D_FECHA_GENERACION = null;
                    evaluacionEncuestaTercero.ID_USUARIO_GENERACION = null;

                    dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Modified;

                    dbSIM.SaveChanges();

                    return "OK";
                }
                catch
                {
                    return "ERROR";
                }
            }
            else
            {
                return "ERROR";
            }
        }

        private string ResultadoEncuesta(int idee, string version)
        {
            List<EVALUACION_RESPUESTA> respuesta;
            List<int> preguntasEvaluar;

            var respuestas = (from re in dbSIM.EVALUACION_RESPUESTA
                              where re.ID_EVALUACION_ENCUESTA == idee && re.EVALUACION_PREGUNTA.S_VERSION == version
                              select re).ToList();

            switch (version)
            {
                case "1":
                    {
                        respuesta = respuestas.Where(r => r.ID_PREGUNTA == 11).ToList(); // Cumplimiento de la muestra minima según poblacion

                        if (respuesta == null || respuesta.Count == 0)
                            return "P";
                        else if (respuesta[0].N_RESPUESTA != 1)
                            return "N";

                        preguntasEvaluar = new List<int>() { 1, 2, 3, 4, 5, 7, 9, 12, 13, 14, 15, 16, 17 };
                        respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

                        if (respuesta == null || respuesta.Count < 13)
                            return "N";

                        respuesta = respuestas.Where(r => r.ID_PREGUNTA == 18).ToList(); // Existe la estrategia de teletrabajo en la empresa

                        if (respuesta == null || respuesta.Count == 0)
                            return "P";
                        else if (respuesta[0].N_RESPUESTA == 1)
                        {
                            respuesta = respuestas.Where(r => r.ID_PREGUNTA == 19).ToList(); // ¿En caso de tener teletrabajo, describe y analiza  la población que realiza esta estrategia?

                            if (respuesta == null || respuesta.Count == 0)
                                return "P";
                            else if (respuesta[0].N_RESPUESTA != 1)
                                return "N";
                        }

                        preguntasEvaluar = new List<int>() { 20, 21, 22, 23, 25 };
                        respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

                        if (respuesta == null || respuesta.Count < 5)
                            return "N";

                        preguntasEvaluar = new List<int>() { 27, 32, 37, 42, 47, 52, 55, 59 };
                        respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

                        if (respuesta == null || respuesta.Count == 0)
                            return "N";
                    }
                    break;
                case "2":
                    {
                        respuesta = respuestas.Where(r => r.ID_PREGUNTA == 109).ToList(); // ¿La cantidad descrita como muestra concuerda?

                        if (respuesta == null || respuesta.Count == 0)
                            return "P";
                        else if (respuesta[0].N_RESPUESTA != 1)
                            return "N";

                        respuesta = respuestas.Where(r => r.ID_PREGUNTA == 111).ToList(); // Cumplimiento de la muestra minima según poblacion

                        if (respuesta == null || respuesta.Count == 0)
                            return "P";
                        else if (respuesta[0].N_RESPUESTA != 1)
                            return "N";

                        preguntasEvaluar = new List<int>() { 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 128, 129, 130, 131, 132, 133, 134, 136, 137, 138 };
                        respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

                        if (respuesta == null || respuesta.Count < 22)
                            return "N";

                        respuesta = respuestas.Where(r => r.ID_PREGUNTA == 124).ToList(); // Existe la estrategia de teletrabajo en la empresa

                        if (respuesta == null || respuesta.Count == 0)
                            return "P";

                        else if (respuesta[0].N_RESPUESTA == 1)
                        {
                            respuesta = respuestas.Where(r => r.ID_PREGUNTA == 125).ToList(); // ¿En caso de tener teletrabajo, describe y analiza  la población que realiza esta estrategia?

                            if (respuesta == null || respuesta.Count == 0)
                                return "P";
                            else if (respuesta[0].N_RESPUESTA != 1)
                                return "N";

                            respuesta = respuestas.Where(r => r.ID_PREGUNTA == 126).ToList(); // ¿En caso de tener teletrabajo, describe y analiza  los días de teletrabajo?

                            if (respuesta == null || respuesta.Count == 0)
                                return "P";
                            else if (respuesta[0].N_RESPUESTA != 1)
                                return "N";

                            respuesta = respuestas.Where(r => r.ID_PREGUNTA == 127).ToList(); // ¿En caso de tener teletrabajo, describe y analiza las razones de los colaboradores  para realizar teletrabajo?

                            if (respuesta == null || respuesta.Count == 0)
                                return "P";
                            else if (respuesta[0].N_RESPUESTA != 1)
                                return "N";
                        }
                    }
                    break;
            }

            return "C";
        }

        private string ResultadoEncuestaT(int ideet, string version)
        {
            List<EVALUACION_RESPUESTA> respuesta;
            List<int> preguntasEvaluar;

            var respuestas = (from re in dbSIM.EVALUACION_RESPUESTA
                              where re.ID_EVALUACION_ENCUESTA_TERCERO == ideet
                              select re).ToList();

            preguntasEvaluar = new List<int>() { 101, 104, 105 };
            respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

            if (respuesta == null || respuesta.Count < 3)
                return "N";
            else
                return "C";
        }

        private string ResultadoEncuestaSeguimiento(int ideet)
        {
            List<EVALUACION_RESPUESTA> respuesta;
            List<int> preguntasEvaluar;

            var respuestas = (from re in dbSIM.EVALUACION_RESPUESTA
                              where re.ID_EVALUACION_ENCUESTA_TERCERO == ideet
                              select re).ToList();

            preguntasEvaluar = new List<int>() { 1001, 1002, 1003, 1004, 1005, 1007, 1008, 1009 };
            respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA)).ToList();

            if (respuesta == null || respuesta.Count < 8)
                return "P";
            else
            {
                //preguntasEvaluar = new List<int>() { 1006, 1010 };
                preguntasEvaluar = new List<int>() { 1006 };
                respuesta = respuestas.Where(r => preguntasEvaluar.Contains((int)r.ID_PREGUNTA) && r.N_RESPUESTA == 1).ToList();

                if (respuesta == null || respuesta.Count == 0)
                    return "N";
                else
                    return "C";
            }
        }

        /*
         * 
        */

        /*[HttpGet]
        [ActionName("EncuestasInstalacionesTercero")]
        public datosConsulta GetEncuestasEvaluacion(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            dynamic modelData;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var tareaEvaluacion = Data.ObtenerValorParametro("TareaEvaluacionPMES");

                var model = (from PMESEvaluacion in dbSIM.VW_PMES_EVALUACION
                             //where PMESEvaluacion.ID_USUARIO == idUsuario
                             select PMESEvaluacion);

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }*/

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PreguntasEvaluacion")]
        public DATOSCONSULTA GetPreguntasEvaluacion(int idEncuestaEvaluacion)
        {
            if (idEncuestaEvaluacion == 0)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var preguntasEvaluacion = from pe in dbSIM.EVALUACION_PREGUNTA.Where(e => (e.S_VERSION == null || e.S_VERSION == "1") && e.EVALUACION_PREGUNTA_GRUPO.ID_EVALUACION_TIPO == 1)
                                          join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA == idEncuestaEvaluacion) on pe.ID equals er.ID_PREGUNTA into perj
                                          from per in perj.DefaultIfEmpty()
                                              //where per.ID_EVALUACION_ENCUESTA == idEncuestaEvaluacion
                                          orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                          select new
                                          {
                                              ID = per == null ? 0 : per.ID,
                                              ID_PREGUNTA = pe.ID,
                                              S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                              S_PREGUNTA = pe.S_DESCRIPCION,
                                              pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                              per.N_RESPUESTA,
                                              per.S_RESPUESTA,
                                              S_CALCULADO = pe.S_CALCULADO ?? "N",
                                              pe.N_VALOR_COMPLEMENTO,
                                              pe.ID_GRUPO_COMPLEMENTO,
                                              COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                          };

                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = preguntasEvaluacion.Count();
                resultado.datos = preguntasEvaluacion.ToList();

                return resultado;
            }
        }

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PreguntasEvaluacionv2")]
        // t: T Preguntas a nivel de Tercero, I Preguntas a nivel de Instalación
        public DATOSCONSULTA GetPreguntasEvaluacionv2(int idEncuestaEvaluacionTercero, string t)
        {
            if (idEncuestaEvaluacionTercero == 0)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();

                if (t == "I")
                {
                    /*var preguntasEvaluacionInstalacion = from pe in dbSIM.EVALUACION_PREGUNTA.Where(e => e.S_VERSION == "2" && e.S_TIPO == "I")
                                                         from ee in dbSIM.EVALUACION_ENCUESTA.Where(e => e.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero && e.S_EXCLUIR == "N")
                                                         join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                                         join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.EVALUACION_ENCUESTA.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero) on pe.ID equals er.ID_PREGUNTA into perj
                                                         from per in perj.DefaultIfEmpty()
                                                         orderby i.S_NOMBRE, pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                                         select new DATOSREGISTRORESPUESTA
                                                         {
                                                             ID = per == null ? 0 : per.ID,
                                                             ID_EVALUACION_ENCUESTA = ee.ID,
                                                             N_ORDENGRUPO = pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN,
                                                             N_ORDEN = pe.N_ORDEN,
                                                             ID_PREGUNTA = pe.ID,
                                                             S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                                             S_GRUPO_INSTALACION = i.S_NOMBRE,
                                                             S_TIPO = pe.S_TIPO,
                                                             S_PREGUNTA = pe.S_DESCRIPCION,
                                                             N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                                             N_RESPUESTA = per.N_RESPUESTA,
                                                             S_RESPUESTA = per.S_RESPUESTA,
                                                             S_CALCULADO = pe.S_CALCULADO ?? "N",
                                                             N_VALOR_COMPLEMENTO = pe.N_VALOR_COMPLEMENTO,
                                                             ID_GRUPO_COMPLEMENTO = pe.ID_GRUPO_COMPLEMENTO,
                                                             COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                                         };*/

                    var preguntasEvaluacionInstalacion = from pe in dbSIM.EVALUACION_PREGUNTA.Where(e => e.S_VERSION == "2" && e.S_TIPO == "I" && e.EVALUACION_PREGUNTA_GRUPO.ID_EVALUACION_TIPO == 1)
                                                         from ee in dbSIM.EVALUACION_ENCUESTA.Where(e => e.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero && e.S_EXCLUIR == "N")
                                                         join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                                         join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.EVALUACION_ENCUESTA.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero) on new { EEID = ee.ID, PID = pe.ID } equals new { EEID = (int)er.ID_EVALUACION_ENCUESTA, PID = (int)er.ID_PREGUNTA } into perj
                                                         from per in perj.DefaultIfEmpty()
                                                         orderby i.S_NOMBRE, pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                                         select new DATOSREGISTRORESPUESTA
                                                         {
                                                             ID = per == null ? 0 : per.ID,
                                                             ID_EVALUACION_ENCUESTA = ee.ID,
                                                             N_ORDENGRUPO = pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN,
                                                             N_ORDEN = pe.N_ORDEN,
                                                             ID_PREGUNTA = pe.ID,
                                                             S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                                             S_GRUPO_INSTALACION = i.S_NOMBRE,
                                                             S_TIPO = pe.S_TIPO,
                                                             S_PREGUNTA = pe.S_DESCRIPCION,
                                                             N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                                             N_RESPUESTA = per.N_RESPUESTA,
                                                             S_RESPUESTA = per.S_RESPUESTA,
                                                             S_CALCULADO = pe.S_CALCULADO ?? "N",
                                                             N_VALOR_COMPLEMENTO = pe.N_VALOR_COMPLEMENTO,
                                                             ID_GRUPO_COMPLEMENTO = pe.ID_GRUPO_COMPLEMENTO,
                                                             COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                                         };

                    var grupoPreguntaInstalacion = (from pg in dbSIM.EVALUACION_PREGUNTA_GRUPO
                                                   where pg.ID == 13
                                                   select pg).FirstOrDefault();

                    var resultadoEvaluacionInstalacion = from ee in dbSIM.EVALUACION_ENCUESTA
                                                         join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                                         where ee.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero && ee.S_EXCLUIR == "N"
                                                         select new DATOSREGISTRORESPUESTA
                                                         {
                                                             ID = ee.ID * (-1),
                                                             ID_EVALUACION_ENCUESTA = ee.ID,
                                                             N_ORDENGRUPO = grupoPreguntaInstalacion.N_ORDEN,
                                                             N_ORDEN = 1000,
                                                             ID_PREGUNTA = ee.ID * (-1),
                                                             S_GRUPO = grupoPreguntaInstalacion.S_NOMBRE,
                                                             S_GRUPO_INSTALACION = i.S_NOMBRE,
                                                             S_TIPO = "I",
                                                             S_PREGUNTA = "Cumplimiento de Contenido",
                                                             N_TIPO_RESPUESTA = 2, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                                             N_RESPUESTA = (ee.S_RESULTADO == "C" ? (decimal?)1 : (ee.S_RESULTADO == "N" ? (decimal?)2 : null)),
                                                             S_RESPUESTA = "",
                                                             S_CALCULADO = "M",
                                                             N_VALOR_COMPLEMENTO = null,
                                                             ID_GRUPO_COMPLEMENTO = null,
                                                             COLOR = (ee.S_RESULTADO == "C" ? 1 : (ee.S_RESULTADO == "N" ? 2 : 0))
                                                         };

                    var preguntasEvaluacionInstalacionResultado = preguntasEvaluacionInstalacion.ToList().Union(resultadoEvaluacionInstalacion.ToList());

                    resultado.numRegistros = preguntasEvaluacionInstalacionResultado.Count();
                    resultado.datos = preguntasEvaluacionInstalacionResultado;
                }

                if (t == "T")
                {
                    var preguntasEvaluacionTercero = from pe in dbSIM.EVALUACION_PREGUNTA.Where(e => e.S_VERSION == "2" && e.S_TIPO == "T" && e.EVALUACION_PREGUNTA_GRUPO.ID_EVALUACION_TIPO == 1)
                                                     join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA_TERCERO == idEncuestaEvaluacionTercero) on pe.ID equals er.ID_PREGUNTA into perj
                                                     from per in perj.DefaultIfEmpty()
                                                     orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                                     select new
                                                     {
                                                         ID = per == null ? 0 : per.ID,
                                                         N_ORDENGRUPO = pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN,
                                                         pe.N_ORDEN,
                                                         ID_PREGUNTA = pe.ID,
                                                         S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                                         pe.S_TIPO,
                                                         S_PREGUNTA = pe.S_DESCRIPCION,
                                                         pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                                         per.N_RESPUESTA,
                                                         per.S_RESPUESTA,
                                                         S_CALCULADO = pe.S_CALCULADO ?? "N",
                                                         pe.N_VALOR_COMPLEMENTO,
                                                         pe.ID_GRUPO_COMPLEMENTO,
                                                         COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                                     };

                    resultado.numRegistros = preguntasEvaluacionTercero.Count();
                    resultado.datos = preguntasEvaluacionTercero.ToList();
                }

                return resultado;

                //var preguntasEvaluacion = preguntasEvaluacionTercero.Union(preguntasEvaluacionInstalacion).OrderBy(e => new { e.N_ORDENGRUPO, e.N_ORDEN }).ToList();

            }
        }

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PreguntasSeguimiento")]
        // t: T Preguntas a nivel de Tercero, I Preguntas a nivel de Instalación
        public DATOSCONSULTA GetPreguntasSeguimiento(int idEncuestaEvaluacionTercero, string t)
        {
            if (idEncuestaEvaluacionTercero == 0)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();

                if (t == "T")
                {
                    var respuestaPeriodo = (from er in dbSIM.EVALUACION_RESPUESTA
                                            where er.ID_EVALUACION_ENCUESTA_TERCERO == idEncuestaEvaluacionTercero && er.ID_PREGUNTA == 1000
                                            select er).Select(r => r.N_RESPUESTA).FirstOrDefault() ?? 0;
                    string porc = (respuestaPeriodo == 0 ? "10%" : (respuestaPeriodo == 1 ? "10%" : "20%"));

                    var preguntasEvaluacionTercero = from pe in dbSIM.EVALUACION_PREGUNTA.Where(e => e.S_VERSION == "1" && e.S_TIPO == "T" && e.EVALUACION_PREGUNTA_GRUPO.ID_EVALUACION_TIPO == 2)
                                                     join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA_TERCERO == idEncuestaEvaluacionTercero) on pe.ID equals er.ID_PREGUNTA into perj
                                                     from per in perj.DefaultIfEmpty()
                                                     orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                                     select new
                                                     {
                                                         ID = per == null ? 0 : per.ID,
                                                         N_ORDENGRUPO = pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN,
                                                         pe.N_ORDEN,
                                                         ID_PREGUNTA = pe.ID,
                                                         S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                                         pe.S_TIPO,
                                                         S_PREGUNTA = (pe.ID == 1006 ? pe.S_DESCRIPCION.Replace("#1000#", porc) : pe.S_DESCRIPCION),
                                                         pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                                         per.N_RESPUESTA,
                                                         per.S_RESPUESTA,
                                                         S_CALCULADO = pe.S_CALCULADO ?? "N",
                                                         pe.N_VALOR_COMPLEMENTO,
                                                         pe.ID_GRUPO_COMPLEMENTO,
                                                         COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                                     };

                    resultado.numRegistros = preguntasEvaluacionTercero.Count();
                    resultado.datos = preguntasEvaluacionTercero.ToList();
                }

                return resultado;

                //var preguntasEvaluacion = preguntasEvaluacionTercero.Union(preguntasEvaluacionInstalacion).OrderBy(e => new { e.N_ORDENGRUPO, e.N_ORDEN }).ToList();

            }
        }

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("Estrategias")]
        public dynamic GetEstrategias()
        {
            var grupos = from eg in dbSIM.EVALUACION_ESTRATEGIAS_GRUPO
                         orderby eg.ID
                         select new
                         {
                             id = eg.ID,
                             grupo = eg.S_NOMBRE
                         };

            var estrategias = from e in dbSIM.EVALUACION_ESTRATEGIAS
                              orderby e.ID
                              select new
                              {
                                  id = e.ID,
                                  idGrupo = e.ID_GRUPO,
                                  estrategia = e.S_NOMBRE
                              };

            return new
            {
                grupos = grupos.ToList(),
                estrategias = estrategias.ToList()
            };
        }

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PreguntasEstrategias")]
        // t: tipo (PE Planeación Estrategias, EM Estrategias Movilidad)
        public DATOSCONSULTA GetPreguntasEstrategias(int idEncuestaEvaluacionTercero, string t)
        {
            if (idEncuestaEvaluacionTercero == 0)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();

                var estrategiasEvaluacionTercero = from ee in dbSIM.EVALUACION_ESTRATEGIAS_T
                                                   where ee.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero && ee.S_TIPO == t
                                                   orderby ee.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE, ee.EVALUACION_ESTRATEGIAS.S_NOMBRE + (ee.S_OTRO == null || ee.S_OTRO == "" ? "" : " (" + ee.S_OTRO + ")")
                                                   select new ESTRATEGIA
                                                   {
                                                       ID = ee.ID,
                                                       ID_EVALUACION_TERCERO = ee.ID_EVALUACION_TERCERO,
                                                       ID_ESTRATEGIA = ee.ID_ESTRATEGIA,
                                                       ID_GRUPO = ee.EVALUACION_ESTRATEGIAS.ID_GRUPO,
                                                       S_GRUPO = ee.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE,
                                                       S_ESTRATEGIA = ee.EVALUACION_ESTRATEGIAS.S_NOMBRE + (ee.S_OTRO == null || ee.S_OTRO == "" ? "" : " (" + ee.S_OTRO + ")"),
                                                       S_OTRO = ee.S_OTRO,
                                                       S_INDICADOR_MEDICION = ee.S_INDICADOR_MEDICION,
                                                       S_UNIDADES_META = ee.S_UNIDADES_META,
                                                       S_UNIDADES_META_NOMBRE = (ee.S_UNIDADES_META == "UN" ? "UN" : "%"),
                                                       N_VALOR_META = ee.N_VALOR_META,
                                                       N_PRESUPUESTO = ee.N_PRESUPUESTO,
                                                       S_TIPO = ee.S_TIPO,
                                                       N_VALOR_META_ALCANZAR = ee.N_VALOR_META_ALCANZAR
                                                   };

                resultado.numRegistros = estrategiasEvaluacionTercero.Count();
                resultado.datos = estrategiasEvaluacionTercero.ToList();

                return resultado;
            }
        }

        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("EliminarEstrategiasEvaluacion")]
        public void GetEliminarEstrategiasEvaluacion(int idEstrategiaTercero)
        {
            var estrategiasEvaluacionTercero = (from ee in dbSIM.EVALUACION_ESTRATEGIAS_T
                                                where ee.ID == idEstrategiaTercero
                                                select ee).FirstOrDefault();

            if (estrategiasEvaluacionTercero != null)
            {
                dbSIM.Entry(estrategiasEvaluacionTercero).State = EntityState.Deleted;

                dbSIM.SaveChanges();
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("InsertarEstrategiaEvaluacion")]
        public void PostInsertarEstrategiaEvaluacion(EVALUACION_ESTRATEGIAS_T ee)
        {
            try
            {
                dbSIM.Entry(ee).State = EntityState.Added;

                dbSIM.SaveChanges();
            } catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Evaluación PMES [PostInsertarEstrategiaEvaluacion] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("ActualizarEstrategiaEvaluacion")]
        public void PostActualizarEstrategiaEvaluacion(EVALUACION_ESTRATEGIAS_T ee)
        {
            try
            {
                    dbSIM.Entry(ee).State = EntityState.Modified;

                    dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Evaluación PMES [PostActualizarEstrategiaEvaluacion] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }

        /*
        // GET api/<controller>
        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("PreguntasEvaluacionTercero")]
        public DATOSCONSULTA GetPreguntasEvaluacionTercero(int idEncuestaEvaluacionTercero)
        {
            if (idEncuestaEvaluacionTercero == 0)
            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var preguntasEvaluacion = from ee in dbSIM.EVALUACION_ENCUESTA.Where(e => e.ID_EVALUACION_TERCERO == idEncuestaEvaluacionTercero)
                                          from pe in dbSIM.EVALUACION_PREGUNTA
                                          join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA == ee.) on pe.ID equals er.ID_PREGUNTA into perj
                                          from per in perj.DefaultIfEmpty()
                                              //where per.ID_EVALUACION_ENCUESTA == idEncuestaEvaluacion
                                          orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                          select new
                                          {
                                              ID = per == null ? 0 : per.ID,
                                              ID_PREGUNTA = pe.ID,
                                              S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                              S_PREGUNTA = pe.S_DESCRIPCION,
                                              pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                              per.N_RESPUESTA,
                                              per.S_RESPUESTA,
                                              S_CALCULADO = pe.S_CALCULADO ?? "N",
                                              pe.N_VALOR_COMPLEMENTO,
                                              pe.ID_GRUPO_COMPLEMENTO,
                                              COLOR = (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != 1 ? 2 : 0)// 0 Sin Color, 1 Verde, 2 Rojo
                                          };

                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = preguntasEvaluacion.Count();
                resultado.datos = preguntasEvaluacion.ToList();

                return resultado;
            }
        }*/

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("ObservacionesEvaluacionActualizar")]
        public string PostObservacionesEvaluacionActualizar(DATOSOBSERVACIONES observaciones)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                                      where ee.ID == observaciones.idEncuestaEvaluacion
                                                      select ee).FirstOrDefault();

            if (evaluacionEncuesta == null)
            {
                return "NO";
            }
            else
            {
                try
                {
                    evaluacionEncuesta.S_OBSERVACIONES = observaciones.observaciones;
                    dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

                    dbSIM.SaveChanges();

                    return "OK";
                }
                catch
                {
                    return "ERROR";
                }
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("ObservacionesEvaluacionActualizarT")]
        public string PostObservacionesEvaluacionActualizarT(DATOSOBSERVACIONES observaciones)
        {
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaT = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                              where eet.ID == observaciones.idEncuestaEvaluacion
                                                      select eet).FirstOrDefault();

            if (evaluacionEncuestaT == null)
            {
                return "NO";
            }
            else
            {
                try
                {
                    evaluacionEncuestaT.S_OBSERVACIONES = observaciones.observaciones;
                    dbSIM.Entry(evaluacionEncuestaT).State = EntityState.Modified;

                    dbSIM.SaveChanges();

                    return "OK";
                }
                catch
                {
                    return "ERROR";
                }
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("ObservacionesEvaluacion")]
        public string GetObservacionesEvaluacion(int idee)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                                      where ee.ID == idee
                                                      select ee).FirstOrDefault();

            if (evaluacionEncuesta == null)
            {
                return "";
            }
            else
            {
                return evaluacionEncuesta.S_OBSERVACIONES;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("ObservacionesEvaluacionT")]
        public string GetObservacionesEvaluacionT(int ideet)
        {
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaT = (from ee in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                      where ee.ID == ideet
                                                      select ee).FirstOrDefault();

            if (evaluacionEncuestaT == null)
            {
                return "";
            }
            else
            {
                return evaluacionEncuestaT.S_OBSERVACIONES;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("PreguntasEvaluacionActualizar")]
        public void PostPreguntasEvaluacionActualizar(DATOSREGISTRO respuestaEvaluacion)
        {
            int idPregunta;
            EVALUACION_RESPUESTA evaluacionRespuesta = null;
            CultureInfo culture = new CultureInfo("en-US");

            var evaluacionEncuesta = (from ee in dbSIM.EVALUACION_ENCUESTA
                                      where ee.ID == respuestaEvaluacion.idEncuestaEvaluacion
                                      select ee).FirstOrDefault();

            evaluacionEncuesta.S_ESTADO = "R";
            evaluacionEncuesta.S_RESULTADO = "P";

            dbSIM.Entry(evaluacionEncuesta).State = EntityState.Modified;

            dbSIM.SaveChanges();

            if (respuestaEvaluacion.id != 0)
            {
                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           //where er.ID == respuestaEvaluacion.id
                                       where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == respuestaEvaluacion.idPregunta
                                       select er).FirstOrDefault();
            }

            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
            {
                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                evaluacionRespuesta.ID_PREGUNTA = respuestaEvaluacion.idPregunta;
                evaluacionRespuesta.N_RESPUESTA = null;
                evaluacionRespuesta.S_RESPUESTA = null;

                if (respuestaEvaluacion.tipoRespuesta <= 4)
                {
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEvaluacion.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    evaluacionRespuesta.S_RESPUESTA = respuestaEvaluacion.valor;
                }

                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }
            else
            {
                evaluacionRespuesta.N_RESPUESTA = null;
                evaluacionRespuesta.S_RESPUESTA = null;

                if (respuestaEvaluacion.tipoRespuesta <= 4 && respuestaEvaluacion.valor != null)
                {
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEvaluacion.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    evaluacionRespuesta.S_RESPUESTA = respuestaEvaluacion.valor;
                }

                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }

            // Recorremos las diferentes preguntas que me afectan datos calculados
            switch (evaluacionRespuesta.ID_PREGUNTA)
            {
                //11 - Cumplimiento de la muestra minima según poblacion
                case 8: // Muestra (n)
                case 10: // Muestra calculada con un 5% de error y un Z de confiabilidad 1.95
                    {
                        idPregunta = 11;
                        var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 8
                                                        select er).FirstOrDefault();

                        var evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 10
                                                        select er).FirstOrDefault();

                        evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                               where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                               select er).FirstOrDefault();

                        if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                        {
                            evaluacionRespuesta = new EVALUACION_RESPUESTA();
                            evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                            evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                            evaluacionRespuesta.N_RESPUESTA = null;
                            evaluacionRespuesta.S_RESPUESTA = null;

                            if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null)
                                evaluacionRespuesta.N_RESPUESTA = (evaluacionRespuestaDep01.N_RESPUESTA >= evaluacionRespuestaDep02.N_RESPUESTA ? 1 : 2); // 1 Cumple, 2 No Cumple;
                            else
                                evaluacionRespuesta.N_RESPUESTA = 2; // 1 Cumple, 2 No Cumple;

                            dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            evaluacionRespuesta.N_RESPUESTA = null;
                            evaluacionRespuesta.S_RESPUESTA = null;

                            if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null)
                                evaluacionRespuesta.N_RESPUESTA = (evaluacionRespuestaDep01.N_RESPUESTA >= evaluacionRespuestaDep02.N_RESPUESTA ? 1 : 2); // 1 Cumple, 2 No Cumple;
                            else
                                evaluacionRespuesta.N_RESPUESTA = 2; // 1 Cumple, 2 No Cumple;

                            dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                            dbSIM.SaveChanges();
                        }
                    }
                    break;
                //111 - Cumplimiento de la muestra minima según poblacion
                case 108: // Muestra (n)
                case 110: // Muestra calculada con un 5% de error y un Z de confiabilidad 1.95
                    {
                        idPregunta = 111;
                        var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 108
                                                        select er).FirstOrDefault();

                        var evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 110
                                                        select er).FirstOrDefault();

                        evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                               where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                               select er).FirstOrDefault();

                        if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                        {
                            evaluacionRespuesta = new EVALUACION_RESPUESTA();
                            evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                            evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                            evaluacionRespuesta.N_RESPUESTA = null;
                            evaluacionRespuesta.S_RESPUESTA = null;

                            if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null)
                                evaluacionRespuesta.N_RESPUESTA = (evaluacionRespuestaDep01.N_RESPUESTA >= evaluacionRespuestaDep02.N_RESPUESTA ? 1 : 2); // 1 Cumple, 2 No Cumple;
                            else
                                evaluacionRespuesta.N_RESPUESTA = 2; // 1 Cumple, 2 No Cumple;

                            dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            evaluacionRespuesta.N_RESPUESTA = null;
                            evaluacionRespuesta.S_RESPUESTA = null;

                            if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null)
                                evaluacionRespuesta.N_RESPUESTA = (evaluacionRespuestaDep01.N_RESPUESTA >= evaluacionRespuestaDep02.N_RESPUESTA ? 1 : 2); // 1 Cumple, 2 No Cumple;
                            else
                                evaluacionRespuesta.N_RESPUESTA = 2; // 1 Cumple, 2 No Cumple;

                            dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                            dbSIM.SaveChanges();
                        }
                    }
                    break;
                case 6: // Numero de trabajadores de la sede (N)
                case 26: // Personas aptas para aplicar estrategias de movilidad en CAMINATA
                case 31: // Personas aptas para aplicar estrategias de movilidad en BICICLETA
                case 36: // Personas aptas para aplicar estrategias de movilidad en SITVA
                case 41: // Personas aptas para aplicar estrategias de movilidad en VEHICULO COMPARTIDO (Carpooling)
                case 46: // Personas aptas para aplicar estrategias de movilidad en RUTAS EMPRESARIALES
                case 51: // Personas aptas para RACIONALIZAR EL USO DEL VEHICULO PARTICULAR (Regular Estacionamiento, Conduccion Ecoeficiente,...) 
                case 54: // Personas aptas para aplicar estrategias para REDUCIR NUMERO DE VIAJES (Teletrabajo, Smart Working, ...)
                    {
                        EVALUACION_RESPUESTA evaluacionRespuestaDep02;

                        var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 6
                                                        select er).FirstOrDefault();

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 26)
                        {
                            idPregunta = 29; // % de personas aptas para aplicar estrategias de movilidad en CAMINATA según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 26
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 31)
                        {
                            idPregunta = 34; // % de personas aptas para aplicar estrategias de movilidad en BICICLETA según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 31
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 36)
                        {
                            idPregunta = 39; // % de personas aptas para aplicar estrategias de movilidad en SITVA según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 36
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 41)
                        {
                            idPregunta = 44; // % de personas aptas para aplicar estrategias de movilidad en VEHICULO COMPARTIDO según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 41
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 46)
                        {
                            idPregunta = 49; // % de personas aptas para aplicar estrategias de movilidad en RUTAS EMPRESARIALES según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 46
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 51)
                        {
                            idPregunta = 53; // % de personas aptas para RACIONALIZAR EL USO DEL VEHICULO PARTICULAR según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 51
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 6 || evaluacionRespuesta.ID_PREGUNTA == 54)
                        {
                            idPregunta = 57; // % de personas aptas para REDUCIR EL NUMERO DE VIAJES según el total de empleados
                            evaluacionRespuestaDep02 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                        where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 54
                                                        select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && evaluacionRespuestaDep02.N_RESPUESTA != null && evaluacionRespuestaDep01.N_RESPUESTA > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep02.N_RESPUESTA / evaluacionRespuestaDep01.N_RESPUESTA;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }
                    }
                    break;
                case 28: // Ton CO2/dia a disminuir si todas las personas aptas para la movilidad en CAMINATA eligieran este modo
                case 33: // Ton CO2/dia a disminuir si todas las personas aptas para la movilidad en BICICLETA eligieran este modo
                case 38: // Ton CO2/dia a disminuir si todas las personas aptas para la movilidad en SITVA eligieran este modo
                case 43: // Ton CO2/dia a disminuir si todas las personas aptas para usar VEHICULO COMPARTIDO lo hicieran
                case 48: // Ton CO2/dia a disminuir si todas las personas aptas para usar RUTAS EMPRESARIALES lo hicieran
                case 56: // Ton CO2/dia a disminuir si todas las personas aptas para REDUCIR EL NUMERO DE VIAJES lo hicieran
                    {
                        var co2 = (from ee in dbSIM.EVALUACION_ENCUESTA
                                       //join vpmes in dbSIM.VWM_PMES on new { ee.EVALUACION_ENCUESTA_TERCERO.ID_TERCERO, ee.ID_INSTALACION, ee.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA } equals new { vpmes.ID_TERCERO, vpmes.ID_INSTALACION, vpmes.VIGENCIA }
                                   join vpmes in dbSIM.VWM_PMES on new { ee.EVALUACION_ENCUESTA_TERCERO.ID_TERCERO, ee.ID_INSTALACION } equals new { vpmes.ID_TERCERO, vpmes.ID_INSTALACION }
                                   where ee.ID == respuestaEvaluacion.idEncuestaEvaluacion && ee.EVALUACION_ENCUESTA_TERCERO.S_VALOR_VIGENCIA == vpmes.VIGENCIA
                                   select vpmes.N_CO2P).Sum();

                        if (evaluacionRespuesta.ID_PREGUNTA == 28)
                        {
                            // 30 - % de reduccion en el CO2 emitido si todas las personas aptas para la movilidad en CAMINATA eligieran este modo
                            idPregunta = 30;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 33)
                        {
                            // 35 - % de reduccion en el CO2 emitido si todas las personas aptas para la movilidad en BICICLETA eligieran este modo
                            idPregunta = 35;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 38)
                        {
                            // 40 - % de reduccion en el CO2 emitido si todas las personas aptas para la movilidad en SITVA eligieran este modo
                            idPregunta = 40;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 43)
                        {
                            // 45 - % de reduccion en el CO2 emitido si todas las personas aptas para usar VEHICULO COMPARTIDO lo hicieran
                            idPregunta = 45;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 48)
                        {
                            // 50 - % de reduccion en el CO2 emitido si todas las personas aptas para usar RUTAS EMPRESARIALES lo hicieran
                            idPregunta = 50;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }

                        if (evaluacionRespuesta.ID_PREGUNTA == 56)
                        {
                            // 58 - % de reduccion en el CO2 emitido si todas las personas aptas para REDUCIR EL NUMERO DE VIAJES lo hicieran
                            idPregunta = 58;
                            var evaluacionRespuestaDep01 = (from er in dbSIM.EVALUACION_RESPUESTA
                                                            where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == evaluacionRespuesta.ID_PREGUNTA
                                                            select er).FirstOrDefault();

                            evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                                   where er.ID_EVALUACION_ENCUESTA == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == idPregunta
                                                   select er).FirstOrDefault();

                            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
                            {
                                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                                evaluacionRespuesta.ID_EVALUACION_ENCUESTA = respuestaEvaluacion.idEncuestaEvaluacion;
                                evaluacionRespuesta.ID_PREGUNTA = idPregunta;
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                evaluacionRespuesta.N_RESPUESTA = null;
                                evaluacionRespuesta.S_RESPUESTA = null;

                                if (evaluacionRespuestaDep01.N_RESPUESTA != null && co2 > 0)
                                    evaluacionRespuesta.N_RESPUESTA = evaluacionRespuestaDep01.N_RESPUESTA / co2;

                                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                                dbSIM.SaveChanges();
                            }
                        }
                    }
                    break;
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpPost]
        [ActionName("PreguntasEvaluacionActualizarT")]
        public void PostPreguntasEvaluacionActualizarT(DATOSREGISTRO respuestaEvaluacion)
        {
            EVALUACION_RESPUESTA evaluacionRespuesta = null;
            CultureInfo culture = new CultureInfo("en-US");

            if (respuestaEvaluacion.id != 0)
            {
                evaluacionRespuesta = (from er in dbSIM.EVALUACION_RESPUESTA
                                           //where er.ID == respuestaEvaluacion.id
                                       where er.ID_EVALUACION_ENCUESTA_TERCERO == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == respuestaEvaluacion.idPregunta
                                       select er).FirstOrDefault();
            }

            if (evaluacionRespuesta == null) // No existe la respuesta de la pregunta en la evaluación de la encuesta, por lo tanto se crea
            {
                evaluacionRespuesta = new EVALUACION_RESPUESTA();
                evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO = respuestaEvaluacion.idEncuestaEvaluacion;
                evaluacionRespuesta.ID_PREGUNTA = respuestaEvaluacion.idPregunta;
                evaluacionRespuesta.N_RESPUESTA = null;
                evaluacionRespuesta.S_RESPUESTA = null;

                if (respuestaEvaluacion.tipoRespuesta <= 4 || respuestaEvaluacion.tipoRespuesta == 10)
                {
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEvaluacion.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    evaluacionRespuesta.S_RESPUESTA = respuestaEvaluacion.valor;
                }

                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }
            else
            {
                evaluacionRespuesta.N_RESPUESTA = null;
                evaluacionRespuesta.S_RESPUESTA = null;

                if ((respuestaEvaluacion.tipoRespuesta <= 4 || respuestaEvaluacion.tipoRespuesta == 10) && respuestaEvaluacion.valor != null)
                {
                    evaluacionRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEvaluacion.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    evaluacionRespuesta.S_RESPUESTA = respuestaEvaluacion.valor;
                }

                dbSIM.Entry(evaluacionRespuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }

            if (evaluacionRespuesta.ID_PREGUNTA == 1000)
            {
                var evaluacionRespuestaPorc = (from er in dbSIM.EVALUACION_RESPUESTA
                                          where er.ID_EVALUACION_ENCUESTA_TERCERO == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 1005
                                          select er).FirstOrDefault();

                var evaluacionRespuestaCumpPorc = (from er in dbSIM.EVALUACION_RESPUESTA
                                               where er.ID_EVALUACION_ENCUESTA_TERCERO == respuestaEvaluacion.idEncuestaEvaluacion && er.ID_PREGUNTA == 1006
                                               select er).FirstOrDefault();

                var porc = (evaluacionRespuesta.N_RESPUESTA == 2 ? -20 : -10);

                if (evaluacionRespuestaPorc.N_RESPUESTA <= porc)
                    evaluacionRespuestaCumpPorc.N_RESPUESTA = 1;
                else
                    evaluacionRespuestaCumpPorc.N_RESPUESTA = 2;

                dbSIM.Entry(evaluacionRespuestaCumpPorc).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }

            var ideet = (evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO != null ? evaluacionRespuesta.ID_EVALUACION_ENCUESTA_TERCERO : evaluacionRespuesta.EVALUACION_ENCUESTA.ID_EVALUACION_TERCERO);

            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                                     where eet.ID == ideet
                                                                     select eet).FirstOrDefault();

            evaluacionEncuestaTercero.S_RESULTADO = "P";

            dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Modified;

            dbSIM.SaveChanges();
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("ValidarGeneracionDocumentoEvaluacion")]
        public int GetValidarGeneracionDocumentoEvaluacion(int eet)
        {
            var encuestaTercero = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                   where et.ID == eet
                                   select et).FirstOrDefault();

            if (encuestaTercero.S_ESTADO == "G")
                return 1;

            if (encuestaTercero.ID_EVALUACION_TIPO == 1)
            {
                string sql = "SELECT ee.ID AS ID_EVALUACION_ENCUESTA, " +
                            "eet.ID AS ID_EVALUACION_TERCERO, " +
                            "t.ID_TERCERO, " +
                            "i.ID_INSTALACION, " +
                            "i.S_NOMBRE INSTALACION, " +
                            "CASE WHEN NVL(ee.S_ESTADO, 'R') = 'R' THEN 'P' ELSE ee.S_RESULTADO END RESULTADO, " +
                            "ee.S_PRINCIPAL " +
                            "FROM CONTROL.EVALUACION_ENCUESTA_TERCERO eet INNER JOIN " +
                            "  GENERAL.TERCERO t ON eet.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                            "  GENERAL.TERCERO_INSTALACION ti ON t.ID_TERCERO = ti.ID_TERCERO INNER JOIN " +
                            "  GENERAL.INSTALACION i ON ti.ID_INSTALACION = i.ID_INSTALACION LEFT OUTER JOIN " +
                            "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND i.ID_INSTALACION = ee.ID_INSTALACION " +
                            "WHERE eet.ID = :eet AND NVL(ee.S_EXCLUIR, 'N') = 'N' " +
                            "ORDER BY i.S_NOMBRE";

                OracleParameter ideetParameter = new OracleParameter("eet", eet);

                var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { ideetParameter }).ToList();

                {
                    var instalacionFaltante = instalacionesEvaluacion.Where(i => i.RESULTADO == "P").FirstOrDefault();

                    if (instalacionFaltante != null)
                        return 2;

                    if (instalacionesEvaluacion.Count > 1)
                    {
                        var instalacionPrincipal = instalacionesEvaluacion.Where(i => i.S_PRINCIPAL == "S").FirstOrDefault();

                        if (instalacionPrincipal == null)
                            return 2;
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                string sql = "SELECT ee.ID AS ID_EVALUACION_ENCUESTA, " +
                            "eet.ID AS ID_EVALUACION_TERCERO, " +
                            "t.ID_TERCERO, " +
                            "i.ID_INSTALACION, " +
                            "i.S_NOMBRE INSTALACION, " +
                            "CASE WHEN NVL(ee.S_ESTADO, 'R') = 'R' THEN 'P' ELSE ee.S_RESULTADO END RESULTADO, " +
                            "ee.S_PRINCIPAL " +
                            "FROM CONTROL.EVALUACION_ENCUESTA_TERCERO eet INNER JOIN " +
                            "  GENERAL.TERCERO t ON eet.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                            "  GENERAL.TERCERO_INSTALACION ti ON t.ID_TERCERO = ti.ID_TERCERO INNER JOIN " +
                            "  GENERAL.INSTALACION i ON ti.ID_INSTALACION = i.ID_INSTALACION LEFT OUTER JOIN " +
                            "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND i.ID_INSTALACION = ee.ID_INSTALACION " +
                            "WHERE eet.ID = :eet AND NVL(ee.S_EXCLUIR, 'N') = 'N' " +
                            "ORDER BY i.S_NOMBRE";

                OracleParameter ideetParameter = new OracleParameter("eet", eet);

                var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { ideetParameter }).ToList();

                {
                    if (instalacionesEvaluacion.Count > 1)
                    {
                        var instalacionPrincipal = instalacionesEvaluacion.Where(i => i.S_PRINCIPAL == "S").FirstOrDefault();

                        if (instalacionPrincipal == null)
                            return 2;
                        else
                            return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        [HttpGet]
        [ActionName("ObtenerDetalleRespuesta")]
        public dynamic GetObtenerDetalleRespuesta(int idRespuesta)
        {
            EVALUACION_RESPUESTA respuesta = dbSIM.EVALUACION_RESPUESTA.Where(r => r.ID == idRespuesta).FirstOrDefault();
            EVALUACION_PREGUNTA pregunta = respuesta.EVALUACION_PREGUNTA;
            EVALUACION_RESPUESTA_DETALLE respuestaDetalle = dbSIM.EVALUACION_RESPUESTA_DETALLE.Where(rd => rd.ID_EVALUACION_RESPUESTA == idRespuesta).FirstOrDefault();

            OracleParameter idTerceroParameter = new OracleParameter("idRespuesta", idRespuesta);

            //var seleccion = dbSIM.Database.SqlQuery<dynamic>("SELECT erd.S_OPCIONES, erd.S_DESCRIPCION FROM CONTROL.EVALUACION_RESPUESTA_DETALLE erd INNER JOIN CONTROL.EVALUACION_RESPUESTA er ON erd.ID_EVALUACION_RESPUESTA = er.ID INNER JOIN CONTROL.EVALUACION_PREGUNTA ep ON er.ID_PREGUNTA = ep.ID WHERE er.ID = :idRespuesta", new object[] { idRespuesta }).FirstOrDefault();

            if (respuestaDetalle == null || respuestaDetalle.S_OPCIONES == null || respuestaDetalle.S_OPCIONES.Trim() == "") // Ninguna Seleccionada
            {
                var opcionesSeleccionadas = dbSIM.Database.SqlQuery<OPCIONESDETALLERESPUESTA>("SELECT ID, S_DESCRIPCION, N_ORDEN, 0 AS SELECCIONADO FROM CONTROL.EVALUACION_PREGUNTA_COMP WHERE ID_GRUPO_COMP = " + pregunta.ID_GRUPO_COMPLEMENTO + " ORDER BY N_ORDEN");

                return new { opciones = opcionesSeleccionadas, texto = (respuestaDetalle != null ? respuestaDetalle.S_DESCRIPCION : null) };
            }
            else // Algunas Seleccionadas
            {
                var opcionesSeleccionadas = dbSIM.Database.SqlQuery<OPCIONESDETALLERESPUESTA>("SELECT ID, S_DESCRIPCION, N_ORDEN, CASE WHEN ID IN (" + respuestaDetalle.S_OPCIONES + ") THEN 1 ELSE 0 END AS SELECCIONADO FROM CONTROL.EVALUACION_PREGUNTA_COMP WHERE ID_GRUPO_COMP = " + pregunta.ID_GRUPO_COMPLEMENTO + " ORDER BY N_ORDEN");

                return new { opciones = opcionesSeleccionadas, texto = (respuestaDetalle != null ? respuestaDetalle.S_DESCRIPCION : null) };
            }
        }

        [HttpPost]
        [ActionName("PreguntasDetalleEvaluacionActualizar")]
        public void PostPreguntasDetalleEvaluacionActualizar(OPCIONESDETALLERESPUESTAINGRESADO datos)
        {
            EVALUACION_RESPUESTA_DETALLE respuestaDetalle = (from rd in dbSIM.EVALUACION_RESPUESTA_DETALLE
                                                             where rd.ID_EVALUACION_RESPUESTA == datos.Id
                                                             select rd).FirstOrDefault();

            if (respuestaDetalle != null)
            {
                respuestaDetalle.S_OPCIONES = datos.Opciones;
                respuestaDetalle.S_DESCRIPCION = datos.Texto;

                dbSIM.Entry(respuestaDetalle).State = EntityState.Modified;
                dbSIM.SaveChanges();
            }
            else
            {
                respuestaDetalle = new EVALUACION_RESPUESTA_DETALLE();
                respuestaDetalle.ID_EVALUACION_RESPUESTA = datos.Id;
                respuestaDetalle.S_OPCIONES = datos.Opciones;
                respuestaDetalle.S_DESCRIPCION = datos.Texto;

                dbSIM.Entry(respuestaDetalle).State = EntityState.Added;
                dbSIM.SaveChanges();
            }
        }

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet]
        [ActionName("ActualizarDatosGenerales")]
        public dynamic GetActualizarDatosGenerales(int idee)
        {
            var evaluacionEncuesta = EvaluacionEncuestaUtilidad.ActualizarDatosGeneralesEvaluacionEncuesta(idee, dbSIM);

            var resultado = new
            {
                MedioEntrega = (evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO"),
                Radicado = evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.S_RADICADO,
                FechaEntrega = (evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuesta.EVALUACION_ENCUESTA_TERCERO.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : ""),
                Coordenada = evaluacionEncuesta.S_COORDENADA,
                Direccion = evaluacionEncuesta.S_DIRECCION,
                TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : ""),
                KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : ""),
                PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : ""),
                PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : ""),
            };

            return resultado;
        }

        [HttpGet]
        [ActionName("ActualizarPMESVista")]
        public string GetActualizarPMESVista()
        {
            DateTime fechaActual = DateTime.Now;
            string sqlInstalacion;
            List<string> registrosProcesados = new List<string>();

            try
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "PMES [GetActualizarPMESVista] : Inicio de Ejecución.");

                var sql1 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion.txt"));
                var sql2 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion_2.txt"));
                var sql3 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion_3.txt"));

                var pendientesActualizar = dbSIM.Database.SqlQuery<ACTUALIZACIONPMES>("SELECT ID, ID_ENCUESTA, ID_TERCERO, ID_INSTALACION, S_VALOR_VIGENCIA FROM CONTROL.ENC_DATOS_MODIFICADOS WHERE D_FECHA_INICIO IS NULL AND NVL(N_ERROR, 0) = 0 AND D_FECHA <= TO_DATE('" + fechaActual.ToString("yyyyMMdd HH:mm:ss") + "', 'YYYYMMDD HH24:MI:SS') ORDER BY ID").ToList<ACTUALIZACIONPMES>();

                foreach (ACTUALIZACIONPMES actualizacion in pendientesActualizar)
                {
                    try
                    {
                        var fechaInicio = DateTime.Now;

                        if (registrosProcesados.Contains(actualizacion.ID_TERCERO.ToString() + "|" + actualizacion.ID_INSTALACION.ToString() + "|" + actualizacion.S_VALOR_VIGENCIA))
                        {
                            dbSIM.Database.ExecuteSqlCommand("DELETE FROM CONTROL.ENC_DATOS_MODIFICADOS WHERE ID = " + actualizacion.ID.ToString());
                        }
                        else
                        {
                            registrosProcesados.Add(actualizacion.ID_TERCERO.ToString() + "|" + actualizacion.ID_INSTALACION.ToString() + "|" + actualizacion.S_VALOR_VIGENCIA);

                            // Ejecución de los pendientes por actualizar

                            switch (actualizacion.ID_ENCUESTA)
                            {
                                case 1:
                                    sqlInstalacion = sql1.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                case 2:
                                    sqlInstalacion = sql2.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                case 3:
                                    sqlInstalacion = sql3.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                default:
                                    sqlInstalacion = sql1.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                            }

                            var datosSQL = dbSIM.Database.SqlQuery<string>(sqlInstalacion).ToList<string>();

                            dbSIM.Database.ExecuteSqlCommand("DELETE FROM CONTROL.PMES_DATOSCALCULADOS WHERE ID_TERCERO = " + actualizacion.ID_TERCERO.ToString() + " AND ID_INSTALACION = " + actualizacion.ID_INSTALACION.ToString() + " AND VIGENCIA = '" + actualizacion.S_VALOR_VIGENCIA + "' AND NVL(ID_ENCUESTA, 1) = " + actualizacion.ID_ENCUESTA.ToString());

                            foreach (string insert in datosSQL)
                            {
                                dbSIM.Database.ExecuteSqlCommand(insert);
                            }

                            var fechaFin = DateTime.Now;

                            dbSIM.Database.ExecuteSqlCommand("UPDATE CONTROL.ENC_DATOS_MODIFICADOS SET N_ERROR = 0, D_FECHA_INICIO = TO_DATE('" + fechaInicio.ToString("yyyyMMdd HH:mm:ss") + "', 'YYYYMMDD HH24:MI:SS'), D_FECHA_FIN = TO_DATE('" + fechaFin.ToString("yyyyMMdd HH:mm:ss") + "', 'YYYYMMDD HH24:MI:SS') WHERE ID = " + actualizacion.ID.ToString());
                        }
                    }
                    catch (Exception error)
                    {
                        dbSIM.Database.ExecuteSqlCommand("UPDATE CONTROL.ENC_DATOS_MODIFICADOS SET N_ERROR = 1 WHERE ID = " + actualizacion.ID.ToString());
                        SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "PMES [GetActualizarPMESVista - " + actualizacion.ID.ToString() + " ] : Se presentó un error generando la información calculada.\r\n" + SIM.Utilidades.LogErrores.ObtenerError(error));
                    }
                }

                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "PMES [GetActualizarPMESVista] : Fin de Ejecución (" + DateTime.Now.Subtract(fechaActual).TotalMinutes + " Minutos).");
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "PMES [GetActualizarPMESVista] : Fin de Ejecución con Error (" + DateTime.Now.Subtract(fechaActual).TotalMinutes + " Minutos).\r\n" + SIM.Utilidades.LogErrores.ObtenerError(error));

                return SIM.Utilidades.LogErrores.ObtenerError(error);
            }
            return "OK";
        }

        [HttpGet]
        [ActionName("ActualizarPMESVistaInstalacion")]
        public void GetActualizarPMESVistaInstalacion(int idTercero, int idInstalacion, string vigencia)
        {
            DateTime fechaActual = DateTime.Now;
            string sqlInstalacion;
            List<string> registrosProcesados = new List<string>();

            var sql = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_individual.txt"));

            sqlInstalacion = sql.Replace("#ID_TERCERO#", idTercero.ToString()).Replace("#ID_INSTALACION#", idInstalacion.ToString()).Replace("#S_VALOR_VIGENCIA#", vigencia);

            var datosSQL = dbSIM.Database.SqlQuery<string>(sqlInstalacion).ToList<string>();

            dbSIM.Database.ExecuteSqlCommand("DELETE FROM CONTROL.PMES_DATOSCALCULADOS WHERE ID_TERCERO = " + idTercero.ToString() + " AND ID_INSTALACION = " + idInstalacion.ToString() + " AND VIGENCIA = '" + vigencia + "'");

            foreach (string insert in datosSQL)
            {
                dbSIM.Database.ExecuteSqlCommand(insert);
            }
        }

        [HttpGet]
        [ActionName("ActualizarPMESVistaIDMasivo")]
        public string GetActualizarPMESVistaIDMasivo(string ids)
        {
            string errores = "";

            foreach (string id in ids.Split(','))
            {
                errores += "<br/>" + GetActualizarPMESVistaID(Convert.ToInt32(id));
            }

            return errores;
        }

        [HttpGet]
        [ActionName("ActualizarPMESVistaID")]
        public string GetActualizarPMESVistaID(int id)
        {
            DateTime fechaActual = DateTime.Now;
            string sqlInstalacion;
            List<string> registrosProcesados = new List<string>();

            try
            {
                var sql1 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion.txt"));
                var sql2 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion_2.txt"));
                var sql3 = File.ReadAllText(HostingEnvironment.MapPath("~/Content/recursos/pmes_instalacion_3.txt"));

                var pendientesActualizar = dbSIM.Database.SqlQuery<ACTUALIZACIONPMES>("SELECT ID, ID_ENCUESTA, ID_TERCERO, ID_INSTALACION, S_VALOR_VIGENCIA FROM CONTROL.ENC_DATOS_MODIFICADOS WHERE ID = " + id.ToString()).ToList<ACTUALIZACIONPMES>();

                foreach (ACTUALIZACIONPMES actualizacion in pendientesActualizar)
                {
                    try
                    {
                        var fechaInicio = DateTime.Now;

                        if (registrosProcesados.Contains(actualizacion.ID_TERCERO.ToString() + "|" + actualizacion.ID_INSTALACION.ToString() + "|" + actualizacion.S_VALOR_VIGENCIA))
                        {
                            dbSIM.Database.ExecuteSqlCommand("DELETE FROM CONTROL.ENC_DATOS_MODIFICADOS WHERE ID = " + actualizacion.ID.ToString());
                        }
                        else
                        {
                            registrosProcesados.Add(actualizacion.ID_TERCERO.ToString() + "|" + actualizacion.ID_INSTALACION.ToString() + "|" + actualizacion.S_VALOR_VIGENCIA);

                            // Ejecución de los pendientes por actualizar

                            switch (actualizacion.ID_ENCUESTA)
                            {
                                case 1:
                                    sqlInstalacion = sql1.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                case 2:
                                    sqlInstalacion = sql2.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                case 3:
                                    sqlInstalacion = sql3.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                                default:
                                    sqlInstalacion = sql1.Replace("#ID#", actualizacion.ID.ToString());
                                    break;
                            }

                            var datosSQL = dbSIM.Database.SqlQuery<string>(sqlInstalacion).ToList<string>();

                            dbSIM.Database.ExecuteSqlCommand("DELETE FROM CONTROL.PMES_DATOSCALCULADOS WHERE ID_TERCERO = " + actualizacion.ID_TERCERO.ToString() + " AND ID_INSTALACION = " + actualizacion.ID_INSTALACION.ToString() + " AND VIGENCIA = '" + actualizacion.S_VALOR_VIGENCIA + "' AND NVL(ID_ENCUESTA, 1) = " + actualizacion.ID_ENCUESTA.ToString());

                            foreach (string insert in datosSQL)
                            {
                                dbSIM.Database.ExecuteSqlCommand(insert);
                            }

                            var fechaFin = DateTime.Now;

                            dbSIM.Database.ExecuteSqlCommand("UPDATE CONTROL.ENC_DATOS_MODIFICADOS SET D_FECHA_INICIO = TO_DATE('" + fechaInicio.ToString("yyyyMMdd HH:mm:ss") + "', 'YYYYMMDD HH24:MI:SS'), D_FECHA_FIN = TO_DATE('" + fechaFin.ToString("yyyyMMdd HH:mm:ss") + "', 'YYYYMMDD HH24:MI:SS') WHERE ID = " + actualizacion.ID.ToString());
                        }
                    }
                    catch (Exception error)
                    {
                        SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "PMES [GetActualizarPMESVista - " + actualizacion.ID.ToString() + " ] : Se presentó un error generando la información calculada.\r\n" + SIM.Utilidades.LogErrores.ObtenerError(error));
                        return SIM.Utilidades.LogErrores.ObtenerError(error);
                    }
                }
            }
            catch (Exception error)
            {
                return SIM.Utilidades.LogErrores.ObtenerError(error);
            }
            return "OK";
        }

        private void CopiarEvaluacion(int eetBase, int eet)
        {
            // Evaluación del Tercero a Copiar
            EVALUACION_ENCUESTA_TERCERO evaluacionTBase = dbSIM.EVALUACION_ENCUESTA_TERCERO.Where(e => e.ID == eetBase).FirstOrDefault();
            //EVALUACION_ENCUESTA_TERCERO evaluacionT = dbSIM.EVALUACION_ENCUESTA_TERCERO.Where(e => e.ID == eet).FirstOrDefault();

            // Respuestas por Encuesta Tercero
            var respuestasT = dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA_TERCERO == eetBase).ToList();

            foreach (EVALUACION_RESPUESTA erBase in respuestasT)
            {

                EVALUACION_RESPUESTA erCopia = new EVALUACION_RESPUESTA();

                erCopia.ID_PREGUNTA = erBase.ID_PREGUNTA;
                erCopia.N_RESPUESTA = erBase.N_RESPUESTA;
                erCopia.S_RESPUESTA = erBase.S_RESPUESTA;
                erCopia.ID_EVALUACION_ENCUESTA = null;
                erCopia.ID_EVALUACION_ENCUESTA_TERCERO = eet;

                dbSIM.Entry(erCopia).State = EntityState.Added;
                dbSIM.SaveChanges();
            }

            // Estrategias por Tercero a Copiar
            var estrategiasT = dbSIM.EVALUACION_ESTRATEGIAS_T.Where(e => e.ID_EVALUACION_TERCERO == eetBase).ToList();

            foreach (EVALUACION_ESTRATEGIAS_T eeBase in estrategiasT)
            {
                EVALUACION_ESTRATEGIAS_T eeCopia = new EVALUACION_ESTRATEGIAS_T();

                eeCopia.ID_EVALUACION_TERCERO = eet;
                eeCopia.ID_ESTRATEGIA = eeBase.ID_ESTRATEGIA;
                eeCopia.S_OTRO = eeBase.S_OTRO;
                eeCopia.S_INDICADOR_MEDICION = eeBase.S_INDICADOR_MEDICION;
                eeCopia.S_UNIDADES_META = eeBase.S_UNIDADES_META;
                eeCopia.N_VALOR_META = eeBase.N_VALOR_META;
                eeCopia.N_VALOR_META_ALCANZAR = eeBase.N_VALOR_META_ALCANZAR;
                eeCopia.N_PRESUPUESTO = eeBase.N_PRESUPUESTO;

                dbSIM.Entry(eeCopia).State = EntityState.Added;
                dbSIM.SaveChanges();
            }

            // Instalaciones del Tercero a Copiar
            var evaluacionBase = dbSIM.EVALUACION_ENCUESTA.Where(e => e.ID_EVALUACION_TERCERO == eetBase).ToList();

            foreach (EVALUACION_ENCUESTA ee in evaluacionBase)
            {
                EVALUACION_ENCUESTA eeC = new EVALUACION_ENCUESTA();

                eeC.ID_EVALUACION_TERCERO = eet;
                eeC.ID_INSTALACION = ee.ID_INSTALACION;
                eeC.S_ESTADO = "R";
                eeC.S_RESULTADO = "P";
                eeC.D_FECHA_GENERACION = null;
                eeC.ID_USUARIO_GENERACION = null;
                eeC.S_COORDENADA = ee.S_COORDENADA;
                eeC.S_DIRECCION = ee.S_DIRECCION;
                eeC.N_CO2P = null;
                eeC.N_CO2I = null;
                eeC.S_OBSERVACIONES = null;
                eeC.S_EXCLUIR = ee.S_EXCLUIR;
                eeC.N_PM25P = null;
                eeC.N_PM25I = null;
                eeC.S_PRINCIPAL = ee.S_PRINCIPAL;

                dbSIM.Entry(eeC).State = EntityState.Added;
                dbSIM.SaveChanges();

                // Respuestas por Encuesta Instalación
                var respuestasI = dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA == ee.ID).ToList();

                foreach (EVALUACION_RESPUESTA erBase in respuestasI)
                {

                    EVALUACION_RESPUESTA erCopia = new EVALUACION_RESPUESTA();

                    erCopia.ID_PREGUNTA = erBase.ID_PREGUNTA;
                    erCopia.N_RESPUESTA = erBase.N_RESPUESTA;
                    erCopia.S_RESPUESTA = erBase.S_RESPUESTA;
                    erCopia.ID_EVALUACION_ENCUESTA = eeC.ID;
                    erCopia.ID_EVALUACION_ENCUESTA_TERCERO = null;

                    dbSIM.Entry(erCopia).State = EntityState.Added;
                    dbSIM.SaveChanges();

                    // Detalle por Encuesta Instalación
                    var detalleRespuesta = dbSIM.EVALUACION_RESPUESTA_DETALLE.Where(e => e.ID_EVALUACION_RESPUESTA == erBase.ID).ToList();

                    foreach (EVALUACION_ESTRATEGIAS_T eeBase in estrategiasT)
                    {
                        EVALUACION_ESTRATEGIAS_T eeCopia = new EVALUACION_ESTRATEGIAS_T();

                        eeCopia.ID_EVALUACION_TERCERO = eet;
                        eeCopia.ID_ESTRATEGIA = eeBase.ID_ESTRATEGIA;
                        eeCopia.S_OTRO = eeBase.S_OTRO;
                        eeCopia.S_INDICADOR_MEDICION = eeBase.S_INDICADOR_MEDICION;
                        eeCopia.S_UNIDADES_META = eeBase.S_UNIDADES_META;
                        eeCopia.N_VALOR_META = eeBase.N_VALOR_META;
                        eeCopia.N_VALOR_META_ALCANZAR = eeBase.N_VALOR_META_ALCANZAR;
                        eeCopia.N_PRESUPUESTO = eeBase.N_PRESUPUESTO;

                        dbSIM.Entry(eeCopia).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
            }
        }
    }
}