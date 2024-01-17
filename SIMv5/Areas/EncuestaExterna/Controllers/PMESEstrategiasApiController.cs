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
using DocumentFormat.OpenXml.EMMA;
using SIM.Areas.GestionDocumental.Models;
using System.ComponentModel.DataAnnotations;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Serializing;
using Org.BouncyCastle.Utilities;
using static System.Net.WebRequestMethods;
using Independentsoft.Office.Odf.Styles;
using static SIM.Areas.EncuestaExterna.Controllers.PMESEvaluacionApiController;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class PMESEstrategiasApiController : ApiController
    {
        public struct DATOSCONSULTA
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct DATOSREGISTRO
        {
            public int idEstrategiaTercero;
            public int id;
            public int idPregunta;
            public int tipoRespuesta;
            public string valor;
        }

        public class META
        {
            public int? ID { get; set; }
            public int ID_ESTRATEGIA_TERCERO { get; set; }
            public int ID_META { get; set; }
            public String S_META { get; set; }
            public String S_MEDICION { get; set; }
            public decimal? N_VALOR { get; set; }
        }

        public class ESTRATEGIA_P
        {
            public int ID { get; set; }
            public int ID_GRUPO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public String S_ESTRATEGIA { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD { get; set; }
            public String S_ACTIVIDAD { get; set; }
            public int? ID_PERIODICIDAD { get; set; }
            public String S_PERIODICIDAD { get; set; }
            public String S_PUBLICO_OBJETIVO { get; set; }
            public String S_TIPOEVIDENCIA { get; set; }
            public String S_EVIDENCIAS { get; set; }
            public String S_TITULO { get; set; }
        }

        public class ESTRATEGIA_F
        {
            public int ID { get; set; }
            public int ID_GRUPO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public String S_ESTRATEGIA { get; set; }
            public String S_OBJETIVO { get; set; }
            public String S_PUBLICO_OBJETIVO { get; set; }
            public String S_COMUNICACION_INTERNA { get; set; }
            public String S_COMUNICACION_EXTERNA { get; set; }
            public String S_TITULO { get; set; }
        }

        public class ESTRATEGIA_ALMACENAR
        {
            public string TIPO { get; set; }
            public int ID { get; set; }
            public int ID_ESTRATEGIA_TERCERO { get; set; }
            public int ID_ESTRATEGIA { get; set; }
            public string S_OTRO { get; set; }
            public string S_PUBLICO_OBJETIVO { get; set; }
            public string S_TIPOEVIDENCIA { get; set; }
            public string S_OBJETIVO { get; set; }
            public string S_COMUNICACION_INTERNA { get; set; }
            public string S_COMUNICACION_EXTERNA { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD1 { get; set; }
            public string S_ACTIVIDAD1 { get; set; }
            public int? ID_PERIODICIDAD1 { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD2 { get; set; }
            public string S_ACTIVIDAD2 { get; set; }
            public int? ID_PERIODICIDAD2 { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD3 { get; set; }
            public string S_ACTIVIDAD3 { get; set; }
            public int? ID_PERIODICIDAD3 { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD4 { get; set; }
            public string S_ACTIVIDAD4 { get; set; }
            public int? ID_PERIODICIDAD4 { get; set; }
            public int? ID_ESTRATEGIA_ACTIVIDAD5 { get; set; }
            public string S_ACTIVIDAD5 { get; set; }
            public int? ID_PERIODICIDAD5 { get; set; }
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [HttpGet]
        [ActionName("EstrategiasGrupo")]
        public DATOSCONSULTA GetEstrategiasGrupo(int idEstrategiasTercero, int idGrupo)
        {
            DATOSCONSULTA resultado = new DATOSCONSULTA();

            var tipoDatos = (from eg in dbSIM.PMES_ESTRATEGIAS_GRUPO
                             where eg.ID == idGrupo
                             select eg).FirstOrDefault();

            if (tipoDatos.ID_ENCABEZADO == 1)
            {
                var estrategiasGrupo = (from etp in dbSIM.PMES_ESTRATEGIAS_TP.Where(eti => eti.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero)
                                        join a in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES on etp.ID equals a.ID_ESTRATEGIA_TP into ealj
                                        from eava in ealj.DefaultIfEmpty()
                                        join p in dbSIM.PMES_ESTRATEGIAS_PERIODICIDAD on eava.ID_PERIODICIDAD equals p.ID into eplj
                                        from epva in eplj.DefaultIfEmpty()
                                        join e in dbSIM.PMES_ESTRATEGIAS on etp.ID_ESTRATEGIA equals e.ID
                                        join g in dbSIM.PMES_ESTRATEGIAS_GRUPO on e.ID_GRUPO equals g.ID
                                        join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on etp.ID_ESTRATEGIA_TERCERO equals et.ID into etlj
                                        where e.ID_GRUPO == idGrupo
                                        orderby etp.ID_ESTRATEGIA
                                        select new ESTRATEGIA_P
                                        {
                                            ID = etp.ID,
                                            ID_GRUPO = e.ID_GRUPO,
                                            ID_ESTRATEGIA = etp.ID_ESTRATEGIA,
                                            S_ESTRATEGIA = e.S_NOMBRE + (e.S_NOMBRE.ToUpper().Trim() == "OTROS" ? " (" + etp.S_OTRO + ")": ""),
                                            ID_ESTRATEGIA_ACTIVIDAD = eava.ID,
                                            S_ACTIVIDAD = eava.S_ACTIVIDAD,
                                            ID_PERIODICIDAD = eava.ID_PERIODICIDAD,
                                            S_PERIODICIDAD = epva.S_PERIODICIDAD,
                                            S_PUBLICO_OBJETIVO = etp.S_PUBLICO_OBJETIVO,
                                            S_TIPOEVIDENCIA = etp.S_TIPOEVIDENCIA,
                                            S_TITULO = g.S_TITULO
                                        }).ToList();

                Dictionary<string, string> tiposEvidencia = new Dictionary<string, string>();

                foreach (ESTRATEGIA_P estrategia in estrategiasGrupo)
                {
                    if (!tiposEvidencia.ContainsKey(estrategia.S_TIPOEVIDENCIA))
                    {
                        List<int> listaEvidencias = estrategia.S_TIPOEVIDENCIA.Split(',').Select(x => Int32.Parse(x)).ToList();

                        var evidencias = (from te in dbSIM.PMES_ESTRATEGIAS_TIPOEVIDENCIA
                                          where listaEvidencias.Contains(te.ID)
                                          select te.S_TIPOEVIDENCIA).ToList();

                        tiposEvidencia.Add(estrategia.S_TIPOEVIDENCIA, string.Join(", ", evidencias));
                    }

                    estrategia.S_EVIDENCIAS = tiposEvidencia[estrategia.S_TIPOEVIDENCIA];
                }

                resultado.numRegistros = estrategiasGrupo.Count();
                resultado.datos = estrategiasGrupo;
            }
            else
            {
                var estrategiasGrupo = from etf in dbSIM.PMES_ESTRATEGIAS_TF.Where(eti => eti.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero)
                                       join e in dbSIM.PMES_ESTRATEGIAS on etf.ID_ESTRATEGIA equals e.ID
                                       join g in dbSIM.PMES_ESTRATEGIAS_GRUPO on e.ID_GRUPO equals g.ID
                                       join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on etf.ID_ESTRATEGIA_TERCERO equals et.ID
                                       where e.ID_GRUPO == idGrupo
                                       select new ESTRATEGIA_F
                                       {
                                           ID = etf.ID,
                                           ID_GRUPO = e.ID_GRUPO,
                                           ID_ESTRATEGIA = etf.ID_ESTRATEGIA,
                                           S_ESTRATEGIA = e.S_NOMBRE + (e.S_NOMBRE.ToUpper().Trim() == "OTROS" ? etf.S_OTRO : ""),
                                           S_OBJETIVO = etf.S_OBJETIVO,
                                           S_PUBLICO_OBJETIVO = etf.S_PUBLICO_OBJETIVO,
                                           S_COMUNICACION_INTERNA = etf.S_COMUNICACION_INTERNA,
                                           S_COMUNICACION_EXTERNA = etf.S_COMUNICACION_EXTERNA,
                                           S_TITULO = g.S_TITULO
                                       };

                resultado.numRegistros = estrategiasGrupo.Count();
                resultado.datos = estrategiasGrupo.ToList();
            }

            return resultado;
        }

        [HttpGet]
        [ActionName("MetasGrupo")]
        public DATOSCONSULTA GetMetasGrupo(int idEstrategiasTercero, int idGrupo)
        {
            DATOSCONSULTA resultado = new DATOSCONSULTA();

            var metasGrupo = from em in dbSIM.PMES_ESTRATEGIAS_METAS
                             join emv in dbSIM.PMES_ESTRATEGIAS_METAS_T.Where(eti => eti.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero) on em.ID equals emv.ID_ESTRATEGIAS_METAS into emlj
                             from emva in emlj.DefaultIfEmpty()
                             join et in dbSIM.PMES_ESTRATEGIAS_TERCERO.Where(eti => eti.ID == idEstrategiasTercero) on emva.ID_ESTRATEGIA_TERCERO equals et.ID into etlj
                             from eta in etlj.DefaultIfEmpty()
                             where em.ID_ESTRATEGIAS_GRUPO == idGrupo
                             orderby em.N_ORDEN
                             select new META
                             {
                                 ID = emva.ID,
                                 ID_META = em.ID,
                                 S_META = em.S_META,
                                 S_MEDICION = em.S_MEDICION,
                                 N_VALOR = emva.N_VALOR
                             };

            resultado.numRegistros = metasGrupo.Count();
            resultado.datos = metasGrupo.ToList();

            return resultado;
        }

        //[HttpPost]
        [HttpGet]
        [ActionName("MetasGrupoActualizar")]
        //public void PostMetasGrupoActualizar(int idEstrategiasTercero, int idEstrategiasMeta, int? id, decimal? valor)
        public void GetMetasGrupoActualizar(int idEstrategiasTercero, int idEstrategiasMeta, int? id, decimal? valor)
        {
            if (id != null)
            {
                var meta = (from em in dbSIM.PMES_ESTRATEGIAS_METAS_T
                           where em.ID == id
                           select em).FirstOrDefault();

                if (meta != null)
                {
                    meta.N_VALOR = valor;

                    dbSIM.Entry(meta).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
            } 
            else
            {
                
                var meta = new PMES_ESTRATEGIAS_METAS_T
                    {
                        ID_ESTRATEGIA_TERCERO = idEstrategiasTercero,
                        ID_ESTRATEGIAS_METAS = idEstrategiasMeta,
                        N_VALOR = valor
                    };
                dbSIM.Entry(meta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }
        }

        [HttpGet]
        [ActionName("PreguntasEncabezado")]
        public DATOSCONSULTA GetPreguntasEncabezado(int idEstrategiasTercero, int idEncabezado)
        {
            DATOSCONSULTA resultado = new DATOSCONSULTA();

            var PreguntasEncabezado = from ep in dbSIM.PMES_ESTRATEGIAS_PREGUNTA
                             join er in dbSIM.PMES_ESTRATEGIAS_RESPUESTA.Where(r => r.ID_ESTRATEGIA_TERCERO == idEstrategiasTercero) on ep.ID equals er.ID_PREGUNTA into erlj
                             from erva in erlj.DefaultIfEmpty()
                             where ep.ID_ENCABEZADO == idEncabezado
                             orderby ep.N_ORDEN
                             select new 
                             {
                                 ID = erva == null ? 0 : erva.ID,
                                 ID_PREGUNTA = ep.ID,
                                 S_PREGUNTA = ep.S_DESCRIPCION,
                                 ep.N_TIPO_RESPUESTA, // 1 Si/No, 2 Cumple/No Cumple, 3 Número, 4 % , 5 String
                                 erva.N_RESPUESTA,
                                 erva.S_RESPUESTA
                             };

            resultado.numRegistros = PreguntasEncabezado.Count();
            resultado.datos = PreguntasEncabezado.ToList();

            return resultado;
        }

        //[HttpPost]
        [HttpGet]
        [ActionName("PreguntasEncabezadoActualizar")]
        //public void PostPreguntasEncabezadoActualizar(int idEstrategiasTercero, int idEstrategiasMeta, int? id, decimal? valor)
        public void GetPreguntasEncabezadoActualizar(int idEstrategiasTercero, int idEstrategiasMeta, int? id, decimal? valor)
        {
            if (id != null)
            {
                var meta = (from em in dbSIM.PMES_ESTRATEGIAS_METAS_T
                            where em.ID == id
                            select em).FirstOrDefault();

                if (meta != null)
                {
                    meta.N_VALOR = valor;

                    dbSIM.Entry(meta).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
            }
            else
            {

                var meta = new PMES_ESTRATEGIAS_METAS_T
                {
                    ID_ESTRATEGIA_TERCERO = idEstrategiasTercero,
                    ID_ESTRATEGIAS_METAS = idEstrategiasMeta,
                    N_VALOR = valor
                };
                dbSIM.Entry(meta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }
        }

        [HttpPost]
        [ActionName("PreguntasEncabezadoActualizar")]
        public void PostPreguntasEncabezadoActualizar(DATOSREGISTRO respuestaEstrategia)
        {
            PMES_ESTRATEGIAS_RESPUESTA estrategiaRespuesta = null;
            CultureInfo culture = new CultureInfo("en-US");

            if (respuestaEstrategia.id != 0)
            {
                estrategiaRespuesta = (from er in dbSIM.PMES_ESTRATEGIAS_RESPUESTA
                                       where er.ID_ESTRATEGIA_TERCERO == respuestaEstrategia.idEstrategiaTercero && er.ID_PREGUNTA == respuestaEstrategia.idPregunta
                                       select er).FirstOrDefault();
            }

            if (estrategiaRespuesta == null) // No existe la respuesta de la pregunta, por lo tanto se crea
            {
                estrategiaRespuesta = new PMES_ESTRATEGIAS_RESPUESTA();
                estrategiaRespuesta.ID_ESTRATEGIA_TERCERO = respuestaEstrategia.idEstrategiaTercero;
                estrategiaRespuesta.ID_PREGUNTA = respuestaEstrategia.idPregunta;
                estrategiaRespuesta.N_RESPUESTA = null;
                estrategiaRespuesta.S_RESPUESTA = null;

                if (respuestaEstrategia.tipoRespuesta <= 4)
                {
                    estrategiaRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEstrategia.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    estrategiaRespuesta.S_RESPUESTA = respuestaEstrategia.valor;
                }

                dbSIM.Entry(estrategiaRespuesta).State = EntityState.Added;

                dbSIM.SaveChanges();
            }
            else
            {
                estrategiaRespuesta.N_RESPUESTA = null;
                estrategiaRespuesta.S_RESPUESTA = null;

                if (respuestaEstrategia.tipoRespuesta <= 4 && respuestaEstrategia.valor != null)
                {
                    estrategiaRespuesta.N_RESPUESTA = Convert.ToDecimal(respuestaEstrategia.valor.Replace(",", culture.NumberFormat.NumberDecimalSeparator).Replace(".", culture.NumberFormat.NumberDecimalSeparator), culture);
                }
                else
                {
                    estrategiaRespuesta.S_RESPUESTA = respuestaEstrategia.valor;
                }

                dbSIM.Entry(estrategiaRespuesta).State = EntityState.Modified;

                dbSIM.SaveChanges();
            }
        }

        [HttpGet]
        [ActionName("Estrategias")]
        public dynamic GetEstrategias()
        {
            var estrategias = from e in dbSIM.PMES_ESTRATEGIAS
                              orderby e.ID
                              select new
                              {
                                  id = e.ID,
                                  idGrupo = e.ID_GRUPO,
                                  estrategia = e.S_NOMBRE
                              };

            var tiposEvidencia = from te in dbSIM.PMES_ESTRATEGIAS_TIPOEVIDENCIA
                                 orderby te.N_ORDEN
                                 select new
                                 {
                                     id = te.ID,
                                     tipoEvidencia = te.S_TIPOEVIDENCIA
                                 };

            var periodicidades = from e in dbSIM.PMES_ESTRATEGIAS_PERIODICIDAD
                                 orderby e.ID
                                 select new
                                 {
                                     id = e.ID,
                                     periodicidad = e.S_PERIODICIDAD
                                 };

            return new
            {
                estrategias = estrategias.ToList(),
                tiposEvidencia = tiposEvidencia.ToList(),
                periodicidades = periodicidades.ToList()
            };
        }

        [HttpGet]
        [ActionName("DatosEstrategiaID")]
        public dynamic GetDatosEstrategiaID(string tipo, int id)
        {
            if (tipo == "P")
            {
                var datosEstrategia = (from tp in dbSIM.PMES_ESTRATEGIAS_TP
                                       join e in dbSIM.PMES_ESTRATEGIAS on tp.ID_ESTRATEGIA equals e.ID
                                       where tp.ID == id
                                       select new ESTRATEGIA_ALMACENAR
                                       {
                                           TIPO = "p",
                                           ID = tp.ID,
                                           ID_ESTRATEGIA_TERCERO = tp.ID_ESTRATEGIA_TERCERO,
                                           ID_ESTRATEGIA = tp.ID_ESTRATEGIA,
                                           S_OTRO = tp.S_OTRO,
                                           S_PUBLICO_OBJETIVO = tp.S_PUBLICO_OBJETIVO,
                                           S_TIPOEVIDENCIA = tp.S_TIPOEVIDENCIA
                                       }).FirstOrDefault();

                var actividades = (from a in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                   where a.ID_ESTRATEGIA_TP == id
                                   orderby a.ID
                                   select a).ToList();

                int i = 1;
                foreach (var a in actividades)
                {
                    switch (i)
                    {
                        case 1:
                            datosEstrategia.ID_ESTRATEGIA_ACTIVIDAD1 = a.ID;
                            datosEstrategia.S_ACTIVIDAD1 = a.S_ACTIVIDAD;
                            datosEstrategia.ID_PERIODICIDAD1 = a.ID_PERIODICIDAD;
                            break;
                        case 2:
                            datosEstrategia.ID_ESTRATEGIA_ACTIVIDAD2 = a.ID;
                            datosEstrategia.S_ACTIVIDAD2 = a.S_ACTIVIDAD;
                            datosEstrategia.ID_PERIODICIDAD2 = a.ID_PERIODICIDAD;
                            break;
                        case 3:
                            datosEstrategia.ID_ESTRATEGIA_ACTIVIDAD3 = a.ID;
                            datosEstrategia.S_ACTIVIDAD3 = a.S_ACTIVIDAD;
                            datosEstrategia.ID_PERIODICIDAD3 = a.ID_PERIODICIDAD;
                            break;
                        case 4:
                            datosEstrategia.ID_ESTRATEGIA_ACTIVIDAD4 = a.ID;
                            datosEstrategia.S_ACTIVIDAD4 = a.S_ACTIVIDAD;
                            datosEstrategia.ID_PERIODICIDAD4 = a.ID_PERIODICIDAD;
                            break;
                        case 5:
                            datosEstrategia.ID_ESTRATEGIA_ACTIVIDAD5 = a.ID;
                            datosEstrategia.S_ACTIVIDAD5 = a.S_ACTIVIDAD;
                            datosEstrategia.ID_PERIODICIDAD5 = a.ID_PERIODICIDAD;
                            break;
                    }

                    i++;
                }

                return datosEstrategia;
            }
            else if (tipo == "F")
            {
                var datosEstrategia = (from tf in dbSIM.PMES_ESTRATEGIAS_TF
                                       join e in dbSIM.PMES_ESTRATEGIAS on tf.ID_ESTRATEGIA equals e.ID
                                       where tf.ID == id
                                       select new ESTRATEGIA_ALMACENAR
                                       {
                                           TIPO = "p",
                                           ID = tf.ID,
                                           ID_ESTRATEGIA_TERCERO = tf.ID_ESTRATEGIA_TERCERO,
                                           ID_ESTRATEGIA = tf.ID_ESTRATEGIA,
                                           S_OTRO = tf.S_OTRO,
                                           S_OBJETIVO = tf.S_OBJETIVO,
                                           S_PUBLICO_OBJETIVO = tf.S_PUBLICO_OBJETIVO,
                                           S_COMUNICACION_INTERNA = tf.S_COMUNICACION_INTERNA,
                                           S_COMUNICACION_EXTERNA = tf.S_COMUNICACION_EXTERNA
                                       }).FirstOrDefault();

                return datosEstrategia;
            }

            return null;
        }

        [HttpPost]
        [ActionName("InsertarEstrategia")]
        public void PostInsertarEstrategia(ESTRATEGIA_ALMACENAR ea)
        {
            try
            {
                if (ea.TIPO == "P")
                {
                    var nuevaEstrategia = new PMES_ESTRATEGIAS_TP
                    {
                        ID_ESTRATEGIA_TERCERO = ea.ID_ESTRATEGIA_TERCERO,
                        ID_ESTRATEGIA = ea.ID_ESTRATEGIA,
                        S_OTRO = ea.S_OTRO,
                        S_PUBLICO_OBJETIVO = ea.S_PUBLICO_OBJETIVO,
                        S_TIPOEVIDENCIA = ea.S_TIPOEVIDENCIA
                    };

                    dbSIM.Entry(nuevaEstrategia).State = EntityState.Added;

                    dbSIM.SaveChanges();

                    if (ea.S_ACTIVIDAD1 != null && ea.S_ACTIVIDAD1.Trim() != "" && ea.ID_PERIODICIDAD1 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = nuevaEstrategia.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD1;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD1;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD2 != null && ea.S_ACTIVIDAD2.Trim() != "" && ea.ID_PERIODICIDAD2 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = nuevaEstrategia.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD2;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD2;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD3 != null && ea.S_ACTIVIDAD3.Trim() != "" && ea.ID_PERIODICIDAD3 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = nuevaEstrategia.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD3;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD3;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD4 != null && ea.S_ACTIVIDAD4.Trim() != "" && ea.ID_PERIODICIDAD4 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = nuevaEstrategia.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD4;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD4;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD5 != null && ea.S_ACTIVIDAD5.Trim() != "" && ea.ID_PERIODICIDAD5 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = nuevaEstrategia.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD5;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD5;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                }
                else
                {
                    var nuevaEstrategia = new PMES_ESTRATEGIAS_TF
                    {
                        ID_ESTRATEGIA_TERCERO = ea.ID_ESTRATEGIA_TERCERO,
                        ID_ESTRATEGIA = ea.ID_ESTRATEGIA,
                        S_OTRO = ea.S_OTRO,
                        S_OBJETIVO = ea.S_OBJETIVO,
                        S_PUBLICO_OBJETIVO = ea.S_PUBLICO_OBJETIVO,
                        S_COMUNICACION_INTERNA = ea.S_COMUNICACION_INTERNA,
                        S_COMUNICACION_EXTERNA = ea.S_COMUNICACION_EXTERNA
                    };

                    dbSIM.Entry(nuevaEstrategia).State = EntityState.Added;

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Estrategias PMES [PostInsertarEstrategia] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }

        [HttpPost]
        [ActionName("ActualizarEstrategia")]
        public void PostActualizarEstrategia(ESTRATEGIA_ALMACENAR ea)
        {
            try
            {
                if (ea.TIPO == "P")
                {
                    var estrategiaTP = (from etp in dbSIM.PMES_ESTRATEGIAS_TP
                                        where etp.ID == ea.ID
                                        select etp).FirstOrDefault();

                    estrategiaTP.ID_ESTRATEGIA_TERCERO = ea.ID_ESTRATEGIA_TERCERO;
                    estrategiaTP.ID_ESTRATEGIA = ea.ID_ESTRATEGIA;
                    estrategiaTP.S_OTRO = ea.S_OTRO;
                    estrategiaTP.S_PUBLICO_OBJETIVO = ea.S_PUBLICO_OBJETIVO;
                    estrategiaTP.S_TIPOEVIDENCIA = ea.S_TIPOEVIDENCIA;

                    dbSIM.Entry(estrategiaTP).State = EntityState.Modified;

                    dbSIM.SaveChanges();

                    if (ea.S_ACTIVIDAD1 != null && ea.S_ACTIVIDAD1.Trim() != "" && ea.ID_PERIODICIDAD1 != null && ea.ID_ESTRATEGIA_ACTIVIDAD1 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD1
                                                                  select epa).FirstOrDefault();

                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD1;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD1;

                        dbSIM.Entry(actividad).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else if (ea.S_ACTIVIDAD1 != null && ea.S_ACTIVIDAD1.Trim() != "" && ea.ID_PERIODICIDAD1 != null && ea.ID_ESTRATEGIA_ACTIVIDAD1 == null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = estrategiaTP.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD1;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD1;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else if ((ea.S_ACTIVIDAD1 == null || ea.S_ACTIVIDAD1.Trim() == "" || ea.ID_PERIODICIDAD1 == null) && ea.ID_ESTRATEGIA_ACTIVIDAD1 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD1
                                                                  select epa).FirstOrDefault();

                        dbSIM.Entry(actividad).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD2 != null && ea.S_ACTIVIDAD2.Trim() != "" && ea.ID_PERIODICIDAD2 != null && ea.ID_ESTRATEGIA_ACTIVIDAD2 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD2
                                                                  select epa).FirstOrDefault();

                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD2;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD2;

                        dbSIM.Entry(actividad).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else if (ea.S_ACTIVIDAD2 != null && ea.S_ACTIVIDAD2.Trim() != "" && ea.ID_PERIODICIDAD2 != null && ea.ID_ESTRATEGIA_ACTIVIDAD2 == null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = estrategiaTP.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD2;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD2;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else if ((ea.S_ACTIVIDAD2 == null || ea.S_ACTIVIDAD2.Trim() == "" || ea.ID_PERIODICIDAD2 == null) && ea.ID_ESTRATEGIA_ACTIVIDAD2 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD2
                                                                  select epa).FirstOrDefault();

                        dbSIM.Entry(actividad).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD3 != null && ea.S_ACTIVIDAD3.Trim() != "" && ea.ID_PERIODICIDAD3 != null && ea.ID_ESTRATEGIA_ACTIVIDAD3 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD3
                                                                  select epa).FirstOrDefault();

                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD3;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD3;

                        dbSIM.Entry(actividad).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else if (ea.S_ACTIVIDAD3 != null && ea.S_ACTIVIDAD3.Trim() != "" && ea.ID_PERIODICIDAD3 != null && ea.ID_ESTRATEGIA_ACTIVIDAD3 == null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = estrategiaTP.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD3;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD3;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else if ((ea.S_ACTIVIDAD3 == null || ea.S_ACTIVIDAD3.Trim() == "" || ea.ID_PERIODICIDAD3 == null) && ea.ID_ESTRATEGIA_ACTIVIDAD3 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD3
                                                                  select epa).FirstOrDefault();

                        dbSIM.Entry(actividad).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD4 != null && ea.S_ACTIVIDAD4.Trim() != "" && ea.ID_PERIODICIDAD4 != null && ea.ID_ESTRATEGIA_ACTIVIDAD4 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD4
                                                                  select epa).FirstOrDefault();

                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD4;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD4;

                        dbSIM.Entry(actividad).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else if (ea.S_ACTIVIDAD4 != null && ea.S_ACTIVIDAD4.Trim() != "" && ea.ID_PERIODICIDAD4 != null && ea.ID_ESTRATEGIA_ACTIVIDAD4 == null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = estrategiaTP.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD4;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD4;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else if ((ea.S_ACTIVIDAD4 == null || ea.S_ACTIVIDAD4.Trim() == "" || ea.ID_PERIODICIDAD4 == null) && ea.ID_ESTRATEGIA_ACTIVIDAD4 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD4
                                                                  select epa).FirstOrDefault();

                        dbSIM.Entry(actividad).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }

                    if (ea.S_ACTIVIDAD5 != null && ea.S_ACTIVIDAD5.Trim() != "" && ea.ID_PERIODICIDAD5 != null && ea.ID_ESTRATEGIA_ACTIVIDAD5 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD5
                                                                  select epa).FirstOrDefault();

                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD5;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD5;

                        dbSIM.Entry(actividad).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else if (ea.S_ACTIVIDAD5 != null && ea.S_ACTIVIDAD5.Trim() != "" && ea.ID_PERIODICIDAD5 != null && ea.ID_ESTRATEGIA_ACTIVIDAD5 == null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = new PMES_ESTRATEGIAS_ACTIVIDADES();

                        actividad.ID_ESTRATEGIA_TP = estrategiaTP.ID;
                        actividad.S_ACTIVIDAD = ea.S_ACTIVIDAD5;
                        actividad.ID_PERIODICIDAD = (int)ea.ID_PERIODICIDAD5;

                        dbSIM.Entry(actividad).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    else if ((ea.S_ACTIVIDAD5 == null || ea.S_ACTIVIDAD5.Trim() == "" || ea.ID_PERIODICIDAD5 == null) && ea.ID_ESTRATEGIA_ACTIVIDAD5 != null)
                    {
                        PMES_ESTRATEGIAS_ACTIVIDADES actividad = (from epa in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                                                  where epa.ID == ea.ID_ESTRATEGIA_ACTIVIDAD5
                                                                  select epa).FirstOrDefault();

                        dbSIM.Entry(actividad).State = EntityState.Deleted;
                        dbSIM.SaveChanges();
                    }
                }
                else
                {
                    var estrategiaTF = (from etf in dbSIM.PMES_ESTRATEGIAS_TF
                                        where etf.ID == ea.ID
                                        select etf).FirstOrDefault();

                    estrategiaTF.ID_ESTRATEGIA_TERCERO = ea.ID_ESTRATEGIA_TERCERO;
                    estrategiaTF.ID_ESTRATEGIA = ea.ID_ESTRATEGIA;
                    estrategiaTF.S_OTRO = ea.S_OTRO;
                    estrategiaTF.S_OBJETIVO = ea.S_OBJETIVO;
                    estrategiaTF.S_PUBLICO_OBJETIVO = ea.S_PUBLICO_OBJETIVO;
                    estrategiaTF.S_COMUNICACION_INTERNA = ea.S_COMUNICACION_INTERNA;
                    estrategiaTF.S_COMUNICACION_EXTERNA = ea.S_COMUNICACION_EXTERNA;

                    dbSIM.Entry(estrategiaTF).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Estrategias PMES [PostActualizarEstrategia] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }

        [HttpGet]
        [ActionName("EliminarEstrategia")]
        public void GetEliminarEstrategia(string t, int id)
        {
            try
            {
                if (t == "P")
                {
                    var actividades = (from a in dbSIM.PMES_ESTRATEGIAS_ACTIVIDADES
                                       where a.ID_ESTRATEGIA_TP == id
                                       select a).ToList();

                    foreach (var actividad in actividades)
                    {
                        dbSIM.Entry(actividad).State = EntityState.Deleted;

                        dbSIM.SaveChanges();
                    }

                    var estrategia = (from e in dbSIM.PMES_ESTRATEGIAS_TP
                                     where e.ID == id
                                     select e).FirstOrDefault();

                    dbSIM.Entry(estrategia).State = EntityState.Deleted;

                    dbSIM.SaveChanges();
                }
                else
                {
                    var estrategia = (from e in dbSIM.PMES_ESTRATEGIAS_TF
                                      where e.ID == id
                                      select e).FirstOrDefault();

                    dbSIM.Entry(estrategia).State = EntityState.Deleted;

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Estrategias PMES [PostInsertarEstrategia] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }

        [HttpGet]
        [ActionName("MarcarEnviado")]
        public void GetMarcarEnviado(int et)
        {
            try
            {
                var encuesta = (from pet in dbSIM.PMES_ESTRATEGIAS_TERCERO
                         join ge in dbSIM.FRM_GENERICO_ESTADO on pet.ID_ESTADO equals ge.ID_ESTADO
                         where pet.ID == et
                         select ge).FirstOrDefault();

                if (encuesta != null)
                {
                    encuesta.TIPO_GUARDADO = 1;
                    dbSIM.Entry(encuesta).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception error)
            {
                SIM.Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Estrategias PMES [GetMarcarEnviado(" + et.ToString() + ")] : " + SIM.Utilidades.LogErrores.ObtenerError(error));
            }
        }
    }
}