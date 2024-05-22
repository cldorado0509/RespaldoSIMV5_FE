using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using SIM.Areas.Dynamics.Data;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SIM.Areas.Dynamics.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CertificadoIngresosController : Controller
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        /// <summary>
        /// GET: Dynamics/CertificadoIngresos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTer"></param>
        /// <param name="Agno"></param>
        /// <returns></returns>
        [ActionName("ImprimirCertificado")]
        public FileContentResult ImprimirCertificado(string IdTer, int Agno)
        {
            MemoryStream _DocSalida = new MemoryStream();
            if (IdTer != "" && Agno > 2020)
            {
                if (Agno >= 2023) _DocSalida = Certificado2023(IdTer, Agno);
                else _DocSalida = Certificado2022(IdTer, Agno);
            }
            _DocSalida.Position = 0;
            var Archivo = _DocSalida.ToArray();
            return File(Archivo, "application/pdf");
        }

        private MemoryStream Certificado2022(string IdTer, int Agno)
        {
            string _Sql = "";
            MemoryStream _DocSalida = new MemoryStream();
            try
            {
                PDFDocument _Doc = new PDFDocument();
                _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                DatosTerceroModel _DatosCert = new DatosTerceroModel();
                string _Formato = Server.MapPath($"~/Areas/Dynamics/Plantilla/Formulario_220_{Agno}.pdf");
                if (System.IO.File.Exists(_Formato))
                {
                    MemoryStream _Plantilla = new MemoryStream(System.IO.File.ReadAllBytes(_Formato));
                    _Doc = new PDFDocument(_Plantilla);
                    _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                    TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 9, true, true);
                    PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                    _Arial.Bold = false;
                    PDFTextFormatOptions tfoRight = new PDFTextFormatOptions();
                    tfoRight.Align = PDFTextAlign.MiddleRight;
                    tfoRight.ClipText = PDFClipText.ClipNone;
                    PDFTextFormatOptions tfoLeft = new PDFTextFormatOptions();
                    tfoLeft.Align = PDFTextAlign.MiddleLeft;
                    tfoLeft.ClipText = PDFClipText.ClipNone;
                    PDFTextFormatOptions tfoCenter = new PDFTextFormatOptions();
                    tfoCenter.Align = PDFTextAlign.MiddleCenter;
                    tfoCenter.ClipText = PDFClipText.ClipNone;
                    string _mesAgno = DateTime.Now.ToString("MMMM DE yyyy", CultureInfo.GetCultureInfo("es-CO")).ToUpper();
                    _Doc.Pages[0].Canvas.DrawTextBox("890984423  3", _Arial, null, brush, 15, 88, 180, 30, tfoRight);
                    _Doc.Pages[0].Canvas.DrawTextBox("AREA METROPOLITANA DEL VALLE DE ABURRA", _Arial, null, brush, 40, 112, 300, 30, tfoLeft);
                    _Sql = $"SELECT TOP(1) * FROM (SELECT V.ACCOUNTNUM AS TERCERO,DT.NAME AS NOMBRE,P.FIRSTNAME,P.MIDDLENAME,P.LASTNAME,P.AP_CO_SECONDLASTNAME,P.RECID FROM VENDTABLE V INNER JOIN DIRPARTYTABLE DT ON V.PARTY=DT.RECID LEFT JOIN DIRPERSONNAME P ON DT.RECID=P.PERSON) QRY WHERE TERCERO ='{IdTer}' ORDER BY RECID DESC";
                    var Persona = dbDynamics.Database.SqlQuery<DatosTerceroModel>(_Sql).FirstOrDefault();
                    if (Persona != null)
                    {
                        //_Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(D.MAINACCOUNTVALUE, SUBSTRING(D2.DISPLAYVALUE,1,6))                AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5')                 AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,6)))                QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        _Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6), SUBSTRING(D2.DISPLAYVALUE,1,6)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1), SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5') AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,6),SUBSTRING(D2.DISPLAYVALUE,1,6))) QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        var DatosCert = dbDynamics.Database.SqlQuery<DatosCertificado>(_Sql).ToList();
                        if (DatosCert != null)
                        {
                            foreach (var _cert in DatosCert)
                            {
                                if (_cert.OPERACION == "RETENCION") Persona.VALORRETENCION = _cert.VALOR;
                                if (_cert.OPERACION == "INGRESO") Persona.VALORINGRESO = _cert.VALOR;
                                Persona.AGNO = Agno;
                            }
                            if (Persona.NOMBRE != "" && Persona.FIRSTNAME == null)
                            {
                                string[] _arrayNombre = Persona.NOMBRE.Split(' ');
                                switch (_arrayNombre.Length)
                                {
                                    case 1:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = "";
                                        Persona.LASTNAME = "";
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 2:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = "";
                                        Persona.LASTNAME = _arrayNombre[1];
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 3:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = _arrayNombre[1];
                                        Persona.LASTNAME = _arrayNombre[2];
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 4:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = _arrayNombre[1];
                                        Persona.LASTNAME = _arrayNombre[2];
                                        Persona.AP_CO_SECONDLASTNAME = _arrayNombre[3];
                                        break;
                                }
                            }
                            _Doc.Pages[0].Canvas.DrawText("C.C.", _Arial, null, brush, 40, 148);
                            _Doc.Pages[0].Canvas.DrawText(Persona.TERCERO, _Arial, null, brush, 120, 148);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.LASTNAME == null ? "" : Persona.LASTNAME, _Arial, null, brush, 230, 136, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.AP_CO_SECONDLASTNAME == null ? "" : Persona.AP_CO_SECONDLASTNAME, _Arial, null, brush, 320, 136, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.FIRSTNAME == null ? "" : Persona.FIRSTNAME, _Arial, null, brush, 410, 136, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.MIDDLENAME == null ? "" : Persona.MIDDLENAME, _Arial, null, brush, 500, 136, 90, 30, tfoCenter);
                            DateTime firstDay = new DateTime(Agno, 1, 1);
                            DateTime lastDay = new DateTime(Agno, 12, 31);
                            _Doc.Pages[0].Canvas.DrawText(firstDay.ToString("yyyy     MM    dd"), _Arial, null, brush, 60, 172);
                            _Doc.Pages[0].Canvas.DrawText(lastDay.ToString("yyyy     MM    dd"), _Arial, null, brush, 170, 172);
                            _Doc.Pages[0].Canvas.DrawText(DateTime.Now.ToString("yyyy     MM    dd"), _Arial, null, brush, 260, 172);
                            _Doc.Pages[0].Canvas.DrawText("Medellín", _Arial, null, brush, 340, 172);
                            _Doc.Pages[0].Canvas.DrawText("05", _Arial, null, brush, 522, 172);
                            _Doc.Pages[0].Canvas.DrawText("05001", _Arial, null, brush, 550, 172);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 195, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 207, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORINGRESO.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 218, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 231, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 243, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 255, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 268, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 280, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 292, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 304, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 316, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 328, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 340, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORINGRESO.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 352, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 376, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 388, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 400, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 410, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 421, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORRETENCION.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 434, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("AREA METROPOLITANA DEL VALLE DE ABURRA", _Arial, null, brush, 160, 434, 300, 30, tfoLeft);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 482, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 494, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 506, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 518, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 530, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 542, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 554, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 482, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 494, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 506, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 518, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 530, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 542, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 554, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORRETENCION.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 566, 90, 10, tfoRight);
                        }
                    }
                }
                _Doc.Save(_DocSalida);
            }
            catch
            {
                return null;
            }
            return _DocSalida;
        }

        private MemoryStream Certificado2023(string IdTer, int Agno)
        {
            var _DocSalida = new MemoryStream();
            string _Sql = "";
            try
            {
                PDFDocument _Doc = new PDFDocument();
                _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                DatosTerceroModel _DatosCert = new DatosTerceroModel();
                string _Formato = Server.MapPath($"~/Areas/Dynamics/Plantilla/Formulario_220_{Agno}.pdf");
                if (System.IO.File.Exists(_Formato))
                {
                    MemoryStream _Plantilla = new MemoryStream(System.IO.File.ReadAllBytes(_Formato));
                    _Doc = new PDFDocument(_Plantilla);
                    _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                    TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 9, true, true);
                    PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                    _Arial.Bold = false;
                    PDFTextFormatOptions tfoRight = new PDFTextFormatOptions();
                    tfoRight.Align = PDFTextAlign.MiddleRight;
                    tfoRight.ClipText = PDFClipText.ClipNone;
                    PDFTextFormatOptions tfoLeft = new PDFTextFormatOptions();
                    tfoLeft.Align = PDFTextAlign.MiddleLeft;
                    tfoLeft.ClipText = PDFClipText.ClipNone;
                    PDFTextFormatOptions tfoCenter = new PDFTextFormatOptions();
                    tfoCenter.Align = PDFTextAlign.MiddleCenter;
                    tfoCenter.ClipText = PDFClipText.ClipNone;
                    _Doc.Pages[0].Canvas.DrawTextBox(Agno.ToString(), _Arial, null, brush, 290, 35, 80, 30, tfoCenter);
                    string _mesAgno = DateTime.Now.ToString("MMMM DE yyyy", CultureInfo.GetCultureInfo("es-CO")).ToUpper();
                    _Doc.Pages[0].Canvas.DrawTextBox("890984423  3", _Arial, null, brush, 15, 88, 180, 30, tfoRight);
                    _Doc.Pages[0].Canvas.DrawTextBox("AREA METROPOLITANA DEL VALLE DE ABURRA", _Arial, null, brush, 90, 100, 300, 30, tfoLeft);
                    _Sql = $"SELECT TOP(1) * FROM (SELECT V.ACCOUNTNUM AS TERCERO,DT.NAME AS NOMBRE,P.FIRSTNAME,P.MIDDLENAME,P.LASTNAME,P.AP_CO_SECONDLASTNAME,P.RECID FROM VENDTABLE V INNER JOIN DIRPARTYTABLE DT ON V.PARTY=DT.RECID LEFT JOIN DIRPERSONNAME P ON DT.RECID=P.PERSON) QRY WHERE TERCERO ='{IdTer}' ORDER BY RECID DESC";
                    var Persona = dbDynamics.Database.SqlQuery<DatosTerceroModel>(_Sql).FirstOrDefault();
                    if (Persona != null)
                    {
                        //_Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(D.MAINACCOUNTVALUE, SUBSTRING(D2.DISPLAYVALUE,1,6))                AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,1)) IN('5')                 AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(D.MAINACCOUNTVALUE,SUBSTRING(D2.DISPLAYVALUE,1,6)))                QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        _Sql = $"SELECT * FROM (SELECT YEAR(T.TRANSDATE) AS AGNO,D.TERCEROVALUE AS TERCERO, D.MAINACCOUNTVALUE AS CUENTAPRINCIPAL,CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) AS VALOR,'RETENCION' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION = D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT = RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION = D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND D.MAINACCOUNTVALUE='24361510' AND D.TERCEROVALUE='{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),D.DISPLAYVALUE,D.MAINACCOUNTVALUE,D.TERCEROVALUE) QRY WHERE QRY.AGNO = {Agno} UNION ALL SELECT * FROM (SELECT AGNO, TERCERO, CUENTAPRINCIPAL, SUM(VALOR) AS VALOR, OPERACION FROM (SELECT YEAR(T.TRANSDATE) AS AGNO, ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) AS TERCERO,ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1), SUBSTRING(D2.DISPLAYVALUE,1,1)) AS CUENTAPRINCIPAL,CASE WHEN  T.ACCOUNTTYPE = 0 THEN CAST(SUM(T.AMOUNTCURDEBIT -AMOUNTCURCREDIT) AS BIGINT) ELSE CAST(SUM(T.AMOUNTCURCREDIT - T.AMOUNTCURDEBIT) AS BIGINT) END AS VALOR, 'INGRESO' AS OPERACION FROM LEDGERJOURNALTABLE J INNER JOIN LEDGERJOURNALTRANS T ON J.JOURNALNUM=T.JOURNALNUM INNER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D ON T.LEDGERDIMENSION=D.RECID LEFT OUTER JOIN APBUDGETRP RP ON T.APBUDGETRPIDREFEDT=RP.RECID LEFT OUTER JOIN DIMENSIONATTRIBUTEVALUECOMBINATION D2 ON T.OFFSETLEDGERDIMENSION=D2.RECID WHERE J.JOURNALNAME LIKE 'PRESTASERV' AND ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1), SUBSTRING(D2.DISPLAYVALUE,1,1)) IN ('5') AND ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE) = '{Persona.TERCERO}' GROUP BY YEAR(T.TRANSDATE),T.ACCOUNTTYPE,ISNULL(D.TERCEROVALUE,D.DISPLAYVALUE),ISNULL(SUBSTRING(D.MAINACCOUNTVALUE,1,1),SUBSTRING(D2.DISPLAYVALUE,1,1))) QRY GROUP BY AGNO,TERCERO,CUENTAPRINCIPAL,OPERACION) QRY WHERE QRY.AGNO = {Agno}";
                        var DatosCert = dbDynamics.Database.SqlQuery<DatosCertificado>(_Sql).ToList();
                        if (DatosCert != null)
                        {
                            foreach (var _cert in DatosCert)
                            {
                                if (_cert.OPERACION == "RETENCION") Persona.VALORRETENCION = _cert.VALOR;
                                if (_cert.OPERACION == "INGRESO") Persona.VALORINGRESO = _cert.VALOR;
                                Persona.AGNO = Agno;
                            }
                            if (Persona.NOMBRE != "" && Persona.FIRSTNAME == null)
                            {
                                string[] _arrayNombre = Persona.NOMBRE.Split(' ');
                                switch (_arrayNombre.Length)
                                {
                                    case 1:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = "";
                                        Persona.LASTNAME = "";
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 2:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = "";
                                        Persona.LASTNAME = _arrayNombre[1];
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 3:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = _arrayNombre[1];
                                        Persona.LASTNAME = _arrayNombre[2];
                                        Persona.AP_CO_SECONDLASTNAME = "";
                                        break;
                                    case 4:
                                        Persona.FIRSTNAME = _arrayNombre[0];
                                        Persona.MIDDLENAME = _arrayNombre[1];
                                        Persona.LASTNAME = _arrayNombre[2];
                                        Persona.AP_CO_SECONDLASTNAME = _arrayNombre[3];
                                        break;
                                }
                            }
                            if (Agno >= 2023) _Doc.Pages[0].Canvas.DrawText("C.C.", _Arial, null, brush, 40, 136);
                            _Doc.Pages[0].Canvas.DrawText(Persona.TERCERO, _Arial, null, brush, 120, 136);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.LASTNAME == null ? "" : Persona.LASTNAME, _Arial, null, brush, 230, 123, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.AP_CO_SECONDLASTNAME == null ? "" : Persona.AP_CO_SECONDLASTNAME, _Arial, null, brush, 320, 123, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.FIRSTNAME == null ? "" : Persona.FIRSTNAME, _Arial, null, brush, 410, 123, 90, 30, tfoCenter);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.MIDDLENAME == null ? "" : Persona.MIDDLENAME, _Arial, null, brush, 500, 123, 90, 30, tfoCenter);
                            DateTime firstDay = new DateTime(Agno, 1, 1);
                            DateTime lastDay = new DateTime(Agno, 12, 31);
                            _Doc.Pages[0].Canvas.DrawText(firstDay.ToString("yyyy     MM    dd"), _Arial, null, brush, 60, 160);
                            _Doc.Pages[0].Canvas.DrawText(lastDay.ToString("yyyy     MM    dd"), _Arial, null, brush, 170, 160);
                            _Doc.Pages[0].Canvas.DrawText(DateTime.Now.ToString("yyyy     MM    dd"), _Arial, null, brush, 260, 160);
                            _Doc.Pages[0].Canvas.DrawText("Medellín", _Arial, null, brush, 340, 160);
                            _Doc.Pages[0].Canvas.DrawText("05", _Arial, null, brush, 522, 160);
                            _Doc.Pages[0].Canvas.DrawText("05001", _Arial, null, brush, 550, 160);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 183, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 195, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 207, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORINGRESO.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 218, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 231, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 243, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 255, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 268, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 280, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 292, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 304, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 316, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 328, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 340, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 352, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 364, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORINGRESO.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 376, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 400, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 410, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 421, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 431, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 441, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 451, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 461, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 471, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORRETENCION.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 482, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("AREA METROPOLITANA DEL VALLE DE ABURRA", _Arial, null, brush, 160, 484, 300, 30, tfoLeft);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 530, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 542, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 554, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 566, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 578, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 590, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 375, 602, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 530, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 542, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 554, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 566, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 578, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 590, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox("0", _Arial, null, brush, 498, 602, 90, 10, tfoRight);
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.VALORRETENCION.ToString("N0", CultureInfo.CreateSpecificCulture("es-CO")), _Arial, null, brush, 498, 614, 90, 10, tfoRight);
                        }
                    }
                }
                _Doc.Save(_DocSalida);
            }
            catch
            {
                return null;
            }
            return _DocSalida;
        }
    }
}