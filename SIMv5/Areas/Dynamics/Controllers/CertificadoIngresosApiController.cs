﻿using Newtonsoft.Json;
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
    public class CertificadoIngresosApiController : ApiController
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customFilters"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaCertificados")]
        public JArray GetCertificados(string customFilters)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            // if (customFilters == "" || customFilters == null) return null;
            List<ListadoTercero> listadoTercero = new List<ListadoTercero>();
            if (customFilters != "" && customFilters != null)
            {
                string[] _Buscar = customFilters.Split(':');
                string _Sql = "";
                if (_Buscar.Length > 0)
                {
                    string[] BuscarF;
                    var Agno = 0;
                    var Tercero = "";
                    switch (_Buscar[0])
                    {
                        case "F":
                            BuscarF = _Buscar[1].Split(';');
                            Agno = int.Parse(BuscarF[1]);
                            Tercero = BuscarF[0];
                            break;
                        case "C":
                            BuscarF = _Buscar[1].Split(';');
                            Agno = int.Parse(BuscarF[1]);
                            Tercero = BuscarF[0];
                            break;
                        case "A":
                            BuscarF = _Buscar[1].Split(';');
                            Agno = int.Parse(BuscarF[0]);
                            Tercero = "";
                            break;
                    }
                    if (Tercero.Length > 0)
                    {
                        _Sql = $"SELECT DISTINCT * FROM (SELECT V.ACCOUNTNUM AS TERCERO, DT.NAME FROM VENDTABLE V  INNER JOIN DIRPARTYTABLE DT ON V.PARTY=DT.RECID LEFT JOIN DIRPERSONNAME P ON DT.RECID = P.PERSON) QRY WHERE TERCERO ='{Tercero}'";
                        var Persona = dbDynamics.Database.SqlQuery<TercerosModel>(_Sql).FirstOrDefault();
                        if (Persona != null)
                        {
                            //_Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Tercero}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(D.MAINACCOUNTVALUE, SUBSTRING(D2.DISPLAYVALUE,1,6)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5') AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Tercero}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,6))) QRY GROUP BY AGNO, TERCERO, CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                            _Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Tercero}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6), SUBSTRING(D2.DISPLAYVALUE,1,6)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1), SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5') AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Tercero}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6),SUBSTRING(D2.DISPLAYVALUE,1,6))) QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                            var PoseeCert = dbDynamics.Database.SqlQuery<DatosCertificado>(_Sql).ToList();
                            if (PoseeCert.Count > 0)
                            {
                                ListadoTercero _ter = new ListadoTercero();
                                _ter.Tercero = Persona.Tercero;
                                _ter.Name = Persona.Name;
                                _ter.Agno = Agno;
                                listadoTercero.Add(_ter);
                            }
                        }
                    }
                    else
                    {
                        //_Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(D.MAINACCOUNTVALUE, SUBSTRING(D2.DISPLAYVALUE,1,6)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5') GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,6))) QRY GROUP BY AGNO, TERCERO, CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        _Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6), SUBSTRING(D2.DISPLAYVALUE,1,6)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1), SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5') GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6),SUBSTRING(D2.DISPLAYVALUE,1,6))) QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        var ListDatCert = dbDynamics.Database.SqlQuery<DatosCertificado>(_Sql).ToList();
                        if (ListDatCert.Count > 0)
                        {
                            string nit = "";
                            foreach (var dato in ListDatCert)
                            {
                                if (dato.TERCERO != nit)
                                {
                                    _Sql = $"SELECT DISTINCT * FROM (SELECT V.ACCOUNTNUM AS TERCERO, DT.NAME FROM VENDTABLE V  INNER JOIN DIRPARTYTABLE DT ON V.PARTY=DT.RECID LEFT JOIN DIRPERSONNAME P ON DT.RECID = P.PERSON) QRY WHERE TERCERO ='{dato.TERCERO}'";
                                    var Persona = dbDynamics.Database.SqlQuery<TercerosModel>(_Sql).FirstOrDefault();
                                    if (Persona != null)
                                    {
                                        ListadoTercero _ter = new ListadoTercero();
                                        _ter.Tercero = dato.TERCERO;
                                        _ter.Name = Persona.Name;
                                        _ter.Agno = Agno;
                                        listadoTercero.Add(_ter);
                                    }
                                }
                                nit = dato.TERCERO;
                            }
                        }
                    }
                }
            }
            return JArray.FromObject(listadoTercero, Js);
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
                string Sql = "SELECT DISTINCT * FROM (SELECT V.ACCOUNTNUM AS TERCERO, DT.NAME FROM VENDTABLE V  INNER JOIN DIRPARTYTABLE DT ON V.PARTY=DT.RECID LEFT JOIN DIRPERSONNAME P ON DT.RECID = P.PERSON) QRY ORDER BY NAME";
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