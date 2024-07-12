using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
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
using SIM.Data.Tramites;
using SIM.Models;
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
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
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
        private string urlSIMAPI = SIM.Utilidades.Data.ObtenerValorParametro("URLSimapi").ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpPost, ActionName("RecibePlantilla")]
        public ResponseFile RecibePlantilla(string IdSolicitud)
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
                                string _resp = ValidaPlantilla(IdSolicitud);
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
                                var ExcelData = LeeExcelToDataTable(_Excel.FullName);
                                if (ExcelData != null && ExcelData.Rows.Count >= 5)
                                {
                                    archivoDTO.IdSolicitud = IdSolicitud;
                                    archivoDTO.SubidaExitosa = true;
                                    archivoDTO.MensajeError = "";
                                    archivoDTO.MensajeExito = "Archivo de Excel subido con éxito!";
                                }
                                else
                                {
                                    archivoDTO.IdSolicitud = IdSolicitud;
                                    archivoDTO.SubidaExitosa = false;
                                    archivoDTO.MensajeError = "La cantidad mínima de documentos a generar es de 5!!";
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
                    dt = LeeExcelToDataTable(_ExcelEncontrado);
                }
            }
            return JArray.FromObject(dt, Js);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMassiveDTO RadicarMasivo(string IdSolicitud)
        {
            var _mensaje = "";
            int _paginas = 0;
            if (IdSolicitud == null || IdSolicitud == "") return new ResponseMassiveDTO() { isSuccess = false, message = "Falta el identificador de la solicitud!!" };
            PDFDocument _doc = new PDFDocument();
            _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            var userId = Int32.Parse(User.Identity.GetUserId());
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + IdSolicitud;
            if (!Directory.Exists(_Dir)) Directory.CreateDirectory(_Dir);
            Radicador radicador = new Radicador();
            DataTable dt = LeeArchivoExcel(IdSolicitud);
            dt.Columns.Add("Radicado Generado", typeof(string));
            dt.Columns.Add("Código Tramite", typeof(Int32));
            dt.Columns.Add("Código Documento", typeof(Int32));
            dt.Columns.Add("Comentarios", typeof(string));
            string _RutaPdf = ObtienePlatillaPdf(IdSolicitud);
            var Masiva = dbSIM.RADMASIVA.Where(w => w.IDSOLICITUD == IdSolicitud).FirstOrDefault();
            if (Masiva == null) return new ResponseMassiveDTO() { isSuccess = false, message = "No se ha guardado el proceso de radicación masiva!!" };
            if (_RutaPdf != "")
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    var Firmas = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == Masiva.ID).ToList();
                    if (Firmas == null || Firmas.Count == 0) return new ResponseMassiveDTO() { isSuccess = false, message = "No se encontraron firmas para la plantilla!!" };
                    var Indices = dbSIM.RADMASIVAINDICES.Where(w => w.ID_RADMASIVO == Masiva.ID).ToList();
                    if (Indices == null || Indices.Count == 0) return new ResponseMassiveDTO() { isSuccess = false, message = "No se encontraron indices para los documentos!!" };
                    PDFFile _docPdf = PDFFile.FromFile(_RutaPdf);
                    var DatosReemplazo = ReemplazoDoc(IdSolicitud, dt.Columns, Firmas.Count);
                    int _correctos = 0;
                    if (DatosReemplazo.Count > 0)
                    {
                        PDFPage _pag = null;
                        bool _continua = true;
                        decimal IdRadicado = -1;
                        // FirmarPlantilla(ref _docPdf, Firmas);
                        foreach (DataRow fila in dt.Rows)
                        {
                            decimal CodTramite = (Masiva.CODTRAMITE != "" && Masiva.CODTRAMITE != null) ? decimal.Parse(Masiva.CODTRAMITE) : decimal.Parse(fila["CODTRAMITE"].ToString());
                            if (!SIM.Utilidades.Tramites.ExisteTramite(CodTramite))
                            {
                                _continua = false;
                                _mensaje += $"El documento de la fila {fila["ID"]} no se pudo generar ya que el Codtramite {CodTramite} no se encontró <br />";
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
                                PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                                tfo.Align = PDFTextAlign.TopJustified;
                                tfo.ClipText = PDFClipText.ClipWords;
                                _paginas = _docPdf.PagesCount;
                                for (var i = 0; i < _paginas; i++)
                                {
                                    ip = _docPdf.ExtractPage(i);
                                    _doc.Pages.Add(ip);
                                }

                                foreach (var reemp in DatosReemplazo)
                                {
                                    _pag = _doc.Pages[reemp.Pagina];
                                    if (!reemp.CampoReemplazo.Contains("Firma"))
                                    {
                                        var Campo = fila[reemp.CampoReemplazo].ToString();
                                        foreach (var dato in reemp.ListReemplazo)
                                        {
                                            pDFTextRun = dato.TextRuns[1];
                                            if (pDFTextRun != null)
                                            {
                                                Font _fnt = new Font(pDFTextRun.FontName, (float)pDFTextRun.FontSize);
                                                TrueTypeFont _Arial = new TrueTypeFont(_fnt, true);
                                                PDFBrush BrushW = new PDFBrush(new PDFRgbColor(255, 255, 255));
                                                PDFBrush brushNegro = new PDFBrush(new PDFRgbColor(Color.Black));
                                                PDFBrush BrushTrans = new PDFBrush(new PDFRgbColor(Color.Transparent));
                                                PDFPen Pen = new PDFPen(new PDFRgbColor(Color.Black));
                                                double _anchoEtiqueta = _Arial.MeasureString(dato.TextRuns.Count > 3 ? pDFTextRun.Text + dato.TextRuns[2].Text : pDFTextRun.Text) + 10;
                                                _pag.Canvas.DrawRectangle(null, BrushW, pDFTextRun.DisplayBounds.Left - 5, pDFTextRun.DisplayBounds.Top - 1, _anchoEtiqueta, pDFTextRun.DisplayBounds.Height + 4, 0);
                                                if (Campo.Length >= 70)
                                                {
                                                    double _anchoBox = _pag.Width - 170;
                                                    double _anchotexto = _Arial.MeasureString(Campo);
                                                    double _altoCaja = (_anchotexto / (_pag.Width - 170)) * (_Arial.Size + 3);
                                                    _pag.Canvas.DrawTextBox(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top) - 1, _anchoBox, _altoCaja, tfo);
                                                }
                                                else _pag.Canvas.DrawText(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var dato in reemp.ListReemplazo)
                                        {
                                            pDFTextRun = dato.TextRuns[0];
                                            if (pDFTextRun != null)
                                            {
                                                int f = int.Parse(reemp.CampoReemplazo.Substring(5));
                                                var _firma = Firmas.Where(w => w.ORDEN_FIRMA == f).FirstOrDefault();
                                                Image imagenFirma;
                                                if (_firma != null)
                                                {
                                                    if (_firma.CODCARGO == null || _firma.CODCARGO == 0)
                                                        imagenFirma = Security.ObtenerFirmaElectronicaFuncionario((long)_firma.FUNC_FIRMA, true, "");
                                                    else
                                                        imagenFirma = Security.ObtenerFirmaElectronicaFuncionario((long)_firma.FUNC_FIRMA, true, "", (int)_firma.CODCARGO, (_firma.S_TIPOFIRMA == "E" ? 1 : (_firma.S_TIPOFIRMA == "A" ? 2 : 0)));

                                                    PDFImage img = new PDFImage((Bitmap)imagenFirma);
                                                    // _pag.Canvas.DrawImage(img, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top), 250, 80);
                                                    _pag.Canvas.DrawImage(img, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top), Convert.ToInt32(img.Width * (240 / Convert.ToDecimal(img.Width))), Convert.ToInt32(img.Height * (240 / Convert.ToDecimal(img.Width))));
                                                }
                                            }
                                        }
                                    }
                                    _doc.Pages[reemp.Pagina] = _pag;
                                }
                                var fechaCreacion = DateTime.Now;
                                DatosRadicado radicadoGenerado = new DatosRadicado();
                                if (dt.Columns.Contains("ID_RADICADO"))
                                {
                                    decimal _IdRadicado = decimal.Parse(fila["ID_RADICADO"].ToString());
                                    var _DatRadicado = dbSIM.RADICADO_DOCUMENTO.Where(w => w.ID_RADICADODOC == _IdRadicado).FirstOrDefault();
                                    if (_DatRadicado != null)
                                    {
                                        radicadoGenerado.IdRadicado = (int)_DatRadicado.ID_RADICADODOC;
                                        radicadoGenerado.Radicado = _DatRadicado.S_RADICADO;
                                        radicadoGenerado.Fecha = _DatRadicado.D_RADICADO;
                                        radicadoGenerado.Etiqueta = _DatRadicado.S_ETIQUETA;
                                    }
                                }
                                else radicadoGenerado = radicador.GenerarRadicado(dbSIM, 12, userId, fechaCreacion);
                                if (radicadoGenerado.IdRadicado > 0) _continua = true;
                                else
                                {
                                    fila["Comentarios"] = "No se encontró o no se pudo generar el radicado, no se generó el documento";
                                    _continua = false;
                                }
                                if (_continua)
                                {
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
                                    List<IndicesDocumento> _Indices = new List<IndicesDocumento>();
                                    IndicesDocumento _Index;
                                    var _asunto = "";
                                    var _para = "";
                                    foreach (var Ind in Indices)
                                    {
                                        _Index = new IndicesDocumento();
                                        var TbIndice = dbSIM.TBINDICESERIE.Where(w => w.CODINDICE == Ind.CODINDICE).FirstOrDefault();
                                        _Index.CODINDICE = TbIndice.CODINDICE;
                                        if (Ind.S_VALOREXCEL != null && Ind.S_VALOREXCEL != "") _Index.VALOR = fila[Ind.S_VALOREXCEL].ToString();
                                        else _Index.VALOR = Ind.S_VALORASIGNADO;
                                        if (TbIndice.INDICE.ToLower().Contains("asunto")) _asunto = _Index.VALOR;
                                        if (TbIndice.INDICE.ToLower().Contains("destinatario")) _para = _Index.VALOR;
                                        _Indices.Add(_Index);
                                    }
                                    var TbIndiceRad = dbSIM.TBINDICESERIE.Where(w => (w.INDICE_RADICADO == "R" || w.INDICE_RADICADO == "F") && w.CODSERIE == 12).ToList();
                                    if (TbIndiceRad != null && TbIndiceRad.Count > 0)
                                    {
                                        foreach (var Ind in TbIndiceRad)
                                        {
                                            if (Ind.INDICE.ToLower().Contains("radicado") || Ind.INDICE.ToLower().Contains("fecha"))
                                            {
                                                _Index = new IndicesDocumento();
                                                _Index.CODINDICE = Ind.CODINDICE;
                                                _Index.VALOR = Ind.INDICE.ToLower().Contains("radicado") ? _Radicado : _FecRad;
                                                _Indices.Add(_Index);
                                            }
                                        }
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
                                        fila["Comentarios"] = $"El documento no se pudo generar ya que ocurrió un problema con el documento";
                                    }
                                    else
                                    {
                                        _correctos++;
                                        var CodDoc = dbSIM.RADICADO_DOCUMENTO.Where(w => w.ID_RADICADODOC == IdRadicado).Select(s => s.CODDOCUMENTO).FirstOrDefault();
                                        fila["Radicado Generado"] = _Radicado;
                                        fila["Código Documento"] = CodDoc;
                                        fila["Código Tramite"] = CodTramite;
                                        if (Masiva.S_ENVIACORREO == "1")
                                        {
                                            var _email = fila["EMAIL"].ToString().Trim().ToLower();
                                            if (_email.Length > 0)
                                            {
                                                if (IsValidMail(_email))
                                                {
                                                    if (!EnviarMailMk(_email, documento.Archivo, _asunto, _para, _Radicado, _FecRad))
                                                    {
                                                        fila["Comentarios"] = "Se generó el documento y se radicó, pero no se pudo enviar el email";
                                                    }
                                                    else fila["Comentarios"] = $"Se generó el documento, se radicó y se envió el correo electrónico a {_email}";
                                                }
                                                else fila["Comentarios"] = "Se generó el documento y se radicó, pero no se pudo enviar el email";
                                            }
                                            else fila["Comentarios"] = "Se generó el documento y se radicó, pero no se encontró un email para el envío";
                                        }
                                        else fila["Comentarios"] = "Radicado y documento generado correctamente";
                                    }

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
                    //MemoryStream stream = new MemoryStream(File.ReadAllBytes(result.FullName));

                    Masiva.S_REALIZADO = "1";
                    dbSIM.Entry(Masiva).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();
                    return new ResponseMassiveDTO() { isSuccess = true, message = _mensaje, responseFile = File.ReadAllBytes(result.FullName) };
                }
                else return new ResponseMassiveDTO() { isSuccess = false, message = "No se ingresaron los índices para la unidad documental!!" };
            }
            else return new ResponseMassiveDTO() { isSuccess = false, message = "No se pudo localizar el archivo Pdf de la plantilla para combinar!!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseMassiveDTO PrevisualizaMasivo(MasivoDTO datos)
        {
            MemoryStream streamDoc = new MemoryStream();
            PDFDocument _doc = new PDFDocument();
            if (!ModelState.IsValid) return new ResponseMassiveDTO() { isSuccess = false, message = "Error generando la previsualización del documento" };
            _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + datos.IdSolicitud;
            if (!Directory.Exists(_Dir)) Directory.CreateDirectory(_Dir);
            if (datos.IdSolicitud != "" || datos.IdSolicitud != "-1")
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
                            PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                            tfo.Align = PDFTextAlign.TopJustified;
                            tfo.ClipText = PDFClipText.ClipWords;
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
                                    pDFTextRun = dato.TextRuns[1];
                                    if (pDFTextRun != null)
                                    {
                                        Font _fnt = new Font(pDFTextRun.FontName, (float)pDFTextRun.FontSize);
                                        TrueTypeFont _Arial = new TrueTypeFont(_fnt, true);
                                        PDFBrush BrushW = new PDFBrush(new PDFRgbColor(255, 255, 255));
                                        PDFBrush brushNegro = new PDFBrush(new PDFRgbColor(Color.Black));
                                        PDFBrush BrushTrans = new PDFBrush(new PDFRgbColor(Color.Transparent));
                                        PDFPen Pen = new PDFPen(new PDFRgbColor(Color.Black));
                                        double _anchoEtiqueta = _Arial.MeasureString(dato.TextRuns.Count > 3 ? pDFTextRun.Text + dato.TextRuns[2].Text : pDFTextRun.Text) + 10;
                                        _pag.Canvas.DrawRectangle(null, BrushW, pDFTextRun.DisplayBounds.Left - 5, pDFTextRun.DisplayBounds.Top - 1, _anchoEtiqueta, pDFTextRun.DisplayBounds.Height + 4, 0);
                                        if (Campo.Length >= 70)
                                        {
                                            double _anchoBox = _pag.Width - 170;
                                            double _anchotexto = _Arial.MeasureString(Campo);
                                            double _altoCaja = (_anchotexto / (_pag.Width - 170)) * (_Arial.Size + 3);
                                            _pag.Canvas.DrawTextBox(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top) - 1, _anchoBox, _altoCaja, tfo);
                                        }
                                        else _pag.Canvas.DrawText(Campo, _Arial, brushNegro, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top));
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
                else return new ResponseMassiveDTO() { isSuccess = false, message = "No se pudo localizar el archivo Pdf de la plantilla para combinar!!" };
            }
            else return new ResponseMassiveDTO() { isSuccess = false, message = "Falta el identificador de la solicitud!!" };
            FileInfo result = new FileInfo(_Dir + @"\" + "Preview.pdf");
            if (result.Exists) result.Delete();
            streamDoc.Position = 0;
            SIM.Utilidades.Archivos.GrabaMemoryStream(streamDoc, result.FullName);
            return new ResponseMassiveDTO() { isSuccess = true, message = "", responseFile = File.ReadAllBytes(result.FullName) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public byte[] LeePlantilla(string IdSolicitud)
        {
            if (string.IsNullOrEmpty(IdSolicitud)) return null;
            var Plantilla = ObtienePlatillaPdf(IdSolicitud);
            if (string.IsNullOrEmpty(Plantilla)) return null;
            return File.ReadAllBytes(Plantilla);
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
                                         where i.CODSERIE == codSerie && i.MOSTRAR == "1" && string.IsNullOrEmpty(i.INDICE_RADICADO)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("EditarIndicesMasivo")]
        public dynamic GetEditarIndicesMasivo(string IdSolicitud)
        {
            var IdMasiva = (from mas in dbSIM.RADMASIVA
                            join ind in dbSIM.RADMASIVAINDICES on mas.ID equals ind.ID_RADMASIVO
                            where mas.IDSOLICITUD == IdSolicitud
                            select mas.ID).FirstOrDefault();
            if (IdMasiva > 0)
            {
                var Editindices = (from mas in dbSIM.RADMASIVA
                                   join ind in dbSIM.RADMASIVAINDICES on mas.ID equals ind.ID_RADMASIVO
                                   join din in dbSIM.TBINDICESERIE on ind.CODINDICE equals din.CODINDICE
                                   join lista in dbSIM.TBSUBSERIE on (decimal)din.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                   from pdis in l.DefaultIfEmpty()
                                   where mas.IDSOLICITUD == IdSolicitud
                                   orderby din.ORDEN
                                   select new IndiceCOD
                                   {
                                       CODINDICE = (int)ind.CODINDICE,
                                       INDICE = din.INDICE,
                                       TIPO = din.TIPO,
                                       LONGITUD = din.LONGITUD,
                                       OBLIGA = din.OBLIGA,
                                       VALORDEFECTO = ind.S_VALOREXCEL,
                                       VALOR = ind.S_VALORASIGNADO,
                                       ID_VALOR = null,
                                       ID_LISTA = din.CODIGO_SUBSERIE,
                                       TIPO_LISTA = pdis.TIPO,
                                       CAMPO_NOMBRE = pdis.CAMPO_NOMBRE,
                                       INDICE_RADICADO = din.INDICE_RADICADO
                                   });
                return Editindices.ToList();
            }
            else
            {
                var indicesSerieDocumental = from i in dbSIM.TBINDICESERIE
                                             join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                             from pdis in l.DefaultIfEmpty()
                                             where i.CODSERIE == 12 && i.MOSTRAR == "1"
                                             orderby i.ORDEN
                                             select new IndiceCOD
                                             {
                                                 CODINDICE = i.CODINDICE,
                                                 INDICE = i.INDICE,
                                                 TIPO = i.TIPO,
                                                 LONGITUD = i.LONGITUD,
                                                 OBLIGA = i.OBLIGA,
                                                 VALORDEFECTO = "",
                                                 VALOR = "",
                                                 ID_VALOR = null,
                                                 ID_LISTA = i.CODIGO_SUBSERIE,
                                                 TIPO_LISTA = pdis.TIPO,
                                                 CAMPO_NOMBRE = pdis.CAMPO_NOMBRE,
                                                 INDICE_RADICADO = i.INDICE_RADICADO
                                             };
                return indicesSerieDocumental.ToList();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("ObtenerFirmas")]
        public dynamic GetObtenerFirmas(string IdSolicitud)
        {
            var firmas = from mas in dbSIM.RADMASIVA
                         join fir in dbSIM.RADMASIVAFIRMAS on mas.ID equals fir.ID_RADMASIVO
                         join fun in dbSIM.QRY_FUNCIONARIO_ALL on fir.FUNC_FIRMA equals fun.CODFUNCIONARIO
                         where mas.IDSOLICITUD == IdSolicitud
                         orderby fir.ORDEN_FIRMA
                         select new
                         {
                             CODFUNCIONARIO = fir.FUNC_FIRMA,
                             FUNCIONARIO = fun.NOMBRES,
                             ORDEN = fir.ORDEN_FIRMA
                         };
            return firmas.ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodFunc"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListadoMasivos")]
        public JArray GetListadoMasivos(decimal CodFunc)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            bool _PuedeRadicar = false;
            var _FuncionariosRadicanMasivos = SIM.Utilidades.Data.ObtenerValorParametro("FuncionariosRadicanMasivos");
            if (_FuncionariosRadicanMasivos != "")
            {
                string[] _Funcionarios = _FuncionariosRadicanMasivos.Split(',');
                _PuedeRadicar = _Funcionarios.Contains(CodFunc.ToString());
            }
            if (!_PuedeRadicar)
            {
                var listaUsr = (from rut in dbSIM.RADMASIVARUTA
                                join mas in dbSIM.RADMASIVA on rut.ID_RADMASIVO equals mas.ID
                                where rut.FECHA_RUTA == dbSIM.RADMASIVARUTA.Where(w => w.ID_RADMASIVO == mas.ID).OrderByDescending(f => f.FECHA_RUTA).Select(s => s.FECHA_RUTA).FirstOrDefault() &&
                                rut.CODFUNCIONARIO == CodFunc && mas.S_REALIZADO == "0"
                                orderby mas.D_FECHA descending
                                select new ListadoMasivos
                                {
                                    ID = mas.ID,
                                    TEMA = mas.S_TEMA,
                                    D_FECHA = mas.D_FECHA,
                                    CANTIDAD_FILAS = mas.CANTIDAD_FILAS,
                                    ESTADO = mas.S_VALIDADO == "0" ? "ELABORACION" : "LISTO PARA RADICAR",
                                    IDSOLICITUD = mas.IDSOLICITUD,
                                    CODTRAMITE = mas.CODTRAMITE,
                                    ENVIACORREO = mas.S_ENVIACORREO,
                                    FUNCIONARIOFIRMA = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == mas.ID && (w.S_FIRMADO == "0" || string.IsNullOrEmpty(w.S_FIRMADO))).Select(s => s.FUNC_FIRMA).FirstOrDefault(),
                                    FUNCIONARIOELABORA = mas.FUNC_ELABORA,
                                    MENSAJE = rut.S_COMENTARIO
                                });
                return JArray.FromObject(listaUsr.ToList(), Js);
            }
            else
            {
                var lista = (from mas in dbSIM.RADMASIVA
                             where mas.S_REALIZADO == "0" && mas.S_VALIDADO.Equals("1")
                             orderby mas.D_FECHA descending
                             select new ListadoMasivos
                             {
                                 ID = mas.ID,
                                 TEMA = mas.S_TEMA,
                                 D_FECHA = mas.D_FECHA,
                                 CANTIDAD_FILAS = mas.CANTIDAD_FILAS,
                                 ESTADO = mas.S_VALIDADO == "0" ? "ELABORACION" : "LISTO PARA RADICAR",
                                 IDSOLICITUD = mas.IDSOLICITUD,
                                 CODTRAMITE = mas.CODTRAMITE,
                                 ENVIACORREO = mas.S_ENVIACORREO,
                                 FUNCIONARIOFIRMA = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == mas.ID && (w.S_FIRMADO == "0" || string.IsNullOrEmpty(w.S_FIRMADO))).Select(s => s.FUNC_FIRMA).FirstOrDefault(),
                                 FUNCIONARIOELABORA = mas.FUNC_ELABORA,
                                 MENSAJE = ""
                             });
                return JArray.FromObject(lista.ToList(), Js);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ObtenerCantidadFirmas")]
        public object GetCantidadFirmas(string IdSolicitud)
        {
            int contFirmas = 0;
            if (IdSolicitud == "" || IdSolicitud == null) return new { resp = "Error", mensaje = "No se ingresó un identificador de solicitud!" };
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + IdSolicitud;
            if (Directory.Exists(_Dir))
            {
                var files = Directory.GetFiles(_Dir, @"*.pdf", SearchOption.TopDirectoryOnly);
                if (files.Length > 0)
                {
                    files = files.Where((val, idx) => !val.ToLower().Contains("preview")).ToArray();
                }
                if (files.Length > 0)
                {
                    PdfDocumentProcessor PdfPlantilla = new PdfDocumentProcessor();
                    PdfPlantilla.LoadDocument(files[0], true);
                    string _textoPdf = PdfPlantilla.Text;
                    Regex rx = new Regex(@"\[(.*?)\]");
                    MatchCollection matchedAuthors = rx.Matches(_textoPdf);
                    foreach (System.Text.RegularExpressions.Match match in matchedAuthors)
                    {
                        if (match.Value.Trim().Substring(1, match.Value.Trim().Length - 2).ToLower().Contains("firma")) contFirmas++;
                    }
                }
                else return new { resp = "Error", mensaje = "No se encontró un archivo de plantilla!" };
            }
            return new { resp = "Ok", mensaje = "", Cantidad = contFirmas };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GuardaMasivo")]
        public object PostGuardaMasivo(MasivoDTO datos)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Faltan datos en la solicitud!" };
            int idUsuario = 0;
            decimal funcionario = 0;
            decimal IdMasivo = -1;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                funcionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
            }
            try
            {
                if (datos.IdSolicitud != null && datos.IdSolicitud != "")
                {
                    var Excel = ObtieneExcel(datos.IdSolicitud);
                    if (Excel == null || Excel == "") return new { resp = "Error", mensaje = "Ocurrió un problema con el archivo de excel!" };
                    var Plantilla = ObtienePlatillaPdf(datos.IdSolicitud);
                    if (Plantilla == null || Plantilla == "") return new { resp = "Error", mensaje = "Ocurrió un problema con la platilla de la COD para el proceso de radicación masiva!" };
                    var RadMasiva = dbSIM.RADMASIVA.Where(w => w.IDSOLICITUD == datos.IdSolicitud).FirstOrDefault();
                    if (RadMasiva != null)
                    {
                        if (RadMasiva.FUNC_ELABORA == funcionario)
                        {
                            IdMasivo = RadMasiva.ID;
                            RadMasiva.S_RUTAEXCEL = Excel;
                            RadMasiva.S_RUTAPLANTILLA = Plantilla;
                            var ListaIndices = dbSIM.RADMASIVAINDICES.Where(w => w.ID_RADMASIVO == RadMasiva.ID).ToList();
                            if (datos.Indices != null && datos.Indices.Count > 0)
                            {
                                dbSIM.RADMASIVAINDICES.RemoveRange(dbSIM.RADMASIVAINDICES.Where(w => w.ID_RADMASIVO == RadMasiva.ID));
                                dbSIM.SaveChanges();
                                foreach (var ind in datos.Indices)
                                {
                                    RADMASIVAINDICES newInd = new RADMASIVAINDICES
                                    {
                                        CODINDICE = ind.CODINDICE,
                                        ID_RADMASIVO = RadMasiva.ID,
                                        S_VALOREXCEL = ind.VALORDEFECTO != "" ? ind.VALORDEFECTO : "",
                                        S_VALORASIGNADO = ind.VALOR != "" ? ind.VALOR : ""
                                    };
                                    dbSIM.RADMASIVAINDICES.Add(newInd);
                                }
                            }
                            else
                            {
                                if (ListaIndices == null || ListaIndices.Count <= 0)
                                {
                                    var Indices = GetObtenerIndicesSerieDocumental(12);
                                    foreach (var ind in Indices)
                                    {
                                        RADMASIVAINDICES newInd = new RADMASIVAINDICES
                                        {
                                            CODINDICE = ind.CODINDICE,
                                            ID_RADMASIVO = RadMasiva.ID,
                                            S_VALOREXCEL = "",
                                            S_VALORASIGNADO = ""
                                        };
                                        dbSIM.RADMASIVAINDICES.Add(newInd);
                                    }
                                }
                            }
                            if (datos.Firmas != null && datos.Firmas.Count > 0)
                            {
                                dbSIM.RADMASIVAFIRMAS.RemoveRange(dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == RadMasiva.ID));
                                dbSIM.SaveChanges();
                                foreach (var fir in datos.Firmas)
                                {
                                    RADMASIVAFIRMAS newFirm = new RADMASIVAFIRMAS
                                    {
                                        ID_RADMASIVO = RadMasiva.ID,
                                        FUNC_FIRMA = fir.CodFuncionario,
                                        ORDEN_FIRMA = fir.Orden
                                    };
                                    dbSIM.RADMASIVAFIRMAS.Add(newFirm);
                                }
                                dbSIM.SaveChanges();
                            }
                            RadMasiva.S_TEMA = datos.TemaMasivo;
                            RadMasiva.CODTRAMITE = datos.CodTramite;
                            RadMasiva.S_ENVIACORREO = datos.EnviarEmail ? "1" : "0";
                            RadMasiva.S_REALIZADO = "0";
                            if (datos.Completo) RadMasiva.S_VALIDADO = "1";
                            dbSIM.Entry(RadMasiva).State = System.Data.Entity.EntityState.Modified;
                            dbSIM.SaveChanges();
                        }
                        else return new { resp = "Error", mensaje = "El proceso de radicación masiva solo puede ser modificado por el funcionario que lo inició!" };
                    }
                    else
                    {
                        RADICACIONMASIVA _masiva = new RADICACIONMASIVA();
                        _masiva.FUNC_ELABORA = funcionario;
                        if (datos.Completo) RadMasiva.S_VALIDADO = "1";
                        _masiva.S_RUTAEXCEL = Excel;
                        _masiva.S_RUTAPLANTILLA = Plantilla;
                        _masiva.IDSOLICITUD = datos.IdSolicitud;
                        _masiva.S_VALIDADO = "0";
                        _masiva.S_REALIZADO = "0";
                        _masiva.CANTIDAD_FILAS = LeeExcelToDataTable(Excel).Rows.Count;
                        _masiva.D_FECHA = DateTime.Now;
                        _masiva.S_TEMA = datos.TemaMasivo;
                        _masiva.S_ENVIACORREO = datos.EnviarEmail ? "1" : "0";
                        _masiva.CODTRAMITE = datos.CodTramite;
                        dbSIM.RADMASIVA.Add(_masiva);
                        dbSIM.SaveChanges();
                        IdMasivo = _masiva.ID;
                        if (datos.Indices != null && datos.Indices.Count > 0)
                        {
                            foreach (var ind in datos.Indices)
                            {
                                RADMASIVAINDICES newInd = new RADMASIVAINDICES
                                {
                                    ID_RADMASIVO = IdMasivo,
                                    CODINDICE = ind.CODINDICE,
                                    S_VALOREXCEL = ind.VALORDEFECTO != "" ? ind.VALORDEFECTO : "",
                                    S_VALORASIGNADO = ind.VALOR != "" ? ind.VALOR : ""
                                };
                                dbSIM.RADMASIVAINDICES.Add(newInd);
                            }
                        }
                        else
                        {
                            var Indices = GetObtenerIndicesSerieDocumental(12);
                            foreach (var ind in Indices)
                            {
                                RADMASIVAINDICES newInd = new RADMASIVAINDICES
                                {
                                    CODINDICE = ind.CODINDICE,
                                    ID_RADMASIVO = IdMasivo,
                                    S_VALOREXCEL = "",
                                    S_VALORASIGNADO = ""
                                };
                                dbSIM.RADMASIVAINDICES.Add(newInd);
                            }
                        }
                        foreach (var fir in datos.Firmas)
                        {
                            RADMASIVAFIRMAS newFirm = new RADMASIVAFIRMAS
                            {
                                ID_RADMASIVO = IdMasivo,
                                FUNC_FIRMA = fir.CodFuncionario,
                                ORDEN_FIRMA = fir.Orden

                            };
                            dbSIM.RADMASIVAFIRMAS.Add(newFirm);
                        }
                        RADMASIVARUTA _ruta = new RADMASIVARUTA
                        {
                            ID_RADMASIVO = IdMasivo,
                            CODFUNCIONARIO = funcionario,
                            FECHA_RUTA = DateTime.Now
                        };
                        dbSIM.RADMASIVARUTA.Add(_ruta);
                        dbSIM.SaveChanges();
                    }
                    if (datos.Completo)
                    {

                        var FuncRuta = dbSIM.RADMASIVAFIRMAS.Where(w => w.ORDEN_FIRMA == 1 && w.ID_RADMASIVO == IdMasivo).Select(s => s.FUNC_FIRMA).FirstOrDefault();
                        if (FuncRuta == 0) return new { resp = "Error", mensaje = "Estableció el proceso como completo pero aún no se han establecido firmas en la plantilla!" };
                        RADMASIVARUTA _ruta = new RADMASIVARUTA
                        {
                            ID_RADMASIVO = IdMasivo,
                            CODFUNCIONARIO = FuncRuta,
                            FECHA_RUTA = DateTime.Now
                        };
                        dbSIM.RADMASIVARUTA.Add(_ruta);
                        dbSIM.SaveChanges();
                    }
                }
                else return new { resp = "Error", mensaje = "Aun no se ha ingresado una identificador para el proceso de radicación masiva!" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Ocurrió el error: " + ex.Message };
            }
            return new { resp = "Ok", mensaje = "Proceso completado correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GuardaFirma")]
        public object PostGuardaFirma(FirmaMasivoDTO firma)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Faltan datos en la solicitud!" };
            try
            {
                if (firma.Firmado)
                {
                    var RadMasiva = dbSIM.RADMASIVA.Where(w => w.IDSOLICITUD == firma.IdSolicitud).FirstOrDefault();
                    if (RadMasiva == null) return new { resp = "Error", mensaje = "Ocurrió el error al consultar el proceso" };
                    var firmaBD = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == RadMasiva.ID && w.FUNC_FIRMA == firma.CodFuncionario).FirstOrDefault();
                    firmaBD.S_FIRMADO = "1";
                    firmaBD.D_FECHAFIRMA = DateTime.Now;
                    firmaBD.S_TIPOFIRMA = firma.TipoFirma;
                    if (firma.Cargo > 0) firmaBD.CODCARGO = firma.Cargo;
                    dbSIM.Entry(firmaBD).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();
                    var firmas = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == RadMasiva.ID && string.IsNullOrEmpty(w.S_FIRMADO)).OrderBy(o => o.ORDEN_FIRMA).ToList();
                    if (firmas.Count == 0)
                    {
                        RADMASIVARUTA ruta = new RADMASIVARUTA();
                        ruta.CODFUNCIONARIO = RadMasiva.FUNC_ELABORA;
                        ruta.ID_RADMASIVO = RadMasiva.ID;
                        ruta.FECHA_RUTA = DateTime.Now;
                        dbSIM.RADMASIVARUTA.Add(ruta);
                        dbSIM.SaveChanges();
                        return new { resp = "Ok", mensaje = "Documento con firmas completas listo para radicar" };
                    }
                    else
                    {
                        RADMASIVARUTA ruta = new RADMASIVARUTA();
                        ruta.CODFUNCIONARIO = firmas[0].FUNC_FIRMA;
                        ruta.ID_RADMASIVO = RadMasiva.ID;
                        ruta.FECHA_RUTA = DateTime.Now;
                        dbSIM.RADMASIVARUTA.Add(ruta);
                        dbSIM.SaveChanges();
                        return new { resp = "Ok", mensaje = "Documento firmado, se avanza para siguiente firma" };
                    }
                }
                else return new { resp = "Error", mensaje = "No se ha aceptado la firma" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Ocurrió el error: " + ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RechazaFirma")]
        public object PostRechazaFirma(FirmaMasivoDTO firma)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Faltan datos en la solicitud!" };
            try
            {
                if (firma.Firmado) return new { resp = "Error", mensaje = "Error en los datos de la solicitud" };
                var RadMasiva = dbSIM.RADMASIVA.Where(w => w.IDSOLICITUD == firma.IdSolicitud).FirstOrDefault();
                if (RadMasiva == null) return new { resp = "Error", mensaje = "Ocurrió el error al consultar el proceso" };
                RADMASIVARUTA ruta = new RADMASIVARUTA();
                ruta.CODFUNCIONARIO = RadMasiva.FUNC_ELABORA;
                ruta.ID_RADMASIVO = RadMasiva.ID;
                ruta.FECHA_RUTA = DateTime.Now;
                ruta.S_COMENTARIO = firma.Comentario;
                dbSIM.RADMASIVARUTA.Add(ruta);
                dbSIM.SaveChanges();
                return new { resp = "Ok", mensaje = "Se avanza al funcionario que elaboró" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Ocurrió el error: " + ex.Message };
            }
        }

        [HttpPost]
        [ActionName("Correo")]
        public async Task<object> PostCorreo(mailDTO dato)
        {
            if (dato.Email.Length == 0) return new { resp = false };
            ApiService apiService = new ApiService();
            try
            {
                var _asunto = "Un asunto de prueba";
                var _radicado = "0344568";
                dato.subject = _asunto != "" ? _asunto : "Sin asunto";
                dato.fromMail = "codelectronicas@metropol.gov.co";
                dato.smtpServer = "smtp.office365.com";
                dato.smtpPort = "587";
                //dato.attachement = _MsPdf;
                dato.attName = _radicado + ".pdf";
                dato.body = "<b>Prueba de envio de correo</b>";
                dato.userPass = "Area2020";

                Response response = await apiService.PostAsync<mailDTO>(urlSIMAPI, "api/", "Terceros/EnviarCorreo", dato);
                if (!response.IsSuccess) return new { resp = false };
                return new { resp = true };
            }
            catch
            {
                return false;
            }
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

        private DataTable LeeExcelToDataTable(string _ruta)
        {
            DataTable dt = new DataTable();
            if (_ruta != null)
            {
                try
                {

                    Workbook wbook = new Workbook();
                    wbook.LoadDocument(_ruta, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    Worksheet worksheet = wbook.Worksheets[0];
                    Range range = worksheet.GetDataRange();

                    dt = worksheet.CreateDataTable(range, true);
                    DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, true);
                    for (int col = 0; col < range.ColumnCount; col++)
                    {
                        CellValueType cellType = range[0, col].Value.Type;
                        for (int r = 1; r < range.RowCount; r++)
                        {
                            if (cellType != range[r, col].Value.Type)
                            {
                                dt.Columns[col].DataType = typeof(string);
                                break;
                            }
                        }
                    }
                    exporter.CellValueConversionError += exporter_CellValueConversionError;
                    exporter.Options.ConvertEmptyCells = true;
                    exporter.Options.SkipEmptyRows = true;
                    exporter.Options.DefaultCellValueToColumnTypeConverter.EmptyCellValue =
                    exporter.Options.DefaultCellValueToColumnTypeConverter.SkipErrorValues = true;
                    exporter.Export();
                }
                catch (Exception ex) { }
            }
            return dt;
        }

        void exporter_CellValueConversionError(object sender, CellValueConversionErrorEventArgs e)
        {
            e.DataTableValue = null;
            e.Action = DataTableExporterAction.Continue;
        }
        private DataTable LeeArchivoExcel(string Identificador)
        {
            DataTable dt = new DataTable();
            if (Identificador != null && Identificador.Length > 0)
            {
                string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
                string _ExcelEncontrado = "";
                string _Dir = _RutaCorrespondencia + @"\" + Identificador;
                if (Directory.Exists(_Dir))
                {
                    var files = Directory.EnumerateFiles(_Dir, "*.*").Where(file => file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx")).ToList();
                    if (files.Count > 0) _ExcelEncontrado = files[0];
                }
                if (_ExcelEncontrado != "")
                {
                    dt = LeeExcelToDataTable(_ExcelEncontrado);
                }
            }
            return dt;
        }

        private string ValidaPlantilla(string IdSolicitud)
        {
            DataTable dtExcel = new DataTable();
            bool _firmas = false;
            string _resp = "";
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            //string[] files;
            string _ExcelEncontrado = "";
            string _PdfEncontrado = "";
            string _Dir = _RutaCorrespondencia + @"\" + IdSolicitud;
            if (Directory.Exists(_Dir))
            {
                var files = Directory.EnumerateFiles(_Dir, "*.*").Where(file => file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx")).ToList();
                if (files.Count > 0) _ExcelEncontrado = files[0];
                files = Directory.EnumerateFiles(_Dir, @"*.pdf").ToList();
                if (files.Count > 0)
                {
                    files = files.Where(val => !val.ToLower().Contains("preview")).ToList();
                    _PdfEncontrado = files[0];
                }
            }
            if (_ExcelEncontrado != "")
            {
                dtExcel = LeeExcelToDataTable(_ExcelEncontrado);
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
                    foreach (System.Text.RegularExpressions.Match match in matchedAuthors)
                    {
                        if (!match.Value.Trim().Substring(1, match.Value.Trim().Length - 2).ToLower().Contains("firma"))
                        {
                            if (!dtExcel.Columns.Contains(match.Value.Trim().Substring(1, match.Value.Trim().Length - 2)))
                            {
                                return $"Error: El archivo de Excel no contiene datos para la clave {match.Value.Trim()} de la platilla pdf!";
                            }
                        }
                        if (match.Value.Trim().Substring(1, match.Value.Trim().Length - 2).ToLower().Contains("firma")) _firmas = true;
                    }
                }
            }
            if (!_firmas) return $"Error: La plantilla no contiene llaves para la o las firmas del documento!";
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
                if (files.Length > 0)
                {
                    files = files.Where((val, idx) => !val.ToLower().Contains("preview")).ToArray();
                    _PdfEncontrado = files[0];
                }
            }
            return _PdfEncontrado;
        }
        private string ObtieneExcel(string Identificador)
        {
            string _ExcelEncontrado = "";
            string _RutaCorrespondencia = SIM.Utilidades.Data.ObtenerValorParametro("RutaCorrespondencia").ToString();
            string _Dir = _RutaCorrespondencia + @"\" + Identificador;
            if (Directory.Exists(_Dir))
            {
                var files = Directory.EnumerateFiles(_Dir, "*.*").Where(file => file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx")).ToList();
                if (files.Count > 0) _ExcelEncontrado = files[0];
            }
            return _ExcelEncontrado;
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
            return ReemplazoDoc(Identificador, Columns, 0);
        }

        private List<ReemplazoDTO> ReemplazoDoc(string Identificador, DataColumnCollection Columns, int Firmas)
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
                for (int j = 1; j <= Firmas; j++)
                {
                    PDFSearchTextResultCollection _result = ip.SearchText("[Firma" + j + "]");
                    if (_result != null && _result.Count > 0)
                    {
                        var encontrado = new ReemplazoDTO()
                        {
                            Pagina = i,
                            CampoReemplazo = "Firma" + j,
                            ListReemplazo = _result
                        };
                        _resp.Add(encontrado);
                    }
                }
            }
            _docPdf.Dispose();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="documento"></param>
        /// <param name="asunto"></param>
        /// <param name="para"></param>
        /// <param name="radicado"></param>
        /// <param name="fechaRad"></param>
        /// <returns></returns>
        private bool EnviarMailMk(string email, byte[] documento, string asunto, string para, string radicado, string fechaRad)
        {
            if (email.Length == 0) return false;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Areas/GestionDocumental/Plantillas/MasivosCOD.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {
                body = body.Replace("[Destinatario]", para);
                body = body.Replace("[Radicado]", radicado);
                body = body.Replace("[FechaRad]", fechaRad);
                string _NombreArchivo = "COD_" + radicado + ".pdf";
                asunto = asunto != "" ? asunto : "Sin asunto";
                if (documento.Length > 0)
                {
                    MemoryStream _anexo = new MemoryStream(documento);
                    try
                    {
                        SIM.Utilidades.EmailMK.EnviarEmail("codelectronicas@metropol.gov.co", email, "codelectronicas@metropol.gov.co", "", asunto, body, "smtp.sendgrid.net", true, "apikey", "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", _anexo, _NombreArchivo);
                        return true;
                    }
                    catch { return false; }
                }
                return false;
            }
            else return false;
        }

        private async Task<bool> EnviarMailSIM(string _email, byte[] _MsPdf, string _asunto, string _para, string _radicado, string _fechaRad)
        {
            if (_email.Length == 0) return false;
            string body = string.Empty;
            ApiService apiService = new ApiService();
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Areas/GestionDocumental/Plantillas/MasivosCOD.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {
                body = body.Replace("[Destinatario]", _para);
                body = body.Replace("[Radicado]", _radicado);
                body = body.Replace("[FechaRad]", _fechaRad);
                try
                {
                    mailDTO _mail = new mailDTO();
                    _mail.Email = _email;
                    _mail.subject = _asunto != "" ? _asunto : "Sin asunto";
                    _mail.fromMail = "codelectronicas@metropol.gov.co";
                    _mail.smtpServer = "smtp.office365.com";
                    _mail.smtpPort = "587";
                    _mail.attachement = _MsPdf;
                    _mail.attName = _radicado + ".pdf";
                    _mail.body = body;
                    _mail.userPass = "Area2020";
                    Response response = await apiService.PostAsync<mailDTO>(urlSIMAPI, "api/", "Terceros/EnviarCorreo", _mail);
                    if (!response.IsSuccess) return false;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        private bool EnviarMailNet(string _email, MemoryStream _MsPdf, string _asunto, string _para, string _radicado, string _fechaRad)
        {
            if (_email.Length == 0) return false;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Areas/GestionDocumental/Plantillas/MasivosCOD.html")))
            {
                body = reader.ReadToEnd();
            }
            if (body.Length > 0)
            {
                body = body.Replace("[Destinatario]", _para);
                body = body.Replace("[Radicado]", _radicado);
                body = body.Replace("[FechaRad]", _fechaRad);
                var Subject = _asunto != "" ? _asunto : "Sin asunto";
                try
                {
                    var respta = SIM.Utilidades.EmailMK.EnviarEmail("codelectronicas@metropol.gov.co", _email, "", "", Subject, body, "smtp.office365.com", true, "codelectronicas@metropol.gov.co", "Area2020", _MsPdf, _radicado + ".pdf");
                    if (respta.StartsWith("[")) return false; else return true;
                }
                catch (SmtpException ex)
                {
                    return false;
                }
            }
            else return false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="documento"></param>
        ///// <param name="asunto"></param>
        ///// <param name="para"></param>
        ///// <param name="radicado"></param>
        ///// <param name="fechaRad"></param>
        ///// <returns></returns>
        //private bool EnviarMail(string email, byte[] documento, string asunto, string para, string radicado, string fechaRad)
        //{
        //    if (email.Length == 0) return false;
        //    string body = string.Empty;
        //    using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Areas/GestionDocumental/Plantillas/MasivosCOD.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    if (body.Length > 0)
        //    {
        //        body = body.Replace("[Destinatario]", para);
        //        body = body.Replace("[Radicado]", radicado);
        //        body = body.Replace("[FechaRad]", fechaRad);
        //        MailMessage correo = new MailMessage();

        //        if (email.Contains(";"))
        //        {
        //            string[] _mails = email.Split(';');
        //            foreach (string _para in _mails) correo.To.Add(new MailAddress(_para));
        //        }
        //        else correo.To.Add(new MailAddress(email));
        //        correo.Subject = asunto;
        //        correo.Body = body;
        //        correo.IsBodyHtml = true;
        //        correo.Priority = MailPriority.High;
        //        if (documento.Length > 0)
        //        {
        //            MemoryStream _anexo = new MemoryStream(documento);
        //            _anexo.Seek(0, SeekOrigin.Begin);
        //            Attachment _File = new Attachment(_anexo, "COD_" + radicado + ".pdf", "application/pdf");
        //            correo.Attachments.Add(_File);
        //        }
        //        SmtpClient smtp = new SmtpClient();
        //        try
        //        {
        //            using (var client = new SmtpClient("smtp.office365.com"))
        //            {
        //                client.UseDefaultCredentials = false;
        //                client.Port = 25;
        //                client.Credentials = new NetworkCredential("codelectronicas@metropol.gov.co", "Area2020");
        //                client.EnableSsl = true;
        //                client.SendMailAsync(correo).Wait(); // Email sent
        //                return true;
        //            }
        //        }
        //        catch (SmtpException ex)
        //        {
        //            return false;
        //        }
        //    }
        //    else return false;
        //}


        private void FirmarPlantilla(ref PDFFile docPdf, List<RADMASIVAFIRMAS> Firmas)
        {
            PDFImportedPage ip = null;
            if (Firmas.Count > 0)
            {
                for (var i = 0; i < docPdf.PagesCount; i++)
                {
                    ip = docPdf.ExtractPage(i);
                    foreach (var firma in Firmas)
                    {
                        PDFSearchTextResultCollection _result = ip.SearchText("[Firma" + firma.ORDEN_FIRMA + "]");
                        if (_result != null && _result.Count > 0)
                        {
                            var pDFTextRun = _result[0].TextRuns[0];
                            Image imagenFirma = Security.ObtenerFirmaElectronicaFuncionario((long)firma.FUNC_FIRMA, true, "");
                            if (imagenFirma != null)
                            {
                                ip.Canvas.DrawImage((Bitmap)imagenFirma, Math.Round(pDFTextRun.DisplayBounds.Left), Math.Round(pDFTextRun.DisplayBounds.Top), 400, 130);
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidMail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
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