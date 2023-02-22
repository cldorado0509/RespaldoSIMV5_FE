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

    [Route("api/[controller]", Name = "DerechoPeticionApi")]
    public class DerechoPeticionApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Retorna el Listado de los Derechos de Petición Registrados
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
        [HttpGet, ActionName("GetDerechosPeticion")]
        public datosConsulta GetDerechosPeticion(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TBDERECHOS_PETICION.OrderBy(f => f.CM);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
            else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
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
                    asuntos = ObtenerCOR(proyecto.CM);
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

        [HttpGet, ActionName("ObtenerDocumentos")]
        public JObject ObtenerDocumentos(string radicado, string anio)
        {
            List<Asunto> actosAsunto = new List<Asunto>();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            Asunto asunto = new Asunto();
            asunto.Id = 0;
            try
            {
                var documentos = this.dbSIM.TBINDICEDOCUMENTO
                    .Where(f => f.CODINDICE == 38 && (f.VALOR.ToLower().Trim() == radicado.ToLower().Trim())).ToList();

                foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in documentos)
                {
                    asunto = new Asunto();
                    asunto.Id = 0;
                    var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                    bool isOk = false;
                    
                    asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                    asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;

                    foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                    {
                        switch (idoc.CODINDICE)
                        {
                            case 2240:
                                asunto.SubTipoSolicitud = idoc.VALOR;
                                break;
                            case 38:
                                asunto.Radicado = idoc.VALOR;
                                break;
                            case 72:
                                asunto.Anio = idoc.VALOR;
                                if (asunto.Anio.Contains(anio)) isOk = true;
                                break;
                            case 39:
                                asunto.Nombre = idoc.VALOR;
                                asunto.Asunto_ = idoc.VALOR;
                                break;
                            case 47:
                                asunto.Solicitante = idoc.VALOR;
                                break;
                            case 83:
                                if (!string.IsNullOrEmpty(idoc.VALOR))
                                {
                                    var cm = idoc.VALOR.Trim();
                                    var proyecto = this.dbSIM.TBPROYECTO.Where(f => f.CM == cm).FirstOrDefault();
                                    if (proyecto != null)
                                    {
                                        asunto.CM = proyecto.CM;
                                        asunto.Proyecto = proyecto.NOMBRE;
                                    }
                                    else
                                    {
                                        asunto.CM = cm;
                                        asunto.Proyecto = "No encontrando/No asignado!";
                                    }
                                }
                                break;
                        }
                    }
                    asunto.Descripcion = asunto.Nombre;
                    if (isOk)
                    {
                        var doc = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE.Value && f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO).FirstOrDefault();
                        var tramite = this.dbSIM.TBTRAMITE.Where(f => f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE.Value).FirstOrDefault();
                        if (doc != null)
                        {
                            asunto.Id = doc.ID_DOCUMENTO;
                        }
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE != null ? tBINDICEDOCUMENTO.CODTRAMITE.Value : 0;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                        asunto.TipoSolicitud = "Comunicación Oficial Recibida";
                        actosAsunto.Add(asunto);
                    }
                }
                var listado = actosAsunto.OrderByDescending(o => o.Radicado).ThenByDescending(o => o.Anio).ToList();
                return JObject.FromObject(listado.Count > 0?listado.FirstOrDefault(): asunto, Js);
            }
            catch (Exception exp)
            {
                string msg = exp.InnerException.Message;
                return JObject.FromObject(actosAsunto.FirstOrDefault(), Js);
            }
        }

               
        [HttpGet, ActionName("ObtenerDocumentosCOD")]
        public JObject ObtenerDocumentosCOD(string radicado, string anio)
        {
            List<Asunto> actosAsunto = new List<Asunto>();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            Asunto asunto = new Asunto();
            asunto.Id = 0;

            if (radicado == null || anio == null) return JObject.FromObject(asunto, Js);

            try
            {
                var documentos = this.dbSIM.TBINDICEDOCUMENTO
                    .Where(f => f.CODINDICE == 51 && (f.VALOR.ToLower().Trim() == radicado.ToLower().Trim())).ToList();

                foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in documentos)
                {
                    asunto = new Asunto();
                    asunto.Id = 0;
                    var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                    bool isOk = false;

                    asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                    asunto.DocumentoId = tBINDICEDOCUMENTO.CODDOCUMENTO;

                    foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                    {
                        switch (idoc.CODINDICE)
                        {
                            case 110:
                                asunto.SubTipoSolicitud = idoc.VALOR;
                                break;
                            case 51:
                                asunto.Radicado = idoc.VALOR;
                                break;
                            case 240:
                                asunto.Anio = idoc.VALOR;
                                if (asunto.Anio.Contains(anio)) isOk = true;
                                break;
                            case 53:
                                asunto.Nombre = idoc.VALOR;
                                asunto.Asunto_ = idoc.VALOR;
                                break;
                            case 54:
                                asunto.Solicitante = idoc.VALOR;
                                break;
                            case 82:
                                if (!string.IsNullOrEmpty(idoc.VALOR))
                                {
                                    var cm = idoc.VALOR.Trim();
                                    var proyecto = this.dbSIM.TBPROYECTO.Where(f => f.CM == cm).FirstOrDefault();
                                    if (proyecto != null)
                                    {
                                        asunto.CM = proyecto.CM;
                                        asunto.Proyecto = proyecto.NOMBRE;
                                    }
                                }
                                break;
                        }
                    }
                    asunto.Descripcion = asunto.Nombre;
                    if (isOk)
                    {
                        var doc = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE.Value && f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO).FirstOrDefault();
                        var tramite = this.dbSIM.TBTRAMITE.Where(f => f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE.Value).FirstOrDefault();
                        if (doc != null)
                        {
                            asunto.Id = doc.ID_DOCUMENTO;
                        }
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE != null ? tBINDICEDOCUMENTO.CODTRAMITE.Value : 0;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                        asunto.TipoSolicitud = "Comunicación Oficial Recibida";
                        actosAsunto.Add(asunto);
                    }
                }
                var listado = actosAsunto.OrderByDescending(o => o.Radicado).ThenByDescending(o => o.Anio).ToList();
                return JObject.FromObject(listado.Count > 0 ? listado.FirstOrDefault() : asunto, Js);
            }
            catch (Exception exp)
            {
                string msg = exp.InnerException.Message;
                return JObject.FromObject(actosAsunto.FirstOrDefault(), Js);
            }
        }

        public List<Asunto> ObtenerCOR(string CM)
        {
            List<Asunto> actosAsunto = new List<Asunto>();
            try
            {
                if (!string.IsNullOrEmpty(CM))
                {

                    var indDoc = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODINDICE == 83 && f.VALOR.Contains(CM)).ToList();
                    foreach (TBINDICEDOCUMENTO tBINDICEDOCUMENTO in indDoc)
                    {
                        var indDocSel = this.dbSIM.TBINDICEDOCUMENTO.Where(f => f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO && f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE).ToList();
                        Asunto asunto = new Asunto();

                        var doc = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == tBINDICEDOCUMENTO.CODTRAMITE.Value && f.CODDOCUMENTO == tBINDICEDOCUMENTO.CODDOCUMENTO).FirstOrDefault();
                        if (doc != null)
                        {
                            asunto.Id = doc.ID_DOCUMENTO;
                        }
                        asunto.TramiteId = tBINDICEDOCUMENTO.CODTRAMITE.Value;
                        asunto.FechaRegistro = tBINDICEDOCUMENTO.FECHAREGISTRO;
                        asunto.TipoSolicitud = "Comunicación Oficial Recibida";
                        foreach (TBINDICEDOCUMENTO idoc in indDocSel)
                        {
                            switch (idoc.CODINDICE)
                            {
                                case 2240:
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
                            }
                        }
                        asunto.Descripcion = asunto.Nombre;
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
                
        [HttpPost, ActionName("GuardarDerechoPeticion")]
        public object GuardarDerechoPeticion(DerechoPeticion objData)
        {
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _Dp = dbSIM.TBDERECHOS_PETICION.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Dp != null)
                    {
                        _Dp.SOLICITUD = objData.Asunto;
                        _Dp.CODTRAMITE = objData.CodigoTramite;
                        _Dp.CODDOCUMENTO = objData.CodigoDocumento;
                        _Dp.PROYECTO = objData.Proyecto;
                        _Dp.TECNICO = objData.Tecnico;
                        _Dp.APOYO = objData.Apoyo;
                        _Dp.CM = objData.CM;
                        _Dp.RADICADO = objData.Radicado;
                        _Dp.ANIO = objData.Anio;
                        _Dp.RADICADO_SALIDA = objData.radicadoSalida;
                        _Dp.SOLICITANTE = objData.Solicitante;
                        _Dp.CODIGO_SOLICITUD = (int)objData.CodigoSolicitud;
                        _Dp.NOMBRE_PROYECTO = objData.NombreProyecto;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBDERECHOS_PETICION _Dp = new TBDERECHOS_PETICION();
                    _Dp.SOLICITUD = objData.Asunto;
                    _Dp.PROYECTO = objData.Proyecto;
                    _Dp.CODTRAMITE = objData.CodigoTramite;
                    _Dp.CODDOCUMENTO = objData.CodigoDocumento;
                    _Dp.CM = objData.CM;
                    _Dp.RADICADO = objData.Radicado;
                    _Dp.ANIO = objData.Anio;
                    _Dp.TECNICO = objData.Tecnico;
                    _Dp.APOYO = objData.Apoyo;
                    _Dp.CODIGO_SOLICITUD = (int)objData.CodigoSolicitud;
                    _Dp.SOLICITANTE = objData.Solicitante;
                    _Dp.NOMBRE_PROYECTO = objData.NombreProyecto;
                    dbSIM.TBDERECHOS_PETICION.Add(_Dp);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ObtenerDerechoPeticion")]
        public JObject ObtenerDerechoPeticion(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var _Dp = dbSIM.TBDERECHOS_PETICION.Where(f => f.ID == _Id).FirstOrDefault();

                DerechoPeticion dp = new DerechoPeticion();
                dp.Id = _Dp.ID;
                dp.Asunto = _Dp.SOLICITUD;
                dp.Proyecto = _Dp.PROYECTO;
                dp.CodigoDocumento = _Dp.CODDOCUMENTO;
                dp.CodigoSolicitud = _Dp.CODIGO_SOLICITUD;
                dp.CodigoTramite = _Dp.CODTRAMITE;
                dp.Solicitante = _Dp.SOLICITANTE;
                dp.Apoyo = _Dp.APOYO;
                dp.CM = _Dp.CM != null? _Dp.CM: "";
                dp.Radicado = _Dp.RADICADO != null? _Dp.RADICADO: "";
                dp.Tecnico = _Dp.TECNICO;
                dp.Anio = _Dp.ANIO;
                dp.NombreProyecto = _Dp.NOMBRE_PROYECTO;

                dp.Apoyo = _Dp.APOYO != null? _Dp.APOYO : "";
                return JObject.FromObject(dp, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("EliminarDerechoPeticion")]
        public object EliminarDerechoPeticion(DerechoPeticion objData)
        {
            try
            {
                var _Dp = dbSIM.TBDERECHOS_PETICION.Where(f => f.ID == objData.Id).FirstOrDefault();
                dbSIM.TBDERECHOS_PETICION.Remove(_Dp);
                dbSIM.SaveChanges();
                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }
    }
}
