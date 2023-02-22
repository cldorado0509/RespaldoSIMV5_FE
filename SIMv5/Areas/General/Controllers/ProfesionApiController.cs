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

namespace SIM.Areas.General.Controllers
{
    public class ProfesionApiController : ApiController
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

        // GET api/<controller>
        [HttpGet]
        [ActionName("Profesiones")]
        public datosConsulta GetProfesiones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                    case "r": // reduced
                    case "f": // full
                        {
                            var model = (from profesion in dbSIM.PROFESION
                                         select new
                                         {
                                             profesion.ID_PROFESION,
                                             profesion.S_NOMBRE,
                                             profesion.S_DESCRIPCION
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from profesion in dbSIM.PROFESION
                                         select new
                                         {
                                             profesion.ID_PROFESION,
                                             S_NOMBRE_LOOKUP = profesion.S_NOMBRE,
                                         });

                            modelData = model;
                        }
                        break;
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
    }
}