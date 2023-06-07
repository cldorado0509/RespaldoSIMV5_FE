using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.Tramites;
using SIM.Data;
using SIM.Data.Tramites;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.EMMA;
using System.Configuration;
using SIM.Utilidades;
using System.IO;
using System.Security.Claims;

namespace SIM.Areas.Tramites.Controllers
{
    [Authorize]
    public class TramitesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public class IndiceProceso
        {
            public decimal CODINDICE { get; set; }
            public string INDICE { get; set; }
            public decimal TIPO { get; set; }
            public decimal LONGITUD { get; set; }
            public decimal OBLIGA { get; set; }
            public string VALORDEFECTO { get; set; }
            public string VALOR { get; set; }
            public Nullable<decimal> ID_LISTA { get; set; }
            public Nullable<int> TIPO_LISTA { get; set; }
            public string CAMPO_NOMBRE { get; set; }
        }

        public class DatosTramite
        {
            public int CodProceso { get; set; }
            public int CodTareaPadre { get; set; }
            public int CodTarea { get; set; }
            public string Comentario { get; set; }
            public List<IndicesValores> Indices { get; set; }
            public Funcionario Responsable { get; set; }
            public List<Funcionario> Copias { get; set; }
        }

        public struct Tarea
        {
            public int CODTAREA { get; set; }
            public string NOMBRE { get; set; }
            public string TIPOTAREA { get; set; }
        }

        public struct Tramite
        {
            public int CODTRAMITE { get; set; }
            public string ASUNTO { get; set; }
            public int CODTAREA { get; set; }
            public int? CODTAREASIGUIENTE { get; set; }
        }

        public struct Funcionario
        {
            public int CODFUNCIONARIO { get; set; }
            public string NOMBRE { get; set; }

            public string DEPENDENCIA { get; set; }
        }

        public class ValorLista
        {
            public int ID { get; set; }
            public string NOMBRE { get; set; }
        }

        public struct DatosConsultaTramites
        {
            public int numRegistros;
            public List<Tramite> datos;
        }

        public struct DatosConsultaTareasFlujo
        {
            public int numRegistros;
            public List<Tarea> datos;
        }

        public struct DatosConsultaResponsablesTarea
        {
            public int numRegistros;
            public List<Funcionario> datos;
        }

        public struct DatosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        public struct TramiteTareaBusqueda
        {
            public int CODTRAMITE { get; set; }
            public int CODDOCUMENTO { get; set; }
        }

        [HttpGet]
        [ActionName("Tramites")]
        public DatosConsultaTramites GetTramites(string tramites)
        {
            var datos = dbSIM.Database.SqlQuery<Tramite>(
                    "SELECT tr.CODTRAMITE, " +
                    "tr.MENSAJE || ' - ' || (SELECT VALOR FROM TRAMITES.TBINDICETRAMITE it_i INNER JOIN TRAMITES.TBINDICEPROCESO ip_i ON it_i.CODINDICE = ip_i.CODINDICE WHERE it_i.CODTRAMITE = tr.CODTRAMITE AND ip_i.CODPROCESO = tr.CODPROCESO AND ip_i.IDENTIFICA_EXPEDIENTE = '1') AS ASUNTO, " +
                    "tt.CODTAREA, " +
                    "NULL AS CODTAREASIGUIENTE " +
                    "FROM  TRAMITES.TBTRAMITE tr INNER JOIN " +
                    "   TRAMITES.TBTRAMITETAREA tt ON tr.CODTRAMITE = tt.CODTRAMITE " +
                    "WHERE tr.CODTRAMITE IN (" + tramites + ") AND tt.ESTADO = 0 AND tt.COPIA = 0");

            var detalleTramites = datos.ToList<Tramite>();

            DatosConsultaTramites resultado = new DatosConsultaTramites();
            resultado.numRegistros = detalleTramites.Count();
            resultado.datos = detalleTramites;

            return resultado;
        }

        [HttpGet]
        [ActionName("TramiteAsunto")]
        public string GetTramiteAsunto(int tramite)
        {
            var datos  = dbSIM.TBTRAMITE.Where(t => t.CODTRAMITE == tramite).FirstOrDefault().MENSAJE;

            return datos;
        }

