using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Core.Objects;
using SIM.Data;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Configuration;
using SIM.Utilidades;
using SIM.Areas.Tramites.Models;
using Newtonsoft.Json;
using SIM.Models;

namespace SIM.Areas.Formularios.Controllers
{
    public class FormulariosController : Controller
    {
        EntitiesControlOracle dbcontrol = new EntitiesControlOracle();
        //Aguas.Models.ModeloAguas db = new Models.ModeloAguas();
        //
        // GET: /Aguas/AguasSuperficiales/
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
            string nombref = Request.Params["nombref"];

            ViewBag.txtNombref = nombref;
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


        public ActionResult consultarJsonInfoGeneral()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal form = Decimal.Parse(Request.Params["idFormulario"]);
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_ITEM(form, idItem, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult consultarEstado()
        {
            String sql = Request.Params["Estado"];

            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        public ActionResult CrearEstadoItemNuevo()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal form = Decimal.Parse(Request.Params["idFormulario"]);
            Decimal idVisita = Decimal.Parse(Request.Params["idVisita"]);
            Decimal instalacion = Decimal.Parse(Request.Params["instalacion"]);
            Decimal tercero = Decimal.Parse(Request.Params["tercero"]);
            ObjectParameter rESPIDESTADO = new ObjectParameter("rESPIDESTADO", typeof(string));
            dbcontrol.SP_CREATE_ESTADO_ITEM(tercero, instalacion, form, idItem, instalacion, idVisita, rESPIDESTADO);
            return Json(rESPIDESTADO.Value);
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
        public ActionResult GuardarInformacionCaracteristicas()
        {
            string jsonInfo = Request.Params["jsonInfo"];
            string strEstado = Request.Params["tblEstados"];
            Decimal idEstado = Decimal.Parse(Request.Params["idEstado"]);
            ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
            dbcontrol.SP_SET_CARACTERISTICAS(idEstado, strEstado, jsonInfo, rTA);

            return Content(rTA.Value + "");

        }
        public ActionResult ConsultarFotografiasForm(int idForm, String tabla, int idEstado)
        {
            AppSettingsReader webConfigReader = new AppSettingsReader();
            string url = (string)webConfigReader.GetValue("url_foto_formulario", typeof(string));
            var str_sql = "select f.S_ETIQUETA,f.id_fotografia,'" + url + "'||f2.ID_FORMULARIO||'/'||f.s_archivo url,f.S_ARCHIVO from CONTROL.FOTOGRAFIA f inner join " + tabla + " f2 on f.ID_FOTOGRAFIA=f2.ID_FOTOGRAFIA where f2.id_estado=" + idEstado;
            ObjectParameter jsonOut = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_DATOS(str_sql, jsonOut);
            return Json(jsonOut.Value);

        }

        public ActionResult eliminarFotoForm(int idfoto, String tabla)
        {
            int id_foto = idfoto;
            dbcontrol.SP_ELIMINAR_FOTO_FORM(id_foto, tabla);
            return Content("0");
        }
        public ActionResult GuardarInformacionEncuesta()
        {
            string jsonInfo = Request.Params["jsonInfo"];
            ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
            Decimal idCapEs = Convert.ToDecimal(Request.Params["idCapEstado"]);
            Decimal strEstado = Convert.ToDecimal(Request.Params["idform"]);
            dbcontrol.SP_SET_ENCUESTAS(idCapEs, strEstado, jsonInfo, rTA);
            return Content("" + rTA.Value);
        }
    }
}