using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using SIM.Areas.ControlVigilancia.Models;
using System.Data.Entity.Core.Objects;
using System.Configuration;
using SIM.Utilidades;
using SIM.Areas.Tramites.Models;
using Newtonsoft.Json;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.ControlVigilancia.Controllers
{
    public class ResiduosController : Controller
    {
        EntitiesControlOracle dbcontrol = new EntitiesControlOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult Residuos()
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
            ViewBag.txtNombref = nombref;
            return View();
        }

        public ActionResult CargarImagenResiduo()
        {
            return View();
        }

        public ActionResult EliminarFotoResiduo()
        {
            return View();
        }

        public ActionResult CargarDocumentoAdjunto()
        {
            return View();
        }

        public ActionResult EliminarDocumentoAdjunto()
        {
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

        public ActionResult consultarJsonDetallegrupo()
        {

            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleGeneracion()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }


        public ActionResult consultarJsonDetalleSeparacion()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleAlmacenamiento()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleAprovechamiento()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleTratamiento()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleDisposicionFinal()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleSoporte()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleFormularioRH1()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleCertificados()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
            var resultado = datosFormulario.json;

            if (resultado != null)
            {
                List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
                Estructuras.EliminarNodosSinHojas(caracteristicas);
                resultado = JsonConvert.SerializeObject(caracteristicas);
            }

            return Json(resultado);
        }

        public ActionResult consultarJsonDetalleFormularioRespelRua()
        {
            Decimal idItem = Decimal.Parse(Request.Params["idItem"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            Decimal idgrupo = Decimal.Parse(Request.Params["idgrupo"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idItem, strEstado, idform, idgrupo, 0, jSONOUT);
            return Json(jSONOUT.Value);*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idItem), Convert.ToInt32(idform), Convert.ToInt32(idgrupo), copia);
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

        public ActionResult consultarJsonEncuestas()
        {
            Decimal idEstado = Decimal.Parse(Request.Params["idEstado"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_ENCUESTAS(idEstado, idform, jSONOUT);
            return Json(jSONOUT.Value);
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
	}
}