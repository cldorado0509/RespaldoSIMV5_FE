using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;
using System.Security.Claims;
using SIM.Data;
using SIM.Models;

namespace SIM.Areas.Sau.Controllers
{

    public class SauController : Controller
    {
        //
        // GET: /Sau/Sau/
        EntitiesControlOracle db = new EntitiesControlOracle();
        EntitiesFloraOracle dbFlora = new EntitiesFloraOracle();
        //SIM.Areas.Tala.Models.EntitiesTala dbf = new SIM.Areas.Tala.Models.EntitiesTala();

        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        string Responsable;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult sau()
        {
            return View();
        }
        public ActionResult cargarExcel()
        {
            return View();
        }
        public ActionResult AsociarArbol()
        {
            return View();
        }
        public ActionResult NuevoArbol()
        {
            return View();
        }
     
        public ActionResult ConsultarArbol(int id_tramite, int id_contrato)
        {


            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_ARBOLES_CARGA(id_tramite, id_contrato, jSONOUT);
            return Json(jSONOUT.Value);


        }
        public ActionResult consultarpoCobama(int id_tramite, String cobama)
        {
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            String sql = " SELECT -1 id_ind_temp, i.id_individuo_disperso id_individuo,i.id_intervencion id_intprop, a.id_especie, e.s_especie, c.n_diametro_normal dap, c.n_altura_total altura, i.id_tipo_intervencion tipo_intervencion, di.s_intervencion, i.s_observaciones obs, c.id_estado_arbol id_estado,de.s_estado_arbol, g.shape.sdo_point.x x, g.shape.sdo_point.y y, " + id_tramite + " id_tramite, 0 ID_ESTADO_CARGA FROM siarburb.int_intervencion i inner join siarburb.INV_INDIVIDUO_DISPERSO a on a.id_individuo_disperso = i.id_individuo_disperso left join  siarburb.flr_especie e on e.id_especie= a.id_especie left join siarburb.int_caracteristicas c on c.id_intervencion= i.id_intervencion left join siarburb.flr_intervencion di on di.id_intervencion  = i.id_tipo_intervencion left join siarburb.flr_estado_arbol de on  de.id_estado_arbol = c.id_estado_arbol left join  sig_amva.p_inv_individuo_disperso g on g.ID_INDIVIDUO_DISP  = a.id_individuo_disperso where  a.COD_INDIVIDUO_DISPERSO   = '" + cobama + "'";
            db.SP_GET_DATOS(sql,jSONOUT);
            return Json(jSONOUT.Value);

        }
        public ActionResult guardarArbol(int fin)
        {


            string jsonOut = Request.Params["jsonInf"];
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));

            db.SP_SET_ARBOLES_CARGA(jsonOut, fin, rta);
            return Json(rta.Value);


        }
           public ActionResult guardarArbolContrato(int fin)
        {


            string jsonOut = Request.Params["jsonInf"];
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));

            //dbf.SP_SET_ARBOLES_CARGA_CONTRATO(jsonOut, fin, rta);
            dbFlora.SP_SET_ARBOLES_CARGA_CONTRATO(jsonOut, fin, rta);
            return Json(rta.Value);


        }
        
        public ActionResult GuardarTercepaso(int idTramite)
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
                Responsable = codFuncionario.ToString();
            }
            ObjectParameter rta = new ObjectParameter("rTA", typeof(string));
            db.SP_END_ARBOLES_CARGA(idTramite, codFuncionario, rta);
            return Json(rta.Value);
        }
        public ActionResult GuardarAsociarArbol(int idTemp, int idArbol)
        {

            db.SP_DESASOCIAR_ARBOL(idTemp, idArbol);
            return Content("1");
        }
            public ActionResult validarGuardado(int idTramite)
        {
            String sql = "select distinct i.TIPO_GUARDADO from flora.INTERVENCION_TRAMITE_TEMP i where i.ID_TRAMITE=" + idTramite;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
        
        
        public ActionResult consultarTramitecarga()
        {
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario));
            }

            
            String sql = "select distinct tt.CODFUNCIONARIO,tt.CODTRAMITE, tt.FECHAINICIOTRAMITE, tt.DIRECCION, tt.MUNICIPIO, tt.ASUNTO, tt.CODTAREA, tt.ORDEN,tt.X,tt.Y from VW_TRAMITE_A_VISITAR tt where tt.CODFUNCIONARIO=" + codFuncionario;
            ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
            db.SP_GET_DATOS(sql, jSONOUT);
            return Json(jSONOUT.Value);
        }
    }
}