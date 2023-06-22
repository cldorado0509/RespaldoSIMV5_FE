using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics.Shapes;
using DevExpress.XtraRichEdit;
using SIM.Areas.Models;
using System.Threading.Tasks;
using SIM.Data;
using PdfSharp.Pdf.IO;
using System.Security.Claims;
using SIM.Data.Tramites;
using System.Web.Http.ModelBinding;

namespace SIM.Utilidades
{
    public class Archivos
    {

        public static void ConvertirAPDF(string rutaOrigen, string rutaDestino)
        {
            Stream stream = ConvertirAPDF(rutaOrigen);
            FileStream fstream = new FileStream(rutaDestino, FileMode.Create, FileAccess.Write);

            stream.CopyTo(fstream);
            fstream.Close();
            stream.Close();
        }

        public static Stream ConvertirAPDF(string rutaOrigen)
        {
            DevExpress.XtraRichEdit.DocumentFormat formato = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
            DevExpress.XtraRichEdit.RichEditDocumentServer server = new DevExpress.XtraRichEdit.RichEditDocumentServer();
            switch (Path.GetExtension(rutaOrigen).Replace(".", "").ToUpper())
            {
                case "DOC":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.Doc;
                    break;
                case "DOCX":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                    break;
                case "RTF":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.Rtf;
                    break;
            }
            server.LoadDocument(rutaOrigen, formato);

            Stream stream = new MemoryStream();
            server.ExportToPdf(stream);
            server.Dispose();

            return stream;
        }

        public static Stream ConvertirAPDF(Stream documentoOrigen, string rutaOrigen)
        {
            DevExpress.XtraRichEdit.DocumentFormat formato = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
            DevExpress.XtraRichEdit.RichEditDocumentServer server = new DevExpress.XtraRichEdit.RichEditDocumentServer();
            switch (Path.GetExtension(rutaOrigen).Replace(".", "").ToUpper())
            {
                case "DOC":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.Doc;
                    break;
                case "DOCX":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                    break;
                case "RTF":
                    formato = DevExpress.XtraRichEdit.DocumentFormat.Rtf;
                    break;
            }
            server.LoadDocument(documentoOrigen, formato);

            Stream stream = new MemoryStream();
            server.ExportToPdf(stream);
            server.Dispose();

            return stream;
        }

        public static void ResizeImage(string rutaOrigen, string rutaDestino, int nuevoAncho, bool borrarOrigen)
        {
            int imageWidth;
            int imageHeight;
            ImageFormat imageFormat;

            System.Drawing.Image image = System.Drawing.Image.FromFile(rutaOrigen);

            image = ExifRotate(image);

            imageWidth = image.Size.Width;
            imageHeight = image.Size.Height;

            if (nuevoAncho < imageWidth)
            {
                switch (rutaDestino.Split('.')[1].ToUpper().Trim())
                {
                    case "GIF":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case "JPEG":
                    case "JPG":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case "PNG":
                        imageFormat = ImageFormat.Png;
                        break;
                    case "BMP":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case "TIFF":
                        imageFormat = ImageFormat.Tiff;
                        break;
                    default:
                        imageFormat = ImageFormat.Jpeg;
                        break;
                }

                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = Convert.ToInt32(nuevoAncho / AspectRatio);
                Bitmap nuevoBitmap = new Bitmap(nuevoAncho, newHeight);
                Graphics nuevoGraph = Graphics.FromImage(nuevoBitmap);
                nuevoGraph.CompositingQuality = CompositingQuality.HighQuality;
                nuevoGraph.SmoothingMode = SmoothingMode.HighQuality;
                nuevoGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, nuevoAncho, newHeight);
                nuevoGraph.DrawImage(image, imageRectangle);
                nuevoBitmap.Save(rutaDestino, imageFormat);
                nuevoGraph.Dispose();
                nuevoBitmap.Dispose();
            }
            else
            {
                File.Copy(rutaOrigen, rutaDestino, true);
            }

            image.Dispose();

            if (borrarOrigen)
                File.Delete(rutaOrigen);
        }

        private static Image ExifRotate(Image img)
        {
            int exifOrientationID = 0x112; //274

            if (!img.PropertyIdList.Contains(exifOrientationID))
                return img;

            var prop = img.GetPropertyItem(exifOrientationID);
            int val = BitConverter.ToUInt16(prop.Value, 0);
            var rot = RotateFlipType.RotateNoneFlipNone;

            if (val == 3 || val == 4)
                rot = RotateFlipType.Rotate180FlipNone;
            else if (val == 5 || val == 6)
                rot = RotateFlipType.Rotate90FlipNone;
            else if (val == 7 || val == 8)
                rot = RotateFlipType.Rotate270FlipNone;

            if (val == 2 || val == 4 || val == 5 || val == 7)
                rot |= RotateFlipType.RotateNoneFlipX;

            if (rot != RotateFlipType.RotateNoneFlipNone)
                img.RotateFlip(rot);

            return img;
        }

