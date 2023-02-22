using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Core.Objects;
using SIM.Data;
using System.Security.Claims;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Utilidades;
using Newtonsoft.Json;
using SIM.Areas.Tramites.Models;
using SIM.Models;

namespace SIM.Areas.Riesgos.Controllers
{
    public class RiesgosController : Controller
    {
        EntitiesControlOracle dbcontrol = new EntitiesControlOracle();
        //SIM.Areas.Riesgos.Models.EntitiesRiesgoQuimico dbRiesgo = new EntitiesRiesgoQuimico();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        public ActionResult Index()
        {
            Int32 idItem = Convert.ToInt32(Request.Params["idItem"]);
            Int32 idForm = Convert.ToInt32(Request.Params["id_formulario"]);
            Int32 idEstadoBase = Convert.ToInt32(Request.Params["idEstadoBase"]);
            String urlMapa = Request.Params["UrlMapa"];
            String tblFotos = Request.Params["tblFotos"];
            Int32 idVisita = Convert.ToInt32(Request.Params["idVisita"]);
            string tipoV = Request.Params["TipoV"];
            Int32 instalacion = Convert.ToInt32(Request.Params["instalacion"]);
            Int32 tercero = Convert.ToInt32(Request.Params["tercero"]);
            String tblEstados = Request.Params["tblEstados"];
            string textoEmpresa = (Request.Params["infoEmpresa"] ?? "").Replace("#", " ");


            ViewBag.txtidItem = idItem;
            ViewBag.txtidEstadoBase = idEstadoBase;
            ViewBag.txturlMapa = urlMapa;
            ViewBag.txttblFotos = tblFotos;
            ViewBag.textoEmpresa = textoEmpresa;
            ViewBag.txtForm = idForm;
            ViewBag.txtidVisita = idVisita;
            ViewBag.txttipoV = tipoV;
            ViewBag.txtinstalacion = instalacion;
            ViewBag.txttercero = tercero;
            ViewBag.txttblEstados = tblEstados;

            return View();
        }

        public ActionResult consultarJsonCaracteristicas()
        {
            Decimal idEstado = Decimal.Parse(Request.Params["idEstadoBase"]);
            Decimal tipo = Decimal.Parse(Request.Params["Tipo"]);
            Decimal padre = Decimal.Parse(Request.Params["padre"]);
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            //dbcontrol.SP_GET_CARACTERISTICASRIESGOS(idEstado, tipo, padre, jSONOUT);  /**** ESTE SP NO EXISTE EN EL ESQUEMA CONTROL *****/
            //return Json(jSONOUT.Value);
            return null;
        }
        public ActionResult CrearEstadoItem()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal form = Decimal.Parse(Request.Params["idFormulario"]);
            Decimal idVisita = Decimal.Parse(Request.Params["idVisita"]);
            Decimal instalacion = Decimal.Parse(Request.Params["instalacion"]);
            Decimal tercero = Decimal.Parse(Request.Params["tercero"]);
            ObjectParameter rESPIDESTADO = new ObjectParameter("rESPIDESTADO", typeof(string));
            dbcontrol.SP_CREATE_ESTADO_ITEM(tercero, instalacion, form, idItem, 1, idVisita, rESPIDESTADO);
            return Json(rESPIDESTADO.Value);
        }
        public ActionResult GuardarInformacionItem()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                string jsonInfo = Request.Params["jsonInfo"];
                Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbcontrol, idUsuario);
                ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
                Decimal form = Decimal.Parse(Request.Params["idFormulario"]);
                dbcontrol.SP_SET_ITEM(form, jsonInfo, decUsuario, "MANUAL", rTA);

                return Content(rTA.Value + "");
            }

            else
                return Content("0");

        }

        public ActionResult consultarJsonEncuestas()
        {
            Decimal idEstado = Decimal.Parse(Request.Params["idEstado"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_ENCUESTAS(idEstado, idform, jSONOUT);
            return Json(jSONOUT.Value);
        }

        public ActionResult consultarJsonDetalle()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, 0, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), 0, copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }
    }
}