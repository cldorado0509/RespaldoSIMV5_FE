using O2S.Components.PDF4NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIM.Areas.Facturacion.Models;
using O2S.Components.PDF4NET.Graphics.Shapes;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics;
using SIM.Data;
using SIM.Areas.Tramites.Models;
using System.Data;
using System.IO;
using System.Web.Hosting;
using System.Drawing;
using AreaMetro.Utilidades;
using SIM.Data.Facturacion;
using SIM.Data.Tramites;

namespace SIM.Utilidades
{
    public class Facturacion
    {
        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Genera una factura especifica
        /// </summary>
        /// <param name="_IdFact"></param>
        /// <returns></returns>
        public MemoryStream GeneraFact(long _IdFact)
        {
            PDFDocument _Doc = new PDFDocument();
            _Doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            MemoryStream oStream = new MemoryStream();
            var _ModFact = (from Fact in dbSIM.FACTURA
                            where Fact.ID_FACTURA == _IdFact
                            select  Fact).FirstOrDefault();
            if (_ModFact != null)
            {
                PDFPage Pagina = _Doc.AddPage();
                Pagina.Width = 2550;
                Pagina.Height = 3300;
                PDFImage _img = new PDFImage(HostingEnvironment.MapPath(@"~/Content/Images/Logo_Area.png"));
                Pagina.Canvas.DrawImage(_img, 100, 100, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
                TrueTypeFont _Arial = new TrueTypeFont(HostingEnvironment.MapPath(@"~/fonts/arialbd.ttf"), 80, true, true);
                _Arial.Bold = true;
                PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
                _Arial.Bold = true;
                Pagina.Canvas.DrawText("ÁREA METROPOLITANA DEL", _Arial, null, brush, 600, 120);
                Pagina.Canvas.DrawText("VALLE DE ABURRÁ", _Arial, null, brush, 780, 210);
                _Arial.Bold = false;
                _Arial.Size = 50;
                Pagina.Canvas.DrawText("FACTURA DE VENTA", _Arial, null, brush, 1900, 160);
                Pagina.Canvas.DrawText("N°", _Arial, null, brush, 1900, 220);
                Pagina.Canvas.DrawText(_ModFact.N_FACTURA.ToString(), _Arial, null, brush, 1970, 220);  //Variable
                _Arial.Size = 40;
                Pagina.Canvas.DrawText("Entidad Administrativa de Derecho Público", _Arial, null, brush, 750, 290);
                Pagina.Canvas.DrawText("IVA Régimen común", _Arial, null, brush, 970, 330);
                _Arial.Size = 30;
                Pagina.Canvas.DrawText("NIT  890984423-3", _Arial, null, brush, 110, 500);
                Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 110, 540);
                Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 110, 580);
                Pagina.Canvas.DrawText("Colombia", _Arial, null, brush, 110, 620);
                Pagina.Canvas.DrawText("Tel. (574) 385 6000", _Arial, null, brush, 110, 660);
                Pagina.Canvas.DrawText("Fax (574) 261 1009", _Arial, null, brush, 110, 700);
                _Arial.Size = 40;
                _Arial.Bold = true;
                Pagina.Canvas.DrawText("Datos", _Arial, null, brush, 550, 500);
                Pagina.Canvas.DrawText("Cliente", _Arial, null, brush, 550, 550);
                PDFPen _Pen = new PDFPen(new PDFRgbColor(Color.Black), 5);
                Pagina.Canvas.DrawLine(_Pen, 720, 500, 720, 745);
                _Arial.Bold = false;
                if (_ModFact.S_TERCERO.Trim().Length <= 68) Pagina.Canvas.DrawText(_ModFact.S_TERCERO.Trim().ToUpper(), _Arial, null, brush, 750, 500); // Variable
                else
                {
                    string _Arriba = "";
                    string _Abajo = "";
                    int _longitud = 0;
                    string[] _PalTercero = _ModFact.S_TERCERO.Trim().ToUpper().Split(' ');
                    foreach (string _pal in _PalTercero)
                    {
                        if (_longitud <= 57) _Arriba += _pal + " ";
                        if (_longitud > 57) _Abajo += _pal + " ";
                        _longitud += _pal.Length;
                    }
                    Pagina.Canvas.DrawText(_Arriba, _Arial, null, brush, 750, 500); // Variable
                    Pagina.Canvas.DrawText(_Abajo, _Arial, null, brush, 750, 540); // Variable
                }
                if (_ModFact.S_DIGITO != null) Pagina.Canvas.DrawText(_ModFact.N_NUIP.ToString() + "-" + _ModFact.S_DIGITO.Substring(0, 1), _Arial, null, brush, 750, 600); // Variable
                else Pagina.Canvas.DrawText(_ModFact.N_NUIP.ToString(), _Arial, null, brush, 750, 600); // Variable
                Pagina.Canvas.DrawText(_ModFact.S_DIRECCIONTERCERO.Trim().ToUpper(), _Arial, null, brush, 750, 640); // Variable
                Pagina.Canvas.DrawText(_ModFact.S_CIUDAD.Trim() + " - " + _ModFact.S_DEPARTAMENTO.Trim(), _Arial, null, brush, 750, 680); // Variable
                Pagina.Canvas.DrawText("Teléfono : " + _ModFact.S_TELEFONOTERCERO.Trim(), _Arial, null, brush, 750, 720); // Variable
                _Pen.Width = 3;
                _Arial.Size = 30;
                Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 565, 600, 180, 0);
                Pagina.Canvas.DrawText("Fecha Elaboración", _Arial, null, brush, 1810, 580);
                Pagina.Canvas.DrawText(_ModFact.D_ELABORACION.Value.ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 580); //Variable
                Pagina.Canvas.DrawLine(_Pen, 1800, 620, 2400, 620);
                Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 1810, 640);
                Pagina.Canvas.DrawText(_ModFact.D_PAGO.Value.ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 640); //Variable
                Pagina.Canvas.DrawLine(_Pen, 1800, 680, 2400, 680);
                Pagina.Canvas.DrawText("Cuentas Vencidas", _Arial, null, brush, 1810, 700);
                if (_ModFact.N_FACTURASVENCIDAS > 0)
                {
                    string _CuentasVenc = _ModFact.N_FACTURASVENCIDAS.ToString() + " (" + _ModFact.N_VALORFACTURASVENCIDAS.Value.ToString("C") + ")";
                    if (_CuentasVenc.Length > 20) Pagina.Canvas.DrawText(_CuentasVenc, _Arial, null, brush, 2070, 700); //Variable 
                    else if (_CuentasVenc.Length > 15) Pagina.Canvas.DrawText(_CuentasVenc, _Arial, null, brush, 2090, 700); //Variable 
                    else Pagina.Canvas.DrawText(_CuentasVenc, _Arial, null, brush, 2110, 700); //Variable
                }
                else Pagina.Canvas.DrawText("0 ($0)", _Arial, null, brush, 2180, 700); //Variable
                _Arial.Size = 35;
                _Arial.Bold = true;
                //Pagina.Canvas.DrawText("GRANDES CONTRIBUYENTES AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 500, 800);
                Pagina.Canvas.DrawText("                       AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 500, 800);
                Pagina.Canvas.DrawLine(_Pen, 100, 850, 2400, 850);
                Pagina.Canvas.DrawRectangle(_Pen, null, 100, 870, 2300, 900, 0);
                Pagina.Canvas.DrawLine(_Pen, 100, 930, 2400, 930);
                _Arial.Size = 30;
                Pagina.Canvas.DrawText("CONCEPTO", _Arial, null, brush, 115, 890);
                Pagina.Canvas.DrawLine(_Pen, 300, 870, 300, 1770);
                Pagina.Canvas.DrawText("DESCRIPCIÓN", _Arial, null, brush, 600, 890);
                Pagina.Canvas.DrawLine(_Pen, 1150, 870, 1150, 1770);
                Pagina.Canvas.DrawText("DETALLE", _Arial, null, brush, 1550, 890);
                Pagina.Canvas.DrawLine(_Pen, 2100, 870, 2100, 1770);
                Pagina.Canvas.DrawText("TOTAL", _Arial, null, brush, 2200, 890);
                Pagina.Canvas.DrawRectangle(_Pen, null, 100, 1770, 2300, 60, 0);
                Pagina.Canvas.DrawText("VALOR EN LETRAS :", _Arial, null, brush, 110, 1795);
                Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 1830, 600, 60, 0);
                Pagina.Canvas.DrawText("TOTAL  ", _Arial, null, brush, 1840, 1850);
                _Arial.Bold = false;
                // INICIA CICLO PARA EL DETALLE POR CADA ITEM DE LA FACTURA
                int _Linea = 960;
                long _ValFact = 0;
                var _DetFact = (from DetFact in dbSIM.DETALLE_FACTURA
                                where DetFact.ID_FACTURA == _ModFact.ID_FACTURA
                                select DetFact);
                if (_DetFact != null)
                {
                    foreach (var det in _DetFact)
                    {
                        Pagina.Canvas.DrawText(det.N_CODSICOF.ToString().Trim(), _Arial, null, brush, 145, _Linea); //Variable
                        if (det.S_NOMBRE.ToString().Trim().Length < 43) Pagina.Canvas.DrawText(det.S_NOMBRE.ToString().Trim().ToUpper(), _Arial, null, brush, 310, _Linea);  //Variable
                        else
                        {
                            Pagina.Canvas.DrawText(det.S_NOMBRE.ToString().Trim().Substring(0, 42).ToUpper(), _Arial, null, brush, 310, _Linea);  //Variable
                            Pagina.Canvas.DrawText(det.S_NOMBRE.ToString().Trim().Substring(41, det.S_NOMBRE.ToString().Trim().Length - 42).ToUpper(), _Arial, null, brush, 310, _Linea + 40);  //Variable
                        }
                        long _Valor = long.Parse(det.N_COSTO.ToString());
                        _ValFact += _Valor;
                        Pagina.Canvas.DrawText(_Valor.ToString("C"), _Arial, null, brush, 2110, _Linea); //Variable
                        _Linea += 140;
                    }
                }
                string _Descripcion = _ModFact.S_DESCRIPCION.ToString().Trim().ToUpper();
                if (_Descripcion.Length > 500) _Descripcion = _Descripcion.Substring(0, 500);
                int _ExtTexto = 50;
                string Aux = "";
                int _PosVert = 960;
                string[] _Palabras = _Descripcion.Split(' ');
                foreach (string Pal in _Palabras)
                {
                    Aux = Aux + " " + Pal;
                    if (Aux.Length > _ExtTexto)
                    {
                        Aux = Aux.Substring(0, Aux.Length - (Pal.Length + 1));
                        Pagina.Canvas.DrawText(Aux, _Arial, null, brush, 1160, _PosVert); //Variable
                        Aux = Pal;
                        _PosVert += 40;
                    }
                    else if (Aux.Length == _ExtTexto)
                    {
                        Pagina.Canvas.DrawText(Aux, _Arial, null, brush, 1160, _PosVert); //Variable
                        _PosVert += 40;
                        Aux = "";
                    }
                }
                if (Aux != "") Pagina.Canvas.DrawText(Aux, _Arial, null, brush, 1160, _PosVert); //Variable
                                                                                                 // TERMINA CICLO
                Pagina.Canvas.DrawText(enletras(_ValFact.ToString().Trim()), _Arial, null, brush, 450, 1795);  //Variable
                Pagina.Canvas.DrawText(_ValFact.ToString("C"), _Arial, null, brush, 2110, 1850);  //Variable
                Pagina.Canvas.DrawText("Por favor conserve la parte superior para verificar su pago", _Arial, null, brush, 145, 2000);
                var _strClave = (from _Clave in dbSIM.TIPO_FACTURA
                                 where _Clave.ID_TIPOFACTURA == _ModFact.ID_TIPOFACTURA
                                 select _Clave.S_CLAVE).FirstOrDefault();
                if (!string.IsNullOrEmpty(_strClave))
                {
                    string _NIT = _ModFact.N_NUIP.Value.ToString("00000000000");
                    string _FACT = _ModFact.N_FACTURA.Value.ToString("000000000");
                    string _ValorF = _ValFact.ToString();
                    if ((_ValorF.Length % 2) == 1) _ValorF = "0" + _ValorF;
                    string _FechaPago = _ModFact.D_PAGO.Value.ToString("yyyyMMdd");
                    string _TextCode = "415" + _strClave + "8020" + _FACT + _NIT + "#3900" + _ValorF + "#96" + _FechaPago; //Variable
                    Image _ImgBc = AreaMetro.Utilidades.CodeBar.CodeBar128Fact(_TextCode, _ModFact.D_PAGO.Value, new System.Drawing.Size(1300, 250));
                    PDFImage _BcImg = new PDFImage((Bitmap)_ImgBc);
                    Pagina.Canvas.DrawImage(_BcImg, 1000, 1910, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
                    Pagina.Canvas.DrawImage(_BcImg, 1000, 2510, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
                }
                _Arial.Size = 40;
                _Arial.Bold = true;
                _Pen.DashStyle = PDFDashStyle.Dash;
                Pagina.Canvas.DrawLine(_Pen, 100, 2220, 2400, 2220);
                _Pen.DashStyle = PDFDashStyle.Solid;
                Pagina.Canvas.DrawText("CUPÓN DE PAGO", _Arial, null, brush, 1100, 2250);
                Pagina.Canvas.DrawRectangle(_Pen, null, 135, 2350, 700, 240, 0);
                Pagina.Canvas.DrawRectangle(_Pen, null, 1500, 2350, 700, 120, 0);
                _Arial.Size = 30;
                Pagina.Canvas.DrawText("Factura N°", _Arial, null, brush, 145, 2370);
                Pagina.Canvas.DrawLine(_Pen, 450, 2350, 450, 2590);
                Pagina.Canvas.DrawLine(_Pen, 135, 2410, 835, 2410);
                Pagina.Canvas.DrawText("Banco", _Arial, null, brush, 1510, 2370);
                Pagina.Canvas.DrawLine(_Pen, 1780, 2350, 1780, 2470);
                Pagina.Canvas.DrawLine(_Pen, 1500, 2410, 2200, 2410);
                Pagina.Canvas.DrawText("NIT Cliente", _Arial, null, brush, 145, 2430);
                Pagina.Canvas.DrawLine(_Pen, 135, 2470, 835, 2470);
                Pagina.Canvas.DrawText("Cuenta Ahorros N°", _Arial, null, brush, 1510, 2430);
                Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 145, 2490);
                Pagina.Canvas.DrawLine(_Pen, 135, 2530, 835, 2530);
                Pagina.Canvas.DrawText("Valor Total", _Arial, null, brush, 145, 2550);
                _Arial.Bold = false;
                Pagina.Canvas.DrawText(_ModFact.N_FACTURA.ToString(), _Arial, null, brush, 490, 2370); //Variable
                var _Banco = (from _Ban in dbSIM.TIPO_FACTURA
                              where _Ban.ID_TIPOFACTURA == _ModFact.ID_TIPOFACTURA
                              select _Ban.S_BANCO).FirstOrDefault();
                if (_Banco != "")
                {
                    Pagina.Canvas.DrawText(_Banco.Trim().ToUpper(), _Arial, null, brush, 1820, 2370); //Variable
                }
                if (_ModFact.S_DIGITO != null) Pagina.Canvas.DrawText(_ModFact.N_NUIP.ToString().Trim() + "-" + _ModFact.S_DIGITO.Substring(0, 1), _Arial, null, brush, 490, 2430); //Variable
                else Pagina.Canvas.DrawText(_ModFact.N_NUIP.ToString().Trim(), _Arial, null, brush, 490, 2430); //Variable
                var _Cuenta = (from Cta in dbSIM.TIPO_FACTURA
                               where Cta.ID_TIPOFACTURA == _ModFact.ID_TIPOFACTURA
                               select Cta.S_CUENTA).FirstOrDefault();
                if (_Cuenta != "")
                {
                    Pagina.Canvas.DrawText(_Cuenta.Trim(), _Arial, null, brush, 1820, 2430); //Variable
                }
                Pagina.Canvas.DrawText(_ModFact.D_PAGO.Value.ToString("yyyy/MM/dd"), _Arial, null, brush, 490, 2490); //Variable
                Pagina.Canvas.DrawText(_ValFact.ToString("C"), _Arial, null, brush, 490, 2550); //Variable
                Pagina.Canvas.DrawLine(_Pen, 100, 2770, 2400, 2770);
                Pagina.Canvas.DrawImage(_img, 100, 2790, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
                Pagina.Canvas.DrawLine(_Pen, 650, 2790, 650, 3157);
                _Arial.Size = 25;
                Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 700, 2950);
                Pagina.Canvas.DrawText("Ed. Área Metropolitana", _Arial, null, brush, 700, 3000);
                Pagina.Canvas.DrawText("NIT 890984423-3", _Arial, null, brush, 700, 3050);
                Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 700, 3100);
                Pagina.Canvas.DrawLine(_Pen, 1350, 2790, 1350, 3157);
                Pagina.Canvas.DrawText("IVA Régimen Común", _Arial, null, brush, 1400, 2900);
                // Pagina.Canvas.DrawText("GRANDES CONTRIBUYENTES", _Arial, null, brush, 1400, 2950);
                Pagina.Canvas.DrawText("AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 1400, 3000);
                Pagina.Canvas.DrawText("metropol@metropol.gov.co", _Arial, null, brush, 1400, 3050);
                Pagina.Canvas.DrawText("http://www.metropol.gov.co", _Arial, null, brush, 1400, 3100);
                _Doc.Save(oStream);
            }
            else return null;
            if (oStream.Length > 0) oStream.Position = 0;
            return oStream;
        }

        /// <summary>
        /// Determina si una factura esta en la base de datos local de facturacion
        /// </summary>
        /// <param name="_Factura">Numero de la factura</param>
        /// <param name="_Ano">Año de la factura </param>
        /// <returns></returns>
        public long ObtenerIdFactura(string _Factura, int _Ano)
        {
            if (string.IsNullOrEmpty(_Factura) || _Ano < 1900) return -1;
            int _NumFactura = int.Parse(_Factura);
            var _ModFact = (from Fact in dbSIM.FACTURA
                            where Fact.N_FACTURA.Value == _NumFactura && Fact.D_ELABORACION.Value.Year == _Ano
                            select Fact.ID_FACTURA).FirstOrDefault();
            if (_ModFact > 0) return _ModFact;
            else return -1;
        }

        /// <summary>
        /// Importa la factura desde el SICOF y retorna el identificador con el queda en el SIM, si no es posible retorna -1
        /// </summary>
        /// <param name="Factura">Numero de factura</param>
        /// <param name="Ano">Año de la factura</param>
        /// <returns></returns>
        public long ImportarFacturaContabilidad(string Factura, int Ano)
        {
            long IdFactura = -1;
            if (string.IsNullOrEmpty(Factura) || Ano < 1900) return IdFactura;
            string Sql = "SELECT NO_FACTURA,FECHA,FECHA_VENCE,DESCRIPCION,CODIGO_CONCEPTO,DESCRP_CONCEPTO,VALOR_CONCEPTO,CODIGO_COMERCIAL,TO_CHAR(NIT),NOMBRE,DIRECCION,TELEFONO,CUENTAS_VENCIDAS,VALOR_VENCIDO,CIUDAD,DEPARTAMENTO FROM SFCOMERCIAL.V_FACTURAS_SICOF WHERE NO_FACTURA=" + Factura.Trim() + " AND EXTRACT(YEAR FROM FECHA)=" + Ano;
            var _Factura = dbSIM.Database.SqlQuery<FactCont>(Sql).ToArray();
            if (_Factura.Length > 0)
            {
                long _IdTipoFactura = ObtenerIdTipoFact(long.Parse(_Factura[0].CODIGO_CONCEPTO.ToString()));
                if (_IdTipoFactura > 0)
                {
                    FACTURA _FacturaNew = new FACTURA();
                    _FacturaNew.ID_TIPOFACTURA = Convert.ToInt32(_IdTipoFactura);
                    _FacturaNew.CODIGO_COMERCIAL = _Factura[0].CODIGO_COMERCIAL;
                    _FacturaNew.S_DESCRIPCION = _Factura[0].DESCRIPCION;
                    string nuip = _Factura[0].NIT;
                    if (nuip.Trim() != "")
                    {
                        string[] words = nuip.Split(',');
                        nuip = words[0].Trim();
                        if (words.Length > 1) _FacturaNew.S_DIGITO = words[1].Trim();
                        else _FacturaNew.S_DIGITO = "";
                        _FacturaNew.N_NUIP = long.Parse(words[0].Trim());
                    }
                    _FacturaNew.D_PAGO = _Factura[0].FECHA_VENCE;
                    _FacturaNew.D_ELABORACION = _Factura[0].FECHA;
                    _FacturaNew.N_FACTURA = _Factura[0].NO_FACTURA;
                    _FacturaNew.S_TERCERO = _Factura[0].NOMBRE;
                    _FacturaNew.S_DIRECCIONTERCERO = _Factura[0].DIRECCION;
                    _FacturaNew.S_TELEFONOTERCERO = _Factura[0].TELEFONO;
                    _FacturaNew.S_CIUDAD = _Factura[0].CIUDAD;
                    _FacturaNew.S_DEPARTAMENTO = _Factura[0].DEPARTAMENTO;
                    _FacturaNew.S_TIPOTERCERO = "J";
                    long _IdTer = 0;
                    if (_FacturaNew.S_DIGITO.Length > 0) _IdTer = IdTerceroSIM(Convert.ToInt32(_FacturaNew.N_NUIP), long.Parse(_FacturaNew.S_DIGITO.Substring(0, 1)));
                    else _IdTer = IdTerceroSIM(Convert.ToInt32(_FacturaNew.N_NUIP), -1);
                    if (_IdTer > 0) _FacturaNew.ID_TERCEROSIM = _IdTer;
                    _FacturaNew.N_FACTURASVENCIDAS = _Factura[0].CUENTAS_VENCIDAS;
                    _FacturaNew.N_VALORFACTURASVENCIDAS = _Factura[0].VALOR_VENCIDO;
                    try
                    {
                        dbSIM.FACTURA.Add(_FacturaNew);
                        dbSIM.SaveChanges();
                        DETALLE_FACTURA _ModDetFact = new DETALLE_FACTURA();
                        int orden = 1;
                        foreach (FactCont _Det in _Factura)
                        {
                            _ModDetFact.ID_FACTURA = _FacturaNew.ID_FACTURA;
                            _ModDetFact.N_ORDEN = orden++;
                            _ModDetFact.S_NOMBRE = _Det.DESCRP_CONCEPTO;
                            if (_Det.VALOR_CONCEPTO > 0)
                            {
                                _ModDetFact.N_COSTO = _Det.VALOR_CONCEPTO;
                                _ModDetFact.N_SUBTOTAL = _Det.VALOR_CONCEPTO;
                            }
                            else
                            {
                                _ModDetFact.N_COSTO = 0;
                                _ModDetFact.N_SUBTOTAL = 0;
                            }
                            _ModDetFact.S_TIPO = "0";
                            _ModDetFact.N_CODSICOF = Convert.ToInt32(_Det.CODIGO_CONCEPTO);
                            _ModDetFact.S_DESCRIPCION = "";
                            dbSIM.DETALLE_FACTURA.Add(_ModDetFact);
                            dbSIM.SaveChanges();
                            _ModDetFact = new DETALLE_FACTURA();
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
            return IdFactura;
        }
        /// <summary>
        /// Retorna el tipo de factura apartir del codigo de convcepto
        /// </summary>
        /// <param name="CodigoConcepto"></param>
        /// <returns></returns>
        public long ObtenerIdTipoFact(long CodigoConcepto)
        {
            var _Tipofact = (from TipoF in dbSIM.CONCEPTO_TIPOFACTURA
                             where TipoF.ID_CONCEPTOTIPO == CodigoConcepto
                             select TipoF.ID_TIPOFACTURA).FirstOrDefault();
            return _Tipofact;
        }
        /// <summary>
        /// Obtiene el identificador del trecero en el SIM
        /// </summary>
        /// <param name="_Nit">Nit sin digito de verificacion</param>
        /// <param name="_Dig">digito de verificacion</param>
        /// <returns></returns>
        public long IdTerceroSIM(long _Nit, long _Dig)
        {
            long _NitN = 0;
            if (_Dig > 0) _NitN = long.Parse(_Nit.ToString() + _Dig.ToString()); else _NitN = _Nit;

            var _IdTercSIM = (from Tercero in dbSIM.TERCERO
                             where Tercero.N_DOCUMENTO == _NitN || Tercero.N_DOCUMENTON == _Nit
                             select Tercero.ID_TERCERO).FirstOrDefault();
            return _IdTercSIM;
        }
        /// <summary>
        /// Convierte a letras un valor especifico
        /// </summary>
        /// <param name="num">El valor a convertir</param>
        /// <returns></returns>
        public static string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;
            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }
            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0) dec = " CON " + decimales.ToString() + "/100";
            res = toText(Convert.ToDouble(entero)) + dec;
            return res + " PESOS";
        }
        /// <summary>
        /// Retorna el valor en letras de una cantidad dada
        /// </summary>
        /// <param name="value">Valor ingresado</param>
        /// <returns></returns>
        private static string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }
        /// <summary>
        /// Permite asociar un informe tecnico con su factura
        /// </summary>
        /// <param name="_Factura">Numero de factura</param>
        /// <param name="_Ano">Año factura</param>
        /// <param name="_CodTramite">Codigo del tramite</param>
        /// <param name="_CodDocumento">Codigo del documento</param>
        /// <returns></returns>
        public string AsociaInformeFacturaInf(string _Factura, int _Ano, long _CodTramite, long _CodDocumento)
        {
            string _Respuesta = "";
            long _IdIndiceFacturaInforme = long.Parse(Data.ObtenerValorParametro("IdIndiceFacturaInforme"));
            var Indices = (from ind in dbSIM.TBINDICEDOCUMENTO
                           where ind.CODTRAMITE == _CodTramite && ind.CODDOCUMENTO == _CodDocumento && ind.CODINDICE == _IdIndiceFacturaInforme
                           select ind).FirstOrDefault();
            if (Indices != null)
            {
                Indices.VALOR = _Factura + "-" + _Ano;
                dbSIM.SaveChanges();
                _Respuesta = "Factura asociada correctamente";
            }
            else
            {
                TBINDICEDOCUMENTO IndiceNew = new TBINDICEDOCUMENTO();
                IndiceNew.CODDOCUMENTO = Convert.ToInt32(_CodDocumento);
                IndiceNew.CODTRAMITE = Convert.ToInt32(_CodTramite);
                IndiceNew.CODINDICE = Convert.ToInt32(_IdIndiceFacturaInforme);
                IndiceNew.VALOR = _Factura + "-" + _Ano;
                IndiceNew.FECHAREGISTRO = DateTime.Now;
                dbSIM.TBINDICEDOCUMENTO.Add(IndiceNew);
                dbSIM.SaveChanges();
                _Respuesta = "Factura asociada correctamente";
            }
            return _Respuesta;
        }
        /// <summary>
        /// Permite asociar una resolucion con su factura
        /// </summary>
        /// <param name="_Factura">Numero de factura</param>
        /// <param name="_Ano">Año factura</param>
        /// <param name="_CodTramite">Codigo del tramite</param>
        /// <param name="_CodDocumento">Codigo del documento</param>
        /// <returns></returns>
        public string AsociaInformeFacturaRes(string _Factura, int _Ano, long _CodTramite, long _CodDocumento)
        {
            string _Respuesta = "";
            long _IdIndiceFacturaInforme = long.Parse(Data.ObtenerValorParametro("IdIndiceFacturaResolucion"));
            if (_IdIndiceFacturaInforme > 0)
            {
                var TipoDc = (from Tip in dbSIM.TBINDICESERIE
                              where Tip.CODINDICE == _IdIndiceFacturaInforme
                              select Tip.CODSERIE).FirstOrDefault();
                var Indices = (from ind in dbSIM.TBINDICEDOCUMENTO
                               where ind.CODTRAMITE == _CodTramite && ind.CODDOCUMENTO == _CodDocumento && ind.CODINDICE == _IdIndiceFacturaInforme
                               select ind).FirstOrDefault();

                if (Indices != null)
                {
                    Indices.VALOR = _Factura + "-" + _Ano;
                    dbSIM.SaveChanges();
                    _Respuesta = "Factura asociada correctamente";
                }
                else
                {
                    TBINDICEDOCUMENTO IndiceNew = new TBINDICEDOCUMENTO();
                    IndiceNew.CODDOCUMENTO = Convert.ToInt32(_CodDocumento);
                    IndiceNew.CODTRAMITE = Convert.ToInt32(_CodTramite);
                    IndiceNew.CODINDICE = Convert.ToInt32(_IdIndiceFacturaInforme);
                    IndiceNew.CODSERIE = TipoDc;
                    IndiceNew.VALOR = _Factura + "-" + _Ano;
                    IndiceNew.FECHAREGISTRO = DateTime.Now;
                    dbSIM.TBINDICEDOCUMENTO.Add(IndiceNew);
                    dbSIM.SaveChanges();
                    _Respuesta = "Factura asociada correctamente";
                }
            }
            return _Respuesta;
        }
        /// <summary>
        /// Permite guardar el valor generado para una factura
        /// </summary>
        /// <param name="Codtramite">Codigo del tramite</param>
        /// <param name="CodDocumento">Codigo del documento</param>
        /// <param name="Valor"></param>
        /// <returns></returns>
        public string GuardaValorFacturaInf(long Codtramite, long CodDocumento, long Valor)
        {
            string _Respuesta = "";
            long _IdIndiceValFactInforme = long.Parse(Data.ObtenerValorParametro("IdIndiceValorFactInforme"));
            if (_IdIndiceValFactInforme > 0)
            {
                var TipoDc = (from Tip in dbSIM.TBINDICESERIE
                              where Tip.CODINDICE == _IdIndiceValFactInforme
                              select Tip.CODSERIE).FirstOrDefault();
                var Indices = (from ind in dbSIM.TBINDICEDOCUMENTO
                               where ind.CODTRAMITE == Codtramite && ind.CODDOCUMENTO == CodDocumento && ind.CODINDICE == _IdIndiceValFactInforme
                               select ind).FirstOrDefault();

                if (Indices != null)
                {
                    Indices.VALOR = Valor.ToString();
                    dbSIM.SaveChanges();
                    _Respuesta = "Valor de Factura guardado correctamente";
                }
                else
                {
                    TBINDICEDOCUMENTO IndiceNew = new TBINDICEDOCUMENTO();
                    IndiceNew.CODDOCUMENTO = Convert.ToInt32(CodDocumento);
                    IndiceNew.CODTRAMITE = Convert.ToInt32(Codtramite);
                    IndiceNew.CODINDICE = Convert.ToInt32(_IdIndiceValFactInforme);
                    IndiceNew.CODSERIE = TipoDc;
                    IndiceNew.VALOR = Valor.ToString();
                    IndiceNew.FECHAREGISTRO = DateTime.Now;
                    dbSIM.TBINDICEDOCUMENTO.Add(IndiceNew);
                    dbSIM.SaveChanges();
                    _Respuesta = "Valor de Factura guardado correctamente";
                }
            }
            return _Respuesta;
        }
        /// <summary>
        /// Obtien el valor del CM del informe tecnico especifico
        /// </summary>
        /// <param name="Codtramite">Codigo del tramite</param>
        /// <param name="CodDocumento">Codigo del documento</param>
        /// <returns></returns>
        public string ObtieneCMInforme(long CodTramite, long CodDocumento)
        {
            string _Respuesta = "";
            long _IdIndiceCMInforme = long.Parse(Data.ObtenerValorParametro("IdIndiceCMInforme"));
            if (_IdIndiceCMInforme > 0)
            {
                var Indices = (from ind in dbSIM.TBINDICEDOCUMENTO
                               where ind.CODTRAMITE == CodTramite && ind.CODDOCUMENTO == CodDocumento && ind.CODINDICE == _IdIndiceCMInforme
                               select ind.VALOR).FirstOrDefault();
                _Respuesta = Indices;
            }
            return _Respuesta;
        }
    }
}