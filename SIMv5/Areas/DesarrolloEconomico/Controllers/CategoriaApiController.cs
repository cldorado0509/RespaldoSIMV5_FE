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
    using SIM.Data.DesarrolloEconomico;
    using SIM.Data;

    [Route("api/[controller]", Name = "CategoriaApi")]
    public class CategoriaApiController : ApiController
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
        [HttpGet, ActionName("GetCategorias")]
        public datosConsulta GetCategorias(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.DDECONO_CATEGORIA.OrderBy(f => f.S_NOMBRE);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpPost, ActionName("GetCategorias")]
        public object GetCategorias(Categoria objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Estado ? "1" : "0";
                if (Id > 0)
                {
                    var Categoria = dbSIM.DDECONO_CATEGORIA.Where(f => f.ID == Id).FirstOrDefault();
                    if (Categoria != null)
                    {
                        Categoria.S_NOMBRE = objData.Nombre;
                        Categoria.S_DESCRIPCION = objData.Descripcion;
                        Categoria.B_ACTIVO = objData.Estado == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    DDECONO_CATEGORIA Categoria = new DDECONO_CATEGORIA
                    {
                        S_NOMBRE = objData.Nombre,
                        S_DESCRIPCION = objData.Descripcion,
                        B_ACTIVO = objData.Estado == true ? "1" : "0",
                    };
                    dbSIM.DDECONO_CATEGORIA.Add(Categoria);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría alamacenada correctamente" };
        }

        [HttpGet, ActionName("ObtenerCategoria")]
        public JObject ObtenerCategoria(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                decimal _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = decimal.Parse(Id);

                var categoria = this.dbSIM.DDECONO_CATEGORIA.Where(f => f.ID == _Id).FirstOrDefault();

                Categoria Categoria = new Categoria();
                Categoria.Id = categoria.ID;
                Categoria.Nombre = categoria.S_NOMBRE;
                Categoria.Descripcion = categoria.S_DESCRIPCION;
                Categoria.Estado = categoria.B_ACTIVO == "1" ? true : false;
                return JObject.FromObject(Categoria, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpPost, ActionName("GuardarCategoria")]
        public object GuardarCategoria(Categoria objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Estado ? "1" : "0";
                if (Id > 0)
                {
                    var _Categoria = dbSIM.DDECONO_CATEGORIA.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Categoria != null)
                    {
                        _Categoria.S_NOMBRE = objData.Nombre;
                        _Categoria.S_DESCRIPCION = objData.Descripcion;
                        _Categoria.B_ACTIVO = objData.Estado == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    DDECONO_CATEGORIA _Categoria = new DDECONO_CATEGORIA
                    {
                        S_NOMBRE = objData.Nombre,
                        S_DESCRIPCION = objData.Descripcion,
                        B_ACTIVO = objData.Estado == true ? "1" : "0",
                    };
                    dbSIM.DDECONO_CATEGORIA.Add(_Categoria);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría almacenada correctamente" };
        }

        [HttpGet, ActionName("EliminarCategoria")]
        public object EliminarCategoria(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la Categoría" };
            try
            {
                int Id = -1;
                if (objData != null && objData != "") Id = int.Parse(objData);
                if (Id > 0)
                {
                    var categoria = this.dbSIM.DDECONO_CATEGORIA.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.DDECONO_CATEGORIA.Remove(categoria);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría eliminada correctamente!!" };
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