        /*public static Stream ConvertirAPDFInterop(string rutaOrigen)
        {
            string output = rutaOrigen.Replace(Path.GetExtension(rutaOrigen), ".pdf");

            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = false;
            object oInput = rutaOrigen;
            object oOutput = output;
            object oFormat = WdSaveFormat.wdFormatPDF;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            DocX document = DocX.Load(output);
            Stream stream = new MemoryStream();

            document.SaveAs(stream);

            return stream;
        }*/

        /// <summary>
        /// Método encargado de Generar la Ruta del Documento perteneciente a un determinado trámite
        /// </summary>
        /// <param name="IdTramite">Identifica el Trámite</param>
        /// <param name="NumeroTramites">Número de Trámites habilitados para guardar  sus documentos en una sola carpeta </param>
        /// <returns></returns>
        public static String GetRutaDocumento(ulong IdTramite, ulong NumeroTramites)
        {
            String Ruta = "";
            ulong Modu;
            int i = 0;
            while (IdTramite > 0)
            {
                Modu = IdTramite % NumeroTramites;
                IdTramite = IdTramite / NumeroTramites;
                if (i > 0)
                {
                    Ruta = "\\" + Modu.ToString().Trim() + Ruta;
                }
                i++;
            }
            Ruta = "0" + Ruta + "\\";
            return Ruta;
        }

