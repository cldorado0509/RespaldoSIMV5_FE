using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Data.Entity.Core.Objects;
using SIM.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using SIM.Data.Control;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class RepartoController : Controller 
    {
        EntitiesSIMOracle db = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        
        public ActionResult Reparto()
        {
            return View();
        }

        public ActionResult Repartoviejo()
        {
            return View();
        
       }


        public ActionResult GridTerceros(int id, String nombreTarea)
        {
            ViewBag.CodTarea = id;
            ViewBag.nombre = nombreTarea;
            return View();
        
       }


        public ActionResult API()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GuardarTramitesGeocodificados(string p_codigo, String p_cx, String p_cy, string p_tipoUbicacion)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;


            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                Decimal decCodigo = Decimal.Parse(p_codigo);
                Decimal decCoorX = Decimal.Parse(p_cx.Replace(".", ci.NumberFormat.NumberDecimalSeparator).Replace(",", ci.NumberFormat.NumberDecimalSeparator));
                Decimal decCoorY = Decimal.Parse(p_cy.Replace(".", ci.NumberFormat.NumberDecimalSeparator).Replace(",", ci.NumberFormat.NumberDecimalSeparator));
                Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                dbControl.SP_NEW_P_TRAMITE2(decCodigo, decCoorX, decCoorY, decUsuario, p_tipoUbicacion);
                return Content("1");
            }
            else
                return Content("0");
            
        }

        /*[ValidateInput(false)]
        public ActionResult GuardarTramitesGeocodificados(string p_codigo, decimal p_cx, decimal p_cy, string p_tipoUbicacion)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                Decimal decCodigo = Decimal.Parse(p_codigo);
                Decimal decCoorX = p_cx; //Decimal.Parse(p_cx.Replace(".", ","));
                Decimal decCoorY = p_cy; // Decimal.Parse(p_cy.Replace(".", ","));
                Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
                db.SP_NEW_P_TRAMITE2(decCodigo, decCoorX, decCoorY, decUsuario, p_tipoUbicacion);
                return Content("1");
            }
            else
                return Content("0");

        }*/

        [ValidateInput(false)]
        public ActionResult GuardarAvanzaTareaTramite()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                Decimal decCodigo_Tramite = Decimal.Parse(Request.Params["p_cod_tramite"]);
                Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                Decimal decCodigo_Tarea = Decimal.Parse(Request.Params["p_cod_tarea"]);
                String strComentario = Request.Params["p_comentario"];
                Decimal decCodigo_Orden = Decimal.Parse(Request.Params["p_cod_orden"]);
                Decimal decCodigo_Responsable = Decimal.Parse(Request.Params["p_cod_resp"]);
                String strCopia = Request.Params["p_cod_copias"];
                Decimal tareaSig = Decimal.Parse(Request.Params["codSig"]);
                
                if (strCopia.Equals(""))
                    strCopia = "0";
                //db.SP_AVANZA_TAREA_TRAMITE(decCodigo_Tramite, decUsuario, decCodigo_Tarea, strComentario, decCodigo_Orden, decCodigo_Responsable, strCopia, tareaSig);

                ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));
                dbTramites.SP_AVANZA_TAREA(1, decCodigo_Tramite, decCodigo_Tarea, tareaSig, decCodigo_Responsable, strCopia, strComentario, rtaResultado);

                //return Content("1");
                return Content(rtaResultado.Value.ToString() == "OK" ? "1" : "0");
            }
            else
                return Content("0");
        }
        
        [ValidateInput(false)]
        public ActionResult Obtener_Tramites_Ubicados()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier)!=null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                decimal codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);                
                System.Collections.Generic.List<VW_REPARTO_GEO> Z = db.VW_REPARTO_GEO.Where(p => p.CODFUNCIONARIO == codFuncionario).ToList();
                return PartialView("_GrdTramitesUbicados", Z);
            }
            else
                return PartialView("_GrdTramitesUbicados");
        }

        [ValidateInput(false)]
        public ActionResult Obtener_Tramites_No_Ubicados()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                decimal codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                System.Collections.Generic.List<VW_REPARTO_NOGEO> Z = db.VW_REPARTO_NOGEO.Where(p => p.CODFUNCIONARIO == codFuncionario).ToList();
                return PartialView("_GrdTramitesNoUbicados", Z);
            }
            else
                return PartialView("_GrdTramitesNoUbicados");
        }

        [ValidateInput(false)]
        public ActionResult Obtener_Responsable(bool Cargar = false)
        {
            System.Collections.Generic.List<VW_FUNCIONARIO> t = db.VW_FUNCIONARIO.ToList();
            return PartialView("_GrdTecnico", t);
        }

        [ValidateInput(false)]
        public ActionResult Obtener_Con_Copia()
        {
            System.Collections.Generic.List<VW_FUNCIONARIO> t = db.VW_FUNCIONARIO.ToList();
            return PartialView("_GrdEncargados", t);
        }

        [ValidateInput(false)]
        public ActionResult Mostrar_Popup_Asignacion_Tramite()
        {
            return View("PopupAsignacionTramite");
        }

        public ActionResult validarSesion()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
          
                return Content("1");
            }
            else
                return Content("0");

        }
        public ActionResult consultarUser()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                Decimal codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                return Content(codFuncionario + "");
            }
            return Content("0");

        }

        public ActionResult consultarTareaSiguiente(String codigotarea)
        {
            //String sql = "select dr.codtareasiguiente from tramites.DETALLE_REGLA dr where dr.s_pordefecto=1 and dr.codtarea='" + codigotarea + "'";
            String sql = "SELECT t.CODTAREA,t.nombre from tramites.TBTAREA t  where t.codtarea in(select  dr.codtareasiguiente from tramites.DETALLE_REGLA dr inner join tramites.TBTAREA t on t.CODTAREA=dr.CODTAREA where dr.codtarea='" + codigotarea + "')";
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult geocodificadorPlanas(String valor,String tramite)
        {
            /*ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            coordenadas coorP = new coordenadas();
            coordenadas coorP2 = new coordenadas();
            var client = new System.Net.WebClient();
            var dato = client.DownloadString("https://www.medellin.gov.co/MapGIS/Geocod/AjaxGeocod?accion=2&valor="+valor+"&f=json");
            coorP = JsonConvert.DeserializeObject<coordenadas>(dato);
            String xp = coorP.x;
            String yp = coorP.y;
            var dato2 = client.DownloadString("https://www.medellin.gov.co/mapas/rest/services/Utilities/Geometry/GeometryServer/project?inSR=6257&outSR=4326&geometries=" + xp + "," + yp + "&transformation=15738&transformForward=true&f=json");
            JObject o = JObject.Parse(dato2);
            Decimal x = (Decimal)o.SelectToken("geometries[0].x");
            Decimal y = (Decimal)o.SelectToken("geometries[0].y");*/

            coordenadas resul = Utilidades.Geocoding.ObtenerCoordenadas(valor);

            Decimal x = Convert.ToDecimal(resul.x);
            Decimal y = Convert.ToDecimal(resul.y);

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                Decimal decCodigo = Decimal.Parse(tramite);
             
                Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
                dbControl.SP_NEW_P_TRAMITE2(decCodigo, x, y, decUsuario, "Geocodificador");
                return Content("1");
            }
            else
                return Content("0");
        }
   
    }
}