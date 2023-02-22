using Newtonsoft.Json;
using SIM.Data;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SIM.Areas.Tramites.Controllers
{
    public class TramitesADAApiController : ApiController
    {

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        [HttpGet]
        [ActionName("GuardarInfoTyC")]
        public object SaveInfoTyC(int idTramite, int idInstalacion, int idTercero, string TYC)
        {
            try
            {
                TERMINOSCONDICIONES_TRAMITE item = new TERMINOSCONDICIONES_TRAMITE();

                item.ID_TRAMITE = idTramite;
                item.ID_INSTALACION = idInstalacion;
                item.ID_TERCERO = idTercero;
                item.TEXTO_TERMINOSCODICIONES = TYC;
                item.FECHA_ACEPTA_TYC = DateTime.Now;
                item.ACEPTA_TYC = 1;

                dbSIM.Entry(item).State = EntityState.Added;
                dbSIM.SaveChanges();

                return Json("1");

            }
            catch (Exception ex)
            {
                return Json("0");
            }

        }
        [HttpGet]
        [ActionName("Tramite")]
        public object GetInfoTramite(int id, int? idInstalacion, int? idTercero)
        {
            try
            {
                var queryParcial = dbSIM.TERMINOSCONDICIONES_TRAMITE.Where(t => t.ID_INSTALACION == idInstalacion && t.ID_TERCERO == idTercero && t.ID_TRAMITE == id).FirstOrDefault();

                if (queryParcial != null)
                {


                    var query = from t1 in dbSIM.TERMINOSCONDICIONES_TRAMITE
                                join t2 in dbSIM.QRY_LISTADOTRAMITES on t1.ID_TRAMITE equals t2.ID_TRAMITE
                                where t1.ID_INSTALACION == idInstalacion && t1.ID_TERCERO == idTercero && t1.ID_TRAMITE == id
                                select new { t1.ID_TRAMITE, t1.ID_TERCERO, t1.ID_INSTALACION, t1.TEXTO_TERMINOSCODICIONES, t1.FECHA_ACEPTA_TYC, t1.ACEPTA_TYC, t2.TRAMITE, t2.DESCRIPCION };

                    //return query.FirstOrDefault()+ "||Ok";
                    string json = JsonConvert.SerializeObject(query.ToList());
                    return Json(json + "||Ok");

                }

                else
                {
                    var query2 = from ls in dbSIM.QRY_LISTADOTRAMITES
                                 where ls.ID_TRAMITE == id
                                 select new { ls.TRAMITE, ls.DESCRIPCION };

                    //var model = dbSIM.QRY_LISTADOTRAMITES.Where(f => f.ID_TRAMITE == id).FirstOrDefault();

                    //return query2.FirstOrDefault() +"||No";
                    string json = JsonConvert.SerializeObject(query2.ToList());
                    return Json(json + "||No");
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                return Json("");
            }
               
        }



        //[HttpGet]
        //[ActionName("Requisitos")]
        //public object GetRequisitosXTramite(int id)
        //{
        ////    if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
        ////    {
        ////        idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
        ////        codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(db, idUsuario);
        ////    }
        ////    String sql = "SELECT INFORME_TECNICO.ID_INF,INFORME_TECNICO.ASUNTO,INFORME_TECNICO.OBSERVACION,INFORME_TECNICO.ID_ESTADOINF,INFORME_TECNICO.ID_VISITA,INFORME_TECNICO.FUNCIONARIO,INFORME_TECNICO.URL,INFESTADO.NOMBRE,INFORME_TECNICO.URL2 FROM control.INFORME_TECNICO INNER JOIN control.INFESTADO ON INFESTADO.ID_ESTADO = INFORME_TECNICO.ID_ESTADOINF where INFORME_TECNICO.ID_ESTADOINF=1 and INFORME_TECNICO.FUNCIONARIO=" + codFuncionario + "";
        ////    ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
        ////    dbControl.SP_GET_DATOS(sql, jSONOUT);
        ////    return Json(jSONOUT.Value);
        //    try
        //    {
        //        var model = dbSIM.QRY_REQUISITOS_TRAMITE.Where(f => f.ID_TRAMITE == id).FirstOrDefault();

        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex.Message.ToString();
        //        throw;
        //    }

        //}

        [HttpGet, ActionName("TipoTramite")]
        public datosConsulta GetTipoTramite()
        {
            var model = from tipoTramite in dbSIM.QRY_LISTADOTRAMITES
                        orderby tipoTramite.TRAMITE
                        select new
                        {
                            tipoTramite.ID_TRAMITE,
                            tipoTramite.TRAMITE
                        };

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }
    }
}

