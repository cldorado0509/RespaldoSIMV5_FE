using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using Microsoft.AspNet.Identity;
using SIM.Data;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;
using SIM.Utilidades;
using System.Data.Entity;
using System.Security.Claims;
using System.Transactions;
using SIM.Data.General;
using SIM.Data.Seguridad;

namespace SIM.Areas.General.Controllers
{
    public class TerceroApiController : ApiController
    {
        public class TERCEROUSUARIO
        {
            public int ID_PROPIETARIO;
            public int ID_TERCERO;
            public int ID_USUARIO;
        }

        public class TERCEROCONTACTO
        {
            public decimal ID_CONTACTO;
            public int ID_TERCERO_NATURAL;
            public decimal ID_JURIDICO;
            public string TIPO;
            public TERCERONATURAL TERCERO;
        }

        public class TERCERONATURAL
        {
            public int? ID_TIPODOCUMENTO;
            public long? N_DOCUMENTON;
            public short? N_DIGITOVER;
            public string S_NOMBRE1;
            public string S_NOMBRE2;
            public string S_APELLIDO1;
            public string S_APELLIDO2;
            public string S_GENERO;
            public DateTime? D_NACIMIENTO;
            public int? ID_ACTIVIDADECONOMICA;
            public long? N_TELEFONO;
            public string S_CORREO;
            public decimal? ID_PROFESION;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize]
        [HttpGet, ActionName("Terceros")]
        public datosConsulta GetTerceros(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords) || (!administrador && idTerceroUsuario == null))
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
                            if (administrador)
                            {
                                var model = (from tercero in dbSIM.TERCERO
                                             select new
                                             {
                                                 tercero.ID_TERCERO,
                                                 S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_ABREVIATURA,
                                                 tercero.N_DOCUMENTON,
                                                 tercero.N_DIGITOVER,
                                                 tercero.S_RSOCIAL,
                                                 tercero.JURIDICA.S_SIGLA,
                                                 S_ACTIVIDAD_ECONOMICA = tercero.ACTIVIDAD_ECONOMICA.S_NOMBRE
                                             });

                                modelData = model;
                            }
                            else
                            {
                                var model = (from tercero in dbSIM.TERCERO
                                             where tercero.ID_TERCERO == idTerceroUsuario
                                             select new
                                             {
                                                 tercero.ID_TERCERO,
                                                 S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_ABREVIATURA,
                                                 tercero.N_DOCUMENTON,
                                                 tercero.N_DIGITOVER,
                                                 tercero.S_RSOCIAL,
                                                 tercero.JURIDICA.S_SIGLA,
                                                 S_ACTIVIDAD_ECONOMICA = tercero.ACTIVIDAD_ECONOMICA.S_NOMBRE
                                             });

                                modelData = model;
                            }
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from tercero in dbSIM.TERCERO
                                         select new
                                         {
                                             tercero.ID_TERCERO,
                                             S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_ABREVIATURA,
                                             tercero.N_DOCUMENTON,
                                             tercero.S_RSOCIAL
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from tercero in dbSIM.TERCERO
                                         select new
                                         {
                                             tercero.ID_TERCERO,
                                             tercero.N_DOCUMENTON,
                                             S_NOMBRE_LOOKUP = tercero.S_RSOCIAL, //tercero.S_RSOCIAL + "(" + tercero.TIPO_DOCUMENTO.S_ABREVIATURA + ":" + SqlFunctions.StringConvert((decimal?)tercero.N_DOCUMENTON) + ")",
                                             tercero.S_RSOCIAL
                                         });

