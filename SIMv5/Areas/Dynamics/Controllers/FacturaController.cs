using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using SIM.Areas.Dynamics.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SIM.Areas.Dynamics.Controllers
{
    public class FacturaController : Controller
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        // GET: Dynamics/Factura
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdFact"></param>
        /// <returns></returns>
        [ActionName("ImprimirFactura")]
        public FileContentResult GetImprimirFactura(string IdFact)
        {
            MemoryStream stream = new MemoryStream();
            stream = GenerarFacturaPdf(IdFact);
            var Archivo = stream.ToArray();
            return File(Archivo, "application/pdf");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdFact"></param>
        /// <returns></returns>
        public MemoryStream GenerarFacturaPdf(string IdFact)
        {
            MemoryStream _DocSalida = new MemoryStream();
            if (IdFact != "")
            {
                try
                {

                    MemoryStream Resp = new MemoryStream();
                    PDFDocument _doc = new PDFDocument();
                    _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                    PDFPage Pagina = _doc.AddPage();
                    Pagina.Width = 2550;
                    Pagina.Height = 3300;
                    PDFImage _img = new PDFImage(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/Images/Logo_Area.png"));
                    Pagina.Canvas.DrawImage(_img, 100, 100, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
                    TrueTypeFont _Arial = new TrueTypeFont(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/fonts/arialbd.ttf"), 80, true, true);
                    _Arial.Bold = true;
                    _Arial.Size = 70;
                    PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                    Pagina.Canvas.DrawText("ÁREA METROPOLITANA DEL", _Arial, null, brush, 600, 120);
                    Pagina.Canvas.DrawText("VALLE DE ABURRÁ", _Arial, null, brush, 780, 210);
                    _Arial.Bold = false;
                    _Arial.Size = 40;
                    Pagina.Canvas.DrawText("DOCUMENTO SOPORTE", _Arial, null, brush, 1800, 160);
                    Pagina.Canvas.DrawText("N°", _Arial, null, brush, 1800, 220);
                    Pagina.Canvas.DrawText(IdFact, _Arial, null, brush, 1870, 220);  //Variable
                    Pagina.Canvas.DrawText("Entidad Administrativa de Derecho Público", _Arial, null, brush, 750, 290);
                    Pagina.Canvas.DrawText("IVA Régimen común", _Arial, null, brush, 970, 330);
                    _Arial.Size = 30;
                    Pagina.Canvas.DrawText("NIT  890984423-3", _Arial, null, brush, 110, 500);
                    Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 110, 540);
                    Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 110, 580);
                    Pagina.Canvas.DrawText("Colombia", _Arial, null, brush, 110, 620);
                    Pagina.Canvas.DrawText("Tel. (604) 385 6000", _Arial, null, brush, 110, 660);
                    _Arial.Size = 40;
                    _Arial.Bold = true;
                    Pagina.Canvas.DrawText("Datos", _Arial, null, brush, 550, 500);
                    Pagina.Canvas.DrawText("Usuario", _Arial, null, brush, 550, 550);
                    PDFPen _Pen = new PDFPen(new PDFRgbColor(Color.Black), 5);
                    Pagina.Canvas.DrawLine(_Pen, 720, 500, 720, 745);
                    _Arial.Bold = false;
                    DatosTerceroFactura tercero = ObtenerDatosTercero(IdFact);
                    if (tercero != null)
                    {
                        if (tercero.Tercero.Trim().Length < 41) Pagina.Canvas.DrawText(tercero.Tercero.Trim().ToUpper(), _Arial, null, brush, 750, 500); // Variable
                        else
                        {
                            Pagina.Canvas.DrawText(tercero.Tercero.Trim().ToUpper().Substring(0, 40), _Arial, null, brush, 750, 500); // Variable
                            Pagina.Canvas.DrawText(tercero.Tercero.Trim().ToUpper().Substring(39, tercero.Tercero.Trim().Length - 40), _Arial, null, brush, 750, 540); // Variable
                        }
                        int DigitoVer = SIM.Utilidades.Facturacion.ObtenerDigitoVerificacion(tercero.Documento);
                        if (DigitoVer.ToString() != "")
                        {
                            Pagina.Canvas.DrawText(tercero.Documento + "-" + DigitoVer.ToString(), _Arial, null, brush, 750, 580); // Variable
                            Pagina.Canvas.DrawText(tercero.Documento + "-" + DigitoVer.ToString(), _Arial, null, brush, 490, 2430); //Variable
                        }
                        else
                        {
                            Pagina.Canvas.DrawText(tercero.Documento, _Arial, null, brush, 750, 580); // Variable
                            Pagina.Canvas.DrawText(tercero.Documento, _Arial, null, brush, 490, 2430); //Variable
                        }
                        if (!string.IsNullOrEmpty(tercero.Direccion)) Pagina.Canvas.DrawText(tercero.Direccion.Trim().ToUpper(), _Arial, null, brush, 750, 620); // Variable
                        if (!string.IsNullOrEmpty(tercero.Ciudad)) Pagina.Canvas.DrawText(tercero.Ciudad.Trim(), _Arial, null, brush, 750, 660); // Variable
                        if (!string.IsNullOrEmpty(tercero.Telefono)) Pagina.Canvas.DrawText("Teléfono : " + tercero.Telefono.Trim(), _Arial, null, brush, 750, 700); // Variable
                        if (!string.IsNullOrEmpty(tercero.Email)) Pagina.Canvas.DrawText("Correo elect. : " + tercero.Email.Trim(), _Arial, null, brush, 750, 740); // Variable
                    }
                    _Pen.Width = 3;
                    _Arial.Size = 30;
                    Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 565, 600, 120, 0);
                    Pagina.Canvas.DrawText("Fecha Elaboración", _Arial, null, brush, 1810, 580);
                    Pagina.Canvas.DrawLine(_Pen, 1800, 630, 2400, 630);
                    Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 1810, 640);
                    DatosFacturaHeader header = ObtenerHeaderFactura(IdFact);
                    if (header != null)
                    {
                        Pagina.Canvas.DrawText(header.FechaElabora.ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 580); //Variable
                        Pagina.Canvas.DrawText(header.FechaVence.ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 640); //Variable
                        double _ValFact = (double)header.ValorFactura;
                        if (_ValFact > 0)
                        {
                            Pagina.Canvas.DrawText(SIM.Utilidades.Facturacion.enletras(_ValFact.ToString().Trim()), _Arial, null, brush, 450, 1795);  //Variable
                            Pagina.Canvas.DrawText(_ValFact.ToString("C0", CultureInfo.GetCultureInfo("es-CO")), _Arial, null, brush, 2110, 1850);  //Variable
                        }
                        else
                        {
                            _ValFact = _ValFact * -1;
                            Pagina.Canvas.DrawText(SIM.Utilidades.Facturacion.enletras(_ValFact.ToString().Trim()), _Arial, null, brush, 450, 1795);  //Variable
                            Pagina.Canvas.DrawText("(" + _ValFact.ToString("C0", CultureInfo.GetCultureInfo("es-CO")) + ")", _Arial, null, brush, 2110, 1850);  //Variable
                        }
                    }
                    _Arial.Size = 35;
                    _Arial.Bold = true;
                    Pagina.Canvas.DrawText("                       AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 500, 800);
                    Pagina.Canvas.DrawLine(_Pen, 100, 850, 2400, 850);
                    Pagina.Canvas.DrawRectangle(_Pen, null, 100, 870, 2300, 900, 0);
                    Pagina.Canvas.DrawLine(_Pen, 100, 930, 2400, 930);
                    _Arial.Size = 30;
                    Pagina.Canvas.DrawText("DESCRIPCIÓN", _Arial, null, brush, 350, 890);
                    Pagina.Canvas.DrawLine(_Pen, 950, 870, 950, 1770);
                    Pagina.Canvas.DrawText("DETALLE", _Arial, null, brush, 1450, 890);
                    Pagina.Canvas.DrawLine(_Pen, 2100, 870, 2100, 1770);
                    Pagina.Canvas.DrawText("TOTAL", _Arial, null, brush, 2200, 890);
                    Pagina.Canvas.DrawRectangle(_Pen, null, 100, 1770, 2300, 60, 0);
                    Pagina.Canvas.DrawText("VALOR EN LETRAS :", _Arial, null, brush, 110, 1795);
                    Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 1830, 600, 60, 0);
                    Pagina.Canvas.DrawText("TOTAL  ", _Arial, null, brush, 1840, 1850);
                    _Arial.Bold = false;
                    int _Linea = 960;
                    PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                    tfo.Align = PDFTextAlign.TopJustified;
                    tfo.ClipText = PDFClipText.ClipNone;
                    var Detalle = ObtenerDetalleFactura(IdFact);
                    if (Detalle != null)
                    {
                        double LinNom = 0;
                        double LinObs = 0;
                        foreach (DatosFacturaDetalle det in Detalle)
                        {
                            if (det.Descripcion.ToString().Trim().Length <= 46) Pagina.Canvas.DrawText(det.Descripcion.ToString().Trim().ToUpper(), _Arial, null, brush, 110, _Linea);  //Variable
                            else
                            {
                                Pagina.Canvas.DrawTextBox(det.Descripcion.ToString().Trim().ToUpper(), _Arial, brush, 110, _Linea, 810, 100, tfo);  //Variable
                                LinNom = Math.Round((double)(det.Descripcion.Trim().Length / 46), MidpointRounding.AwayFromZero);
                            }
                            Pagina.Canvas.DrawTextBox(det.Detalle.Trim().ToUpper(), _Arial, brush, 960, _Linea, 1100, 100, tfo); //Variable
                            LinObs = Math.Round((double)(det.Detalle.Trim().Length / 50), MidpointRounding.AwayFromZero);
                            if (det.Valor > 0) Pagina.Canvas.DrawText(double.Parse(det.Valor.ToString()).ToString("C0", CultureInfo.GetCultureInfo("es-CO")), _Arial, null, brush, 2110, _Linea); //Variable
                            else Pagina.Canvas.DrawText("(" + double.Parse((det.Valor * -1).ToString()).ToString("C0", CultureInfo.GetCultureInfo("es-CO")) + ")", _Arial, null, brush, 2110, _Linea); //Variable
                            if (LinNom > LinObs) _Linea += (int)(LinNom * 40);
                            else if (LinNom < LinObs) _Linea += (int)(LinObs * 40);
                            else _Linea += (int)(LinObs * 40);
                            _Linea += 100;
                        }
                    }
                    Pagina.Canvas.DrawText("Por favor conserve la parte superior para verificar su pago", _Arial, null, brush, 145, 2000);
                    if (tercero != null && header != null)
                    {
                        //Codigo de barras
                        var S_CLAVE = "7709998019133";
                        if (!string.IsNullOrEmpty(S_CLAVE) && header.ValorFactura > 0)
                        {
                            string _NIT = long.Parse(tercero.Documento).ToString("00000000000");
                            var NumFact = Int32.Parse(Regex.Match(header.Factura, @"\d+").Value);
                            string _FACT = NumFact.ToString("000000000");
                            string _ValorF = header.ValorFactura.Value.ToString("00000000000000");
                            string _FechaPago = header.FechaVence.ToString("yyyyMMdd");
                            string _TextCode = "415" + S_CLAVE.Trim() + "8020" + _NIT + _FACT + "#3900" + _ValorF + "#96" + _FechaPago; //Variable
                            Image _ImgBc = SIM.Utilidades.Facturacion.CodeBar128Fact(_TextCode, DateTime.Now.AddMonths(1), new System.Drawing.Size(1300, 250));
                            PDFImage _BcImg = new PDFImage((Bitmap)_ImgBc);
                            Pagina.Canvas.DrawImage(_BcImg, 1000, 1910, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
                            Pagina.Canvas.DrawImage(_BcImg, 1000, 2510, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
                        }
                        Pagina.Canvas.DrawText(header.Factura.Trim(), _Arial, null, brush, 490, 2370); //Variable
                        if (header.Banco != "") Pagina.Canvas.DrawText(header.Banco.Trim().ToUpper(), _Arial, null, brush, 1800, 2370); //Variable
                        Pagina.Canvas.DrawText(header.Cuenta.Trim(), _Arial, null, brush, 1800, 2430); //Variable
                        Pagina.Canvas.DrawText(DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd"), _Arial, null, brush, 490, 2490); //Variable
                        if (header.ValorFactura > 0) Pagina.Canvas.DrawText(header.ValorFactura.Value.ToString("C0", CultureInfo.GetCultureInfo("es-CO")), _Arial, null, brush, 490, 2550); //Variable
                        else Pagina.Canvas.DrawText("(" + (header.ValorFactura * -1).Value.ToString("C0", CultureInfo.GetCultureInfo("es-CO")) + ")", _Arial, null, brush, 490, 2550); //Variable
                    }
                    _Arial.Size = 40;
                    _Arial.Bold = true;
                    _Pen.DashStyle = PDFDashStyle.DashDot;
                    Pagina.Canvas.DrawLine(_Pen, 100, 2220, 2400, 2220);
                    _Pen.DashStyle = PDFDashStyle.Solid;
                    Pagina.Canvas.DrawText("DOCUMENTO SOPORTE", _Arial, null, brush, 1100, 2250);
                    Pagina.Canvas.DrawRectangle(_Pen, null, 135, 2350, 700, 240, 0);
                    Pagina.Canvas.DrawRectangle(_Pen, null, 1500, 2350, 900, 120, 0);
                    _Arial.Size = 30;
                    Pagina.Canvas.DrawText("Factura N°", _Arial, null, brush, 145, 2370);
                    Pagina.Canvas.DrawLine(_Pen, 450, 2350, 450, 2590);
                    Pagina.Canvas.DrawLine(_Pen, 135, 2410, 835, 2410);
                    Pagina.Canvas.DrawText("Banco", _Arial, null, brush, 1510, 2370);
                    Pagina.Canvas.DrawLine(_Pen, 1780, 2350, 1780, 2470);
                    Pagina.Canvas.DrawLine(_Pen, 1500, 2410, 2400, 2410);
                    Pagina.Canvas.DrawText("NIT Cliente", _Arial, null, brush, 145, 2430);
                    Pagina.Canvas.DrawLine(_Pen, 135, 2470, 835, 2470);
                    Pagina.Canvas.DrawText("Cuenta Ahorros N°", _Arial, null, brush, 1510, 2430);
                    Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 145, 2490);
                    Pagina.Canvas.DrawLine(_Pen, 135, 2530, 835, 2530);
                    Pagina.Canvas.DrawText("Valor Total", _Arial, null, brush, 145, 2550);
                    _Arial.Bold = false;
                    Pagina.Canvas.DrawLine(_Pen, 100, 2770, 2400, 2770);
                    Pagina.Canvas.DrawImage(_img, 100, 2790, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
                    Pagina.Canvas.DrawLine(_Pen, 650, 2790, 650, 3157);
                    _Arial.Size = 25;
                    Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 700, 2950);
                    Pagina.Canvas.DrawText("Ed. Área Metropolitana", _Arial, null, brush, 700, 3000);
                    Pagina.Canvas.DrawText("NIT 890984423-3", _Arial, null, brush, 700, 3050);
                    Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 700, 3100);
                    Pagina.Canvas.DrawLine(_Pen, 1350, 2790, 1350, 3157);
                    Pagina.Canvas.DrawText("IVA Régimen Común", _Arial, null, brush, 1400, 2900);
                    Pagina.Canvas.DrawText("AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 1400, 3000);
                    Pagina.Canvas.DrawText("metropol@metropol.gov.co", _Arial, null, brush, 1400, 3050);
                    Pagina.Canvas.DrawText("http://www.metropol.gov.co", _Arial, null, brush, 1400, 3100);
                    _doc.Save(_DocSalida);
                }
                catch
                {
                    return null;
                }
            }
            _DocSalida.Position = 0;
            return _DocSalida;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Factura"></param>
        /// <returns></returns>
        public DatosTerceroFactura ObtenerDatosTercero(string Factura)
        {
            if (string.IsNullOrEmpty(Factura)) { return null; }
            string _Sql = $"SELECT DISTINCT CIT.INVOICEACCOUNT AS DOCUMENTO,DPT.NAME AS TERCERO,LPA.STREET AS DIRECCION,(COUNTY.NAME + ' - ' + STATE.NAME) AS CIUDAD,LEA_PCP.LOCATOR AS TELEFONO,LEA_PCE.LOCATOR AS EMAIL FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT = CT.ACCOUNTNUM INNER JOIN DIRPARTYTABLE AS DPT ON CT.PARTY = DPT.RECID LEFT OUTER JOIN LOGISTICSPOSTALADDRESS AS LPA ON CIT.POSTALADDRESS = LPA.RECID LEFT OUTER JOIN LOGISTICSELECTRONICADDRESS AS LEA_PCP ON DPT.PRIMARYCONTACTPHONE = LEA_PCP.RECID LEFT OUTER JOIN LOGISTICSELECTRONICADDRESS AS LEA_PCE ON DPT.PRIMARYCONTACTEMAIL = LEA_PCE.RECID LEFT OUTER JOIN LOGISTICSADDRESSSTATE AS STATE ON STATE.COUNTRYREGIONID = LPA.COUNTRYREGIONID AND STATE.STATEID = LPA.STATE LEFT OUTER JOIN LOGISTICSADDRESSCOUNTY AS COUNTY ON COUNTY.COUNTRYREGIONID = LPA.COUNTRYREGIONID AND COUNTY.STATEID = LPA.STATE AND COUNTY.COUNTYID = LPA.COUNTY WHERE (CIT.POSTED = 1) AND CIT.INVOICEID='{Factura}' ORDER BY DOCUMENTO";
            var Tercero = dbDynamics.Database.SqlQuery<DatosTerceroFactura>(_Sql).FirstOrDefault();
            if (Tercero == null) return null;
            return Tercero;
        }

        #region Metodos provados de la clase
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Factura"></param>
        /// <returns></returns>
        private DatosFacturaHeader ObtenerHeaderFactura(string Factura)
        {
            if (string.IsNullOrEmpty(Factura)) { return null; }
            string _Sql = $"SELECT CIT.INVOICEID AS Factura,CIT.INVOICEDATE AS FechaElabora,CIT.DUEDATE AS FechaVence,BA.ACCOUNTNUM AS Cuenta,BG.Name AS Banco FROM CUSTINVOICETABLE AS CIT INNER JOIN CUSTTABLE AS CT ON CIT.INVOICEACCOUNT=CT.ACCOUNTNUM LEFT OUTER JOIN CUSTLEDGER AS CL ON CIT.POSTINGPROFILE=CL.POSTINGPROFILE LEFT OUTER JOIN BANKACCOUNTTABLE AS BA ON CL.AP_VA_BANKACCOUNTS = BA.ACCOUNTID LEFT OUTER JOIN BANKGROUP BG ON BA.BANKGROUPID=BG.BANKGROUPID WHERE (CIT.POSTED = 1) AND CIT.INVOICEID='{Factura}'";
            var _Header = dbDynamics.Database.SqlQuery<DatosFacturaHeader>(_Sql).FirstOrDefault();
            if (_Header == null) return null;
            _Sql = $"SELECT DISTINCT SUM(CIL.AMOUNTCUR) AS VALOR FROM CUSTINVOICELINE AS CIL LEFT OUTER JOIN CUSTINVOICETABLE AS CIT ON CIL.PARENTRECID = CIT.RECID WHERE (CIT.POSTED = 1) AND CIT.INVOICEID='{Factura}'";
            var Total = dbDynamics.Database.SqlQuery<decimal>(_Sql).FirstOrDefault();
            _Header.ValorFactura = Total;
            return _Header;
        }

        private List<DatosFacturaDetalle> ObtenerDetalleFactura(string Factura)
        {
            if (string.IsNullOrEmpty(Factura)) { return null; }
            string _Sql = $"SELECT DISTINCT CIT.INVOICEID AS Factura,CIL.DESCRIPTION AS Descripcion,CID.TXT AS Detalle,CIL.LINENUM AS Linea,CIL.AMOUNTCUR AS Valor FROM CUSTINVOICELINE AS CIL LEFT OUTER JOIN CUSTINVOICETABLE AS CIT ON CIL.PARENTRECID=CIT.RECID INNER JOIN CUSTTRANS CID ON CIT.INVOICEID=CID.INVOICE WHERE (CIT.POSTED = 1) AND CID.TXT IS NOT NULL AND CIT.INVOICEID='{Factura}' ORDER BY CIL.LINENUM";
            var Detalle = dbDynamics.Database.SqlQuery<DatosFacturaDetalle>(_Sql).ToList();
            if (Detalle == null) return null;
            return Detalle;
        }

        #endregion
    }
}