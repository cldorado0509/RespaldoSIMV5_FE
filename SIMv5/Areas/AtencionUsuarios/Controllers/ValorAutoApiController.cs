using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Tramites.Models;
using SIM.Data;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;
using System.Security.Claims;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Transactions;
using SIM.Areas.Seguridad.Controllers;
using SIM.Data.Tramites;

namespace SIM.Areas.AtencionUsuarios.Controllers
{
    public class ValorAutoApiController : ApiController
    {
        public struct datosParams
        {
            public string N_DOCUMENTO;
            public int CODIGO_TRAMITE;
            public int N_NUMPROF;
            public int N_VALORPROY;
            public int N_UNIDAD;
            public string N_CM;
            public string S_OBSERVACIONES;
        }

        public struct datosResult
        {
            public int ID_CALCULO_E;
            public int ID_CALCULO_S;
            public int N_SUELDOS_E;
            public int N_VIAJE_E;
            public int N_LABORATORIO_E;
            public int N_ADMINISTRACION_E;
            public int N_TOTALTARIFA_E;
            public int N_TOPES_E;
            public int N_TOTALTRAMITE_E;
            public int N_SUELDOS_S;
            public int N_VIAJE_S;
            public int N_LABORATORIO_S;
            public int N_ADMINISTRACION_S;
            public int N_TOTALTARIFA_S;
            public int N_TOPES_S;
            public int N_TOTALTRAMITE_S;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
            public datosResult valoresCalculo;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema AtencionUsuarios y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //EntitiesTramitesOracle dbSIMTramites = new EntitiesTramitesOracle();

        [HttpPost, ActionName("CalcularValorAuto")]
        public datosRespuesta PostCalcularValorAuto(datosParams parametros)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            datosResult valoresCalculo = new datosResult();

            var honorarios = (from tarifasParametro in dbSIM.TBTARIFAS_PARAMETRO
                             where tarifasParametro.NOMBRE == "SALARIO" && tarifasParametro.ACTIVO == "1" && tarifasParametro.ANO == DateTime.Today.Year.ToString()
                             select tarifasParametro.VALOR).FirstOrDefault();

            if (honorarios <= 0)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "No se ha ingresado un salario mensual para este año para el personal que participara en el trámite." };
            }

            var transporte = (from tarifasParametro in dbSIM.TBTARIFAS_PARAMETRO
                              where tarifasParametro.NOMBRE == "PASAJE" && tarifasParametro.ACTIVO == "1" && tarifasParametro.ANO == DateTime.Today.Year.ToString()
                              select tarifasParametro.VALOR).FirstOrDefault();

