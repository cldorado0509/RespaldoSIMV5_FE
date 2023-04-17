using AreaMetro.Seguridad;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Web.Internal;
using DevExpress.XtraReports.Native;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.Graphics;
using O2S.Components.PDF4NET.Graphics.Fonts;
using O2S.Components.PDF4NET.Graphics.Shapes;
using SIM.Areas.AtencionUsuarios.Models;
using SIM.Areas.GestionDocumental.Controllers;
using SIM.Areas.Tala.Controllers;
using SIM.Data;
using SIM.Data.Tramites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using static SIM.Areas.Tramites.Controllers.ProyeccionDocumentoApiController;

namespace SIM.Areas.AtencionUsuarios.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
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
                                 CONSECUTIVO = Sop.N_CONSECUTIVO.Value
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// Obtiene los parametros del calculo almacenado en el sistema
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
                    parametrosCalculo.Publicacion = Calculo.PUBLICACION != null ? Calculo.PUBLICACION.Value : 0;
                    parametrosCalculo.Calculado = true;
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
            var _resp = new ParametrosCalculo();
            try
            {
                _resp = CalculaTotalesValorTramite(DatosTramite);
            } catch (Exception ex)
            {
                _resp.Calculado = false;
                _resp.Mensaje = ex.Message;
            }
            return (JObject)JToken.FromObject(_resp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DatosTramite"></param>
        /// <returns></returns>
        [ActionName("ImprimeCalculoTramite")]
        public JObject PostImprimeCalculoTramite(DatosValorTramite DatosTramite)
        {
            var _resp = new ParametrosCalculo();
            MemoryStream oStream = new MemoryStream();
            string _RutaArchSopValorAuto = SIM.Utilidades.Data.ObtenerValorParametro("ArchivosValorAuto").ToString();
            int idUsuario = 0;
            decimal funcionario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            }

            try
            {
                _resp = CalculaTotalesValorTramite(DatosTramite);
                var MaxNum = (from M in dbSIM.TBTARIFAS_CALCULO
                              where M.N_CONSECUTIVO > 0
                              select M.N_CONSECUTIVO).Max();
                if (MaxNum == 0 || MaxNum == null) return null;
                MaxNum++;
                var Tipotra = decimal.Parse(DatosTramite.TipoTramite);
                var PropTramites = (from Tra in dbSIM.TBTARIFAS_TRAMITE
                                    where Tra.CODIGO_TRAMITE == Tipotra && Tra.TIPO_ACTUACION == "E"
                                    select Tra).FirstOrDefault();
                if (PropTramites == null) return null;
                decimal Documento = decimal.Parse(DatosTramite.NIT);
                var Tercero = (from Ter in dbSIM.QRY_TERCERO
                               where Ter.DOCUMENTO == Documento
                               select Ter).FirstOrDefault();
                if (Tercero == null) return null;
                oStream = GeneraSoporte(_resp, DatosTramite, MaxNum.Value, PropTramites, Tercero, funcionario);
                if (oStream.Length > 0)
                {
                    TBTARIFAS_CALCULO _Calculo = new TBTARIFAS_CALCULO();
                    _Calculo.N_CONSECUTIVO = MaxNum.Value;
                    _Calculo.CM = DatosTramite.CM;
                    _Calculo.CODIGO_TRAMITE = decimal.Parse(DatosTramite.TipoTramite);
                    _Calculo.TIPO = "E";
                    _Calculo.TOPES = _resp.Topes;
                    _Calculo.S_TRAMITE = PropTramites.NOMBRE;
                    _Calculo.NRO_TRAMITES = DatosTramite.Items;
                    _Calculo.FECHA = DateTime.Now;
                    _Calculo.COSTOS_A = _resp.Sueldos;
                    _Calculo.COSTOS_A_UNI = _resp.Sueldos / (decimal)DatosTramite.TramitesSINA;
                    _Calculo.COSTOS_B = _resp.Viajes;
                    _Calculo.COSTOS_C = _resp.Otros;
                    _Calculo.COSTOS_D = _resp.Admin;
                    _Calculo.SESION = "";
                    _Calculo.VALOR = _resp.Valor;
                    _Calculo.PUBLICACION = (int)_resp.Publicacion;
                    _Calculo.ID_USR = idUsuario;
                    _Calculo.NIT = DatosTramite.NIT;
                    _Calculo.NRO_TECNICOS = DatosTramite.NumeroProfesionales;
                    _Calculo.VALOR_PROYECTO = DatosTramite.ValorProyecto;
                    _Calculo.FECHA = DateTime.Now;
                    _Calculo.VALIDADOR = Guid.NewGuid().ToString();
                    _Calculo.OBSERVACION = DatosTramite.Observaciones;
                    _Calculo.NRO_ITEMS = DatosTramite.Items;
                    _Calculo.NRO_NORMAS = DatosTramite.CantNormas;
                    _Calculo.NRO_LINEAS = DatosTramite.CantLineas;
                    _Calculo.SOPORTES = DatosTramite.ConSoportes == 1 ? "1" : "0";
                    _Calculo.RELIQUIDACION = "0";
                    _Calculo.ID_CALCULOREL = 0;
                    _Calculo.SALDO_USUARIO = 0;
                    _Calculo.SALDO_ENTIDAD = 0;
                    _Calculo.VALOR_REL = 0;
                    _Calculo.INFORME = DatosTramite.HorasInforme;
                    _Calculo.N_VISITAS = DatosTramite.NumeroVisitas;
                    _Calculo.VISITA = DatosTramite.DuracionVisita;
                    _Calculo.S_DIGITO = Tercero.DIGITO.ToString();
                    _Calculo.S_TERCERO = Tercero.TERCERO;
                    _Calculo.S_DIRECCIONTERCERO = Tercero.DIRECCION;
                    _Calculo.S_CIUDAD = Tercero.MUNICIPIO.ToUpper();
                    _Calculo.ID_CIUDAD = Tercero.ID_MUNICIPIO != null ? (int)Tercero.ID_MUNICIPIO.Value : 0;
                    _Calculo.S_DEPARTAMENTO = Tercero.DEPARTAMENTO.ToUpper();
                    _Calculo.ID_DEPARTAMENTO = Tercero.ID_DEPARTAMENTO != null ? (int)Tercero.ID_DEPARTAMENTO.Value : 0;
                    _Calculo.S_TELEFONOTERCERO = Tercero.TELEFONO.ToString();
                    _Calculo.S_NOMBRE1 = Tercero.NOMBRE1;
                    _Calculo.S_NOMBRE2 = Tercero.NOMBRE2;
                    _Calculo.S_APELLIDO1 = Tercero.APELLIDO1;
                    _Calculo.S_APELLIDO2 = Tercero.APELLIDO2;
                    _Calculo.D_ELABORACION = System.DateTime.Now;
                    _Calculo.D_PAGO = System.DateTime.Now.AddMonths(1);
                    if (int.Parse(PropTramites.CONCEPTO_CONTABLE) == 393) _Calculo.ID_TIPOFACTURA = 5;
                    else _Calculo.ID_TIPOFACTURA = 4;
                    _Calculo.S_TIPOFACTURA = "Permisos Ambientales";
                    _Calculo.S_EXISTESICOF = "0";
                    dbSIM.TBTARIFAS_CALCULO.Add(_Calculo);
                    dbSIM.SaveChanges();
                    decimal _IdCalculo = _Calculo.ID_CALCULO;
                    _resp.IdCalculo = _IdCalculo;   
                    FileInfo _Soporte = new FileInfo(_RutaArchSopValorAuto + _Calculo.D_ELABORACION.Value.ToString("MMyyyy") + @"\" + _IdCalculo.ToString() + ".pdf");
                    if (!_Soporte.Directory.Exists) _Soporte.Directory.Create();
                    SIM.Utilidades.Archivos.GrabaMemoryStream(oStream, _Soporte.FullName);
                }
            }
            catch (Exception ex)
            {
                _resp.Calculado = false;
                _resp.Mensaje = ex.Message;
            }
            return (JObject)JToken.FromObject(_resp);
        }

        #region Metodos privados de la clase
        private ParametrosCalculo CalculaTotalesValorTramite(DatosValorTramite datosTramite)
        {
            var resp = new ParametrosCalculo();
            double factorE = -1;
            double TotalHTecnicoE = 0;
            double TotalHTecnicoEE = 0;
            double ValorSalariosE = 0;
            double ValorSalariosEE = 0;
            double ValorTransporteE = 0;
            resp.Mensaje = "";
            decimal _TipoTramite = decimal.Parse(datosTramite.TipoTramite);
            var Valida = Validar(datosTramite);
            if (Valida.Length > 0) resp.Mensaje = Valida;
            var PropTramite = (from Prop in dbSIM.TBTARIFAS_TRAMITE
                              where Prop.CODIGO_TRAMITE == _TipoTramite && Prop.TIPO_ACTUACION == "E"
                              select Prop).FirstOrDefault();
            if (PropTramite == null) resp.Mensaje = "Existe problema consultando los datos del tipo de trámite";
            decimal Salario = ObtenerHonorarios(datosTramite.Agno);
            if (Salario <= 0) resp.Mensaje = "No se ha ingresado un salario mensual para este año para el personal que participara en el trámite";
            if (PropTramite.N_RELACION.Value > 0)
            {
                if (datosTramite.Items > 50)
                {
                    factorE = datosTramite.Items / (double)PropTramite.N_RELACION.Value;
                    factorE = Math.Ceiling(factorE);
                }
                else factorE = 1;
            }else factorE = 0;
            if (factorE > 0)
            {
                    TotalHTecnicoE = (double)datosTramite.HorasInforme + (double)datosTramite.DuracionVisita;
                    TotalHTecnicoEE = ((double)datosTramite.HorasInforme * factorE) + (double)datosTramite.DuracionVisita;
            }
            else
            {
                    TotalHTecnicoE = (double)datosTramite.HorasInforme + (double)datosTramite.DuracionVisita;
            }
            if (long.Parse(datosTramite.TipoTramite) == 26)
            {
                TotalHTecnicoE = ((double)datosTramite.DuracionVisita + (double)datosTramite.HorasInforme) * (double)datosTramite.NumeroVisitas;
            }
            double TotalHAbogadoE = (double)PropTramite.AUTO_INICIO + (double)PropTramite.RESOLUCION;
            ValorSalariosE = ((double)Salario / 240) * (TotalHTecnicoE + TotalHAbogadoE);
            ValorSalariosEE = ((double)Salario / 240) * (TotalHTecnicoE);
            ValorSalariosE = ValorSalariosE * (double)datosTramite.TramitesSINA;
            resp.Sueldos = (decimal)ValorSalariosE;
            decimal Transporte = ObtenerPasaje(datosTramite.Agno);
            if (Transporte <= 0) resp.Mensaje = "No se ha ingresado un valor de transporte para este año para el personal que participara en el trámite";
            ValorTransporteE = (double)datosTramite.DuracionVisita * (double)Transporte * (double)datosTramite.NumeroVisitas * (double)datosTramite.TramitesSINA;
            resp.Viajes = (decimal)ValorTransporteE;
            resp.Otros = 0;
            double ValorAdminE = (ValorSalariosE + ValorTransporteE + 0) * 0.25;
            double ValorAdminEE = (ValorSalariosEE + ValorTransporteE + 0) * 0.25;
            resp.Admin = (decimal)ValorAdminE;
            double TotalNetoE = (ValorSalariosE + ValorTransporteE + 0 + ValorAdminE);
            if (factorE > 0)
            {
                double TotalNetoEE = (ValorSalariosEE + ValorTransporteE + 0 + ValorAdminEE);
                TotalNetoE = (TotalNetoE - TotalNetoEE) + (TotalNetoEE * factorE);
            }
            resp.Costo = (decimal)TotalNetoE;
            decimal CalTope = CalculaTopes(datosTramite.ValorProyecto, datosTramite.Agno);
            if (CalTope <= 0 ) resp.Mensaje = "No se ha ingresado un valor de salario mínimo mensual como parámetro para este año para el cálculo de los topes.";
            resp.Topes = CalTope;
            if (datosTramite.ConSoportes == 1)
            {
                if ((TotalNetoE / datosTramite.TramitesSINA) > (double)CalTope)
                {
                    CalTope = CalTope * datosTramite.TramitesSINA;
                    resp.Valor = CalTope;
                }
                else
                {
                    resp.Valor = (decimal)TotalNetoE;
                }
            }
            else
            {
                resp.Valor = (decimal)TotalNetoE;
            }
            resp.Publicacion = datosTramite.ValorPublicacion;

            if (resp.Mensaje != "" && resp.Mensaje != null) resp.Calculado = false;
            else resp.Calculado = true;
            return resp;
        }

        private decimal ObtenerHonorarios(decimal Agno)
        {
            var Sal = dbSIM.TBTARIFAS_PARAMETRO.Where(w => w.NOMBRE == "SALARIO" && w.ACTIVO == "1" && w.ANO == Agno.ToString()).Select(s => s.VALOR).FirstOrDefault();
            return Sal;  
        }

        private decimal ObtenerPasaje(decimal Agno)
        {
            var Trans = dbSIM.TBTARIFAS_PARAMETRO.Where(w => w.NOMBRE == "PASAJE" && w.ACTIVO == "1" && w.ANO == Agno.ToString()).Select(s => s.VALOR).FirstOrDefault();
            return Trans;
        }

        private decimal CalculaTopes(decimal ValorProyecto, decimal Agno)
        {
            var topes = dbSIM.TBTARIFAS_TOPES.OrderBy(o => o.ID_TOPE).ToList();
            if (topes == null) return 0;
            var SalMinimo = dbSIM.TBTARIFAS_PARAMETRO.Where(w => w.NOMBRE == "SMMLV" && w.ACTIVO == "1" && w.ANO == Agno.ToString()).Select(s => s.VALOR).FirstOrDefault();
            if (SalMinimo <=  0) return 0;  
            bool _TopePorc = false;
            decimal CoefTope = 0;
            foreach (var tope in topes)
            {
                decimal TopeMin = 0;
                decimal TopeMax = 0;
                if (tope.MINIMO > 0) TopeMin = SalMinimo * tope.MINIMO.Value;
                if (tope.MAXIMO > 0) TopeMax = SalMinimo * tope.MAXIMO.Value;
                if (ValorProyecto > TopeMin && ValorProyecto <= TopeMax)
                {
                    if (tope.TOPE == null && tope.TARIFA != null && tope.TARIFA > 0)
                    {
                        CoefTope = tope.TARIFA.Value;
                        _TopePorc = true;
                        break;
                    } else if (tope.TOPE != null && tope.TOPE > 0 && tope.TARIFA == null)
                    {
                        CoefTope = tope.TOPE.Value;
                        _TopePorc = false;
                        break;
                    }
                }
            }
            decimal TopePry = 0;
            if (_TopePorc) TopePry = (ValorProyecto * CoefTope) / 100;
            else TopePry = CoefTope;
            return TopePry;
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
            decimal _Doctercero = decimal.Parse(datosTramite.NIT);
            var Tercero = (from Ter in dbSIM.QRY_TERCERO
                           where Ter.DOCUMENTO == _Doctercero
                           select Ter).FirstOrDefault();
            if (Tercero != null)
            {
                if (Tercero.DIRECCION == null || Tercero.DIRECCION.Length == 0) return "No se encontró la dirección de la instalación del tercero y esto puede generar error!!";
                if (Tercero.ID_MUNICIPIO == null) return "No se encontró el municipio de la instalación del tercero y esto puede generar error!!";
                if (Tercero.ID_DEPARTAMENTO == null) return "No se encontró el departamento de la instalación del tercero y esto puede generar error!!";
            }
            return "" ;
        }

        private MemoryStream GeneraSoporte(ParametrosCalculo parametrosCalculo, DatosValorTramite Datos, int Consecutivo, TBTARIFAS_TRAMITE Tarifas, QRY_TERCERO Tercero, decimal funcionario)
        {
            MemoryStream Resp = new MemoryStream();
            PDFDocument _doc = new PDFDocument();
            _doc.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
            int MaxNum = Consecutivo;
            int IdTipoFactura = 0;
            PDFPage Pagina = _doc.AddPage();
            Pagina.Width = 2550;
            Pagina.Height = 3300;
            PDFImage _img = new PDFImage(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/Images/Logo_Area.png"));
            Pagina.Canvas.DrawImage(_img, 100, 100, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
            TrueTypeFont _Arial = new TrueTypeFont(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/fonts/arialbd.ttf"), 80, true, true);
            _Arial.Bold = true;
            _Arial.Size = 70;
            PDFBrush brush = new PDFBrush(new PDFRgbColor(Color.Black));
            Pagina.Canvas.DrawText("ÁREA METROPOLITANA DEL", _Arial, null, brush, 600, 120);
            Pagina.Canvas.DrawText("VALLE DE ABURRÁ", _Arial, null, brush, 780, 210);
            _Arial.Bold = false;
            _Arial.Size = 40;
            Pagina.Canvas.DrawText("SOPORTE DE RECIBO DE PAGO", _Arial, null, brush, 1800, 160);
            Pagina.Canvas.DrawText("N°", _Arial, null, brush, 1800, 220);
            Pagina.Canvas.DrawText(MaxNum.ToString(), _Arial, null, brush, 1870, 220);  //Variable
            Pagina.Canvas.DrawText("Entidad Administrativa de Derecho Público", _Arial, null, brush, 750, 290);
            Pagina.Canvas.DrawText("IVA Régimen común", _Arial, null, brush, 970, 330);
            _Arial.Size = 30;
            Pagina.Canvas.DrawText("NIT  890984423-3", _Arial, null, brush, 110, 500);
            Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 110, 540);
            Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 110, 580);
            Pagina.Canvas.DrawText("Colombia", _Arial, null, brush, 110, 620);
            Pagina.Canvas.DrawText("Tel. (574) 385 6000", _Arial, null, brush, 110, 660);
            _Arial.Size = 40;
            _Arial.Bold = true;
            Pagina.Canvas.DrawText("Datos", _Arial, null, brush, 550, 500);
            Pagina.Canvas.DrawText("Usuario", _Arial, null, brush, 550, 550);
            PDFPen _Pen = new PDFPen(new PDFRgbColor(Color.Black), 5);
            Pagina.Canvas.DrawLine(_Pen, 720, 500, 720, 745);
            _Arial.Bold = false;
            if (Datos.Tercero.Trim().Length < 41) Pagina.Canvas.DrawText(Datos.Tercero.Trim().ToUpper(), _Arial, null, brush, 750, 500); // Variable
            else
            {
                Pagina.Canvas.DrawText(Datos.Tercero.Trim().ToUpper().Substring(0, 40), _Arial, null, brush, 750, 500); // Variable
                Pagina.Canvas.DrawText(Datos.Tercero.Trim().ToUpper().Substring(39, Datos.Tercero.Trim().Length - 40), _Arial, null, brush, 750, 540); // Variable
            }
            var _Tercero = Tercero;
            if (_Tercero.DIGITO.ToString() != "") Pagina.Canvas.DrawText(Datos.NIT + "-" + _Tercero.DIGITO.ToString(), _Arial, null, brush, 750, 600); // Variable
            else Pagina.Canvas.DrawText(Datos.NIT, _Arial, null, brush, 750, 600); // Variable
            Pagina.Canvas.DrawText(_Tercero.DIRECCION.Trim().ToUpper(), _Arial, null, brush, 750, 640); // Variable
            Pagina.Canvas.DrawText(_Tercero.MUNICIPIO.Trim() + " - " + _Tercero.DEPARTAMENTO.Trim(), _Arial, null, brush, 750, 680); // Variable
            Pagina.Canvas.DrawText("Teléfono : " + _Tercero.TELEFONO.ToString().Trim(), _Arial, null, brush, 750, 720); // Variable
            var _codentidad = (from Dep in dbSIM.DEPENDENCIA 
                               where Dep.ID_DEPENDENCIA == (from Df in dbSIM.FUNCIONARIO_DEPENDENCIA
                                                            where Df.CODFUNCIONARIO == funcionario && Df.D_SALIDA == null
                                                            select Df.ID_DEPENDENCIA).FirstOrDefault()
                               select Dep.CODENTIDAD).FirstOrDefault();
            if (_codentidad == "10")
            {
                Pagina.Canvas.DrawText("Municipio: Envigado", _Arial, null, brush, 1800, 340);
                Pagina.Canvas.DrawText("Secretaría del Medio Ambiente", _Arial, null, brush, 1800, 400);
                Pagina.Canvas.DrawText("y Desarrollo Agropecuario", _Arial, null, brush, 1800, 460);
            }
            _Pen.Width = 3;
            _Arial.Size = 30;
            Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 565, 600, 120, 0);
            Pagina.Canvas.DrawText("Fecha Elaboración", _Arial, null, brush, 1810, 580);
            Pagina.Canvas.DrawText(DateTime.Now.ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 580); //Variable
            Pagina.Canvas.DrawLine(_Pen, 1800, 630, 2400, 630);
            Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 1810, 640);
            Pagina.Canvas.DrawText(DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd"), _Arial, null, brush, 2180, 640);
            _Arial.Size = 35;
            _Arial.Bold = true;
            Pagina.Canvas.DrawText("                       AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 500, 800);
            Pagina.Canvas.DrawLine(_Pen, 100, 850, 2400, 850);
            Pagina.Canvas.DrawRectangle(_Pen, null, 100, 870, 2300, 900, 0);
            Pagina.Canvas.DrawLine(_Pen, 100, 930, 2400, 930);
            _Arial.Size = 30;
            Pagina.Canvas.DrawText("CONCEPTO", _Arial, null, brush, 115, 890);
            Pagina.Canvas.DrawLine(_Pen, 300, 870, 300, 1770);
            Pagina.Canvas.DrawText("DESCRIPCIÓN", _Arial, null, brush, 600, 890);
            Pagina.Canvas.DrawLine(_Pen, 1150, 870, 1150, 1770);
            Pagina.Canvas.DrawText("DETALLE", _Arial, null, brush, 1550, 890);
            Pagina.Canvas.DrawLine(_Pen, 2100, 870, 2100, 1770);
            Pagina.Canvas.DrawText("TOTAL", _Arial, null, brush, 2200, 890);
            Pagina.Canvas.DrawRectangle(_Pen, null, 100, 1770, 2300, 60, 0);
            Pagina.Canvas.DrawText("VALOR EN LETRAS :", _Arial, null, brush, 110, 1795);
            Pagina.Canvas.DrawRectangle(_Pen, null, 1800, 1830, 600, 60, 0);
            Pagina.Canvas.DrawText("TOTAL  ", _Arial, null, brush, 1840, 1850);
            _Arial.Bold = false;
            int _Linea = 960;
            long _ValFact = (long)parametrosCalculo.Valor;
            PDFTextFormatOptions tfo = new PDFTextFormatOptions();
            tfo.Align = PDFTextAlign.TopJustified;
            tfo.ClipText = PDFClipText.ClipNone;
            if (Tarifas != null)
            {
                double LinNom = 0;
                double LinObs = 0;
                Pagina.Canvas.DrawText(Tarifas.CONCEPTO_CONTABLE.Trim(), _Arial, null, brush, 145, _Linea); //Variable
                if (Tarifas.NOMBRE.ToString().Trim().Length <= 41) Pagina.Canvas.DrawText(Tarifas.NOMBRE.ToString().Trim().ToUpper(), _Arial, null, brush, 310, _Linea);  //Variable
                else
                {
                    Pagina.Canvas.DrawTextBox(Tarifas.NOMBRE.ToString().Trim().ToUpper(), _Arial, brush, 310, _Linea, 810, 100, tfo);  //Variable
                    LinNom = Math.Round((double)(Tarifas.NOMBRE.Trim().Length / 41), MidpointRounding.AwayFromZero);
                }
                Pagina.Canvas.DrawTextBox(Datos.Observaciones.Trim().ToUpper(), _Arial, brush, 1160, 960, 900, 100, tfo); //Variable
                LinObs = Math.Round((double)(Datos.Observaciones.Trim().Length / 50), MidpointRounding.AwayFromZero);
                Pagina.Canvas.DrawText(double.Parse(parametrosCalculo.Valor.ToString()).ToString("C0"), _Arial, null, brush, 2110, _Linea); //Variable
                if (LinNom > LinObs) _Linea += (int)(LinNom * 40);
                else if (LinNom < LinObs) _Linea += (int)(LinObs * 40);
                else _Linea += (int)(LinObs * 40);
                _Linea += 100;
                if (parametrosCalculo.Publicacion > 0)
                {
                    Pagina.Canvas.DrawText(Tarifas.CONCEPTO_PUBLICACION.Trim(), _Arial, null, brush, 145, _Linea); //Variable
                    if (Tarifas.CONCEPTO_PUBLICACION.Trim() != "395") Pagina.Canvas.DrawText("PUBLICACIONES AMBIENTALES", _Arial, null, brush, 310, _Linea);  //Variable
                    else Pagina.Canvas.DrawText("PUBLICACIONES RECURSO FORESTAL (FONDO VERDE)", _Arial, null, brush, 310, _Linea);  //Variable
                    Pagina.Canvas.DrawText(double.Parse(parametrosCalculo.Publicacion.ToString()).ToString("C0"), _Arial, null, brush, 2110, _Linea); //Variable
                    _ValFact += (long)parametrosCalculo.Publicacion;
                }
                if (int.Parse(Tarifas.CONCEPTO_CONTABLE) == 393) IdTipoFactura = 5;
                else IdTipoFactura = 4;
            } else IdTipoFactura = 4;
            Pagina.Canvas.DrawText(SIM.Utilidades.Facturacion.enletras(_ValFact.ToString().Trim()), _Arial, null, brush, 450, 1795);  //Variable
            Pagina.Canvas.DrawText(_ValFact.ToString("C0"), _Arial, null, brush, 2110, 1850);  //Variable
            Pagina.Canvas.DrawText("Por favor conserve la parte superior para verificar su pago", _Arial, null, brush, 145, 2000);
            var TipoFactura = (from _Clave in dbSIM.TIPO_FACTURA
                             where _Clave.ID_TIPOFACTURA == IdTipoFactura
                             select _Clave).FirstOrDefault();
            if (!string.IsNullOrEmpty(TipoFactura.S_CLAVE))
            {
                string _NIT = long.Parse(Datos.NIT).ToString("00000000000");
                string _FACT = MaxNum.ToString("000000000");
                string _ValorF = _ValFact.ToString("00000000000000");
               // if ((_ValorF.Length % 2) == 1) _ValorF = "0" + _ValorF;
                string _FechaPago = DateTime.Now.AddMonths(1).ToString("yyyyMMdd");
                string _TextCode = "415" + TipoFactura.S_CLAVE.Trim() + "8020" + _FACT + _NIT + "#3900" + _ValorF + "#96" + _FechaPago; //Variable
                Image _ImgBc = AreaMetro.Utilidades.CodeBar.CodeBar128Fact(_TextCode, DateTime.Now.AddMonths(1), new System.Drawing.Size(1300, 250));
                PDFImage _BcImg = new PDFImage((Bitmap)_ImgBc);
                Pagina.Canvas.DrawImage(_BcImg, 1000, 1910, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
                Pagina.Canvas.DrawImage(_BcImg, 1000, 2510, double.Parse(_BcImg.Width.ToString()), double.Parse(_BcImg.Height.ToString()), 0, PDFKeepAspectRatio.KeepNone);
            }
            _Arial.Size = 40;
            _Arial.Bold = true;
            _Pen.DashStyle = PDFDashStyle.Dash;
            Pagina.Canvas.DrawLine(_Pen, 100, 2220, 2400, 2220);
            _Pen.DashStyle = PDFDashStyle.Solid;
            Pagina.Canvas.DrawText("SOPORTE DE RECIBO DE PAGO", _Arial, null, brush, 1100, 2250);
            Pagina.Canvas.DrawRectangle(_Pen, null, 135, 2350, 700, 240, 0);
            Pagina.Canvas.DrawRectangle(_Pen, null, 1500, 2350, 700, 120, 0);
            _Arial.Size = 30;
            Pagina.Canvas.DrawText("Soporte N°", _Arial, null, brush, 145, 2370);
            Pagina.Canvas.DrawLine(_Pen, 450, 2350, 450, 2590);
            Pagina.Canvas.DrawLine(_Pen, 135, 2410, 835, 2410);
            Pagina.Canvas.DrawText("Banco", _Arial, null, brush, 1510, 2370);
            Pagina.Canvas.DrawLine(_Pen, 1780, 2350, 1780, 2470);
            Pagina.Canvas.DrawLine(_Pen, 1500, 2410, 2200, 2410);
            Pagina.Canvas.DrawText("NIT Cliente", _Arial, null, brush, 145, 2430);
            Pagina.Canvas.DrawLine(_Pen, 135, 2470, 835, 2470);
            Pagina.Canvas.DrawText("Cuenta Ahorros N°", _Arial, null, brush, 1510, 2430);
            Pagina.Canvas.DrawText("Fecha Vencimiento", _Arial, null, brush, 145, 2490);
            Pagina.Canvas.DrawLine(_Pen, 135, 2530, 835, 2530);
            Pagina.Canvas.DrawText("Valor Total", _Arial, null, brush, 145, 2550);
            _Arial.Bold = false;
            Pagina.Canvas.DrawText(MaxNum.ToString(), _Arial, null, brush, 490, 2370); //Variable
            if (TipoFactura.S_BANCO != "") Pagina.Canvas.DrawText(TipoFactura.S_BANCO.Trim().ToUpper(), _Arial, null, brush, 1820, 2370); //Variable
            if (_Tercero.DIGITO.ToString() != "") Pagina.Canvas.DrawText(Datos.NIT.Trim() + "-" + _Tercero.DIGITO.ToString(), _Arial, null, brush, 490, 2430); //Variable
            else Pagina.Canvas.DrawText(Datos.NIT.Trim(), _Arial, null, brush, 490, 2430); //Variable
            Pagina.Canvas.DrawText(TipoFactura.S_CUENTA.Trim(), _Arial, null, brush, 1820, 2430); //Variable
            Pagina.Canvas.DrawText(DateTime.Now.AddMonths(1).ToString("yyyy/MM/dd"), _Arial, null, brush, 490, 2490); //Variable
            Pagina.Canvas.DrawText(_ValFact.ToString("C0"), _Arial, null, brush, 490, 2550); //Variable
            Pagina.Canvas.DrawLine(_Pen, 100, 2770, 2400, 2770);
            Pagina.Canvas.DrawImage(_img, 100, 2790, _img.Width, _img.Height, 0, PDFKeepAspectRatio.KeepNone);
            Pagina.Canvas.DrawLine(_Pen, 650, 2790, 650, 3157);
            _Arial.Size = 25;
            Pagina.Canvas.DrawText("Carrera 53 # 40A - 31", _Arial, null, brush, 700, 2950);
            Pagina.Canvas.DrawText("Ed. Área Metropolitana", _Arial, null, brush, 700, 3000);
            Pagina.Canvas.DrawText("NIT 890984423-3", _Arial, null, brush, 700, 3050);
            Pagina.Canvas.DrawText("Medellín - Antioquia", _Arial, null, brush, 700, 3100);
            Pagina.Canvas.DrawLine(_Pen, 1350, 2790, 1350, 3157);
            Pagina.Canvas.DrawText("IVA Régimen Común", _Arial, null, brush, 1400, 2900);
            Pagina.Canvas.DrawText("AGENTE RETENEDOR DEL IMPUESTO SOBRE LAS VENTAS", _Arial, null, brush, 1400, 3000);
            Pagina.Canvas.DrawText("metropol@metropol.gov.co", _Arial, null, brush, 1400, 3050);
            Pagina.Canvas.DrawText("http://www.metropol.gov.co", _Arial, null, brush, 1400, 3100);
            _doc.Save(Resp);
            return Resp;
        }

        private int ObtieneDigitoVerificacionNIT(string numeroIdentificacion)
        {
            var digitos = new byte[9];
            for (int i = 0; i < numeroIdentificacion.Length; i++)
            {
                if (!char.IsDigit(numeroIdentificacion[i]))
                    return -1;
                digitos[i] = byte.Parse(numeroIdentificacion[i].ToString());
            }
            var v = 41 * digitos[0] +
                    37 * digitos[1] +
                    29 * digitos[2] +
                    23 * digitos[3] +
                    19 * digitos[4] +
                    17 * digitos[5] +
                    13 * digitos[6] +
                    7 * digitos[7] +
                    3 * digitos[8];
            v %= 11;
            if (v >= 2)
                v = 11 - v;
            return v ;
        }
        #endregion
    }
}
