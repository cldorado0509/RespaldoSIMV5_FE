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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace SIM.Areas.General.Controllers
{
   
    public class TipoPersonalApiController : ApiController
    {


        private class AELookUp
        {
            public int ID_TIPOPERSONAL { get; set; }
            [Column("S_NOMBRE_LOOKUP", TypeName = "VARCHAR2")]
            public string S_NOMBRE_LOOKUP { get; set; }
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

        // GET api/<controller>
        [HttpGet]
        [ActionName("TiposPersonal")]
        public datosConsulta GetTiposPersonal(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                            var model = (from ae in dbSIM.TIPO_PERSONAL_DGA
                                         select new
                                         {
                                             ae.ID_TIPOPERSONAL,
                                             ae.S_NOMBRE,
                                             ae.S_DESCRIPCION,
                                         });

                            modelData = model;
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from ae in dbSIM.TIPO_PERSONAL_DGA
                                         select new
                                         {
                                             ae.ID_TIPOPERSONAL,
                                             ae.S_NOMBRE,
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            if (take == 1)
                            {
                                var model = (from ae in dbSIM.TIPO_PERSONAL_DGA
                                             select new
                                             {
                                                 ID_TIPOPERSONAL = ae.ID_TIPOPERSONAL,
                                                 S_NOMBRE_LOOKUP = ae.S_NOMBRE,
                                                 //S_NOMBRE_LOOKUP = string.Concat(10, ae.S_NOMBRE, " (", ae.S_CODIGO, ")"),
                                             });

                                modelData = model;
                            }
                            else
                            {
                                var model = (from ae in dbSIM.TIPO_PERSONAL_DGA
                                             select new
                                             {
                                                 ae.ID_TIPOPERSONAL,
                                                 S_NOMBRE_LOOKUP = ae.S_NOMBRE,
                                             });

                            

                                modelData = model;
                            }
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