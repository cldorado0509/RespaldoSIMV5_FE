using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Newtonsoft.Json;
using System.Security.Claims;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    public class InstalacionApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize]
        [HttpGet, ActionName("Instalaciones")]
        public datosConsulta GetInstalaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            dynamic modelData;

            var administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                administrador = claimPpal.IsInRole("XINSTALACION");
            }

            if (!administrador)
            {
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) != null)
                {
                    filter = (filter == null ? "" : filter);
                    filter = "ID_TERCERO,=," + ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value + (filter == "" ? "" : ",and," + filter);
                }
                else
                {
                    datosConsulta resultado = new datosConsulta();
                    resultado.numRegistros = 0;
                    resultado.datos = null;

                    return resultado;
                }
            }

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var model = (from instalacion in dbSIM.INSTALACION
                                         join terceroinstalacion in dbSIM.TERCERO_INSTALACION on instalacion.ID_INSTALACION equals terceroinstalacion.ID_INSTALACION
                                         //join tipoinstalacion in dbSIM.TIPO_INSTALACION on terceroinstalacion.ID_TIPOINSTALACION equals tipoinstalacion.ID_TIPOINSTALACION
                                         join divipola in dbSIM.DIVIPOLA on instalacion.ID_DIVIPOLA equals divipola.ID_DIVIPOLA into divipolaInstalacion
                                         from di in divipolaInstalacion.DefaultIfEmpty()
                                         select new
                                         {
                                             terceroinstalacion.ID_TERCERO,
                                             instalacion.ID_INSTALACION,
                                             instalacion.S_NOMBRE,
                                             S_MUNICIPIO = di.S_NOMBRE,
                                             S_DIRECCION = instalacion.TIPO_VIA.S_ABREVIATURA + " " + instalacion.N_NUMEROVIAPPAL.ToString() + instalacion.LETRA_VIA.S_NOMBRE + " " + instalacion.S_SENTIDOVIAPPAL + " " + instalacion.TIPO_VIA1.S_ABREVIATURA + " " + instalacion.N_NUMEROVIASEC.ToString() + instalacion.LETRA_VIA1.S_NOMBRE + " " + instalacion.S_SENTIDOVIASEC + " " + instalacion.N_PLACA.ToString() + " " + instalacion.N_INTERIOR.ToString(),
                                             //S_TIPOINSTALACION = tipoinstalacion.S_NOMBRE,
                                             //S_ACTIVIDADECONOMICA = terceroinstalacion.TERCERO.ACTIVIDAD_ECONOMICA.S_NOMBRE,
                                             S_ESTADO = instalacion.ESTADO.S_NOMBRE
                                         });

                            modelData = model;
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from instalacion in dbSIM.INSTALACION
                                         select new
                                         {
                                             instalacion.ID_INSTALACION,
                                             instalacion.S_NOMBRE
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from instalacion in dbSIM.INSTALACION
                                         select new
                                         {
                                             instalacion.ID_INSTALACION,
                                             instalacion.S_NOMBRE
                                         });

                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        // GET api/Tercero/<id>
        [HttpGet, ActionName("Instalacion")]
        public object GetInstalacion(int id, int idTercero)
        {
            if (id > 0)
            {
                var instalacion = from instalacionConsulta in dbSIM.INSTALACION
                                  where instalacionConsulta.ID_INSTALACION == id
                                  select new
                                  {
                                      instalacionConsulta.ID_INSTALACION,
                                      instalacionConsulta.S_NOMBRE,
                                      instalacionConsulta.S_CEDULACATASTRAL,
                                      instalacionConsulta.S_MATRICULAINMOBILIARIA,
                                      instalacionConsulta.S_TELEFONO,
                                      instalacionConsulta.ID_DIVIPOLA,
                                      instalacionConsulta.D_REGISTRO,
                                      instalacionConsulta.ID_TIPOVIAPPAL,
                                      instalacionConsulta.N_NUMEROVIAPPAL,
                                      instalacionConsulta.ID_LETRAVIAPPAL,
                                      instalacionConsulta.S_SENTIDOVIAPPAL,
                                      instalacionConsulta.ID_TIPOVIASEC,
                                      instalacionConsulta.N_NUMEROVIASEC,
                                      instalacionConsulta.ID_LETRAVIASEC,
                                      instalacionConsulta.S_SENTIDOVIASEC,
                                      instalacionConsulta.N_PLACA,
                                      instalacionConsulta.N_INTERIOR,
                                      instalacionConsulta.N_COORDX,
                                      instalacionConsulta.N_COORDY,
                                      instalacionConsulta.S_OBSERVACION,
                                      /*TERCERO_INSTALACION = new {
                                          instalacionConsulta.TERCERO_INSTALACION.FirstOrDefault().ID_TERCERO,
                                          instalacionConsulta.TERCERO_INSTALACION.FirstOrDefault().ID_ACTIVIDADECONOMICA,
                                          instalacionConsulta.TERCERO_INSTALACION.FirstOrDefault().ID_TIPOINSTALACION,
                                          instalacionConsulta.TERCERO_INSTALACION.FirstOrDefault().D_INICIO,
                                      }*/

                                      //TERCERO_INSTALACION = instalacionConsulta.TERCERO_INSTALACION.FirstOrDefault(ti => ti.D_FIN == null)
                                      TERCERO_INSTALACION = from terceroInstalacion in dbSIM.TERCERO_INSTALACION
                                                            where terceroInstalacion.ID_INSTALACION == instalacionConsulta.ID_INSTALACION //&& terceroInstalacion.D_FIN == null
                                                            select new
                                                            {
                                                                terceroInstalacion.ID_TERCERO,
                                                                terceroInstalacion.ID_ACTIVIDADECONOMICA,
                                                                VERSION_AE = terceroInstalacion.ACTIVIDAD_ECONOMICA.S_VERSION,
                                                                terceroInstalacion.ID_TIPOINSTALACION,
                                                                terceroInstalacion.D_INICIO
                                                            },
                                      INSTALACION_TIPO = from instalacionTipo in dbSIM.INSTALACION_TIPO
                                                         where instalacionTipo.ID_INSTALACION == instalacionConsulta.ID_INSTALACION
                                                         select instalacionTipo.ID_TIPO
                                  };

                return instalacion.FirstOrDefault();
            }
            else
            {
                INSTALACION instalacion = new INSTALACION();
                instalacion.TERCERO_INSTALACION = new TERCERO_INSTALACION[] { new TERCERO_INSTALACION() { ID_TERCERO = idTercero } };

                return instalacion;
            }
        }

        // POST api/<controller>
        [HttpPost, ActionName("Instalacion")]
        public object Post(INSTALACION item)
        {
            bool nuevo = false;
            var model = dbSIM.INSTALACION;
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ID_INSTALACION == 0) // Nueva Instalacion
                    {
                        nuevo = true;
                        item.D_REGISTRO = DateTime.Now;
                        dbSIM.Entry(item).State = EntityState.Added;
                        var terceroInstalacion = item.TERCERO_INSTALACION.FirstOrDefault();
                        if (terceroInstalacion != null)
                        {
                            terceroInstalacion.S_ACTUAL = "0";
                            dbSIM.Entry(terceroInstalacion).State = EntityState.Added;
                        }
                        dbSIM.SaveChanges();
                    }
                    else // Instalacion Existente
                    {
                        var modelItem = model.FirstOrDefault(it => it.ID_INSTALACION == item.ID_INSTALACION);
                        if (modelItem != null)
                        {
                            modelItem.ID_INSTALACION = item.ID_INSTALACION;
                            modelItem.S_NOMBRE = item.S_NOMBRE;
                            modelItem.S_CEDULACATASTRAL = item.S_CEDULACATASTRAL;
                            modelItem.S_MATRICULAINMOBILIARIA = item.S_MATRICULAINMOBILIARIA;
                            modelItem.S_TELEFONO = item.S_TELEFONO;
                            modelItem.ID_DIVIPOLA = item.ID_DIVIPOLA;
                            modelItem.D_REGISTRO = item.D_REGISTRO;
                            modelItem.ID_TIPOVIAPPAL = item.ID_TIPOVIAPPAL;
                            modelItem.N_NUMEROVIAPPAL = item.N_NUMEROVIAPPAL;
                            modelItem.ID_LETRAVIAPPAL = item.ID_LETRAVIAPPAL;
                            modelItem.S_SENTIDOVIAPPAL = item.S_SENTIDOVIAPPAL;
                            modelItem.ID_TIPOVIASEC = item.ID_TIPOVIASEC;
                            modelItem.N_NUMEROVIASEC = item.N_NUMEROVIASEC;
                            modelItem.ID_LETRAVIASEC = item.ID_LETRAVIASEC;
                            modelItem.S_SENTIDOVIASEC = item.S_SENTIDOVIASEC;
                            modelItem.N_PLACA = item.N_PLACA;
                            modelItem.N_INTERIOR = item.N_INTERIOR;
                            modelItem.N_COORDX = item.N_COORDX;
                            modelItem.N_COORDY = item.N_COORDY;
                            modelItem.S_OBSERVACION = item.S_OBSERVACION;

                            //var terceroInstalacion = modelItem.TERCERO_INSTALACION.FirstOrDefault(ti => ti.D_FIN == null);
                            var terceroInstalacion = modelItem.TERCERO_INSTALACION.FirstOrDefault();
                            if (terceroInstalacion != null && terceroInstalacion.ID_TERCERO == item.TERCERO_INSTALACION.FirstOrDefault().ID_TERCERO)
                            {
                                terceroInstalacion.ID_ACTIVIDADECONOMICA = item.TERCERO_INSTALACION.FirstOrDefault().ID_ACTIVIDADECONOMICA;
                                terceroInstalacion.ID_TIPOINSTALACION = item.TERCERO_INSTALACION.FirstOrDefault().ID_TIPOINSTALACION;
                                terceroInstalacion.D_INICIO = item.TERCERO_INSTALACION.FirstOrDefault().D_INICIO;
                            }
                            else
                            {
                                if (terceroInstalacion != null)
                                    terceroInstalacion.D_FIN = DateTime.Today;

                                TERCERO_INSTALACION terceroInstalacionNueva = new TERCERO_INSTALACION();
                                terceroInstalacionNueva.ID_INSTALACION = item.ID_INSTALACION;
                                terceroInstalacionNueva.ID_TERCERO = item.TERCERO_INSTALACION.FirstOrDefault().ID_TERCERO;
                                terceroInstalacionNueva.ID_ACTIVIDADECONOMICA = item.TERCERO_INSTALACION.FirstOrDefault().ID_ACTIVIDADECONOMICA;
                                terceroInstalacionNueva.ID_TIPOINSTALACION = item.TERCERO_INSTALACION.FirstOrDefault().ID_TIPOINSTALACION;
                                terceroInstalacionNueva.D_INICIO = item.TERCERO_INSTALACION.FirstOrDefault().D_INICIO;
                                terceroInstalacionNueva.D_REGISTRO = DateTime.Today;
                                terceroInstalacionNueva.S_ACTUAL = "0";

                                modelItem.TERCERO_INSTALACION.Add(terceroInstalacionNueva);
                            }

                            dbSIM.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = (nuevo ? "Error Insertando Instalación" : "Error Actualizando Instalación") };
                }
                return new { resp = "OK", mensaje = (nuevo ? "Instalación Insertada Satisfactoriamente" : "Instalación Actualizada Satisfactoriamente"), datos = item };
            }
            else
                return new { resp = "Error", mensaje = "Datos Inválidos" };
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}