        [HttpPost]
        [ActionName("ActualizarIndiceTramite")]
        public string PostActualizarIndiceTramite(TBINDICETRAMITE valorIndice)
        {
            var indiceTramite = dbSIM.TBINDICETRAMITE.Where(it => it.CODTRAMITE == valorIndice.CODTRAMITE && it.CODINDICE == valorIndice.CODINDICE).FirstOrDefault();

            try
            {
                if (indiceTramite != null)
                {
                    indiceTramite.VALOR = valorIndice.VALOR;
                    indiceTramite.FECHAACTUALIZA = DateTime.Now;

                    dbSIM.Entry(indiceTramite).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    valorIndice.FECHAREGISTRO = DateTime.Now;
                    valorIndice.FECHAACTUALIZA = DateTime.Now;

                    dbSIM.Entry(valorIndice).State = System.Data.Entity.EntityState.Added;
                }

                dbSIM.SaveChanges();
            } catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Tramites [PostActualizarIndiceTramite - " + valorIndice.CODTRAMITE.ToString() + " - " + valorIndice.CODINDICE + " - '" + valorIndice.VALOR + "']: Se presentó un error Almacenando el Indice del Trámite.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR:Se presentó un error Almacenando el Indice del Trámite.";
            }

            return "OK:Indice Almacenado Satisfactoriamente";
        }

        [HttpGet]
        [ActionName("TareasFlujoSiguiente")]
        public DatosConsultaTareasFlujo GetTareasFlujoSiguiente(int codTarea, int? tipo)
        {
            List<Tarea> tareasFlujo;

            if (tipo == null || tipo == 0)
            {
                var datos = dbSIM.Database.SqlQuery<Tarea>(
                        "SELECT DISTINCT dr.CODTAREASIGUIENTE AS CODTAREA, t.NOMBRE, CASE TO_CHAR(T.FIN) || TO_CHAR(T.INICIO) WHEN '10' THEN 'Final' WHEN '01' THEN 'Inicio' WHEN '00' THEN 'Flujo' END AS TIPOTAREA " +
                        "FROM TRAMITES.TBDETALLEREGLA dr INNER JOIN " +
                        "   TRAMITES.TBTAREA t ON dr.CODTAREASIGUIENTE = t.CODTAREA " +
                        "WHERE dr.CODTAREA = " + codTarea.ToString());

                tareasFlujo = datos.ToList<Tarea>();
            }
            else
            {
                var datos = dbSIM.Database.SqlQuery<Tarea>(
                        "SELECT DISTINCT dr.CODTAREASIGUIENTE AS CODTAREA, t.NOMBRE, CASE TO_CHAR(T.FIN) || TO_CHAR(T.INICIO) WHEN '10' THEN 'Final' WHEN '01' THEN 'Inicio' WHEN '00' THEN 'Flujo' END AS TIPOTAREA " +
                        "FROM TRAMITES.DETALLE_REGLA dr INNER JOIN " +
                        "   TRAMITES.TBTAREA t ON dr.CODTAREASIGUIENTE = t.CODTAREA " +
                        "WHERE dr.CODTAREA = " + codTarea.ToString() + " AND NVL(S_VISIBLE, 'S') = 'S'");

                tareasFlujo = datos.ToList<Tarea>();
            }

            DatosConsultaTareasFlujo resultado = new DatosConsultaTareasFlujo();
            resultado.numRegistros = tareasFlujo.Count();
            resultado.datos = tareasFlujo;

            return resultado;
        }

        [HttpGet]
        [ActionName("ResponsablesTarea")]
        public DatosConsultaResponsablesTarea GetResponsablesTarea(string filter, int skip, int take, int codTareaActual, int codTarea)
        {
            //List<Funcionario> funcionariosTarea;
            IEnumerable<Funcionario> funcionariosTarea;

            string sql = "SELECT TR.CODFUNCIONARIO, FUN.NOMBRES AS NOMBRE, FUN.DEPENDENCIA AS DEPENDENCIA " +
                    "FROM TRAMITES.TBTAREARESPONSABLE TR INNER JOIN " +
                    "TRAMITES.QRY_FUNCIONARIO_ALL FUN ON TR.CODFUNCIONARIO = FUN.CODFUNCIONARIO " +
                    "WHERE TR.CODTAREA = " + codTarea.ToString() + " AND FUN.ACTIVO=1"; // + " AND tr.CODTAREAPADRE = " + codTareaActual.ToString();

            if (filter != null && filter != "")
            {
                string[] criterios = filter.Split(',');
                switch (criterios[0])
                {
                    case "NOMBRE":
                        switch (criterios[1])
                        {
                            case "contains":
                                sql += " AND (UPPER(FUN.NOMBRES) LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            case "notcontains":
                                sql += " AND (UPPER(FUN.NOMBRES) NOT LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            case "startswith":
                                sql += " AND (UPPER(FUN.NOMBRES) LIKE UPPER('" + filter.Split(',')[2] + "%'))";
                                break;
                            case "endswith":
                                sql += " AND (UPPER(FUN.NOMBRES) LIKE UPPER('%" + filter.Split(',')[2] + "'))";
                                break;
                            case "=":
                                sql += " AND (UPPER(FUN.NOMBRES) = UPPER('" + filter.Split(',')[2] + "'))";
                                break;
                            case "<>":
                                sql += " AND (UPPER(FUN.NOMBRES) <> UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            default:
                                sql += " AND (UPPER(FUN.NOMBRES) LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                        }
                        break;
                    case "DEPENDENCIA":
                        switch (criterios[1])
                        {
                            case "contains":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            case "notcontains":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) NOT LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            case "startswith":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) LIKE UPPER('" + filter.Split(',')[2] + "%'))";
                                break;
                            case "endswith":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) LIKE UPPER('%" + filter.Split(',')[2] + "'))";
                                break;
                            case "=":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) = UPPER('" + filter.Split(',')[2] + "'))";
                                break;
                            case "<>":
                                sql += " AND (UPPER(FUN.DEPENDENCIA) <> UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                            default:
                                sql += " AND (UPPER(FUN.DEPENDENCIA) LIKE UPPER('%" + filter.Split(',')[2] + "%'))";
                                break;
                        }
                        break;
                }
            }

            sql += " ORDER BY FUN.NOMBRES";

            var datos = dbSIM.Database.SqlQuery<Funcionario>(sql);

            funcionariosTarea = datos.ToList<Funcionario>().Skip<Funcionario>(skip).Take<Funcionario>(take);

            DatosConsultaResponsablesTarea resultado = new DatosConsultaResponsablesTarea();
            resultado.numRegistros = datos.ToList<Funcionario>().Count();
            resultado.datos = funcionariosTarea.ToList<Funcionario>();

            return resultado;
        }

        /// <summary>
        /// Activa un usuario que realizó el registro.
        /// </summary>
        /// <param name="idRolSolicitado">Id de la solicitud del registro</param>
        // <returns></returns>
        [HttpPost, ActionName("AvanzaTareaTramite")]
        public DatosRespuesta PostAvanzaTareaTramite(DatosAvanzaTareaTramite datosAvanzaTareaTramite)
        {
            DatosRespuesta respuesta = new DatosRespuesta();
            var tramite = new TramitesLibrary();

            var respuestaAvanza = tramite.AvanzaTareaTramite(datosAvanzaTareaTramite);

            respuesta.tipoRespuesta = respuestaAvanza.tipoRespuesta;
            respuesta.detalleRespuesta = respuestaAvanza.detalleRespuesta;

            return respuesta;

            /*bool resultado = true;

            foreach (string codTramite in datosAvanzaTareaTramite.codTramites.Split(','))
            {
                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                dbSIM.SP_AVANZA_TAREA(datosAvanzaTareaTramite.tipo, Convert.ToInt32(codTramite), datosAvanzaTareaTramite.codTarea, datosAvanzaTareaTramite.codTareaSiguiente, datosAvanzaTareaTramite.codFuncionario, datosAvanzaTareaTramite.copias, datosAvanzaTareaTramite.comentario, rtaResultado);

                if (rtaResultado.Value.ToString() != "OK")
                {
                    resultado = false;
                }
            }

            if (resultado)
                return new DatosRespuesta() { tipoRespuesta = "OK", detalleRespuesta = "Trámites Avanzados Satisfactoriamente." };
            else
                return new DatosRespuesta() { tipoRespuesta = "ERROR", detalleRespuesta = "Por lo menos un Trámite no fue avanzado Satisfactoriamente" };
                */
        }

        [HttpGet, ActionName("GenerarIndicesFullTextDocumento")]
        public void GetGenerarIndicesFullTextDocumento(int ct, int cd)
        {
            TramitesLibrary utilidad = new TramitesLibrary();

            utilidad.GenerarIndicesFullTextDocumento(ct, cd);
        }

        [HttpGet, ActionName("GenerarIndicesFullTextPendientes")]
        public void GetGenerarIndicesFullTextPendientes()
        {
            bool procesarDocumentos = true;

            while (procesarDocumentos)
            {
                var datos = dbSIM.Database.SqlQuery<TramiteTareaBusqueda>(
                        "SELECT td.CODTRAMITE, td.CODDOCUMENTO " +
                        "FROM TRAMITES.TBTRAMITEDOCUMENTO td LEFT OUTER JOIN " +
                        "    TRAMITES.BUSQUEDA_DOCUMENTO bd ON td.CODTRAMITE = bd.COD_TRAMITE AND td.CODDOCUMENTO = bd.COD_DOCUMENTO " +
                        "WHERE bd.ID_BUSQUEDADOC IS NULL AND td.FECHACREACION IS NOT NULL AND ROWNUM <= 100").ToList();

                if (datos.Count > 0)
                {
                    TramitesLibrary utilidad = new TramitesLibrary();

                    foreach (var documento in datos)
                    {
                        utilidad.GenerarIndicesFullTextDocumento(documento.CODTRAMITE, documento.CODDOCUMENTO);
                    }

                    System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    procesarDocumentos = false;
                }
            }
        }

        [HttpGet, ActionName("ObtieneTramite")]
        public JArray GetObtieneTramite(decimal CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            //if (CodTramite < 0) return null;
            try
            {
              var model = (from TRA in dbSIM.TBTRAMITE 
                         join PRO in dbSIM.TBPROCESO on TRA.CODPROCESO equals PRO.CODPROCESO
                         join INE in dbSIM.QRY_INDICE_EXPEDIENTETRAMITE on TRA.CODTRAMITE equals INE.CODTRAMITE
                         where TRA.CODTRAMITE == CodTramite 
                         select new
                         {
                             CODTRAMITE = TRA.CODTRAMITE.ToString(),
                             VITAL = TRA.NUMERO_VITAL,
                             PROCESO = PRO.NOMBRE,
                             TAREA = (from TTE in dbSIM.TBTRAMITETAREA join TAR in dbSIM.TBTAREA on TTE.CODTAREA equals TAR.CODTAREA
                                      where TTE.CODTRAMITE == TRA.CODTRAMITE && TTE.ESTADO == 0 && TTE.COPIA == 0
                                      select TAR.NOMBRE).FirstOrDefault(),
                             INICIOTRAMITE = TRA.FECHAINI,
                             INE.EXPEDIENTE,
                             INICIOTAREA = (from TTE in dbSIM.TBTRAMITETAREA
                                         where TTE.CODTRAMITE == TRA.CODTRAMITE && TTE.ESTADO == 0 && TTE.COPIA == 0
                                         select TTE.FECHAINI).FirstOrDefault(),
                             MARCAR = PRO.S_MARCATAREA,
                             COLOR = PRO.S_COLORMARCA,
                             ASUNTO = TRA.MENSAJE,
                             TIPO = (from TTE in dbSIM.TBTRAMITETAREA
                                     where TTE.CODTRAMITE == TRA.CODTRAMITE && TTE.ESTADO == 0 && TTE.COPIA == 0
                                     select TTE.DEVOLUCION == "1" ? "Devolución" : "Normal").FirstOrDefault(),
                             FINTAREA = (from TTE in dbSIM.TBTRAMITETAREA
                                         where TTE.CODTRAMITE == TRA.CODTRAMITE && TTE.ESTADO == 0 && TTE.COPIA == 0
                                         select TTE.FECHAFIN).FirstOrDefault()
                         }).ToList(); 
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerProcesos")]
        public dynamic GetObtenerProcesos()
        {
            var procesos = from p in dbSIM.TBPROCESO
                           where p.ACTIVO == "1"
                           orderby p.NOMBRE
                           select new
                           {
                               p.CODPROCESO,
                               p.NOMBRE
                           };

            return procesos.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerTareasProceso")]
        public dynamic GetObtenerTareasProceso(int codProceso)
        {
            var tareas = from t in dbSIM.TBTAREA
                           where t.CODPROCESO == codProceso
                           orderby t.INICIO descending, t.FIN, t.NOMBRE
                           select new
                           {
                               t.CODTAREA,
                               t.NOMBRE
                           };

            return tareas.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesProceso")]
        public dynamic GetObtenerIndicesProceso(int codProceso)
        {
            var indicesProceso = from ip in dbSIM.TBINDICEPROCESO
                                 join lista in dbSIM.TBSUBSERIE on (decimal)ip.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                 from pdis in l.DefaultIfEmpty()
                                 where ip.CODPROCESO == codProceso && ip.MOSTRAR == "1"
                                 orderby ip.ORDEN
                                 select new IndiceProceso
                                 {
                                     CODINDICE = ip.CODINDICE,
                                     INDICE = ip.INDICE,
                                     TIPO = (decimal)ip.TIPO,
                                     LONGITUD = (decimal)ip.LONGITUD,
                                     OBLIGA = (decimal)ip.OBLIGA,
                                     VALORDEFECTO = ip.VALORDEFECTO,
                                     VALOR = "",
                                     ID_LISTA = ip.CODIGO_SUBSERIE,
                                     TIPO_LISTA = pdis.TIPO,
                                     CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                                 };

            return indicesProceso.ToList();
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndiceValoresLista")]
        public dynamic GetObtenerIndiceValoresLista(int id)
        {
            List<ValorLista> resultadoConsulta;
            string sql;
            TBSUBSERIE lista = dbSIM.TBSUBSERIE.Where(ss => ss.CODIGO_SUBSERIE == id).FirstOrDefault();

            if (lista != null)
            {
                if (lista.TIPO == 0) // La lista se toma de la tabla TBDETALLE_SUBSERIE
                {
                    sql = "SELECT CODIGO_DETALLE AS ID, NOMBRE FROM TRAMITES.TBDETALLE_SUBSERIE WHERE CODIGO_SUBSERIE = " + lista.CODIGO_SUBSERIE.ToString() + " ORDER BY NOMBRE";

                    resultadoConsulta = dbSIM.Database.SqlQuery<ValorLista>(sql).ToList<ValorLista>();
                }
                else
                {
                    resultadoConsulta = dbSIM.Database.SqlQuery<ValorLista>("SELECT " + lista.CAMPO_ID + " AS ID, " + lista.CAMPO_NOMBRE + " AS NOMBRE FROM (" + lista.SQL + ") datos").ToList<ValorLista>();
                }

                return resultadoConsulta;
            }
            else
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost, ActionName("CrearTramite")]
        public string PostCrearTramite(DatosTramite datos)
        {
            int idUsuario = 0;
            int[] nuevoTramite;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            try
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                if (idUsuario == 0)
                {
                    return "ERROR:El Usuario no se encuentra autenticado.";
                }

                nuevoTramite = Utilidades.Tramites.CrearTramite(datos.CodProceso, datos.CodTarea, datos.CodTareaPadre, datos.Comentario, datos.Comentario, datos.Responsable.CODFUNCIONARIO, datos.Responsable.CODFUNCIONARIO, datos.Copias.Select(f => f.CODFUNCIONARIO).ToArray(), datos.Indices);
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Crear Trámite - TramitesApiController [PostCrearTramite] : Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return "ERROR: Se pudo haber almacenado parcialmente la Información.";
            }

            return "OK:" + nuevoTramite[0].ToString();
        }

        // GET api/<controller>
        [Authorize]
        [HttpGet, ActionName("Funcionarios")]
        public datosConsulta GetFuncionarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                             where f.ACTIVO == "1"
                             select new
                             {
                                 f.CODFUNCIONARIO,
                                 NOMBRE = f.NOMBRES
                             });

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
    }
}

