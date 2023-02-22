

namespace SIM.Areas.Retributivas.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using SIM.Data.Agua;
    using SIM.Data;


    public class ReportesApiControllerOld : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }


        /// <summary>
        /// TRIBUTARY
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Tributary")]
        public datosConsulta Tributary(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }

            else
            {

                modelData = (from trn in dbSIM.TSIMTASA_CUENCAS
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.NOMBRE,
                                 trn.AREA,
                                 trn.CAUDAL,
                                 trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 trn.LONGITUD,
                                 trn.ID_MUNICIPIO
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveTributary")]
        public object GetRemoveTributary(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_CUENCAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_CUENCAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }



        [HttpGet, ActionName("loadTributary")]
        public JObject loadTurn(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_CUENCAS
                             where trn.ID == Id
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.NOMBRE,
                                 trn.AREA,
                                 trn.CAUDAL,
                                 trn.LONGITUD,
                                 trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 trn.ID_MUNICIPIO

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [System.Web.Http.ActionName("InsertTributary")]
        [HttpPost]
        public object InsertTurn(TSIMTASA_CUENCAS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_CUENCAS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.NOMBRE = objData.NOMBRE;
                            _turnUpdate.AREA = objData.AREA;
                            _turnUpdate.CAUDAL = objData.CAUDAL;
                            _turnUpdate.LONGITUD = objData.LONGITUD;
                            _turnUpdate.TSIMTASA_TIPO_CUENCAS_ID = objData.TSIMTASA_TIPO_CUENCAS_ID;         
                            _turnUpdate.ID_MUNICIPIO = objData.ID_MUNICIPIO;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_CUENCAS _newTurn = new TSIMTASA_CUENCAS
                        {
                            NOMBRE = objData.NOMBRE,
                            AREA = objData.AREA,
                            CAUDAL = objData.CAUDAL,
                            LONGITUD = objData.LONGITUD,
                            TSIMTASA_TIPO_CUENCAS_ID = objData.TSIMTASA_TIPO_CUENCAS_ID,
                            ID_MUNICIPIO = objData.ID_MUNICIPIO
                        };
                        dbSIM.TSIMTASA_CUENCAS.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }



    }
}
