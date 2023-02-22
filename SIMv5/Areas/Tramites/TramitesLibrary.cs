using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.Drawing;
using SIM.Data;
using SIM.Areas.Tramites.Models;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Core.Objects;
using SIM.Utilidades;
using PdfSharp.Pdf.IO;
using DevExpress.Pdf;
using O2S.Components.PDF4NET.PDFFile;
using AreaMetro.Seguridad;
using DevExpress.BarCodes;
using System.Globalization;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Web.Hosting;
using System.Drawing.Imaging;
using PdfSharp.Drawing;
using SIM.Data.Tramites;
using SIM.Models;
using PdfSharp.Drawing.Layout;
using System.Security.Claims;
using co.com.certicamara.encryption3DES.code;

namespace SIM.Areas.Tramites
{
    public struct DatosAvanzaTareaTramite
    {
        public int tipo; //
        public string codTramites;
        public int codTarea;
        public int codTareaSiguiente;
        public int codFuncionario;
        public string copias;
        public int? idGrupo;
        public string comentario;
    }

    public struct DatosAvanzaTareaTramiteFormulario
    {
        public string codTramites;
        public int codTarea;
        public int codTareaSiguiente;
        public int codFuncionario;
        public string formularioSiguiente;
        public string copias;
        public int? idGrupo;
        public string comentario;
    }

    public struct Respuesta
    {
        public string tipoRespuesta; // OK, Error
        public string detalleRespuesta;
        public string datosAdicionales;
    }

