using DevExpress.BarCodes;
using DevExpress.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xceed.Words.NET;

namespace SIM.Utilidades
{
    public class Security
    {
        public static string ObtenerHTMLMenu(List<OPCIONMENU> menu)
        {
            return ObtenerHTMLMenu(menu, true);
        }

        public static string ObtenerHTMLMenu(List<OPCIONMENU> menu, bool raiz)
        {
            StringBuilder html = new StringBuilder();

            if (raiz)
                html.Append("<nav id=\"menuSIM\" style=\"display:none\">");
            html.Append("<ul>");

            foreach (OPCIONMENU opcionMenu in menu)
            {
                if (opcionMenu.MENU != null && opcionMenu.MENU.Count > 0)
                {
                    html.Append("<li><span>" + opcionMenu.NOMBRE + "</span>");
                    html.Append(ObtenerHTMLMenu(opcionMenu.MENU, false));
                }
                else
                {
                    html.Append("<li><a href=\"" + opcionMenu.URL + "\">" + opcionMenu.NOMBRE + "</a>");
                }
                html.Append("</li>");
            }

            html.Append("</ul>");
            if (raiz)
                html.Append("</nav>");

            return html.ToString();
        }

        public static void CifrarDocumento(String Archivo, string archivoEncriptado)
        {
            try
            {
                Utilidades.Cryptografia crypt = new Cryptografia();

                byte[] key = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");
                byte[] iv = UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ");

                crypt.Encriptar(Archivo, archivoEncriptado, key, iv);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario"></param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario)
        {
            return ObtenerFirmaElectronicaFuncionario(_CodFuncionario, true, 12, true, "", null);
        }
        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_ConCargo">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="textoAdicional"></param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario, bool _ConCargo, string textoAdicional)
        {
            return ObtenerFirmaElectronicaFuncionario(_CodFuncionario, _ConCargo, 12, false, textoAdicional, null);
        }
        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_ConCargo">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="_TipoFirma">0 Normal, 1 Encargado, 2 Adhoc</param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario, bool _ConCargo, string textoAdicional, int? _CodCargo, int _TipoFirma = 0)
        {
            return ObtenerFirmaElectronicaFuncionario(_CodFuncionario, _ConCargo, 12, false, textoAdicional, _CodCargo, _TipoFirma);
        }
        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_ConCargo">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="_TamanoFuente">Tamaño de la fuente</param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario, bool _ConCargo, float _TamanoFuente, string textoAdicional)
        {
            return ObtenerFirmaElectronicaFuncionario(_CodFuncionario, _ConCargo, _TamanoFuente, false, false, textoAdicional, null);
        }
        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_ConCargo">Tamaño de la fuente</param>
        /// <param name="_TamanoFuente">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="_FuncEncargo">Especifica si el cargo del funcionario es en encargo (E)</param>
        /// <param name="textoAdicional"></param>
        /// <param name="_TipoFirma">0 Normal, 1 Encargado, 2 Adhoc</param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario, bool _ConCargo, float _TamanoFuente, bool _FuncEncargo, string textoAdicional, int? _CodCargo, int _TipoFirma = 0)
        {
            return ObtenerFirmaElectronicaFuncionario(_CodFuncionario, _ConCargo, _TamanoFuente, false, _FuncEncargo, textoAdicional, _CodCargo, _TipoFirma);
        }
        /// <summary>
        /// Obtiene una imagen con la firma del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_TamañoFuente">Tamaño de la fuente</param>
        /// <param name="_ConCargo">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="_ConCodigo">Especifica si se incluye el codigo QR en la firma</param>
        /// <param name="_TipoFirma">0 Normal, 1 Encargado, 2 Adhoc</param>
        /// <param name="textoAdicional"></param>
        /// <param name="_CodCargo"></param>
        /// <param name="_FuncionarioEncargo"></param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerFirmaElectronicaFuncionario(long _CodFuncionario, bool _ConCargo, float _TamañoFuente, bool _ConCodigo, bool _FuncionarioEncargo, string textoAdicional, int? _CodCargo, int _TipoFirma = 0)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var rutaFirmas = dbSIM.Database.SqlQuery<string>("SELECT VALOR FROM GENERAL.PARAMETROS WHERE CLAVE = 'RutaFirmaFuncionario'").FirstOrDefault();
            var nombreFuncionario = dbSIM.Database.SqlQuery<string>(
                "SELECT S_NOMBRES || ' ' || S_APELLIDOS " +
                "FROM SEGURIDAD.USUARIO u INNER JOIN " +
                "SEGURIDAD.USUARIO_FUNCIONARIO uf ON u.ID_USUARIO = uf.ID_USUARIO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault();
            var cedulaFuncionario = dbSIM.Database.SqlQuery<int>(
                "SELECT t.N_DOCUMENTON " +
                "FROM SEGURIDAD.USUARIO_FUNCIONARIO uf INNER JOIN " +
                "SEGURIDAD.PROPIETARIO p ON uf.ID_USUARIO = p.ID_USUARIO INNER JOIN " +
                "GENERAL.TERCERO t ON p.ID_TERCERO = t.ID_TERCERO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault().ToString();
            /*var cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                "SELECT d.S_NOMBRE " +
                "FROM SEGURIDAD.FUNCIONARIO_DEPENDENCIA fd INNER JOIN " +
                "SEGURIDAD.DEPENDENCIA d ON fd.ID_DEPENDENCIA = d.ID_DEPENDENCIA " +
                "WHERE fd.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault();*/
            string cargoFuncionario;

            if (_CodCargo == null)
            {
                cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                    "SELECT CAR.NOMBRE " +
                    "FROM TRAMITES.TBFUNCIONARIO FUN INNER JOIN " +
                    "   TRAMITES.TBCARGO CAR ON FUN.CODCARGO = CAR.CODCARGO " +
                    "WHERE FUN.CODFUNCIONARIO = " + _CodFuncionario.ToString() + "AND ROWNUM = 1").FirstOrDefault();
            }
            else
            {
                if (_TipoFirma == 1)
                {
                    cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT CAR.NOMBRE_ENCARGO " +
                        "FROM TRAMITES.TBCARGO CAR " +
                        "WHERE CAR.CODCARGO = " + _CodCargo.ToString()).FirstOrDefault();
                }
                else
                {
                    cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT CAR.NOMBRE " +
                        "FROM TRAMITES.TBCARGO CAR " +
                        "WHERE CAR.CODCARGO = " + _CodCargo.ToString()).FirstOrDefault();
                }
            }

            if (_CodFuncionario < 0) return null;
            string _FirmaFunc = rutaFirmas + _CodFuncionario.ToString() + ".tif";
            FileInfo _Firma = new FileInfo(_FirmaFunc);
            if (_Firma.Exists)
            {
                Bitmap _ImgFirma;
                if (textoAdicional != null && textoAdicional.Trim() != "")
                {
                    int numLineas = textoAdicional.Count(t => t == '\r');
                    //_ImgFirma = new Bitmap(400, 150 + (textoAdicional.IndexOf('\r') >= 0 ? 20 : 0));
                    _ImgFirma = new Bitmap(400, 150 + (numLineas == 1 ? 20 : (numLineas > 1 ? 40 : 0)));
                }
                else
                    _ImgFirma = new Bitmap(400, 130);

                _ImgFirma.SetResolution(96, 96);

                Graphics canvas = Graphics.FromImage(_ImgFirma);
                Pen blackPen = new Pen(Color.Black, 1);
                canvas.Clear(Color.White);
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Cryptografia crypt = new Cryptografia();
                MemoryStream ms = crypt.DesEncriptar(_FirmaFunc, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));

                //Image img = RedimensionaImagen(Image.FromFile(_Firma.FullName), 220, 100);
                System.Drawing.Image img = RedimensionaImagen(System.Drawing.Image.FromStream(ms), 220, 100);
                canvas.DrawImage(img, new Point(0, 0));
                string _Funcionario = nombreFuncionario;
                Font drawFont = new Font("Arial", _TamañoFuente);
                SolidBrush _Writer = new SolidBrush(Color.Black);
                canvas.DrawString(_Funcionario, drawFont, _Writer, new PointF(0, 95));
                if (_ConCargo)
                {
                    TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                    string _Cargo = cargoFuncionario;
                    //_Cargo = _FuncionarioEncargo ? _Cargo + " (E)" : _Cargo;
                    //_Cargo = (_CodCargo != null) ? _Cargo + (_TipoFirma == 1 ? " (E)" : " Ad Hoc") : _Cargo;
                    _Cargo = (_CodCargo != null) ? _Cargo + (_TipoFirma == 2 ? " Ad Hoc" : "") : _Cargo;
                    canvas.DrawString(texto.ToTitleCase(_Cargo.ToLower()), drawFont, _Writer, new PointF(0, 110));
                }

                if (textoAdicional != null && textoAdicional.Trim() != "")
                {
                    Font drawFontAdicional = new Font("Arial", 10);

                    canvas.DrawString(textoAdicional, drawFontAdicional, _Writer, new PointF(0, 130));
                }

                if (_ConCodigo)
                {
                    string _Clave = _CodFuncionario.ToString() + cedulaFuncionario;
                    BarCode _BarCode = new BarCode();
                    _BarCode.Symbology = Symbology.QRCode;
                    _BarCode.CodeText = "http://webservices.metropol.gov.co/pqrsd/ValidaFuncionario.aspx?" + _CodFuncionario.ToString() + "=" + System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(_Clave, "Sha1");
                    _BarCode.BackColor = Color.White;
                    _BarCode.ForeColor = Color.Black;
                    _BarCode.RotationAngle = 0;
                    _BarCode.CodeBinaryData = Encoding.Default.GetBytes(_BarCode.CodeText);
                    _BarCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
                    _BarCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q;
                    _BarCode.Options.QRCode.ShowCodeText = false;
                    _BarCode.Module = 3;
                    _BarCode.DpiX = 96;
                    _BarCode.DpiY = 96;
                    _BarCode.Unit = GraphicsUnit.Pixel;
                    _BarCode.ImageWidth = 50;
                    _BarCode.ImageHeight = 50;
                    MemoryStream _ms = new MemoryStream();
                    _BarCode.BarCodeImage.Save(_ms, System.Drawing.Imaging.ImageFormat.Png);
                    System.Drawing.Image _imQR = System.Drawing.Image.FromStream(_ms);
                    canvas.DrawImage(_imQR, 320, 40);
                }
                canvas.Save();
                return _ImgFirma;
            }
            else return null;
        }