        /// <summary>
        /// Convierte un archivo TIF en memoria a un archivo PDF aun en memoria usando PDF4NET
        /// </summary>
        /// <param name="_Tif">Stream con el archivo TIF</param>
        /// <returns></returns>
        public static MemoryStream TifToPDF(MemoryStream _Tif)
        {
            PDFDocument doc = new PDFDocument();
            doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            MemoryStream _Dest = new MemoryStream();
            if (_Tif != null)
            {
                Bitmap bitmap = new Bitmap(_Tif);
                FrameDimension frameDimension = new FrameDimension(bitmap.FrameDimensionsList[0]);
                int framesCount = bitmap.GetFrameCount(frameDimension);
                for (int i = 0; i < framesCount; i++)
                {
                    PDFPage page = doc.AddPage();
                    PDFImage image = new PDFImage(bitmap, i);
                    page.Canvas.DrawImage(image, 0, 0, page.Width, page.Height, 0, PDFKeepAspectRatio.KeepNone);
                }
                doc.Save(_Dest);
            }
            return _Dest;
        }
        /// <summary>
        /// Abre un documento especifico
        /// </summary>
        /// <param name="IdTramite">Codigo del tramite en el sistema</param>
        /// <param name="IdDocumento">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        public static async Task<MemoryStream> AbrirDocumento(long IdTramite, long IdDocumento)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            bool Pdf = false;
            MemoryStream _Ms = new MemoryStream();
            try
            {
                Cryptografia Arc = new Cryptografia();
                string Ruta = "";

                var Docu = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                            where Doc.CODDOCUMENTO == IdDocumento && Doc.CODTRAMITE == IdTramite
                            select Doc).FirstOrDefault();
                if (Docu != null)
                {
                    Ruta = Docu.RUTA;
                    string Extension = Ruta.Trim().Substring(Ruta.Trim().Length - 3, 3);
                    if (Extension.ToLower() == "pdf") Pdf = true;
                    if (Docu.CIFRADO == "1")
                    {
                        if (Docu.MAPAARCHIVO == "M")
                        {
                            byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            _Ms = Arc.DesEncriptar(Ruta, key, iv);
                        }
                        else
                        {
                            _Ms = Arc.DesEncriptarMNP(Ruta, "\tb?Ee??");
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream(Ruta, FileMode.Open))
                        {
                            await stream.CopyToAsync(_Ms);
                        }
                    }
                    if (!Pdf) _Ms = TifToPDF(_Ms);
                }
            }
            catch
            {
                return null;
            }
            return _Ms;
        }
        /// <summary>
        /// Abre un documento especifico
        /// </summary>
        /// <param name="IdTramite">Codigo del tramite en el sistema</param>
        /// <param name="IdDocumento">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        public static async Task<MemoryStream> AbrirDocumento(long IdDocumento)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            bool Pdf = false;
            MemoryStream _Ms = new MemoryStream();
            try
            {
                Cryptografia Arc = new Cryptografia();
                string Ruta = "";

                var Docu = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                            where Doc.ID_DOCUMENTO == IdDocumento
                            select Doc).FirstOrDefault();
                if (Docu != null)
                {
                    Ruta = Docu.RUTA;
                    string Extension = Ruta.Trim().Substring(Ruta.Trim().Length - 3, 3);
                    if (Extension.ToLower() == "pdf") Pdf = true;
                    if (Docu.CIFRADO == "1")
                    {
                        if (Docu.MAPAARCHIVO == "M")
                        {
                            byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            _Ms = Arc.DesEncriptar(Ruta, key, iv);
                        }
                        else
                        {
                            _Ms = Arc.DesEncriptarMNP(Ruta, "\tb?Ee??");
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream(Ruta, FileMode.Open))
                        {
                            await stream.CopyToAsync(_Ms);
                        }
                    }
                    if (!Pdf) _Ms = TifToPDF(_Ms);
                }
            }
            catch
            {
                return null;
            }
            return _Ms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public static async Task<MemoryStream> AbrirDocumentoFun(long IdDocumento, decimal funcionario)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            bool Pdf = false;

            MemoryStream _Ms = new MemoryStream();
            try
            {
                Cryptografia Arc = new Cryptografia();
                string Ruta = "";

                var Docu = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                            where Doc.ID_DOCUMENTO == IdDocumento
                            select Doc).FirstOrDefault();
                if (Docu != null)
                {
                    Ruta = Docu.RUTA;
                    string Extension = Ruta.Trim().Substring(Ruta.Trim().Length - 3, 3);
                    if (Extension.ToLower() == "pdf") Pdf = true;
                    if (Docu.CIFRADO == "1")
                    {
                        if (Docu.MAPAARCHIVO == "M")
                        {
                            byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                            _Ms = Arc.DesEncriptar(Ruta, key, iv);
                        }
                        else
                        {
                            _Ms = Arc.DesEncriptarMNP(Ruta, "\tb?Ee??");
                        }
                    }
                    else
                    {
                        _Ms = new MemoryStream(File.ReadAllBytes(Ruta));    
                        //using (var stream = new FileStream(Ruta, FileMode.Open))
                        //{
                        //    await stream.CopyToAsync(_Ms);
                        //}
                    }
                    if (!Pdf) _Ms = TifToPDF(_Ms);
                    var Vis = new SIM.Data.Tramites.TBVISITADOCUMENTO();
                    Vis.CODDOCUMENTO = Docu.CODDOCUMENTO;
                    Vis.CODTRAMITE = Docu.CODTRAMITE;
                    Vis.CODFUNCIONARIO = funcionario;
                    Vis.FECHA = DateTime.Now;
                    Vis.ID_DOCUMENTO = IdDocumento;
                    dbSIM.TBVISITADOCUMENTO.Add(Vis);
                    dbSIM.SaveChanges();
                }
            }
            catch
            {
                return null;
            }
            return _Ms;
        }

        /// <summary>
        /// Retorna objeto con los datos del documento temporal que se esta abriendo
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        public static async Task<SIM.Controllers.Temporal> AbrirTemporal(long IdDocumento)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            SIM.Controllers.Temporal Resp = new Controllers.Temporal();
            MemoryStream _Ms = new MemoryStream();
            try
            {
                Cryptografia Arc = new Cryptografia();
                string Ruta = "";

                var Docu = (from Doc in dbSIM.DOCUMENTO_TEMPORAL
                            where Doc.ID_DOCUMENTO == IdDocumento
                            select Doc).FirstOrDefault();
                if (Docu != null)
                {
                    Ruta = Docu.S_RUTA;
                    string Extension = Path.GetExtension(Ruta);
                    Resp.fileType = Extension;
                    Resp.filName = Docu.S_RUTA;
                    using (var stream = new FileStream(Ruta, FileMode.Open))
                    {
                            await stream.CopyToAsync(_Ms);
                    }
                    Resp.dataFile = _Ms;
                }
            }
            catch
            {
                return null;
            }
            return Resp;
        }

        /// <summary>
        /// Retorna objeto con los datos del documento temporal que se esta abriendo
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        public static async Task<SIM.Controllers.Temporal> AbrirTemporal(long IdDocumento, decimal CodFuncionario)
        {
            if (IdDocumento == 0 || CodFuncionario == 0 ) return null;
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            SIM.Controllers.Temporal Resp = new Controllers.Temporal();
            MemoryStream _Ms = new MemoryStream();
            try
            {
                Cryptografia Arc = new Cryptografia();
                string Ruta = "";

                var Docu = (from Doc in dbSIM.DOCUMENTO_TEMPORAL
                            where Doc.ID_DOCUMENTO == IdDocumento
                            select Doc).FirstOrDefault();
                if (Docu != null)
                {
                    Ruta = Docu.S_RUTA;
                    string Extension = Path.GetExtension(Ruta);
                    Resp.fileType = Extension;
                    Resp.filName = Docu.S_RUTA;
                    using (var stream = new FileStream(Ruta, FileMode.Open))
                    {
                        await stream.CopyToAsync(_Ms);
                    }
                    Resp.dataFile = _Ms;
                    LOG_TEMPORALES log = new LOG_TEMPORALES();
                    log.CODFUNCIONARIO = CodFuncionario;
                    log.CODTRAMITE = Docu.CODTRAMITE;
                    log.FECHAEVENTO = DateTime.Now;
                    log.ID_DOCUMENTOTEMP = Docu.ID_DOCUMENTO;
                    log.NOMBREARCHIVO = Docu.S_RUTA;
                    log.EVENTO = "El funcionario abrió el documento";
                    dbSIM.LOGTEMPORALES.Add(log);
                    dbSIM.SaveChanges();    
                }
            }
            catch
            {
                return null;
            }

            return Resp;
        }

        /// <summary>
        /// Graba un memorystream a un archivo especifico
        /// </summary>
        /// <param name="ms">El memorystream a grabar</param>
        /// <param name="FileName"></param>
        public static void GrabaMemoryStream(MemoryStream ms, string FileName)
        {
            if (File.Exists(FileName)) File.Delete(FileName);
            FileStream outStream = File.Create(FileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
        }
        /// <summary>
        /// Lee un archivo y retorna un array de bytes
        /// </summary>
        /// <param name="_File">La ruta del archivo a leer</param>
        /// <returns></returns>
        public static byte[] LeeArchivo(string _File)
        {
            if (File.Exists(_File))
                return File.ReadAllBytes(_File);
            else return new byte[] { };
        }
        /// <summary>
        /// Lee un archivo y retorna un array de bytes
        /// </summary>
        /// <param name="fileName">La ruta del archivo a leer</param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCodOrden"></param>
        /// <returns></returns>
        public static MemoryStream AbrirSoporte(long IdCodOrden)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            if (IdCodOrden == 0) return null;
            MemoryStream _Ms = new MemoryStream();
            try
            {
                string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaDocsCorrespondencia").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaDocsCorrespondencia").ToString() : "";
                string Ruta = "";
                byte[] _resp;
                DateTime FechaOrden = (from ModOrden in dbSIM.CORRESPONDENCIAENVIADA
                                       where ModOrden.ID_COD == IdCodOrden
                                       select ModOrden.D_FECHA).FirstOrDefault();
                if (FechaOrden.Year > 1900)
                {
                    Ruta = _RutaBase + FechaOrden.ToString("yyyyMM") + @"\" + IdCodOrden.ToString() + ".pdf";
                    FileInfo _Archivo = new FileInfo(Ruta);
                    if (_Archivo.Exists)
                    {
                        _resp = ReadAllBytes(_Archivo.FullName);
                        _Ms.Write(_resp, 0, _resp.Length);
                    }
                    else return null;
                }
            }
            catch
            {
                return null;
            }
            return _Ms;
        }
        /// <summary>
        /// Convierte un archivo TIF en memoria a un archivo PDF aun en memoria usando DevExpress PdfDocument
        /// </summary>
        /// <param name="_Tif"></param>
        /// <returns></returns>
        public static MemoryStream TifToPDFDE(MemoryStream _Tif)
        {
            MemoryStream _Dest = new MemoryStream();
            if (_Tif != null)
            {
                using (RichEditDocumentServer server = new RichEditDocumentServer())
                {
                    Image TiffImage = Image.FromStream(_Tif);
                    int pageCount = TiffImage.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                    MemoryStream _Bmp;
                    server.CreateNewDocument();
                    for (int pageNum = 0; pageNum < pageCount; pageNum++)
                    {
                        TiffImage.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, pageNum);
                        _Bmp = new MemoryStream();
                        TiffImage.Save(_Bmp, ImageFormat.Bmp);

                        DevExpress.XtraRichEdit.API.Native.DocumentImage docImage = server.Document.Images.Append(DevExpress.XtraRichEdit.API.Native.DocumentImageSource.FromStream(_Bmp));
                        server.Document.Sections[pageNum].Page.Width = docImage.Size.Width + server.Document.Sections[pageNum].Margins.Right + server.Document.Sections[pageNum].Margins.Left;
                        server.Document.Sections[pageNum].Page.Height = docImage.Size.Height + server.Document.Sections[pageNum].Margins.Top + server.Document.Sections[pageNum].Margins.Bottom;
                    }
                    server.ExportToPdf(_Dest);
                    _Dest.Position = 0;
                }
            }
            return _Dest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Archivo"></param>
        /// <param name="_Extension"></param>
        /// <param name="IdTramite"></param>
        /// <param name="_CodProceso"></param>
        /// <returns></returns>
        public static string SubirDocumentoServidorSinCifrar(MemoryStream _Archivo, string _Extension, string IdTramite, long _CodProceso, int CodDoc)
        {
            string Extension = _Extension;
            string Ruta = "";
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var Path = (from Pat in dbSIM.TBRUTAPROCESO
                        where Pat.CODPROCESO == _CodProceso
                        select Pat.PATH).FirstOrDefault();
            if (Path != null)
            {
                Ruta =Path;
                Ruta += "\\" + GetRutaDocumento(ulong.Parse(IdTramite), 100);
                if (!File.Exists(Ruta))
                {
                    Directory.CreateDirectory(Ruta);
                }

                //Copia el Archivo
                Ruta += IdTramite.Trim() + "-" + CodDoc.ToString().Trim() + "." + Extension;

                try
                {
                    FileInfo _Arch = new FileInfo(Ruta);
                    _Arch.Directory.Create();
                    try
                    {
                        if (_Arch.Exists) _Arch.Delete();
                        if (_Archivo.Length > 0)
                        {
                            Cryptografia.GrabaMemoryStream(_Archivo, Ruta);
                        }
                    }
                    catch
                    {
                        Exception ex = new Exception("Se produjo un error al escribir o borrar en el directorio de almacenamiento en el servidor de Archivos");
                        ex.Source = "Cifrado de Archivos";
                        throw ex;
                    }

                    return Ruta;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Exception ex = new Exception("Se debe configurar la Ruta en el Servidor de Archivos para los Documentos de este Tipo de Trámite");
                ex.Source = "Cifrado de Archivos";
                throw ex;
            }
        }
        /// <summary>
        /// Abre un documento cifrado en ruta y lo devuelve en un stream
        /// </summary>
        /// <param name="_Ruta">Ruta donde se encuentra el documento cifrado</param>
        /// <returns></returns>
        public static MemoryStream AbrirDocumentoCifrado(string _Ruta)
        {
            Cryptografia Arc = new Cryptografia();
            MemoryStream _Ms = new MemoryStream();
            if (_Ruta.Length == 0) return _Ms;
            try
            {
                byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                _Ms = Arc.DesEncriptar(_Ruta, key, iv);
            }
            catch
            {
                return _Ms;
            }
            return _Ms;
        }
        /// <summary>
        /// Recibe un documento cifrado en stream y lo devuelve en un stream
        /// </summary>
        /// <param name="_Ruta">Ruta donde se encuentra el documento cifrado</param>
        /// <returns></returns>
        public static MemoryStream AbrirDocumentoCifradoMs(MemoryStream _AuxFile)
        {
            MemoryStream _Ms = new MemoryStream();
            if (_AuxFile.Length == 0) return _Ms;
            try
            {
                byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                _Ms = Cryptografia.DesCifradoMs(_AuxFile, key, iv);
            }
            catch
            {
                return _Ms;
            }
            return _Ms;
        }

        public static void CombinarPDF(string File1, string File2, string output)
        {
            using (PdfSharp.Pdf.PdfDocument one = PdfReader.Open(File1, PdfDocumentOpenMode.Import))
            using (PdfSharp.Pdf.PdfDocument two = PdfReader.Open(File2, PdfDocumentOpenMode.Import))
            using (PdfSharp.Pdf.PdfDocument outPdf = new PdfSharp.Pdf.PdfDocument())
            {
                CopiarPaginasPDF(one, outPdf);
                CopiarPaginasPDF(two, outPdf);

                outPdf.Save(output);
            }
        }

        public static void CopiarPaginasPDF(PdfSharp.Pdf.PdfDocument from, PdfSharp.Pdf.PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
    }
}