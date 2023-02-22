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
using SIM.Data.Control;
using SIM.Data.Tramites;
using SIM.Models;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class ENCUESTASINSTALACIONES
    {
        public int ID_TERCERO { get; set; }
        public int ID_INSTALACION { get; set; }
        public string S_VALOR_VIGENCIA { get; set; }
    }

    public class DATOSINSTALACION
    {
        public string INSTALACION { get; set; }
        public string S_COORDENADA { get; set; }
        public string S_DIRECCION { get; set; }
        public decimal CO2P { get; set; }
        public decimal CO2I { get; set; }
        public decimal PM25P { get; set; }
        public decimal PM25I { get; set; }
    }

    public class PMESEvaluacionController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        decimal idTerceroUsuario;

        [Authorize(Roles = "VPMESEVALUACION")]
        // ve: Versión (PMES 1 o PMES 2)
        // t: Tipo de evaluación (1 PMES, 2 Seguimiento PMES)
        // c: Tipo de evaluación (0 Normal, 1 Copia resultado de la última evaluación (Ajustes de Planes))
        public ActionResult EvaluacionTerceros(int? ve, int? t, int? c)
        {
            int version = ve ?? 1;
            int tipo = t ?? 1;
            int copia = c ?? 0;

            string titulo = (from et in dbSIM.EVALUACION_TIPO
                          where et.ID == tipo
                          select et.S_NOMBRE).FirstOrDefault();

            if (copia == 0)
                titulo = titulo.Replace("%v", "Versión " + version.ToString());
            else
                titulo = titulo.Replace("%v", "Ajuste");

            ViewBag.Titulo = titulo;
            ViewBag.Version = version;
            ViewBag.Tipo = tipo;
            ViewBag.Copia = copia;
            return View();
        }

        // id: Id Tercero
        // v: Vigencia
        [Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionTercero(int? ve, int? id, int? ter, string v, int? tra, int? t, int? c)
        {
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero;
            int tipo = t ?? 1;
            string version = (ve ?? 1).ToString();
            int copia = c ?? 0;

            if (copia == 1)
            {
                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID_TERCERO == ter && eet.S_VALOR_VIGENCIA == v
                                             orderby eet.ID descending
                                             select eet
                                             ).FirstOrDefault();

                var evaluacionEncuestaTerceroCopia = new EVALUACION_ENCUESTA_TERCERO();



            }

            string titulo = (from et in dbSIM.EVALUACION_TIPO
                             where et.ID == tipo
                             select et.S_NOMBRE).FirstOrDefault();

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
                ve = Convert.ToInt32(evaluacionEncuestaTercero.S_VERSION ?? "1");
                t = evaluacionEncuestaTercero.ID_EVALUACION_TIPO;

                titulo = titulo.Replace("%v", "Versión " + ve.ToString());

                ViewBag.Titulo = titulo;
            }
            else
            {
                titulo = titulo.Replace("%v", "Versión " + ve.ToString());

                ViewBag.Titulo = titulo;

                if (ter == null || v == null || tra == null)
                    return null;

                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                             where eet.ID_TERCERO == ter && eet.S_VALOR_VIGENCIA == v && eet.ID_EVALUACION_TIPO == t && (eet.S_VERSION ?? "1") == version
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
                              where ee.CODTRAMITE == (int)tra && ee.ID_EVALUACION_TIPO == tipo
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
                                    where ee.CODTRAMITE == (int)tra && ee.ID_TERCERO == (int)ter && ee.S_VALOR_VIGENCIA == v && ee.ID_EVALUACION_TIPO == tipo
                                    select new datosRadicado { S_RADICADO = ee.S_RADICADO, D_FECHA = ee.D_RADICADO }).FirstOrDefault();
                    }
                    else
                    {
                        medioEntregaSIM = true;
                    }

                    evaluacionEncuestaTercero = new EVALUACION_ENCUESTA_TERCERO();

                    t = t ?? 1;

                    evaluacionEncuestaTercero.ID_EVALUACION_TIPO = (int)t; // PMES
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
                    evaluacionEncuestaTercero.S_VERSION = (ve ?? 1).ToString();

                    dbSIM.Entry(evaluacionEncuestaTercero).State = EntityState.Added;

                    dbSIM.SaveChanges();
                }
            }

            var tercero = (from te in dbSIM.TERCERO
                            where te.ID_TERCERO == ter
                            select te.S_RSOCIAL).FirstOrDefault();

            ViewBag.IdEvaluacionEncuestaTercero = evaluacionEncuestaTercero.ID;
            ViewBag.IdTercero = (int)ter;
            ViewBag.Tercero = tercero;
            ViewBag.ValorVigencia = v;
            ViewBag.Version = ve ?? 1;
            ViewBag.Tipo = t ?? 1;
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
        public ActionResult EvaluacionEncuesta(int? idee, int? ideet, int? i, string v, int t)
        {
            string estado = "";
            EVALUACION_ENCUESTA evaluacionEncuesta;
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero;

            // ENCUESTA PMES VERSION 1
            if ((v == null || v == "1") && t == 1)
            {
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
                        evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, (int)i, false, false, dbSIM);
                    }
                    else
                    {
                        return null;
                    }
                }

                ViewBag.IdEvaluacionEncuesta = evaluacionEncuesta.ID;

                dynamic datosVigencia = EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);

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

                    evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                     where eet.ID == evaluacionEncuesta.ID_EVALUACION_TERCERO
                                                     select eet).FirstOrDefault();

                    ViewBag.MedioEntrega = (evaluacionEncuestaTercero.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                    ViewBag.Radicado = evaluacionEncuestaTercero.S_RADICADO;
                    ViewBag.FechaEntrega = (evaluacionEncuestaTercero.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuestaTercero.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");
                    ViewBag.Coordenada = evaluacionEncuesta.S_COORDENADA;
                    ViewBag.Direccion = evaluacionEncuesta.S_DIRECCION;
                    ViewBag.TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : "");
                    ViewBag.KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : "");
                    ViewBag.PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : "");
                    ViewBag.PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : "");

                    ViewBag.Resultado = evaluacionEncuesta.S_RESULTADO;

                    ViewBag.SoloLectura = (evaluacionEncuestaTercero.S_ESTADO == "G" ? "S" : "N");

                    return View();
                }
                else
                {
                    return null;
                }
            }
            else if (t == 1 && v == "2") // ENCUESTA PMES VERSION 2
            {
                string sql = "SELECT ti.ID_TERCERO, " +
                    "ti.ID_INSTALACION, " +
                    "eet.S_VALOR_VIGENCIA " +
                    "FROM GENERAL.TERCERO_INSTALACION ti INNER JOIN " +
                    "  CONTROL.EVALUACION_ENCUESTA_TERCERO eet ON ti.ID_TERCERO = eet.ID_TERCERO LEFT OUTER JOIN " +
                    "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND ti.ID_INSTALACION = ee.ID_INSTALACION " +
                    "WHERE eet.ID = :ideet AND ee.ID IS NULL";

                OracleParameter idEETParameter = new OracleParameter("ideet", ideet);

                var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { idEETParameter }).ToList();

                foreach (ENCUESTASINSTALACIONES ei in instalacionesEvaluacion)
                {
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, ei.ID_INSTALACION, false, false, dbSIM);
                    EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);
                }

                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                 where eet.ID == ideet
                                                 select eet).FirstOrDefault();

                int idtercero = evaluacionEncuestaTercero.ID_TERCERO;
                var tercero = (from ter in dbSIM.TERCERO
                               where ter.ID_TERCERO == idtercero
                               select ter.S_RSOCIAL).FirstOrDefault();

                estado = evaluacionEncuestaTercero.S_ESTADO;

                ViewBag.Tercero = tercero;

                ViewBag.ValorVigencia = evaluacionEncuestaTercero.S_VALOR_VIGENCIA;
                ViewBag.MedioEntrega = (evaluacionEncuestaTercero.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                ViewBag.Radicado = evaluacionEncuestaTercero.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuestaTercero.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuestaTercero.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");

                var evaluacionEncuestas = (from ee in dbSIM.EVALUACION_ENCUESTA
                                           join ins in dbSIM.INSTALACION on ee.ID_INSTALACION equals ins.ID_INSTALACION
                                           where ee.ID_EVALUACION_TERCERO == (int)ideet && ee.S_EXCLUIR == "N"
                                           orderby ins.S_NOMBRE
                                           select new DATOSINSTALACION
                                           {
                                               INSTALACION = ins.S_NOMBRE,
                                               S_COORDENADA = ee.S_COORDENADA,
                                               S_DIRECCION = ee.S_DIRECCION,
                                               CO2P = (ee.N_CO2P != null ? ((decimal)ee.N_CO2P) : 0),
                                               CO2I = (ee.N_CO2I != null ? ((decimal)ee.N_CO2I) : 0),
                                               PM25P = (ee.N_PM25P != null ? ((decimal)ee.N_PM25P) : 0),
                                               PM25I = (ee.N_PM25I != null ? ((decimal)ee.N_PM25I) : 0),
                                           }).ToList();

                ViewBag.DatosInstalacion = evaluacionEncuestas;

                ViewBag.IdEvaluacionEncuestaTercero = ideet;
                ViewBag.SoloLectura = (estado == "G" || estado == "" ? "S" : "N");
                ViewBag.Resultado = evaluacionEncuestaTercero.S_RESULTADO;

                return View("EvaluacionEncuestav2");
            }
            else // SEGUIMIENTO DE PLANES MES
            {
                evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                 where eet.ID == ideet
                                                 select eet).FirstOrDefault();

                EvaluacionEncuestaUtilidad.AlimentarRespuestasInicialesT(evaluacionEncuestaTercero, dbSIM);

                int idtercero = evaluacionEncuestaTercero.ID_TERCERO;
                var tercero = (from ter in dbSIM.TERCERO
                               where ter.ID_TERCERO == idtercero
                               select ter.S_RSOCIAL).FirstOrDefault();

                estado = evaluacionEncuestaTercero.S_ESTADO;

                ViewBag.Tercero = tercero;

                ViewBag.ValorVigencia = evaluacionEncuestaTercero.S_VALOR_VIGENCIA;
                ViewBag.MedioEntrega = (evaluacionEncuestaTercero.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                ViewBag.Radicado = evaluacionEncuestaTercero.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuestaTercero.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuestaTercero.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");

                string sql = "SELECT ti.ID_TERCERO, " +
                    "ti.ID_INSTALACION, " +
                    "eet.S_VALOR_VIGENCIA " +
                    "FROM GENERAL.TERCERO_INSTALACION ti INNER JOIN " +
                    "  CONTROL.EVALUACION_ENCUESTA_TERCERO eet ON ti.ID_TERCERO = eet.ID_TERCERO LEFT OUTER JOIN " +
                    "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND ti.ID_INSTALACION = ee.ID_INSTALACION " +
                    "WHERE eet.ID = :ideet AND ee.ID IS NULL";

                OracleParameter idEETParameter = new OracleParameter("ideet", ideet);

                var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { idEETParameter }).ToList();

                foreach (ENCUESTASINSTALACIONES ei in instalacionesEvaluacion)
                {
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, ei.ID_INSTALACION, false, false, dbSIM);
                }

                var evaluacionEncuestas = (from ee in dbSIM.EVALUACION_ENCUESTA
                                           join ins in dbSIM.INSTALACION on ee.ID_INSTALACION equals ins.ID_INSTALACION
                                           where ee.ID_EVALUACION_TERCERO == (int)ideet && ee.S_EXCLUIR == "N"
                                           orderby ins.S_NOMBRE
                                           select new DATOSINSTALACION
                                           {
                                               INSTALACION = ins.S_NOMBRE,
                                               S_COORDENADA = ee.S_COORDENADA,
                                               S_DIRECCION = ee.S_DIRECCION,
                                               CO2P = (ee.N_CO2P != null ? ((decimal)ee.N_CO2P) : 0),
                                               CO2I = (ee.N_CO2I != null ? ((decimal)ee.N_CO2I) : 0),
                                               PM25P = (ee.N_PM25P != null ? ((decimal)ee.N_PM25P) : 0),
                                               PM25I = (ee.N_PM25I != null ? ((decimal)ee.N_PM25I) : 0),
                                           }).ToList();

                ViewBag.DatosInstalacion = evaluacionEncuestas;

                ViewBag.IdEvaluacionEncuestaTercero = ideet;
                ViewBag.SoloLectura = (estado == "G" || estado == "" ? "S" : "N");
                ViewBag.Resultado = evaluacionEncuestaTercero.S_RESULTADO;

                return View("EvaluacionSeguimiento");
            }
        }

        /*
        [Authorize(Roles = "VPMESEVALUACION")]
        public ActionResult EvaluacionEncuestav2(int ideet)
        {
            EVALUACION_ENCUESTA evaluacionEncuesta;
            EVALUACION_ENCUESTA_TERCERO evaluacionEncuestaTercero;

            evaluacionEncuestaTercero = dbSIM.EVALUACION_ENCUESTA_TERCERO.Where(eet => eet.ID == ideet).FirstOrDefault();

            if (evaluacionEncuestaTercero == null)
            {
                return null;
            }
            else
            {
                string sql = "SELECT ti.ID_TERCERO, " +
                        "ti.ID_INSTALACION, " +
                        "eet.S_VALOR_VIGENCIA, " +
                        "FROM GENERAL.TERCERO_INSTALACION ti INNER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA_TERCERO eet ON ti.ID_TERCERO = eet.ID_TERCERO LEFT OUTER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND ti.ID_INSTALACION = ee.ID_INSTALACION " +
                        "WHERE eet.ID = :ideet AND ee.ID IS NULL";

                OracleParameter idEETParameter = new OracleParameter("ideet", ideet);

                var instalacionesEvaluacion = dbSIM.Database.SqlQuery<ENCUESTASINSTALACIONES>(sql, new object[] { idEETParameter }).ToList();

                foreach (ENCUESTASINSTALACIONES ei in instalacionesEvaluacion)
                {
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta(ideet, ei.ID_INSTALACION, false, false, dbSIM);
                }

                    ViewBag.IdEvaluacionEncuesta = evaluacionEncuesta.ID;

            dynamic datosVigencia = EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);

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

                var evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                 where eet.ID == evaluacionEncuesta.ID_EVALUACION_TERCERO
                                                 select eet).FirstOrDefault();

                ViewBag.MedioEntrega = (evaluacionEncuestaTercero.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                ViewBag.Radicado = evaluacionEncuestaTercero.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuestaTercero.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuestaTercero.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");
                ViewBag.Coordenada = evaluacionEncuesta.S_COORDENADA;
                ViewBag.Direccion = evaluacionEncuesta.S_DIRECCION;
                ViewBag.TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : "");
                ViewBag.KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : "");
                ViewBag.PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : "");
                ViewBag.PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : "");

                ViewBag.Resultado = evaluacionEncuesta.S_RESULTADO;

                ViewBag.SoloLectura = (evaluacionEncuestaTercero.S_ESTADO == "G" ? "S" : "N");

                return View();
                }
            }



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

            

            {
                DATOSCONSULTA resultado = new DATOSCONSULTA();
                resultado.numRegistros = instalacionesEvaluacion.Count();
                if (resultado.numRegistros == 0)
                    resultado.datos = null;
                else
                    resultado.datos = (IEnumerable<dynamic>)instalacionesEvaluacion.AsEnumerable();

                return resultado;
            }


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
                    evaluacionEncuesta = EvaluacionEncuestaUtilidad.GenerarEvaluacionEncuesta((int)ideet, (int)i, false, false, dbSIM);
                }
                else
                {
                    return null;
                }
            }

            ViewBag.IdEvaluacionEncuesta = evaluacionEncuesta.ID;

            dynamic datosVigencia = EvaluacionEncuestaUtilidad.AlimentarRespuestasIniciales(evaluacionEncuesta, dbSIM);

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

                var evaluacionEncuestaTercero = (from eet in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                                 where eet.ID == evaluacionEncuesta.ID_EVALUACION_TERCERO
                                                 select eet).FirstOrDefault();

                ViewBag.MedioEntrega = (evaluacionEncuestaTercero.S_MEDIO_ENTREGA == "S" ? "PLATAFORMA" : "FISICO - OFICIO");
                ViewBag.Radicado = evaluacionEncuestaTercero.S_RADICADO;
                ViewBag.FechaEntrega = (evaluacionEncuestaTercero.D_FECHA_ENTREGA != null ? ((DateTime)evaluacionEncuestaTercero.D_FECHA_ENTREGA).ToString("dd/MM/yyyy") : "");
                ViewBag.Coordenada = evaluacionEncuesta.S_COORDENADA;
                ViewBag.Direccion = evaluacionEncuesta.S_DIRECCION;
                ViewBag.TonEmitido = (evaluacionEncuesta.N_CO2P != null ? ((decimal)evaluacionEncuesta.N_CO2P).ToString("#,##0.0000") : "");
                ViewBag.KgEmitido = (evaluacionEncuesta.N_CO2I != null ? ((decimal)evaluacionEncuesta.N_CO2I).ToString("#,##0.0000") : "");
                ViewBag.PM25PEmitido = (evaluacionEncuesta.N_PM25P != null ? ((decimal)evaluacionEncuesta.N_PM25P).ToString("#,##0.0000") : "");
                ViewBag.PM25IEmitido = (evaluacionEncuesta.N_PM25I != null ? ((decimal)evaluacionEncuesta.N_PM25I).ToString("#,##0.0000") : "");

                ViewBag.Resultado = evaluacionEncuesta.S_RESULTADO;

                ViewBag.SoloLectura = (evaluacionEncuestaTercero.S_ESTADO == "G" ? "S" : "N");

                return View();
            }
            else
            {
                return null;
            }

            return null;
        }*/

        [Authorize(Roles = "VPMESEVALUACION")]
        [HttpGet, ActionName("GenerarDocumentoEvaluacion")]
        public ActionResult GetGenerarDocumentoEvaluacion(int eet, string v, int? t)
        {
            t = (t ?? 1);

            var encuestaTercero = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                   where et.ID == eet
                                   select et).FirstOrDefault();

            if (encuestaTercero.S_ESTADO == "G")
                return null;

            int tipoOficio = 0; // 1 Aprobación, 2 Requerimiento

            dynamic datosTerceroEvaluacion;

            if (t == 1)
            {
                datosTerceroEvaluacion = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                          join ter in dbSIM.TERCERO on et.ID_TERCERO equals ter.ID_TERCERO
                                          join ee in dbSIM.EVALUACION_ENCUESTA on et.ID equals ee.ID_EVALUACION_TERCERO
                                          join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                          join m in dbSIM.TBMUNICIPIO on i.ID_DIVIPOLA equals m.ID_DIVIPOLA into mid
                                          from mi in mid.DefaultIfEmpty()
                                          where et.ID == eet
                                          orderby ee.S_PRINCIPAL descending
                                          select new
                                          {
                                              //S_REPRESENTANTE_LEGAL = t.CONTACTOS.Where(c => c.TIPO == "R").OrderBy(c => c.D_FIN).FirstOrDefault(),
                                              et.ID_TERCERO,
                                              N_DOCUMENTO = ter.N_DOCUMENTON,
                                              ter.S_RSOCIAL,
                                              i.ID_INSTALACION,
                                              et.S_RADICADO,
                                              et.D_FECHA_ENTREGA,
                                              et.S_CM,
                                              et.S_ESTADO,
                                              et.S_RESULTADO,
                                              //et.N_COD_MUNICIPIO
                                              CODIGO_MUNICIPIO = (mi == null ? 0 : mi.CODIGO_MUNICIPIO),
                                              et.S_OBSERVACIONES
                                          }).FirstOrDefault();
            }
            else
            {
                var instalacionTercero = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                         join ee in dbSIM.EVALUACION_ENCUESTA on et.ID equals ee.ID_EVALUACION_TERCERO
                                         join i in dbSIM.INSTALACION on ee.ID_INSTALACION equals i.ID_INSTALACION
                                         join m in dbSIM.TBMUNICIPIO on i.ID_DIVIPOLA equals m.ID_DIVIPOLA into mid
                                         from mi in mid.DefaultIfEmpty()
                                         where et.ID_TERCERO == encuestaTercero.ID_TERCERO && et.S_VALOR_VIGENCIA == encuestaTercero.S_VALOR_VIGENCIA && et.ID_EVALUACION_TIPO == 2
                                         orderby ee.S_PRINCIPAL descending
                                         select new
                                         {
                                             i.ID_INSTALACION,
                                             CODIGO_MUNICIPIO = (mi == null ? 0 : mi.CODIGO_MUNICIPIO),
                                         }).FirstOrDefault();

                datosTerceroEvaluacion = (from et in dbSIM.EVALUACION_ENCUESTA_TERCERO
                                          join ter in dbSIM.TERCERO on et.ID_TERCERO equals ter.ID_TERCERO
                                          where et.ID == eet
                                          select new
                                          {
                                              et.ID_TERCERO,
                                              N_DOCUMENTO = ter.N_DOCUMENTON,
                                              ter.S_RSOCIAL,
                                              ID_INSTALACION = instalacionTercero.ID_INSTALACION,
                                              et.S_RADICADO,
                                              et.D_FECHA_ENTREGA,
                                              et.S_CM,
                                              et.S_ESTADO,
                                              et.S_RESULTADO,
                                              CODIGO_MUNICIPIO = instalacionTercero.CODIGO_MUNICIPIO,
                                              et.S_OBSERVACIONES
                                          }).FirstOrDefault();
            }



            if (datosTerceroEvaluacion == null || datosTerceroEvaluacion.S_ESTADO == "G")
                return null;

            int idTercero = datosTerceroEvaluacion.ID_TERCERO;

            var representanteLegal = (from c in dbSIM.CONTACTOS
                                      where c.ID_JURIDICO == idTercero
                                      orderby c.D_INICIO descending
                                      select c.TERCERO.S_RSOCIAL).FirstOrDefault();

            var instalacion = datosTerceroEvaluacion.ID_INSTALACION;

            var datosInstalacionPrincipal = ((IEnumerable<datosInstalacion>)dbSIM.Database.SqlQuery<datosInstalacion>("SELECT tvp.S_ABREVIATURA || ' '  || i.N_NUMEROVIAPPAL || lvp.S_NOMBRE || ' ' || i.S_SENTIDOVIAPPAL || ' ' || tvs.S_ABREVIATURA || ' ' || i.N_NUMEROVIASEC || lvs.S_NOMBRE || ' ' || i.S_SENTIDOVIASEC || '-' || i.N_PLACA || ' ' || i.N_INTERIOR AS S_DIRECCION, i.S_TELEFONO AS S_TELEFONO, t.S_CORREO, d.S_NOMBRE AS S_MUNICIPIO FROM GENERAL.INSTALACION i LEFT OUTER JOIN GENERAL.DIVIPOLA d ON i.ID_DIVIPOLA = d.ID_DIVIPOLA LEFT OUTER JOIN GENERAL.TERCERO_INSTALACION ti ON i.ID_INSTALACION = ti.ID_INSTALACION LEFT OUTER JOIN GENERAL.TERCERO t ON ti.ID_TERCERO = t.ID_TERCERO LEFT OUTER JOIN GENERAL.TIPO_VIA tvp ON i.ID_TIPOVIAPPAL = tvp.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvp ON i.ID_LETRAVIAPPAL = lvp.ID_LETRAVIA LEFT OUTER JOIN GENERAL.TIPO_VIA tvs ON i.ID_TIPOVIASEC = tvs.ID_TIPOVIA LEFT OUTER JOIN GENERAL.LETRA_VIA lvs ON i.ID_LETRAVIASEC = lvs.ID_LETRAVIA WHERE i.ID_INSTALACION = " + instalacion.ToString())).FirstOrDefault();

            List<encuestasInstalaciones> instalacionesEvaluacion = null;

            if (t == 1)
            {
                string sql = "SELECT ee.ID AS ID_EVALUACION_ENCUESTA, " +
                        "eet.ID AS ID_EVALUACION_TERCERO, " +
                        "t.ID_TERCERO, " +
                        "i.ID_INSTALACION, " +
                        "i.S_NOMBRE INSTALACION, " +
                        "CASE WHEN NVL(ee.S_ESTADO, 'R') = 'R' THEN 'P' ELSE ee.S_RESULTADO END RESULTADO, " +
                        "CASE WHEN NVL(eet.S_ESTADO, 'R') = 'R' THEN 'P' ELSE eet.S_RESULTADO END RESULTADO_TERCERO " +
                        "FROM CONTROL.EVALUACION_ENCUESTA_TERCERO eet INNER JOIN " +
                        "  GENERAL.TERCERO t ON eet.ID_TERCERO = t.ID_TERCERO INNER JOIN " +
                        "  GENERAL.TERCERO_INSTALACION ti ON t.ID_TERCERO = ti.ID_TERCERO INNER JOIN " +
                        "  GENERAL.INSTALACION i ON ti.ID_INSTALACION = i.ID_INSTALACION LEFT OUTER JOIN " +
                        "  CONTROL.EVALUACION_ENCUESTA ee ON eet.ID = ee.ID_EVALUACION_TERCERO AND i.ID_INSTALACION = ee.ID_INSTALACION " +
                        "WHERE eet.ID = :eet AND NVL(ee.S_EXCLUIR, 'N') = 'N' " +
                        "ORDER BY i.S_NOMBRE";

                OracleParameter ideetParameter = new OracleParameter("eet", eet);

                instalacionesEvaluacion = dbSIM.Database.SqlQuery<encuestasInstalaciones>(sql, new object[] { ideetParameter }).ToList();

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
            else
            {
                if (datosTerceroEvaluacion.S_RESULTADO == "C")
                    tipoOficio = 1;
                else
                    tipoOficio = 2;
            }

            if (tipoOficio == 0)
                return null;

            var stream = new MemoryStream();

            DocX documentoEncabezado;

            if (t == 1)
            {
                switch (v)
                {
                    case "2":
                        documentoEncabezado = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Encabezado2.docx"));
                        break;
                    default:
                        documentoEncabezado = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Encabezado.docx"));
                        break;
                }
            }
            else
            {
                if (tipoOficio == 1)
                    documentoEncabezado = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_SeguimientoAprobacion.docx"));
                else
                    documentoEncabezado = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_SeguimientoRequerimiento.docx"));
            }

            //if (datosTerceroEvaluacion.S_REPRESENTANTE_LEGAL != null)
            if (representanteLegal != null)
                documentoEncabezado.ReplaceText("{REPRESENTANTE LEGAL}", representanteLegal);
            if (datosTerceroEvaluacion != null && datosTerceroEvaluacion.S_RSOCIAL != null)
                documentoEncabezado.ReplaceText("{TERCERO}", datosTerceroEvaluacion.S_RSOCIAL.ToString());
            if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_DIRECCION != null)
                documentoEncabezado.ReplaceText("{DIRECCION}", datosInstalacionPrincipal.S_DIRECCION);
            if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_CORREO != null)
                documentoEncabezado.ReplaceText("{EMAIL}", datosInstalacionPrincipal.S_CORREO);
            if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_TELEFONO != null)
                documentoEncabezado.ReplaceText("{TELEFONO}", datosInstalacionPrincipal.S_TELEFONO);
            if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_MUNICIPIO != null)
                documentoEncabezado.ReplaceText("{MUNICIPIO}", datosInstalacionPrincipal.S_MUNICIPIO);

            CultureInfo esCO = new CultureInfo("es-CO");

            documentoEncabezado.ReplaceText("{RADICADO}", datosTerceroEvaluacion.S_RADICADO.ToString() + " del " + Convert.ToDateTime(datosTerceroEvaluacion.D_FECHA_ENTREGA).ToString("dd \\de MMMM \\de yyyy", esCO));

            if (t == 1 && v == "2")
            {
                var preguntasEvaluacionT = from pe in dbSIM.EVALUACION_PREGUNTA
                                           join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA_TERCERO == eet) on pe.ID equals er.ID_PREGUNTA into perj
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

                for (int idPregunta = 101; idPregunta <= 105; idPregunta++)
                {
                    var respuesta = preguntasEvaluacionT.Where(pe => pe.ID_PREGUNTA == idPregunta).FirstOrDefault();

                    if (respuesta != null)
                    {
                        documentoEncabezado.ReplaceText("{DP" + idPregunta.ToString() + "}", respuesta.S_PREGUNTA);

                        if (respuesta != null && respuesta.S_RESPUESTA != null)
                        {
                            documentoEncabezado.ReplaceText("{P" + idPregunta.ToString() + "}", respuesta.S_RESPUESTA);
                        }
                        else
                        {
                            documentoEncabezado.ReplaceText("{P" + idPregunta.ToString() + "}", "-");
                        }
                    }
                }
            }
            else if (t == 2)
            {
                var preguntasEvaluacionT = from pe in dbSIM.EVALUACION_PREGUNTA
                                           join er in dbSIM.EVALUACION_RESPUESTA.Where(e => e.ID_EVALUACION_ENCUESTA_TERCERO == eet) on pe.ID equals er.ID_PREGUNTA into perj
                                           from per in perj.DefaultIfEmpty()
                                           orderby pe.EVALUACION_PREGUNTA_GRUPO.N_ORDEN, pe.N_ORDEN
                                           select new
                                           {
                                               ID = per == null ? 0 : per.ID,
                                               ID_PREGUNTA = pe.ID,
                                               S_GRUPO = pe.EVALUACION_PREGUNTA_GRUPO.S_NOMBRE,
                                               S_PREGUNTA = pe.S_DESCRIPCION,
                                               N_TIPO_RESPUESTA = pe.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                               S_RESPUESTA = (pe.N_TIPO_RESPUESTA == 1 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "SI" : "NO") : (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA != null ? (per.N_RESPUESTA == 1 ? "CUMPLE" : "NO CUMPLE") : (pe.N_TIPO_RESPUESTA == 3 && per.N_RESPUESTA != null ? per.N_RESPUESTA.ToString() : (pe.N_TIPO_RESPUESTA == 4 && per.N_RESPUESTA != null ? (per.N_RESPUESTA * 100).ToString() + " %" : (pe.N_TIPO_RESPUESTA == 5 && per.N_RESPUESTA != null) ? per.S_RESPUESTA : (pe.N_TIPO_RESPUESTA == 10 && per.N_RESPUESTA == 2 ? "20%" : "10%"))))),
                                               S_RESPUESTA_AUX = (pe.N_TIPO_RESPUESTA == 1 && per.N_RESPUESTA_AUX != null ? (per.N_RESPUESTA_AUX == 1 ? "SI" : "NO") : (pe.N_TIPO_RESPUESTA == 2 && per.N_RESPUESTA_AUX != null ? (per.N_RESPUESTA_AUX == 1 ? "CUMPLE" : "NO CUMPLE") : (pe.N_TIPO_RESPUESTA == 3 && per.N_RESPUESTA_AUX != null ? per.N_RESPUESTA_AUX.ToString() : (pe.N_TIPO_RESPUESTA == 4 && per.N_RESPUESTA_AUX != null ? (per.N_RESPUESTA_AUX * 100).ToString() + " %" : (pe.N_TIPO_RESPUESTA == 5 && per.N_RESPUESTA_AUX != null) ? per.S_RESPUESTA_AUX : null))))
                                           };

                var respuestaPeriodo = preguntasEvaluacionT.Where(pe => pe.ID_PREGUNTA == 1000).FirstOrDefault();

                for (int idPregunta = 1001; idPregunta <= 1010; idPregunta++)
                {
                    var respuesta = preguntasEvaluacionT.Where(pe => pe.ID_PREGUNTA == idPregunta).FirstOrDefault();

                    if (respuesta != null)
                    {
                        documentoEncabezado.ReplaceText("{DP" + idPregunta.ToString() + "}", (idPregunta == 1006 ? respuesta.S_PREGUNTA.Replace("#1000#", respuestaPeriodo.S_RESPUESTA) : respuesta.S_PREGUNTA));

                        if (respuesta != null && respuesta.S_RESPUESTA != null)
                        {
                            documentoEncabezado.ReplaceText("{P" + idPregunta.ToString() + "}", respuesta.S_RESPUESTA);
                        }
                        else
                        {
                            documentoEncabezado.ReplaceText("{P" + idPregunta.ToString() + "}", "-");
                        }

                        if (idPregunta == 1004)
                        {
                            documentoEncabezado.ReplaceText("{CO2}", respuesta.S_RESPUESTA_AUX);
                        }

                        if (idPregunta == 1008)
                        {
                            documentoEncabezado.ReplaceText("{PM25}", respuesta.S_RESPUESTA_AUX);
                        }
                    }
                }
            }

            if (t == 1)
            {
                foreach (encuestasInstalaciones instalacionEvaluacion in instalacionesEvaluacion)
                {
                    var encuestaInstalacion = (from ee in dbSIM.EVALUACION_ENCUESTA
                                               where ee.ID == instalacionEvaluacion.ID_EVALUACION_ENCUESTA
                                               select ee).FirstOrDefault();

                    DocX documentoEvaluacion;

                    switch (v)
                    {
                        case "2":
                            documentoEvaluacion = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Evaluacion2.docx"));
                            documentoEvaluacion.ReplaceText("{CC}", encuestaInstalacion.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE");
                            break;
                        default:
                            documentoEvaluacion = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Evaluacion.docx"));
                            break;
                    }

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

                    switch (v)
                    {
                        case "2":
                            for (int idPregunta = 106; idPregunta <= 138; idPregunta++)
                            {
                                var respuesta = preguntasEvaluacion.Where(pe => pe.ID_PREGUNTA == idPregunta).FirstOrDefault();

                                documentoEvaluacion.ReplaceText("{DP" + idPregunta.ToString() + "}", respuesta.S_PREGUNTA);

                                if (respuesta != null && respuesta.S_RESPUESTA != null)
                                {
                                    documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", respuesta.S_RESPUESTA);
                                }
                                else
                                {
                                    documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", "-");
                                }
                            }

                            documentoEncabezado.Tables[0].InsertTableAfterSelf(documentoEvaluacion.Tables[0]);
                            break;
                        default:
                            for (int idPregunta = 1; idPregunta <= 59; idPregunta++)
                            {
                                var respuesta = preguntasEvaluacion.Where(pe => pe.ID_PREGUNTA == idPregunta).FirstOrDefault();

                                if (respuesta != null && respuesta.S_RESPUESTA != null)
                                    documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", respuesta.S_RESPUESTA);
                                else
                                    documentoEvaluacion.ReplaceText("{P" + idPregunta.ToString() + "}", "-");
                            }

                            documentoEncabezado.InsertDocument(documentoEvaluacion, true);
                            break;
                    }

                    documentoEvaluacion.Dispose();
                }
            }

            documentoEncabezado.ReplaceText("{OBSERVACIONES}", datosTerceroEvaluacion.S_OBSERVACIONES ?? "--------");

            if (t == 2)
            {
                if (datosTerceroEvaluacion != null && datosTerceroEvaluacion.N_DOCUMENTO != null)
                    documentoEncabezado.ReplaceText("{NIT}", datosTerceroEvaluacion.N_DOCUMENTO.ToString());
                if (datosTerceroEvaluacion.S_CM != null)
                    documentoEncabezado.ReplaceText("{CM}", datosTerceroEvaluacion.S_CM);
                documentoEncabezado.ReplaceText("{COD MUNICIPIO}", ((int)datosTerceroEvaluacion.CODIGO_MUNICIPIO).ToString("00"));

                var preguntasEstrategias = (from ee in dbSIM.EVALUACION_ESTRATEGIAS_T
                                            where ee.ID_EVALUACION_TERCERO == eet && ee.S_TIPO == "MC"
                                            orderby ee.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE, ee.EVALUACION_ESTRATEGIAS.S_NOMBRE
                                            select ee).ToList();
                int contMC = 1;
                foreach (var estrategia in preguntasEstrategias)
                {
                    documentoEncabezado.ReplaceText("{GP1_" + contMC.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE);
                    documentoEncabezado.ReplaceText("{E1_" + contMC.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.S_NOMBRE + (estrategia.S_OTRO == null || estrategia.S_OTRO == "" ? "" : " (" + estrategia.S_OTRO + ")"));
                    documentoEncabezado.ReplaceText("{I1_" + contMC.ToString("00") + "}", estrategia.S_INDICADOR_MEDICION ?? "");
                    documentoEncabezado.ReplaceText("{U1_" + contMC.ToString("00") + "}", estrategia.S_UNIDADES_META ?? "");
                    documentoEncabezado.ReplaceText("{M1_" + contMC.ToString("00") + "}", estrategia.N_VALOR_META.ToString());
                    documentoEncabezado.ReplaceText("{MA1_" + contMC.ToString("00") + "}", (estrategia.N_VALOR_META_ALCANZAR ?? 0).ToString());

                    contMC++;
                }

                for (int i = contMC; i < 31; i++)
                {
                    documentoEncabezado.Tables[0].RemoveRow(contMC+9);
                }

                documentoEncabezado.ReplaceText("{CUMP}", (encuestaTercero.S_RESULTADO == "C" ? "CUMPLE" : "NO CUMPLE"));

                //int idUsuario = 0;

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
                        documentoEncabezado.ReplaceText("{USUARIO GENERACION}", usuario.S_NOMBRES + " " + usuario.S_APELLIDOS);
                }
                else
                {
                    return null;
                }

                documentoEncabezado.ReplaceText("{TRAMITE}", encuestaTercero.CODTRAMITE.ToString());
                documentoEncabezado.SaveAs(stream);
                documentoEncabezado.Dispose();
            }
            else
            {
                DocX documentoFinal;

                switch (v)
                {
                    case "2":
                        documentoFinal = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Final" + (tipoOficio == 1 ? "Aprobacion" : "Requerimiento") + "2.docx"));
                        break;
                    default:
                        documentoFinal = DocX.Load(HostingEnvironment.MapPath(@"~/App_Data/Plantillas/OficioPMES_Final" + (tipoOficio == 1 ? "Aprobacion" : "Requerimiento") + ".docx"));
                        break;
                }

                //if (datosTerceroEvaluacion.S_REPRESENTANTE_LEGAL != null)
                if (representanteLegal != null)
                    documentoFinal.ReplaceText("{REPRESENTANTE LEGAL}", representanteLegal);
                if (datosTerceroEvaluacion != null && datosTerceroEvaluacion.S_RSOCIAL != null)
                    documentoFinal.ReplaceText("{TERCERO}", datosTerceroEvaluacion.S_RSOCIAL.ToString());
                if (datosTerceroEvaluacion != null && datosTerceroEvaluacion.N_DOCUMENTO != null)
                    documentoFinal.ReplaceText("{NIT}", datosTerceroEvaluacion.N_DOCUMENTO.ToString());
                if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_DIRECCION != null)
                    documentoFinal.ReplaceText("{DIRECCION}", datosInstalacionPrincipal.S_DIRECCION);
                if (datosInstalacionPrincipal != null && datosInstalacionPrincipal.S_MUNICIPIO != null)
                    documentoFinal.ReplaceText("{MUNICIPIO}", datosInstalacionPrincipal.S_MUNICIPIO);
                if (datosTerceroEvaluacion.S_CM != null)
                    documentoFinal.ReplaceText("{CM}", datosTerceroEvaluacion.S_CM);
                documentoFinal.ReplaceText("{COD MUNICIPIO}", ((int)datosTerceroEvaluacion.CODIGO_MUNICIPIO).ToString("00"));

                if (v == "2")
                {
                    var preguntasEstrategias = (from ee in dbSIM.EVALUACION_ESTRATEGIAS_T
                                                where ee.ID_EVALUACION_TERCERO == eet && ee.S_TIPO == "PE"
                                                orderby ee.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE, ee.EVALUACION_ESTRATEGIAS.S_NOMBRE
                                                select ee).ToList();
                    int contPE = 1;
                    foreach (var estrategia in preguntasEstrategias)
                    {
                        documentoFinal.ReplaceText("{GP1_" + contPE.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE);
                        documentoFinal.ReplaceText("{E1_" + contPE.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.S_NOMBRE + (estrategia.S_OTRO == null || estrategia.S_OTRO == "" ? "" : " (" + estrategia.S_OTRO + ")"));
                        documentoFinal.ReplaceText("{I1_" + contPE.ToString("00") + "}", estrategia.S_INDICADOR_MEDICION??"");
                        documentoFinal.ReplaceText("{U1_" + contPE.ToString("00") + "}", estrategia.S_UNIDADES_META??"");
                        documentoFinal.ReplaceText("{M1_" + contPE.ToString("00") + "}", estrategia.N_VALOR_META.ToString());

                        contPE++;
                    }

                    for (int i = contPE; i < 31; i++)
                    {
                        documentoFinal.Tables[0].RemoveRow(contPE);
                    }

                    var preguntasEstrategiasMovilidad = (from ee in dbSIM.EVALUACION_ESTRATEGIAS_T
                                                         where ee.ID_EVALUACION_TERCERO == eet && ee.S_TIPO == "EM"
                                                         orderby ee.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE, ee.EVALUACION_ESTRATEGIAS.S_NOMBRE
                                                         select ee).ToList();
                    int contEM = 1;
                    foreach (var estrategia in preguntasEstrategiasMovilidad)
                    {
                        documentoFinal.ReplaceText("{GP2_" + contEM.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.EVALUACION_ESTRATEGIAS_GRUPO.S_NOMBRE);
                        documentoFinal.ReplaceText("{E2_" + contEM.ToString("00") + "}", estrategia.EVALUACION_ESTRATEGIAS.S_NOMBRE + (estrategia.S_OTRO == null || estrategia.S_OTRO == "" ? "" : " (" + estrategia.S_OTRO + ")"));
                        documentoFinal.ReplaceText("{I2_" + contEM.ToString("00") + "}", estrategia.S_INDICADOR_MEDICION ?? "");
                        documentoFinal.ReplaceText("{U2_" + contEM.ToString("00") + "}", estrategia.S_UNIDADES_META ?? "");
                        documentoFinal.ReplaceText("{M2_" + contEM.ToString("00") + "}", estrategia.N_VALOR_META.ToString());

                        contEM++;
                    }

                    for (int i = contEM; i < 31; i++)
                    {
                        documentoFinal.Tables[1].RemoveRow(contEM);
                    }
                }

                //int idUsuario = 0;

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
            }

            string nombreArchivo;

            switch (tipoOficio)
            {
                case 1:
                    if (t == 1)
                        nombreArchivo = "Oficio Aprobación Plan MES.docx";
                    else
                        nombreArchivo = "Oficio Aprobación Seguimiento Plan MES.docx";
                    break;
                case 2:
                    if (t == 1)
                        nombreArchivo = "Oficio Requerimiento Plan MES.docx";
                    else
                        nombreArchivo = "Oficio Requerimiento Seguimiento Plan MES.docx";
                    break;
                default:
                    if (t == 1)
                        nombreArchivo = "Oficio Aprobación Plan MES.docx";
                    else
                        nombreArchivo = "Oficio Aprobación Seguimiento Plan MES.docx";
                    break;
            }

            //if (v != "2" && t == 1) // &&&&&& Quitar esta condición cuando se deseen almacenar nuevamente el documento y asociarlo al trámite. //$%
            //{//$%
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

                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);

                /*var versionActual = dbSIM.Database.SqlQuery<decimal?>("SELECT N_VERSION FROM TRAMITES.DOCUMENTO_TEMPORAL WHERE CODTRAMITE = " + encuestaTercero.CODTRAMITE + " AND S_DESCRIPCION = '" + Path.GetFileNameWithoutExtension(nombreArchivo) + "' ORDER BY N_VERSION DESC").FirstOrDefault();

                versionActual = (versionActual ?? 1) + 1;*/

                decimal versionActual = -1;

                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                dbTramites.SP_ASIGNAR_TEMPORAL_TRAMITE(encuestaTercero.CODTRAMITE, 4545, codFuncionario, -1, versionActual, Path.GetFileNameWithoutExtension(nombreArchivo), ruta, rtaResultado);

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
            //} //$%

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
        }
    }

    /*private class EvaluacionPreguntas
    {
        public int ID { get; set;}
        public int ID_PREGUNTA { get; set; }
        public string S_GRUPO { get; set; }
        public string S_PREGUNTA { get; set; }
        public int N_TIPO_RESPUESTA { get; set; }
        public string S_RESPUESTA { get; set; }
    }*/
}