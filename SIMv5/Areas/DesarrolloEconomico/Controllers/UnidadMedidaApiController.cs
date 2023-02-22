namespace SIM.Areas.DesarrolloEconomico.Controllers
{
    using System.Data.Entity;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SIM.Areas.DesarrolloEconomico.Models;
    using SIM.Areas.Models;
    using SIM.Data;
    using SIM.Data.DesarrolloEconomico;

    [Route("api/[controller]", Name = "UnidadMedidaApi")]
    public class UnidadMedidaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //Utilidades.Tramites Tramites = new Utilidades.Tramites();

        /// <summary>
        /// Consulta de Lista de Unidades de Medida
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
        [HttpGet, ActionName("GetUnidadesMedida")]
        public datosConsulta GetUnidadesMedida(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.DDECONO_UNIDAD_MEDIDA.OrderBy(f => f.S_NOMBRE);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpPost,ActionName("GuardarUnidad")]
        public object GuardarUnidad(UnidadMedida objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Estado? "1" : "0";
                if (Id > 0)
                {
                    var UnidadMed = dbSIM.DDECONO_UNIDAD_MEDIDA.Where(f => f.ID == Id).FirstOrDefault();
                    if (UnidadMed != null)
                    {
                        UnidadMed.S_NOMBRE = objData.Nombre;
                        UnidadMed.S_DESCRIPCION = objData.Descripcion;
                        UnidadMed.B_ACTIVO = objData.Estado == true ? "1" : "0";
                        dbSIM.SaveChanges();
                     }
                }
                else if (Id <= 0)
                {
                    DDECONO_UNIDAD_MEDIDA UnidadMed = new DDECONO_UNIDAD_MEDIDA { 
                        S_NOMBRE = objData.Nombre,
                        S_DESCRIPCION = objData.Descripcion,
                        B_ACTIVO = objData.Estado == true ? "1" : "0",
                    };
                    dbSIM.DDECONO_UNIDAD_MEDIDA.Add(UnidadMed);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Unidad de Medida: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Unidad de Medida almacenada correctamente" };
        }

        [HttpGet,ActionName("ObtenerUnidad")]
        public JObject ObtenerUnidad(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var unidadMed = this.dbSIM.DDECONO_UNIDAD_MEDIDA.Where(f => f.ID == _Id).FirstOrDefault(); 
                
                UnidadMedida unidadMedida = new UnidadMedida();
                unidadMedida.Id= unidadMed.ID;
                unidadMedida.Nombre = unidadMed.S_NOMBRE;
                unidadMedida.Descripcion = unidadMed.S_DESCRIPCION;
                unidadMedida.Estado = unidadMed.B_ACTIVO == "1"? true: false;
                return JObject.FromObject(unidadMedida, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("EliminarUnidad")]
        public object EliminarUnidad(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la Unidad de Medida" };
            try
            {
                int Id = -1;
                if (objData != null && objData != "") Id = int.Parse(objData);
                if (Id > 0)
                {
                    var unidadMed = this.dbSIM.DDECONO_UNIDAD_MEDIDA.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.DDECONO_UNIDAD_MEDIDA.Remove(unidadMed);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Unidad de Medida eliminada correctamente!!" };
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
}
