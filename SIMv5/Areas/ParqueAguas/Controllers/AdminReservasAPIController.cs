namespace SIM.Areas.ParqueAguas.Controllers
{
    using System.Data.Entity;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using SIM.Data;
    using System.Linq.Dynamic;

    [Route("api/[controller]", Name = "AdminReservasAPI")]
    public class AdminReservasAPIController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Retorna el Listado de las Series Documentales
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usaurio</param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetReservas")]
        public datosConsulta GetReservas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TPARESE_RESERVAs.OrderBy(f => f.D_RESERVA);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }


        [HttpGet, ActionName("CancelarReserva")]
        public System.Web.Http.Results.JsonResult<string> CancelarReserva(int Id)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            int estado = 0;
            try
            {
         
                var reserva = this.dbSIM.TPARESE_RESERVAs.Where(f => f.ID == Id).FirstOrDefault();
                if (reserva != null)
                {
                    reserva.B_CANCELADA = "1";
                    this.dbSIM.SaveChanges();
                    estado = 1;
                }
             
                return this.Json(estado.ToString());
            }
            catch (Exception exp)
            {
                return this.Json(exp.Message);
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
}
