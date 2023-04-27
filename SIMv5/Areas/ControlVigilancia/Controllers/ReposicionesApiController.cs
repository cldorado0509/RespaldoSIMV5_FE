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
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]", Name = "ReposicionesApi")]
    public class ReposicionesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Retorna el Listado de las Reposiciones
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

            model = dbSIM.TBREPOSICION.Where(f => f.ES_TRAMITE_NUEVO != "1").OrderBy(f => f.CODIGO_SOLICITUD);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
            else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        /// <summary>
        /// Retorna el Listado de las Reposiciones
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
        [HttpGet, ActionName("GetReposicionesReporte")]
        public datosConsulta GetReposicionesReporte(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
          
            datosConsulta resultado = new datosConsulta();
            var model = dbSIM.Database.SqlQuery<V_REPOCISIONES>(@"SELECT r.id,r.codigo_solicitud,r.codigo_actoadministrativo,r.tala_autorizado,r.dap_men_10_autorizado,r.volumen_autorizado,r.trasplante_autorizado,r.poda_autorizado,r.conservacion_autorizado,r.reposicion_autorizado,r.tipo_medidaid,r.autorizado,r.observaciones,r.cm,r.asunto,r.coordenadax,r.coordenaday,r.tipo_medidaadicional_id,tma.nombre As TipoMedidaAdicional,r.medida_adicional_asignada,r.proyecto,r.nro_lenios_solicitados,r.valoracion_inventario_forestal,r.valoracion_tala,r.inversion_reposicion_minima,r.inversion_medidas_adicionales,r.inversion_medida_adicional_siembra,r.cantidad_mantenimiento,r.inversion_medida_adicional_mantenimiento,r.cantidad_destoconado,r.inversion_medida_adicional_destoconado,r.cantidad_levantamiento_piso,r.inversion_medida_adicional_levantamiento_piso,r.pago_fondo_verde_metropolitano,r.es_tramite_nuevo,r.reposicion_propuesta,r.reposicion_minima_obligatoria,r.nro_lenios_autorizados,r.tala_solicitada,r.dap_men_10_solicitado,r.trasplante_solicitado,r.poda_solicitado,r.conservacion_solicitado,r.nombre_proyecto,dr.tipo_documento,dr.numero_acto,dr.fecha_acto,EXTRACT(year FROM dr.fecha_acto)anio_acto,dr.tala_ejecutada,dr.id iddetail,dr.dap_men_10_ejecutada,dr.volumen_ejecutado,dr.trasplante_ejecutado,dr.poda_ejecutada,dr.conservacion_ejecutada,dr.reposicion_ejecutada,dr.medida_adicional_ejecutada,dr.fecha_control,dr.id_usuariovisita,dr.observaciones_visita as ObservacionVisita,dr.fecha_visita as  FechaVisita,dr.radicado_visita as RadicadoVisita,dr.fecha_radicado_visita ,proy.direccion,proy.nombre as NombreProyecto,proy.entidad_publica,mun.Nombre as Municipio,r.codigo_tramite,f.Nombres || ' ' || f.Apellidos as Tecnico FROM  control.tbreposicion r LEFT JOIN control.tipo_medidaadicional  tma ON tma.id = r.tipo_medidaadicional_id LEFT JOIN control.tbdetalle_reposicion  dr ON dr.reposicion_id = r.id LEFT JOIN tramites.tbproyecto proy ON r.cm = proy.cm LEFT JOIN tramites.tbmunicipio           mun ON mun.codigo_municipio = proy.codigo_municipio LEFT JOIN tramites.tbfuncionario  f ON f.codfuncionario = dr.id_usuariovisita WHERE r.es_tramite_nuevo != '1'");
            
            resultado.numRegistros = model.Count();
            if (skip == 0 && take == 0) resultado.datos = model.ToList();
            else resultado.datos = model.Skip(skip).Take(take).ToList();

            return resultado;

        }

        /// <summary>
        /// Obtiene una Reposición
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
                reposicion.Autorizado = rep.AUTORIZADO != null ? rep.AUTORIZADO : 0;
                reposicion.CodigoTramite = rep.CODIGO_TRAMITE;
                reposicion.CodigoDocumento = rep.CODIGO_DOCUMENTO.ToString();
                reposicion.CM = rep.CM;
                reposicion.Proyecto = proyecto != null?proyecto.NOMBRE:"";
                reposicion.NombreProyecto = rep.NOMBRE_PROYECTO;
                reposicion.CodigoActoAdministrativo = rep.CODIGO_ACTOADMINISTRATIVO;
                reposicion.CodigoSolicitud = rep.CODIGO_SOLICITUD;
                reposicion.ConservacionAutorizada = rep.CONSERVACION_AUTORIZADO != null ? rep.CONSERVACION_AUTORIZADO : 0;
                reposicion.DAPMen10Autorizada = rep.DAP_MEN_10_AUTORIZADO != null ? rep.DAP_MEN_10_AUTORIZADO : 0;
                reposicion.Observaciones = rep.OBSERVACIONES == null ? "" : rep.OBSERVACIONES;
                reposicion.PodaAutorizada = rep.PODA_AUTORIZADO != null ? rep.PODA_AUTORIZADO : 0;
                reposicion.PodaSolicitada = rep.PODA_SOLICITADO != null ? rep.PODA_SOLICITADO : 0;
                reposicion.ReposicionAutorizada = rep.REPOSICION_AUTORIZADO != null ? rep.REPOSICION_AUTORIZADO : 0;
                reposicion.TalaAutorizada = rep.TALA_AUTORIZADO != null ? rep.TALA_AUTORIZADO : 0;
                if (rep.TIPO_MEDIDAID != null) reposicion.TipoMedidaId = rep.TIPO_MEDIDAID;
                reposicion.TransplanteAutorizado = rep.TRASPLANTE_AUTORIZADO != null ? rep.TRASPLANTE_AUTORIZADO : 0;
                reposicion.VolumenAutorizado = rep.VOLUMEN_AUTORIZADO != null ? rep.VOLUMEN_AUTORIZADO : 0;
                reposicion.CoordenadaX = rep.COORDENADAX;
                reposicion.CoordenadaY = rep.COORDENADAY;
                if (rep.TIPO_MEDIDAADICIONAL_ID != null) reposicion.TipoMedidaId = rep.TIPO_MEDIDAADICIONAL_ID;
                reposicion.NroLeniosSolicitados = rep.NRO_LENIOS_SOLICITADOS != null ? rep.NRO_LENIOS_SOLICITADOS.Value : 0;
                reposicion.ValoracionInventarioForestal = rep.VALORACION_INVENTARIO_FORESTAL != null ? rep.VALORACION_INVENTARIO_FORESTAL.Value : 0;
                reposicion.ValoracionTala = rep.VALORACION_TALA != null ? rep.VALORACION_TALA.Value : 0;
                reposicion.InversionReposicionMinima = rep.INVERSION_REPOSICION_MINIMA != null ? rep.INVERSION_REPOSICION_MINIMA.Value : 0;
                reposicion.InversionMedidasAdicionales = rep.INVERSION_MEDIDAS_ADICIONALES != null ? rep.INVERSION_MEDIDAS_ADICIONALES.Value : 0;
                reposicion.CantidadSiembraAdicional = rep.CANTIDAD_SIEMBRA_ADICIONAL != null ? rep.CANTIDAD_SIEMBRA_ADICIONAL.Value : 0;
                reposicion.InversionMedidaAdicionalSiembra = rep.INVERSION_MEDIDA_ADICIONAL_SIEMBRA != null ? rep.INVERSION_MEDIDA_ADICIONAL_SIEMBRA.Value : 0;
                reposicion.CantidadMantenimiento = rep.CANTIDAD_MANTENIMIENTO != null ? rep.CANTIDAD_MANTENIMIENTO.Value : 0;
                reposicion.InversionMedidaAdicionalMantenimiento = rep.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO != null ? rep.INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO.Value : 0;
                reposicion.CantidadDestoconado = rep.CANTIDAD_DESTOCONADO != null ? rep.CANTIDAD_DESTOCONADO.Value : 0;
                reposicion.InversionMedidaAdicionalDestoconado = rep.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO != null ? rep.INVERSION_MEDIDA_ADICIONAL_DESTOCONADO.Value : 0;
                reposicion.CantidadLevantamientoPiso = rep.CANTIDAD_LEVANTAMIENTO_PISO != null ? rep.CANTIDAD_LEVANTAMIENTO_PISO.Value : 0;
                reposicion.InversionMedidaAdicionalLevantamientoPiso = rep.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO != null ? rep.INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO.Value : 0;
                reposicion.PagoFondoVerdeMetropolitano = rep.PAGO_FONDO_VERDE_METROPOLITANO != null ? rep.PAGO_FONDO_VERDE_METROPOLITANO.Value : 0;
                reposicion.TalaSolicitada = rep.TALA_SOLICITADA != null ? rep.TALA_SOLICITADA.Value : 0;
                reposicion.DAPMen10Solicitada = rep.DAP_MEN_10_SOLICITADO != null ? rep.DAP_MEN_10_SOLICITADO.Value : 0;
                reposicion.NroLeniosAutorizados = rep.NRO_LENIOS_AUTORIZADOS != null ? rep.NRO_LENIOS_AUTORIZADOS.Value : 0;
                reposicion.TransplanteSolicitado = rep.TRASPLANTE_SOLICITADO != null ? rep.TRASPLANTE_SOLICITADO.Value : 0;
                reposicion.ConservacionSolicitada = rep.CONSERVACION_SOLICITADO != null ? rep.CONSERVACION_SOLICITADO.Value : 0;
                reposicion.ReposicionPropuesta = rep.REPOSICION_PROPUESTA != null ? rep.REPOSICION_PROPUESTA.Value : 0;
                reposicion.ReposicionMinimaObligatoria = rep.REPOSICION_MINIMA_OBLIGATORIA != null ? rep.REPOSICION_MINIMA_OBLIGATORIA.Value : 0;

                return JObject.FromObject(reposicion, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

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

        [HttpGet, ActionName("ObtenerActuacionesCM")]
        public JObject ObtenerActuacionesCM(string radicado, string anio, string cm)
        {
            if (string.IsNullOrEmpty(anio)) anio = DateTime.Now.Year.ToString();
            if (string.IsNullOrEmpty(radicado)) radicado = "000000";
            if (string.IsNullOrEmpty(cm)) cm = "000000";
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
                    asuntos = ObtenerActosCM(radicado,anio,cm);
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

                        asunto.Id = tBINDICEDOCUMENTO.ID_INDICE_DOCUMENTO;
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;

                        asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        bool sw = false;
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
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

        [HttpGet, ActionName("ObtenerActos")]
        public datosConsulta ObtenerActos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, string Id)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {


               // List<Asunto> actosAsunto = new List<Asunto>();
                try
                {
                    int _Id = -1;
                    if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);
                    if (_Id > 0)
                    {
                        var rep = this.dbSIM.TBREPOSICION.Where(f => f.ID == _Id).FirstOrDefault();
                        if (rep == null) throw new Exception("No se encuentra el registro");
                        IQueryable<Asunto>  actosAsunto = ObtenerActosCM(rep.CM);

                       // IQueryable<Asunto> modelIque = actosAsunto.AsQueryable();

                       // modelData = model;
                        IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(actosAsunto, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                        resultado.numRegistros = modelFiltered.Count();
                        if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                        else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                        return resultado;
                    }

                    return new datosConsulta { datos= null, numRegistros = 0 };
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
            
        }

        [HttpGet, ActionName("ObtenerListadoActos")]
        public datosConsulta ObtenerListadoActos(string CM)
        {
            datosConsulta resultado = new datosConsulta();
            //List<Asunto> actosAsunto = new List<Asunto>();
            IQueryable<dynamic> actosAsunto = null;
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
        public IQueryable<Asunto> ObtenerActosCM(string CM)
        {
           List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                if (!string.IsNullOrEmpty(CM))
                {

                    var indDoc = this.dbSIM.TBINDICEDOCUMENTO.Where(f => (f.CODINDICE == 98 && f.VALOR.Contains(CM)) || (f.CODINDICE == 101 && f.VALOR.Contains(CM)) || (f.CODINDICE == 83 && f.VALOR.Contains(CM))).ToList();
                    foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDoc)
                    {
                        var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                        Asunto asunto = new Asunto();

                        asunto.Id = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                       
                        asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
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
                                    asunto.TipoSolicitud = "Resoluciones Administrativas";
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
                                    break;
                                case 360:
                                    asunto.Nombre = idoc.VALOR;
                                    break;
                            }
                        }
                        asunto.Descripcion = $"Radicado: {asunto.Radicado} - Año:{asunto.Anio} - {asunto.TipoSolicitud} - {asunto.SubTipoSolicitud} - {asunto.Nombre} key({asunto.TramiteId}-{asunto.DocumentoId})";
                        actosAsunto.Add(asunto);
                    }
                }
                return actosAsunto.OrderByDescending(o => o.FechaRegistro).ThenByDescending(o => o.Anio).AsQueryable();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
      
        [HttpGet, ActionName("ObtenerControles")]
        public datosConsulta ObtenerControles(string Id)
        {
            datosConsulta resultado = new datosConsulta();
            List<DetalleReposicion> controlesList = new List<DetalleReposicion>();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);
                if (_Id > 0)
                {
                    var controles = this.dbSIM.TBDETALLE_REPOSICION.Where(f => f.REPOSICION_ID == _Id).ToList();
                    if (controles != null)
                    {
                        foreach (TBDETALLE_REPOSICION visita in controles)
                        {
                            var anio = new DateTime(visita.ANIO_ACTO, 1, 1);
                            controlesList.Add(new DetalleReposicion { Id= visita.ID, TecnicoId= visita.ID_USUARIOVISITA, FechaRadicadoVisita= visita.FECHA_RADICADO_VISITA, AnioRadicadoVisita= visita.FECHA_RADICADO_VISITA.Value.Year, FechaVisita= visita.FECHA_VISITA, ObservacionVisita= visita.OBSERVACIONES_VISITA, 
                                RadicadoVisita= visita.RADICADO_VISITA, AnioActo = anio, ConservacionEjecutada = visita.CONSERVACION_EJECUTADA, DAPMen10Ejecutada = visita.DAP_MEN_10_EJECUTADA,
                                DocumentoId = visita.DOCUMENTO_ID, TramiteId= visita.CODIGO_TRAMITE, FechaActo= visita.FECHA_ACTO, FechaControl = visita.FECHA_CONTROL, MedidaAdicionalEjecutada = visita.MEDIDA_ADICIONAL_EJECUTADA, NumeroActo = visita.NUMERO_ACTO, 
                                PodaEjecutada = visita .PODA_EJECUTADA, ReposicionEjecutada = visita .REPOSICION_EJECUTADA, ReposicionId = visita .REPOSICION_ID, TalaEjecutada= visita .TALA_EJECUTADA, TransplanteEjecutado= visita .TRASPLANTE_EJECUTADO, 
                                VolumenEjecutado = visita .VOLUMEN_EJECUTADO});
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

        [HttpPost, ActionName("GuardarReposicion")]
        public object GuardarReposicion(Reposicion objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el registro : " + ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage };
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                var jtramiteDoc = ObtenerTramiteDocumento(objData.Asunto);
                var tramiteDoc = jtramiteDoc.ToObject<TramiteDocumento>();
                if (Id > 0)
                {
                    var _Reposicion = dbSIM.TBREPOSICION.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Reposicion != null)
                    {
                        _Reposicion.ASUNTO = objData.Asunto;
                        _Reposicion.AUTORIZADO = objData.Autorizado;
                        _Reposicion.PROYECTO = objData.Proyecto;
                        _Reposicion.CM = objData.CM;
                        if(tramiteDoc.IdTramite > 0) _Reposicion.CODIGO_TRAMITE =tramiteDoc.IdTramite.ToString();
                        if(tramiteDoc.IdDocumento > 0) _Reposicion.CODIGO_DOCUMENTO =tramiteDoc.IdDocumento;
                        _Reposicion.CODIGO_SOLICITUD = (int)objData.CodigoSolicitud;
                        _Reposicion.CONSERVACION_AUTORIZADO = objData.ConservacionAutorizada;
                        _Reposicion.DAP_MEN_10_AUTORIZADO = objData.DAPMen10Autorizada;
                        _Reposicion.OBSERVACIONES = objData.Observaciones;
                        _Reposicion.PODA_AUTORIZADO = objData.PodaAutorizada;
                        _Reposicion.REPOSICION_AUTORIZADO = objData.ReposicionAutorizada;
                        _Reposicion.TALA_AUTORIZADO = objData.TalaAutorizada;
                        _Reposicion.TRASPLANTE_AUTORIZADO = objData.TransplanteAutorizado;
                        _Reposicion.VOLUMEN_AUTORIZADO = objData.VolumenAutorizado;
                        _Reposicion.COORDENADAX = objData.CoordenadaX != null? objData.CoordenadaX.Value :0;
                        _Reposicion.COORDENADAY = objData.CoordenadaY != null? objData.CoordenadaY.Value : 0;
                        _Reposicion.TIPO_MEDIDAADICIONAL_ID = objData.TipoMedidaId;
                        _Reposicion.ES_TRAMITE_NUEVO = "0";
                        _Reposicion.NOMBRE_PROYECTO = objData.NombreProyecto;
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
                    _Reposicion.CONSERVACION_AUTORIZADO = objData.ConservacionAutorizada;
                    _Reposicion.DAP_MEN_10_AUTORIZADO = objData.DAPMen10Autorizada;
                    _Reposicion.CODIGO_TRAMITE = tramiteDoc.IdTramite.ToString();
                    _Reposicion.CODIGO_DOCUMENTO =tramiteDoc.IdDocumento;
                    _Reposicion.OBSERVACIONES = objData.Observaciones;
                    _Reposicion.PODA_AUTORIZADO = objData.PodaAutorizada;
                    _Reposicion.REPOSICION_AUTORIZADO = objData.ReposicionAutorizada;
                    _Reposicion.TALA_AUTORIZADO = objData.TalaAutorizada;
                    _Reposicion.TRASPLANTE_AUTORIZADO = objData.TransplanteAutorizado;
                    _Reposicion.VOLUMEN_AUTORIZADO = objData.VolumenAutorizado;
                    _Reposicion.COORDENADAX = objData.CoordenadaX != null? objData.CoordenadaX.Value:0;
                    _Reposicion.COORDENADAY = objData.CoordenadaY != null ? objData.CoordenadaY.Value : 0;
                    _Reposicion.TIPO_MEDIDAADICIONAL_ID = objData.TipoMedidaId;
                    _Reposicion.ES_TRAMITE_NUEVO = "0";
                    _Reposicion.NOMBRE_PROYECTO = objData.NombreProyecto;
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

        [HttpPost, ActionName("GuardarControl")]
        public object GuardarControl(DetalleReposicion objData)
        {
            if (objData.FechaRadicadoVisita == null) { objData.FechaRadicadoVisita = new DateTime(1900, 01, 01); }
            try
            {
                decimal Id =0;
                Id = objData.Id;

                var datosTramiteIforme = ObtenerDatosIformeTecnico(objData.RadicadoVisita, objData.AnioActo.Year);
                if (Id > 0)
                {
                    var _detalleReposicion = dbSIM.TBDETALLE_REPOSICION.Where(f => f.ID == Id).FirstOrDefault();
                    if (_detalleReposicion != null)
                    {
                        _detalleReposicion.ANIO_ACTO = objData.AnioActo.Year;
                        _detalleReposicion.CONSERVACION_EJECUTADA = objData.ConservacionEjecutada;
                        _detalleReposicion.DAP_MEN_10_EJECUTADA = objData.DAPMen10Ejecutada;
                        _detalleReposicion.DOCUMENTO_ID = objData.DocumentoId;
                        _detalleReposicion.FECHA_ACTO = objData.FechaActo;
                        _detalleReposicion.ANIO_ACTO = objData.FechaActo.Year;
                        _detalleReposicion.FECHA_CONTROL = objData.FechaControl;
                        _detalleReposicion.MEDIDA_ADICIONAL_EJECUTADA = objData.MedidaAdicionalEjecutada;
                        _detalleReposicion.NUMERO_ACTO = objData.NumeroActo;
                        _detalleReposicion.PODA_EJECUTADA = objData.PodaEjecutada;
                        _detalleReposicion.REPOSICION_EJECUTADA = objData.ReposicionEjecutada;
                        _detalleReposicion.REPOSICION_ID = objData.ReposicionId;
                        _detalleReposicion.TALA_EJECUTADA  = objData.TalaEjecutada;
                        _detalleReposicion.TIPO_DOCUMENTO = objData.TipoDocumento.ToString();
                        _detalleReposicion.TRASPLANTE_EJECUTADO = objData.TransplanteEjecutado;
                        _detalleReposicion.VOLUMEN_EJECUTADO = objData.VolumenEjecutado;
                        _detalleReposicion.ID_USUARIOVISITA = objData.TecnicoId;
                        _detalleReposicion.OBSERVACIONES_VISITA = objData.ObservacionVisita;
                        _detalleReposicion.FECHA_VISITA = objData.FechaVisita.Value;
                        _detalleReposicion.RADICADO_VISITA = objData.RadicadoVisita;
                        _detalleReposicion.FECHA_RADICADO_VISITA = objData.FechaRadicadoVisita;
                        _detalleReposicion.CODIGO_TRAMITE = datosTramiteIforme.IdTramite.ToString();
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBDETALLE_REPOSICION _detalleReposicion = new TBDETALLE_REPOSICION();
                    _detalleReposicion.ANIO_ACTO = objData.AnioActo.Year;
                    _detalleReposicion.CONSERVACION_EJECUTADA = objData.ConservacionEjecutada;
                    _detalleReposicion.DAP_MEN_10_EJECUTADA = objData.DAPMen10Ejecutada;
                    _detalleReposicion.DOCUMENTO_ID = objData.DocumentoId;
                    _detalleReposicion.FECHA_ACTO = objData.FechaActo;
                    _detalleReposicion.ANIO_ACTO = objData.FechaActo.Year;
                    _detalleReposicion.FECHA_CONTROL = objData.FechaControl;
                    _detalleReposicion.MEDIDA_ADICIONAL_EJECUTADA = objData.MedidaAdicionalEjecutada;
                    _detalleReposicion.NUMERO_ACTO = objData.NumeroActo;
                    _detalleReposicion.PODA_EJECUTADA = objData.PodaEjecutada;
                    _detalleReposicion.REPOSICION_EJECUTADA = objData.ReposicionEjecutada;
                    _detalleReposicion.REPOSICION_ID = objData.ReposicionId;
                    _detalleReposicion.TALA_EJECUTADA = objData.TalaEjecutada;
                    _detalleReposicion.TIPO_DOCUMENTO = objData.TipoDocumento.ToString();
                    _detalleReposicion.TRASPLANTE_EJECUTADO = objData.TransplanteEjecutado;
                    _detalleReposicion.VOLUMEN_EJECUTADO = objData.VolumenEjecutado;
                    _detalleReposicion.ID_USUARIOVISITA = objData.TecnicoId;
                    _detalleReposicion.OBSERVACIONES_VISITA = objData.ObservacionVisita;
                    _detalleReposicion.FECHA_VISITA = objData.FechaVisita;
                    _detalleReposicion.RADICADO_VISITA = objData.RadicadoVisita;
                    _detalleReposicion.FECHA_RADICADO_VISITA = objData.FechaRadicadoVisita;
                    _detalleReposicion.CODIGO_TRAMITE = datosTramiteIforme.IdTramite.ToString();
                    dbSIM.TBDETALLE_REPOSICION.Add(_detalleReposicion);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ObtenerRegistroControl")]
        public JObject ObtenerRegistroControl(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var dr = this.dbSIM.TBDETALLE_REPOSICION.Where(f => f.ID == _Id).FirstOrDefault();

                if (dr == null) return null;
                int tipoDoc = 0;
                int.TryParse(dr.TIPO_DOCUMENTO, out tipoDoc);

                var AnioActo = new DateTime(dr.ANIO_ACTO, 1, 1);
                DetalleReposicion detalleReposicion = new DetalleReposicion
                {
                   
                    ConservacionEjecutada = dr.CONSERVACION_EJECUTADA,
                    DAPMen10Ejecutada = dr.DAP_MEN_10_EJECUTADA,
                    DocumentoId = dr.DOCUMENTO_ID,
                    FechaActo = dr.FECHA_ACTO,
                    FechaControl = dr.FECHA_CONTROL,
                    MedidaAdicionalEjecutada = dr.MEDIDA_ADICIONAL_EJECUTADA,
                    NumeroActo = dr.NUMERO_ACTO,
                    PodaEjecutada = dr.PODA_EJECUTADA,
                    ReposicionEjecutada = dr.REPOSICION_EJECUTADA,
                    ReposicionId = dr.REPOSICION_ID,
                    TalaEjecutada = dr.TALA_EJECUTADA,
                    TipoDocumento = tipoDoc,
                    TransplanteEjecutado = dr.TRASPLANTE_EJECUTADO,
                    VolumenEjecutado = dr.VOLUMEN_EJECUTADO,
                    TecnicoId = dr.ID_USUARIOVISITA,
                    FechaRadicadoVisita = dr.FECHA_RADICADO_VISITA,
                    RadicadoVisita = dr.RADICADO_VISITA,
                    FechaVisita = dr.FECHA_VISITA,
                    ObservacionVisita = dr.OBSERVACIONES_VISITA,
                    Id = dr.ID,
                };
               

                return JObject.FromObject(detalleReposicion, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

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

        [HttpGet, ActionName("EliminarControl")]
        public object EliminarControl(string id)
        {
            try
            {
                int Id = -1;
                if (id != null && id != "") Id = int.Parse(id);
                if (Id > 0)
                {
                    var reposicion = this.dbSIM.TBDETALLE_REPOSICION.Where(f => f.ID == Id).FirstOrDefault();
                    this.dbSIM.TBDETALLE_REPOSICION.Remove(reposicion);
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
                                 Nombre =  Mod.NOMBRES + " " + Mod.APELLIDOS
                             }).ToList().OrderBy(o => o.Nombre);

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
      
        [HttpGet, ActionName("ObtenerTramiteDocumento")]
        public JObject ObtenerTramiteDocumento(string id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            int posIni = id.IndexOf("key(");
            string[] subvalues = { };

            if (id == null || posIni == -1 || string.IsNullOrEmpty(id)) return JObject.FromObject(new TramiteDocumento { IdDocumento=0, IdTramite =0}, Js);
            string keyDocumento = string.Empty;
            if (posIni > 1)
            {
                keyDocumento = id.Substring(posIni + 4);
                keyDocumento = keyDocumento.Substring(0, keyDocumento.Length - 1);
                subvalues = keyDocumento.Split('-');
            }

            decimal idDoc = 0;
            decimal idTra = 0;
            decimal.TryParse(subvalues[0], out idTra);
            decimal.TryParse(subvalues[1], out idDoc);

            TramiteDocumento tramiteDocumento = new TramiteDocumento
            {
                IdDocumento = idDoc,
                IdTramite = idTra,
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

        [HttpGet, ActionName("ObtenerInformeTecnico")]
        public JObject ObtenerInformeTecnico(string radicado,int anioradicado)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
           
            try
            {
                
                return JObject.FromObject(ObtenerDatosIformeTecnico(radicado, anioradicado), Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public TramiteDocumento ObtenerDatosIformeTecnico(string Radicado, int FechaRadicado)
        {
            

            decimal idDoc = 0;
            decimal idTra = 0;

            TramiteDocumento tramiteDocumento = new TramiteDocumento
            {
                IdDocumento = idDoc,
                IdTramite = idTra,
            };

            try
            {
                var indDoc = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODINDICE == 381 && f.VALOR == Radicado).ToList();

                foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDoc)
                {
                    var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODINDICE == 382 &&  f.VALOR.Contains(FechaRadicado.ToString()) && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE && f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO).FirstOrDefault();
                    if (indDocSel != null)
                    {
                        tramiteDocumento.IdTramite = indDocSel.CODTRAMITE.Value;
                        tramiteDocumento.IdDocumento = indDocSel.CODDOCUMENTO;
                        break;
                    }
                }
                return tramiteDocumento;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


    }
}
