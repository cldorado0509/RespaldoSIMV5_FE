using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using MailKit.Security;
using Microsoft.AspNet.Identity;
using MimeKit;
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
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
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
                                    var _asunto = "";
                                    var _para = "";
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
                                        if (Ind.INDICE.ToLower().Contains("asunto")) _asunto = Ind.VALOR;
                                        if (Ind.INDICE.ToLower().Contains("destinatario"))
                                        {
                                            if (Ind.VALORDEFECTO != null && Ind.VALORDEFECTO != "") _para = fila[Ind.VALORDEFECTO].ToString();
                                            else _para = Ind.VALOR;
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
                                        if (datos.EnviarEmail)
                                        {
                                            var _email = fila["EMAIL"].ToString();
                                            if (_email.Length > 0)
                                            {
                                                if (!EnviarMailMk(_email, documento.Archivo, _asunto, _para, _Radicado, _FecRad))
                                                {
                                                    fila["Comentarios"] = "Se generó el documento y se radicó, pero no se pudo enviar el email";
                                                }
                                            }
                                            else fila["Comentarios"] = "Se generó el documento y se radicó, pero no se encontró un email para el envío";
                                        }
                                        else fila["Comentarios"] = "Radicado y documento generado correctamente";
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
        public ResponseMassiveDTO PrevisualizaMasivo(MasivoDTO datos)
        {
            MemoryStream streamDoc = new MemoryStream();
            PDFDocument _doc = new PDFDocument();
            if (!ModelState.IsValid) return new ResponseMassiveDTO() { isSuccess = false, message = "Error generando la previsualización del documento" };
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("EditarIndicesMasivo")]
        public dynamic GetEditarIndicesMasivo(string IdSolicitud)
        {
            var indices = (from mas in dbSIM.RADMASIVA
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
            return indices.ToList();
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
            var lista = (from mas in dbSIM.RADMASIVA
                         join rut in dbSIM.RADMASIVARUTA on mas.ID equals rut.ID_RADMASIVO
                         where rut.CODFUNCIONARIO == CodFunc && mas.S_REALIZADO == "0" &&
                         rut.FECHA_RUTA == dbSIM.RADMASIVARUTA.Where(w => w.ID == rut.ID).OrderByDescending(f => f.FECHA_RUTA).Select(s => s.FECHA_RUTA).FirstOrDefault()
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
                             FUNCIONARIOFIRMA = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == mas.ID).Select(s => s.FUNC_FIRMA).FirstOrDefault(),
                             FUNCIONARIOELABORA = mas.FUNC_ELABORA,
                             MENSAJE = rut.S_COMENTARIO
                         }).ToList();

            return JArray.FromObject(lista, Js);
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
                        IdMasivo = RadMasiva.ID;
                        RadMasiva.S_RUTAEXCEL = Excel;
                        RadMasiva.S_RUTAPLANTILLA = Plantilla;
                        if (datos.Indices == null || datos.Indices.Count == 0) RadMasiva.S_VALIDADO = "0";
                        else
                        {
                            RadMasiva.S_VALIDADO = "1";
                            var IndicesBD = dbSIM.RADMASIVAINDICES.Where(w => w.ID_RADMASIVO == RadMasiva.ID).ToList();
                            if (IndicesBD != null && IndicesBD.Count > 0)
                            {
                                foreach (var ind in datos.Indices)
                                {
                                    var IndBd = IndicesBD.Where(i => i.CODINDICE == ind.CODINDICE).FirstOrDefault();
                                    if (IndBd != null)
                                    {
                                        if (ind.VALORDEFECTO != "") IndBd.S_VALOREXCEL = ind.VALORDEFECTO;
                                        else IndBd.S_VALORASIGNADO = ind.VALOR;
                                        dbSIM.Entry(IndBd).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    else
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
                                foreach (var ind in IndicesBD)
                                {
                                    var _ind = datos.Indices.Where(w => w.CODINDICE == ind.CODINDICE).FirstOrDefault();
                                    if (_ind == null) dbSIM.Entry(ind).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                            else
                            {
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
                        }
                        if (datos.Firmas == null || datos.Firmas.Count == 0) RadMasiva.S_VALIDADO = "0";
                        else
                        {
                            RadMasiva.S_VALIDADO = "1";
                            var FirmasBD = dbSIM.RADMASIVAFIRMAS.Where(w => w.ID_RADMASIVO == RadMasiva.ID).ToList();
                            if (FirmasBD != null && FirmasBD.Count > 0)
                            {
                                foreach (var fir in datos.Firmas)
                                {
                                    var FirBd = FirmasBD.Where(i => i.FUNC_FIRMA == fir.CodFuncionario).FirstOrDefault();
                                    if (FirBd != null)
                                    {
                                        FirBd.ORDEN_FIRMA = fir.Orden;
                                        dbSIM.Entry(FirBd).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    else
                                    {
                                        RADMASIVAFIRMAS newFirm = new RADMASIVAFIRMAS
                                        {
                                            FUNC_FIRMA = fir.CodFuncionario,
                                            ORDEN_FIRMA = fir.Orden

                                        };
                                        dbSIM.RADMASIVAFIRMAS.Add(newFirm);
                                    }
                                }
                                foreach (var fir in FirmasBD)
                                {
                                    var _fir = datos.Firmas.Where(w => w.CodFuncionario == fir.FUNC_FIRMA).FirstOrDefault();
                                    if (_fir == null) dbSIM.Entry(fir).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                            else
                            {
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
                            }
                            dbSIM.SaveChanges();
                        }
                        RadMasiva.S_TEMA = datos.TemaMasivo;
                        RadMasiva.S_REALIZADO = "0";
                        dbSIM.Entry(RadMasiva).State = System.Data.Entity.EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        RADICACIONMASIVA _masiva = new RADICACIONMASIVA();
                        _masiva.FUNC_ELABORA = funcionario;
                        if (datos.Indices.Count > 0) _masiva.S_VALIDADO = "1";
                        else _masiva.S_VALIDADO = "0";
                        if (datos.Firmas.Count > 0) _masiva.S_VALIDADO = "1";
                        else _masiva.S_VALIDADO = "0";
                        _masiva.S_RUTAEXCEL = Excel;
                        _masiva.S_RUTAPLANTILLA = Plantilla;
                        _masiva.IDSOLICITUD = datos.IdSolicitud;
                        _masiva.S_REALIZADO = "0";
                        _masiva.CANTIDAD_FILAS = LeeExcel(Excel).Rows.Count;
                        _masiva.D_FECHA = DateTime.Now;
                        _masiva.S_TEMA = datos.TemaMasivo;
                        dbSIM.RADMASIVA.Add(_masiva);
                        dbSIM.SaveChanges();
                        IdMasivo = _masiva.ID;
                        foreach (var ind in datos.Indices)
                        {
                            RADMASIVAINDICES newInd = new RADMASIVAINDICES
                            {
                                ID_RADMASIVO = _masiva.ID,
                                CODINDICE = ind.CODINDICE,
                                S_VALOREXCEL = ind.VALORDEFECTO != "" ? ind.VALORDEFECTO : "",
                                S_VALORASIGNADO = ind.VALOR != "" ? ind.VALOR : ""
                            };
                            dbSIM.RADMASIVAINDICES.Add(newInd);
                        }
                        foreach (var fir in datos.Firmas)
                        {
                            RADMASIVAFIRMAS newFirm = new RADMASIVAFIRMAS
                            {
                                ID_RADMASIVO = _masiva.ID,
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
                        return new { resp = "Ok", mensaje = "Documento con firmado, se avanza para siguiente firma" };
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
                string _ExcelEncontrado = "";
                string _Dir = _RutaCorrespondencia + @"\" + Identificador;
                if (Directory.Exists(_Dir))
                {
                    var files = Directory.EnumerateFiles(_Dir, "*.*").Where(file => file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx")).ToList();
                    if (files.Count > 0) _ExcelEncontrado = files[0];
                }
                if (_ExcelEncontrado != "")
                {
                    dt = LeeExcel(_ExcelEncontrado);
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
                var correo = new MimeMessage();

                if (email.Contains(";"))
                {
                    string[] _mails = email.Split(';');
                    foreach (string _para in _mails) correo.To.Add(MailboxAddress.Parse(_para));
                }
                else correo.To.Add(MailboxAddress.Parse(email));
                correo.Subject = asunto != "" ? asunto : "Sin asunto";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                correo.Priority = MessagePriority.Urgent;
                if (documento.Length > 0)
                {
                    MemoryStream _anexo = new MemoryStream(documento);
                    _anexo.Seek(0, SeekOrigin.Begin);
                    //Attachment _File = new Attachment(_anexo, "COD_" + radicado + ".pdf", "application/pdf");
                    //correo.Attachments.Add(_File);
                    bodyBuilder.Attachments.Add("Receipt.pdf", _anexo);
                }
                correo.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("codelectronicas@metropol.gov.co", "Area2020");
                    smtp.Send(correo);
                    smtp.Disconnect(true);
                    return true;
                }
            }
            else return false;
        }

        private bool EnviarMailPrueba(string email, byte[] documento, string asunto, string para, string radicado, string fechaRad)
        {
            if (email.Length == 0) return false;
            string body = string.Empty;
            body = body.Replace("[Destinatario]", para);
            body = body.Replace("[Radicado]", radicado);
            body = body.Replace("[FechaRad]", fechaRad);
            var correo = new MimeMessage();

            correo.To.Add(MailboxAddress.Parse(email));
            correo.Subject = asunto;
            correo.Priority = MessagePriority.Urgent;
            correo.Body = new TextPart("hola, esto es una prueba de envio de correo!!!!");
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("codelectronicas@metropol.gov.co", "Area2020");
                smtp.Send(correo);
                smtp.Disconnect(true);
                return true;
            }
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