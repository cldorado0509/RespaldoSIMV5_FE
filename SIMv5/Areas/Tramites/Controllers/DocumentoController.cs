using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Models;
using SIM.Utilidades;
using System.IO;
using System.Text;
using BaxterSoft.Graphics;
using SIM.Data;
using SIM.Data.Tramites;

namespace SIM.Areas.Tramites.Controllers
{
    public class DocumentoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public ActionResult ConsultarInformeTecnicoRadicado(int idTramite, int idDocumento, int descargar = 1)
        {
            string extension;
            TBTRAMITEDOCUMENTO documento = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == idTramite && td.CODDOCUMENTO == idDocumento).FirstOrDefault();

            if (documento != null)
            {
                if (System.IO.File.Exists(documento.RUTA))
                {

                    if (documento.CIFRADO == "1")
                    {
                        Cryptografia crypt = new Cryptografia();

                        var ms = crypt.DesEncriptar(documento.RUTA, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"));

                        if (Path.GetExtension(documento.RUTA).ToUpper().Trim() == ".TIFF" || Path.GetExtension(documento.RUTA).ToUpper().Trim() == ".TIF")
                        {
                            // **** CONVIERTE A PDF
                            TIFFReader tr = new TIFFReader(ms);
                            TIFFDocument tdoc = tr.Read(false);
                            tr.Close();
                            PDFWriter pdf = new PDFWriter(tdoc);

                            ms = pdf.SavePDF();

                            extension = ".PDF";
                        }
                        else
                        {
                            extension = Path.GetExtension(documento.RUTA);
                        }

                        //return pdf.SavePDF();
                        // **** CONVIERTE A PDF

                        if (descargar == 1)
                            return File(ms.GetBuffer(), "application/" + Path.GetExtension(documento.RUTA).Replace(".", ""), "Documento_" + DateTime.Now.ToString("yyyyMMddHHmm") + extension);
                        else
                            return File(ms.GetBuffer(), "application/" + Path.GetExtension(documento.RUTA).Replace(".", ""));
                    }
                    else
                    {
                        extension = Path.GetExtension(documento.RUTA);

                        if (descargar == 1)
                            return File(documento.RUTA, "application/" + Path.GetExtension(documento.RUTA).Replace(".", ""), "Documento_" + DateTime.Now.ToString("yyyyMMddHHmm") + extension);
                        else
                            return File(documento.RUTA, "application/" + Path.GetExtension(documento.RUTA).Replace(".", ""));
                    }

                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public ActionResult ConsultarDocumentoTemporal(int idDocumento)
        {
            string extension;
            /*SIM.Data.Tramites.DOCUMENTO_TEMPORAL documento = dbSIM.Database.SqlQuery<SIM.Data.Tramites.DOCUMENTO_TEMPORAL>("SELECT S_RUTA "
                + "FROM TRAMITES.DOCUMENTO_TEMPORAL "
                + "WHERE ID_DOCUMENTO = " + idDocumento.ToString()).FirstOrDefault();*/
            var documento = dbSIM.DOCUMENTO_TEMPORAL.Where(dt => dt.ID_DOCUMENTO == idDocumento).FirstOrDefault();

            if (documento != null)
            {
                if (System.IO.File.Exists(documento.S_RUTA))
                {
                    var bytesArchivo = System.IO.File.ReadAllBytes(documento.S_RUTA);
                    var ms = new MemoryStream(bytesArchivo, 0, bytesArchivo.Count(), true, true);

                    if (Path.GetExtension(documento.S_RUTA).ToUpper().Trim() == ".TIFF" || Path.GetExtension(documento.S_RUTA).ToUpper().Trim() == ".TIF")
                    {
                        // **** CONVIERTE A PDF
                        TIFFReader tr = new TIFFReader(ms);
                        TIFFDocument tdoc = tr.Read(false);
                        tr.Close();
                        PDFWriter pdf = new PDFWriter(tdoc);

                        ms = pdf.SavePDF();

                        extension = ".PDF";
                    }
                    else
                    {
                        extension = Path.GetExtension(documento.S_RUTA);
                    }

                    // **** CONVIERTE A PDF

                    return File(ms.GetBuffer(), "application/" + Path.GetExtension(documento.S_RUTA).Replace(".", ""), "Documento_" + DateTime.Now.ToString("yyyyMMddHHmm") + extension);
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
    }
}