    public class TramitesLibrary
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        public Respuesta AvanzaTareaTramite(DatosAvanzaTareaTramite datosAvanzaTareaTramite)
        {
            bool resultado = true;
            string erroresAvanza = "";
            string parametros;
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            string funcionariosGrupo = "";

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

            if (datosAvanzaTareaTramite.idGrupo != null && datosAvanzaTareaTramite.idGrupo != -1)
            {
                funcionariosGrupo = String.Join(",", dbSIM.Database.SqlQuery<int>(
                        "SELECT td.CODTRAMITE, td.CODDOCUMENTO " +
                        "FROM TRAMITES.MEMORANDO_FUNCGRUPO " +
                        "WHERE ID_GRUPOMEMO = " + datosAvanzaTareaTramite.idGrupo.ToString()).ToList());

                if (funcionariosGrupo.Trim() == ",")
                    funcionariosGrupo = "";
            }

            try
            {
                foreach (string codTramite in datosAvanzaTareaTramite.codTramites.Split(','))
                {
                    parametros = "";
                    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                    if (codTramite.Contains("|"))
                    {
                        var codTramiteSel = codTramite.Split('|')[0];
                        var codTareaSel = codTramite.Split('|')[1];

                        parametros = "[" + datosAvanzaTareaTramite.tipo.ToString() + "," + codTramiteSel + ",0," + codTareaSel + "," + datosAvanzaTareaTramite.codFuncionario.ToString() + "]";

                        dbTramites.SP_AVANZA_TAREA(datosAvanzaTareaTramite.tipo, Convert.ToInt32(codTramiteSel), 0, Convert.ToInt32(codTareaSel), datosAvanzaTareaTramite.codFuncionario, codFuncionario, datosAvanzaTareaTramite.copias + (datosAvanzaTareaTramite.copias.Trim() == "" ? funcionariosGrupo : (funcionariosGrupo != "" ? "," + funcionariosGrupo : "")), datosAvanzaTareaTramite.comentario, rtaResultado);
                    }
                    else
                    {
                        parametros = "[" + datosAvanzaTareaTramite.tipo.ToString() + "," + codTramite + "," + datosAvanzaTareaTramite.codTarea.ToString() + "," + datosAvanzaTareaTramite.codTareaSiguiente.ToString() + "," + datosAvanzaTareaTramite.codFuncionario.ToString() + "]";

                        dbTramites.SP_AVANZA_TAREA(datosAvanzaTareaTramite.tipo, Convert.ToInt32(codTramite), datosAvanzaTareaTramite.codTarea, datosAvanzaTareaTramite.codTareaSiguiente, datosAvanzaTareaTramite.codFuncionario, codFuncionario, datosAvanzaTareaTramite.copias + (datosAvanzaTareaTramite.copias.Trim() == "" ? funcionariosGrupo : (funcionariosGrupo != "" ? "," + funcionariosGrupo : "")), datosAvanzaTareaTramite.comentario, rtaResultado);
                    }

                    if (rtaResultado.Value.ToString() != "OK")
                    {
                        resultado = false;

                        erroresAvanza += parametros + " -> " + rtaResultado.Value + "\r\n";
                    }
                }

                if (resultado)
                    return new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "Trámites Avanzados Satisfactoriamente." };
                else
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Errores (AvanzaTareaTramite):\r\n" + erroresAvanza);
                    return new Respuesta() { tipoRespuesta = "ERROR", detalleRespuesta = "Por lo menos un Trámite no fue avanzado Satisfactoriamente" };
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Errores (AvanzaTareaTramite):\r\n" + Utilidades.LogErrores.ObtenerError(error));
                return new Respuesta() { tipoRespuesta = "ERROR", detalleRespuesta = "Por lo menos un Trámite no fue avanzado Satisfactoriamente.\r\n" + error.Message };
            }
        }

        public Respuesta AvanzaTareaTramiteFormulario(DatosAvanzaTareaTramiteFormulario datosAvanzaTareaTramiteFormulario)
        {
            bool resultado = true;
            string erroresAvanza = "";
            string parametros;
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            string funcionariosGrupo = "";

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

            if (datosAvanzaTareaTramiteFormulario.idGrupo != null && datosAvanzaTareaTramiteFormulario.idGrupo != -1)
            {
                funcionariosGrupo = String.Join(",", dbSIM.Database.SqlQuery<int>(
                        "SELECT td.CODTRAMITE, td.CODDOCUMENTO " +
                        "FROM TRAMITES.MEMORANDO_FUNCGRUPO " +
                        "WHERE ID_GRUPOMEMO = " + datosAvanzaTareaTramiteFormulario.idGrupo.ToString()).ToList());

                if (funcionariosGrupo.Trim() == ",")
                    funcionariosGrupo = "";
            }

            try
            {
                foreach (string codTramite in datosAvanzaTareaTramiteFormulario.codTramites.Split(','))
                {
                    parametros = "";

                    ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

                    if (codTramite.Contains("|"))
                    {
                        var codTramiteSel = codTramite.Split('|')[0];
                        var codTareaSel = codTramite.Split('|')[1];

                        parametros = "[" + codTramiteSel + "," + datosAvanzaTareaTramiteFormulario.codTarea.ToString() + "," + codTareaSel + "," + datosAvanzaTareaTramiteFormulario.codFuncionario.ToString() + "," + datosAvanzaTareaTramiteFormulario.formularioSiguiente + "]";

                        dbTramites.SP_AVANZA_TAREA_FORMULARIO(Convert.ToInt32(codTramiteSel), datosAvanzaTareaTramiteFormulario.codTarea, Convert.ToInt32(codTareaSel), datosAvanzaTareaTramiteFormulario.codFuncionario, datosAvanzaTareaTramiteFormulario.formularioSiguiente, datosAvanzaTareaTramiteFormulario.copias + (datosAvanzaTareaTramiteFormulario.copias.Trim() == "" ? funcionariosGrupo : (funcionariosGrupo != "" ? "," + funcionariosGrupo : "")), datosAvanzaTareaTramiteFormulario.comentario, rtaResultado);
                    }
                    else
                    {
                        parametros = "[" + codTramite + "," + datosAvanzaTareaTramiteFormulario.codTarea.ToString() + "," + datosAvanzaTareaTramiteFormulario.codTareaSiguiente.ToString() + "," + datosAvanzaTareaTramiteFormulario.codFuncionario.ToString() + "," + datosAvanzaTareaTramiteFormulario.formularioSiguiente + "]";

                        dbTramites.SP_AVANZA_TAREA_FORMULARIO(Convert.ToInt32(codTramite), datosAvanzaTareaTramiteFormulario.codTarea, datosAvanzaTareaTramiteFormulario.codTareaSiguiente, datosAvanzaTareaTramiteFormulario.codFuncionario, datosAvanzaTareaTramiteFormulario.formularioSiguiente, datosAvanzaTareaTramiteFormulario.copias + (datosAvanzaTareaTramiteFormulario.copias.Trim() == "" ? funcionariosGrupo : (funcionariosGrupo != "" ? "," + funcionariosGrupo : "")), datosAvanzaTareaTramiteFormulario.comentario, rtaResultado);
                    }


                    if (rtaResultado.Value.ToString() != "OK")
                    {
                        resultado = false;

                        erroresAvanza += parametros + " -> " + rtaResultado.Value + "\r\n";
                    }
                }

                if (resultado)
                    return new Respuesta() { tipoRespuesta = "OK", detalleRespuesta = "Trámites Avanzados Satisfactoriamente." };
                else
                {
                    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Errores (AvanzaTareaTramiteFormulario):\r\n" + erroresAvanza);
                    return new Respuesta() { tipoRespuesta = "ERROR", detalleRespuesta = "Por lo menos un Trámite no fue avanzado Satisfactoriamente" };
                }
            }
            catch (Exception error)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Errores (AvanzaTareaTramiteFormulario):\r\n" + Utilidades.LogErrores.ObtenerError(error));

                return new Respuesta() { tipoRespuesta = "ERROR", detalleRespuesta = "Por lo menos un Trámite no fue avanzado Satisfactoriamente.\r\n" + error.Message };
            }
        }

        public dynamic DocumentoRadicado(int id, bool radicado, int idRadicado, bool watermark, string textoFirma)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            Encrypt3DES ed3des = new Encrypt3DES();
            int numPaginas = 0;
            var streamDocumento = new MemoryStream();

            PROYECCION_DOC proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();

            if (proyeccionDocumento != null)
            {
                //PdfDocumentProcessor documento = new PdfDocumentProcessor();
                PdfSharp.Pdf.PdfDocument documento = new PdfSharp.Pdf.PdfDocument();

                // Se carga el documento principal

                PROYECCION_DOC_DET_ARCH archivoPrincipal = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                                            where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 1 && da.S_ACTIVO == "S"
                                                            select da).FirstOrDefault();

                //var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id && pf.S_ESTADO == "S");
                var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id);
                if (radicado) firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id && pf.S_TIPO=="Firma");

                if (archivoPrincipal != null)
                {
                    if (Path.GetExtension(archivoPrincipal.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                    {
                        MemoryStream streamDocumentoWord;
                        streamDocumentoWord = (MemoryStream)Archivos.ConvertirAPDF(archivoPrincipal.S_RUTA_ARCHIVO);

                        var inputDocument = PdfReader.Open(streamDocumentoWord, PdfDocumentOpenMode.Import);
                        var count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                            documento.AddPage(page);

                            numPaginas++;
                        }

                        streamDocumentoWord.Close();
                        streamDocumentoWord.Dispose();
                    }
                    else
                    {
                        PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
                        documentoDE.LoadDocument(archivoPrincipal.S_RUTA_ARCHIVO, true);

                        MemoryStream documentoDEPDF = new MemoryStream();
                        documentoDE.SaveDocument(documentoDEPDF);
                        documentoDE.CloseDocument();

                        //var inputDocument = PdfReader.Open(archivoPrincipal.S_RUTA_ARCHIVO, PdfDocumentOpenMode.Import);
                        var inputDocument = PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);
                        var count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                            documento.AddPage(page);

                            numPaginas++;
                        }

                        inputDocument.Close();
                        inputDocument.Dispose();
                        documentoDEPDF.Close();
                        documentoDEPDF.Dispose();
                    }

                    numPaginas = documento.Pages.Count;

                    PdfDocumentProcessor documentoRadicado = new PdfDocumentProcessor();
                    documentoRadicado.LoadDocument(archivoPrincipal.S_RUTA_ARCHIVO, true);

                    foreach (PROYECCION_DOC_FIRMAS firma in firmas)
                    {
                        PdfTextSearchResults ubicacionFirma = documentoRadicado.FindText("[Firma" + firma.N_ORDEN.ToString() + "]");

                        if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                        {
                            // Dibujo Firmas
                            Image imagenFirma;
                            if (firma.CODCARGO == null)
                                imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, (textoFirma.Trim() != "" ? textoFirma.Trim() + " el " + ((DateTime)firma.D_FECHA_FIRMA).ToString("dd/MM/yyyy") : "") + (firma.S_APRUEBA == "S" ? "\r\nAprobó" : "") + (firma.S_REVISA == "S" ? "\r\nRevisó" : ""));
                            else
                                imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, (textoFirma.Trim() != "" ? textoFirma.Trim() + " el " + ((DateTime)firma.D_FECHA_FIRMA).ToString("dd/MM/yyyy") : "") + (firma.S_APRUEBA == "S" ? "\r\nAprobó" : "") + (firma.S_REVISA == "S" ? "\r\nRevisó" : ""), firma.CODCARGO, (firma.S_TIPOFIRMA == "E" ? 1 : (firma.S_TIPOFIRMA == "A" ? 2 : 0)));

                            //Image imagenFirma = (new AreaMetro.Seguridad.FuncionarioC()).ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true);

                            if (imagenFirma != null)
                            {
                                float topPagina = (float)ubicacionFirma.Page.CropBox.Top;

                                /*using (PdfGraphics graphics = documento.CreateGraphics())
                                {
                                    graphics.FillRectangle(Brushes.White, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, (float)ubicacionFirma.Rectangles[0].Width, (float)ubicacionFirma.Rectangles[0].Height));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);

                                    graphics.DrawImage(imagenFirma, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, 240, 78));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);
                                }*/


                                XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber-1]);
                                graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top)-2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height)+2));
                                //DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionFirma.Rectangles[0].Top), 240, 78);
                                DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionFirma.Rectangles[0].Top), Convert.ToInt32(imagenFirma.Width*(240/Convert.ToDecimal(imagenFirma.Width))), Convert.ToInt32(imagenFirma.Height*(240/ Convert.ToDecimal(imagenFirma.Width))));
                                graphics.Dispose();
                            }
                        }
                    }

                    PdfTextSearchResults ubicacionProyecta = documentoRadicado.FindText("Proyecta]");

                    if (ubicacionProyecta.Status == PdfTextSearchStatus.Found)
                    {
                        var imagenProyecta = Security.ObtenerNombreFuncionario(proyeccionDocumento.CODFUNCIONARIO);

                        float topPagina = (float)ubicacionProyecta.Page.CropBox.Top;

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionProyecta.PageNumber - 1]);
                        graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionProyecta.Rectangles[0].Left-5), Convert.ToInt32(topPagina - (float)ubicacionProyecta.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionProyecta.Rectangles[0].Width+5), Convert.ToInt32(ubicacionProyecta.Rectangles[0].Height) + 2));
                        //graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionProyecta.Rectangles[0].Left - 5), Convert.ToInt32(topPagina - (float)ubicacionProyecta.Rectangles[0].Top) - 2, 450, 30));
                        //DrawImage(graphics, imagenProyecta, Convert.ToInt32(ubicacionProyecta.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionProyecta.Rectangles[0].Top), 450, 30);
                        DrawImage(graphics, imagenProyecta, Convert.ToInt32(ubicacionProyecta.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionProyecta.Rectangles[0].Top), Convert.ToInt32(imagenProyecta.Width * (340 / Convert.ToDecimal(imagenProyecta.Width))), Convert.ToInt32(imagenProyecta.Height * (340 / Convert.ToDecimal(imagenProyecta.Width))));
                        graphics.Dispose();
                    }

                    if (radicado)
                    {
                        MemoryStream imagenEtiqueta = new MemoryStream();

                        Radicador radicador = new Radicador();
                        var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
                        imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[0]);
                        DrawImage(graphics, imagenRadicado, 300, 30, 288, 72);
                        graphics.Dispose();

                        string seriesDobleEtiqueta = ConfigurationManager.AppSettings["SeriesProyeccionDobleEtiqueta"];

                        if (seriesDobleEtiqueta != null && seriesDobleEtiqueta.Trim() != "" && ("," + seriesDobleEtiqueta.Replace(" ", "") + ",").Contains("," + proyeccionDocumento.CODSERIE.ToString() + ","))
                        {
                            if (documento.Pages.Count > 1)
                            {
                                graphics = XGraphics.FromPdfPage(documento.Pages[documento.Pages.Count - 1]);
                                DrawImage(graphics, imagenRadicado, 300, 30, 288, 72);
                                graphics.Dispose();
                            }
                        }
                    }

                    PdfTextSearchResults ubicacionTramites = documentoRadicado.FindText("[Tramites]");

                    if (ubicacionTramites.Status == PdfTextSearchStatus.Found)
                    {
                        /*Image imagenFirma = ObtenerImagenTextoTramites(proyeccionDocumento.S_TRAMITES);

                        float topPagina = (float)ubicacionTramites.Page.CropBox.Top;

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionTramites.PageNumber - 1]);
                        graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionTramites.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionTramites.Rectangles[0].Width), Convert.ToInt32(ubicacionTramites.Rectangles[0].Height) + 2));
                        DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionTramites.Rectangles[0].Top), Convert.ToInt32(Convert.ToDouble(imagenFirma.Width)*0.7), Convert.ToInt32(Convert.ToDouble(imagenFirma.Height)*0.7));
                        
                        graphics.Dispose();*/

                        float topPagina = (float)ubicacionTramites.Page.CropBox.Top;

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionTramites.PageNumber - 1]);

                        graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionTramites.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionTramites.Rectangles[0].Width), Convert.ToInt32(ubicacionTramites.Rectangles[0].Height) + 2));

                        XFont font = new XFont("Arial", 9, XFontStyle.Regular);
                        var formatter = new XTextFormatter(graphics);

                        List<string> lineas;

                        if (proyeccionDocumento.S_TRAMITES != null && proyeccionDocumento.S_TRAMITES.Trim() != "")
                        {
                            lineas = LineasTextoTramitesPdf(proyeccionDocumento.S_TRAMITES, font, graphics);
                        }
                        else
                        {
                            lineas = new List<string>();

                            lineas.Add("Trámites:");
                            lineas.Add("Nuevo Trámite.");
                        }


                        int cont = 0;
                        foreach (string linea in lineas)
                        {
                            var layoutRectangle = new XRect(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionTramites.Rectangles[0].Top) + cont*10, 450, 10);

                            formatter.DrawString(linea, font, XBrushes.Black, layoutRectangle, XStringFormats.TopLeft);
                            cont++;
                        }

                        graphics.Dispose();
                    }
                }
                else
                {
                    return null;
                }

                var archivosAdjuntos = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                        where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 2 && da.PROYECCION_DOC_ARCHIVOS.S_ACTIVO == "S" && da.S_ACTIVO == "S"
                                        select da);

                if (archivosAdjuntos != null)
                {
                    foreach (var archivoAdjunto in archivosAdjuntos)
                    {
                        if (Path.GetExtension(archivoAdjunto.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                        {
                            MemoryStream streamDocumentoWord;
                            streamDocumentoWord = (MemoryStream)Archivos.ConvertirAPDF(archivoAdjunto.S_RUTA_ARCHIVO);

                            var inputDocument = PdfReader.Open(streamDocumentoWord, PdfDocumentOpenMode.Import);
                            var count = inputDocument.PageCount;
                            for (int idx = 0; idx < count; idx++)
                            {
                                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                                documento.AddPage(page);

                                numPaginas++;
                            }

                            streamDocumentoWord.Close();
                            streamDocumentoWord.Dispose();
                        }
                        else
                        {
                            PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
                            documentoDE.LoadDocument(archivoAdjunto.S_RUTA_ARCHIVO, true);

                            MemoryStream documentoDEPDF = new MemoryStream();
                            documentoDE.SaveDocument(documentoDEPDF);
                            documentoDE.CloseDocument();

                            var inputDocument = PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);

                            //var inputDocument = PdfReader.Open(archivoAdjunto.S_RUTA_ARCHIVO, PdfDocumentOpenMode.Import);

                            var count = inputDocument.PageCount;
                            for (int idx = 0; idx < count; idx++)
                            {
                                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                                documento.AddPage(page);

                                numPaginas++;
                            }

                            inputDocument.Close();
                            inputDocument.Dispose();
                            documentoDEPDF.Close();
                            documentoDEPDF.Dispose();
                        }

                        numPaginas = documento.Pages.Count;
                    }
                }

                if (watermark)
                    AddWatermark("BORRADOR", documento);
                MemoryStream ms = new MemoryStream();
                MemoryStream msFirmado = new MemoryStream();
                documento.Save(ms);
                bool _FirmadoDig = true;
                string errorFirma = "";
                if (radicado)
                {
                    PdfDocumentProcessor documentoFirma = new PdfDocumentProcessor();

                    firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id && pf.S_TIPO == "Digital").OrderBy(o => o.N_ORDEN);
                    foreach (PROYECCION_DOC_FIRMAS firma in firmas)
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        documentoFirma.LoadDocument(ms, true);
                        PdfTextSearchResults ubicacionFirma = documentoFirma.FindText("[Firma" + firma.N_ORDEN.ToString() + "]");

                        if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                        {
                            // Dibujo Firmas
                            Image imagenFirma = null;
                            imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, "");
                            MemoryStream msFirma = new MemoryStream();
                            int PagFirma = -1;
                            string CampoFirmaDig = "";
                            if (imagenFirma != null)
                            {
                                float topPagina = (float)ubicacionFirma.Page.CropBox.Top;
                                XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber - 1]);
                                graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height) + 2));
                                graphics.Dispose();
                                imagenFirma.Save(msFirma, ImageFormat.Png);
                                msFirma.Seek(0, SeekOrigin.Begin);
                                CampoFirmaDig = "[Firma" + firma.N_ORDEN.ToString() + "]";
                                PagFirma = ubicacionFirma.PageNumber;
                            }
                            //Firma digital del documento
                            string _pass = ed3des.decrypt(firma.TMP_PASS);
                            var _usr = (from U in dbSIM.TBFUNCIONARIO where U.CODFUNCIONARIO == firma.CODFUNCIONARIO select U.USUARIO_FIRMA).FirstOrDefault();
                            Utilidades.FirmaDigital.RespuestaFirma _Rpta = Utilidades.FirmaDigital.FirmaProyeccion.FirmaDocumento(ms.ToArray(), msFirma.ToArray(), PagFirma, CampoFirmaDig, _usr, _pass).Result;

                            if (_Rpta.Exito)
                            {
                                msFirmado = new MemoryStream(_Rpta.ArchivoFirmado);
                                ms = new MemoryStream(_Rpta.ArchivoFirmado);
                                firma.TMP_PASS = "";
                                dbSIM.Entry(firma).State = System.Data.Entity.EntityState.Modified;
                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                var nombreFuncionario = (from U in dbSIM.TBFUNCIONARIO where U.CODFUNCIONARIO == firma.CODFUNCIONARIO select U.NOMBRES + " " + U.APELLIDOS).FirstOrDefault();

                                if (nombreFuncionario == null || nombreFuncionario.Trim() == "")
                                    errorFirma = "[" + id.ToString() + "]" + _Rpta.Mensaje;
                                else
                                    errorFirma = "(" + nombreFuncionario + ") [" + id.ToString() + "]" + _Rpta.Mensaje;

                                _FirmadoDig = false;
                                break;
                            }
                        }
                    }
                }
                if (!_FirmadoDig)
                {
                    return new { exito = false, mensaje = errorFirma };
                }
                else
                {
                    var documentoRetorno = msFirmado.Length > 0 ? msFirmado.ToArray() : ms.ToArray();

                    documento.Close();
                    ms.Close();
                    ms.Dispose();

                    return new { exito = true, documentoBinario = documentoRetorno, numPaginas = numPaginas };
                }
            }
            else
            {
                return null;
            }
        }

        public dynamic DocumentoRadicadoFirmaDigital(int id, bool radicado, int idRadicado, string textoFirma, string usuario, string pass, int orden)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            int numPaginas = 0;
            var streamDocumento = new MemoryStream();

            PROYECCION_DOC proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();

            if (proyeccionDocumento != null)
            {
                int PagFirma = -1;
                string CampoFirmaDig = "";
                Image FirmaDigital = null;
                MemoryStream msFirma = new MemoryStream();

                //PdfDocumentProcessor documento = new PdfDocumentProcessor();
                PdfSharp.Pdf.PdfDocument documento = new PdfSharp.Pdf.PdfDocument();

                // Se carga el documento principal

                PROYECCION_DOC_DET_ARCH archivoPrincipal = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                                            where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 1 && da.S_ACTIVO == "S"
                                                            select da).FirstOrDefault();

                //var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id && pf.S_ESTADO == "S");
                var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id).OrderBy(o => o.N_ORDEN);

                if (archivoPrincipal != null)
                {
                    if (Path.GetExtension(archivoPrincipal.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                    {
                        MemoryStream streamDocumentoWord;
                        streamDocumentoWord = (MemoryStream)Archivos.ConvertirAPDF(archivoPrincipal.S_RUTA_ARCHIVO);

                        var inputDocument = PdfReader.Open(streamDocumentoWord, PdfDocumentOpenMode.Import);
                        var count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                            documento.AddPage(page);

                            numPaginas++;
                        }

                        streamDocumentoWord.Close();
                        streamDocumentoWord.Dispose();
                    }
                    else
                    {
                        PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
                        documentoDE.LoadDocument(archivoPrincipal.S_RUTA_ARCHIVO, true);

                        MemoryStream documentoDEPDF = new MemoryStream();
                        documentoDE.SaveDocument(documentoDEPDF);
                        documentoDE.CloseDocument();

                        //var inputDocument = PdfReader.Open(archivoPrincipal.S_RUTA_ARCHIVO, PdfDocumentOpenMode.Import);
                        var inputDocument = PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);
                        var count = inputDocument.PageCount;
                        for (int idx = 0; idx < count; idx++)
                        {
                            PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                            documento.AddPage(page);

                            numPaginas++;
                        }

                        inputDocument.Close();
                        inputDocument.Dispose();
                        documentoDEPDF.Close();
                        documentoDEPDF.Dispose();
                    }

                    numPaginas = documento.Pages.Count;
                    PdfDocumentProcessor documentoRadicado = new PdfDocumentProcessor();
                    documentoRadicado.LoadDocument(archivoPrincipal.S_RUTA_ARCHIVO, true);

                    foreach (PROYECCION_DOC_FIRMAS firma in firmas)
                    {
                        PdfTextSearchResults ubicacionFirma = documentoRadicado.FindText("[Firma" + firma.N_ORDEN.ToString() + "]");

                        if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                        {
                            // Dibujo Firmas
                            Image imagenFirma = null;
                            if (firma.N_ORDEN != orden) imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, textoFirma);
                            else FirmaDigital = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, "");
                            //Image imagenFirma = (new AreaMetro.Seguridad.FuncionarioC()).ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true);

                            if (imagenFirma != null && firma.N_ORDEN != orden)
                            {
                                float topPagina = (float)ubicacionFirma.Page.CropBox.Top;

                                /*using (PdfGraphics graphics = documento.CreateGraphics())
                                {
                                    graphics.FillRectangle(Brushes.White, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, (float)ubicacionFirma.Rectangles[0].Width, (float)ubicacionFirma.Rectangles[0].Height));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);

                                    graphics.DrawImage(imagenFirma, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, 240, 78));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);
                                }*/


                                XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber - 1]);
                                graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height) + 2));
                                DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionFirma.Rectangles[0].Top), 240, 78);
                                graphics.Dispose();
                            }
                            else if (firma.N_ORDEN == orden)
                            {
                                float topPagina = (float)ubicacionFirma.Page.CropBox.Top;
                                XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber - 1]);
                                graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height) + 2));
                                graphics.Dispose();
                                FirmaDigital.Save(msFirma, ImageFormat.Png);
                                msFirma.Seek(0, SeekOrigin.Begin);
                                CampoFirmaDig = "[Firma" + firma.N_ORDEN.ToString() + "]";
                                PagFirma = ubicacionFirma.PageNumber;
                            }
                        }
                    }

                    if (radicado)
                    {
                        MemoryStream imagenEtiqueta = new MemoryStream();

                        Radicador radicador = new Radicador();
                        var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
                        imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[0]);
                        DrawImage(graphics, imagenRadicado, 300, 30, 288, 72);
                        graphics.Dispose();

                        string seriesDobleEtiqueta = ConfigurationManager.AppSettings["SeriesProyeccionDobleEtiqueta"];

                        if (seriesDobleEtiqueta != null && seriesDobleEtiqueta.Trim() != "" && ("," + seriesDobleEtiqueta.Replace(" ", "") + ",").Contains("," + proyeccionDocumento.CODSERIE.ToString() + ","))
                        {
                            if (documento.Pages.Count > 1)
                            {
                                graphics = XGraphics.FromPdfPage(documento.Pages[documento.Pages.Count - 1]);
                                DrawImage(graphics, imagenRadicado, 300, 30, 288, 72);
                                graphics.Dispose();
                            }
                        }
                    }

                    PdfTextSearchResults ubicacionTramites = documentoRadicado.FindText("[Tramites]");

                    if (ubicacionTramites.Status == PdfTextSearchStatus.Found)
                    {
                        /*Image imagenFirma = ObtenerImagenTextoTramites(proyeccionDocumento.S_TRAMITES);

                        float topPagina = (float)ubicacionTramites.Page.CropBox.Top;

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionTramites.PageNumber - 1]);
                        graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionTramites.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionTramites.Rectangles[0].Width), Convert.ToInt32(ubicacionTramites.Rectangles[0].Height) + 2));
                        DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionTramites.Rectangles[0].Top), Convert.ToInt32(Convert.ToDouble(imagenFirma.Width)*0.7), Convert.ToInt32(Convert.ToDouble(imagenFirma.Height)*0.7));
                        
                        graphics.Dispose();*/

                        float topPagina = (float)ubicacionTramites.Page.CropBox.Top;

                        XGraphics graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionTramites.PageNumber - 1]);

                        graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionTramites.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionTramites.Rectangles[0].Width), Convert.ToInt32(ubicacionTramites.Rectangles[0].Height) + 2));

                        XFont font = new XFont("Arial", 9, XFontStyle.Regular);
                        var formatter = new XTextFormatter(graphics);

                        List<string> lineas;

                        if (proyeccionDocumento.S_TRAMITES != null && proyeccionDocumento.S_TRAMITES.Trim() != "")
                        {
                            lineas = LineasTextoTramitesPdf(proyeccionDocumento.S_TRAMITES, font, graphics);
                        }
                        else
                        {
                            lineas = new List<string>();

                            lineas.Add("Trámites:");
                            lineas.Add("Nuevo Trámite.");
                        }


                        int cont = 0;
                        foreach (string linea in lineas)
                        {
                            var layoutRectangle = new XRect(Convert.ToInt32(ubicacionTramites.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionTramites.Rectangles[0].Top) + cont * 10, 450, 10);

                            formatter.DrawString(linea, font, XBrushes.Black, layoutRectangle, XStringFormats.TopLeft);
                            cont++;
                        }

                        graphics.Dispose();
                    }
                }
                else
                {
                    return null;
                }

                var archivosAdjuntos = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                        where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 2 && da.PROYECCION_DOC_ARCHIVOS.S_ACTIVO == "S" && da.S_ACTIVO == "S"
                                        select da);

                if (archivosAdjuntos != null)
                {
                    foreach (var archivoAdjunto in archivosAdjuntos)
                    {
                        if (Path.GetExtension(archivoAdjunto.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                        {
                            MemoryStream streamDocumentoWord;
                            streamDocumentoWord = (MemoryStream)Archivos.ConvertirAPDF(archivoAdjunto.S_RUTA_ARCHIVO);

                            var inputDocument = PdfReader.Open(streamDocumentoWord, PdfDocumentOpenMode.Import);
                            var count = inputDocument.PageCount;
                            for (int idx = 0; idx < count; idx++)
                            {
                                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                                documento.AddPage(page);

                                numPaginas++;
                            }

                            streamDocumentoWord.Close();
                            streamDocumentoWord.Dispose();
                        }
                        else
                        {
                            PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
                            documentoDE.LoadDocument(archivoAdjunto.S_RUTA_ARCHIVO, true);

                            MemoryStream documentoDEPDF = new MemoryStream();
                            documentoDE.SaveDocument(documentoDEPDF);
                            documentoDE.CloseDocument();

                            var inputDocument = PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);

                            //var inputDocument = PdfReader.Open(archivoAdjunto.S_RUTA_ARCHIVO, PdfDocumentOpenMode.Import);

                            var count = inputDocument.PageCount;
                            for (int idx = 0; idx < count; idx++)
                            {
                                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                                documento.AddPage(page);

                                numPaginas++;
                            }

                            inputDocument.Close();
                            inputDocument.Dispose();
                            documentoDEPDF.Close();
                            documentoDEPDF.Dispose();
                        }

                        numPaginas = documento.Pages.Count;
                    }
                }

                MemoryStream ms = new MemoryStream();
                documento.Save(ms);

                //Firma digital del documento

                ms.Seek(0, SeekOrigin.Begin);
                byte[] _Aux;
                Utilidades.FirmaDigital.RespuestaFirma _Rpta = Utilidades.FirmaDigital.FirmaProyeccion.FirmaDocumento(ms.ToArray(), msFirma.ToArray(), PagFirma, CampoFirmaDig, usuario, pass).Result;
                if (_Rpta.Exito) _Aux = _Rpta.ArchivoFirmado;
                else _Aux = null;
                var documentoRetorno = _Aux;

                documento.Close();
                ms.Close();
                ms.Dispose();

                return new { documentoBinario = documentoRetorno, numPaginas = numPaginas };
            }
            else
            {
                return null;
            }
        }

        public void DocumentoMarcaAgua(string rutaArchivo, string watermark, bool copiaOriginal)
        {
            if (copiaOriginal)
            {
                File.Copy(rutaArchivo, rutaArchivo.Replace(".pdf", "_Anulado.pdf"));
            }

            PdfSharp.Pdf.PdfDocument documento = PdfReader.Open(rutaArchivo, PdfDocumentOpenMode.Modify);

            AddWatermarkRed("ANULADO", documento);

            MemoryStream archivo = new MemoryStream();

            documento.Save(archivo);
            documento.Close();
            documento.Dispose();

            FileStream archivoAnulado = new FileStream(rutaArchivo, FileMode.Create);
            archivo.WriteTo(archivoAnulado);

            archivoAnulado.Close();
            archivoAnulado.Dispose();
            archivo.Close();
            archivo.Dispose();
        }

        private List<string> LineasTextoTramitesPdf(string tramites, XFont font, XGraphics graphics)
        {
            double tamanoLinea = 480;
            string linea = "";
            List<string> lineas = new List<string>();

            tramites = tramites.Replace(" ", "") + ".";

            lineas.Add("Trámites:");

            var listaTramites = tramites.Split(',');

            foreach (string tramite in listaTramites)
            {
                if (linea == "")
                    linea = tramite;
                else
                {
                    XSize sizeLine = graphics.MeasureString(linea + "," + tramite, font);

                    if (sizeLine.Width <= tamanoLinea)
                    {
                        linea += "," + tramite;
                    }
                    else
                    {
                        lineas.Add(linea + ",");
                        linea = tramite;
                    }
                }
            }

            lineas.Add(linea);

            return lineas;
        }

        private System.Drawing.Image ObtenerImagenTextoTramites(string tramites)
        {
            int letrasLinea = 110;
            string linea = "";
            List<string> lineas = new List<string>();

            tramites = tramites.Replace(" ", "") + ".";

            lineas.Add("Trámites:");

            var listaTramites = tramites.Split(',');

            foreach (string tramite in listaTramites)
            {
                if (linea == "")
                    linea = tramite;
                else
                {
                    if ((linea + "," + tramite).Length <= letrasLinea)
                    {
                        linea += "," + tramite;
                    }
                    else
                    {
                        lineas.Add(linea + ",");
                        linea = tramite;
                    }
                }
            }

            lineas.Add(linea);

            int textLength = tramites.Length;
            int fontSize = 10;

            // Set canvas width & height
            int width;
            int height;

            //width = (fontSize * textLength) - ((textLength * fontSize) / 3);
            width = (fontSize * letrasLinea) - ((letrasLinea * fontSize) / 3);
            height = (fontSize + 2)*lineas.Count;

            // Initialize graphics
            RectangleF rectF = new RectangleF(-1, -1, width+1, height+1);
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format64bppArgb);
            Graphics g = Graphics.FromImage(pic);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Set colors
            Color fontColor = Color.Black;
            Color rectColor = Color.White;
            SolidBrush fgBrush = new SolidBrush(fontColor);
            SolidBrush bgBrush = new SolidBrush(rectColor);

            g.FillRectangle(bgBrush, rectF);

            FontFamily fontFamily = FontFamily.GenericMonospace;

            FontStyle style = FontStyle.Regular;
            Font font = new Font(fontFamily, fontSize, style, GraphicsUnit.Pixel);

            // Finally, draw the font
            //g.DrawString(tramites, font, fgBrush, rectF, StringFormat.GenericDefault);

            int cont = 0;
            foreach (string lineaTexto in lineas)
            {
                RectangleF rectFLinea = new RectangleF(0, cont*(fontSize + 2), width, (fontSize + 2));

                g.DrawString(lineaTexto, font, fgBrush, rectFLinea, StringFormat.GenericDefault);
                cont++;
            }

            // Dispose objects
            return pic;
        }

        private void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }

        public dynamic DocumentoRadicadoDE(int id, bool radicado, int idRadicado, bool watermark, string textoFirma)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            int numPaginas = 0;

            PROYECCION_DOC proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();

            if (proyeccionDocumento != null)
            {
                PdfDocumentProcessor documento = new PdfDocumentProcessor();

                // Se carga el documento principal

                PROYECCION_DOC_DET_ARCH archivoPrincipal = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                                            where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 1 && da.S_ACTIVO == "S"
                                                            select da).FirstOrDefault();

                //var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id && pf.S_ESTADO == "S");
                var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id);

                if (archivoPrincipal != null)
                {
                    if (Path.GetExtension(archivoPrincipal.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                    {
                        MemoryStream streamDocumento;
                        streamDocumento = (MemoryStream)Archivos.ConvertirAPDF(archivoPrincipal.S_RUTA_ARCHIVO);

                        documento.LoadDocument(streamDocumento, true);
                        streamDocumento.Close();
                        streamDocumento.Dispose();
                    }
                    else
                    {
                        documento.AppendDocument(archivoPrincipal.S_RUTA_ARCHIVO);
                    }

                    numPaginas = documento.Document.Pages.Count;

                    foreach (PROYECCION_DOC_FIRMAS firma in firmas)
                    {
                        PdfTextSearchResults ubicacionFirma = documento.FindText("[Firma" + firma.N_ORDEN.ToString() + "]");

                        if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                        {
                            // Dibujo Firmas
                            Image imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true, textoFirma);
                            //Image imagenFirma = (new AreaMetro.Seguridad.FuncionarioC()).ObtenerFirmaElectronicaFuncionario(firma.CODFUNCIONARIO, true);

                            if (imagenFirma != null)
                            {
                                float topPagina = (float)ubicacionFirma.Page.CropBox.Top;

                                using (PdfGraphics graphics = documento.CreateGraphics())
                                {
                                    graphics.FillRectangle(Brushes.White, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, (float)ubicacionFirma.Rectangles[0].Width, (float)ubicacionFirma.Rectangles[0].Height));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);

                                    graphics.DrawImage(imagenFirma, new RectangleF((float)ubicacionFirma.Rectangles[0].Left, topPagina - (float)ubicacionFirma.Rectangles[0].Top, 240, 78));
                                    graphics.AddToPageForeground(ubicacionFirma.Page, 72, 72);
                                }
                            }
                        }
                    }

                    if (radicado)
                    {
                        float topPagina = (float)documento.Document.Pages[0].CropBox.Top;

                        /*Radicado01Report etiqueta = new Radicado01Report();
                        MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(idRadicado, "png");*/

                        MemoryStream imagenEtiqueta = new MemoryStream();

                        Radicador radicador = new Radicador();
                        var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
                        imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);

                        using (PdfGraphics graphics = documento.CreateGraphics())
                        {
                            graphics.DrawImage(imagenEtiqueta, new RectangleF(300, 30, 288, 72));
                            graphics.AddToPageBackground(documento.Document.Pages[0], 72, 72);
                        }
                    }
                }
                else
                {
                    return null;
                }

                var archivosAdjuntos = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                        where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 2 && da.PROYECCION_DOC_ARCHIVOS.S_ACTIVO == "S" && da.S_ACTIVO == "S"
                                        select da);

                if (archivosAdjuntos != null)
                {
                    foreach (var archivoAdjunto in archivosAdjuntos)
                    {
                        if (Path.GetExtension(archivoAdjunto.S_RUTA_ARCHIVO).ToUpper().Contains("DOCX"))
                        {
                            MemoryStream streamDocumento;
                            streamDocumento = (MemoryStream)Archivos.ConvertirAPDF(archivoAdjunto.S_RUTA_ARCHIVO);

                            documento.AppendDocument(streamDocumento);
                            streamDocumento.Close();
                            streamDocumento.Dispose();
                        }
                        else
                        {
                            documento.AppendDocument(archivoAdjunto.S_RUTA_ARCHIVO);
                        }

                        numPaginas = documento.Document.Pages.Count;
                    }
                }

                MemoryStream ms = new MemoryStream();

                documento.SaveDocument(ms);

                if (watermark)
                    this.AddWatermarkDE("BORRADOR", ms);

                var documentoRetorno = ms.GetBuffer();

                documento.CloseDocument();
                ms.Close();
                ms.Dispose();

                return new { documentoBinario = documentoRetorno, numPaginas = numPaginas };
            }
            else
            {
                return null;
            }
        }

        public string ValidarFirmasDocumento(int id)
        {
            int numFirmas = 0;
            string respuesta = "";
            string validacionProyecta = "";
            PROYECCION_DOC proyeccionDocumento = dbSIM.PROYECCION_DOC.Where(pd => pd.ID_PROYECCION_DOC == id).FirstOrDefault();

            if (proyeccionDocumento != null)
            {
                //PdfDocumentProcessor documento = new PdfDocumentProcessor();
                PdfSharp.Pdf.PdfDocument documento = new PdfSharp.Pdf.PdfDocument();

                // Se carga el documento principal

                PROYECCION_DOC_DET_ARCH archivoPrincipal = (from da in dbSIM.PROYECCION_DOC_DET_ARCH
                                                            where da.PROYECCION_DOC_ARCHIVOS.ID_PROYECCION_DOC == id && da.PROYECCION_DOC_ARCHIVOS.N_TIPO == 1 && da.S_ACTIVO == "S"
                                                            select da).FirstOrDefault();

                var firmas = dbSIM.PROYECCION_DOC_FIRMAS.Where(pf => pf.ID_PROYECCION_DOC == id);

                if (archivoPrincipal != null)
                {
                    PdfDocumentProcessor documentoRadicado = new PdfDocumentProcessor();
                    documentoRadicado.LoadDocument(archivoPrincipal.S_RUTA_ARCHIVO, true);

                    for (int i = 1; i <= 10; i++)
                    {
                        PdfTextSearchResults ubicacionFirma = documentoRadicado.FindText("[Firma" + i.ToString() + "]");

                        if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                        {
                            ubicacionFirma = documentoRadicado.FindText("[Firma" + i.ToString() + "]");

                            if (ubicacionFirma.Status == PdfTextSearchStatus.Found)
                            {
                                respuesta = "La etiqueta [Firma" + i.ToString() + "] se encuentra repetida en el documento Principal.";
                                break;
                            }

                            numFirmas++;

                            var firmaConfigurada = firmas.Where(f => f.N_ORDEN == i).FirstOrDefault();

                            if (firmaConfigurada == null)
                            {
                                respuesta = "La cantidad etiquetas de Firmas en el documento no corresponden a las firmas configuradas para el documento.";
                                break;
                            }
                        }
                    }

                    if (proyeccionDocumento.CODSERIE == 11 || proyeccionDocumento.CODSERIE == 17 || proyeccionDocumento.CODSERIE == 722 || proyeccionDocumento.CODSERIE == 1082 || proyeccionDocumento.CODSERIE == 1102)
                    {
                        PdfTextSearchResults ubicacionProyecta = documentoRadicado.FindText("[Proyecta]");

                        if (ubicacionProyecta.Status != PdfTextSearchStatus.Found)
                        {
                            validacionProyecta = "La etiqueta [Proyecta] es requerida para las RESOLUCIONES y AUTOS.";
                        }
                    }

                    documentoRadicado.CloseDocument();
                    documentoRadicado.Dispose();

                    if (respuesta == "" && numFirmas != firmas.Count())
                    {
                        respuesta = "La cantidad etiquetas de Firmas en el documento no corresponden a las firmas configuradas para el documento.";
                    }

                    if (respuesta != "")
                        respuesta += (validacionProyecta == "" ? "" : ". ") + validacionProyecta;
                    else
                        respuesta += validacionProyecta;
                }
                else
                {
                    respuesta = "Error Cargando el Archivo Principal.";
                }
            }
            else
            {
                respuesta = "Error Consultando el Documento.";
            }

            return respuesta;
        }

        private void AddWatermark(string text, PdfSharp.Pdf.PdfDocument document)
        {
            try
            {
                string fontName = "Arial Black";
                //int fontSize = 12;
                //var fontBase = new Font(fontName, fontSize);
                var font = new XFont(fontName, 60, XFontStyle.Bold);

                foreach (var page in document.Pages)
                {
                    var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                    // Get the size (in points) of the text.
                    var size = gfx.MeasureString(text, font);
                    // Define a rotation transformation at the center of the page.
                    gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                    gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                    gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                    // Create a string format.
                    var format = new XStringFormat();
                    format.Alignment = XStringAlignment.Near;
                    format.LineAlignment = XLineAlignment.Near;
                    // Create a dimmed red brush.
                    //XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));
                    XBrush brush = new XSolidBrush(XColor.FromKnownColor(KnownColor.LightGray));
                    // Draw the string.
                    gfx.DrawString(text, font, brush, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);
                }
            } catch (Exception error)
            {
                string e = error.Message;
            }
        }

        private void AddWatermarkRed(string text, PdfSharp.Pdf.PdfDocument document)
        {
            try
            {
                string fontName = "Arial Black";
                //int fontSize = 12;
                //var fontBase = new Font(fontName, fontSize);
                var font = new XFont(fontName, 120, XFontStyle.Bold);

                foreach (var page in document.Pages)
                {
                    var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);
                    // Get the size (in points) of the text.
                    var size = gfx.MeasureString(text, font);
                    // Define a rotation transformation at the center of the page.
                    gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                    gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                    gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                    // Create a string format.
                    var format = new XStringFormat();
                    format.Alignment = XStringAlignment.Near;
                    format.LineAlignment = XLineAlignment.Near;
                    // Create a dimmed red brush.
                    //XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));
                    //XBrush brush = new XSolidBrush(XColor.FromKnownColor(KnownColor.LightGray));
                    XBrush brush = new XSolidBrush(XColor.FromKnownColor(KnownColor.Red));
                    // Draw the string.
                    gfx.DrawString(text, font, brush, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);
                }
            }
            catch (Exception error)
            {
                string e = error.Message;
            }
        }

        private void AddWatermarkDE(string text, string fileName, string resultFileName)
        {
            using (var documentProcessor = new PdfDocumentProcessor())
            {
                string fontName = "Arial Black";
                int fontSize = 12;
                PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                stringFormat.Alignment = PdfStringAlignment.Center;
                stringFormat.LineAlignment = PdfStringAlignment.Center;
                documentProcessor.LoadDocument(fileName);

                using (var brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                {
                    using (var font = new Font(fontName, fontSize))
                    {
                        foreach (var page in documentProcessor.Document.Pages)
                        {
                            //Single watermarkSize = Convert.ToSingle(page.CropBox.Width * 0.75);
                            Single watermarkSize = Convert.ToSingle(page.CropBox.Width * 0.9);
                            using (var graphics = documentProcessor.CreateGraphics())
                            {
                                SizeF stringSize = graphics.MeasureString(text, font);
                                Single scale = watermarkSize / stringSize.Width;
                                graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                graphics.RotateTransform(-45.0F);
                                graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                using (var actualFont = new Font(fontName, fontSize * scale))
                                {
                                    RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                    graphics.DrawString(text, actualFont, brush, rect, stringFormat);
                                }
                                graphics.AddToPageForeground(page, 72, 72);
                            }
                        }
                    }
                }
                documentProcessor.SaveDocument(resultFileName);
            }
        }

        private void AddWatermarkDE(string text, Stream document)
        {
            using (var documentProcessor = new PdfDocumentProcessor())
            {
                string fontName = "Arial Black";
                int fontSize = 12;
                PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                stringFormat.Alignment = PdfStringAlignment.Center;
                stringFormat.LineAlignment = PdfStringAlignment.Center;
                documentProcessor.LoadDocument(document);

                using (var brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                {
                    using (var font = new Font(fontName, fontSize))
                    {
                        foreach (var page in documentProcessor.Document.Pages)
                        {
                            //Single watermarkSize = Convert.ToSingle(page.CropBox.Width * 0.75);
                            Single watermarkSize = Convert.ToSingle(page.CropBox.Width * 0.9);
                            using (var graphics = documentProcessor.CreateGraphics())
                            {
                                SizeF stringSize = graphics.MeasureString(text, font);
                                Single scale = watermarkSize / stringSize.Width;
                                graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                graphics.RotateTransform(-45.0F);
                                graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                using (var actualFont = new Font(fontName, fontSize * scale))
                                {
                                    RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                    graphics.DrawString(text, actualFont, brush, rect, stringFormat);
                                }
                                graphics.AddToPageForeground(page, 72, 72);
                            }
                        }
                    }
                }

                documentProcessor.SaveDocument(document);
            }
        }

        public void GenerarIndicesFullTextDocumento(int codTramite, int codDocumento)
        {
            ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

            dbTramites.SP_CREAR_INDICE_FULLTEXT(codTramite, codDocumento, rtaResultado);

            if (rtaResultado.Value.ToString() != "OK")
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "GenerarIndicesFullTextDocumento (" + codTramite.ToString() + ", " + codDocumento.ToString() + "): " + rtaResultado.Value.ToString());
            }
        }

        public void PruebaFirmas()
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            Encrypt3DES ed3des = new Encrypt3DES();
            int numPaginas = 0;
            var streamDocumento = new MemoryStream();

            PdfSharp.Pdf.PdfDocument documento = new PdfSharp.Pdf.PdfDocument();

            PdfDocumentProcessor documentoDE = new PdfDocumentProcessor();
            documentoDE.LoadDocument(@"C:\Temp\Firmas\Prueba.pdf", true);

            MemoryStream documentoDEPDF = new MemoryStream();
            documentoDE.SaveDocument(documentoDEPDF);
            documentoDE.CloseDocument();

            var inputDocument = PdfReader.Open(documentoDEPDF, PdfDocumentOpenMode.Import);
            var count = inputDocument.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                documento.AddPage(page);

                numPaginas++;
            }

            inputDocument.Close();
            inputDocument.Dispose();
            documentoDEPDF.Close();
            documentoDEPDF.Dispose();

            numPaginas = documento.Pages.Count;

            PdfDocumentProcessor documentoRadicado = new PdfDocumentProcessor();
            documentoRadicado.LoadDocument(@"C:\Temp\Firmas\Prueba.pdf", true);

            PdfTextSearchResults ubicacionFirma;

            // Dibujo Firmas
            Image imagenFirma;
            float topPagina;
            XGraphics graphics;

            ubicacionFirma = documentoRadicado.FindText("[Firma1]");

            imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(1111117934, true, "Firmado 000/00/000\r\nRevisó");

            topPagina = (float)ubicacionFirma.Page.CropBox.Top;

            graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber - 1]);
            graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height) + 2));
            DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionFirma.Rectangles[0].Top), 240, 78);
            graphics.Dispose();

            ubicacionFirma = documentoRadicado.FindText("[Firma2]");

            imagenFirma = Security.ObtenerFirmaElectronicaFuncionario(4001, true, "Firmado 000/00/000\r\nAprobó", 621, 1);
            topPagina = (float)ubicacionFirma.Page.CropBox.Top;

            graphics = XGraphics.FromPdfPage(documento.Pages[ubicacionFirma.PageNumber - 1]);
            graphics.DrawRectangle(Brushes.White, new Rectangle(Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - (float)ubicacionFirma.Rectangles[0].Top) - 2, Convert.ToInt32(ubicacionFirma.Rectangles[0].Width), Convert.ToInt32(ubicacionFirma.Rectangles[0].Height) + 2));
            DrawImage(graphics, imagenFirma, Convert.ToInt32(ubicacionFirma.Rectangles[0].Left), Convert.ToInt32(topPagina - ubicacionFirma.Rectangles[0].Top), 240, 78);
            graphics.Dispose();

            documento.Save(@"C:\Temp\Firmas\PruebaFirmas.pdf");
            documento.Close();
            documento.Dispose();
        }
    }
}