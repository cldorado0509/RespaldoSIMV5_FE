using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.GestionDocumental.Models;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Areas.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Security.Claims;
using SIM.Areas.Tramites.Models;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using SIM.Utilidades;
using SIM.Data;
using SIM.Models;
using SIM.Data.Control;
using SIM.Data.Tramites;

namespace SIM.Areas.Tramites.Controllers
{
    public class ActuacionJuridicaApiController : ApiController
    {
        public class DatosCaracteristicas
        {
            public int idActuacion { get; set; }
            public int idTipoActo { get; set; }
            public int idTramite { get; set; }
            public int idDocumento { get; set; }
            public int idSerie { get; set; }
            public string documento { get; set; }
            public int idEstado { get; set; }
            public int idFormulario { get; set; }
            public int idTercero { get; set; }
            public string jsonDatos { get; set; }
            public bool finalizar { get; set; }
            public string error { get; set; }
        }

        public class documentoTramiteRes
        {
            public int idTramite { get; set; }
            public int idDocumento { get; set; }
        }

        public class ItemsFormulario
        {
            public int ID { get; set; }
            public string NOMBRE { get; set; }
        }

        public class ActuacionesItem
        {
            public int ID_ESTADO { get; set; }
            public int ID_VISITA { get; set; }
            public string ASUNTO { get; set; }
            public int ID_TIPOVISITA { get; set; }
            public string TIPO_VISITA { get; set; }
            public int ID_ESTADOVISITA { get; set; }
            public string ESTADO { get; set; }
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        [HttpGet, ActionName("PruebaJson")]
        public void PruebaJson()
        {
            var a = new FormularioDatos();

            var b = a.ObtenerJsonEstadoActuacion(40, 12, 0, false);

            var json = b.json;
        }

