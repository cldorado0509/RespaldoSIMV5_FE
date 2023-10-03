using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using O2S.Components.PDF4NET.PDFFile;
using O2S.Components.PDF4NET.Text;
using PdfSharp.Drawing;
using SIM.Areas.GestionDocumental.Models;
using SIM.Data;
using SIM.Services;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace SIM.Areas.GestionDocumental.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReemplazoDTO
    {
        public int Pagina { get; set; }
        public string CampoReemplazo { get; set; }
        public PDFSearchTextResultCollection ListReemplazo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MasivosApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RecibePlantilla")]
        public ResponseFile RecibePantilla(string IdSolicitud)
        {
            ResponseFile archivoDTO = new ResponseFile();
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var httpRequest = context.Request;
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            if (IdSolicitud == "-1")
            {
                IdSolicitud = new Guid(Guid.NewGuid().ToString()).ToString("N");
            }
            if (httpRequest.Files.Count > 0)
            {
                if (IdSolicitud != null)
                {
                    var File = httpRequest.Files[0];
                    if (File != null && File.ContentLength > 0)
                    {
                        try
                        {
                            MemoryStream _ArchivoPdf = new MemoryStream();
                            string[] fileExtensions = { ".pdf" };
                            var fileName = File.FileName.ToLower();
                            var isValidExtenstion = fileExtensions.Any(ext =>
                            {
                                return fileName.LastIndexOf(ext) > -1;
                            });
                            if (isValidExtenstion)
                            {
                                FileInfo _Plantilla = new FileInfo(_RutaCorrespondencia + @"\" + IdSolicitud + @"\" + fileName);
                                if (!_Plantilla.Directory.Exists) _Plantilla.Directory.Create();
                                BinaryReader b = new BinaryReader(File.InputStream);
                                byte[] FileData = b.ReadBytes(File.ContentLength);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(new MemoryStream(FileData), _Plantilla.FullName);
                                string _resp = ValidaPlatilla(IdSolicitud);
                                if (_resp != null && _resp.StartsWith("Error:"))
                                {
                                    Directory.GetFiles(_Plantilla.Directory.FullName, "*.pdf").ToList().ForEach(f => System.IO.File.Delete(f));
                                    archivoDTO.IdSolicitud = IdSolicitud;
                                    archivoDTO.SubidaExitosa = false;
                                    archivoDTO.MensajeError = _resp;
                                }
                                else
                                {
                                    archivoDTO.IdSolicitud = IdSolicitud;
                                    archivoDTO.SubidaExitosa = true;
                                    archivoDTO.MensajeError = "";
                                    archivoDTO.MensajeExito = "Plantilla Pdf del documento subida con éxito!";
                                }
                            }
                            else
                            {
                                archivoDTO.IdSolicitud = IdSolicitud;
                                archivoDTO.SubidaExitosa = false;
                                archivoDTO.MensajeError = "El tipo de archivo no esta permitido";
                            }
                        }
                        catch (Exception exp)
                        {
                            archivoDTO.IdSolicitud = IdSolicitud;
                            archivoDTO.SubidaExitosa = false;
                            archivoDTO.MensajeError = exp.Message;
                        }
                    }
                }
                else
                {
                    archivoDTO.IdSolicitud = IdSolicitud;
                    archivoDTO.SubidaExitosa = false;
                    archivoDTO.MensajeError = "No se ingresó un identificador para poder subir los anexos de su solicitud";
                }
            }
            return archivoDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RecibeExcel")]
        public ResponseFile RecibeExcel(string IdSolicitud)
        {
            ResponseFile archivoDTO = new ResponseFile();
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var httpRequest = context.Request;
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            if (IdSolicitud == "-1")
            {
                IdSolicitud = new Guid(Guid.NewGuid().ToString()).ToString("N");
            }
            if (httpRequest.Files.Count > 0)
            {
                if (IdSolicitud != null)
                {
                    var File = httpRequest.Files[0];
                    if (File != null && File.ContentLength > 0)
                    {
                        try
                        {
                            MemoryStream _ArchivoPdf = new MemoryStream();
                            string[] fileExtensions = { ".xls", ".xlsx" };
                            var fileName = File.FileName.ToLower();
                            var isValidExtenstion = fileExtensions.Any(ext =>
                            {
                                return fileName.LastIndexOf(ext) > -1;
                            });
                            if (isValidExtenstion)
                            {
                                FileInfo _Excel = new FileInfo(_RutaCorrespondencia + @"\" + IdSolicitud + @"\" + fileName);
                                if (!_Excel.Directory.Exists) _Excel.Directory.Create();
                                else foreach (System.IO.FileInfo file in _Excel.Directory.GetFiles()) file.Delete();
                                BinaryReader b = new BinaryReader(File.InputStream);
                                byte[] FileData = b.ReadBytes(File.ContentLength);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(new MemoryStream(FileData), _Excel.FullName);

                                archivoDTO.IdSolicitud = IdSolicitud;
                                archivoDTO.SubidaExitosa = true;
                                archivoDTO.MensajeError = "";
                                archivoDTO.MensajeExito = "Archivo de Excel subido con éxito!";
                            }
                            else
                            {
                                archivoDTO.IdSolicitud = IdSolicitud;
                                archivoDTO.SubidaExitosa = false;
                                archivoDTO.MensajeError = "El tipo de archivo no esta permitido";
                            }
                        }
                        catch (Exception exp)
                        {
                            archivoDTO.IdSolicitud = IdSolicitud;
                            archivoDTO.SubidaExitosa = false;
                            archivoDTO.MensajeError = exp.Message;
                        }
                    }
                }
                else
                {
                    archivoDTO.IdSolicitud = IdSolicitud;
                    archivoDTO.SubidaExitosa = false;
                    archivoDTO.MensajeError = "No se ingresó un identificador para poder subir los anexos de su solicitud";
                }
            }
            return archivoDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("CargaExcel")]
        public JArray GetCargaExcel(string IdSolicitud)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            DataTable dt = new DataTable();

            if (IdSolicitud != null && IdSolicitud.Length > 0)
            {
                string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
                string[] files;
                string _ExcelEncontrado = "";
                string _Dir = _RutaCorrespondencia + @"\" + IdSolicitud;
                if (Directory.Exists(_Dir))
                {
                    files = Directory.GetFiles(_Dir, @"*.xls", SearchOption.TopDirectoryOnly);
                    if (files.Length > 0) _ExcelEncontrado = files[0];
                    else
                    {
                        files = Directory.GetFiles(_Dir, @"*.xlsx", SearchOption.TopDirectoryOnly);
                        if (files.Length > 0) _ExcelEncontrado = files[0];
                    }
                }
                if (_ExcelEncontrado != "")
                {
                    dt = LeeExcel(_ExcelEncontrado);
                }
            }
            return JArray.FromObject(dt, Js);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMassiveDTO RadicarMasivo(MasivoDTO datos)
        {
            var _mensaje = "";
            int _paginas = 0;
            if (!ModelState.IsValid) return new ResponseMassiveDTO() { isSuccess = false, message = "Error generando la radicación masiva" };
            PDFDocument _doc = new PDFDocument();
            _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            var userId = Int32.Parse(User.Identity.GetUserId());
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + datos.IdSolicitud;
            if (!Directory.Exists(_Dir)) Directory.CreateDirectory(_Dir);
            Radicador radicador = new Radicador();
            if (datos.IdSolicitud != "")
            {
                DataTable dt = LeeArchivoExcel(datos.IdSolicitud);
                dt.Columns.Add("Radicado Generado", typeof(string));
                dt.Columns.Add("Código Tramite", typeof(Int32));
                dt.Columns.Add("Código Documento", typeof(Int32));
                dt.Columns.Add("Comentarios", typeof(string));
                string _RutaPdf = ObtienePlatillaPdf(datos.IdSolicitud);
                if (_RutaPdf != "")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        PDFFile _docPdf = PDFFile.FromFile(_RutaPdf);
                        var DatosReemplazo = ReemplazoDoc(datos.IdSolicitud, dt.Columns);
                        int _correctos = 0;
                        if (DatosReemplazo.Count > 0)
                        {
                            PDFPage _pag = null;
                            bool _continua = true;
                            decimal IdRadicado = -1;
                            foreach (DataRow fila in dt.Rows)
                            {
                                if (datos.CodTramite == "")
                                {
                                    if (!SIM.Utilidades.Tramites.ExisteTramite(decimal.Parse(fila["CODTRAMITE"].ToString())))
                                    {
                                        _continua = false;
                                        _mensaje += $"El documento de la fila {fila["ID"]} no se pudo generar ya que el Cotramite {fila["CODTRAMITE"]} no se encontró <br />";
                                    }
                                }
                                else _continua = true;
                                if (_continua)
                                {
                                    string _Radicado = "";
                                    string _FecRad = "";
                                    _doc = new PDFDocument();
                                    _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                                    PDFImportedPage ip = null;
                                    PDFTextRun pDFTextRun = null;
                                    _paginas = _docPdf.PagesCount;
                                    for (var i = 0; i < _paginas; i++)
                                    {
                                        ip = _docPdf.ExtractPage(i);
                                        _doc.Pages.Add(ip);
                                    }
                                    foreach (var reemp in DatosReemplazo)
                                    {
                                        _pag = _doc.Pages[reemp.Pagina];
                                        var Campo = fila[reemp.CampoReemplazo].ToString();
                                        foreach (var dato in reemp.ListReemplazo)
                                        {
                                            pDFTextRun = dato.TextRuns[0];
                                            if (pDFTextRun != null)
                                            {
                                                Font _fnt = new Font(pDFTextRun.FontName, (float)pDFTextRun.FontSize);
                                                TrueTypeFont _Arial = new TrueTypeFont(_fnt, true);
                                                PDFBrush BrushW = new PDFBrush(new PDFRgbColor(255, 255, 255));
                                                PDFBrush brushNegro = new PDFBrush(new PDFRgbColor(Color.Black));
                                                PDFBrush BrushTrans = new PDFBrush(new PDFRgbColor(Color.Transparent));
                                                PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                                                PDFPen Pen = new PDFPen(new PDFRgbColor(Color.Black));
                                                tfo.Align = PDFTextAlign.TopLeft;
                                                tfo.ClipText = PDFClipText.ClipNone;

                                                _pag.Canvas.DrawRectangle(null, BrushW, pDFTextRun.DisplayBounds.Left, pDFTextRun.DisplayBounds.Top, 120, pDFTextRun.DisplayBounds.Height + 2, 0);
                                                _pag.Canvas.DrawText(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top));
                                            }
                                        }
                                        _doc.Pages[reemp.Pagina] = _pag;
                                    }
                                    var fechaCreacion = DateTime.Now;
                                    DatosRadicado radicadoGenerado = radicador.GenerarRadicado(dbSIM, 12, userId, fechaCreacion);
                                    var imagenRadicado = radicador.ObtenerImagenRadicadoArea(radicadoGenerado.IdRadicado);
                                    if (imagenRadicado != null)
                                    {
                                        _pag = _doc.Pages[0];
                                        _pag.Canvas.DrawImage(imagenRadicado, 300, 30, 288, 72);
                                        _Radicado = radicadoGenerado.Radicado;
                                        _FecRad = radicadoGenerado.Fecha.ToString("dd/MM/yyyy");
                                        IdRadicado = radicadoGenerado.IdRadicado;
                                        _doc.Pages[0] = _pag;
                                    }
                                    decimal CodTramite = datos.CodTramite != "" ? decimal.Parse(datos.CodTramite) : decimal.Parse(fila["CODTRAMITE"].ToString());
                                    List<IndicesDocumento> _Indices = new List<IndicesDocumento>();
                                    IndicesDocumento _Index;
                                    foreach (var Ind in datos.Indices)
                                    {
                                        _Index = new IndicesDocumento();
                                        _Index.CODINDICE = Ind.CODINDICE;
                                        if (Ind.INDICE.ToLower().Contains("radicado") || Ind.INDICE.ToLower().Contains("fecha"))
                                        {
                                            _Index.VALOR = Ind.INDICE.ToLower().Contains("radicado") ? _Radicado : _FecRad;
                                        }
                                        else
                                        {
                                            if (Ind.VALORDEFECTO != null && Ind.VALORDEFECTO != "") _Index.VALOR = fila[Ind.VALORDEFECTO].ToString();
                                            else _Index.VALOR = Ind.VALOR;
                                        }
                                        _Indices.Add(_Index);
                                    }
                                    SIM.Utilidades.Documento documento = new Utilidades.Documento();
                                    documento.TipoDocumento = 1;
                                    documento.Extension = "pdf";
                                    documento.Codfuncionario = SIM.Utilidades.Tramites.ObtenerCodiogoFuncionario(userId);
                                    documento.CodSerie = 12;
                                    documento.IdUsuario = userId;
                                    documento.Paginas = _paginas;
                                    MemoryStream streamDoc = new MemoryStream();
                                    _doc.Save(streamDoc);


                                    documento.Archivo = streamDoc.ToArray();
                                    if (!SIM.Utilidades.Tramites.AdicionaDocRadicadoTramite(CodTramite, IdRadicado, documento, _Indices))
                                    {
                                        _mensaje += $"El documento de la fila {fila["ID"]} no se pudo generar ya ocurrió un problema con el documento <br />";
                                        fila["Comentarios"] = $"El documento no se pudo generar ya ocurrió un problema con el documento";
                                    }
                                    else
                                    {
                                        _correctos++;
                                        var CodDoc = dbSIM.RADICADO_DOCUMENTO.Where(w => w.ID_RADICADODOC == IdRadicado).Select(s => s.CODDOCUMENTO).FirstOrDefault();
                                        fila["Radicado Generado"] = _Radicado;
                                        fila["Código Documento"] = CodDoc;
                                        fila["Código Tramite"] = CodTramite;
                                        fila["Comentarios"] = "Radicado y documento generado correctamente";
                                    }
                                }
                                else fila["Comentarios"] = $"El documento no se pudo generar ya que el Cotramite {fila["CODTRAMITE"]} no se encontró";
                            }
                            dt.AcceptChanges();
                        }
                        if (_mensaje != "") _mensaje = _correctos + " documentos creados correctamente con excepciones: <br />" + _mensaje;
                        else _mensaje = _correctos + " documentos creados correctamente.";
                        FileInfo result = new FileInfo(_Dir + @"\" + "Resultado.xlsx");
                        Workbook wb = new Workbook();
                        wb.Worksheets[0].Import(dt, true, 0, 0);
                        wb.SaveDocument(result.FullName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                        StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                          Select(column => column.ColumnName);
                        sb.AppendLine(string.Join(",", columnNames));

                        foreach (DataRow row in dt.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                            sb.AppendLine(string.Join(",", fields));
                        }
                        MemoryStream stream = new MemoryStream(File.ReadAllBytes(result.FullName));


                        return new ResponseMassiveDTO() { isSuccess = true, message = _mensaje, responseFile = File.ReadAllBytes(result.FullName) };
                    }
                    else return new ResponseMassiveDTO() { isSuccess = false, message = "No se ingresaron los índices para la unidad documental!!" };

                }
                else return new ResponseMassiveDTO() { isSuccess = false, message = "No se pudo localizar el archivo Pdf de la plantilla para combinar!!" };
            }
            else return new ResponseMassiveDTO() { isSuccess = false, message = "Falta el identificador de la solicitud!!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        // public System.Web.Mvc.FileContentResult PrevisualizaMasivo(MasivoDTO datos)
        public byte[] PrevisualizaMasivo(MasivoDTO datos)
        {
            MemoryStream streamDoc = new MemoryStream();
            PDFDocument _doc = new PDFDocument();
            _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + datos.IdSolicitud;
            if (!Directory.Exists(_Dir)) Directory.CreateDirectory(_Dir);
            if (datos.IdSolicitud != "")
            {
                DataTable dt = LeeArchivoExcel(datos.IdSolicitud);
                DataRow dr = dt.Rows[0];
                string _RutaPdf = ObtienePlatillaPdf(datos.IdSolicitud);
                if (_RutaPdf != "")
                {
                    if (dr != null)
                    {
                        PDFFile _docPdf = PDFFile.FromFile(_RutaPdf);
                        var DatosReemplazo = ReemplazoDoc(datos.IdSolicitud, dt.Columns);
                        if (DatosReemplazo.Count > 0)
                        {
                            PDFPage _pag = null;
                            PDFImportedPage ip = null;
                            PDFTextRun pDFTextRun = null;
                            int _paginas = _docPdf.PagesCount;
                            for (var i = 0; i < _paginas; i++)
                            {
                                ip = _docPdf.ExtractPage(i);
                                _doc.Pages.Add(ip);
                            }
                            foreach (var reemp in DatosReemplazo)
                            {
                                _pag = _doc.Pages[reemp.Pagina];
                                var Campo = dr[reemp.CampoReemplazo].ToString();
                                foreach (var dato in reemp.ListReemplazo)
                                {
                                    pDFTextRun = dato.TextRuns[0];
                                    if (pDFTextRun != null)
                                    {
                                        Font _fnt = new Font(pDFTextRun.FontName, (float)pDFTextRun.FontSize);
                                        TrueTypeFont _Arial = new TrueTypeFont(_fnt, true);
                                        PDFBrush BrushW = new PDFBrush(new PDFRgbColor(255, 255, 255));
                                        PDFBrush brushNegro = new PDFBrush(new PDFRgbColor(Color.Black));
                                        PDFBrush BrushTrans = new PDFBrush(new PDFRgbColor(Color.Transparent));
                                        PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                                        PDFPen Pen = new PDFPen(new PDFRgbColor(Color.Black));
                                        tfo.Align = PDFTextAlign.TopLeft;
                                        tfo.ClipText = PDFClipText.ClipNone;

                                        _pag.Canvas.DrawRectangle(null, BrushW, pDFTextRun.DisplayBounds.Left, pDFTextRun.DisplayBounds.Top, 120, pDFTextRun.DisplayBounds.Height + 2, 0);
                                        _pag.Canvas.DrawText(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top));
                                    }
                                }
                                if (reemp.Pagina == 0)
                                {
                                    Bitmap _BmpRad = ObtenerImagenEspacioRadicado();
                                    _pag.Canvas.DrawImage(_BmpRad, 300, 30, 288, 72);
                                }
                                _doc.Pages[reemp.Pagina] = _pag;
                            }
                        }
                        _doc.Save(streamDoc);
                    }
                }
                else return null;
            }
            else return null;

            streamDoc.Position = 0;
            return streamDoc.ToArray();
            //var Archivo = streamDoc.ToArray();
            //return new System.Web.Mvc.FileContentResult(Archivo, "application/pdf");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codSerie"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesSerieDocumental")]
        public dynamic GetObtenerIndicesSerieDocumental(int codSerie)
        {
            var indicesSerieDocumental = from i in dbSIM.TBINDICESERIE
                                         join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                         from pdis in l.DefaultIfEmpty()
                                         where i.CODSERIE == codSerie && i.MOSTRAR == "1"
                                         orderby i.ORDEN
                                         select new IndiceCOD
                                         {
                                             CODINDICE = i.CODINDICE,
                                             INDICE = i.INDICE,
                                             TIPO = i.TIPO,
                                             LONGITUD = i.LONGITUD,
                                             OBLIGA = i.OBLIGA,
                                             VALORDEFECTO = i.VALORDEFECTO,
                                             VALOR = "",
                                             ID_VALOR = null,
                                             ID_LISTA = i.CODIGO_SUBSERIE,
                                             TIPO_LISTA = pdis.TIPO,
                                             CAMPO_NOMBRE = pdis.CAMPO_NOMBRE,
                                             INDICE_RADICADO = i.INDICE_RADICADO
                                         };

            return indicesSerieDocumental.ToList();
        }

        #region Metodos Privados de la clase
        private DataTable LeeExcel(string _ruta)
        {
            DataTable dt = new DataTable();
            if (_ruta != null)
            {
                string conn = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_ruta};Extended Properties='Excel 8.0;HDR=yes'";
                OleDbConnection excelConn = new OleDbConnection(conn);
                excelConn.Open();
                DataTable dtExcel = excelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string excelSheet = dtExcel.Rows[0]["TABLE_NAME"].ToString();
                OleDbCommand cmd = new OleDbCommand("Select * from [" + excelSheet + "]", excelConn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                excelConn.Close();
            }
            return dt;
        }

        private DataTable LeeArchivoExcel(string Identificador)
        {
            DataTable dt = new DataTable();
            if (Identificador != null && Identificador.Length > 0)
            {
                string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
                string[] files;
                string _ExcelEncontrado = "";
                string _Dir = _RutaCorrespondencia + @"\" + Identificador;
                if (Directory.Exists(_Dir))
                {
                    files = Directory.GetFiles(_Dir, @"*.xls", SearchOption.TopDirectoryOnly);
                    if (files.Length > 0) _ExcelEncontrado = files[0];
                    else
                    {
                        files = Directory.GetFiles(_Dir, @"*.xlsx", SearchOption.TopDirectoryOnly);
                        if (files.Length > 0) _ExcelEncontrado = files[0];
                    }
                }
                if (_ExcelEncontrado != "")
                {
                    dt = LeeExcel(_ExcelEncontrado);
                }
            }
            return dt;
        }

        private string ValidaPlatilla(string IdSolicitud)
        {
            DataTable dtExcel = new DataTable();
            string _resp = "";
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string[] files;
            string _ExcelEncontrado = "";
            string _PdfEncontrado = "";
            string _Dir = _RutaCorrespondencia + @"\" + IdSolicitud;
            if (Directory.Exists(_Dir))
            {
                files = Directory.GetFiles(_Dir, @"*.xls", SearchOption.TopDirectoryOnly);
                if (files.Length > 0) _ExcelEncontrado = files[0];
                else
                {
                    files = Directory.GetFiles(_Dir, @"*.xlsx", SearchOption.TopDirectoryOnly);
                    if (files.Length > 0) _ExcelEncontrado = files[0];
                }
                files = Directory.GetFiles(_Dir, @"*.pdf", SearchOption.TopDirectoryOnly);
                if (files.Length > 0) _PdfEncontrado = files[0];
            }
            if (_ExcelEncontrado != "")
            {
                dtExcel = LeeExcel(_ExcelEncontrado);
            }
            if (dtExcel.Rows.Count > 0)
            {
                if (_PdfEncontrado != "")
                {
                    PdfDocumentProcessor PdfPlantilla = new PdfDocumentProcessor();
                    PdfPlantilla.LoadDocument(_PdfEncontrado, true);
                    string _textoPdf = PdfPlantilla.Text;
                    Regex rx = new Regex(@"\[(.*?)\]");
                    MatchCollection matchedAuthors = rx.Matches(_textoPdf);
                    foreach (Match match in matchedAuthors)
                    {
                        if (!dtExcel.Columns.Contains(match.Value.Trim().Substring(1, match.Value.Trim().Length - 2)))
                        {
                            return $"Error: El archivo de Excel no contiene datos para la clave {match.Value.Trim()} de la platilla pdf!";
                        }
                    }
                }
            }
            return _resp;
        }

        private string ObtienePlatillaPdf(string Identificador)
        {
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _PdfEncontrado = "";
            string[] files;
            string _Dir = _RutaCorrespondencia + @"\" + Identificador;
            if (Directory.Exists(_Dir))
            {
                files = Directory.GetFiles(_Dir, @"*.pdf", SearchOption.TopDirectoryOnly);
                if (files.Length > 0) _PdfEncontrado = files[0];
            }
            return _PdfEncontrado;
        }

        private void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }

        private List<ReemplazoDTO> ReemplazoDoc(string Identificador, DataColumnCollection Columns)
        {
            List<ReemplazoDTO> _resp = new List<ReemplazoDTO>();
            string _RutaPdf = ObtienePlatillaPdf(Identificador);
            PDFFile _docPdf = PDFFile.FromFile(_RutaPdf);
            PDFImportedPage ip = null;
            for (var i = 0; i < _docPdf.PagesCount; i++)
            {
                ip = _docPdf.ExtractPage(i);
                foreach (DataColumn col in Columns)
                {
                    PDFSearchTextResultCollection _result = ip.SearchText("[" + col.ColumnName + "]");
                    if (_result != null && _result.Count > 0)
                    {
                        var encontrado = new ReemplazoDTO()
                        {
                            Pagina = i,
                            CampoReemplazo = col.ColumnName,
                            ListReemplazo = _result
                        };
                        _resp.Add(encontrado);
                    }
                }
            }
            return _resp;
        }

        /// <summary>
        /// Obtiene el espacio donde va el radicado en imagen
        /// </summary>
        /// <returns></returns>
        private Bitmap ObtenerImagenEspacioRadicado()
        {
            Bitmap canvas = new Bitmap(590, 150);
            canvas.SetResolution(150, 150);
            Font _fnt = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            Pen pen = new Pen(Color.Black, 1);
            pen.Alignment = PenAlignment.Inset;
            Rectangle _rect = new Rectangle(0, 0, canvas.Width - 1, canvas.Height - 1);
            using (Graphics gra = Graphics.FromImage(canvas))
            {
                gra.FillRectangle(new SolidBrush(Color.White), _rect);
                gra.DrawRectangle(pen, _rect);
                gra.DrawString("Espacio para el RADICADO", _fnt, brush, 100, 60);
            }
            return canvas;
        }


        #endregion
    }

    public class IndiceCOD
    {
        public int CODINDICE { get; set; }
        public string INDICE { get; set; }
        public byte TIPO { get; set; }
        public long LONGITUD { get; set; }
        public int OBLIGA { get; set; }
        public string VALORDEFECTO { get; set; }
        public string VALOR { get; set; }
        public int? ID_VALOR { get; set; }
        public Nullable<int> ID_LISTA { get; set; }
        public Nullable<int> TIPO_LISTA { get; set; }
        public string CAMPO_NOMBRE { get; set; }
        public string MAXIMO { get; set; }
        public string MINIMO { get; set; }
        public string INDICE_RADICADO { get; set; }
    }
}