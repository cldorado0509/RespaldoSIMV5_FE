using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SIM.Areas.Seguridad.Controllers
{
    public class UsuarioApiController : ApiController
    {
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
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        // GET api/<controller>
        [HttpGet]
        [ActionName("Usuarios")]
        public datosConsulta GetUsuarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                            var idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                            var propietario = (from propietarioUsuario in dbSIM.PROPIETARIO
                                               where propietarioUsuario.ID_USUARIO == idUsuario
                                               select propietarioUsuario).FirstOrDefault();

                            if (propietario == null)
                            {
                                datosConsulta resultadoVacio = new datosConsulta();
                                resultadoVacio.numRegistros = 0;
                                resultadoVacio.datos = null;

                                return resultadoVacio;
                            }
                            else
                            {
                                var model = (from usuario in dbSIM.USUARIO
                                             join solicitado in dbSIM.ROL_SOLICITADO on usuario.ID_USUARIO equals solicitado.ID_USUARIO
                                             where solicitado.S_ESTADO == "V" && solicitado.ID_TERCERO == propietario.ID_TERCERO
                                             select new
                                             {
                                                 usuario.ID_USUARIO,
                                                 usuario.S_LOGIN,
                                                 usuario.S_APELLIDOS,
                                                 usuario.S_NOMBRES
                                             });

                                modelData = model;
                            }
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         select new
                                         {
                                             usuario.ID_USUARIO,
                                             usuario.S_LOGIN,
                                             usuario.S_APELLIDOS,
                                             usuario.S_NOMBRES
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         select new
                                         {
                                             ID_POPUP = usuario.ID_USUARIO,
                                             S_NOMBRE_LOOKUP = usuario.S_NOMBRES + " " + usuario.S_APELLIDOS + " (" + usuario.S_LOGIN + ")",
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

        [HttpGet]
        [ActionName("Funcionarios")]
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
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                                         where f.ACTIVO == "1"
                                            select new
                                            {
                                                CODFUNCIONARIO =f.CODFUNCIONARIO,
                                                NOMBRE = f.NOMBRES,
                                                DEPENDENCIA = f.DEPENDENCIA
                                            });

                            modelData = model;
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                                         where f.ACTIVO == "1"
                                         select new
                                         {
                                             f.CODFUNCIONARIO,
                                             NOMBRE = f.NOMBRES,
                                             DEPENDENCIA = f.DEPENDENCIA
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         join funcionario in dbSIM.USUARIO_FUNCIONARIO on usuario.ID_USUARIO equals funcionario.ID_USUARIO
                                         select new
                                         {
                                             ID_POPUP = funcionario.CODFUNCIONARIO,
                                             S_NOMBRE_LOOKUP = usuario.S_NOMBRES + " " + usuario.S_APELLIDOS
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
    }
}