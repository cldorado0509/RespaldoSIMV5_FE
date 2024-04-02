using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Dynamics.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SIM.Areas.Dynamics.Controllers
{
    public class FacturasApiController : ApiController
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        [HttpGet]
        [ActionName("ObtenerFacturas")]
        public JArray GetFacturas(string customFilters)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<ListadoFacturas> listadoFacturas = new List<ListadoFacturas>();
            if (customFilters != "" && customFilters != null)
            {
                string[] _Buscar = customFilters.Split(';');
                string _Sql = "SELECT CIT.INVOICEID AS FACTURA,CIT.INVOICEDATE AS FECHAFACTURA,CIT.INVOICEACCOUNT AS DOCUMENTO,DPT.NAME AS TERCERO,COUNTY.NAME AS MUNICIPIO, LEA_PCE.LOCATOR AS EMAIL FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT=CT.ACCOUNTNUM INNER JOIN DIRPARTYTABLE AS DPT ON CT.PARTY=DPT.RECID LEFT OUTER JOIN LOGISTICSPOSTALADDRESS AS LPA ON CIT.POSTALADDRESS=LPA.RECID LEFT OUTER JOIN LOGISTICSELECTRONICADDRESS AS LEA_PCE ON DPT.PRIMARYCONTACTEMAIL=LEA_PCE.RECID LEFT OUTER JOIN LOGISTICSADDRESSCOUNTY AS COUNTY ON COUNTY.COUNTRYREGIONID=LPA.COUNTRYREGIONID AND COUNTY.STATEID=LPA.STATE AND COUNTY.COUNTYID=LPA.COUNTY ";
                string _Where = " WHERE (CIT.POSTED = 1)";
                string _WhereAux = "";
                string _Order = " ORDER BY FECHAFACTURA DESC";
                foreach (string _buscar in _Buscar)
                {
                    string[] _filtro = _buscar.Split(':');
                    if (_filtro.Length > 0)
                    {
                        switch (_filtro[0])
                        {
                            case "D":
                                _WhereAux += $" OR CIT.INVOICEACCOUNT='{_filtro[1]}'";
                                break;
                            case "T":
                                _WhereAux += $" OR DPT.NAME='{_filtro[1]}'";
                                break;
                            case "B":
                                string[] _Facturas = _filtro[1].Split(',');
                                if (_Facturas.Length > 0)
                                {
                                    _WhereAux += $" OR  CIT.INVOICEID IN ('";
                                    foreach (string _factura in _Facturas)
                                    {
                                        _WhereAux += "'" + _factura + "',";
                                    }
                                    _WhereAux = _WhereAux.Substring(0, _WhereAux.Length - 1) + ")";
                                }
                                break;
                            case "F":
                                if (_filtro[1].Contains(","))
                                {
                                    string[] _Fechas = _filtro[1].Split(',');
                                    DateTime _dtAuxDesde;
                                    DateTime _dtAuxHasta;
                                    if (DateTime.TryParse(_Fechas[0], out _dtAuxDesde) && DateTime.TryParse(_Fechas[0], out _dtAuxHasta))
                                        _WhereAux += $"CIT.INVOICEDATE BETWEEN '{_dtAuxDesde:dd-MM-yyyy}' AND '{_dtAuxHasta:dd-MM-yyyy}'";
                                }
                                else
                                {
                                    DateTime _dtAux;
                                    if (DateTime.TryParse(_filtro[1], out _dtAux))
                                        _WhereAux += $" CIT.INVOICEDATE='{_dtAux:dd-MM-yyyy}'";
                                }
                                break;
                        }
                    }
                }
                if (_WhereAux.Length > 0) _Where += $" AND ({_WhereAux.Substring(3)})";
                _Sql += $"{_Where} {_Order}";
                var Facturas = dbDynamics.Database.SqlQuery<ListadoFacturas>(_Sql).ToList();
                listadoFacturas = Facturas;
            }
            return JArray.FromObject(listadoFacturas, Js);
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
                string Sql = "SELECT DISTINCT  RTRIM(LTRIM(DPT.NAME)) AS Name, CIT.INVOICEACCOUNT AS Tercero FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT = CT.ACCOUNTNUM INNER JOIN DIRPARTYTABLE AS DPT ON CT.PARTY = DPT.RECID WHERE (CIT.POSTED = 1) ORDER BY Name";
                var RespBienes = dbDynamics.Database.SqlQuery<TercerosModel>(Sql);
                return JArray.FromObject(RespBienes, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
