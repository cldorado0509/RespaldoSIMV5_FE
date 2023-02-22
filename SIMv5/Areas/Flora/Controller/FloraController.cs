using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
//using SIM.Areas.Flora.Models;
//using System.Data.Objects;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.Flora.Controllers   
{
    public class FloraController : Controller
    {
        EntitiesFloraOracle db = new EntitiesFloraOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        //SIM.Areas.Flora.Models.Entities dbFlora = new Models.Entities();

        //
        // GET: /Flora/Flora/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Flora()
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

        public ActionResult CargarImagenFlora()
        {
            return View();
        }

        public ActionResult EliminarFotoFlora()
        {
            return View();
        }

        public ActionResult ConsultarEspecies()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_ESPECIES(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarEstados()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_ESTADOS(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarEntorno()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_ENTORNO(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarRiesgos()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_RIESGO(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarAgente()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_AGENTE_DM(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarSintomaDM()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_SINTOMA_DM(jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult ConsultarSintomaEF()
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            dbControl.SP_GET_SINTOMA_EF(jSONOUT);
            return Json(jSONOUT.Value);


        }
      
        public ActionResult ConsultarArboles()
        {
            Decimal id_visita = Decimal.Parse(Request.Params["idVisita"]);
             string lstInd = Request.Params["lstInd"];
             if (lstInd.Equals(""))
             {
                 lstInd = "-1";
             }
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_ARBOLES(id_visita,lstInd,jSONOUT);
            return Json(jSONOUT.Value);


        }
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        Int32 idUsuario;
        decimal codFuncionario;

        public ActionResult GuardarGeneral()
        {
            Decimal id_visita = Decimal.Parse(Request.Params["idVisita"]);

            string jsonOut = Request.Params["json"];
             /*if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
             {
                 idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                  codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);

             }
             Decimal user = idUsuario;*/
            ObjectParameter rta = new ObjectParameter("rta", typeof(string));
            //dbFlora.SP_SET_ARBOLES(id_visita, jsonOut, rta);
            db.SP_SET_ARBOLES(id_visita, jsonOut, rta);
            return Json(rta.Value);


        }

        
       

	}
}