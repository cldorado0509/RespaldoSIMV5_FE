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
    public class PazsalvoBienesController : Controller
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
        /// <param name="Tercero"></param>
        /// <returns></returns>
        [ActionName("GeneraPazySalvo")]
        public FileContentResult GetGeneraPazySalvo(string Tercero)
        {
            MemoryStream _DocSalida = new MemoryStream();
            string _Sql = "";
            if (Tercero != "")
            {
                try
                {
                    PDFDocument _Doc = new PDFDocument();
                    _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                    DatosTerceroModel _DatosCert = new DatosTerceroModel();
                    string _Formato = Server.MapPath($"~/Areas/Dynamics/Plantilla/Plantilla_Paz_y_Salvo_Bienes.pdf");
                    if (System.IO.File.Exists(_Formato))
                    {
                        MemoryStream _Plantilla = new MemoryStream(System.IO.File.ReadAllBytes(_Formato));
                        _Doc = new PDFDocument(_Plantilla);
                        _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                        TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 9, true, true);
                        PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                        _Arial.Bold = false;
                        PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                        tfo.Align = PDFTextAlign.TopJustified;
                        tfo.ClipText = PDFClipText.ClipNone;
                        string _FechaCorta = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es-CO"));
                        string _Hora = DateTime.Now.ToString("HH:mm:ss");
                        string _FechaLarga = DateTime.Now.ToString("MMMM dd \'de\' yyyy", CultureInfo.GetCultureInfo("es-CO"));
                        _Doc.Pages[0].Canvas.DrawText(_FechaCorta, _Arial, null, brush, 100, 35);
                        _Doc.Pages[0].Canvas.DrawText(_Hora, _Arial, null, brush, 100, 50);
                        _Arial.Size = 12;
                        _Doc.Pages[0].Canvas.DrawText(_FechaCorta, _Arial, null, brush, 315, 70);
                        _Sql = $"SELECT DISTINCT (DPN.FIRSTNAME + ' ' + ISNULL(DPN.MIDDLENAME,'') + ' ' + ISNULL(DPN.LASTNAME,'') + ' ' + ISNULL(DPN.AP_CO_SECONDLASTNAME,'')) AS Name,HW.PersonnelNumber AS Tercero FROM HCMWORKER AS HW LEFT OUTER JOIN DIRPERSONNAME AS DPN ON HW.PERSON = DPN.PERSON WHERE HW.PersonnelNumber='{Tercero}'";
                        var Persona = dbDynamics.Database.SqlQuery<TercerosModel>(_Sql).FirstOrDefault();
                        if (Persona != null)
                        {
                            string _Contenido = $"El señor(a): {Persona.Name.ToUpper()}, identificado(a) con cédula de ciudadanía No. {Persona.Tercero}, a la fecha {_FechaLarga} se encuentra a paz y salvo con el área de inventarios por no tener en la actualidad ningún bien devolutivo (activo) a su cargo.";
                            _Doc.Pages[0].Canvas.DrawTextBox(_Contenido, _Arial, null, brush, 40, 150, _Doc.Pages[0].Width - 80, 300, tfo);
                            //_Doc.Pages[0].Canvas.DrawText(Persona.Name.ToUpper(), _Arial, null, brush, 400, 650);
                            tfo.Align = PDFTextAlign.MiddleCenter;
                            tfo.ClipText = PDFClipText.ClipNone;
                            _Doc.Pages[0].Canvas.DrawTextBox(Persona.Name, _Arial, null, brush, 370, 638, 200, 10, tfo);
                        }
                    }
                    _Doc.Save(_DocSalida);
                }
                catch
                {
                    return null;
                }
            }
            _DocSalida.Position = 0;
            var Archivo = _DocSalida.ToArray();
            return File(Archivo, "application/pdf");
        }
    }
}