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
using System.Drawing;
using SIM.Areas.Models;
using SIM.Areas.Tramites.Models;
using System.Data.Entity;
using System.IO;
using SIM.Areas.ControlVigilancia.Models;
using System.Drawing.Text;
using System.Web.Hosting;
using System.Drawing.Drawing2D;
using System.Globalization;
using DevExpress.BarCodes;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;

namespace SIM.Utilidades
{
    public struct DatosRadicado
    {
        public int IdRadicado;
        public string Radicado;
        public DateTime Fecha;
        public string Etiqueta;
    }

    public interface IRadicador
    {
        MemoryStream GenerarEtiqueta(int idRadicado, string tipoRetorno);
    }

    public class Radicador
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema AtencionUsuarios y Seguridad
        /// </summary>
        //EntitiesTramitesOracle dbSIMTra = new EntitiesTramitesOracle();
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public Radicador() { }

        /// <summary>
        /// Obtiene el siguiente radicado para la unidad documental y retorna los datos asociados al radicado generado
        /// </summary>
        /// <param name="idUnidadDocumental">Unidad Documental para la cual se va a generar el radicado</param>
        /// <param name="idUsuario">Usuario autenticado en el sistema</param>
        /// <param name="fechaRadica">Fecha de radicación, con la cual se deduce el consecutivo si se reinicia cada año</param>
        /// <returns>Retorna los datos asociados al radicado generado (Radicado, Fecha, Etiqueta)</returns>
        public DatosRadicado GenerarRadicado(int idUnidadDocumental, int idUsuario, DateTime fechaRadica)
        {
            return GenerarRadicado(null, idUnidadDocumental, idUsuario, fechaRadica, null);
        }

        public DatosRadicado GenerarRadicado(EntitiesSIMOracle dbSIMConexion, int idUnidadDocumental, int idUsuario, DateTime fechaRadica)
        {
            return GenerarRadicado(dbSIMConexion, idUnidadDocumental, idUsuario, fechaRadica, null);
        }

        public DatosRadicado GenerarRadicado(int idUnidadDocumental, int idUsuario, DateTime fechaRadica, string claveParametroFuncionario)
        {
            return GenerarRadicado(null, idUnidadDocumental, idUsuario, fechaRadica, claveParametroFuncionario);
        }