        /// <summary>
        /// Obtiene una imagen con el nombre del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerNombreFuncionario(long _CodFuncionario)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var rutaFirmas = dbSIM.Database.SqlQuery<string>("SELECT VALOR FROM GENERAL.PARAMETROS WHERE CLAVE = 'RutaFirmaFuncionario'").FirstOrDefault();
            var nombreFuncionario = dbSIM.Database.SqlQuery<string>(
                "SELECT S_NOMBRES || ' ' || S_APELLIDOS " +
                "FROM SEGURIDAD.USUARIO u INNER JOIN " +
                "SEGURIDAD.USUARIO_FUNCIONARIO uf ON u.ID_USUARIO = uf.ID_USUARIO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault();
            var cedulaFuncionario = dbSIM.Database.SqlQuery<int>(
                "SELECT t.N_DOCUMENTON " +
                "FROM SEGURIDAD.USUARIO_FUNCIONARIO uf INNER JOIN " +
                "SEGURIDAD.PROPIETARIO p ON uf.ID_USUARIO = p.ID_USUARIO INNER JOIN " +
                "GENERAL.TERCERO t ON p.ID_TERCERO = t.ID_TERCERO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault().ToString();

            string cargoFuncionario;

            cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                "SELECT CAR.NOMBRE " +
                "FROM TRAMITES.TBFUNCIONARIO FUN INNER JOIN " +
                "   TRAMITES.TBCARGO CAR ON FUN.CODCARGO = CAR.CODCARGO " +
                "WHERE FUN.CODFUNCIONARIO = " + _CodFuncionario.ToString() + "AND ROWNUM = 1").FirstOrDefault();

            if (_CodFuncionario < 0) return null;

            Bitmap _ImgNombreFuncionario;
            _ImgNombreFuncionario = new Bitmap(450, 30);

            _ImgNombreFuncionario.SetResolution(96, 96);

            Graphics canvas = Graphics.FromImage(_ImgNombreFuncionario);
            Pen blackPen = new Pen(Color.Black, 1);
            canvas.Clear(Color.White);
            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

            string _Funcionario = "Proyectado por: " + nombreFuncionario + " (" + cargoFuncionario + ")";
            Font drawFont = new Font("Arial", 6);
            SolidBrush _Writer = new SolidBrush(Color.Black);
            canvas.DrawString(_Funcionario, drawFont, _Writer, new PointF(0, 10));
            canvas.Save();
            return _ImgNombreFuncionario;
        }

