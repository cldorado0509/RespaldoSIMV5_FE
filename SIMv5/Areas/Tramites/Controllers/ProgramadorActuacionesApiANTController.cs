using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.GestionDocumental.Models;
using SIM.Areas.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Security.Claims;
using SIM.Areas.Tramites.Models;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProgramadorActuacionesApiController : ApiController
    {
        public class PROGRAMACION
        {
            public int basadaEn = 0;
            public int id;
            public int? queja;
            public int? cm;
            public int? asunto;
            public int tramite;
            public int documento;
            public int tiempo;
            public int tipoTiempo;
            public int tipoActuacion;
            public DateTime? fechaNotificacion;
            public string observaciones;
            public int frecuencia;
            public int tipoFrecuencia;
            public int tipoPeriodicidad;
            public int repeticiones;
            public int instalacion;
            public int tercero;
            public int idAsignacion;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        [HttpGet, ActionName("ConsultaQuejas")]
        public datosConsulta GetConsultaQuejas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var model = (from queja in dbSIM.TBQUEJA
                                         select new
                                         {
                                             ID_POPUP = queja.CODIGO_QUEJA,
                                             queja.CODIGO_AFECTACION,
                                             queja.CODIGO_RECURSO,
                                             queja.ANO,
                                             queja.QUEJA,
                                             queja.CODIGO_MUNICIPIO,
                                             NOMBRE_POPUP = queja.CODIGO_QUEJA + "-" + queja.ANO + "-" + queja.ASUNTO
                                         });
                            modelData = model;
                        }
                        break;
                    default: // lookup o reduced
                        {
                            var model = (from queja in dbSIM.TBQUEJA
                                         select new
                                         {
                                             ID_LOOKUP = queja.CODIGO_QUEJA,
                                             S_NOMBRE_LOOKUP = queja.CODIGO_QUEJA + "-" + queja.ANO + "-" + queja.ASUNTO
                                         });
                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [HttpGet, ActionName("ConsultaCM")]
        public datosConsulta GetConsultaCM(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from tercero in dbSIM.TERCERO
                             join terceroInstalacion in dbSIM.TERCERO_INSTALACION on tercero.ID_TERCERO equals terceroInstalacion.ID_TERCERO
                             join instalacion in dbSIM.INSTALACION on terceroInstalacion.ID_INSTALACION equals instalacion.ID_INSTALACION
                             join proyecto in dbSIM.TBPROYECTO on terceroInstalacion.CODIGO_PROYECTO equals proyecto.CODIGO_PROYECTO into instalacionCM
                             from proyecto in instalacionCM.DefaultIfEmpty()
                             select new
                             {
                                 tercero.ID_TERCERO,
                                 instalacion.ID_INSTALACION, 
                                 N_DOCUMENTO = tercero.N_DOCUMENTON.ToString(),
                                 S_RSOCIAL = tercero.S_RSOCIAL.Trim(),
                                 INSTALACION = instalacion.S_NOMBRE.Trim(),
                                 CM = proyecto == null ? "(Sin CM)" : proyecto.CM,
                                 //ID_POPUP = instalacion.ID_INSTALACION.ToString() + "," + (proyecto == null ? "" : proyecto.CODIGO_PROYECTO.ToString()),
                                 ID_POPUP = proyecto == null ? -1 : proyecto.CODIGO_PROYECTO,
                                 NOMBRE_POPUP = (proyecto == null ? "(Sin CM)" : "CM: " + proyecto.CM) + " - " + tercero.S_RSOCIAL.Trim() + " - " + instalacion.S_NOMBRE.Trim()
                             });

                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ConsultaAsuntos")]
        public datosConsulta GetConsultaAsuntos(int cm, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from proyecto in dbSIM.TBPROYECTO
                             join solicitud in dbSIM.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                             join tipoSolicitud in dbSIM.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                             join municipio in dbSIM.TBMUNICIPIO on solicitud.CODIGO_MUNICIPIO equals municipio.CODIGO_MUNICIPIO
                             where proyecto.CODIGO_PROYECTO == cm && solicitud.FECHA_FINAL == null
                             select new
                             {
                                 ID_POPUP = solicitud.CODIGO_SOLICITUD,
                                 NOMBRE_POPUP = solicitud.CONEXO + " - " + solicitud.NUMERO + " - " + tipoSolicitud.NOMBRE + " (" + municipio.NOMBRE + ")",
                                 tipoSolicitud.CODIGO_TIPO_SOLICITUD,
                                 TIPOSOLICITUD = tipoSolicitud.NOMBRE,
                                 solicitud.NUMERO,
                                 TRAMO = solicitud.NOMBRE,
                                 solicitud.CONEXO,
                                 MUNICIPIO = municipio.NOMBRE
                             });
                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        // tipo: 1 CM, 2 Queja
        [HttpGet, ActionName("ConsultaTramites")]
        public datosConsulta GetConsultaTramites(int tipo, int idTipo, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            // TRAMITE_EXPEDIENTE_AMBIENTAL (EN ESTE CASO EL TRAMITE ESTÁ RELACIONADO CON PROYECTO(CM-INSTALACION) EN VEZ DE ESTAR RELACIONADO AL ASUNTO)
            // TRAMITE_EXPEDIENTE_QUEJA

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipo)
                {
                    case 1:
                        {
                            var model = (from tramiteAmbiental in dbSIM.TRAMITE_EXPEDIENTE_AMBIENTAL
                                         join tramite in dbSIM.TBTRAMITE on tramiteAmbiental.CODTRAMITE equals tramite.CODTRAMITE
                                         join proceso in dbSIM.TBPROCESO on tramite.CODPROCESO equals proceso.CODPROCESO
                                         where tramiteAmbiental.CODIGO_PROYECTO == idTipo
                                         select new
                                         {
                                             ID_POPUP = tramiteAmbiental.CODTRAMITE,
                                             NOMBRE_POPUP = tramite.COMENTARIOS,
                                             TIPOTRAMITE = proceso.NOMBRE,
                                             tramite.FECHAINI,
                                             tramite.FECHAFIN
                                         }).Distinct();
                            modelData = model;
                        }
                        break;
                    case 2:
                        {
                            var model = (from tramiteQueja in dbSIM.TRAMITE_EXPEDIENTE_QUEJA
                                         join proceso in dbSIM.TBPROCESO on tramiteQueja.TBTRAMITE.CODPROCESO equals proceso.CODPROCESO
                                         where tramiteQueja.TBQUEJA.CODIGO_QUEJA == idTipo
                                         select new
                                         {
                                             ID_POPUP = tramiteQueja.TBTRAMITE.CODTRAMITE,
                                             NOMBRE_POPUP = tramiteQueja.TBTRAMITE.COMENTARIOS,
                                             TIPOTRAMITE = proceso.NOMBRE,
                                             tramiteQueja.TBTRAMITE.FECHAINI,
                                             tramiteQueja.TBTRAMITE.FECHAFIN
                                         }).Distinct();
                            modelData = model;
                        }
                        break;
                    default:
                        {
                            var model = (from tramite in dbSIM.TBTRAMITE
                                         join proceso in dbSIM.TBPROCESO on tramite.CODPROCESO equals proceso.CODPROCESO
                                         select new
                                         {
                                             ID_POPUP = tramite.CODTRAMITE,
                                             NOMBRE_POPUP = tramite.COMENTARIOS,
                                             TIPOTRAMITE = proceso.NOMBRE,
                                             tramite.FECHAINI,
                                             tramite.FECHAFIN
                                         }).Distinct();
                            modelData = model;
                        }
                        break;
                }
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ConsultaDocumentos")]
        public datosConsulta GetConsultaDocumentos(int tramite, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            var model = (from documento in dbSIM.TBTRAMITEDOCUMENTO
                         join tipoDocumental in dbSIM.TBSERIE on documento.CODSERIE equals tipoDocumental.CODSERIE
                         where documento.CODTRAMITE == tramite
                         select new
                         {
                             ID_POPUP = documento.CODDOCUMENTO,
                             NOMBRE_POPUP = tipoDocumental.NOMBRE
                         });
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ConsultaIndicesDocumentos")]
        public datosConsulta GetConsultaIndicesDocumentos(int tramite, int documento, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            // Select Ido.Valor,Ise.Indice,Ise.Orden,Ido.FechaRegistro,Ido.CodIndice,tra.FechaIni,s.nombre as serie 
            // From TbIndiceDocumento Ido," + Usuario + "TbIndiceSerie Ise," + Usuario + "TbTramite tra," + Usuario + "tbserie s 
            // Where tra.codtramite = Ido.codtramite and Ise.CodIndice = Ido.CodIndice And s.codserie = Ise.codserie and Ido.CodTramite=@CodTramite And Ido.CodDocumento=@CodDocumento Order By Ise.Orden
            var model = (from indiceDocumento in dbSIM.TBINDICEDOCUMENTO
                         join indiceSerie in dbSIM.TBINDICESERIE on indiceDocumento.CODINDICE equals indiceSerie.CODINDICE
                         where indiceDocumento.CODTRAMITE == tramite && indiceDocumento.CODDOCUMENTO == documento
                         select new
                         {
                             indiceSerie.ORDEN,
                             indiceSerie.INDICE,
                             indiceDocumento.VALOR
                         });
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
            return new datosConsulta();
        }

        [HttpPost, ActionName("ProgramacionActuacion")]
        public object PostProgramacionActuacion(PROGRAMACION item)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            try
            {
                PROGRAMACION_ACTUACION programacion;
                
                if (item.id > 0) // Actualiza la programación
                {
                    programacion = (from programacionActuacion in dbSIM.PROGRAMACION_ACTUACION
                                    where programacionActuacion.ID_PROGRAMACION == item.id
                                    select programacionActuacion).FirstOrDefault();
                }
                else // Nueva programación
                {
                    programacion = new PROGRAMACION_ACTUACION();
                }

                int userId = int.Parse(Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(System.Web.HttpContext.Current.User.Identity));

                if (item.id == 0)
                {
                    programacion.ID_USUARIO = userId;
                    programacion.ESTADO = 1;
                }
                programacion.TIPO = item.basadaEn;
                programacion.CODIGO_QUEJA = item.queja;
                programacion.CODIGO_PROYECTO = item.cm;
                programacion.CODIGO_SOLICITUD = item.asunto;
                programacion.CODIGO_TRAMITE = item.tramite;
                programacion.CODIGO_DOCUMENTO = item.documento;
                programacion.TIEMPO = item.tiempo;
                programacion.TIPO_TIEMPO = item.tipoTiempo;
                programacion.CODIGO_TIPO_ACTUACION = item.tipoActuacion;
                programacion.FECHA_NOTIFICACION = item.fechaNotificacion;
                programacion.OBSERVACIONES = item.observaciones;
                programacion.FRECUENCIA = item.frecuencia;
                programacion.TIPO_FRECUENCIA = item.tipoFrecuencia;
                programacion.PERIODICIDAD = item.tipoPeriodicidad;
                programacion.REPETICIONES = item.repeticiones;
                programacion.ID_INSTALACION = item.instalacion;
                programacion.ID_TERCERO = item.tercero;
                programacion.ID_ASIGNACION = item.idAsignacion;

                if (item.id == 0)
                    dbSIM.Entry(programacion).State = System.Data.Entity.EntityState.Added;
                else
                    dbSIM.Entry(programacion).State = System.Data.Entity.EntityState.Modified;

                dbSIM.SaveChanges();

                ObjectParameter rTA = new ObjectParameter("rtaI", typeof(string));
                dbSIM.SP_PROGRAMACION_PROXEJECUCION(programacion.ID_PROGRAMACION, rTA);
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Registrando Programacion" + Utilidades.Data.ObtenerError(e) };
            }
            return new { resp = "OK", mensaje = "Programación Registrada Satisfactoriamente", datos = item };
        }

        [HttpGet, ActionName("ConsultaProgramacion")]
        public dynamic GetConsultaProgramacion(int id)
        {
            string cmSel = null, asuntoSel = null, quejaSel = null, tramiteSel = null, documentoSel = null;

            PROGRAMACION_ACTUACION programacionSel = (from programacion in dbSIM.PROGRAMACION_ACTUACION
                                  where programacion.ID_PROGRAMACION == id
                                  select programacion).FirstOrDefault();
            
            if (programacionSel != null)
            {
                if (programacionSel.TIPO == 1) // CM
                {
                    cmSel = (from tercero in dbSIM.TERCERO
                            join terceroInstalacion in dbSIM.TERCERO_INSTALACION on tercero.ID_TERCERO equals terceroInstalacion.ID_TERCERO
                            join instalacion in dbSIM.INSTALACION on terceroInstalacion.ID_INSTALACION equals instalacion.ID_INSTALACION
                            join proyecto in dbSIM.TBPROYECTO on terceroInstalacion.CODIGO_PROYECTO equals proyecto.CODIGO_PROYECTO
                            where proyecto.CODIGO_PROYECTO == programacionSel.CODIGO_PROYECTO && tercero.ID_TERCERO == programacionSel.ID_TERCERO && instalacion.ID_INSTALACION == programacionSel.ID_INSTALACION
                            select proyecto.CM.Trim() + " - " + tercero.S_RSOCIAL.Trim() + " - " + instalacion.S_NOMBRE.Trim()
                        ).FirstOrDefault();

                    asuntoSel = (from proyecto in dbSIM.TBPROYECTO
                             join solicitud in dbSIM.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                             join tipoSolicitud in dbSIM.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                             join municipio in dbSIM.TBMUNICIPIO on solicitud.CODIGO_MUNICIPIO equals municipio.CODIGO_MUNICIPIO
                             where solicitud.CODIGO_SOLICITUD == programacionSel.CODIGO_SOLICITUD
                             select solicitud.CONEXO + " - " + solicitud.NUMERO + " - " + tipoSolicitud.NOMBRE + " (" + municipio.NOMBRE + ")"
                        ).FirstOrDefault();
                }
                else // Queja
                {
                    quejaSel = (from queja in dbSIM.TBQUEJA
                            where queja.CODIGO_QUEJA == programacionSel.CODIGO_QUEJA
                            select queja.CODIGO_QUEJA + "-" + queja.ANO + "-" + queja.ASUNTO
                        ).FirstOrDefault();
                }

                tramiteSel = (from tramite in dbSIM.TBTRAMITE
                                  where tramite.CODTRAMITE == programacionSel.CODIGO_TRAMITE
                                  select tramite.COMENTARIOS
                             ).FirstOrDefault();

                documentoSel = (from documento in dbSIM.TBTRAMITEDOCUMENTO
                                    join tipoDocumental in dbSIM.TBSERIE on documento.CODSERIE equals tipoDocumental.CODSERIE
                                    where documento.CODTRAMITE == programacionSel.CODIGO_TRAMITE && documento.CODDOCUMENTO == programacionSel.CODIGO_DOCUMENTO
                                    select tipoDocumental.NOMBRE
                             ).FirstOrDefault();

                return new
                {
                    programacion = programacionSel,
                    cm = cmSel,
                    asunto = asuntoSel,
                    queja = quejaSel,
                    tramite = tramiteSel,
                    documento = documentoSel
                };
            } else {
                return null;
            }
        }

        [HttpGet, ActionName("ConsultaProgramaciones")]
        public datosConsulta GetConsultaProgramaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;
            var idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            var idFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbSIM, idUsuario));

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (
                                from programacion in dbSIM.PROGRAMACION_ACTUACION
                                join asignacion in dbSIM.PROGRAMACION_ASIGNACION on new { programacion.TIPO, programacion.ID_ASIGNACION } equals new { asignacion.TIPO, asignacion.ID_ASIGNACION}
                                join proyecto in dbSIM.TBPROYECTO on programacion.CODIGO_PROYECTO equals proyecto.CODIGO_PROYECTO
                                //join terceroInstalacion in dbSIM.TERCERO_INSTALACION on programacion.CODIGO_PROYECTO equals terceroInstalacion.CODIGO_PROYECTO
                                join tercero in dbSIM.TERCERO on programacion.ID_TERCERO equals tercero.ID_TERCERO
                                join instalacion in dbSIM.INSTALACION on programacion.ID_INSTALACION equals instalacion.ID_INSTALACION
                                /*join solicitud in dbSIM.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                                join tipoSolicitud in dbSIM.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                                join municipio in dbSIM.TBMUNICIPIO on solicitud.CODIGO_MUNICIPIO equals municipio.CODIGO_MUNICIPIO
                                */
                                where (programacion.ID_USUARIO == idUsuario || asignacion.CODFUNCIONARIO == idFuncionario) && programacion.ESTADO == 1 && programacion.TIPO == 1
                                select new {
                                    programacion.ID_PROGRAMACION,
                                    TIPO =  "CM-" + proyecto.CM + "-" + tercero.S_RSOCIAL,
                                    TIPO_ACTUACION = programacion.CODIGO_TIPO_ACTUACION == 1 ? "JURIDICA" : "TECNICA",
                                    programacion.FECHA_NOTIFICACION,
                                    programacion.FECHA_PROXIMA_EJECUCION
                                }
                             ).Union(
                                 from programacion in dbSIM.PROGRAMACION_ACTUACION
                                 join asignacion in dbSIM.PROGRAMACION_ASIGNACION on new { programacion.TIPO, programacion.ID_ASIGNACION } equals new { asignacion.TIPO, asignacion.ID_ASIGNACION }
                                 join queja in dbSIM.TBQUEJA on programacion.CODIGO_QUEJA equals queja.CODIGO_QUEJA
                                 where (programacion.ID_USUARIO == idUsuario || asignacion.CODFUNCIONARIO == idFuncionario) && programacion.ESTADO == 1 && programacion.TIPO == 2
                                 select new
                                 {
                                     programacion.ID_PROGRAMACION,
                                     TIPO = "QUEJA-" + queja.CODIGO_QUEJA + "-" + queja.ANO + "-" + queja.ASUNTO,
                                     TIPO_ACTUACION = programacion.CODIGO_TIPO_ACTUACION == 1 ? "JURIDICA" : "TECNICA",
                                     programacion.FECHA_NOTIFICACION,
                                     programacion.FECHA_PROXIMA_EJECUCION
                                 }
                            ).Union(
                                 from programacion in dbSIM.PROGRAMACION_ACTUACION
                                 where programacion.ID_USUARIO == idUsuario && programacion.ESTADO == 1 && programacion.TIPO == 3
                                 select new
                                 {
                                     programacion.ID_PROGRAMACION,
                                     TIPO = "SOLICITUD-" + programacion.CODIGO_TRAMITE + "-" + programacion.CODIGO_DOCUMENTO,
                                     TIPO_ACTUACION = programacion.CODIGO_TIPO_ACTUACION == 1 ? "JURIDICA" : "TECNICA",
                                     programacion.FECHA_NOTIFICACION,
                                     programacion.FECHA_PROXIMA_EJECUCION
                                 }
                            );
                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }
    }

    /// <summary>
    /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
    /// </summary>
    public struct datosConsulta
    {
        public int numRegistros;
        public IEnumerable<dynamic> datos;
    }
}
