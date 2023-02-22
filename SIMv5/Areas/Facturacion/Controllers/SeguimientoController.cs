using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using SIM.Areas.Facturacion.Models;
using SIM.Data;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SIM.Areas.Facturacion.Controllers
{
    [Authorize]
    public class SeguimientoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        // GET: Facturacion/Seguimiento
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene los paremetros registrados para un tipo d etramite especifico
        /// </summary>
        /// <param name="Tramite">Codigo del tramite ambiental a consultar</param>
        /// <returns></returns>
        [HttpGet, ActionName("ParametrosSeguimiento")]
        public JObject ObtieneValoresSeguimiento(long Tramite)
        {
            JObject resp = null;
            var Parametro = (from tar in dbSIM.TBTARIFAS_TRAMITE
                             where tar.CODIGO_TRAMITE == Tramite && tar.TIPO_ACTUACION == "S"
                             select new ParametrosCalculo
                             {
                                 DuracionVisita = tar.VISITA.Value,
                                 HorasInforme = tar.INFORME.Value,
                                 NumeroVisitas = tar.N_VISITAS.Value,
                                 NumeroProfesionales = tar.TECNICOS.Value,
                                 Unidad = tar.S_UNIDAD == "N/A" ? "Items :" : tar.S_UNIDAD + " :"
                             }).FirstOrDefault();
            if (Parametro != null)
            {
                 resp = (JObject)JToken.FromObject(Parametro);
            }
            return resp;
        }

        /// <summary>
        /// Determina si un tercero existe o no a partir del documento sin digito de verificacion
        /// </summary>
        /// <param name="Documento">Numero de documento sin digito de verificacion</param>
        /// <returns></returns>
        [HttpGet, ActionName("ExisteTercero")]
        public bool ExisteTercero(decimal Documento)
        {
            var Tercero = (from Ter in dbSIM.TERCERO
                           where Ter.N_DOCUMENTON == Documento
                           select Ter).FirstOrDefault();
            if (Tercero != null) return true;
            else return false;
        }

        [HttpGet, ActionName("TiposTramite")]
        public JArray ObtieneTiposTramite()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var TipoTramite = (from Ttr in dbSIM.TBTARIFAS_TRAMITE
                                   where Ttr.TIPO_ACTUACION == "S" && Ttr.N_VISIBLE == 1
                                   orderby Ttr.NOMBRE
                                   select new
                                   {
                                       Ttr.CODIGO_TRAMITE,
                                       Ttr.NOMBRE
                                   }).ToList();
                return JArray.FromObject(TipoTramite, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Route("CalcularSeguimiento")]
        public JObject CalcularSeguimiento(DatosCalculo datosCalculo)
        {
            Seguimiento RetCalculo = new Seguimiento();
            CalculoSeguimiento Calculo = new CalculoSeguimiento();
            //string Ano = DateTime.Now.Year.ToString();
            string Ano = datosCalculo.Agno.ToString();
            decimal Salario = this.ObtenerHonorarios(Ano);
            RetCalculo = ValidarDatosSeguimiento(datosCalculo);
            if (!RetCalculo.CalculoExito) return (JObject)JToken.FromObject(RetCalculo);
            string _IdTipoTramite = datosCalculo.TipoTramite;
            int tarifaS = ObtenerHorasAbogadoSeguimiento(decimal.Parse(_IdTipoTramite));
            decimal Relacion = ObtenerRelacionSeguimiento(decimal.Parse(_IdTipoTramite));
            double ValorSalariosS = 0;
            double ValorSalariosSS = 0;
            double ValorTransporteS = 0;
            double Factor = 0;
            double DuracionVisita = Convert.ToDouble(datosCalculo.DuracionVisita);
            double HorasInforme = Convert.ToDouble(datosCalculo.HorasInforme);
            double NumeroVisitas = Convert.ToDouble(datosCalculo.NumeroVisitas);
            double tramitesSINA = Convert.ToDouble(datosCalculo.TramitesSINA);
            double NumeroProfesionales = Convert.ToDouble(datosCalculo.NumeroProfesionales);
            double CantidadNormas = Convert.ToDouble(datosCalculo.CantNormas);
            double CantidadLineas = Convert.ToDouble(datosCalculo.CantLineas);
            double TotalHTecnicoS = 0;
            double TotalHTecnicoSS = 0;
            var cultureInfo = CultureInfo.GetCultureInfo("es-CO");
            if (decimal.Parse(_IdTipoTramite) == 26)
            {
                HorasInforme = (5 * CantidadNormas);
                DuracionVisita = (2 * CantidadNormas);
                if (CantidadLineas > 2)
                {
                    HorasInforme += (CantidadLineas - 2);
                    DuracionVisita += (CantidadLineas - 2);
                }
            }
            else
            {
                if (Relacion != 0)
                {
                    if (datosCalculo.Items > 50) Factor = Math.Ceiling((double)(datosCalculo.Items / Relacion)); // (double)(datosCalculo.Items / Relacion);
                    else Factor = 1;
                }
                else Factor = 0;
            }
            if (Salario > 0)
            {
                if (Factor > 0)
                {
                    TotalHTecnicoS = (HorasInforme + DuracionVisita);
                    TotalHTecnicoSS = (HorasInforme * Factor ) + DuracionVisita;
                }
                else TotalHTecnicoS = HorasInforme + DuracionVisita;
                //  double TotalHAbogadoS = (double)tarifaS;
                //   ValorSalariosS = ((double)Salario / 240) * (TotalHTecnicoS + TotalHAbogadoS);
                //   ValorSalariosS = ValorSalariosS * tramitesSINA;
                ValorSalariosS = ((double)Salario / 240) * (TotalHTecnicoS + tarifaS);
                ValorSalariosSS = ((double)Salario / 240) * TotalHTecnicoS;
            }
            Calculo.Salarios = ((double)Salario / 240) * TotalHTecnicoS;
            decimal Transporte = ObtenerPasaje(Ano);
            if (Transporte > 0)
            {
                ValorTransporteS = (DuracionVisita * (long)Transporte * NumeroVisitas * tramitesSINA);
                Calculo.Transporte = ValorTransporteS;
            }
            Calculo.Laboratorio = 0;
            double ValorAdminS = (ValorSalariosS + ValorTransporteS + Calculo.Laboratorio) * 0.25;
            double ValorAdminSS = (ValorSalariosSS + ValorTransporteS + Calculo.Laboratorio) * 0.25;
            Calculo.Administracion = Math.Round(ValorAdminS, 0);
            double TotalNetoS = (ValorSalariosS + ValorTransporteS + Calculo.Laboratorio + ValorAdminS);
            if (Factor > 0)
            {
                double TotalNetoSS = (ValorSalariosSS + ValorTransporteS + Calculo.Laboratorio + ValorAdminSS);
                TotalNetoS = (TotalNetoS - TotalNetoSS) + (TotalNetoSS * Factor);
            }
            Calculo.TotalNeto = Math.Round(TotalNetoS, 0);
            decimal SalMinimo = ObtenerMinimo(Ano);
            Calculo.TotalPagar = Math.Round(TotalNetoS, 0).ToString("C", cultureInfo);
            Calculo.TotalPublicacion = 0;
            RetCalculo.TotalPagar = Calculo.TotalPagar;
            Calculo.Tramite = ObtenerNombreTramite(decimal.Parse(_IdTipoTramite));
            Calculo.CM = datosCalculo.CM;
            Calculo.NIT = datosCalculo.NIT;
            Calculo.Tercero = datosCalculo.Tercero;
            Calculo.Items = datosCalculo.Items;
            Calculo.Tecnicos = (int)datosCalculo.NumeroProfesionales;
            Calculo.Visitas = (int)datosCalculo.NumeroVisitas;
            Calculo.HorasInforme = (decimal)HorasInforme;
            Calculo.DuracionVisita = (decimal)DuracionVisita;
            Calculo.Observaciones = datosCalculo.Observaciones;
            Calculo.IdTramite = int.Parse( _IdTipoTramite);
            Calculo.CantNormas = datosCalculo.CantNormas;
            Calculo.CantLineas = datosCalculo.CantLineas;
            RetCalculo.Soporte = SoportePdfSeguimiento(Calculo);
            JObject resp = (JObject)JToken.FromObject(RetCalculo);
            return resp;
        }

        private Seguimiento ValidarDatosSeguimiento(DatosCalculo datosCalculo)
        {
            Seguimiento ret = new Seguimiento();
            ret.CalculoExito = true;
            if (datosCalculo == null)
            {
                ret.CalculoExito = false;
                ret.Mensaje = "Llamado errado al metodo para clacular el seguimiento";
            }
            if (datosCalculo.NIT == "")
            {
                ret.CalculoExito = false;
                ret.Mensaje = "Falta ingresar el NIT de la empresa";
            }
            if (datosCalculo.Tercero == "")
            {
                ret.CalculoExito = false;
                ret.Mensaje = "Falta ingresar el nombre del tercero";
            }
            if (datosCalculo.TipoTramite == "")
            {
                ret.CalculoExito = false;
                ret.Mensaje = "No se seleccionó un tipo de trámite";
            }
            if (datosCalculo.NumeroProfesionales <= 0)
            {
                ret.CalculoExito = false;
                ret.Mensaje = "Debe ingresar la cantidad de profesionales técnicos que participaran en el tramite";
            }
            return ret;
        }

        private byte[] SoportePdfSeguimiento(CalculoSeguimiento datosCalculo)
        {
            PDFDocument _Doc = new PDFDocument();
            MemoryStream oStream = new MemoryStream();
            _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            if (datosCalculo != null)
            {
                var cultureInfo = CultureInfo.GetCultureInfo("es-CO");
                if (datosCalculo.CM == null) datosCalculo.CM = "";
                PDFPage Pagina = _Doc.AddPage();
                Pagina.Width = 2550;
                Pagina.Height = 3300;
                PDFImage _img = new PDFImage(HostingEnvironment.MapPath(@"~/Content/Images/Logo_Area_Soporte.png"));
                Pagina.Canvas.DrawImage(_img, 100, 100, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
                TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 80, true, true);
                _Arial.Bold = true;
                _Arial.Size = 70;
                PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                _Arial.Bold = true;
                Pagina.Canvas.DrawText("ÁREA METROPOLITANA DEL", _Arial, null, brush, 600, 120);
                Pagina.Canvas.DrawText("VALLE DE ABURRÁ", _Arial, null, brush, 780, 210);
                _Arial.Bold = false;
                _Arial.Size = 40;
                Pagina.Canvas.DrawText("Entidad Administrativa de Derecho Público", _Arial, null, brush, 750, 290);
                _Arial.Size = 30;
                PDFPen _Pen = new PDFPen(new PDFRgbColor(Color.Green), 1);
                Pagina.Canvas.DrawRectangle(_Pen, null, 110, 500, 2000, 60, 0);
                Pagina.Canvas.DrawText("Cálculo valor trámite ambiental (Seguimiento)", _Arial, null, brush, (Pagina.Width / 2) - 300, 510);
                Pagina.Canvas.DrawText("Tipo de Trámite:         ", _Arial, null, brush, 110, 580);
                Pagina.Canvas.DrawText(datosCalculo.Tramite.Trim().ToUpper(), _Arial, null, brush, 450, 580);
                Pagina.Canvas.DrawText("Tercero:                       ", _Arial, null, brush, 110, 620);
                Pagina.Canvas.DrawText(datosCalculo.NIT.ToString() + "     " + datosCalculo.Tercero.Trim().ToUpper(), _Arial, null, brush, 450, 620);
                Pagina.Canvas.DrawText("CM:                            ", _Arial, null, brush, 110, 660);
                Pagina.Canvas.DrawText(datosCalculo.CM.ToString(), _Arial, null, brush, 450, 660);
                Pagina.Canvas.DrawText("Valor Proyecto:          ", _Arial, null, brush, 110, 700);
                Pagina.Canvas.DrawText("$0", _Arial, null, brush, 450, 700);
                Pagina.Canvas.DrawText("Soportes: N/A", _Arial, null, brush, 750, 700);
                Pagina.Canvas.DrawText("Valor Publicación:       ", _Arial, null, brush, 110, 740);
                Pagina.Canvas.DrawText(datosCalculo.TotalPublicacion.ToString("C", cultureInfo), _Arial, null, brush, 450, 740);
                Pagina.Canvas.DrawText("Nro. Trámites SINA:      ", _Arial, null, brush, 110, 780);
                Pagina.Canvas.DrawText(datosCalculo.Items.ToString() + "       " + UnidadMedidaItems(datosCalculo.IdTramite) + "  " + datosCalculo.Items.ToString(), _Arial, null, brush, 450, 780);
                Pagina.Canvas.DrawText("Parametro                              Valor", _Arial, null, brush, 110, 820);
                Pagina.Canvas.DrawText("Nro. Profesionales            ", _Arial, null, brush, 110, 860);
                Pagina.Canvas.DrawText(datosCalculo.Tecnicos.ToString(), _Arial, null, brush, 550, 860);
                Pagina.Canvas.DrawText("Nro. Visitas                  ", _Arial, null, brush, 110, 900);
                Pagina.Canvas.DrawText(datosCalculo.Visitas.ToString(), _Arial, null, brush, 550, 900);
                Pagina.Canvas.DrawText("Nro. Horas Informe            ", _Arial, null, brush, 110, 940);
                Pagina.Canvas.DrawText(datosCalculo.HorasInforme.ToString(), _Arial, null, brush, 550, 940);
                Pagina.Canvas.DrawText("Duracion Visita               ", _Arial, null, brush, 110, 980);
                Pagina.Canvas.DrawText(datosCalculo.DuracionVisita.ToString(), _Arial, null, brush, 550, 980);
                if (datosCalculo.IdTramite == 26)
                {
                    Pagina.Canvas.DrawText("Cantidad normas               ", _Arial, null, brush, 110, 1020);
                    Pagina.Canvas.DrawText(datosCalculo.CantNormas.ToString(), _Arial, null, brush, 550, 1020);
                    Pagina.Canvas.DrawText("Cantidad líneas               ", _Arial, null, brush, 110, 1060);
                    Pagina.Canvas.DrawText(datosCalculo.CantLineas.ToString(), _Arial, null, brush, 550, 1060);
                }
                Pagina.Canvas.DrawText("Observación                ", _Arial, null, brush, 110, 1100);
                PDFTextFormatOptions tfo = new PDFTextFormatOptions();
                tfo.Align = PDFTextAlign.TopJustified;
                tfo.ClipText = PDFClipText.ClipNone;
                Pagina.Canvas.DrawTextBox(datosCalculo.Observaciones != null ? datosCalculo.Observaciones.Trim().ToUpper(): "", _Arial, brush, 200, 1100, 2000, 200, tfo);
                Pagina.Canvas.DrawRectangle(_Pen, null, (Pagina.Width / 2) - 800, 1350, 1500, 100, 0);
                Pagina.Canvas.DrawText("CALCULO DEL VALOR DEL SEGUIMIENTO", _Arial, null, brush, 1000, 1360);
                Pagina.Canvas.DrawText("ITEM                                                                                   VALOR", _Arial, null, brush, 900, 1400);
                Pagina.Canvas.DrawRectangle(_Pen, null, (Pagina.Width / 2) - 800, 1450, 1500, 300, 0);
                Pagina.Canvas.DrawText("GASTOS POR SUELDOS Y HONORARIOS (A) :                                   ", _Arial, null, brush, 500, 1470);
                Pagina.Canvas.DrawText(datosCalculo.Salarios.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1470);
                Pagina.Canvas.DrawText("GASTOS DE VIAJE (B) :                                                   ", _Arial, null, brush, 500, 1510);
                Pagina.Canvas.DrawText(datosCalculo.Transporte.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1510);
                Pagina.Canvas.DrawText("GASTOS DE ANÁLISIS DE LABORATORIO Y OTROS TRABAJOS TÉCNICOS (C) :       ", _Arial, null, brush, 500, 1550);
                Pagina.Canvas.DrawText(datosCalculo.Laboratorio.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1550);
                Pagina.Canvas.DrawText("GASTOS DE ADMINISTRACIÓN 25% (D) :                                      ", _Arial, null, brush, 500, 1590);
                Pagina.Canvas.DrawText(datosCalculo.Administracion.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1590);
                Pagina.Canvas.DrawText("COSTO TOTAL DE LA TARIFA :                                              ", _Arial, null, brush, 500, 1630);
                Pagina.Canvas.DrawText(datosCalculo.TotalNeto.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1630);
                Pagina.Canvas.DrawText("DETERMINACIÓN DE LOS TOPES DE LAS TÁRIFAS (To) :                        ", _Arial, null, brush, 500, 1670);
                Pagina.Canvas.DrawText(datosCalculo.TopeDeterminado.ToString("C", cultureInfo), _Arial, null, brush, 1700, 1670);
                Pagina.Canvas.DrawText("VALOR A CANCELAR POR TRÁMITE :                                          ", _Arial, null, brush, 500, 1710);
                Pagina.Canvas.DrawText(datosCalculo.TotalPagar, _Arial, null, brush, 1700, 1710);
            }
            _Doc.Save(oStream);
            oStream.Seek(0, SeekOrigin.Begin);
            return oStream.ToArray();
        }

        private decimal ObtenerHonorarios(string Ano)
        {
            try
            {
                var Salario = (from Sal in dbSIM.TBTARIFAS_PARAMETRO
                               where Sal.NOMBRE == "SALARIO" && Sal.ACTIVO == "1" && Sal.ANO == Ano
                               select Sal.VALOR).FirstOrDefault();
                if (Salario > 0) return Salario;
                else return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private int ObtenerHorasAbogadoSeguimiento(decimal TipoTramite)
        {
            var HorasAbogado = (from Hor in dbSIM.TBTARIFAS_TRAMITE
                                where Hor.CODIGO_TRAMITE == TipoTramite && Hor.TIPO_ACTUACION == "S"
                                select new { Valor = Hor.AUTO_INICIO + Hor.RESOLUCION }).FirstOrDefault();
            if (HorasAbogado.Valor > 0) return HorasAbogado.Valor;
            else return 0;
        }

        private decimal ObtenerPasaje(string Ano)
        {
            try
            {
                var Pasaje = (from Pas in dbSIM.TBTARIFAS_PARAMETRO
                              where (Pas.NOMBRE == "PASAJE") && (Pas.ACTIVO == "1") && (Pas.ANO == Ano)
                              select Pas.VALOR).FirstOrDefault();
                if (Pasaje > 0) return Pasaje;
                else return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private List<TBTARIFAS_TOPES> ObtenerTopes()
        {
            try
            {
                var topes = (from Top in dbSIM.TBTARIFAS_TOPES
                             orderby Top.ID_TOPE
                             select Top).ToList();
                if (topes != null) return topes;
                else return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private decimal ObtenerMinimo(string Ano)
        {
            try
            {
                var Minimo = (from Min in dbSIM.TBTARIFAS_PARAMETRO
                              where Min.NOMBRE == "SMMLV" && Min.ACTIVO == "1" && Min.ANO == Ano
                              select Min.VALOR).FirstOrDefault();
                if (Minimo > 0) return Minimo;
                else return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private string ObtenerNombreTramite(decimal TipoTramite)
        {
            var Tramite = (from Tra in dbSIM.TBTARIFAS_TRAMITE
                                where Tra.CODIGO_TRAMITE == TipoTramite && Tra.TIPO_ACTUACION == "S"
                                select Tra.NOMBRE).FirstOrDefault();
            if (Tramite != null) return Tramite;
            else return "";
        }

        private string UnidadMedidaItems(decimal TipoTramite)
        {
            var Unidad = (from Tra in dbSIM.TBTARIFAS_TRAMITE
                           where Tra.CODIGO_TRAMITE == TipoTramite 
                           select Tra.S_UNIDAD).FirstOrDefault();
            if (Unidad != null) return Unidad;
            else return "";
        }

        private decimal ObtenerRelacionSeguimiento(decimal TipoTramite)
        {
            var Relacion = (from Rel in dbSIM.TBTARIFAS_TRAMITE
                                where Rel.CODIGO_TRAMITE == TipoTramite && Rel.TIPO_ACTUACION == "S"
                                select new { Valor = Rel.N_RELACION }).FirstOrDefault();
            if (Relacion.Valor >= 0) return Relacion.Valor.Value;
            else return 0;
        }
    }
}