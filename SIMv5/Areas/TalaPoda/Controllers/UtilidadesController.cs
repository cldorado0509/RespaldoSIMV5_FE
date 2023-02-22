using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Areas.TalaPoda.Models;
using SIM.Data;
using SIM.Data.Flora;

namespace SIM.Areas.TalaPoda.Controllers
{
    public class UtilidadesController : Controller
    {
        private EntitiesSIMOracle db = new EntitiesSIMOracle();

        /*public ActionResult MostrarIndividuosxVisita(int CodigoVisita)
        {
            //var Resultado = from trvs in db.TRAMITE_VISITA
            //                join intr in db.INV_INDIVIDUO_TRAMITE on trvs.ID_TRAMITE equals intr.ID_TRAMITE
            //                join ind in db.INV_INDIVIDUO_DISPERSO on intr.ID_INDIVIDUO_DISPERSO equals ind.ID_INDIVIDUO_DISPERSO
            //                where trvs.ID_VISITA == CodigoVisita
            //                select ind;

            var Resultado = from ind in db.INV_INDIVIDUO_DISPERSO
                            select ind;

            ViewBag.CodigoVisita = CodigoVisita;

            return View(Resultado.OrderBy(I => I.ID_INDIVIDUO_DISPERSO).Take(10).ToList());
        }*/

        public ActionResult CrearIndividuo(int CodigoVisita)
        {
            ViewBag.CodigoVisita = CodigoVisita;
            return View();
        }

        public ActionResult InfoIndividuo()
        {
            return View();
        }

        public ActionResult InfoEstados()
        {            
            ViewBag.Riesgos = db.FLR_RIESGO.OrderBy(F => F.S_RIESGO).ToList();
            ViewBag.RiesgosExtincion = db.FLR_RIESGO_EXTINCION.OrderBy(F => F.S_RIESGO_EXTINCION).ToList();
            ViewBag.SintomaEF = db.FLR_SINTOMA_EF.OrderBy(F => F.S_SINTOMA_EF).ToList();
            ViewBag.SintomaDM = db.FLR_SINTOMA_DM.OrderBy(F => F.S_SINTOMA_DM).ToList();

            return View();
        }

        public ActionResult MostrarTipoPerjuicio()
        {
            System.Collections.Generic.List<FLR_RIESGO> Riesgos = db.FLR_RIESGO.ToList();
            System.Collections.Generic.List<FLR_SINTOMA_EF> Sintomas = db.FLR_SINTOMA_EF.ToList();

            ViewBag.Riesgos = Riesgos.OrderBy(R => R.S_RIESGO).ToList();
            ViewBag.Sintomas = Sintomas.OrderBy(S => S.S_SINTOMA_EF).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GuardarTipoPerjuicio(string Tipo, string Chequeado)
        {
            return Content(Tipo);
        }

        public ActionResult MostrarIndividuosDispersos()
        {
            return View();
        }

        public ActionResult EditarIndividuoDisperso(int Codigo)
        {
            GridTalaPodaViewModel Resultado = new GridTalaPodaViewModel();
            Resultado.IdIndividuo = Codigo;
            return View(Resultado);
        }

			/*public ActionResult ReplicarIndividuoDisperso(int Codigo)
				{
					INFO_INDIVIDUO_DISPERSO tblIndividuo = db.INV_INDIVIDUO_DISPERSO.FirstOrDefault(k => k.ID_INDIVIDUO_DISPERSO == Codigo);
					INV_INDIVIDUO_DISPERSO tblReplica = new INV_INDIVIDUO_DISPERSO();
					tblReplica.D_ACTUALIZACION = tblReplica.D_INGRESO = DateTime.Now;
					tblReplica.COD_INDIVIDUO_DISPERSO = tblIndividuo.COD_INDIVIDUO_DISPERSO;
					db.INV_INDIVIDUO_DISPERSO.Add(tblReplica);
					//db.SaveChanges();
					
					return View();
				}*/

        /*public ActionResult ObtenerIndividuosDispersos()
        {
            List<GridTalaPodaViewModel> lsIndividuosDispersos = (from especie in db.FLR_ESPECIE 
                                                                 join individuo in db.INV_INDIVIDUO_DISPERSO
                                                                 on especie.ID_ESPECIE equals individuo.ID_ESPECIE
                                                                 join estado in db.INV_ESTADO_INDIVIDUO
                                                                 on individuo.ID_ESTADO equals estado.ID_ESTADO
                                                                 
                                                                 select new GridTalaPodaViewModel
                                                                 {
                                                                     Especie = especie.S_NOMBRE_COMUN,
                                                                     Estado = estado.S_ESTADO,
                                                                     IdIndividuo = individuo.ID_INDIVIDUO_DISPERSO,
																																		 Accion = individuo.ID_INDIVIDUO_DISPERSO,
																																		 Excluir = individuo.ID_INDIVIDUO_DISPERSO
                                                                 }).Take(20).ToList();

            return Json(lsIndividuosDispersos, JsonRequestBehavior.AllowGet);
        }*/

        private int ObtenerNroRegistro(int intContador)
        {
            return intContador++;
        }
    }
}