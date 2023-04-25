namespace SIM.Areas.ControlVigilancia.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Data;
    using SIM.Data.Control;
    using SIM.Data.Tramites;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// Apis' Control de Seguimiento a Trámites Nuevos
    /// </summary>
    [Route("api/[controller]", Name = "TramitesNuevosApi")]
    public class TramitesNuevosApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Retorna el Listado de las Series Documentales
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usaurio</param>
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
        [HttpGet, ActionName("GetReposiciones")]
        public datosConsulta GetReposiciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TBREPOSICION.Where(f => f.ES_TRAMITE_NUEVO == "1").OrderBy(f => f.CODIGO_SOLICITUD);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
            else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerReposicion")]
        public JObject ObtenerReposicion(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var rep = this.dbSIM.TBREPOSICION.Where(f => f.ID == _Id).FirstOrDefault();
                var proyecto = this.dbSIM.TBPROYECTO.Where(f => f.CM == rep.CM).FirstOrDefault();

                Reposicion reposicion = new Reposicion();
                reposicion.Id = rep.ID;
                reposicion.Asunto = rep.ASUNTO;
                reposicion.Autorizado = rep.AUTORIZADO!=null?rep.AUTORIZADO:0;
                reposicion.CM = rep.CM;
                reposicion.Proyecto = proyecto.NOMBRE;
                reposicion.NombreProyecto = rep.NOMBRE_PROYECTO;
                reposicion.CodigoActoAdministrativo = rep.CODIGO_ACTOADMINISTRATIVO;
                reposicion.CodigoSolicitud = rep.CODIGO_SOLICITUD;
                reposicion.ConservacionAutorizada = rep.CONSERVACION_AUTORIZADO!=null?rep.CONSERVACION_AUTORIZADO:0;
                reposicion.DAPMen10Autorizada = rep.DAP_MEN_10_AUTORIZADO!=null?rep.DAP_MEN_10_AUTORIZADO:0;
                reposicion.Observaciones = rep.OBSERVACIONES == null ? "" : rep.OBSERVACIONES;
                reposicion.PodaAutorizada = rep.PODA_AUTORIZADO!= null?rep.PODA_AUTORIZADO:0;
                reposicion.PodaSolicitada = rep.PODA_SOLICITADO != null? rep.PODA_SOLICITADO : 0;
                reposicion.ReposicionAutorizada = rep.REPOSICION_AUTORIZADO!=null?rep.REPOSICION_AUTORIZADO:0;
                reposicion.TalaAutorizada = rep.TALA_AUTORIZADO!=null?rep.TALA_AUTORIZADO:0;
                if(rep.TIPO_MEDIDAID!= null) reposicion.TipoMedidaId = rep.TIPO_MEDIDAID;
                reposicion.TransplanteAutorizado = rep.TRASPLANTE_AUTORIZADO!=null?rep.TRASPLANTE_AUTORIZADO:0;
                reposicion.VolumenAutorizado = rep.VOLUMEN_AUTORIZADO != null?rep.VOLUMEN_AUTORIZADO:0;
                reposicion.CoordenadaX = rep.COORDENADAX;
                reposicion.CoordenadaY = rep.COORDENADAY;
                if(rep.TIPO_MEDIDAADICIONAL_ID != null) reposicion.TipoMedidaId = rep.TIPO_MEDIDAADICIONAL_ID;
                reposicion.NroLeniosSolicitados = rep.NRO_LENIOS_SOLICITADOS != null? rep.NRO_LENIOS_SOLICITADOS.Value: 0;
                reposicion.ValoracionInventarioForestal = rep.VALORACION_INVENTARIO_FORESTAL != null? rep.VALORACION_INVENTARIO_FORESTAL.Value:0;
                reposicion.ValoracionTala = rep.VALORACION_TALA != null? rep.VALORACION_TALA.Value: 0;
                reposicion.InversionReposicionMinima = rep.INVERSION_REPOSICION_MINIMA != null?rep.INVERSION_REPOSICION_MINIMA.Value:0;
                reposicion.InversionMedidasAdicionales = rep.INVERSION_MEDIDAS_ADICIONALES != null?rep.INVERSION_MEDIDAS_ADICIONALES.Value:0;
                reposicion.CantidadSiembraAdicional = rep.CANTIDAD_SIEMBRA_ADICIONAL != null?rep.CANTIDAD_SIEMBRA_ADICIONAL.Value: 0;
                reposicion.InversionMedidaAdicionalSiembra = rep.INVERSION_MEDIDA_ADICIONAL_SIEMBRA != null?rep.INVERSION_MEDIDA_ADICIONAL_SIEMBRA.Value:0;
                reposicion.CantidadMantenimiento = rep.CANTIDAD_MANTENIMIENTO != null?rep.CANTIDAD_MANTENIMIENTO.Value:0;
                reposicion.InversionMedidaAdicionalMantenimiento = rep.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO!=null?rep.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO.Value:0;
                reposicion.CantidadDestoconado = rep.CANTIDAD_DESTOCONADO != null?rep.CANTIDAD_DESTOCONADO.Value:0;
                reposicion.InversionMedidaAdicionalDestoconado = rep.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO != null?rep.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO.Value:0;
                reposicion.CantidadLevantamientoPiso = rep.CANTIDAD_LEVANTAMIENTO_PISO != null?rep.CANTIDAD_LEVANTAMIENTO_PISO.Value:0;
                reposicion.InversionMedidaAdicionalLevantamientoPiso = rep.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO!=null?rep.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO.Value:0;
                reposicion.PagoFondoVerdeMetropolitano = rep.PAGO_FONDO_VERDE_METROPOLITANO!=null?rep.PAGO_FONDO_VERDE_METROPOLITANO.Value:0;
                reposicion.TalaSolicitada = rep.TALA_SOLICITADA != null?rep.TALA_SOLICITADA.Value : 0;
                reposicion.DAPMen10Solicitada = rep.DAP_MEN_10_SOLICITADO != null? rep.DAP_MEN_10_SOLICITADO.Value: 0;
                reposicion.NroLeniosAutorizados = rep.NRO_LENIOS_AUTORIZADOS != null?rep.NRO_LENIOS_AUTORIZADOS.Value : 0;
                reposicion.TransplanteSolicitado = rep.TRASPLANTE_SOLICITADO != null?rep.TRASPLANTE_SOLICITADO.Value : 0;
                reposicion.ConservacionSolicitada = rep.CONSERVACION_SOLICITADO != null? rep.CONSERVACION_SOLICITADO.Value: 0;
                reposicion.ReposicionPropuesta = rep.REPOSICION_PROPUESTA != null? rep.REPOSICION_PROPUESTA.Value : 0;
                reposicion.ReposicionMinimaObligatoria = rep.REPOSICION_MINIMA_OBLIGATORIA != null?rep.REPOSICION_MINIMA_OBLIGATORIA.Value:0;
                reposicion.EsTramiteNuevo = rep.ES_TRAMITE_NUEVO;

                return JObject.FromObject(reposicion, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
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
        public datosConsulta GetReposicionesReporte(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {

            datosConsulta resultado = new datosConsulta();
            var model = dbSIM.Database.SqlQuery<V_REPOCISIONES>(@"SELECT r.id,r.codigo_solicitud,r.codigo_actoadministrativo,r.tala_autorizado,r.dap_men_10_autorizado,r.volumen_autorizado,r.trasplante_autorizado,r.poda_autorizado,r.conservacion_autorizado,r.reposicion_autorizado,r.tipo_medidaid,r.autorizado,r.observaciones,r.cm,r.asunto,r.coordenadax,r.coordenaday,r.tipo_medidaadicional_id,tma.nombre As TipoMedidaAdicional,r.medida_adicional_asignada,r.proyecto,r.nro_lenios_solicitados,r.valoracion_inventario_forestal,r.valoracion_tala,r.inversion_reposicion_minima,r.inversion_medidas_adicionales,r.inversion_medida_adicional_siembra,r.cantidad_mantenimiento,r.inversion_medida_adicional_mantenimiento,r.cantidad_destoconado,r.inversion_medida_adicional_destoconado,r.cantidad_levantamiento_piso,r.inversion_medida_adicional_levantamiento_piso,r.pago_fondo_verde_metropolitano,r.es_tramite_nuevo,r.reposicion_propuesta,r.reposicion_minima_obligatoria,r.nro_lenios_autorizados,r.tala_solicitada,r.dap_men_10_solicitado,r.trasplante_solicitado,r.poda_solicitado,r.conservacion_solicitado,r.nombre_proyecto,dr.tipo_documento,dr.numero_acto,dr.fecha_acto,EXTRACT(year FROM dr.fecha_acto)anio_acto,dr.tala_ejecutada,dr.id iddetail,dr.dap_men_10_ejecutada,dr.volumen_ejecutado,dr.trasplante_ejecutado,dr.poda_ejecutada,dr.conservacion_ejecutada,dr.reposicion_ejecutada,dr.medida_adicional_ejecutada,dr.fecha_control,dr.id_usuariovisita,dr.observaciones_visita as ObservacionVisita,dr.fecha_visita as  FechaVisita,dr.radicado_visita as RadicadoVisita,dr.fecha_radicado_visita ,proy.direccion,proy.nombre as NombreProyecto,proy.entidad_publica,mun.Nombre as Municipio,r.codigo_tramite,f.Nombres || ' ' || f.Apellidos as Tecnico FROM  control.tbreposicion r LEFT JOIN control.tipo_medidaadicional  tma ON tma.id = r.tipo_medidaadicional_id LEFT JOIN control.tbdetalle_reposicion  dr ON dr.reposicion_id = r.id LEFT JOIN tramites.tbproyecto proy ON r.cm = proy.cm LEFT JOIN tramites.tbmunicipio           mun ON mun.codigo_municipio = proy.codigo_municipio LEFT JOIN tramites.tbfuncionario  f ON f.codfuncionario = dr.id_usuariovisita WHERE r.es_tramite_nuevo = '1'");
         

            resultado.numRegistros = model.Count();
            if (skip == 0 && take == 0) resultado.datos = model.ToList();
            else resultado.datos = model.Skip(skip).Take(take).ToList();

            return resultado;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="anio"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerActuacionesCM")]
        public JObject ObtenerActuacionesCM(string radicado, string anio, string cm)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ProyectoCM proyectoCM = new ProyectoCM
            {
                Id = 0,
                CoordenadaX = 0,
                CoordenadaY = 0,
                CoordenadaZ = 0,
                Nombre = string.Empty,
                Direccion = string.Empty,
                Observacion = string.Empty,
            };

            try
            {
                var proyecto = this.dbSIM.TBPROYECTO.Where(f => f.CM == cm).FirstOrDefault();
                if (proyecto != null)
                {
                    List<Asunto> asuntos = new List<Asunto>();
                    proyectoCM.Nombre = proyecto.NOMBRE;
                    proyectoCM.Direccion = proyecto.DIRECCION;
                    proyectoCM.Observacion = proyecto.OBSERVACION;
                    asuntos = ObtenerActosCM(radicado, anio, cm);
                    proyectoCM.Asuntos = asuntos;
                }


                return JObject.FromObject(proyectoCM, Js);
            }
            catch (Exception exp)
            {
                string msg = exp.InnerException.Message;
                proyectoCM.Id = -1;
                return JObject.FromObject(proyectoCM, Js);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radicado"></param>
        /// <param name="anio"></param>
        /// <param name="cm"></param>
        /// <returns></returns>
        public List<Asunto> ObtenerActosCM(string radicado, string anio, string cm)
        {
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                if (!string.IsNullOrEmpty(cm))
                {

                    var indDoc = this.dbSIM.TBINDICEDOCUMENTO.Where(f => (f.CODINDICE == 43 && f.VALOR.Contains(radicado)) || (f.CODINDICE == 103 && f.VALOR.Contains(radicado)) || (f.CODINDICE == 38 && f.VALOR.Contains(radicado)) ||
                        (f.CODINDICE == 51 && f.VALOR.Contains(radicado)) || (f.CODINDICE == 1580 && f.VALOR.Contains(radicado)) || (f.CODINDICE == 381 && f.VALOR.Contains(radicado))).ToList();

                    foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDoc)
                    {
                        var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();

                        Asunto asunto = new Asunto();

                        asunto.Id = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;

                        asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        bool sw = false;
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                        {
                            if (idoc.VALOR != null)
                            {
                                switch (idoc.CODINDICE)
                                {
                                    case 98:
                                        asunto.TipoSolicitud = "Resolución";
                                        break;
                                    case 101:
                                        asunto.TipoSolicitud = "Auto";
                                        break;
                                    case 82:
                                        asunto.TipoSolicitud = "Comunicación Oficial Recibida";
                                        break;
                                    case 83:
                                        asunto.TipoSolicitud = "Comunicación Oficial Despachada";
                                        break;
                                    case 1562:
                                        asunto.TipoSolicitud = "Resoluciones Administrativa";
                                        break;
                                    case 327:
                                        asunto.TipoSolicitud = "Informe Técnico";
                                        break;
                                    case 99:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 43:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 44:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 45:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                    case 102:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 103:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 105:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 2340:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                    case 106:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 38:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 72:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 39:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                    case 53:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 51:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 240:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 110:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                    case 1620:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 1580:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 1582:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 1561:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                    case 321:
                                        asunto.SubTipoSolicitud = idoc.VALOR;
                                        break;
                                    case 381:
                                        asunto.Radicado = idoc.VALOR;
                                        break;
                                    case 382:
                                        asunto.Anio = idoc.VALOR;
                                        if (idoc.VALOR.Contains(anio)) sw = true;
                                        break;
                                    case 360:
                                        asunto.Nombre = idoc.VALOR;
                                        break;
                                }
                            }
                        }
                        if (sw)
                        {
                            asunto.Descripcion = $"Radicado: {asunto.Radicado} - Año:{asunto.Anio} - {asunto.TipoSolicitud} - {asunto.SubTipoSolicitud} - {asunto.Nombre} key({asunto.TramiteId}-{asunto.DocumentoId})";
                            actosAsunto.Add(asunto);
                        }
                    }
                }
                return actosAsunto.OrderByDescending(o => o.FechaRegistro).ThenByDescending(o => o.Anio).ToList();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerCM")]
        public JObject ObtenerCM(string cm)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ProyectoCM proyectoCM = new ProyectoCM
            {
                Id = 0,
                CoordenadaX = 0,
                CoordenadaY = 0,
                CoordenadaZ = 0,
                Nombre = string.Empty,
                Direccion = string.Empty,
                Observacion = string.Empty,
            };

            try
            {
                var proyecto = this.dbSIM.TBPROYECTO.Where(f => f.CM == cm).FirstOrDefault();
                if (proyecto != null)
                {
                    List<Asunto> asuntos = new List<Asunto>();
                    proyectoCM.Nombre = proyecto.NOMBRE;
                    proyectoCM.Direccion = proyecto.DIRECCION;
                    proyectoCM.Observacion = proyecto.OBSERVACION;
                    //asuntos = ObtenerActosCM(proyecto.CM);
                    proyectoCM.Asuntos = asuntos;
                }


                return JObject.FromObject(proyectoCM, Js);
            }
            catch (Exception exp)
            {
                string msg = exp.InnerException.Message;
                proyectoCM.Id = -1;
                return JObject.FromObject(proyectoCM, Js);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tramite"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerInformeTecnico")]
        public JArray ObtenerInformeTecnico(int tramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
               
                var tramDoc = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == tramite).ToList();

                foreach (TBTRAMITEDOCUMENTO tBTRAMITEDOCUMENTO in tramDoc)
                {
                    var indDocTram = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODTRAMITE == tramite && f.CODDOCUMENTO == tBTRAMITEDOCUMENTO.CODDOCUMENTO && (f.CODINDICE == 321 || f.CODINDICE == 381 || f.CODINDICE == 382 || f.CODINDICE == 360)).Distinct().ToList();

                    Asunto asunto = new Asunto();
                    foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDocTram)
                    {
                        var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                        asunto.Id = tBINDICEDOCUMENTO.ID_INDICE_DOCUMENTO;
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                        asunto.TipoSolicitud = "Informe Técnico";
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                        {

                            switch (idoc.CODINDICE)
                            {
                                case 321:
                                    asunto.SubTipoSolicitud = idoc.VALOR;
                                    break;
                                case 381:
                                    asunto.Radicado = idoc.VALOR;
                                    break;
                                case 382:
                                    asunto.Anio = idoc.VALOR.Trim().Replace("/", "-").Replace(@"\", "-").Replace(" ", "-");
                                    break;
                                case 360:
                                    asunto.Nombre = idoc.VALOR;
                                    break;
                            }
                        }
                   
                    }
                    if (!string.IsNullOrEmpty(asunto.Radicado))
                    {
                        asunto.Descripcion = $"Radicado: {asunto.Radicado} - {asunto.DocumentoId} - Año : {asunto.Anio} - {asunto.TipoSolicitud} - {asunto.SubTipoSolicitud} - {asunto.Nombre} key({asunto.TramiteId}-{asunto.DocumentoId})";
                        actosAsunto.Add(asunto);
                    }
                }
                return JArray.FromObject(actosAsunto, Js);
            }
            catch (Exception exp)
            {
                string msg = exp.InnerException.Message;

                return JArray.FromObject(actosAsunto, Js);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerActos")]
        public datosConsulta ObtenerActos(string Id)
        {
            datosConsulta resultado = new datosConsulta();
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);
                if (_Id > 0)
                {
                    var rep = this.dbSIM.TBREPOSICION.Where(f => f.ID == _Id).FirstOrDefault();
                    if (rep == null) throw new Exception("No se encuentra el registro");
                    actosAsunto = ObtenerActosCM(rep.CM);
                }

                resultado.numRegistros = actosAsunto.Count();
                resultado.datos = actosAsunto;

                return resultado;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CM"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerListadoActos")]
        public datosConsulta ObtenerListadoActos(string CM)
        {
            datosConsulta resultado = new datosConsulta();
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                if (!string.IsNullOrEmpty(CM))
                {
                    actosAsunto = ObtenerActosCM(CM);
                }
                resultado.numRegistros = actosAsunto.Count();
                resultado.datos = actosAsunto;

                return resultado;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CM"></param>
        /// <returns></returns>
        public List<Asunto> ObtenerActosCM(string CM)
        {
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                if (!string.IsNullOrEmpty(CM))
                {

                
                    var indDoc = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODINDICE == 101 && f.VALOR.Contains(CM)).ToList();
                    foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDoc)
                    {

                        var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                        Asunto asunto = new Asunto();
                        asunto.Id = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                        asunto.TipoSolicitud = "Auto";
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                        {
                            switch (idoc.CODINDICE)
                            {
                                case 102:
                                    asunto.SubTipoSolicitud = idoc.VALOR;
                                    break;
                                case 103:
                                    asunto.Radicado = idoc.VALOR;
                                    break;
                                case 105:
                                    asunto.Anio = idoc.VALOR;
                                    break;
                                case 2340:
                                    asunto.Nombre = idoc.VALOR;
                                    break;
                            }
                        }
                        asunto.Descripcion = $"Radicado: {asunto.Radicado} - Año : {asunto.Anio} - {asunto.TipoSolicitud} - {asunto.SubTipoSolicitud} - {asunto.Nombre} key({asunto.TramiteId}-{asunto.DocumentoId})";
                        actosAsunto.Add(asunto);
                    }
                }
                return actosAsunto.OrderByDescending(o => o.FechaRegistro).ThenByDescending(o => o.Anio).ToList();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerControles")]
        public datosConsulta ObtenerControles(string Id)
        {
            datosConsulta resultado = new datosConsulta();
            List<SeguimientoTramiteNuevo> controlesList = new List<SeguimientoTramiteNuevo>();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);
                if (_Id > 0)
                {
                    var controles = this.dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Where(f => f.REPOSICION_ID == _Id).ToList();
                    if (controles != null)
                    {
                        foreach (TBSEGUIMIENTO_TRAMITE_NUEVO seguimiento in controles)
                        {
                            controlesList.Add(new SeguimientoTramiteNuevo { Id = seguimiento.ID, AnioRadicado = seguimiento.ANIO_ACTO,
                                Estado = seguimiento.ESTADO,
                                FechaRadicado = seguimiento.ANIO_ACTO,
                                Radicado = seguimiento.NUMERO_ACTO,
                                Tecnico = seguimiento.TECNICO,
                                TramiteId = seguimiento.TRAMITE_ID,
                                DocumentoId = seguimiento.DOCUMENTO_ID,
                                DescripcionEstado =  seguimiento.ESTADO == "1"? "Aprobado":"Requerimientos",
                                FechaControl = seguimiento.FECHA_SEGUIMIENTO });
                        }
                    }
                }
                resultado.numRegistros = controlesList.Count();
                resultado.datos = controlesList;

                return resultado;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetTiposMedidaAdicional")]
        public JArray GetTiposMedidaAdicional()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.TIPO_MEDIDAADICIONAL
                             orderby Mod.NOMBRE
                             select new
                             {
                                 Id = (int)Mod.ID,
                                 Nombre = Mod.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarReposicion")]
        public object GuardarReposicion(Reposicion objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el registro : " + ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage };
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _Reposicion = dbSIM.TBREPOSICION.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Reposicion != null)
                    {
                        _Reposicion.ASUNTO = objData.Asunto;
                        _Reposicion.PROYECTO = objData.Proyecto;
                        _Reposicion.AUTORIZADO = objData.Autorizado;
                        _Reposicion.CM = objData.CM;
                        _Reposicion.CODIGO_SOLICITUD = objData.CodigoActoAdministrativo;
                        _Reposicion.CONSERVACION_SOLICITADO = objData.ConservacionSolicitada;
                        _Reposicion.CONSERVACION_AUTORIZADO = objData.ConservacionAutorizada;
                        _Reposicion.DAP_MEN_10_SOLICITADO = objData.DAPMen10Solicitada;
                        _Reposicion.DAP_MEN_10_AUTORIZADO = objData.DAPMen10Autorizada;
                        _Reposicion.OBSERVACIONES = objData.Observaciones;
                        _Reposicion.PODA_SOLICITADO = objData.PodaSolicitada;
                        _Reposicion.PODA_AUTORIZADO = objData.PodaAutorizada;
                        _Reposicion.REPOSICION_AUTORIZADO = objData.ReposicionAutorizada;
                        _Reposicion.REPOSICION_PROPUESTA = objData.ReposicionPropuesta;
                        _Reposicion.REPOSICION_MINIMA_OBLIGATORIA = objData.ReposicionMinimaObligatoria;
                        _Reposicion.TALA_SOLICITADA = objData.TalaSolicitada.Value;
                        _Reposicion.TALA_AUTORIZADO = objData.TalaAutorizada;
                        _Reposicion.TRASPLANTE_SOLICITADO = objData.TransplanteSolicitado;
                        _Reposicion.TRASPLANTE_AUTORIZADO = objData.TransplanteAutorizado;
                        _Reposicion.VOLUMEN_AUTORIZADO = objData.VolumenAutorizado;
                        _Reposicion.COORDENADAX = objData.CoordenadaX != null ? objData.CoordenadaX.Value : 0;
                        _Reposicion.COORDENADAY = objData.CoordenadaY != null ? objData.CoordenadaY.Value : 0;
                        _Reposicion.TIPO_MEDIDAADICIONAL_ID = objData.TipoMedidaId;
                        _Reposicion.NRO_LENIOS_SOLICITADOS = objData.NroLeniosSolicitados;
                        _Reposicion.NRO_LENIOS_AUTORIZADOS = objData.NroLeniosAutorizados;
                        _Reposicion.VALORACION_INVENTARIO_FORESTAL = objData.ValoracionInventarioForestal;
                        _Reposicion.VALORACION_TALA = objData.ValoracionTala;
                        _Reposicion.INVERSION_REPOSICION_MINIMA = objData.InversionReposicionMinima;
                        _Reposicion.INVERSION_MEDIDAS_ADICIONALES = objData.InversionMedidasAdicionales;
                        _Reposicion.CANTIDAD_SIEMBRA_ADICIONAL = objData.CantidadSiembraAdicional;
                        _Reposicion.INVERSION_MEDIDA_ADICIONAL_SIEMBRA = objData.InversionMedidaAdicionalSiembra;
                        _Reposicion.CANTIDAD_MANTENIMIENTO = objData.CantidadMantenimiento;
                        _Reposicion.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO = objData.InversionMedidaAdicionalMantenimiento;
                        _Reposicion.CANTIDAD_DESTOCONADO = objData.CantidadDestoconado;
                        _Reposicion.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO = objData.InversionMedidaAdicionalDestoconado;
                        _Reposicion.CANTIDAD_LEVANTAMIENTO_PISO = objData.CantidadLevantamientoPiso;
                        _Reposicion.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO = objData.InversionMedidaAdicionalLevantamientoPiso;
                        _Reposicion.PAGO_FONDO_VERDE_METROPOLITANO = objData.PagoFondoVerdeMetropolitano;
                        _Reposicion.NOMBRE_PROYECTO = objData.NombreProyecto;
                        _Reposicion.ES_TRAMITE_NUEVO = "1";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBREPOSICION _Reposicion = new TBREPOSICION();
                    _Reposicion.ASUNTO = objData.Asunto;
                    _Reposicion.PROYECTO = objData.Proyecto;
                    _Reposicion.AUTORIZADO = objData.Autorizado;
                    _Reposicion.CM = objData.CM;
                    _Reposicion.CODIGO_SOLICITUD = objData.CodigoActoAdministrativo;
                    _Reposicion.CONSERVACION_SOLICITADO = objData.ConservacionSolicitada;
                    _Reposicion.CONSERVACION_AUTORIZADO = objData.ConservacionAutorizada;
                    _Reposicion.DAP_MEN_10_SOLICITADO = objData.DAPMen10Solicitada;
                    _Reposicion.DAP_MEN_10_AUTORIZADO = objData.DAPMen10Autorizada;
                    _Reposicion.OBSERVACIONES = objData.Observaciones;
                    _Reposicion.PODA_SOLICITADO = objData.PodaSolicitada;
                    _Reposicion.PODA_AUTORIZADO = objData.PodaAutorizada;
                    _Reposicion.REPOSICION_AUTORIZADO = objData.ReposicionAutorizada;
                    _Reposicion.REPOSICION_PROPUESTA = objData.ReposicionPropuesta;
                    _Reposicion.REPOSICION_MINIMA_OBLIGATORIA = objData.ReposicionMinimaObligatoria;
                    _Reposicion.TALA_SOLICITADA = objData.TalaSolicitada;
                    _Reposicion.TALA_AUTORIZADO = objData.TalaAutorizada;
                    _Reposicion.TRASPLANTE_SOLICITADO = objData.TransplanteSolicitado;
                    _Reposicion.TRASPLANTE_AUTORIZADO = objData.TransplanteAutorizado;
                    _Reposicion.VOLUMEN_AUTORIZADO = objData.VolumenAutorizado;
                    _Reposicion.COORDENADAX = objData.CoordenadaX != null ? objData.CoordenadaX.Value : 0;
                    _Reposicion.COORDENADAY = objData.CoordenadaY != null ? objData.CoordenadaY.Value : 0;
                    _Reposicion.TIPO_MEDIDAADICIONAL_ID = objData.TipoMedidaId;
                    _Reposicion.NRO_LENIOS_SOLICITADOS = objData.NroLeniosSolicitados;
                    _Reposicion.NRO_LENIOS_AUTORIZADOS = objData.NroLeniosAutorizados;
                    _Reposicion.VALORACION_INVENTARIO_FORESTAL = objData.ValoracionInventarioForestal;
                    _Reposicion.VALORACION_TALA = objData.ValoracionTala;
                    _Reposicion.INVERSION_REPOSICION_MINIMA = objData.InversionReposicionMinima;
                    _Reposicion.INVERSION_MEDIDAS_ADICIONALES = objData.InversionMedidasAdicionales;
                    _Reposicion.CANTIDAD_SIEMBRA_ADICIONAL = objData.CantidadSiembraAdicional;
                    _Reposicion.INVERSION_MEDIDA_ADICIONAL_SIEMBRA = objData.InversionMedidaAdicionalSiembra;
                    _Reposicion.CANTIDAD_MANTENIMIENTO = objData.CantidadMantenimiento;
                    _Reposicion.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO = objData.InversionMedidaAdicionalMantenimiento;
                    _Reposicion.CANTIDAD_DESTOCONADO = objData.CantidadDestoconado;
                    _Reposicion.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO = objData.InversionMedidaAdicionalDestoconado;
                    _Reposicion.CANTIDAD_LEVANTAMIENTO_PISO = objData.CantidadLevantamientoPiso;
                    _Reposicion.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO = objData.InversionMedidaAdicionalLevantamientoPiso;
                    _Reposicion.PAGO_FONDO_VERDE_METROPOLITANO = objData.PagoFondoVerdeMetropolitano;
                    _Reposicion.NOMBRE_PROYECTO = objData.NombreProyecto;
                    _Reposicion.ES_TRAMITE_NUEVO = "1";
                    dbSIM.TBREPOSICION.Add(_Reposicion);

                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarControl")]
        public object GuardarControl(SeguimientoTramiteNuevo objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el registro : " + ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage };
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string[] documentoId = objData.Radicado.Split('-');
                if (documentoId == null || documentoId.Length == 0 ) return new { resp = "Error", mensaje = "Documento no encontrado!" };
                if (Id > 0)
                {
                    var _detalleSeguimiento = dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Where(f => f.ID == Id).FirstOrDefault();
                    if (_detalleSeguimiento != null)
                    {
                        _detalleSeguimiento.REPOSICION_ID = objData.ReposicionId;
                        _detalleSeguimiento.TECNICO = objData.Tecnico;
                        _detalleSeguimiento.ESTADO = objData.Estado;
                        _detalleSeguimiento.NUMERO_ACTO =  documentoId[0].Trim();
                        _detalleSeguimiento.DOCUMENTO_ID = int.Parse(documentoId[1]);
                        _detalleSeguimiento.ANIO_ACTO = objData.AnioRadicado;
                        _detalleSeguimiento.FECHA_SEGUIMIENTO = DateTime.Now;
                        _detalleSeguimiento.TRAMITE_ID = objData.TramiteId;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBSEGUIMIENTO_TRAMITE_NUEVO _detalleSeguimiento = new TBSEGUIMIENTO_TRAMITE_NUEVO();
                    _detalleSeguimiento.REPOSICION_ID = objData.ReposicionId;
                    _detalleSeguimiento.TECNICO = objData.Tecnico;
                    _detalleSeguimiento.ESTADO = objData.Estado;
                    _detalleSeguimiento.NUMERO_ACTO = documentoId[0].Trim();
                    _detalleSeguimiento.DOCUMENTO_ID = int.Parse(documentoId[1]);
                    _detalleSeguimiento.ANIO_ACTO = objData.AnioRadicado;
                    _detalleSeguimiento.FECHA_SEGUIMIENTO = DateTime.Now;
                    _detalleSeguimiento.TRAMITE_ID = objData.TramiteId;
                    dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Add(_detalleSeguimiento);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerRegistroControl")]
        public JObject ObtenerRegistroControl(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var dr = this.dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Where(f => f.ID == _Id).FirstOrDefault();

                if (dr == null) return null;


                SeguimientoTramiteNuevo seguimientoTramiteNuevo = new SeguimientoTramiteNuevo
                {
                    Id = dr.ID,
                    Estado = dr.ESTADO,
                    FechaRadicado = dr.ANIO_ACTO,
                    Radicado = dr.NUMERO_ACTO + "-" + dr.DOCUMENTO_ID.ToString(),
                    Tecnico = dr.TECNICO,
                    TramiteId = dr.TRAMITE_ID,
                    AnioRadicado = dr.ANIO_ACTO,
                };


                return JObject.FromObject(seguimientoTramiteNuevo, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("EliminarReposicion")]
        public object EliminarReposicion(string id)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el registro" };
            try
            {
                int Id = -1;
                if (id != null && id != "") Id = int.Parse(id);
                if (Id > 0)
                {
                    var reposicion = this.dbSIM.TBREPOSICION.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.TBREPOSICION.Remove(reposicion);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error al eliminar el registro: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Reposición eliminada correctamente!!" };
        }

        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetTecnicos")]
        public JArray GetTecnicos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.TBFUNCIONARIO
                             orderby Mod.NOMBRES
                             select new
                             {
                                 Id = (decimal)Mod.CODFUNCIONARIO,
                                 Nombre = Mod.NOMBRES + " " + Mod.APELLIDOS
                             }).ToList().OrderBy(o => o.Nombre);

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetEstados")]
        public JArray GetEstados()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
               List<EstadoSolicitud> listaEstados = new List<EstadoSolicitud>();
               listaEstados.Add(new EstadoSolicitud { Id = 1, Estado = "Aprobado" });
               listaEstados.Add(new EstadoSolicitud { Id = 2, Estado = "Requerimientos" });
               return JArray.FromObject(listaEstados, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTramiteDocumento")]
        public JObject ObtenerTramiteDocumento(string id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            if (id == null || string.IsNullOrEmpty(id)) return JObject.FromObject(new TramiteDocumento { IdDocumento = 0, IdTramite = 0 }, Js);
            int posIni = id.IndexOf("key(");

            string keyDocumento = id.Substring(posIni + 4);
            keyDocumento = keyDocumento.Substring(0, keyDocumento.Length - 1);
            string[] subvalues = keyDocumento.Split('-');


            TramiteDocumento tramiteDocumento = new TramiteDocumento
            {
                IdDocumento = decimal.Parse(subvalues[1]),
                IdTramite = decimal.Parse(subvalues[0]),
            };

            try
            {
                return JObject.FromObject(tramiteDocumento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
          
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("EliminarSeguimiento")]
        public object EliminarSeguimiento(string id)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el registro" };
            try
            {
                int Id = -1;
                if (id != null && id != "") Id = int.Parse(id);
                if (Id > 0)
                {
                    var seguimiento = this.dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.TBSEGUIMIENTO_TRAMITE_NUEVO.Remove(seguimiento);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error al eliminar el registro: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Reposición eliminada correctamente!!" };
        }

    }
}
