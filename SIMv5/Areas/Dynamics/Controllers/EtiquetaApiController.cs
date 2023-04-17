namespace SIM.Areas.Dynamics.Controllers
{
    using DevExpress.CodeParser;
    using DevExpress.Utils.Win.Hook;
    using DocumentFormat.OpenXml.Drawing.Charts;
    using DocumentFormat.OpenXml.EMMA;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.Dynamics.Data;
    using SIM.Controllers;
    using SIM.Data.General;
    using System;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaBienes")]
        public datosConsulta GetBienes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string customFilters)
        {
            datosConsulta resultado = new datosConsulta();
            dynamic modelData;
            if (customFilters == "" || customFilters == null)
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
                                _Sql = "SELECT * FROM (SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE,AT.LOCATIONMEMO UBICACION,AT.BARCODE AS SICOF,DPN.PERSON FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY WHERE QRY.PERSON = " + _Buscar[1].ToUpper();
                                var RespBienes = dbDynamics.Database.SqlQuery<BienModel>(_Sql);
                                //long CodFun = long.Parse(_Buscar[1]);
                                //var RespBienes = (from AT in dbDynamics.AssetTable
                                //                  join HW in dbDynamics.HcmWorker on AT.WorkerResponsible equals HW.RECID into TraG
                                //                  from trab in TraG.DefaultIfEmpty()
                                //                  join DPN in dbDynamics.DirPersonName on trab.Person equals DPN.Person into PerG
                                //                  from per in PerG.DefaultIfEmpty()
                                //                  join AB in dbDynamics.AssetBook on AT.AssetId equals AB.AssetId into BookG
                                //                  from book in BookG.DefaultIfEmpty()
                                //                  where per.Person == CodFun
                                //                  select new BienModel
                                //                  {                      
                                //                      ASSETID = AT.AssetId,
                                //                      SICOF = AT.Barcode,
                                //                      ASSETNAME = AT.Name,
                                //                      ESTADO = book.Status == 0 ? "NO ADQUIRIDO" :
                                //                               book.Status == 1 ? "ABIERTO" :
                                //                               book.Status == 2 ? "SUSPENDIDO" :
                                //                               book.Status == 3 ? "CERRADO" :
                                //                               book.Status == 4 ? "VENDIDO" :
                                //                               book.Status == 5 ? "DADO DE BAJA" :
                                //                               book.Status == 6 ? "TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO" : "OTRO",
                                //                      RESPONSIBLE = (per.FirstName + " " + per.MiddleName + " " + per.LastName + " " + per.AP_CO_SecondLastName),
                                //                      UBICACION = AT.LocationMemo
                                //                  }
                                //                  );
                                //modelData = RespBienes;
                                //IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                                resultado.numRegistros = RespBienes.Count();
                                if (skip > 0 || take > 0) resultado.datos = RespBienes.Skip(skip).Take(take).ToList();
                                else resultado.datos = RespBienes.ToList();
                                break;
                            case "C":
                                _Sql = "SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE,AT.LOCATIONMEMO UBICACION,AT.BARCODE AS SICOF FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON WHERE AT.ASSETID LIKE '%" + _Buscar[1].ToUpper() + "%' ";
                                var CodBien = dbDynamics.Database.SqlQuery<BienModel>(_Sql);
                                if (skip > 0 || take > 0) resultado.datos = CodBien.Skip(skip).Take(take).ToList();
                                else resultado.datos = CodBien.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                            case "P":
                                string[] valores = _Buscar[1].Split(';');
                                _Sql = "SELECT ASSETID,ASSETNAME,ESTADO,RESPONSIBLE FROM (SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE, SUBSTRING(AT.ASSETID, PATINDEX('%[0-9]%', AT.ASSETID), PATINDEX('%[0-9][^0-9]%', AT.ASSETID + 't') - PATINDEX('%[0-9]%', AT.ASSETID) + 1) AS Number,AT.LOCATIONMEMO UBICACION,AT.BARCODE AS SICOF FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON WHERE AT.ASSETID LIKE '" + valores[0].Trim() + "%') QRY WHERE CAST(QRY.Number AS INT) BETWEEN " + int.Parse(valores[1].Trim()) + " AND " + int.Parse(valores[2].Trim());
                                var CodRan = dbDynamics.Database.SqlQuery<BienModel>(_Sql);
                                if (skip > 0 || take > 0) resultado.datos = CodRan.Skip(skip).Take(take).ToList();
                                else resultado.datos = CodRan.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                        }
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
                else
                {
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    return resultado;
                }
                return resultado;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Responsables")]
        public JArray GetResponsables()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                // string Sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS ORDEN,RESPONSIBLE FROM (SELECT  AT.ASSETID,(DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE FROM  ASSETTABLE AS AT LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY GROUP BY QRY.RESPONSIBLE ORDER BY QRY.RESPONSIBLE";
                string Sql = "SELECT isnull(ID,0) AS ORDEN,RESPONSIBLE FROM (SELECT AT.ASSETID,DPN.PERSON AS ID,(DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE FROM  ASSETTABLE AS AT LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY GROUP BY QRY.ID,QRY.RESPONSIBLE ORDER BY QRY.RESPONSIBLE";
                var RespBienes = dbDynamics.Database.SqlQuery<ResponsableModel>(Sql);
                return JArray.FromObject(RespBienes, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Prefijos")]
        public JArray GetPrefijos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string Sql = "SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS ORDEN,PREFIJO FROM (SELECT  SUBSTRING(AT.ASSETID, 0,CHARINDEX('-', AT.ASSETID)) AS PREFIJO FROM  ASSETTABLE AS AT) QRY GROUP BY QRY.PREFIJO ORDER BY QRY.PREFIJO";
                var PrefijoBienes = dbDynamics.Database.SqlQuery<PrefijoModel>(Sql);
                return JArray.FromObject(PrefijoBienes, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