        /// <summary>
        /// Obtiene el siguiente radicado para la unidad documental y retorna los datos asociados al radicado generado
        /// </summary>
        /// <param name="idUnidadDocumental">Unidad Documental para la cual se va a generar el radicado</param>
        /// <param name="idUsuario">Usuario autenticado en el sistema</param>
        /// <param name="fechaRadica">Fecha de radicación, con la cual se deduce el consecutivo si se reinicia cada año</param>
        /// <returns>Retorna los datos asociados al radicado generado (Radicado, Fecha, Etiqueta)</returns>
        public DatosRadicado GenerarRadicado(EntitiesSIMOracle dbSIMConexion, int idUnidadDocumental, int idUsuario, DateTime fechaRadica, string claveParametroFuncionario)
        {
            EntitiesControlOracle dbControl = new EntitiesControlOracle();
            int consecutivoRadicado = 1;
            int anoUltimoRadicado = 0;
            bool reinicioAnual = false;
            string radicado = "";
            string etiqueta = "";
            int codFuncionario;

            if (dbSIMConexion == null)
                dbSIMConexion = dbSIM;


            // PARA REUTILIZAR RADICADO - INICIO &&&&&&
            // return new DatosRadicado() { IdRadicado = 149987, Radicado = null, Etiqueta = null, Fecha = DateTime.Today };
            // PARA REUTILIZAR RADICADO - FIN &&&&&&


            dynamic unidadDocumental = (from ud in dbSIMConexion.TBSERIE
                                        join ssd in dbSIMConexion.TBSUBSERIE_DOCUMENTAL on ud.CODSUBSERIE_DOCUMENTAL equals ssd.CODSUBSERIE_DOCUMENTAL
                                       where ud.CODSERIE == idUnidadDocumental
                                       select new
                                       {
                                           IDUnidadDocumental = ud.CODSERIE,
                                           IDSubSerieDocumental = ssd.CODSUBSERIE_DOCUMENTAL,
                                           IDSerieDocumental = ssd.CODSERIE_DOCUMENTAL
                                       }).FirstOrDefault();

            // Radicador utilizado para la unidad documental
            RADICADO_UNIDADDOC radicador = (from radicadorUD in dbSIMConexion.RADICADO_UNIDADDOC
                               where radicadorUD.CODSERIE == idUnidadDocumental
                               select radicadorUD).FirstOrDefault();

            if (radicador == null)
            {
                //throw new Exception("Radicador No Existe para la Unidad Documental");
                return new DatosRadicado { IdRadicado = -1, Radicado = "SERIE DOCUMENTAL SIN RADICADO", Etiqueta = "", Fecha = DateTime.Now };
            }

            // Siguiente consecutivo del radicador
            DEFRADICADOS configuracionRadicador = (from confRadicador in dbSIMConexion.DEFRADICADOS
                                where confRadicador.ID_RADICADO == radicador.ID_RADICADO
                                select confRadicador).FirstOrDefault();

            if (configuracionRadicador == null)
            {
                throw new Exception("Configuración del Radicador No Existe para la Unidad Documental");
            }

            reinicioAnual = (configuracionRadicador.S_REINICIOANUAL != null && configuracionRadicador.S_REINICIOANUAL == "1");

            RADICADO_DOCUMENTO ultimoRadicado = (from ultimoRadicadoUD in dbSIMConexion.RADICADO_DOCUMENTO
                                                 where ultimoRadicadoUD.ID_RADICADO == radicador.ID_RADICADO && (!reinicioAnual || ultimoRadicadoUD.D_RADICADO.Year == DateTime.Today.Year)
                                                 orderby ultimoRadicadoUD.N_CONSECUTIVO descending
                                                 select ultimoRadicadoUD).FirstOrDefault();

            if (ultimoRadicado != null)
                anoUltimoRadicado = ultimoRadicado.D_RADICADO.Year;
            else
                anoUltimoRadicado = DateTime.Today.Year;

            // Si se reinicia cada año y es el primero del año se asigna 1, de lo contrario se obtiene el siguiente
            if ((reinicioAnual && anoUltimoRadicado != fechaRadica.Year) || ultimoRadicado == null)
            {
                consecutivoRadicado = 1;
            }
            else
            {
                consecutivoRadicado = Convert.ToInt32(ultimoRadicado.N_CONSECUTIVO) + 1;
            }

            //consecutivoRadicado = -1; //&&&& BORRAR

            // Formato1, Formato2 y Separador de la configuración del Radicado
            radicado = (configuracionRadicador.S_FORMATO1 == null ? "" : (configuracionRadicador.S_FORMATO1.ToUpper() == "YYYY" ? fechaRadica.ToString("yyyy") : (configuracionRadicador.S_FORMATO1.Length > 3 && configuracionRadicador.S_FORMATO1.Substring(0, 3) == "999" ? consecutivoRadicado.ToString(configuracionRadicador.S_FORMATO1.Replace('9', '0')) : configuracionRadicador.S_FORMATO1))) +
                (configuracionRadicador.S_SEPARADOR == null ? "" : configuracionRadicador.S_SEPARADOR) +
                (configuracionRadicador.S_FORMATO2 == null ? "" : (configuracionRadicador.S_FORMATO2.ToUpper() == "YYYY" ? fechaRadica.ToString("yyyy") : (configuracionRadicador.S_FORMATO2.Length > 3 && configuracionRadicador.S_FORMATO2.Substring(0, 3) == "999" ? consecutivoRadicado.ToString(configuracionRadicador.S_FORMATO2.Replace('9', '0')) : configuracionRadicador.S_FORMATO2)));

            etiqueta = fechaRadica.ToString("yyyyMMddHHmm") + unidadDocumental.IDSerieDocumental.ToString("0") + unidadDocumental.IDSubSerieDocumental.ToString("0") + unidadDocumental.IDUnidadDocumental.ToString("0") + consecutivoRadicado.ToString("0");

            if (claveParametroFuncionario == null)
            {
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario));
            }
            else
            {
                //var funcionario = dbSIM.Database.SqlQuery<string>("SELECT VALOR FROM GENERAL.PARAMETROS WHERE CLAVE = '" + claveParametroFuncionario + "'").FirstOrDefault();
                var funcionario = Data.ObtenerValorParametro(claveParametroFuncionario);

                if (funcionario == null)
                {
                    throw new Exception("Funcionario Requerido. Verificar la configuración");
                }

                codFuncionario = Convert.ToInt32(funcionario);
            }

            RADICADO_DOCUMENTO nuevoRadicado = new RADICADO_DOCUMENTO() { ID_RADICADO = radicador.ID_RADICADO, CODSERIE = idUnidadDocumental, CODFUNCIONARIO = codFuncionario, S_RADICADO = radicado, S_ETIQUETA = etiqueta, D_RADICADO = fechaRadica, N_CONSECUTIVO = consecutivoRadicado, S_ESTADO = "R" };
            dbSIMConexion.Entry(nuevoRadicado).State = EntityState.Added;
            dbSIMConexion.SaveChanges();

            return new DatosRadicado() { IdRadicado = Convert.ToInt32(nuevoRadicado.ID_RADICADODOC), Radicado = radicado, Etiqueta = etiqueta, Fecha = fechaRadica };
        }

        /// <summary>
        /// Obtiene los datos asociados al radicado
        /// </summary>
        /// <param name="idRadicado">Radicado al cual se le van a obtener los datos</param>
        /// <returns>Retorna los datos asociados al radicado (Radicado, Fecha, Etiqueta)</returns>
        public DatosRadicado ObtenerDatosRadicado(int idRadicado)
        {
            RADICADO_DOCUMENTO radicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == Convert.ToDecimal(idRadicado)).FirstOrDefault();

            return new DatosRadicado() { IdRadicado = idRadicado, Radicado = radicado.S_RADICADO, Etiqueta = radicado.S_ETIQUETA, Fecha = radicado.D_RADICADO };
        }

        public MemoryStream GenerarEtiquetaRadicado(int idRadicado, IRadicador reporteEtiqueta, string tipoRetorno)
        {
            return reporteEtiqueta.GenerarEtiqueta(idRadicado, tipoRetorno);
        }

        public Bitmap ObtenerImagenRadicadoArea(int idRadicado)
        {
            decimal idRadicadoDec = Convert.ToDecimal(idRadicado);
            RADICADO_DOCUMENTO radicado = dbSIM.RADICADO_DOCUMENTO.Where(r => r.ID_RADICADODOC == idRadicadoDec).FirstOrDefault();
            TBSERIE unidadDocumental = dbSIM.TBSERIE.Where(ud => ud.CODSERIE == radicado.CODSERIE).FirstOrDefault();

            Bitmap canvas = new Bitmap(590, 150);
            canvas.SetResolution(150, 150);
            PrivateFontCollection privateFonts = new System.Drawing.Text.PrivateFontCollection();
            privateFonts.AddFontFile(HostingEnvironment.MapPath(@"~/fonts/OCRAEXT.TTF"));

            Font ttf11b = new Font(privateFonts.Families[0], 11, FontStyle.Bold);
            Font ttf9b = new Font(privateFonts.Families[0], 9, FontStyle.Bold);
            Font ttf8 = new Font(privateFonts.Families[0], 8, FontStyle.Regular);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            Pen pen = new Pen(Color.Black, 1);
            pen.Alignment = PenAlignment.Inset;
            TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
            BarCode barCode = new BarCode();
            barCode.Symbology = Symbology.Code128;
            barCode.CodeText = radicado.S_ETIQUETA;
            barCode.CodeTextFont = ttf8;
            barCode.AutoSize = false;
            barCode.BackColor = Color.White;
            barCode.ForeColor = Color.Black;
            barCode.RotationAngle = 0;
            barCode.Options.Code128.ShowCodeText = true;
            barCode.Module = 2.5;
            barCode.DpiX = 150;
            barCode.DpiY = 150;
            barCode.Unit = GraphicsUnit.Pixel;
            barCode.ImageSize = new SizeF(400, 70);
            barCode.CodeBinaryData = System.Text.Encoding.Default.GetBytes(barCode.CodeText);
            MemoryStream code128 = new MemoryStream();
            barCode.BarCodeImage.Save(code128, System.Drawing.Imaging.ImageFormat.Png);
            Bitmap barras = new Bitmap(code128);
            code128.Dispose();

            Bitmap logo = new Bitmap(HostingEnvironment.MapPath(@"~/Content/images/Logo_AreaRad.png"));
            Rectangle rect = new Rectangle(0, 0, canvas.Width - 1, canvas.Height - 1);

            using (Graphics gra = Graphics.FromImage(canvas))
            {
                gra.FillRectangle(new SolidBrush(Color.White), rect);
                gra.DrawRectangle(pen, rect);
                gra.DrawImage(barras, 20, 3);
                gra.DrawImage(logo, 470, 18, 115, 115);
                gra.DrawString(unidadDocumental.NOMBRE, ttf9b, brush, 2, 80);
                CultureInfo esCO = new CultureInfo("es-CO");
                gra.DrawString(texto.ToTitleCase(radicado.D_RADICADO.ToString("MMMM dd, yyyy H:mm", esCO)), ttf9b, brush, 2, 100);

                var sql = "SELECT CODENTIDAD FROM SEGURIDAD.DEPENDENCIA WHERE ID_DEPENDENCIA = (SELECT ID_DEPENDENCIA FROM SEGURIDAD.FUNCIONARIO_DEPENDENCIA WHERE CODFUNCIONARIO = " + radicado.CODFUNCIONARIO.ToString() + " AND D_SALIDA IS NULL)";
                var codigoRadicado = dbSIM.Database.SqlQuery<string>(sql).FirstOrDefault();

                if (codigoRadicado == null || codigoRadicado.Trim() == "")
                {
                    codigoRadicado = "00" + "-" + radicado.S_RADICADO.Trim();
                }
                else
                {
                    codigoRadicado = codigoRadicado.Trim() + "-" + radicado.S_RADICADO.Trim();
                }

                gra.DrawString("Radicado   " + codigoRadicado, ttf11b, brush, 2, 120);
            }

            return canvas;
        }

        /// <summary>
        /// Determina si en el dia y la hora especificada se puede generar memorandos
        /// </summary>
        /// <param name="diaHora">Dia y hora especifica</param>
        /// <returns></returns>
        public bool SePuedeGenerarRadicado(DateTime diaHora)
        {
            bool resp = false;
            try
            {
                var radicar = (from cal in dbSIM.CAL_LABORAL
                                where (cal.D_JORNADA1INICIO <= diaHora && cal.D_JORNADA1FIN >= diaHora) || (cal.D_JORNADA2INICIO <= diaHora && cal.D_JORNADA2FIN >= diaHora) && cal.S_LABORAL == "1"
                                select cal.ID_DIA).FirstOrDefault();
                if (radicar > 0) resp = true;
                else resp = false;
            }
            catch (Exception exp)
            {
                return resp;
            }
            return resp;
        }

    }
}