        /// <summary>
        /// Obtiene una imagen con el nombre del funcionario
        /// </summary>
        /// <param name="_CodFuncionario">Codigo interno del funcionario</param>
        /// <param name="_ConCargo">Especifica si la firma va con el cargo del funcionario</param>
        /// <param name="_CodCargo">Codigo del cargo del funcionario</param>
        /// <param name="textoAdicional"></param>
        /// <param name="_TipoFirma">0 Normal, 1 Encargado, 2 Adhoc</param>
        /// <returns></returns>
        public static System.Drawing.Image ObtenerNombreFuncionario(long _CodFuncionario,bool _ConCargo, int? _CodCargo, string textoAdicional, int _TipoFirma = 0)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            float _TamañoFuente = 12;
            var rutaFirmas = dbSIM.Database.SqlQuery<string>("SELECT VALOR FROM GENERAL.PARAMETROS WHERE CLAVE = 'RutaFirmaFuncionario'").FirstOrDefault();
            var nombreFuncionario = dbSIM.Database.SqlQuery<string>(
                "SELECT S_NOMBRES || ' ' || S_APELLIDOS " +
                "FROM SEGURIDAD.USUARIO u INNER JOIN " +
                "SEGURIDAD.USUARIO_FUNCIONARIO uf ON u.ID_USUARIO = uf.ID_USUARIO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault();
            var cedulaFuncionario = dbSIM.Database.SqlQuery<int>(
                "SELECT t.N_DOCUMENTON " +
                "FROM SEGURIDAD.USUARIO_FUNCIONARIO uf INNER JOIN " +
                "SEGURIDAD.PROPIETARIO p ON uf.ID_USUARIO = p.ID_USUARIO INNER JOIN " +
                "GENERAL.TERCERO t ON p.ID_TERCERO = t.ID_TERCERO " +
                "WHERE uf.CODFUNCIONARIO = " + _CodFuncionario.ToString()).FirstOrDefault().ToString();

            string cargoFuncionario;

            if (_CodCargo == null)
            {
                cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                    "SELECT CAR.NOMBRE " +
                    "FROM TRAMITES.TBFUNCIONARIO FUN INNER JOIN " +
                    "   TRAMITES.TBCARGO CAR ON FUN.CODCARGO = CAR.CODCARGO " +
                    "WHERE FUN.CODFUNCIONARIO = " + _CodFuncionario.ToString() + "AND ROWNUM = 1").FirstOrDefault();
            }
            else
            {
                if (_TipoFirma == 1)
                {
                    cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT CAR.NOMBRE_ENCARGO " +
                        "FROM TRAMITES.TBCARGO CAR " +
                        "WHERE CAR.CODCARGO = " + _CodCargo.ToString()).FirstOrDefault();
                }
                else
                {
                    cargoFuncionario = dbSIM.Database.SqlQuery<string>(
                        "SELECT CAR.NOMBRE " +
                        "FROM TRAMITES.TBCARGO CAR " +
                        "WHERE CAR.CODCARGO = " + _CodCargo.ToString()).FirstOrDefault();
                }
            }

            if (_CodFuncionario < 0) return null;

            Bitmap _ImgFirma;
            if (textoAdicional != null && textoAdicional.Trim() != "")
            {
                int numLineas = textoAdicional.Count(t => t == '\r');
                //_ImgFirma = new Bitmap(400, 150 + (textoAdicional.IndexOf('\r') >= 0 ? 20 : 0));
                _ImgFirma = new Bitmap(400, 150 + (numLineas == 1 ? 20 : (numLineas > 1 ? 40 : 0)));
            }
            else
                _ImgFirma = new Bitmap(400, 130);

            _ImgFirma.SetResolution(96, 96);

            Graphics canvas = Graphics.FromImage(_ImgFirma);
            Pen blackPen = new Pen(Color.Black, 1);
            canvas.Clear(Color.White);
            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

            string _Funcionario = nombreFuncionario;
            Font drawFont = new Font("Arial", _TamañoFuente);
            SolidBrush _Writer = new SolidBrush(Color.Black);
            canvas.DrawString(_Funcionario, drawFont, _Writer, new PointF(0, 95));
            if (_ConCargo)
            {
                TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                string _Cargo = cargoFuncionario;
                //_Cargo = _FuncionarioEncargo ? _Cargo + " (E)" : _Cargo;
                //_Cargo = (_CodCargo != null) ? _Cargo + (_TipoFirma == 1 ? " (E)" : " Ad Hoc") : _Cargo;
                _Cargo = (_CodCargo != null) ? _Cargo + (_TipoFirma == 2 ? " Ad Hoc" : "") : _Cargo;
                canvas.DrawString(texto.ToTitleCase(_Cargo.ToLower()), drawFont, _Writer, new PointF(0, 110));
            }

            if (textoAdicional != null && textoAdicional.Trim() != "")
            {
                Font drawFontAdicional = new Font("Arial", 10);

                canvas.DrawString(textoAdicional, drawFontAdicional, _Writer, new PointF(0, 130));
            }
            canvas.Save();
            return _ImgFirma;
        }


        /// <summary>
        /// Redimensiona la imágen especificada al ancho y alto.
        /// </summary>
        /// <param name="image">La imágen para redimensionar.</param>
        /// <param name="width">El ancho para la imágen.</param>
        /// <param name="height">El alto para la imágen.</param>
        /// <returns>La imágen redimensionada.</returns>
        public static Bitmap RedimensionaImagen(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, image.PixelFormat);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            image.Dispose();
            return destImage;
        }

        // rutaDocumento: Ruta documento de Word
        public static void FirmarDocumento(string rutaDocumento, int codigoFuncionario)
        {
            int count;

            PdfSharp.Pdf.PdfDocument inputDocument;
            var stream = new MemoryStream();
            int numPaginas = 0;

            // Convertir a PDF, Obtener Coordenas del tag {FIRMA} y eliminar el archivo
            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

            inputDocument = PdfReader.Open(Archivos.ConvertirAPDF(rutaDocumento), PdfDocumentOpenMode.Import);

            count = inputDocument.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                outputDocument.AddPage(page);

                numPaginas++;
            }

            outputDocument.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            outputDocument.Close();
            PdfDocumentProcessor documento = new PdfDocumentProcessor();
            documento.LoadDocument("C:\\Temp\\InformeTecnicoTemp.pdf");
            PdfTextSearchResults result = documento.FindText("FIRMA1");
            //PdfTextSearchResults result2 = documento.FindText("FIRMA2");
            //PdfTextSearchResults result3 = documento.FindText("FIRMA3");
            //PdfTextSearchResults result4 = documento.FindText("FIRMA4");
            documento.CloseDocument();
            File.Delete("C:\\Temp\\InformeTecnicoTemp.pdf");

            // Eliminar el Texto {FIRMA}
            DocX document = DocX.Load("C:\\Temp\\InformeTecnico.docx");
            //document.ReplaceText("{FIRMA}", "");
            document.SaveAs("C:\\Temp\\InformeTecnicoTemp.docx");

            // Convertir a PDF sin el Texto {FIRMA} e insertar la imagen de la firma
            inputDocument = PdfReader.Open(Archivos.ConvertirAPDF("C:\\Temp\\InformeTecnicoTemp.docx"), PdfDocumentOpenMode.Import);
            outputDocument = new PdfSharp.Pdf.PdfDocument();

            count = inputDocument.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                outputDocument.AddPage(page);

                numPaginas++;
            }
            outputDocument.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            outputDocument.Close();

            PdfSharp.Pdf.PdfDocument documentFirma = PdfReader.Open("C:\\Temp\\InformeTecnicoTemp.pdf", PdfDocumentOpenMode.Modify);
            // Get an XGraphics object for drawing
            var alturaPagina = documentFirma.Pages[0].Height.Value;
            XGraphics gfx = XGraphics.FromPdfPage(documentFirma.Pages[result.PageNumber - 1]);
            DrawImage(gfx, Security.ObtenerFirmaElectronicaFuncionario(codigoFuncionario), Convert.ToInt32(result.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(400), Convert.ToInt32(130));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result.Rectangles[0].Left ), Convert.ToInt32((alturaPagina-result.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result2.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result2.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result3.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result3.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            //DrawImage(gfx, "C:\\Temp\\FirmaTransparente.png", Convert.ToInt32(result4.Rectangles[0].Left ), Convert.ToInt32((alturaPagina - result4.Rectangles[0].Top)  - (177 * 0.7) / 2), Convert.ToInt32(249 * 0.7), Convert.ToInt32(177 * 0.7));
            documentFirma.Save("C:\\Temp\\InformeTecnicoTemp.pdf");
            documentFirma.Close();
        }

        static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        static void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }

        /// <summary>
        /// Redcupera el codigo del funcionario a partir de su codigo de usuario
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static long Obtener_Codigo_Funcionario(EntitiesControlOracle bd, long idUsuario)
        {
            ObjectParameter OutCodFuncionario = new ObjectParameter("codFuncionario", typeof(decimal));
            bd.SP_OBTENER_CODFUNCIONARIO(idUsuario, OutCodFuncionario);
            return long.Parse(OutCodFuncionario.Value.ToString());
        }

        /// <summary>
        /// Recupera el codigo del funcionario a partir de su codigo de usuario
        /// </summary>
        /// <param name="idUsuario">Idenbntificador del usuario</param>
        /// <returns></returns>
        public static long Obtener_Codigo_Funcionario(long idUsuario)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            var funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            return funcionario;
        }
    }
}