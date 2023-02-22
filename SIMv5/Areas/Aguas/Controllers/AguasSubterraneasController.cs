using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SIM.Areas.Aguas.Models;
using System.Data.Entity.Core.Objects;
using SIM.Data;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using System.Configuration;
using SIM.Areas.Tramites.Models;
using Newtonsoft.Json;
using SIM.Utilidades;
using SIM.Models;

namespace SIM.Areas.Aguas.Controllers
{
    [Authorize]
    public class AguasSubterraneasController : Controller
  {
      
      EntitiesControlOracle dbcontrol = new EntitiesControlOracle();
 
      // GET: /Aguas/AguasSubterraneasController/
      System.Web.HttpContext context = System.Web.HttpContext.Current;
      Int32 idUsuario;
      public ActionResult Index()
      {
          Int32 idCaptacion = Convert.ToInt32(Request.Params["idCaptacion"]);
          Int32 IdTipoEstdo = Convert.ToInt32(Request.Params["idTipoEstado"]);
          Int32 idVisita = Convert.ToInt32(Request.Params["idVisita"]);
          Int32 idEstadoBase = Convert.ToInt32(Request.Params["idEstadoBase"]);
          Int32 idForm = Convert.ToInt32(Request.Params["id_formulario"]);
          string textoEmpresa = (Request.Params["infoEmpresa"] ?? "").Replace("#", " ");
            string tipoV = Request.Params["TipoV"];
          string nombref = Request.Params["nombref"];
          String urlMapa = Request.Params["UrlMapa"];
          String tblFotos = Request.Params["tblFotos"];
          Int32 instalacion = Convert.ToInt32(Request.Params["instalacion"]);
          Int32 tercero = Convert.ToInt32(Request.Params["tercero"]);
          String tblEstados = Request.Params["tblEstados"];




          ViewBag.txturlMapa = urlMapa;
          ViewBag.txttblFotos = tblFotos;
          ViewBag.txtinstalacion = instalacion;
          ViewBag.txttercero = tercero;
          ViewBag.txttblEstados = tblEstados;
          ViewBag.txtCaptacion = idCaptacion;
          ViewBag.textoEmpresa = textoEmpresa;
          ViewBag.txtTipoEstado = IdTipoEstdo;
          ViewBag.txtVisita = idVisita;
          ViewBag.txtEstadoBase = idEstadoBase;
          ViewBag.txtForm = idForm;
          ViewBag.txttipoV = tipoV;
          ViewBag.txtNombref = nombref;

          return View();
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
      public ActionResult ConsultarInformacionCaptacion()
      {

          Decimal idCaptacion = Decimal.Parse(Request.Params["idCaptacion"]);
          Decimal CoorX = Decimal.Parse(Request.Params["X"].Replace(".", ","));
          Decimal CoorY = Decimal.Parse(Request.Params["Y"].Replace(".", ","));
          ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
          dbcontrol.SP_GET_CAPTACION(idCaptacion, CoorX, CoorY, jSONOUT);
          return Json(jSONOUT.Value);


      }
      public ActionResult GuardarInformacionCaptacion()
      {
          if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
          {
              idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
              string jsonInfo = Request.Params["jsonInfo"];
              Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbcontrol, idUsuario);
              ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
              Decimal tipoestado = Convert.ToDecimal(Request.Params["tipoestado"]);
              Decimal idCap = Convert.ToDecimal(Request.Params["idcaptacion"]);
              Decimal idVisita = Convert.ToDecimal(Request.Params["idvisita"]);
              dbcontrol.SP_SET_CAPTACION(jsonInfo, decUsuario, "MANUAL", rTA);

              return Content(rTA.Value + "");
          }

          else
              return Content("0");
      }
      public ActionResult GuardarInformacionCaptacionAndEstado()
      {
          if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
          {
              idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
              string jsonInfo = Request.Params["jsonInfo"];
              Decimal decUsuario = clsGenerales.Obtener_Codigo_Funcionario(dbcontrol, idUsuario);
              ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
              Decimal tipoestado = Convert.ToDecimal(Request.Params["tipoestado"]);
              Decimal idCap = Convert.ToDecimal(Request.Params["idcaptacion"]);
              Decimal idVisita = Convert.ToDecimal(Request.Params["idvisita"]);
              Decimal form = Decimal.Parse(Request.Params["idFormulario"]);
              Decimal instalacion = Decimal.Parse(Request.Params["instalacion"]);
              Decimal tercero = Decimal.Parse(Request.Params["tercero"]);
              dbcontrol.SP_SET_CAPTACION(jsonInfo, decUsuario, "MANUAL", rTA);
              if (rTA.Value.Equals("Ok"))
              {
                  ObjectParameter rTA2 = new ObjectParameter("rESPIDESTADO", typeof(string));
                  dbcontrol.SP_CREATE_ESTADO(form, tercero, instalacion, idCap, tipoestado, idVisita, rTA2);
                  return Content("" + rTA2.Value);
              }
              return Content("error");
          }

          else
              return Content("0");
      }
      public ActionResult ConsultarInformacionUsos()
      {

          Decimal idCapEstado = Decimal.Parse(Request.Params["idCapEstado"]);
          ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
          dbcontrol.SP_GET_USOS(idCapEstado, jSONOUT);
          return Json(jSONOUT.Value);


      }
      public ActionResult GuardarInformacionUsos()
      {
          string jsonInfo = Request.Params["jsonInfo"];
          ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
          Decimal idCapEs = Convert.ToDecimal(Request.Params["idCapEstado"]);
          dbcontrol.SP_SET_USOS(idCapEs, jsonInfo, rTA);
          return Content("" + rTA.Value);
      }
        public ActionResult consultarJsonDetalle()
        {
            Decimal idCapEstado = Decimal.Parse(Request.Params["idCapEstado"]);
            Decimal idform = Decimal.Parse(Request.Params["form"]);
            string strEstado = Request.Params["tblEstados"];
            bool copia = ((Request.Params["copia"] ?? "0") == "1");

            /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbcontrol.SP_GET_CARACTERISTICAS(idCapEstado, strEstado, idform, 0, 0, jSONOUT);
            var resultado = jSONOUT.Value.ToString();*/

            var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(Convert.ToInt32(idCapEstado), Convert.ToInt32(idform), 0, copia);
            var resultado = datosFormulario.json;

            List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado);
            Estructuras.EliminarNodosSinHojas(caracteristicas);
            resultado = JsonConvert.SerializeObject(caracteristicas);

