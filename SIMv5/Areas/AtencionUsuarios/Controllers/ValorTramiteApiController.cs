using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraReports.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.AtencionUsuarios.Models;
using SIM.Areas.GestionDocumental.Controllers;
using SIM.Data;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCalculo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ExisteSoporte")]
        public object GetExisteSoporte(decimal IdCalculo)
        {
            if (IdCalculo <= 0) return new { resp = "Error", mensaje = "No se ha ingresado un identificador de para ubicar el soporte" };
            try
            {
                var _Elaboracion = dbSIM.TBTARIFAS_CALCULO.Where(w => w.ID_CALCULO == IdCalculo).Select(s => s.FECHA).FirstOrDefault();
                if (_Elaboracion != null)
                {
                    string _RutaArchSopValorAuto = SIM.Utilidades.Data.ObtenerValorParametro("ArchivosValorAuto").ToString();
                    FileInfo _Soporte = new FileInfo(_RutaArchSopValorAuto + _Elaboracion.ToString("MMyyyy") + @"\" + IdCalculo.ToString() + ".pdf");
                    if (_Soporte.Exists) return new { resp = "Ok", mensaje = "" };
                    else return new { resp = "Error", mensaje = "No se encontro el documento del soporte generado" };
                }
            } catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Ocurrio el error : " + ex.Message };
            }
            return new { resp = "Error", mensaje = "No se encontro el documento del soporte generado" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTramite"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ParametrosEvaluacion")]
        public JObject ObtieneValoresEvaluacion(decimal IdTramite)
        {
            JObject resp = null;
            var Parametro = (from tar in dbSIM.TBTARIFAS_TRAMITE
                             where tar.CODIGO_TRAMITE == IdTramite && tar.TIPO_ACTUACION == "E"
                             select new ParametrosEvaluacion
                             {
                                 DuracionVisita = tar.VISITA.Value,
                                 HorasInforme = tar.INFORME.Value,
                                 NumeroVisitas = tar.N_VISITAS.Value,
                                 NumeroProfesionales = tar.TECNICOS.Value,
                                 Unidad = tar.S_UNIDAD == "N/A" ? "Items :" : tar.S_UNIDAD + " :"
                             }).FirstOrDefault();
            if (Parametro != null)
            {
                resp = (JObject)JToken.FromObject(Parametro);
            }
            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("TiposTramiteEvaluacion")]
        public JArray ObtieneTiposTramiteEvaluacion()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var TipoTramite = (from Ttr in dbSIM.TBTARIFAS_TRAMITE
                                   where Ttr.TIPO_ACTUACION == "E" && Ttr.N_VISIBLE == 1
                                   orderby Ttr.NOMBRE
                                   select new
                                   {
                                       Ttr.CODIGO_TRAMITE,
                                       Ttr.NOMBRE
                                   }).ToList();
                return JArray.FromObject(TipoTramite, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el nombre de un tercero a partir de su documento de indetificacion sin digito de verificacion
        /// </summary>
        /// <param name="Documento">Numero de documento sin digito de verificacion</param>
        /// <returns></returns>
        [HttpGet, ActionName("NombreTercero")]
        public string ObtenerNombreTerceroDoc(decimal Documento)
        {
            string _resp = "Tercero no encontrado en nuestra base de datos";
            var Tercero = (from Ter in dbSIM.QRY_TERCERO
                           where Ter.DOCUMENTO == Documento
                           select Ter).FirstOrDefault();
            if (Tercero != null)
            {
                _resp = Tercero.TERCERO.ToUpper().Trim();
                if (Tercero.DIGITO <= 0) _resp += ";No se encontró el digito de verificación del documento y esto puede generar error!!";
                if (Tercero.DIRECCION == null || Tercero.DIRECCION.Length == 0) _resp += ";No se encontró la dirección de la instalación y esto puede generar error!!";
                if (Tercero.ID_MUNICIPIO == null) _resp += ";No se encontró el municipio de la instalación y esto puede generar error!!";
                if (Tercero.ID_DEPARTAMENTO == null) _resp += ";No se encontró el departamento de la instalación y esto puede generar error!!";
            }
            return _resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DatosTramite"></param>
        /// <returns></returns>
        [ActionName("CalcularValorTramite")]
        public JObject PostCalculaTramite(DatosValorTramite DatosTramite)
        {
            var _resp = new ParametrosEvaluacion();
            try
            {
                _resp = CalculaTotalesValorTramite(DatosTramite);
            } catch (Exception ex)
            {

            }
            return (JObject)JToken.FromObject(_resp);
        }

        #region Metodos privados de la clase
        private string CalculaTotalesValorTramite(DatosValorTramite datosTramite)
        {
            var Valida = Validar(datosTramite);
            if (Valida.Length > 0) return Valida;
            decimal Salario = ObtenerHonorarios(datosTramite.Agno);
            if (Salario <= 0) return ""
        }

        private decimal ObtenerHonorarios(decimal Agno)
        {
            var Sal = dbSIM.TBTARIFAS_PARAMETRO.Where(w => w.NOMBRE == "SALARIO" && w.ACTIVO == "1" && w.ANO == Agno.ToString()).Select(s => s.VALOR).FirstOrDefault();
            return Sal;  
        }
        private string Validar(DatosValorTramite datosTramite)
        {
            if (datosTramite.NIT == "") return  "Debe ingresar el NIT de la empresa";
            if (datosTramite.Tercero == "") return "Debe ingresar el tercero";
            if (datosTramite.TipoTramite == "") return  "Debe seleccionar un tipo de trámite";
            if (datosTramite.NumeroProfesionales <= 0) return "Debe ingresar la cantidad de profesionales técnicos que participaran en el tramite";
            if (datosTramite.ValorProyecto <= 0) return  "Debe ingresar el valor del proyecto";
            if (long.Parse(datosTramite.TipoTramite) == 26)
            {
                if (datosTramite.CantNormas == 0) return "Debe ingresar la cantidad de Normas";
                if (datosTramite.CantLineas == 0) return "Debe ingresar la cantidad de Líneas";
            }
            return "" ;
        }

        #endregion
    }
}
