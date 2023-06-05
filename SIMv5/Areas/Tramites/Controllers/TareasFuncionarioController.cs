using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Tramites.Models;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIM.Areas.Tramites.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TareasFuncionarioController : Controller
    {

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        // GET: Tramites/TareasFuncionario
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ResumenTareasFunc")]
        public JObject GetResumenTareasFuncionario(decimal CodFuncionario)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            var DatosTareas = new Tramites.Models.DatosTareasFuncionario();
            if (ModelState.IsValid)
            {
                try
                {

                    DatosTareas.NroTareasPendientes = (from tar in dbSIM.TBTRAMITETAREA
                                                       where (tar.COPIA == 0)
                                                       && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == CodFuncionario)
                                                       select tar).ToList().Count;
                    DatosTareas.NroTareasTerminadas = (from tar in dbSIM.TBTRAMITETAREA
                                                       where (tar.COPIA == 0)
                                                       && (tar.ESTADO == 1) && (tar.CODFUNCIONARIO == CodFuncionario)
                                                       select tar).ToList().Count;
                    DatosTareas.NroTareasNoAbiertas = (from tar in dbSIM.TBTRAMITETAREA
                                                       where (tar.RECIBIDA != 1) && (tar.COPIA == 0)
                                                       && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == CodFuncionario)
                                                       select tar).ToList().Count;
                    DatosTareas.NroTareasCopiaPendientes = (from tar in dbSIM.TBTRAMITETAREA
                                                            where (tar.COPIA == 1) && (tar.ESTADO == 0) &&
                                                            (tar.CODFUNCIONARIO == CodFuncionario)
                                                            select tar).ToList().Count;
                    DatosTareas.NroTareasCopiaTerminadas = (from tar in dbSIM.TBTRAMITETAREA
                                                            where (tar.COPIA == 1) && (tar.ESTADO == 1) &&
                                                            (tar.CODFUNCIONARIO == CodFuncionario)
                                                            select tar).ToList().Count;
                    DatosTareas.NroTareasCopiaNoAbiertas = (from tar in dbSIM.TBTRAMITETAREA
                                                            where (tar.RECIBIDA != 1) && (tar.COPIA == 1)
                                                            && (tar.ESTADO == 0) && (tar.CODFUNCIONARIO == CodFuncionario)
                                                            select tar).ToList().Count;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
            return JObject.FromObject(DatosTareas, Js);
        }
    }
}