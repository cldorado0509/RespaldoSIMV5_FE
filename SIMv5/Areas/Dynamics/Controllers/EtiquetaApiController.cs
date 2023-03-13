namespace SIM.Areas.Dynamics.Controllers
{
    using DevExpress.CodeParser;
    using DocumentFormat.OpenXml.Drawing.Charts;
    using SIM.Areas.Dynamics.Data;
    using SIM.Controllers;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        /// <param name="customFilters"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaBienes")]
        public datosConsulta GetBienes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string customFilters, bool noFilterNoRecords)
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
                //var model;
                if (customFilters != "")
                {
                        string[] _Buscar = customFilters.Split(':');
                        string _Sql = "";
                    if (_Buscar.Length > 0)
                    {
                        switch (_Buscar[0])
                        {
                            case "R":
                                _Sql = "SELECT * FROM (SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + DPN.MIDDLENAME + ' ' + DPN.LASTNAME + ' ' + DPN.AP_CO_SECONDLASTNAME) AS RESPONSIBLE FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY WHERE QRY.RESPONSIBLE LIKE '%" + _Buscar[1].ToUpper()  + "%' ";
                                var RespBienes = dbDynamics.Database.SqlQuery<BienModel>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = RespBienes.Skip(skip).Take(take).ToList();
                                else resultado.datos = RespBienes.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                            case "C":
                                _Sql = "SELECT * FROM (SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + DPN.MIDDLENAME + ' ' + DPN.LASTNAME + ' ' + DPN.AP_CO_SECONDLASTNAME) AS RESPONSIBLE FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY WHERE QRY.RESPONSIBLE LIKE '%" + _Buscar[1].ToUpper() + "%' ";
                                var CodBien = dbDynamics.Database.SqlQuery<BienModel>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = CodBien.Skip(skip).Take(take).ToList();
                                else resultado.datos = CodBien.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;


                    }



                    //model = (from eti in dbDynamics.AssetTable
                    //             join bo in dbDynamics.AssetBook on eti.AssetId equals bo.AssetId into books
                    //             from beti in books.DefaultIfEmpty()
                    //             join worAsset in dbDynamics.HcmWorker on eti.WorkerResponsible equals worAsset.RECID into workers
                    //             from trabajador in workers.DefaultIfEmpty()
                    //             join worTrabaja in dbDynamics.DirPersonName on trabajador.Person equals worTrabaja.Person into personas
                    //             from persona in personas.DefaultIfEmpty()
                    //             select new BienModel
                    //             {
                    //                 Codigo = eti.AssetId,
                    //                 NombreBien = eti.Name,
                    //                 EstadoBien = beti.Status == 0 ? "NO ADQUIRIDO" :
                    //                              beti.Status == 1 ? "ABIERTO" :
                    //                              beti.Status == 2 ? "SUSPENDIDO" :
                    //                              beti.Status == 3 ? "CERRADO" :
                    //                              beti.Status == 4 ? "VENDIDO" :
                    //                              beti.Status == 5 ? "DADO DE BAJA" :
                    //                              beti.Status == 6 ? "TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO" : "OTRO",
                    //                 PersonaBien = (persona.FirstName + ' ' + persona.MiddleName + ' ' + persona.LastName + ' ' + persona.AP_CO_SecondLastName)
                    //             }
                    //          );
                }
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;

            }

        }

    }
}
