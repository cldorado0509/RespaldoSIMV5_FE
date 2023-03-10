namespace SIM.Areas.Dynamics.Controllers
{
    using SIM.Areas.Dynamics.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    public class EtiquetaApiController : ApiController
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
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
        [HttpGet]
        [ActionName("ConsultaBienes")]
        public datosConsulta GetBienes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, bool noFilterNoRecords)
        {
            datosConsulta resultado = new datosConsulta();
            dynamic modelData;
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from eti in dbDynamics.AssetTable
                             join bo in dbDynamics.AssetBook on eti.AssetId equals bo.AssetId into books
                             from beti in books.DefaultIfEmpty()
                             join worAsset in dbDynamics.HcmWorker on eti.WorkerResponsible equals worAsset.RECID into workers
                             from trabajador in workers.DefaultIfEmpty()
                             join worTrabaja in dbDynamics.DirPersonName on trabajador.Person equals worTrabaja.Person into personas
                             from persona in personas.DefaultIfEmpty()
                             select new BienModel
                             {
                                 Codigo = eti.AssetId,
                                 NombreBien = eti.Name,
                                 EstadoBien = beti.Status == 0 ? "NO ADQUIRIDO" :
                                              beti.Status == 1 ? "ABIERTO" :
                                              beti.Status == 2 ? "SUSPENDIDO" :
                                              beti.Status == 3 ? "CERRADO" :
                                              beti.Status == 4 ? "VENDIDO" :
                                              beti.Status == 5 ? "DADO DE BAJA" :
                                              beti.Status == 6 ? "TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO" : "OTRO",
                                 PersonaBien = (persona.FirstName + ' ' + persona.MiddleName + ' ' + persona.LastName + ' ' + persona.AP_CO_SecondLastName)
                             }
                          );
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;

            }

        }

    }
}
