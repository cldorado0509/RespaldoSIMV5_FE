using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.AtencionUsuarios.Models;
using SIM.Areas.GestionDocumental.Controllers;
using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SIM.Areas.AtencionUsuarios.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ValorTramiteApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        /// <summary>
        /// Obtiene los procesos para cargar el grid de la ventana principal
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtienSoportes")]
        public datosConsulta GetObtienSoportes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Sop in dbSIM.TBTARIFAS_CALCULO
                             where Sop.TIPO == "E"
                             orderby Sop.FECHA
                             select new
                             {
                                 Sop.ID_CALCULO,
                                 TIPO = dbSIM.TBTARIFAS_TRAMITE.Where(w => w.CODIGO_TRAMITE == Sop.CODIGO_TRAMITE && w.TIPO_ACTUACION == "E").Select(s => s.NOMBRE.Trim()).FirstOrDefault(),
                                 Sop.NIT,
                                 TERCERO = Sop.S_TERCERO,
                                 Sop.FECHA,
                                 CONSECUTIVO = Sop.N_CONSECUTIVO
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCalculo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ParametrosCalculo")]
        public JObject GetParametrosCalculo(decimal IdCalculo)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ParametrosCalculo parametrosCalculo = new ParametrosCalculo();
                var Calculo = (from Cal in dbSIM.TBTARIFAS_CALCULO
                               where Cal.ID_CALCULO == IdCalculo && Cal.TIPO == "E"
                               select Cal).First();
                if (Calculo != null)
                {
                    parametrosCalculo.IdCalculo = Calculo.ID_CALCULO;
                    parametrosCalculo.Sueldos = Calculo.COSTOS_A;
                    parametrosCalculo.Viajes = Calculo.COSTOS_B;
                    parametrosCalculo.Otros = Calculo.COSTOS_C.Value;
                    parametrosCalculo.Admin = Calculo.COSTOS_D;
                    parametrosCalculo.Costo = parametrosCalculo.Sueldos + parametrosCalculo.Sueldos + parametrosCalculo.Viajes + parametrosCalculo.Otros + parametrosCalculo.Admin;
                    parametrosCalculo.Topes = Calculo.TOPES;
                    parametrosCalculo.Valor = Calculo.VALOR;   
                    parametrosCalculo.Publicacion = Calculo.PUBLICACION.Value;
                }
                return JObject.FromObject(parametrosCalculo, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

    }
}