        [HttpGet, ActionName("ConsultaInstalacion")]
        public datosConsulta GetConsultaInstalacion(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado;
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from tercero in dbSIM.TERCERO
                             join terceroInstalacion in dbSIM.TERCERO_INSTALACION on tercero.ID_TERCERO equals terceroInstalacion.ID_TERCERO
                             join instalacion in dbSIM.INSTALACION on terceroInstalacion.ID_INSTALACION equals instalacion.ID_INSTALACION
                             join proyecto in dbSIM.TBPROYECTO on terceroInstalacion.CODIGO_PROYECTO equals proyecto.CODIGO_PROYECTO into instalacionCM
                             from proyecto in instalacionCM.DefaultIfEmpty()
                             select new
                             {
                                 tercero.ID_TERCERO,
                                 N_DOCUMENTO = tercero.N_DOCUMENTON.ToString(),
                                 S_RSOCIAL = tercero.S_RSOCIAL.Trim(),
                                 INSTALACION = instalacion.S_NOMBRE.Trim(),
                                 CM = proyecto == null ? "(Sin CM)" : proyecto.CM,
                                 ID_POPUP = instalacion.ID_INSTALACION.ToString() + "," + (proyecto == null ? "" : proyecto.CODIGO_PROYECTO.ToString()),
                                 NOMBRE_POPUP = (proyecto == null ? "(Sin CM)" : "CM: " + proyecto.CM) + " - " + tercero.S_RSOCIAL.Trim() + " - " + instalacion.S_NOMBRE.Trim()
                             });
                modelData = model;
            }

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ConsultaDocumentos")]
        public datosConsulta GetConsultaDocumentos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

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
                            try
                            {
                                var model = (from documento in dbSIM.TBTRAMITEDOCUMENTO
                                             join indiceDocumentoAno in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 163) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAno.CODTRAMITE, codDocumento = (int)indiceDocumentoAno.CODDOCUMENTO } // Año
                                             join indiceDocumentoRes in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 43) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoRes.CODTRAMITE, codDocumento = (int)indiceDocumentoRes.CODDOCUMENTO } // Resolución
                                             join indiceDocumentoAsunto in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 45) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAsunto.CODTRAMITE, codDocumento = (int)indiceDocumentoAsunto.CODDOCUMENTO } // Asunto
                                             //join indiceDocumentoEmpresa in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 884) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoEmpresa.CODTRAMITE, codDocumento = (int)indiceDocumentoEmpresa.CODDOCUMENTO } // Empresa
                                             join indiceDocumentoCM in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 98) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoCM.CODTRAMITE, codDocumento = (int)indiceDocumentoCM.CODDOCUMENTO } // CM
                                             join indiceTipoResolucion in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 99) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceTipoResolucion.CODTRAMITE, codDocumento = (int)indiceTipoResolucion.CODDOCUMENTO } // Tipo Resolución
                                             join indiceDocumentoEmpresa in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 884) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoEmpresa.CODTRAMITE, codDocumento = (int)indiceDocumentoEmpresa.CODDOCUMENTO } into indiceDocumentoEmpresaLJ // Empresa
                                             from indiceDocumentoEmpresa in indiceDocumentoEmpresaLJ.DefaultIfEmpty()
                                             join serie in dbSIM.TBSERIE on documento.CODSERIE equals serie.CODSERIE
                                             where documento.CODSERIE == 11
                                             select new
                                             {
                                                 documento.CODSERIE,
                                                 ID_POPUP = documento.CODTRAMITE.ToString() + "," + documento.CODDOCUMENTO.ToString(),
                                                 TIPO_DOCUMENTO = serie.NOMBRE,
                                                 ANO = indiceDocumentoAno.VALOR,
                                                 NUMERO = indiceDocumentoRes.VALOR,
                                                 ASUNTO = indiceDocumentoAsunto.VALOR,
                                                 EMPRESA = indiceDocumentoEmpresa.VALOR,
                                                 CM = indiceDocumentoCM.VALOR,
                                                 NOMBRE_POPUP = indiceDocumentoAno.VALOR + "-" + indiceDocumentoRes.VALOR + " [" + indiceDocumentoAsunto.VALOR + "]",
                                                 TIPO = indiceTipoResolucion.VALOR
                                             }).Union(
                                             from documento in dbSIM.TBTRAMITEDOCUMENTO
                                             join indiceDocumentoAno in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 162) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAno.CODTRAMITE, codDocumento = (int)indiceDocumentoAno.CODDOCUMENTO } // Año
                                             join indiceDocumentoAuto in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 103) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAuto.CODTRAMITE, codDocumento = (int)indiceDocumentoAuto.CODDOCUMENTO } // Auto
                                             join indiceDocumentoSolicitud in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 108) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoSolicitud.CODTRAMITE, codDocumento = (int)indiceDocumentoSolicitud.CODDOCUMENTO } // Solicitud
                                             //join indiceDocumentoEmpresa in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 861) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoEmpresa.CODTRAMITE, codDocumento = (int)indiceDocumentoEmpresa.CODDOCUMENTO } // Empresa
                                             join indiceDocumentoCM in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 101) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoCM.CODTRAMITE, codDocumento = (int)indiceDocumentoCM.CODDOCUMENTO } // CM
                                             join indiceTipoAuto in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 102) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceTipoAuto.CODTRAMITE, codDocumento = (int)indiceTipoAuto.CODDOCUMENTO } // Tipo Auto
                                             join indiceDocumentoEmpresa in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 861) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoEmpresa.CODTRAMITE, codDocumento = (int)indiceDocumentoEmpresa.CODDOCUMENTO } into indiceDocumentoEmpresaLJ // Empresa
                                             from indiceDocumentoEmpresa in indiceDocumentoEmpresaLJ.DefaultIfEmpty()
                                             join serie in dbSIM.TBSERIE on documento.CODSERIE equals serie.CODSERIE
                                             where documento.CODSERIE == 17
                                             select new
                                             {
                                                 documento.CODSERIE,
                                                 ID_POPUP = documento.CODTRAMITE.ToString() + "," + documento.CODDOCUMENTO.ToString(),
                                                 TIPO_DOCUMENTO = serie.NOMBRE,
                                                 ANO = indiceDocumentoAno.VALOR,
                                                 NUMERO = indiceDocumentoAuto.VALOR,
                                                 ASUNTO = indiceDocumentoSolicitud.VALOR,
                                                 EMPRESA = indiceDocumentoEmpresa.VALOR,
                                                 CM = indiceDocumentoCM.VALOR,
                                                 NOMBRE_POPUP = indiceDocumentoAno.VALOR + "-" + indiceDocumentoAuto.VALOR + " [" + indiceDocumentoSolicitud.VALOR + "]",
                                                 TIPO = indiceTipoAuto.VALOR
                                             }
                                             );
                                modelData = model;
                            }
                            catch (Exception error)
                            {
                                modelData = null;
                            }
                        }
                        break;
                    default: // lookup o reduced
                        {
                            modelData = null;
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

        [HttpGet, ActionName("Formulario")]
        public datosConsulta GetFormulario()
        {
            var model = from formulario in dbSIM.FORMULARIO
                        where formulario.TBL_ITEM != null && formulario.TBL_ITEM.Trim().ToUpper() != "NO APLICA"
                        orderby formulario.S_NOMBRE
                        select new
                        {
                            formulario.ID_FORMULARIO,
                            formulario.S_NOMBRE
                        };

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }

        [HttpGet, ActionName("ActuacionesItem")]
        public datosConsulta GetActuacionesItem(int idFormulario, int idItem)
        {
            datosConsulta resultado;

            resultado = new datosConsulta();
            var formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();

            if (formulario != null && formulario.TBL_ITEM != null && formulario.TBL_ITEM.Trim().ToUpper() != "NO APLICA")
            {
                var sql = "SELECT ie." + formulario.S_CAMPO_ID_ITEM + "_ESTADO ID_ESTADO, v.ID_VISITA, v.S_ASUNTO ASUNTO, tv.ID_TIPOVISITA, tv.S_NOMBRE TIPO_VISITA, ev.ID_ESTADOVISITA, ev.S_NOMBRE ESTADO " +
                     "FROM CONTROL.VISITAESTADO ve INNER JOIN " +
                     "CONTROL.ESTADOVISITA ev ON ve.ID_ESTADOVISITA = ev.ID_ESTADOVISITA INNER JOIN " +
                     "CONTROL.VISITA v ON ve.ID_VISITA = v.ID_VISITA INNER JOIN " +
                     "CONTROL.TIPO_VISITA tv ON v.ID_TIPOVISITA = tv.ID_TIPOVISITA INNER JOIN " +
                     formulario.TBL_ESTADOS + " ie ON v.ID_VISITA = ie." + formulario.S_CAMPO_ID_VISITA + " " +
                     "WHERE ie." + formulario.S_CAMPO_ID_ITEM + " = " + idItem.ToString() + " AND ve.D_FIN IS NULL " +
                     " ORDER BY v.ID_VISITA DESC";
                var datos = dbSIM.Database.SqlQuery<ActuacionesItem>(sql);

                resultado.numRegistros = datos.Count();
                resultado.datos = datos.ToList();
            }
            else
            {
                resultado.numRegistros = 0;
                resultado.datos = null;
            }

            return resultado;
        }

        [HttpGet, ActionName("TiposActoFormulario")]
        public datosConsulta GetTiposActoFormulario(int idSerie, int idFormulario)
        {
            var model = from tipoActo in dbSIM.TIPO_ACTO
                        join formularioTipoActo in dbSIM.FORMULARIO_TIPOACTO on tipoActo.ID_TIPOACTO equals formularioTipoActo.ID_TIPOACTO
                        where tipoActo.ID_SERIE == idSerie && formularioTipoActo.ID_FORMULARIO == idFormulario
                        orderby tipoActo.S_DESCRIPCION
                        select new
                        {
                            tipoActo.ID_TIPOACTO,
                            tipoActo.S_DESCRIPCION
                        };

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = model.Count();
            resultado.datos = model.ToList();

            return resultado;
        }

        [HttpGet, ActionName("CrearItem")]
        public dynamic GetCrearItem(int idTercero, int idInstalacion, int idFormulario, string nombre)
        {
            ObjectParameter idItem = new ObjectParameter("respidItem", typeof(decimal));
            ObjectParameter msg = new ObjectParameter("respMsg", typeof(string));

            try
            {
                dbControl.SP_CREATE_ITEM((decimal)idTercero, (decimal)idInstalacion, (decimal)idFormulario, nombre, idItem, msg);
                if (Convert.ToInt32(idItem.Value) < 0)
                    return new { resp = "Error", mensaje = msg.Value.ToString(), id = -1 };
                else
                    return new { resp = "OK", mensaje = "", id = Convert.ToInt32(idItem.Value) };
            }
            catch (Exception error)
            {
                return new { resp = "Error", mensaje = error.Message, id = -1 };
            }
        }

        [HttpGet, ActionName("CrearActuacion")]
        public dynamic GetCrearActuacion(int idTercero, int idInstalacion, string asunto, int idFormulario, int idItem)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idUsuario;
            var formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();

            if (formulario != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                // Se crea la Actuacion
                VISITA actuacion = new VISITA();
                actuacion.S_ASUNTO = asunto;
                actuacion.S_OBSERVACION = asunto;
                actuacion.ID_TIPOVISITA = 6;
                actuacion.ID_USUARIO = idUsuario;

                dbSIM.Entry(actuacion).State = System.Data.Entity.EntityState.Added;
                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Creando Actuación. " + error.Message, id = -1 };
                }

                // Se crea el Estado de la Actuacion
                VISITAESTADO estadoActuacion = new VISITAESTADO();
                estadoActuacion.ID_VISITA = actuacion.ID_VISITA;
                estadoActuacion.ID_TERCERO = idTercero;
                estadoActuacion.ID_ESTADOVISITA = 3;
                estadoActuacion.D_INICIO = DateTime.Today;
                estadoActuacion.D_FIN = null;

                dbSIM.Entry(estadoActuacion).State = System.Data.Entity.EntityState.Added;
                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Registrando el Estado de la Actuación. " + error.Message, id = -1 };
                }

                INSTALACION_VISITA instalacionVisita = new INSTALACION_VISITA();
                instalacionVisita.ID_VISITA = actuacion.ID_VISITA;
                instalacionVisita.ID_TERCERO = idTercero;
                instalacionVisita.ID_INSTALACION = idInstalacion;

                dbSIM.Entry(instalacionVisita).State = System.Data.Entity.EntityState.Added;
                try
                {
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Insertando la Relación Instalación-Actuación. " + error.Message, id = -1 };
                }

                try
                {
                    ObjectParameter idItemEstado = new ObjectParameter("respidItemEstado", typeof(decimal));
                    ObjectParameter msg = new ObjectParameter("respMsg", typeof(string));

                    dbControl.SP_CREATE_ITEM_ESTADO((decimal)idFormulario, (decimal)actuacion.ID_VISITA, (decimal)idItem, idItemEstado, msg);
                    if (Convert.ToInt32(idItemEstado.Value) < 0)
                        return new { resp = "Error", mensaje = msg.Value.ToString(), id = -1 };
                    else
                        return new { resp = "OK", mensaje = "", id = Convert.ToInt32(idItemEstado.Value) };
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Creando Item. " + error.Message, id = -1 };
                }
            }
            else
            {
                return new { resp = "Error", mensaje = "Formulario Inválido", id = -1 };
            }
        }



        [HttpGet, ActionName("Items")]
        public datosConsulta GetItems(int idTercero, int idInstalacion, int idFormulario)
        {
            datosConsulta resultado;

            resultado = new datosConsulta();
            var formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();

            if (formulario != null && formulario.TBL_ITEM != null && formulario.TBL_ITEM.Trim().ToUpper() != "NO APLICA")
            {
                /*var datos = dbSIM.Database.SqlQuery<ItemsFormulario>("SELECT i." + formulario.S_CAMPO_ID_ITEM.Trim() + " ID, i." + formulario.S_CAMPO_NOMBRE.Trim() + " NOMBRE, e." + formulario.S_CAMPO_ID_VISITA.Trim() + " ID_ACTUACION, e." + formulario.S_CAMPO_ID_ITEM.Trim() + "_ESTADO ID_ESTADO " +
                    "FROM CONTROL.FORMULARIO_ITEM fi INNER JOIN " +
                    formulario.TBL_ITEM + " i ON fi.ID_ITEM = i." + formulario.S_CAMPO_ID_ITEM.Trim() + " LEFT OUTER JOIN " +
                    formulario.TBL_ESTADOS + " e ON i." + formulario.S_CAMPO_ID_ITEM.Trim() + " = e." + formulario.S_CAMPO_ID_ITEM.Trim() + " LEFT OUTER JOIN " +
                    "CONTROL.VISITA v ON e." + formulario.S_CAMPO_ID_VISITA.Trim() + " = v.ID_VISITA " +
                    "WHERE fi.ID_FORMULARIO = " + idFormulario.ToString() + " AND fi.ID_TERCERO = " + idTercero.ToString() + " AND fi.ID_INSTALACION = " + idInstalacion.ToString() + " AND (v.ID_TIPOVISITA = 6 OR v.ID_TIPOVISITA IS NULL)");*/

                var sql = "SELECT i." + formulario.S_CAMPO_ID_ITEM.Trim() + " ID, i." + formulario.S_CAMPO_NOMBRE.Trim() + " NOMBRE " +
                    "FROM CONTROL.FORMULARIO_ITEM fi INNER JOIN " +
                    formulario.TBL_ITEM + " i ON fi.ID_ITEM = i." + formulario.S_CAMPO_ID_ITEM.Trim() + " " +
                    "WHERE fi.ID_FORMULARIO = " + idFormulario.ToString() + " AND fi.ID_TERCERO = " + idTercero.ToString() + " AND fi.ID_INSTALACION = " + idInstalacion.ToString() + " ORDER BY i." + formulario.S_CAMPO_NOMBRE.Trim();

                var datos = dbSIM.Database.SqlQuery<ItemsFormulario>(sql);

                resultado.numRegistros = datos.Count();
                resultado.datos = datos.ToList();
            }
            else
            {
                resultado.numRegistros = 0;
                resultado.datos = null;
            }

            return resultado;
        }

        // tipoConsulta: 1 Código, 2 SP
        [HttpGet, ActionName("ConsultarDatosFormulario")]
        public DatosCaracteristicas GetConsultarDatosFormulario(int idEstado, int idFormulario, int idActuacion, int? tipoConsulta)
        {
            DatosCaracteristicas resultado = new DatosCaracteristicas();

            if (tipoConsulta == null)
                tipoConsulta = 1;

            try
            {
                var actuacionDocumento = dbSIM.ACTUACION_DOCUMENTO.Where(ad => ad.ID_ACTUACION == idActuacion).FirstOrDefault();

                if (actuacionDocumento == null)
                {
                    resultado.idTramite = 0;
                    resultado.idDocumento = 0;
                    resultado.idTipoActo = 0;
                }
                else
                {
                    var datosDocumento = ((from documento in dbSIM.TBTRAMITEDOCUMENTO
                                           join indiceDocumentoAno in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 163) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAno.CODTRAMITE, codDocumento = (int)indiceDocumentoAno.CODDOCUMENTO } // Año
                                           join indiceDocumentoRes in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 43) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoRes.CODTRAMITE, codDocumento = (int)indiceDocumentoRes.CODDOCUMENTO } // Resolución
                                           join indiceDocumentoAsunto in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 45) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAsunto.CODTRAMITE, codDocumento = (int)indiceDocumentoAsunto.CODDOCUMENTO } // Asunto
                                           where documento.CODTRAMITE == actuacionDocumento.CODTRAMITE && documento.CODDOCUMENTO == actuacionDocumento.CODDOCUMENTO && documento.CODSERIE == 11
                                           select new
                                           {
                                               ID_POPUP = documento.CODTRAMITE.ToString() + "," + documento.CODDOCUMENTO.ToString(),
                                               NOMBRE_POPUP = indiceDocumentoAno.VALOR + "-" + indiceDocumentoRes.VALOR + " [" + indiceDocumentoAsunto.VALOR + "]",
                                               documento.CODSERIE
                                           }).Union(
                                            from documento in dbSIM.TBTRAMITEDOCUMENTO
                                            join indiceDocumentoAno in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 162) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAno.CODTRAMITE, codDocumento = (int)indiceDocumentoAno.CODDOCUMENTO } // Año
                                            join indiceDocumentoAuto in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 103) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoAuto.CODTRAMITE, codDocumento = (int)indiceDocumentoAuto.CODDOCUMENTO } // Auto
                                            join indiceDocumentoSolicitud in dbSIM.TBINDICEDOCUMENTO.Where(id => id.CODINDICE == 108) on new { codTramite = (int)documento.CODTRAMITE, codDocumento = (int)documento.CODDOCUMENTO } equals new { codTramite = (int)indiceDocumentoSolicitud.CODTRAMITE, codDocumento = (int)indiceDocumentoSolicitud.CODDOCUMENTO } // Solicitud
                                            where documento.CODTRAMITE == actuacionDocumento.CODTRAMITE && documento.CODDOCUMENTO == actuacionDocumento.CODDOCUMENTO && documento.CODSERIE == 17
                                            select new
                                            {
                                                ID_POPUP = documento.CODTRAMITE.ToString() + "," + documento.CODDOCUMENTO.ToString(),
                                                NOMBRE_POPUP = indiceDocumentoAno.VALOR + "-" + indiceDocumentoAuto.VALOR + " [" + indiceDocumentoSolicitud.VALOR + "]",
                                                documento.CODSERIE
                                            }
                                            )).FirstOrDefault();

                    resultado.idTramite = actuacionDocumento.CODTRAMITE;
                    resultado.idDocumento = actuacionDocumento.CODDOCUMENTO;
                    resultado.idTipoActo = (int)actuacionDocumento.ID_TIPOACTO;

                    if (datosDocumento != null)
                    {
                        resultado.documento = datosDocumento.NOMBRE_POPUP;
                        resultado.idSerie = int.Parse(datosDocumento.CODSERIE.ToString());
                    }
                }
            }
            catch (Exception error)
            {
                resultado.error = "Error Cargando Documento Relacionado. " + error.Message;
            }

            FORMULARIO formulario = null;

            try
            {
                formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == idFormulario).FirstOrDefault();
            }
            catch (Exception error)
            {
                resultado.error = "Error Consultando Formulario. " + error.Message;
            }

            if (formulario != null)
            {
                try
                {
                    if (tipoConsulta == 1)
                    {
                        var datosFormulario = (new FormularioDatos()).ObtenerJsonEstadoActuacion(idEstado, idFormulario, 0, false);
                        resultado.jsonDatos = datosFormulario.json;
                    }
                    else
                    {
                        ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                        dbControl.SP_GET_CARACTERISTICAS(idEstado, formulario.TBL_ESTADOS, idFormulario, 0, 0, jSONOUT);
                        resultado.jsonDatos = jSONOUT.Value.ToString();
                    }

                    List<CARACTERISTICA> caracteristicas = JsonConvert.DeserializeObject<List<CARACTERISTICA>>(resultado.jsonDatos);

                    Estructuras.EliminarNodosSinHojas(caracteristicas);

                    resultado.jsonDatos = JsonConvert.SerializeObject(caracteristicas);

                    return resultado;
                }
                catch (Exception error)
                {
                    resultado.error = "Error Obteniendo las Características. " + error.Message;
                }
            }
            else
            {
                resultado.error = "Formulario no Existe";
            }

            return resultado;
        }

        [HttpPost, ActionName("AlmacenarDatosFormulario")]
        public dynamic PostAlmacenarDatosFormulario(DatosCaracteristicas datosCaracteristicas)
        {
            ACTUACION_DOCUMENTO actuacionDocumento;
            var formulario = dbSIM.FORMULARIO.Where(f => f.ID_FORMULARIO == datosCaracteristicas.idFormulario).FirstOrDefault();

            if (formulario != null)
            {
                try
                {
                    ObjectParameter respuesta = new ObjectParameter("rTA", typeof(string));
                    dbControl.SP_SET_CARACTERISTICAS(datosCaracteristicas.idEstado, formulario.TBL_ESTADOS, datosCaracteristicas.jsonDatos, respuesta);

                    if (respuesta.Value.ToString().ToUpper() != "OK")
                    {
                        return new { resp = "Error", mensaje = "Error Almacenando Características. (" + respuesta.Value.ToString() + ")" };
                    }
                }
                catch (Exception error)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Características. (" + error.Message + ")" };
                }
            }
            else
            {
                return new { resp = "Error", mensaje = "El Formulario NO Existe" };
            }

            actuacionDocumento = dbSIM.ACTUACION_DOCUMENTO.Where(ad => ad.ID_ACTUACION == datosCaracteristicas.idActuacion).FirstOrDefault();

            if (actuacionDocumento == null)
            {
                actuacionDocumento = new ACTUACION_DOCUMENTO();
                actuacionDocumento.ID_ACTUACION = datosCaracteristicas.idActuacion;
                actuacionDocumento.ID_TIPOACTO = datosCaracteristicas.idTipoActo;
                actuacionDocumento.CODTRAMITE = datosCaracteristicas.idTramite;
                actuacionDocumento.CODDOCUMENTO = datosCaracteristicas.idDocumento;

                dbSIM.Entry(actuacionDocumento).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                actuacionDocumento.ID_TIPOACTO = datosCaracteristicas.idTipoActo;
                actuacionDocumento.CODTRAMITE = datosCaracteristicas.idTramite;
                actuacionDocumento.CODDOCUMENTO = datosCaracteristicas.idDocumento;

                dbSIM.Entry(actuacionDocumento).State = System.Data.Entity.EntityState.Modified;
            }

            try
            {
                dbSIM.SaveChanges();
            }
            catch (Exception error)
            {
                return new { resp = "Error", mensaje = "Características Almacenadas. Error Almacenando el Documento Relacionado. (" + error.Message + ")" };
            }

            if (datosCaracteristicas.finalizar)
            {
                VISITAESTADO visitaEstado = dbSIM.VISITAESTADO.Where(ve => ve.ID_VISITA == datosCaracteristicas.idActuacion && ve.D_FIN == null).FirstOrDefault();
                if (visitaEstado == null)
                {
                    return new { resp = "Error", mensaje = "Características Almacenadas. Estado Inválido de la Actuación" };
                }
                else
                {
                    visitaEstado.D_FIN = DateTime.Today;

                    dbSIM.Entry(visitaEstado).State = System.Data.Entity.EntityState.Modified;
                    dbSIM.SaveChanges();

                    visitaEstado = new VISITAESTADO();
                    visitaEstado.ID_TERCERO = datosCaracteristicas.idTercero;
                    visitaEstado.ID_VISITA = datosCaracteristicas.idActuacion;
                    visitaEstado.ID_ESTADOVISITA = 5;
                    visitaEstado.D_INICIO = DateTime.Today;

                    dbSIM.Entry(visitaEstado).State = System.Data.Entity.EntityState.Added;
                    dbSIM.SaveChanges();
                }
            }

            return new { resp = "OK", mensaje = "Características Almacenadas Satisfactoriamente" };
        }
    }
}
