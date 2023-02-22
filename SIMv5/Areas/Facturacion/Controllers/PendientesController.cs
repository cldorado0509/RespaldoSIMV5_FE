using SIM.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Facturacion.Controllers
{
    public class PendientesController : Controller
    {
        // GET: Facturacion/Pendientes
        [Authorize(Roles = "VFACTURACION")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Facturacion/Asociadas
        [Authorize(Roles = "VFACTURACION")]
        public ActionResult Asociadas()
        {
            return View();
        }

        /// <summary>
        /// Consulta un archivo del sistema y lo retorna en arreglo de bytes
        /// </summary>
        /// <param name="CodTramite">Codigo del tramite en el sistema</param>
        /// <param name="Coddocuemnto">Numero de documento dentro del tramite</param>
        /// <returns></returns>
        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("LeeFactura")]
        public ActionResult GetFactura(string Factura)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            if (string.IsNullOrEmpty(Factura)) return null;
            MemoryStream oStream = new MemoryStream();
            string[] _Factura = Factura.Split('-');
            if (_Factura.Length == 2)
            {
                if (int.Parse(_Factura[1].ToString()) > 1900)
                {
                    long _IdFactura = _Util.ObtenerIdFactura(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()));
                    if (_IdFactura > 0)
                    {
                        oStream = _Util.GeneraFact(_IdFactura);
                    }
                    else
                    {
                        _IdFactura = _Util.ImportarFacturaContabilidad(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()));
                        if (_IdFactura > 0) { oStream = _Util.GeneraFact(_IdFactura); }
                        else return Content("<center><h1>La factura " + Factura + " no existe en el SIM ni en el SICOF</h1></center>");
                    }
                }
                else return Content("<center><h1>El año para la factura " + Factura + " esta mal establecido</h1></center>");
            }
            else return Content("<center><h1>El formato para la factura es 999999-AAAA que corresponde a 999999 numero de factura y AAAA año para la factura </h1></center>"); ; ;
            if (oStream.Length > 0) oStream.Position = 0;
            var Archivo = oStream.GetBuffer();
            return File(Archivo, "application/pdf");
        }
        /// <summary>
        /// Asocia un factura especifica con un codigo de tramite y documento de informes Tecnicos
        /// </summary>
        /// <param name="Factura"></param>
        /// <param name="CodTramite"></param>
        /// <param name="CodDoc"></param>
        /// <returns></returns>
        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("AsociaFacturaInf")]
        public ActionResult AsociaFacturaInforme(string Factura, string CodTramite, string CodDoc)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            string _Respuesta = "";
            if (string.IsNullOrEmpty(Factura)) _Respuesta = "No ingresó  un número de factura";
            try
            {
                string[] _Factura = Factura.Split('-');
                if (_Factura.Length == 2)
                {
                    long _IdFactura = _Util.ObtenerIdFactura(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()));
                    if (_IdFactura > 0)
                    {
                        _Respuesta = _Util.AsociaInformeFacturaInf(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()), long.Parse(CodTramite), long.Parse(CodDoc));
                    }
                    else _Respuesta = "La factura " + Factura + " no se encontró en el SIM";
                }
                else _Respuesta = "No ingresó  un número de factura válido";
            }
            catch (Exception ex)
            {
                _Respuesta = "Ocrrio el error " + ex.Message;
            }
            return Content(_Respuesta);
        }
        /// <summary>
        /// Asocia un factura especifica con un codigo de tramite y documento de Resoluciones
        /// </summary>
        /// <param name="Factura"></param>
        /// <param name="CodTramite"></param>
        /// <param name="CodDoc"></param>
        /// <returns></returns>
        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("AsociaFacturaRes")]
        public ActionResult AsociaFacturaResolucion(string Factura, string CodTramite, string CodDoc)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            string _Respuesta = "";
            if (string.IsNullOrEmpty(Factura)) _Respuesta = "No ingresó  un número de factura";
            try
            {
                string[] _Factura = Factura.Split('-');
                if (_Factura.Length == 2)
                {
                    long _IdFactura = _Util.ObtenerIdFactura(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()));
                    if (_IdFactura > 0)
                    {
                        _Respuesta = _Util.AsociaInformeFacturaRes(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()), long.Parse(CodTramite), long.Parse(CodDoc));
                    }
                    else _Respuesta = "La factura " + Factura + " no se encontró en el SIM";
                }
                else _Respuesta = "No ingresó  un número de factura válido";
            }
            catch (Exception ex)
            {
                _Respuesta = "Ocrrio el error " + ex.Message;
            }
            return Content(_Respuesta);
        }

        /// <summary>
        /// Determina si una factura exite en la base ded atos local del SIM
        /// </summary>
        /// <param name="Factura">Cadena con el numero de la factura y el año separada por un guin</param>
        /// <returns></returns>
        [Authorize(Roles = "VFACTURACION")]
        [HttpGet, ActionName("ExisteFactura")]
        public bool ExisteFactura(string Factura)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            bool _Respuesta = false;
            if (string.IsNullOrEmpty(Factura)) _Respuesta = false;
            try
            {
                string[] _Factura = Factura.Split('-');
                if (_Factura.Length == 2)
                {
                    long _IdFactura = _Util.ObtenerIdFactura(_Factura[0].ToString(), int.Parse(_Factura[1].ToString()));
                    if (_IdFactura > 0) _Respuesta = true;
                }
            }catch
            {
                _Respuesta = false;
            }
            return _Respuesta;
        }
        /// <summary>
        /// Guarda el valor de la factura asociada a una informe tecnico
        /// </summary>
        /// <param name="Codtramite"></param>
        /// <param name="CodDocumento"></param>
        /// <param name="Valor"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardaValorInf")]
        public ActionResult GuardaValorInf(long Codtramite, long CodDocumento, long Valor)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            string _Respuesta = "";
            if (Valor <= 0) _Respuesta = "No ingresó  un valor para la factura";
            try
            {
                _Respuesta = _Util.GuardaValorFacturaInf(Codtramite, CodDocumento, Valor);
            }
            catch (Exception ex)
            {
                _Respuesta = "Ocrrio el error " + ex.Message;
            }
            return Content(_Respuesta);
        }
        /// <summary>
        /// Obtiene el numero del CM a partir de su codigo de tramite y documento para el informe tecnico
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <param name="CodDocumento"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerCMInforme")]
        public ActionResult ObtenerCMInforme(long CodTramite, long CodDocumento)
        {
            SIM.Utilidades.Facturacion _Util = new Utilidades.Facturacion();
            string _Respuesta = "";
            try
            {
                _Respuesta = _Util.ObtieneCMInforme(CodTramite, CodDocumento);
            }
            catch (Exception ex)
            {
                _Respuesta = "Ocrrio el error " + ex.Message;
            }
            return Content(_Respuesta);
        }
    }
}