                            modelData = model;
                        }
                        break;
                }

                // Obtiene consulta linq dinámicamente de acuerdo a los filtros establecidos
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Consulta de Lista de Contactos de un Tercero con filtros y agrupación
        /// </summary>
        /// <param name="idTercero">Id del Tercero</param>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("TerceroContactos")]
        public datosConsulta GetTerceroContactos(int idTercero, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords) || (!administrador && (idTercero == null || idTerceroUsuario != idTercero )))
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                /*var modelRP = (from contactos in dbSIM.CONTACTOS
                               where contactos.ID_JURIDICO == idTercero
                               select contactos).Max(c => c.D_INICIO);*/

                var model = from contactos in dbSIM.CONTACTOS
                            //where contactos.ID_JURIDICO == idTercero && (!modelRP.HasValue || (contactos.D_INICIO == modelRP.Value && contactos.TIPO == "R"))
                            where contactos.ID_JURIDICO == idTercero && contactos.D_FIN == null
                            orderby contactos.D_INICIO descending
                            select new
                            {
                                contactos.ID_CONTACTO,
                                contactos.TERCERO.ID_TERCERO,
                                contactos.TERCERO.ID_TIPODOCUMENTO,
                                S_TIPO_DOCUMENTO = contactos.TERCERO.TIPO_DOCUMENTO.S_ABREVIATURA,
                                contactos.TERCERO.N_DOCUMENTON,
                                contactos.TERCERO.N_DIGITOVER,
                                S_NOMBRES = contactos.TERCERO.NATURAL.S_NOMBRE1 + (contactos.TERCERO.NATURAL.S_NOMBRE2 == null ? "" : " " + contactos.TERCERO.NATURAL.S_NOMBRE2),
                                S_APELLIDOS = contactos.TERCERO.NATURAL.S_APELLIDO1 + (contactos.TERCERO.NATURAL.S_APELLIDO2 == null ? "" : " " + contactos.TERCERO.NATURAL.S_APELLIDO2),
                                contactos.TIPO,
                                contactos.TERCERO.S_CORREO,
                                contactos.TERCERO.S_WEB
                            };

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Consulta del detalle de un contacto
        /// </summary>
        /// <param name="idContacto">Id de Contacto</param>
        /// <returns>Objeto tipo TERCEROCONTACTO con los datos del contacto</returns>
        [ActionName("ContactoTercero")]
        public object GetContactoTercero(int idContacto)
        {
            TERCEROCONTACTO contactoTercero;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (administrador)
            {
                contactoTercero = (from contactos in dbSIM.CONTACTOS
                                   where contactos.ID_CONTACTO == idContacto
                                   select new TERCEROCONTACTO
                                   {
                                       ID_CONTACTO = contactos.ID_CONTACTO,
                                       ID_TERCERO_NATURAL = contactos.ID_TERCERO_NATURAL,
                                       ID_JURIDICO = contactos.ID_JURIDICO,
                                       TIPO = contactos.TIPO.Trim(),
                                       TERCERO = new TERCERONATURAL
                                       {
                                           ID_TIPODOCUMENTO = contactos.TERCERO.ID_TIPODOCUMENTO,
                                           N_DOCUMENTON = contactos.TERCERO.N_DOCUMENTON,
                                           N_DIGITOVER = contactos.TERCERO.N_DIGITOVER,
                                           S_NOMBRE1 = contactos.TERCERO.NATURAL.S_NOMBRE1,
                                           S_NOMBRE2 = contactos.TERCERO.NATURAL.S_NOMBRE2,
                                           S_APELLIDO1 = contactos.TERCERO.NATURAL.S_APELLIDO1,
                                           S_APELLIDO2 = contactos.TERCERO.NATURAL.S_APELLIDO2,
                                           S_GENERO = contactos.TERCERO.NATURAL.S_GENERO,
                                           D_NACIMIENTO = contactos.TERCERO.NATURAL.D_NACIMIENTO,
                                           ID_ACTIVIDADECONOMICA = contactos.TERCERO.ID_ACTIVIDADECONOMICA,
                                           N_TELEFONO = contactos.TERCERO.N_TELEFONO,
                                           S_CORREO = contactos.TERCERO.S_CORREO,
                                           ID_PROFESION = contactos.TERCERO.NATURAL.ID_PROFESION
                                       }
                                   }).FirstOrDefault();
            }
            else
            {
                contactoTercero = (from contactos in dbSIM.CONTACTOS
                                   where contactos.ID_CONTACTO == idContacto && contactos.ID_JURIDICO == idTerceroUsuario
                                   select new TERCEROCONTACTO
                                   {
                                       ID_CONTACTO = contactos.ID_CONTACTO,
                                       ID_TERCERO_NATURAL = contactos.ID_TERCERO_NATURAL,
                                       ID_JURIDICO = contactos.ID_JURIDICO,
                                       TIPO = contactos.TIPO.Trim(),
                                       TERCERO = new TERCERONATURAL
                                       {
                                           ID_TIPODOCUMENTO = contactos.TERCERO.ID_TIPODOCUMENTO,
                                           N_DOCUMENTON = contactos.TERCERO.N_DOCUMENTON,
                                           N_DIGITOVER = contactos.TERCERO.N_DIGITOVER,
                                           S_NOMBRE1 = contactos.TERCERO.NATURAL.S_NOMBRE1,
                                           S_NOMBRE2 = contactos.TERCERO.NATURAL.S_NOMBRE2,
                                           S_APELLIDO1 = contactos.TERCERO.NATURAL.S_APELLIDO1,
                                           S_APELLIDO2 = contactos.TERCERO.NATURAL.S_APELLIDO2,
                                           S_GENERO = contactos.TERCERO.NATURAL.S_GENERO,
                                           D_NACIMIENTO = contactos.TERCERO.NATURAL.D_NACIMIENTO,
                                           ID_ACTIVIDADECONOMICA = contactos.TERCERO.ID_ACTIVIDADECONOMICA,
                                           N_TELEFONO = contactos.TERCERO.N_TELEFONO,
                                           S_CORREO = contactos.TERCERO.S_CORREO,
                                           ID_PROFESION = contactos.TERCERO.NATURAL.ID_PROFESION
                                       }
                                   }).FirstOrDefault();
            }

            return contactoTercero;
        }

        /// <summary>
        /// Consulta del detalle de un contacto
        /// </summary>
        /// <param name="idTercero">Id de Tercero</param>
        /// <param name="tipoDocumento">Tipo de documento del Tercero</param>
        /// <param name="identificacion">Identificación del Tercero</param>
        /// <returns>Objeto tipo TERCEROCONTACTO con los datos del contacto</returns>
        [ActionName("ContactoTerceroIdentificacion")]
        public object GetContactoTerceroIdentificacion(int idTercero, int tipoDocumento, long identificacion)
        {
            TERCEROCONTACTO contactoTercero;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (administrador)
            {

                contactoTercero = (from contactos in dbSIM.CONTACTOS
                                   where contactos.ID_JURIDICO == idTercero && contactos.TERCERO.ID_TIPODOCUMENTO == tipoDocumento && contactos.TERCERO.N_DOCUMENTON == identificacion
                                   select new TERCEROCONTACTO
                                   {
                                       ID_CONTACTO = contactos.ID_CONTACTO,
                                       ID_TERCERO_NATURAL = contactos.ID_TERCERO_NATURAL,
                                       ID_JURIDICO = contactos.ID_JURIDICO,
                                       TIPO = contactos.TIPO,
                                       TERCERO = new TERCERONATURAL
                                       {
                                           ID_TIPODOCUMENTO = contactos.TERCERO.ID_TIPODOCUMENTO,
                                           N_DOCUMENTON = contactos.TERCERO.N_DOCUMENTON,
                                           N_DIGITOVER = contactos.TERCERO.N_DIGITOVER,
                                           S_NOMBRE1 = contactos.TERCERO.NATURAL.S_NOMBRE1,
                                           S_NOMBRE2 = contactos.TERCERO.NATURAL.S_NOMBRE2,
                                           S_APELLIDO1 = contactos.TERCERO.NATURAL.S_APELLIDO1,
                                           S_APELLIDO2 = contactos.TERCERO.NATURAL.S_APELLIDO2,
                                           S_GENERO = contactos.TERCERO.NATURAL.S_GENERO,
                                           D_NACIMIENTO = contactos.TERCERO.NATURAL.D_NACIMIENTO,
                                           ID_ACTIVIDADECONOMICA = contactos.TERCERO.ID_ACTIVIDADECONOMICA,
                                           N_TELEFONO = contactos.TERCERO.N_TELEFONO,
                                           S_CORREO = contactos.TERCERO.S_CORREO,
                                           ID_PROFESION = contactos.TERCERO.NATURAL.ID_PROFESION
                                       }
                                   }).FirstOrDefault();

                if (contactoTercero == null)
                {
                    contactoTercero = (from tercero in dbSIM.TERCERO
                                       where tercero.ID_TIPODOCUMENTO == tipoDocumento && tercero.N_DOCUMENTON == identificacion
                                       select new TERCEROCONTACTO
                                       {
                                           ID_CONTACTO = 0,
                                           ID_TERCERO_NATURAL = tercero.ID_TERCERO,
                                           ID_JURIDICO = idTercero,
                                           TIPO = null,
                                           TERCERO = new TERCERONATURAL
                                           {
                                               ID_TIPODOCUMENTO = tercero.ID_TIPODOCUMENTO,
                                               N_DOCUMENTON = tercero.N_DOCUMENTON,
                                               N_DIGITOVER = tercero.N_DIGITOVER,
                                               S_NOMBRE1 = tercero.NATURAL.S_NOMBRE1,
                                               S_NOMBRE2 = tercero.NATURAL.S_NOMBRE2,
                                               S_APELLIDO1 = tercero.NATURAL.S_APELLIDO1,
                                               S_APELLIDO2 = tercero.NATURAL.S_APELLIDO2,
                                               S_GENERO = tercero.NATURAL.S_GENERO,
                                               D_NACIMIENTO = tercero.NATURAL.D_NACIMIENTO,
                                               ID_ACTIVIDADECONOMICA = tercero.ID_ACTIVIDADECONOMICA,
                                               N_TELEFONO = tercero.N_TELEFONO,
                                               S_CORREO = tercero.S_CORREO,
                                               ID_PROFESION = tercero.NATURAL.ID_PROFESION
                                           }
                                       }).FirstOrDefault();
                }
            }
            else
            {
                contactoTercero = (from contactos in dbSIM.CONTACTOS
                                   where contactos.ID_JURIDICO == idTercero && contactos.TERCERO.ID_TIPODOCUMENTO == tipoDocumento && contactos.TERCERO.N_DOCUMENTON == identificacion && contactos.ID_JURIDICO == idTerceroUsuario
                                   select new TERCEROCONTACTO
                                   {
                                       ID_CONTACTO = contactos.ID_CONTACTO,
                                       ID_TERCERO_NATURAL = contactos.ID_TERCERO_NATURAL,
                                       ID_JURIDICO = contactos.ID_JURIDICO,
                                       TIPO = contactos.TIPO,
                                       TERCERO = new TERCERONATURAL
                                       {
                                           ID_TIPODOCUMENTO = contactos.TERCERO.ID_TIPODOCUMENTO,
                                           N_DOCUMENTON = contactos.TERCERO.N_DOCUMENTON,
                                           N_DIGITOVER = contactos.TERCERO.N_DIGITOVER,
                                           S_NOMBRE1 = contactos.TERCERO.NATURAL.S_NOMBRE1,
                                           S_NOMBRE2 = contactos.TERCERO.NATURAL.S_NOMBRE2,
                                           S_APELLIDO1 = contactos.TERCERO.NATURAL.S_APELLIDO1,
                                           S_APELLIDO2 = contactos.TERCERO.NATURAL.S_APELLIDO2,
                                           S_GENERO = contactos.TERCERO.NATURAL.S_GENERO,
                                           D_NACIMIENTO = contactos.TERCERO.NATURAL.D_NACIMIENTO,
                                           ID_ACTIVIDADECONOMICA = contactos.TERCERO.ID_ACTIVIDADECONOMICA,
                                           N_TELEFONO = contactos.TERCERO.N_TELEFONO,
                                           S_CORREO = contactos.TERCERO.S_CORREO,
                                           ID_PROFESION = contactos.TERCERO.NATURAL.ID_PROFESION
                                       }
                                   }).FirstOrDefault();

                if (contactoTercero == null)
                {
                    contactoTercero = (from tercero in dbSIM.TERCERO
                                       where tercero.ID_TIPODOCUMENTO == tipoDocumento && tercero.N_DOCUMENTON == identificacion// && tercero.ID_TERCERO == idTerceroUsuario
                                       select new TERCEROCONTACTO
                                       {
                                           ID_CONTACTO = 0,
                                           ID_TERCERO_NATURAL = tercero.ID_TERCERO,
                                           ID_JURIDICO = idTercero,
                                           TIPO = null,
                                           TERCERO = new TERCERONATURAL
                                           {
                                               ID_TIPODOCUMENTO = tercero.ID_TIPODOCUMENTO,
                                               N_DOCUMENTON = tercero.N_DOCUMENTON,
                                               N_DIGITOVER = tercero.N_DIGITOVER,
                                               S_NOMBRE1 = tercero.NATURAL.S_NOMBRE1,
                                               S_NOMBRE2 = tercero.NATURAL.S_NOMBRE2,
                                               S_APELLIDO1 = tercero.NATURAL.S_APELLIDO1,
                                               S_APELLIDO2 = tercero.NATURAL.S_APELLIDO2,
                                               S_GENERO = tercero.NATURAL.S_GENERO,
                                               D_NACIMIENTO = tercero.NATURAL.D_NACIMIENTO,
                                               ID_ACTIVIDADECONOMICA = tercero.ID_ACTIVIDADECONOMICA,
                                               N_TELEFONO = tercero.N_TELEFONO,
                                               S_CORREO = tercero.S_CORREO,
                                               ID_PROFESION = tercero.NATURAL.ID_PROFESION
                                           }
                                       }).FirstOrDefault();
                }
            }

            return contactoTercero;
        }

        /// <summary>
        /// Elimina un contacto
        /// </summary>
        /// <param name="idContacto">Id de Contacto</param>
        /// <returns>Mensaje de resultado de la operacion (OK, Error)</returns>
        [HttpPost, ActionName("ContactoTerceroDelete")]
        public object PostContactoTerceroDelete(int idContacto)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            var contactoTercero = (from contacto in dbSIM.CONTACTOS
                                   where contacto.ID_CONTACTO == idContacto
                                   select contacto).FirstOrDefault();

            if (contactoTercero == null)
            {
                return new { resp = "Error", mensaje = "Error Eliminando Contacto Tercero. El Contacto no Existe." };
            }
            else
            {
                // Evita que un usuario que no sea administrador de terceros o el usuario asociado al tercero, pueda borrar el contacto
                if (!administrador && (contactoTercero.ID_JURIDICO != idTerceroUsuario))
                {
                    return new { resp = "Error", mensaje = "Error Eliminando Contacto Tercero. No tiene permisos para reliazar esta operación." };
                }
                
                dbSIM.Entry(contactoTercero).State = EntityState.Deleted;

                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Eliminando Contacto Tercero" };
                }
            }

            return new { resp = "OK", mensaje = "Contacto Eliminado Satisfactoriamente" };
        }

        /// <summary>
        /// Consulta de Lista de Instalaciones de un Tercero con filtros y agrupación
        /// </summary>
        /// <param name="idTercero">Id del Tercero</param>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("TerceroInstalaciones")]
        public datosConsulta GetTerceroInstalaciones(int idTercero, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords) || (!administrador && (idTercero == null || idTerceroUsuario != idTercero )))
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                /*var model1 = from instalacion in dbSIM.INSTALACION
                                           join tercero in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals tercero.ID_INSTALACION
                                           where tercero.ID_TERCERO == idTercero
                                           join tipoinstalacion in dbSIM.TIPO_INSTALACION on tercero.ID_TIPOINSTALACION equals tipoinstalacion.ID_TIPOINSTALACION
                                           join actividadeconomica in dbSIM.ACTIVIDAD_ECONOMICA on tercero.ID_ACTIVIDADECONOMICA equals actividadeconomica.ID_ACTIVIDADECONOMICA into aej
                                           from instalaciones in aej.DefaultIfEmpty()
                                           join estado in dbSIM.ESTADO on tercero.ID_ESTADO equals estado.ID_ESTADO into ej
                                           from estados in ej.DefaultIfEmpty()
                                           select new
                                           {
                                               instalacion.ID_INSTALACION,
                                               S_INSTALACION = instalacion.S_NOMBRE,
                                               S_TIPOINSTALACION = tipoinstalacion.S_NOMBRE,
                                               S_ACTIVIDADECONOMICA = instalaciones.S_NOMBRE,
                                               instalacion.S_TELEFONO,
                                               tercero.ID_TERCERO,
                                               tercero.D_INICIO,
                                               tercero.D_FIN,
                                               S_ESTADO = estados.S_NOMBRE
                                           };*/

                var model = from instalacionTercero in dbSIM.TERCERO_INSTALACION
                            join tipoInstalacion in dbSIM.TIPO_INSTALACION on instalacionTercero.ID_TIPOINSTALACION equals tipoInstalacion.ID_TIPOINSTALACION
                            where instalacionTercero.ID_TERCERO == idTercero
                            select new
                            {
                                instalacionTercero.ID_TERCERO,
                                instalacionTercero.ID_INSTALACION,
                                instalacionTercero.INSTALACION.S_NOMBRE,
                                instalacionTercero.ID_TIPOINSTALACION,
                                S_TIPOINSTALACION = tipoInstalacion.S_NOMBRE,
                                instalacionTercero.TERCERO.ID_ACTIVIDADECONOMICA,
                                S_ACTIVIDAD_ECONOMICA = instalacionTercero.TERCERO.ACTIVIDAD_ECONOMICA.S_NOMBRE,
                                instalacionTercero.TERCERO.ID_ESTADO,
                                S_ESTADO = instalacionTercero.TERCERO.ESTADO.S_NOMBRE
                            };

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Consulta de Lista de Usuarios de un Tercero con filtros y agrupación
        /// </summary>
        /// <param name="idTercero">Id del Tercero</param>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("TerceroUsuarios")]
        public datosConsulta GetTerceroUsuarios(int idTercero, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            // Obtiene el rol y idTercero del usuario autenticado
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                //administrador = claimPpal.IsInRole("XDGA");
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords) || (!administrador && (idTercero == null || idTerceroUsuario != idTercero )))
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = from propietario in dbSIM.PROPIETARIO
                            //join usuarios in dbSIM.USUARIO on propietario.ID_USUARIO equals usuarios.ID_USUARIO
                            where propietario.ID_TERCERO == idTercero
                            select new
                            {
                                propietario.ID_PROPIETARIO,
                                propietario.ID_TERCERO,
                                propietario.ID_USUARIO,
                                propietario.USUARIO.S_LOGIN,
                                propietario.USUARIO.S_NOMBRES,
                                propietario.USUARIO.S_APELLIDOS
                            };

                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        //[Authorize(Roles = "VTERCERO")]
        [ActionName("TerceroBasicoIdentificacion")]
        public object GetTerceroBasicoIdentificacion(int tipoTercero, long identificacion)
        {
            var tercero = from terceroConsulta in dbSIM.TERCERO
                          where terceroConsulta.ID_TIPODOCUMENTO == tipoTercero && terceroConsulta.N_DOCUMENTON == identificacion
                          select new
                          {
                              terceroConsulta.ID_TERCERO,
                              terceroConsulta.ID_TIPODOCUMENTO,
                              terceroConsulta.N_DOCUMENTON,
                              terceroConsulta.N_DOCUMENTO,
                              terceroConsulta.N_DIGITOVER,
                              terceroConsulta.S_RSOCIAL,
                              NATURAL = new
                              {
                                  terceroConsulta.NATURAL.ID_TERCERO,
                                  terceroConsulta.NATURAL.S_NOMBRE1,
                                  terceroConsulta.NATURAL.S_NOMBRE2,
                                  terceroConsulta.NATURAL.S_APELLIDO1,
                                  terceroConsulta.NATURAL.S_APELLIDO2,
                                  terceroConsulta.NATURAL.S_GENERO,
                              }
                          };

            return tercero.FirstOrDefault();
        }

        [ActionName("TerceroValidacion")]
        public int GetTerceroValidacion(long identificacion)
        {
            var tercero = (from terceroConsulta in dbSIM.TERCERO
                          where terceroConsulta.N_DOCUMENTON == identificacion
                          select terceroConsulta.ID_TERCERO).FirstOrDefault();

            return tercero;
        }

        [Authorize]
        [ActionName("TerceroBasico")]
        public object GetTerceroBasico(int id)
        {
            return GetTercero(id, null, "basico");
        }

        [Authorize]
        [ActionName("TerceroBasico")]
        public object GetTerceroBasico(int id, string tipoTercero)
        {
            return GetTercero(id, tipoTercero, "basico");
        }

        [HttpGet]
        [ActionName("Tercero")]
        [Authorize]
        public object GetTercero(int id)
        {
            return GetTercero(id, null, null);
        }

        [HttpGet]
        [ActionName("Tercero")]
        [Authorize]
        public object GetTercero(int id, string tipoTercero)
        {
            return GetTercero(id, tipoTercero, null);
        }
        // GET api/Tercero/<id>
        [HttpGet]
        [ActionName("Tercero")]
        [Authorize]
        public object GetTercero(int? id, string tipoTercero, string tipoRespuesta)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XTERCERO");
            }

            if (!administrador)
            {
                // Si no es administrador de terceros, solamente puede devolver el tercero que le corresponde a su usuario
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    id = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                }
                else
                {
                    // No es administrador de terceros y no tiene tercero asignado, por lo tanto no debe devolver datos
                    return null;
                }
            }

            if (tipoTercero == null)
            {
                var terceroObtenerTipo = dbSIM.TERCERO.Find(id);
                if (terceroObtenerTipo != null)
                    tipoTercero = (terceroObtenerTipo.ID_TIPODOCUMENTO == 2 ? "J" : "N");
            }

            if (tipoRespuesta != null && tipoRespuesta == "basico")
            {
                var tercero = from terceroConsulta in dbSIM.TERCERO
                              where terceroConsulta.ID_TERCERO == id
                              select new
                              {
                                  terceroConsulta.ID_TERCERO,
                                  terceroConsulta.ID_TIPODOCUMENTO,
                                  terceroConsulta.N_DOCUMENTON,
                                  terceroConsulta.N_DOCUMENTO,
                                  terceroConsulta.N_DIGITOVER,
                                  terceroConsulta.S_RSOCIAL
                              };

                return tercero.FirstOrDefault();
            }

            if (tipoTercero == "N")
            {
                var tiposIdentificacion = ModelsToListGeneral.GetTiposDocumentoNatural();

                var tercero = from terceroConsulta in dbSIM.TERCERO
                              where terceroConsulta.ID_TERCERO == id
                              select new
                              {
                                  terceroConsulta.ID_TERCERO,
                                  terceroConsulta.ID_TIPODOCUMENTO,
                                  terceroConsulta.N_DOCUMENTON,
                                  terceroConsulta.N_DOCUMENTO,
                                  terceroConsulta.N_DIGITOVER,
                                  terceroConsulta.S_RSOCIAL,
                                  terceroConsulta.ID_ACTIVIDADECONOMICA,
                                  VERSION_AE = terceroConsulta.ACTIVIDAD_ECONOMICA.S_VERSION,
                                  terceroConsulta.N_TELEFONO,
                                  terceroConsulta.N_FAX,
                                  terceroConsulta.S_CORREO,
                                  terceroConsulta.S_WEB,
                                  NATURAL = new
                                  {
                                      terceroConsulta.NATURAL.ID_TERCERO,
                                      terceroConsulta.NATURAL.S_NOMBRE1,
                                      terceroConsulta.NATURAL.S_NOMBRE2,
                                      terceroConsulta.NATURAL.S_APELLIDO1,
                                      terceroConsulta.NATURAL.S_APELLIDO2,
                                      terceroConsulta.NATURAL.S_GENERO,
                                      terceroConsulta.NATURAL.D_NACIMIENTO,
                                      terceroConsulta.NATURAL.S_MATRICULAPROFESIONAL
                                  }
                              };

                return tercero.FirstOrDefault();
            }
            else
            {
                var tiposIdentificacion = ModelsToListGeneral.GetTiposDocumentoJuridica();

                var tercero = from terceroConsulta in dbSIM.TERCERO
                              where terceroConsulta.ID_TERCERO == id
                              select new
                              {
                                  terceroConsulta.ID_TERCERO,
                                  terceroConsulta.ID_TIPODOCUMENTO,
                                  terceroConsulta.N_DOCUMENTON,
                                  terceroConsulta.N_DOCUMENTO,
                                  terceroConsulta.N_DIGITOVER,
                                  terceroConsulta.S_RSOCIAL,
                                  terceroConsulta.ID_ACTIVIDADECONOMICA,
                                  VERSION_AE = terceroConsulta.ACTIVIDAD_ECONOMICA.S_VERSION,
                                  terceroConsulta.N_TELEFONO,
                                  terceroConsulta.N_FAX,
                                  terceroConsulta.S_CORREO,
                                  terceroConsulta.S_WEB,
                                  JURIDICA = new
                                  {
                                      terceroConsulta.JURIDICA.ID_TERCERO,
                                      terceroConsulta.JURIDICA.S_NATURALEZA,
                                      terceroConsulta.JURIDICA.S_SIGLA,
                                      terceroConsulta.JURIDICA.D_CONSTITUCION,
                                      terceroConsulta.JURIDICA.S_DCAMCOMERCIO,
                                  },
                              };

                return tercero.FirstOrDefault();
            }
        }

        // GET api/Tercero/TIN
        [HttpGet]
        [ActionName("TiposIdentificacionNatural")]
        public object GetTiposIdentificacionNatural()
        {
            var tiposIdentificacion = ModelsToListGeneral.GetTiposDocumentoNatural();

            return tiposIdentificacion;
        }

        // GET api/Tercero/TIJ
        [HttpGet]
        [ActionName("TiposIdentificacionJuridica")]
        public object GetTiposIdentificacionJuridica()
        {
            var tiposIdentificacion = ModelsToListGeneral.GetTiposDocumentoJuridica();

            return tiposIdentificacion;
        }

        //[Authorize(Roles = "CTERCERO,ATERCERO")]
        [Authorize]
        [HttpPost, ActionName("ContactoTercero")]
        public object PostContactoTercero(TERCEROCONTACTO item)
        {
            int idTercero = 0;

            using (var trans = new TransactionScope())
            {
                try
                {
                    // Actualizamos los datos del Tercero Natural
                    var tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == item.ID_TERCERO_NATURAL).FirstOrDefault();

                    if (tercero != null)
                    {
                        tercero.NATURAL.S_NOMBRE1 = item.TERCERO.S_NOMBRE1;
                        tercero.NATURAL.S_NOMBRE2 = item.TERCERO.S_NOMBRE2;
                        tercero.NATURAL.S_APELLIDO1 = item.TERCERO.S_APELLIDO1;
                        tercero.NATURAL.S_APELLIDO2 = item.TERCERO.S_APELLIDO2;
                        tercero.S_RSOCIAL = (item.TERCERO.S_NOMBRE1.Trim() + " " + (item.TERCERO.S_NOMBRE2 == null ? "" : item.TERCERO.S_NOMBRE2.Trim()) + " " + item.TERCERO.S_APELLIDO1.Trim() + " " + (item.TERCERO.S_APELLIDO2 == null ? "" : item.TERCERO.S_APELLIDO2.Trim())).Trim().Replace("  ", " ");
                        tercero.NATURAL.S_GENERO = item.TERCERO.S_GENERO;
                        tercero.NATURAL.D_NACIMIENTO = item.TERCERO.D_NACIMIENTO;
                        tercero.ID_ACTIVIDADECONOMICA = item.TERCERO.ID_ACTIVIDADECONOMICA;
                        tercero.NATURAL.ID_PROFESION = item.TERCERO.ID_PROFESION;
                        tercero.N_TELEFONO = item.TERCERO.N_TELEFONO;
                        tercero.S_CORREO = item.TERCERO.S_CORREO;

                        dbSIM.Entry(tercero).State = EntityState.Modified;

                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        tercero = new TERCERO();
                        tercero.ID_TIPODOCUMENTO = item.TERCERO.ID_TIPODOCUMENTO;
                        tercero.N_DOCUMENTON = item.TERCERO.N_DOCUMENTON;
                        tercero.N_DIGITOVER = Utilidades.Data.ObtenerDigitoVerificacion(item.TERCERO.N_DOCUMENTON.ToString());
                        tercero.N_DOCUMENTO = ((long)item.TERCERO.N_DOCUMENTON) * 10 + (byte)tercero.N_DIGITOVER;
                        tercero.NATURAL = new NATURAL();
                        tercero.NATURAL.S_NOMBRE1 = item.TERCERO.S_NOMBRE1;
                        tercero.NATURAL.S_NOMBRE2 = item.TERCERO.S_NOMBRE2;
                        tercero.NATURAL.S_APELLIDO1 = item.TERCERO.S_APELLIDO1;
                        tercero.NATURAL.S_APELLIDO2 = item.TERCERO.S_APELLIDO2;
                        tercero.S_RSOCIAL = (item.TERCERO.S_NOMBRE1.Trim() + " " + (item.TERCERO.S_NOMBRE2 == null ? "" : item.TERCERO.S_NOMBRE2.Trim()) + " " + item.TERCERO.S_APELLIDO1.Trim() + " " + (item.TERCERO.S_APELLIDO2 == null ? "" : item.TERCERO.S_APELLIDO2.Trim())).Trim().Replace("  ", " ");
                        tercero.NATURAL.S_GENERO = item.TERCERO.S_GENERO;
                        tercero.NATURAL.D_NACIMIENTO = item.TERCERO.D_NACIMIENTO;
                        tercero.ID_ACTIVIDADECONOMICA = item.TERCERO.ID_ACTIVIDADECONOMICA;
                        tercero.NATURAL.ID_PROFESION = item.TERCERO.ID_PROFESION;
                        tercero.N_TELEFONO = item.TERCERO.N_TELEFONO;
                        tercero.S_CORREO = item.TERCERO.S_CORREO;

                        dbSIM.Entry(tercero).State = EntityState.Added;

                        dbSIM.SaveChanges();
                    }

                    idTercero = tercero.ID_TERCERO;
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Contacto Tercero" };
                }

                // Verificamos si el Contacto existe y se actualiza
                if (item.ID_CONTACTO > 0) // Contacto Existe
                {
                    try
                    {
                        // Actualizamos los datos del Contacto
                        var contactoTercero = dbSIM.CONTACTOS.Where(tcontacto => tcontacto.ID_CONTACTO == item.ID_CONTACTO).FirstOrDefault();

                        contactoTercero.ID_TERCERO_NATURAL = idTercero;
                        contactoTercero.ID_JURIDICO = item.ID_JURIDICO;
                        contactoTercero.D_INICIO = DateTime.Today;
                        contactoTercero.D_FIN = null;
                        contactoTercero.TIPO = item.TIPO;

                        dbSIM.Entry(contactoTercero).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return new { resp = "Error", mensaje = "Error Almacenando Contacto Tercero" };
                    }

                    trans.Complete();
                    return new { resp = "OK", mensaje = "Contacto Tercero Almacenado Satisfactoriamente" };
                }
                else
                {
                    try
                    {
                        // Nuevo Contacto
                        var contactoTercero = new CONTACTOS();

                        contactoTercero.ID_CONTACTO = item.ID_CONTACTO;
                        contactoTercero.ID_TERCERO_NATURAL = idTercero;
                        contactoTercero.ID_JURIDICO = item.ID_JURIDICO;
                        contactoTercero.D_INICIO = DateTime.Today;
                        contactoTercero.TIPO = item.TIPO;

                        dbSIM.Entry(contactoTercero).State = EntityState.Added;
                        dbSIM.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return new { resp = "Error", mensaje = "Error Almacenando Contacto Tercero" };
                    }

                    trans.Complete();
                    return new { resp = "OK", mensaje = "Contacto Tercero Almacenado Satisfactoriamente" };
                }
            }
        }

        //[Authorize(Roles = "CTERCERO,ATERCERO")]
        [Authorize]
        [HttpGet, ActionName("ContactoTerceroDeshabilitar")]
        public object GetContactoTerceroDeshabilitar(int idContacto)
        {
            try
            {
                // Actualizamos los datos del Contacto
                var contactoTercero = dbSIM.CONTACTOS.Where(tcontacto => tcontacto.ID_CONTACTO == (decimal)idContacto).FirstOrDefault();

                contactoTercero.D_FIN = DateTime.Today;

                dbSIM.Entry(contactoTercero).State = EntityState.Modified;
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Contacto Tercero" };
            }

            return new { resp = "OK", mensaje = "Contacto Tercero Almacenado Satisfactoriamente" };
        }

        //[Authorize(Roles = "CTERCERO,ATERCERO")]
        [Authorize]
        [HttpPost, ActionName("UsuarioTercero")]
        public object PostUsuarioTercero(TERCEROUSUARIO item)
        {
            // Verificación que el usuario no se encuentre asignado a algún tercero

            var usuario = (from propietario in dbSIM.PROPIETARIO
                           //where propietario.ID_TERCERO == item.ID_TERCERO && propietario.ID_USUARIO == item.ID_USUARIO
                           where propietario.ID_USUARIO == item.ID_USUARIO
                           select new { propietario.ID_USUARIO }).FirstOrDefault();

            if (usuario == null)
            {
                try
                {
                    // Actualizamos los datos del Contacto
                    var usuarioTercero = new PROPIETARIO();

                    usuarioTercero.ID_TERCERO = item.ID_TERCERO;
                    usuarioTercero.ID_USUARIO = item.ID_USUARIO;
                    usuarioTercero.D_INICIO = DateTime.Today;

                    dbSIM.Entry(usuarioTercero).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Usuario Tercero" };
                }
                return new { resp = "OK", mensaje = "Usuario Tercero Almacenado Satisfactoriamente" };
            }
            else
            {
                return new { resp = "Error", mensaje = "Error Almacenando Usuario Tercero. El Usuario ya está asignado a un Tercero" };
            }
        }

        //[Authorize(Roles = "CTERCERO,ATERCERO")]
        [Authorize]
        [HttpPost, ActionName("Tercero")]
        public object PostTercero(TERCERO item)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool nuevo = false;
            var model = dbSIM.TERCERO;
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ID_TERCERO == 0) // Nuevo Tercero
                    {
                        var modelItem = model.FirstOrDefault(t => t.N_DOCUMENTON == item.N_DOCUMENTON);
                        if (modelItem != null)
                        {
                            return new { resp = "Error", mensaje = "El Tercero YA Existe" };
                        }
                        else
                        {
                            using (var trans = new TransactionScope())
                            {
                                nuevo = true;
                                item.N_DOCUMENTO = ((long)item.N_DOCUMENTON) * 10 + (byte)item.N_DIGITOVER;



                                if (item.ID_TIPODOCUMENTO != 2) // Natural
                                {
                                    item.S_RSOCIAL = (item.NATURAL.S_NOMBRE1.Trim() + " " + (item.NATURAL.S_NOMBRE2 == null ? "" : item.NATURAL.S_NOMBRE2.Trim()) + " " + item.NATURAL.S_APELLIDO1.Trim() + " " + (item.NATURAL.S_APELLIDO2 == null ? "" : item.NATURAL.S_APELLIDO2.Trim())).Trim().Replace("  ", " ");
                                    item.JURIDICA = null;
                                }
                                else
                                {
                                    item.NATURAL = null;
                                }

                                dbSIM.Entry(item).State = EntityState.Added;
                                dbSIM.SaveChanges();

                                var administrador = false;

                                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
                                {
                                    administrador = claimPpal.IsInRole("XTERCERO");
                                }

                                if (!administrador)
                                {
                                    var propietario = new PROPIETARIO();
                                    int idUsuario = Convert.ToInt32(User.Identity.GetUserId());

                                    propietario.ID_TERCERO = item.ID_TERCERO;
                                    propietario.ID_USUARIO = idUsuario;

                                    dbSIM.Entry(propietario).State = EntityState.Added;
                                    dbSIM.SaveChanges();

                                    SIM.Areas.Seguridad.Controllers.AccountController a = new Seguridad.Controllers.AccountController();
                                    a.LoginUpdateSync(User, context);
                                }

                                trans.Complete();
                            }
                        }
                    }
                    else // Tercero Existente
                    {
                        if (item.ID_TIPODOCUMENTO == 2) // Nit, por lo tanto valida que tenga representante legal registrado
                        {
                            var representanteLegal = (from rl in dbSIM.CONTACTOS
                                                      where rl.ID_JURIDICO == item.ID_TERCERO && rl.TIPO == "R" && rl.D_FIN == null
                                                      select rl).FirstOrDefault();

                            if (representanteLegal == null)
                            {
                                return new { resp = "Error", mensaje = "Error Actualizando Tercero\r\nDebe Existir por lo menos el Contacto del Representante Legal" };
                            }
                        }

                        var modelItem = model.FirstOrDefault(it => it.ID_TERCERO == item.ID_TERCERO);
                        if (modelItem != null)
                        {
                            modelItem.ID_TERCERO = item.ID_TERCERO;
                            modelItem.ID_TIPODOCUMENTO = item.ID_TIPODOCUMENTO;
                            modelItem.N_DOCUMENTON = item.N_DOCUMENTON;
                            modelItem.N_DIGITOVER = item.N_DIGITOVER;
                            modelItem.N_DOCUMENTO = ((long)item.N_DOCUMENTON) * 10 + (byte)item.N_DIGITOVER;
                            modelItem.S_RSOCIAL = item.S_RSOCIAL;
                            modelItem.ID_ACTIVIDADECONOMICA = item.ID_ACTIVIDADECONOMICA;
                            modelItem.N_TELEFONO = item.N_TELEFONO;
                            modelItem.N_FAX = item.N_FAX;
                            modelItem.S_CORREO = item.S_CORREO;
                            modelItem.S_WEB = item.S_WEB;

                            if (item.NATURAL != null)
                            {
                                item.S_RSOCIAL = (item.NATURAL.S_NOMBRE1.Trim() + " " + (item.NATURAL.S_NOMBRE2 == null ? "" : item.NATURAL.S_NOMBRE2.Trim()) + " " + item.NATURAL.S_APELLIDO1.Trim() + " " + (item.NATURAL.S_APELLIDO2 == null ? "" : item.NATURAL.S_APELLIDO2.Trim())).Trim().Replace("  ", " ");
                                modelItem.NATURAL.ID_TERCERO = item.NATURAL.ID_TERCERO;
                                modelItem.NATURAL.S_NOMBRE1 = item.NATURAL.S_NOMBRE1;
                                modelItem.NATURAL.S_NOMBRE2 = item.NATURAL.S_NOMBRE2;
                                modelItem.NATURAL.S_APELLIDO1 = item.NATURAL.S_APELLIDO1;
                                modelItem.NATURAL.S_APELLIDO2 = item.NATURAL.S_APELLIDO2;
                                modelItem.S_RSOCIAL = item.S_RSOCIAL;
                                modelItem.NATURAL.S_GENERO = item.NATURAL.S_GENERO;
                                modelItem.NATURAL.D_NACIMIENTO = item.NATURAL.D_NACIMIENTO;
                                modelItem.NATURAL.S_MATRICULAPROFESIONAL = item.NATURAL.S_MATRICULAPROFESIONAL;
                            }

                            if (item.JURIDICA != null)
                            {
                                modelItem.JURIDICA.ID_TERCERO = item.JURIDICA.ID_TERCERO;
                                modelItem.JURIDICA.S_NATURALEZA = item.JURIDICA.S_NATURALEZA;
                                modelItem.JURIDICA.S_SIGLA = item.JURIDICA.S_SIGLA;
                                modelItem.JURIDICA.S_DCAMCOMERCIO = item.JURIDICA.S_DCAMCOMERCIO;
                                modelItem.JURIDICA.D_CONSTITUCION = item.JURIDICA.D_CONSTITUCION;
                            }

                            dbSIM.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = (nuevo ? "Error Insertando Tercero" : "Error Actualizando Tercero") + "\r\n" + Utilidades.Data.ObtenerError(e) };
                }
                return new { resp = "OK", mensaje = (nuevo ? "Tercero Insertado Satisfactoriamente" : "Tercero Actualizado Satisfactoriamente"), datos = item };
            }
            else
                return new { resp = "Error", mensaje = "Datos Inválidos" };
        }

        [Authorize(Roles = "ETERCERO")]
        public void Delete(int id)
        {
        }
    }
}