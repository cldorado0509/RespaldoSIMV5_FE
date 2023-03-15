using AreaMetro.Seguridad;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.BarCode;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using Independentsoft.Office.Word.Fields;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Barcodes;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using SIM.Areas.Dynamics.Data;
using SIM.Areas.Facturacion.Models;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

namespace SIM.Areas.Dynamics.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EtiquetaController : Controller
    {

        // GET: Dynamics/Etiqueta
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bien"></param>
        /// <returns></returns>
        [ActionName("ImprimirEti")]
        public FileContentResult GetImprimirEti(string Bien)
        {
            PDFDocument _Doc = new PDFDocument();
            MemoryStream oStream = new MemoryStream();
            _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            if (Bien != "")
            {
                TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 7, true, true);
                PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                PDFImage _imgLogo = new PDFImage(Server.MapPath(@"~/Content/Images/Logo_AreaRad.png"));
                PDFCode128Barcode dm = new PDFCode128Barcode();
                dm.Data = Bien;
                dm.YDimension = 20;
                dm.XDimension = 0.8;
                dm.Font = _Arial;
                PDFPage Pagina = _Doc.AddPage();
                Pagina.Width = 288;
                Pagina.Height = 72;
                Pagina.Canvas.DrawText("ÁREA METROPOLITANA", _Arial, null, brush, 10, 12);
                Pagina.Canvas.DrawText("DEL VALLE DE ABURRÁ", _Arial, null, brush, 10, 20);
                Pagina.Canvas.DrawImage(_imgLogo, 110, 5, 25, 25, 0, PDFKeepAspectRatio.KeepNone);
                Pagina.Canvas.DrawBarcode(dm, 0, 30);
                string _mesAgno = DateTime.Now.ToString("MMMM DE yyyy", CultureInfo.GetCultureInfo("es-CO")).ToUpper();
                Pagina.Canvas.DrawText(_mesAgno, _Arial, null, brush, 40, 60);
                _Doc.Save(oStream);
            }
            oStream.Position = 0;
            var Archivo = oStream.ToArray();
            return File(Archivo, "application/pdf");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListaEti"></param>
        /// <returns></returns>
        [ActionName("ImprimirEtiSel")]
        public FileContentResult GetImprimirEtiSel(string ListaEti)
        {
            PDFDocument _Doc = new PDFDocument();
            MemoryStream oStream = new MemoryStream();
            _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            if (ListaEti.Length > 0)
            {
                List<string> arrEtiquetas = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string[]>(ListaEti).ToList();
                TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 7, true, true);
                PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                MemoryStream _ImgCode = new MemoryStream();
                PDFImage _imgLogo = new PDFImage(Server.MapPath(@"~/Content/Images/Logo_AreaRad.png"));
                for (var i=0; i < arrEtiquetas.Count; i = i + 2) {
                    string _IzqEti = arrEtiquetas[i];
                    string _DerEti = "";
                    if ((arrEtiquetas.Count - 1) > i) _DerEti = arrEtiquetas[i + 1];
                    PDFCode128Barcode dm = new PDFCode128Barcode();
                    dm.Data = _IzqEti;
                    dm.YDimension = 20;
                    dm.XDimension = 0.5;
                    dm.Font = _Arial;
                    dm.Left = 2;
                    PDFPage Pagina = _Doc.AddPage();                    
                    Pagina.Width = 288;
                    Pagina.Height = 72;
                    Pagina.Canvas.DrawText("ÁREA METROPOLITANA", _Arial, null, brush, 10, 12);
                    Pagina.Canvas.DrawText("DEL VALLE DE ABURRÁ", _Arial, null, brush, 10, 20);
                    Pagina.Canvas.DrawImage(_imgLogo, 110, 5, 25, 25, 0, PDFKeepAspectRatio.KeepNone);
                    Pagina.Canvas.DrawBarcode(dm, 0, 30);
                    string _mesAgno = DateTime.Now.ToString("MMMM DE yyyy", CultureInfo.GetCultureInfo("es-CO")).ToUpper();
                    Pagina.Canvas.DrawText(_mesAgno, _Arial, null, brush, 40, 60);
                    if (_DerEti != "")
                    {
                        dm = new PDFCode128Barcode();
                        dm.Data = _DerEti;
                        dm.YDimension = 20;
                        dm.XDimension = 0.5;
                        dm.Font = _Arial;
                        dm.Left = 2;
                       
                        Pagina.Canvas.DrawText("ÁREA METROPOLITANA", _Arial, null, brush, 160, 12);
                        Pagina.Canvas.DrawText("DEL VALLE DE ABURRÁ", _Arial, null, brush, 160, 20);
                        Pagina.Canvas.DrawImage(_imgLogo, 260, 5, 25, 25, 0, PDFKeepAspectRatio.KeepNone);
                        Pagina.Canvas.DrawBarcode(dm, 150, 30);
                        Pagina.Canvas.DrawText(_mesAgno, _Arial, null, brush, 190, 60);
                    }
                }
                _Doc.Save(oStream);

            }
            oStream.Position = 0;
            var Archivo = oStream.ToArray();
            return File(Archivo, "application/pdf");
        }
    }
}