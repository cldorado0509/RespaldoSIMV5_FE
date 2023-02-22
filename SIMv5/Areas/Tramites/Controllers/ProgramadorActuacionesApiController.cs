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
using SIM.Areas.Tramites.Models;
using SIM.Data.Tramites;
using SIM.Data;

namespace SIM.Areas.Tramites.Controllers
{
    public class ProgramadorActuacionesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        public struct datosProgramacion
        {
            public PROGRAMACION_ACTUACION programacion;
            string CM;
            string TRAMO;
            public IEnumerable<dynamic> tramites;
            public IEnumerable<dynamic> nuevosAsuntos;
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet, ActionName("ConsultaProgramaciones")]
        public datosConsulta GetConsultaProgramaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                var model = (from p in dbSIM.PROGRAMACION_ACTUACION
                             join py in dbSIM.TBPROYECTO on p.CODIGO_PROYECTO equals py.CODIGO_PROYECTO into cm
                             from py in cm.DefaultIfEmpty()
                             join zp in dbSIM.ZONA_PROGRAMACION on p.ID_ZONA equals zp.ID_ZONA into zona
                             from z in zona.DefaultIfEmpty()
                             select new
                             {
                                 p.ID_PROGRAMACION,
                                 TIPO_PROGRAMACION = p.TIPO,
                                 TIPO = (p.TIPO == 1 ? "CM" : "ZONA"),
                                 CMZONA = (p.TIPO == 1 ? py.CM : z.NOMBRE),
                                 TRAMITES = p.S_TRAMITES,
                                 FECHA_PROGRAMACION = p.FECHA_PROGRAMACION,
                                 p.ANO,
                                 p.MES,
                                 FECHA_EJECUCION = p.MES + "/" + p.ANO
                             });

                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet, ActionName("ConsultaProgramacion")]
        public datosProgramacion GetConsultaProgramacion(int id)
        {
            var programacion = (from p in dbSIM.PROGRAMACION_ACTUACION
                            where p.ID_PROGRAMACION == id
                            select p).FirstOrDefault();

            var tramites = (from t in dbSIM.PROGRAMACION_TRAMITES
                            where t.ID_PROGRAMACION == id
                           select t).ToList();

            var nuevosAsuntos = (from na in dbSIM.PROGRAMACION_NUEVOS_ASUNTOS
                                 where na.ID_PROGRAMACION == id
                                 select na).ToList();

            var resultado = new datosProgramacion();

            resultado.programacion = programacion;
            resultado.tramites = tramites;
            resultado.nuevosAsuntos = nuevosAsuntos;

            return resultado;
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
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
                                 proyecto.DIRECCION,
                                 X = (decimal?)proyecto.X_CORD,
                                 Y = (decimal?)proyecto.Y_CORD,
                                 CM = proyecto == null ? "(Sin CM)" : proyecto.CM,
                                 S_PROYECTO = proyecto == null ? "(Sin Proyecto)" : proyecto.NOMBRE,
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

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet, ActionName("ConsultaTramo")]
        public datosConsulta GetConsultaTramo(int? cm, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                if (cm == null)
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
                                     NOMBRE_POPUP = solicitud.CONEXO + " - " + solicitud.NUMERO + " - " + tipoSolicitud.NOMBRE + " - " + solicitud.NOMBRE + " (" + municipio.NOMBRE + ")",
                                     tipoSolicitud.CODIGO_TIPO_SOLICITUD,
                                     TIPOSOLICITUD = tipoSolicitud.NOMBRE,
                                     solicitud.NUMERO,
                                     TRAMO = solicitud.NOMBRE,
                                     solicitud.CONEXO,
                                     MUNICIPIO = municipio.NOMBRE
                                 });
                    modelData = model;
                }
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet]
        [ActionName("TareasReparto")]
        public datosConsulta GetTareasReparto(int? idProgramacion, string cm, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XPROGRAMADORACTUACIONES");
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
                if (administrador)
                {
                    var model = (from pp in dbSIM.VW_PROGRAMACIONES_PENDIENTES
                                 where (cm == null || pp.CM == cm)
                                 orderby pp.CODTRAMITE
                                 select pp);

                    modelData = model;
                }
                else
                {
                    var model = (from pp in dbSIM.VW_PROGRAMACIONES_PENDIENTES
                                 where (cm == null || pp.CM == cm)
                                 where pp.ID_USUARIO == idUsuario
                                 orderby pp.CODTRAMITE
                                 select pp);

                    modelData = model;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                if (take == 0)
                    resultado.datos = modelFiltered.ToList();
                else
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet]
        [ActionName("ConsultaZonas")]
        public datosConsulta GetConsultaZonas()
        {
            //var seleccion = dbSIM.Database.SqlQuery<ZONA>("SELECT ID_ZONA, NOMBRE FROM GENERAL.ZONA ORDER BY CODIGO");
            var seleccion = from z in dbSIM.ZONA_PROGRAMACION
                            select z;

            return new datosConsulta() { numRegistros = seleccion.Count(), datos = seleccion.ToList<ZONA_PROGRAMACION>() };
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpGet]
        [ActionName("ConsultaAsuntosTipo")]
        public datosConsulta GetConsultaAsuntosTipo(int? idProgramacion)
        {
            var asuntos = dbSIM.Database.SqlQuery<ASUNTOTIPO>("SELECT ID, S_NOMBRE AS NOMBRE, 'N' AS SELECCIONADO FROM TRAMITES.VW_ASUNTOS_PROGRAMACION ORDER BY S_NOMBRE");

            return new datosConsulta() { numRegistros = asuntos.Count(), datos = asuntos.ToList<ASUNTOTIPO>() };
        }

        [Authorize(Roles = "VPROGRAMADORACTUACIONES")]
        [HttpPost]
        [ActionName("ProgramarTramites")]
        public string PostProgramarTramites(DATOSPROGRAMACION programacion)
        {
            PROGRAMACION_ACTUACION programacionActuacion;
            int idUsuario;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            dynamic modelData;
            StringBuilder tramites = new StringBuilder();

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return "Usuario Inválido";
            }

            if (programacion.Tramites != null)
            {
                foreach (DATOSTRAMITE tramite in programacion.Tramites)
                {
                    if (tramites.Length == 0)
                        tramites.Append(tramite.CodTramite.ToString());
                    else
                        tramites.Append("," + tramite.CodTramite.ToString());
                }
            }

            if (programacion.Id == null) // Nueva Programación
            {
                programacionActuacion = new PROGRAMACION_ACTUACION();
                programacionActuacion.ID_USUARIO = idUsuario;
                programacionActuacion.TIPO = programacion.TipoProgramacion;
                programacionActuacion.CODIGO_PROYECTO = programacion.CM;
                programacionActuacion.CODIGO_SOLICITUD = programacion.Tramo;
                programacionActuacion.ID_ZONA = programacion.Zona;
                programacionActuacion.ANO = programacion.Ano;
                programacionActuacion.MES = programacion.Mes;
                programacionActuacion.FECHA_PROGRAMACION = DateTime.Now;
                programacionActuacion.ESTADO = 1; // 1 Programado, 2 Ejecutado
                programacionActuacion.S_TRAMITES = tramites.ToString();

                dbSIM.Entry(programacionActuacion).State = EntityState.Added;
                dbSIM.SaveChanges();
            }
            else // Programación Existente
            {
                programacionActuacion = (from pa in dbSIM.PROGRAMACION_ACTUACION
                                                                where pa.ID_PROGRAMACION == programacion.Id
                                                                select pa).FirstOrDefault();

                if (programacionActuacion == null)
                    return "Programación No Existe";
                else
                {
                    programacionActuacion.ID_USUARIO = idUsuario;
                    programacionActuacion.TIPO = programacion.TipoProgramacion;
                    programacionActuacion.CODIGO_PROYECTO = programacion.CM;
                    programacionActuacion.CODIGO_SOLICITUD = programacion.Tramo;
                    programacionActuacion.ID_ZONA = programacion.Zona;
                    programacionActuacion.ANO = programacion.Ano;
                    programacionActuacion.MES = programacion.Mes;
                    programacionActuacion.FECHA_PROGRAMACION = DateTime.Now;
                    programacionActuacion.ESTADO = 1; // 1 Programado, 2 Ejecutado
                    programacionActuacion.S_TRAMITES = tramites.ToString();

                    dbSIM.Entry(programacionActuacion).State = EntityState.Modified;
                    dbSIM.SaveChanges();
                }
            }

            programacionActuacion.PROGRAMACION_TRAMITES.Clear();

            if (programacion.Tramites != null)
            {
                foreach (DATOSTRAMITE tramite in programacion.Tramites)
                {
                    programacionActuacion.PROGRAMACION_TRAMITES.Add(new PROGRAMACION_TRAMITES() { CODTRAMITE = tramite.CodTramite, CODTAREA = tramite.CodTarea, ID_USUARIO_ASIGNADO = idUsuario, S_ASUNTO = tramite.Asunto });
                }
            }

            if (programacion.NuevasTareas != null)
            {
                programacionActuacion.PROGRAMACION_NUEVOS_ASUNTOS.Clear();
                foreach (int idAsunto in programacion.NuevasTareas)
                {
                    programacionActuacion.PROGRAMACION_NUEVOS_ASUNTOS.Add(new PROGRAMACION_NUEVOS_ASUNTOS() { ID_ASUNTO = idAsunto, ID_USUARIO_ASIGNADO = idUsuario });
                }
            }

            dbSIM.Entry(programacionActuacion).State = EntityState.Modified;
            dbSIM.SaveChanges();

            return "OK";
        }
    }

    public struct datosConsulta
    {
        public int numRegistros;
        public IEnumerable<dynamic> datos;
        public string seleccionados;
    }

    public class ZONA
    {
        public int ID_ZONA { set; get; }
        public string NOMBRE { set; get; }
    }

    public class ASUNTOTIPO
    {
        public int ID { set; get; }
        public string NOMBRE { set; get; }
        public string SELECCIONADO { set; get; }
    }

    public class DATOSTRAMITE
    {
        public int CodTramite { set; get; }
        public int CodTarea { set; get; }
        public string Asunto { set; get; }
    }

    public class DATOSPROGRAMACION
    {
        public int? Id { set; get; }
        public int TipoProgramacion { set; get; }
        public int? CM { set; get; }
        public int? Tramo { set; get; }
        public int? Zona { set; get; }
        public int Ano { set; get; }
        public int Mes { set; get; }
        public DATOSTRAMITE[] Tramites { set; get; }
        public int[] NuevasTareas { set; get; }
    }
}