            if (transporte <= 0)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "No se ha ingresado un salario mensual para este año para el personal que participara en el trámite." };
            }

            var salarioMinimo = (from tarifasParametro in dbSIM.TBTARIFAS_PARAMETRO
                              where tarifasParametro.NOMBRE == "SMMLV" && tarifasParametro.ACTIVO == "1" && tarifasParametro.ANO == DateTime.Today.Year.ToString()
                              select tarifasParametro.VALOR).FirstOrDefault();

            if (salarioMinimo <= 0)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "No se ha ingresado un valor de salario mínimo mensual como parametro para este año para el calculo de los topes." };
            }

            var tarifaE = (from tarifasTramite in dbSIM.TBTARIFAS_TRAMITE
                          where tarifasTramite.CODIGO_TRAMITE == parametros.CODIGO_TRAMITE && tarifasTramite.TIPO_ACTUACION == "E"
                          group tarifasTramite by tarifasTramite.CODIGO_TRAMITE into tarifasTramiteGroup
                          select new {
                              CODIGO_TRAMITE = tarifasTramiteGroup.Key,
                              AUTO_INICIO = tarifasTramiteGroup.Sum(f => f.AUTO_INICIO),
                              VISITA = tarifasTramiteGroup.Sum(f => f.VISITA),
                              INFORME = tarifasTramiteGroup.Sum(f => f.INFORME),
                              RESOLUCION = tarifasTramiteGroup.Sum(f => f.RESOLUCION),
                              N_VISITAS = tarifasTramiteGroup.Max(f => f.N_VISITAS),
                              //N_FCTINFORME = tarifasTramiteGroup.Max(f => f.N_FCTINFORME),
                              //N_FCTVISITA = tarifasTramiteGroup.Max(f => f.N_FCTVISITA)
                          }).FirstOrDefault();

            var tarifaS = (from tarifasTramite in dbSIM.TBTARIFAS_TRAMITE
                          where tarifasTramite.CODIGO_TRAMITE == parametros.CODIGO_TRAMITE && tarifasTramite.TIPO_ACTUACION == "S"
                          group tarifasTramite by tarifasTramite.CODIGO_TRAMITE into tarifasTramiteGroup
                          select new {
                              CODIGO_TRAMITE = tarifasTramiteGroup.Key,
                              AUTO_INICIO = tarifasTramiteGroup.Sum(f => f.AUTO_INICIO),
                              VISITA = tarifasTramiteGroup.Sum(f => f.VISITA),
                              INFORME = tarifasTramiteGroup.Sum(f => f.INFORME),
                              RESOLUCION = tarifasTramiteGroup.Sum(f => f.RESOLUCION),
                              N_VISITAS = tarifasTramiteGroup.Max(f => f.N_VISITAS),
                              //N_FCTINFORME = tarifasTramiteGroup.Max(f => f.N_FCTINFORME),
                              //N_FCTVISITA = tarifasTramiteGroup.Max(f => f.N_FCTVISITA)
                          }).FirstOrDefault();
            

            // Cálculo Salarios
            /*if (tarifaE.N_VISITAS == null && tarifaE.N_FCTVISITA == null || tarifaS.N_VISITAS == null && tarifaS.N_FCTVISITA == null)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error de Configuración de Visitas." };
            }

            decimal totalHTecnicoE = (decimal)(tarifaE.VISITA == null ? 0 : tarifaE.VISITA) * (tarifaE.N_FCTVISITA == null || tarifaE.N_FCTVISITA == 0 ? (decimal)tarifaE.N_VISITAS : parametros.N_UNIDAD * (decimal)tarifaE.N_FCTVISITA) +
                (decimal)(tarifaE.N_FCTINFORME == null || tarifaE.N_FCTINFORME == 0 ? (decimal)tarifaE.INFORME : parametros.N_UNIDAD * (decimal)tarifaE.N_FCTINFORME);
            decimal totalHTecnicoS = (decimal)(tarifaS.VISITA == null ? 0 : tarifaS.VISITA) * (tarifaS.N_FCTVISITA == null || tarifaS.N_FCTVISITA == 0 ? (decimal)tarifaS.N_VISITAS : parametros.N_UNIDAD * (decimal)tarifaS.N_FCTVISITA) +
                (decimal)(tarifaS.N_FCTINFORME == null || tarifaS.N_FCTINFORME == 0 ? (decimal)(tarifaS.INFORME == null ? 0 : tarifaS.INFORME) : parametros.N_UNIDAD * (decimal)tarifaS.N_FCTINFORME);*/

            decimal totalHTecnicoE = 0; // Se pusieron estos datos en 0 porque el campo N_FCTVISITA ya no mapeaba bien en el modelo, se corregirá cuando se necesite retormar este módulo
            decimal totalHTecnicoS = 0; // Se pusieron estos datos en 0 porque el campo N_FCTVISITA ya no mapeaba bien en el modelo, se corregirá cuando se necesite retormar este módulo

            totalHTecnicoE = totalHTecnicoE * parametros.N_NUMPROF;

            int totalHAbogadoE = (int)(tarifaE.AUTO_INICIO == null ? 0 : tarifaE.AUTO_INICIO) + (int)(tarifaE.RESOLUCION == null ? 0 : tarifaE.RESOLUCION);
            int totalHAbogadoS = (int)(tarifaS.AUTO_INICIO == null ? 0 : tarifaS.AUTO_INICIO) + (int)(tarifaS.RESOLUCION == null ? 0 : tarifaS.RESOLUCION);

            decimal coefDedicaE = Convert.ToDecimal(totalHTecnicoE + totalHAbogadoE) / 240;
            decimal coefDedicaS = Convert.ToDecimal(totalHTecnicoS) / 240;

            valoresCalculo.N_SUELDOS_E = Convert.ToInt32(Math.Round(honorarios * coefDedicaE, 0));
            valoresCalculo.N_SUELDOS_S = Convert.ToInt32(Math.Round(honorarios * coefDedicaS, 0));

            // Cálculo Transporte
            valoresCalculo.N_VIAJE_E = valoresCalculo.N_VIAJE_S = Convert.ToInt32(transporte * parametros.N_NUMPROF * 2);

            // Cálculo Laboratorio
            valoresCalculo.N_LABORATORIO_E = 0;
            valoresCalculo.N_LABORATORIO_S = 0;

            // Cálculo Administración
            valoresCalculo.N_ADMINISTRACION_E = Convert.ToInt32(Math.Round((valoresCalculo.N_SUELDOS_E + valoresCalculo.N_VIAJE_E + valoresCalculo.N_LABORATORIO_E) * Convert.ToDecimal(0.25), 0));
            valoresCalculo.N_ADMINISTRACION_S = Convert.ToInt32(Math.Round((valoresCalculo.N_SUELDOS_S + valoresCalculo.N_VIAJE_S + valoresCalculo.N_LABORATORIO_S) * Convert.ToDecimal(0.25), 0));

            valoresCalculo.N_TOTALTARIFA_E = valoresCalculo.N_SUELDOS_E + valoresCalculo.N_VIAJE_E + valoresCalculo.N_LABORATORIO_E + valoresCalculo.N_ADMINISTRACION_E;
            valoresCalculo.N_TOTALTARIFA_S = valoresCalculo.N_SUELDOS_S + valoresCalculo.N_VIAJE_S + valoresCalculo.N_LABORATORIO_S + valoresCalculo.N_ADMINISTRACION_S;

            // Cálculo Topes
            var tope = (from topes in dbSIM.TBTARIFAS_TOPES
                       where parametros.N_VALORPROY > salarioMinimo*topes.MINIMO && parametros.N_VALORPROY <= salarioMinimo*topes.MAXIMO && (topes.TARIFA != null || topes.TOPE != null)
                       select new {
                           N_TOPEPROY = (topes.TARIFA != null ?  parametros.N_VALORPROY * topes.TARIFA / 100 : topes.TOPE)
                       }).FirstOrDefault();

            if (tope != null)
            {
                valoresCalculo.N_TOPES_E = (int)tope.N_TOPEPROY;
                valoresCalculo.N_TOPES_S = (int)tope.N_TOPEPROY;
            }
            else
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Configuración de Topes Inválida." };
            }

            if (valoresCalculo.N_TOTALTARIFA_E > tope.N_TOPEPROY)
            {
                valoresCalculo.N_TOTALTRAMITE_E = Convert.ToInt32(Math.Round((decimal)tope.N_TOPEPROY));
            }
            else
            {
                valoresCalculo.N_TOTALTRAMITE_E = Convert.ToInt32(Math.Round((decimal)valoresCalculo.N_TOTALTARIFA_E));
            }

            if (valoresCalculo.N_TOTALTARIFA_S > tope.N_TOPEPROY)
            {
                valoresCalculo.N_TOTALTRAMITE_S = Convert.ToInt32(Math.Round((decimal)tope.N_TOPEPROY));
            }
            else
            {
                valoresCalculo.N_TOTALTRAMITE_S = Convert.ToInt32(Math.Round((decimal)valoresCalculo.N_TOTALTARIFA_S));
            }

            var calculoTramiteE = (from calculo in dbSIM.TBTARIFAS_CALCULO
                                   where calculo.NIT == parametros.N_DOCUMENTO && calculo.TIPO == "E" && calculo.SESION == context.Request.LogonUserIdentity.User.Value
                                   select calculo).FirstOrDefault();

            var calculoTramiteS = (from calculo in dbSIM.TBTARIFAS_CALCULO
                                   where calculo.NIT == parametros.N_DOCUMENTO && calculo.TIPO == "S" && calculo.SESION == context.Request.LogonUserIdentity.User.Value
                                   select calculo).FirstOrDefault();

            var fechaActual = DateTime.Now;

            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Debe ingresar a la aplicación con un usuario para poder realizar el cálculo." };
            }

            var validador = Utilidades.Cryptografia.GetSHA1(userId + fechaActual.ToString("yyyyMMddHHmmss")).ToUpper();

            try
            {
                using (var trans = new TransactionScope())
                {
                    if (calculoTramiteE == null) // Nuevo
                    {
                        calculoTramiteE = new TBTARIFAS_CALCULO();

                        calculoTramiteE.NIT = parametros.N_DOCUMENTO;
                        calculoTramiteE.TIPO = "E";
                        calculoTramiteE.SESION = context.Request.LogonUserIdentity.User.Value;
                        calculoTramiteE.CM = parametros.N_CM;
                        calculoTramiteE.CODIGO_TRAMITE = Convert.ToInt32(parametros.CODIGO_TRAMITE);
                        calculoTramiteE.NRO_TECNICOS = parametros.N_NUMPROF;
                        calculoTramiteE.VALOR_PROYECTO = parametros.N_VALORPROY;
                        calculoTramiteE.COSTOS_A = valoresCalculo.N_SUELDOS_E;
                        calculoTramiteE.COSTOS_B = valoresCalculo.N_VIAJE_E;
                        calculoTramiteE.COSTOS_C = valoresCalculo.N_LABORATORIO_E;
                        calculoTramiteE.COSTOS_D = valoresCalculo.N_ADMINISTRACION_E;
                        calculoTramiteE.TOPES = valoresCalculo.N_TOPES_E;
                        calculoTramiteE.VALOR = valoresCalculo.N_TOTALTRAMITE_E;
                        calculoTramiteE.ID_USR = Convert.ToInt32(userId);
                        calculoTramiteE.FECHA = fechaActual;
                        calculoTramiteE.VALIDADOR = validador;
                        calculoTramiteE.CM = parametros.N_CM;
                        calculoTramiteE.OBSERVACION = parametros.S_OBSERVACIONES;
                        //calculoTramiteE.N_UNIDAD = parametros.N_UNIDAD;

                        dbSIM.Entry(calculoTramiteE).State = EntityState.Added;

                        dbSIM.SaveChanges();

                        valoresCalculo.ID_CALCULO_E = Convert.ToInt32(calculoTramiteE.ID_CALCULO);
                    }
                    else
                    {
                        calculoTramiteE.CM = parametros.N_CM;
                        calculoTramiteE.CODIGO_TRAMITE = Convert.ToInt32(parametros.CODIGO_TRAMITE);
                        calculoTramiteE.NRO_TECNICOS = parametros.N_NUMPROF;
                        calculoTramiteE.VALOR_PROYECTO = parametros.N_VALORPROY;
                        calculoTramiteE.COSTOS_A = valoresCalculo.N_SUELDOS_E;
                        calculoTramiteE.COSTOS_B = valoresCalculo.N_VIAJE_E;
                        calculoTramiteE.COSTOS_C = valoresCalculo.N_LABORATORIO_E;
                        calculoTramiteE.COSTOS_D = valoresCalculo.N_ADMINISTRACION_E;
                        calculoTramiteE.TOPES = valoresCalculo.N_TOPES_E;
                        calculoTramiteE.VALOR = valoresCalculo.N_TOTALTRAMITE_E;
                        calculoTramiteE.ID_USR = Convert.ToInt32(userId);
                        calculoTramiteE.FECHA = fechaActual;
                        calculoTramiteE.VALIDADOR = validador;
                        calculoTramiteE.CM = parametros.N_CM;
                        calculoTramiteE.OBSERVACION = parametros.S_OBSERVACIONES;
                        //calculoTramiteE.N_UNIDAD = parametros.N_UNIDAD;

                        dbSIM.Entry(calculoTramiteE).State = EntityState.Modified;

                        dbSIM.SaveChanges();
                        valoresCalculo.ID_CALCULO_E = Convert.ToInt32(calculoTramiteE.ID_CALCULO);
                    }

                    if (calculoTramiteS == null) // Nuevo
                    {
                        calculoTramiteS = new TBTARIFAS_CALCULO();

                        calculoTramiteS.NIT = parametros.N_DOCUMENTO;
                        calculoTramiteS.TIPO = "S";
                        calculoTramiteS.SESION = context.Request.LogonUserIdentity.User.Value;
                        calculoTramiteS.CM = parametros.N_CM;
                        calculoTramiteS.CODIGO_TRAMITE = Convert.ToInt32(parametros.CODIGO_TRAMITE);
                        calculoTramiteS.NRO_TECNICOS = parametros.N_NUMPROF;
                        calculoTramiteS.VALOR_PROYECTO = parametros.N_VALORPROY;
                        calculoTramiteS.COSTOS_A = valoresCalculo.N_SUELDOS_S;
                        calculoTramiteS.COSTOS_B = valoresCalculo.N_VIAJE_S;
                        calculoTramiteS.COSTOS_C = valoresCalculo.N_LABORATORIO_S;
                        calculoTramiteS.COSTOS_D = valoresCalculo.N_ADMINISTRACION_S;
                        calculoTramiteS.TOPES = valoresCalculo.N_TOPES_S;
                        calculoTramiteS.VALOR = valoresCalculo.N_TOTALTRAMITE_S;
                        calculoTramiteS.ID_USR = Convert.ToInt32(userId);
                        calculoTramiteS.FECHA = fechaActual;
                        calculoTramiteS.VALIDADOR = validador;
                        calculoTramiteS.CM = parametros.N_CM;
                        calculoTramiteS.OBSERVACION = parametros.S_OBSERVACIONES;
                        //calculoTramiteS.N_UNIDAD = parametros.N_UNIDAD;

                        dbSIM.Entry(calculoTramiteS).State = EntityState.Added;

                        dbSIM.SaveChanges();

                        valoresCalculo.ID_CALCULO_S = Convert.ToInt32(calculoTramiteS.ID_CALCULO);
                    }
                    else
                    {
                        calculoTramiteS.CM = parametros.N_CM;
                        calculoTramiteS.CODIGO_TRAMITE = Convert.ToInt32(parametros.CODIGO_TRAMITE);
                        calculoTramiteS.NRO_TECNICOS = parametros.N_NUMPROF;
                        calculoTramiteS.VALOR_PROYECTO = parametros.N_VALORPROY;
                        calculoTramiteS.COSTOS_A = valoresCalculo.N_SUELDOS_S;
                        calculoTramiteS.COSTOS_B = valoresCalculo.N_VIAJE_S;
                        calculoTramiteS.COSTOS_C = valoresCalculo.N_LABORATORIO_S;
                        calculoTramiteS.COSTOS_D = valoresCalculo.N_ADMINISTRACION_S;
                        calculoTramiteS.TOPES = valoresCalculo.N_TOPES_S;
                        calculoTramiteS.VALOR = valoresCalculo.N_TOTALTRAMITE_S;
                        calculoTramiteS.ID_USR = Convert.ToInt32(userId);
                        calculoTramiteS.FECHA = fechaActual;
                        calculoTramiteS.VALIDADOR = validador;
                        calculoTramiteS.CM = parametros.N_CM;
                        calculoTramiteS.OBSERVACION = parametros.S_OBSERVACIONES;
                        //calculoTramiteS.N_UNIDAD = parametros.N_UNIDAD;

                        dbSIM.Entry(calculoTramiteS).State = EntityState.Modified;

                        dbSIM.SaveChanges();

                        valoresCalculo.ID_CALCULO_S = Convert.ToInt32(calculoTramiteS.ID_CALCULO);
                    }

                    trans.Complete();
                }
            }
            catch (Exception error)
            {
                return new datosRespuesta { tipoRespuesta = "Error", detalleRespuesta = "Error almacenando el cálculo." };
            }

            return new datosRespuesta { tipoRespuesta = "OK", detalleRespuesta = "", valoresCalculo = valoresCalculo };
        }

        [HttpGet, ActionName("PrintCalculoTramite")]
        public HttpResponseMessage PrintCalculoTramite(int id)
        {
            var tramiteE = dbSIM.TBTARIFAS_CALCULO.FirstOrDefault(f => f.ID_CALCULO == id);
            var tramiteS = dbSIM.TBTARIFAS_CALCULO.FirstOrDefault(f => f.ID_CALCULO == (id + 1));
            var tipoTramite = dbSIM.TBTARIFAS_TRAMITE.FirstOrDefault(f => f.CODIGO_TRAMITE == tramiteE.CODIGO_TRAMITE).NOMBRE;
            long nit = Convert.ToInt64(tramiteE.NIT);
            var terceroRS = dbSIM.TERCERO.FirstOrDefault(f => f.N_DOCUMENTON == nit).S_RSOCIAL;

            var report = new CalculoReport();
            report.CargarDatos(tramiteE, tramiteS, tipoTramite, terceroRS);

            var stream = new MemoryStream();

            report.ExportToPdf(stream);
            report.Dispose();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StreamContent(stream);
            response.Content = new ByteArrayContent(stream.ToArray());
            //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }
    }
}