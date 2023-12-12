using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Dynamics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SIM.Areas.Dynamics.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PazsalvoBienesApiController : ApiController
    {

        DynamicsContext dbDynamics = new DynamicsContext();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tercero"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaBienes")]
        public JArray GetConsultaBienes(string Tercero)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var ListaBienes = new List<BienModel>();
            if (Tercero != "" && Tercero != null)
            {
                if (Tercero.Length > 0)
                {
                    string _Sql = $"SELECT ASSETID,ASSETNAME,ESTADO,RESPONSIBLE,UBICACION,FECHAINV FROM (SELECT AT.ASSETID,AT.NAME AS ASSETNAME,(CASE WHEN AB.STATUS = 0 THEN 'NO ADQUIRIDO' WHEN AB.STATUS = 1 THEN 'ABIERTO' WHEN AB.STATUS = 2 THEN 'SUSPENDIDO' WHEN AB.STATUS = 3 THEN 'CERRADO' WHEN AB.STATUS = 4 THEN 'VENDIDO' WHEN AB.STATUS = 5 THEN 'DADO DE BAJA' WHEN AB.STATUS = 6 THEN 'TRANSFERIDO A ACTIVOS DE VALOR DEDUCIDO' ELSE 'OTRO' END) AS ESTADO, (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS RESPONSIBLE,AT.LOCATIONMEMO UBICACION,CASE WHEN YEAR(AT.PhysicalInventory) > 1900 THEN AT.PhysicalInventory ELSE NULL END AS FECHAINV,HW.PersonnelNumber AS CEDULA FROM ASSETTABLE AS AT LEFT OUTER JOIN ASSETBOOK AS AB ON AT.ASSETID = AB.ASSETID LEFT OUTER JOIN HCMWORKER AS HW ON AT.WORKERRESPONSIBLE = HW.RECID LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON) QRY WHERE QRY.CEDULA = '{Tercero}'";
                    var Bienes = dbDynamics.Database.SqlQuery<BienModel>(_Sql).ToList();
                    ListaBienes = Bienes;
                }
            }
            return JArray.FromObject(ListaBienes, Js);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Terceros")]
        public JArray GetTerceros()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string Sql = "SELECT DISTINCT (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS Name,HW.PersonnelNumber AS Tercero FROM HCMWORKER AS HW LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON ORDER BY Name";
                var RespBienes = dbDynamics.Database.SqlQuery<TercerosModel>(Sql);
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
        /// <param name="Documento"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ExisteTercero")]
        public object GetExisteTercero(string Documento)
        {
            try
            {
                if (Documento.Length > 0)
                {
                    string Sql = $"SELECT COUNT(*) FROM HCMWORKER AS HW LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON WHERE HW.PersonnelNumber='{Documento}'";
                    var Existe = dbDynamics.Database.SqlQuery<int>(Sql).First();
                    if (Existe <= 0) return new { resp = "Error", mensaje = "El documento ingresado no se encontró en la base de datos!" };
                    else return new { resp = "Ok", mensaje = "" };
                }
                else return new { resp = "Error", mensaje = "No se ingreso un documento para buscar" };
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