            return Json(resultado);
        }

      public ActionResult consultarJsonEncuestas()
      {
          Decimal idCapEstado = Decimal.Parse(Request.Params["idCapEstado"]);
          Decimal idform = Decimal.Parse(Request.Params["form"]);
          ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
          dbcontrol.SP_GET_ENCUESTAS(idCapEstado, idform, jSONOUT);
          return Json(jSONOUT.Value);
      }
      public ActionResult GuardarInformacionDetalle()
      {
          string jsonInfo = Request.Params["jsonInfo"];
          ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
          Decimal idCapEs = Convert.ToDecimal(Request.Params["idCapEstado"]);
          string strEstado = Request.Params["tblEstados"];
          dbcontrol.SP_SET_CARACTERISTICAS(idCapEs, strEstado, jsonInfo, rTA);
          return Content("" + rTA.Value);
      }
      public ActionResult GuardarInformacionEncuesta()
      {
          string jsonInfo = Request.Params["jsonInfo"];
          ObjectParameter rTA = new ObjectParameter("rTA", typeof(string));
          Decimal idCapEs = Convert.ToDecimal(Request.Params["idCapEstado"]);
          Decimal form = Decimal.Parse(Request.Params["form"]); ;
          dbcontrol.SP_SET_ENCUESTAS(idCapEs, form, jsonInfo, rTA);
          return Content("" + rTA.Value);
      }


  }

}