using SIM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace SIM.Areas.General.Controllers
{
    /// <summary>
    /// Controlador RadicadorApi: Operaciones para Generar Radicados e imprimir etiquetas. También suministra los datos de serie, subserie, unidad documental y el documento asociado al radicado.
    /// </summary>
    public partial class RadicadorApiController : ApiController
    {
        EntitiesSIMOracle dbSIMTramites = new EntitiesSIMOracle();

        [HttpGet, ActionName("ExpedienteAmbiental")]
        public datosConsulta GetExpedienteAmbiental(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                /*SELECT TO_NUMBER(TO_CHAR(TP.CODIGO_PROYECTO) || TO_CHAR(TS.CODIGO_TIPO_SOLICITUD)) ID_I, 
                    TP.CM S1, TS.CODIGO_TIPO_SOLICITUD I1, 
                    TS.NUMERO S2, 
                    TP.NOMBRE S3, 
                    TTS.NOMBRE AS S4, 
                    TS.NOMBRE S5 
                FROM Tramites.TBPROYECTO TP INNER JOIN 
                    Tramites.TBSOLICITUD TS ON TP.CODIGO_PROYECTO = TS.CODIGO_PROYECTO INNER JOIN 
                    Tramites.TBTIPO_SOLICITUD TTS ON TS.CODIGO_TIPO_SOLICITUD = TTS.CODIGO_TIPO_SOLICITUD
                */
                switch (tipoData)
                {
                    case "f": // full
                        {
                            /*var model = (from proyecto in dbSIMTramites.TBPROYECTO
                                         join solicitud in dbSIMTramites.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                                         join tipoSolicitud in dbSIMTramites.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                                         join municipio in dbSIMTramites.TBMUNICIPIO on proyecto.CODIGO_MUNICIPIO equals municipio.CODIGO_MUNICIPIO
                                         select new
                                         {
                                             ID_POPUP = solicitud.CODIGO_SOLICITUD,
                                             proyecto.CM,
                                             NOMBRE_POPUP = proyecto.NOMBRE,
                                             tipoSolicitud.CODIGO_TIPO_SOLICITUD,
                                             TIPOSOLICITUD = tipoSolicitud.NOMBRE,
                                             solicitud.NUMERO,
                                             TRAMO = solicitud.NOMBRE,
                                             CONEXO = solicitud.CONEXO,
                                             FECHA_CM = solicitud.FECHA_ARCHIVO,
                                             MUNICIPIO = municipio.NOMBRE
                                         });*/

                            var model = (from proyecto in dbSIMTramites.TEXPAMB_EXPEDIENTEAMBIENTAL
                                         join solicitud in dbSIMTramites.TEXPAMB_PUNTOCONTROL on proyecto.ID equals solicitud.EXPEDIENTEAMBIENTAL_ID
                                         join tipoSolicitud in dbSIMTramites.DEXPAMB_TIPOSOLICITUDAMBIENTAL on solicitud.TIPOSOLICITUDAMBIENTAL_ID equals tipoSolicitud.ID_MIGRACION
                                         join municipio in dbSIMTramites.TBMUNICIPIO on proyecto.MUNICIPIO_ID equals municipio.CODIGO_MUNICIPIO
                                         select new
                                         {
                                             ID_POPUP = solicitud.ID,
                                             CM = proyecto.S_CM,
                                             NOMBRE_POPUP = proyecto.S_NOMBRE,
                                             CODIGO_TIPO_SOLICITUD = tipoSolicitud.ID,
                                             TIPOSOLICITUD = tipoSolicitud.S_NOMBRE,
                                             //solicitud.NUMERO,
                                             TRAMO = solicitud.S_NOMBRE,
                                             CONEXO = solicitud.S_CONEXO,
                                             FECHA_CM = solicitud.D_REGISTRO,
                                             MUNICIPIO = municipio.NOMBRE
                                         });

                            modelData = model;
                        }
                        break;
                    default: // lookup o reduced
                        {
                            /*var model = (from proyecto in dbSIMTramites.TBPROYECTO
                                         join solicitud in dbSIMTramites.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                                         join tipoSolicitud in dbSIMTramites.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                                         select new
                                         {
                                             ID_LOOKUP = solicitud.CODIGO_SOLICITUD,
                                             S_NOMBRE_LOOKUP = tipoSolicitud.NOMBRE + " - " + proyecto.NOMBRE
                                         });*/

                            var model = (from proyecto in dbSIMTramites.TEXPAMB_EXPEDIENTEAMBIENTAL
                                         join solicitud in dbSIMTramites.TEXPAMB_PUNTOCONTROL on proyecto.ID equals solicitud.EXPEDIENTEAMBIENTAL_ID
                                         join tipoSolicitud in dbSIMTramites.DEXPAMB_TIPOSOLICITUDAMBIENTAL on solicitud.TIPOSOLICITUDAMBIENTAL_ID equals tipoSolicitud.ID_MIGRACION
                                         select new
                                         {
                                             ID_LOOKUP = solicitud.ID,
                                             S_NOMBRE_LOOKUP = tipoSolicitud.S_NOMBRE + " - " + proyecto.S_NOMBRE
                                         });

                            modelData = model;
                        }
                        break;
                }

                // Las siguientes instrucciones intentan cargar dinámicamente la consulta basado en un string SQL y al cual se le podrían aplicar filtros que son mostrados en un grid o lookup
                //var modelData2 = dbSIMTramites.Database.SqlQuery<DATOSEXT>("SELECT TO_NUMBER(TO_CHAR(TP.CODIGO_PROYECTO) || TO_CHAR(TS.CODIGO_TIPO_SOLICITUD)) ID_I, TP.CM S1, TS.CODIGO_TIPO_SOLICITUD I1, TS.NUMERO S2, TP.NOMBRE S3, TTS.NOMBRE AS S4, TS.NOMBRE S5 FROM Tramites.TBPROYECTO TP INNER JOIN Tramites.TBSOLICITUD TS ON TP.CODIGO_PROYECTO = TS.CODIGO_PROYECTO INNER JOIN Tramites.TBTIPO_SOLICITUD TTS ON TS.CODIGO_TIPO_SOLICITUD = TTS.CODIGO_TIPO_SOLICITUD", new object[] { });
                //var modelData2 = dbSIMTramites.Database.SqlQuery<DATOSEXT>("SELECT ID_TERCERO ID_I, S_RSOCIAL S3 FROM GENERAL.TERCERO", new object[] { });
                //searchExpr = "S3";
                //sort = "[{\"selector\":\"S3\",\"desc\":false}]";
                //IEnumerable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData2, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        public Dictionary<string, datosDocumentosAsociados> DatosExpedienteAmbiental(int id)
        {
            Dictionary<string, datosDocumentosAsociados> resultado = null;

            /*var model = (from proyecto in dbSIMTramites.TBPROYECTO
                         join solicitud in dbSIMTramites.TBSOLICITUD on proyecto.CODIGO_PROYECTO equals solicitud.CODIGO_PROYECTO
                         join tipoSolicitud in dbSIMTramites.TBTIPO_SOLICITUD on solicitud.CODIGO_TIPO_SOLICITUD equals tipoSolicitud.CODIGO_TIPO_SOLICITUD
                         where solicitud.CODIGO_SOLICITUD == id
                         select new
                         {
                            solicitud.CODIGO_SOLICITUD,
                            solicitud.CODIGO_MUNICIPIO,
                            proyecto.CM,
                            PROYECTO = proyecto.NOMBRE,
                            ASUNTO = solicitud.CODIGO_TIPO_SOLICITUD,
                            TIPOSOLICITUD = tipoSolicitud.NOMBRE,
                            OBJETO = solicitud.NUMERO,
                            TRAMO = solicitud.NOMBRE,
                            CONEXO = solicitud.CONEXO,
                            FECHA_CM = solicitud.FECHA_ARCHIVO,
                         }).FirstOrDefault();*/

            var model = (from proyecto in dbSIMTramites.TEXPAMB_EXPEDIENTEAMBIENTAL
                         join solicitud in dbSIMTramites.TEXPAMB_PUNTOCONTROL on proyecto.ID equals solicitud.EXPEDIENTEAMBIENTAL_ID
                         join tipoSolicitud in dbSIMTramites.DEXPAMB_TIPOSOLICITUDAMBIENTAL on solicitud.TIPOSOLICITUDAMBIENTAL_ID equals tipoSolicitud.ID_MIGRACION
                         where solicitud.ID == id
                         select new
                         {
                             CODIGO_SOLICITUD = solicitud.ID,
                             CODIGO_MUNICIPIO = proyecto.MUNICIPIO_ID,
                             CM = proyecto.S_CM,
                             PROYECTO = proyecto.S_NOMBRE,
                             ASUNTO = solicitud.TIPOSOLICITUDAMBIENTAL_ID,
                             TIPOSOLICITUD = tipoSolicitud.S_NOMBRE,
                             OBJETO = 1,
                             TRAMO = solicitud.S_NOMBRE,
                             CONEXO = solicitud.S_CONEXO,
                             FECHA_CM = solicitud.D_REGISTRO,
                         }).FirstOrDefault();

            if (model != null)
            {
                resultado = new Dictionary<string, datosDocumentosAsociados>();

                resultado.Add("CODIGO_SOLICITUD", new datosDocumentosAsociados() { nombre = "CODIGO_SOLICITUD", valor = model.CODIGO_SOLICITUD.ToString() });
                resultado.Add("CODIGO_MUNICIPIO", new datosDocumentosAsociados() { nombre = "CODIGO_MUNICIPIO", valor = model.CODIGO_MUNICIPIO.ToString() });
                resultado.Add("CM", new datosDocumentosAsociados() { nombre = "CM", valor = model.CM.ToString() });
                resultado.Add("PROYECTO", new datosDocumentosAsociados() { nombre = "PROYECTO", valor = model.PROYECTO.ToString() });
                resultado.Add("ASUNTO", new datosDocumentosAsociados() { nombre = "ASUNTO", valor = model.ASUNTO.ToString() });
                resultado.Add("TIPOSOLICITUD", new datosDocumentosAsociados() { nombre = "TIPOSOLICITUD", valor = model.TIPOSOLICITUD.ToString() });
                resultado.Add("OBJETO", new datosDocumentosAsociados() { nombre = "OBJETO", valor = model.OBJETO == null ? "" : model.OBJETO.ToString() });
                resultado.Add("TRAMO", new datosDocumentosAsociados() { nombre = "TRAMO", valor = model.TRAMO == null ? "" : model.TRAMO });
                resultado.Add("CONEXO", new datosDocumentosAsociados() { nombre = "CONEXO", valor = model.CONEXO == null ? "" : model.CONEXO });
                resultado.Add("FECHA_CM", new datosDocumentosAsociados() { nombre = "FECHA_CM", valor = model.FECHA_CM == null ? "" : ((DateTime)model.FECHA_CM).ToString("yyyy/MM/dd") });
            }

            return resultado;
        }

        [HttpGet, ActionName("ExpedienteQuejas")]
        public datosConsulta GetExpedienteQuejas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                            var model = (from queja in dbSIMTramites.TBQUEJA
                                         select new
                                         {
                                             ID_POPUP = queja.CODIGO_QUEJA,
                                             queja.CODIGO_AFECTACION,
                                             queja.CODIGO_RECURSO,
                                             queja.ANO,
                                             queja.QUEJA,
                                             queja.CODIGO_MUNICIPIO,
                                             NOMBRE_POPUP = queja.ASUNTO
                                         });
                            modelData = model;
                        }
                        break;
                    default: // lookup o reduced
                        {
                            var model = (from queja in dbSIMTramites.TBQUEJA
                                         select new
                                         {
                                             ID_LOOKUP = queja.CODIGO_QUEJA,
                                             S_NOMBRE_LOOKUP = queja.ASUNTO
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

        public Dictionary<string, datosDocumentosAsociados> DatosExpedienteQuejas(int id)
        {
            Dictionary<string, datosDocumentosAsociados> resultado = null;

            var model = (from queja in dbSIMTramites.TBQUEJA
                         where queja.CODIGO_QUEJA == id
                         select new
                         {
                             queja.CODIGO_QUEJA,
                             queja.CODIGO_AFECTACION,
                             queja.CODIGO_RECURSO,
                             queja.QUEJA,
                             queja.ANO,
                             queja.CODIGO_MUNICIPIO,
                             queja.ASUNTO
                         }).FirstOrDefault();

            if (model != null)
            {
                resultado = new Dictionary<string, datosDocumentosAsociados>();

                resultado.Add("CODIGO_QUEJA", new datosDocumentosAsociados() { nombre = "CODIGO_QUEJA", valor = model.CODIGO_QUEJA.ToString() });
                resultado.Add("CODIGO_AFECTACION", new datosDocumentosAsociados() { nombre = "CODIGO_AFECTACION", valor = model.CODIGO_AFECTACION.ToString() });
                resultado.Add("CODIGO_RECURSO", new datosDocumentosAsociados() { nombre = "CODIGO_RECURSO", valor = model.CODIGO_RECURSO.ToString() });
                resultado.Add("QUEJA", new datosDocumentosAsociados() { nombre = "QUEJA", valor = model.QUEJA.ToString() });
                resultado.Add("CODIGO_MUNICIPIO", new datosDocumentosAsociados() { nombre = "CODIGO_MUNICIPIO", valor = model.CODIGO_MUNICIPIO.ToString() });
                resultado.Add("ANO", new datosDocumentosAsociados() { nombre = "ANO", valor = model.ANO.ToString() });
                resultado.Add("ASUNTO", new datosDocumentosAsociados() { nombre = "ASUNTO", valor = model.ASUNTO.ToString() });
            }

            return resultado;
        }

        [HttpGet, ActionName("ExpedienteContractual")]
        public datosConsulta GetExpedienteContractual(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                            var model = (from contrato in dbSIM.CONTRATO
                                         join contratoTercero in dbSIM.CONTRATO_TERCERO on contrato.ID_CONTRATO equals contratoTercero.ID_CONTRATO
                                         join tercero in dbSIM.TERCERO on contratoTercero.ID_TERCERO equals tercero.ID_TERCERO
                                         join asociacion in dbSIM.TIPO_ASOCIACION on contrato.ID_TIPOASOCIACION equals asociacion.ID_TIPOASOCIACION
                                         join conveniomarco in dbSIM.CONTRATO on contrato.ID_CONVENIO equals conveniomarco.ID_CONTRATO into contratoConvenio
                                         from conveniomarco in contratoConvenio.DefaultIfEmpty()
                                         where contratoTercero.ID_TIPOTERCEROCONTRATO == 2
                                         select new
                                         {
                                             ID_POPUP = contrato.ID_CONTRATO,
                                             NOMBRE_POPUP = contrato.S_OBJETO,
                                             ANO = contrato.N_ANO,
                                             NUMCONTRATO = contrato.N_CONTRATO,
                                             contrato.ID_TIPOCONTRATO,
                                             ASOCIACION = asociacion.S_NOMBRE + " " + contrato.N_ANO.ToString() + "-" + contrato.N_CONTRATO.ToString(),
                                             TIPO_CONTRATO = contrato.TIPO_CONTRATO.S_NOMBRE,
                                             CONTRATISTA = tercero.S_RSOCIAL,
                                             CONVENIO = conveniomarco == null ? "" : conveniomarco.N_ANO.ToString() + "-" + conveniomarco.N_CONTRATO.ToString(),
                                         });
                            modelData = model;
                        }
                        break;
                    default: // lookup o reduced
                        {
                            var model = (from contrato in dbSIM.CONTRATO
                                         select new
                                         {
                                             ID_POPUP = contrato.ID_CONTRATO,
                                             NOMBRE_POPUP = contrato.S_OBJETO,
                                             ANO = contrato.N_ANO,
                                             NUMCONTRATO = contrato.N_CONTRATO,
                                             TIPO_CONTRATO = contrato.TIPO_CONTRATO.S_NOMBRE
                                         });
                            modelData = model;
                        }
                        break;
                }

                // Las siguientes instrucciones intentan cargar dinámicamente la consulta basado en un string SQL y al cual se le podrían aplicar filtros que son mostrados en un grid o lookup
                //var modelData2 = dbSIMTramites.Database.SqlQuery<DATOSEXT>("SELECT TO_NUMBER(TO_CHAR(TP.CODIGO_PROYECTO) || TO_CHAR(TS.CODIGO_TIPO_SOLICITUD)) ID_I, TP.CM S1, TS.CODIGO_TIPO_SOLICITUD I1, TS.NUMERO S2, TP.NOMBRE S3, TTS.NOMBRE AS S4, TS.NOMBRE S5 FROM Tramites.TBPROYECTO TP INNER JOIN Tramites.TBSOLICITUD TS ON TP.CODIGO_PROYECTO = TS.CODIGO_PROYECTO INNER JOIN Tramites.TBTIPO_SOLICITUD TTS ON TS.CODIGO_TIPO_SOLICITUD = TTS.CODIGO_TIPO_SOLICITUD", new object[] { });
                //var modelData2 = dbSIMTramites.Database.SqlQuery<DATOSEXT>("SELECT ID_TERCERO ID_I, S_RSOCIAL S3 FROM GENERAL.TERCERO", new object[] { });
                //searchExpr = "S3";
                //sort = "[{\"selector\":\"S3\",\"desc\":false}]";
                //IEnumerable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData2, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        public Dictionary<string, datosDocumentosAsociados> DatosExpedienteContractual(int id)
        {
            Dictionary<string, datosDocumentosAsociados> resultado = null;

            var model = (from contrato in dbSIM.CONTRATO
                         join contratoTercero in dbSIM.CONTRATO_TERCERO on contrato.ID_CONTRATO equals contratoTercero.ID_CONTRATO
                         join tercero in dbSIM.TERCERO on contratoTercero.ID_TERCERO equals tercero.ID_TERCERO
                         join asociacion in dbSIM.TIPO_ASOCIACION on contrato.ID_TIPOASOCIACION equals asociacion.ID_TIPOASOCIACION
                         join conveniomarco in dbSIM.CONTRATO on contrato.ID_CONVENIO equals conveniomarco.ID_CONTRATO into contratoConvenio
                         from conveniomarco in contratoConvenio.DefaultIfEmpty()
                         where contratoTercero.ID_TIPOTERCEROCONTRATO == 2 && contrato.ID_CONTRATO == id
                         select new
                         {
                             contrato.ID_CONTRATO,
                             OBJETO = contrato.S_OBJETO,
                             ANO = contrato.N_ANO,
                             NUMCONTRATO = contrato.N_CONTRATO,
                             contrato.ID_TIPOCONTRATO,
                             ASOCIACION = asociacion.S_NOMBRE + " " + contrato.N_ANO.ToString() + "-" + contrato.N_CONTRATO.ToString(),
                             TIPO_CONTRATO = contrato.TIPO_CONTRATO.S_NOMBRE,
                             CONTRATISTA = tercero.S_RSOCIAL,
                             CONVENIO = conveniomarco == null ? "" : conveniomarco.N_ANO.ToString() + "-" + conveniomarco.N_CONTRATO.ToString(),
                             TEXTO_ASOCIACION = asociacion.S_NOMBRE + " " + contrato.N_ANO.ToString() + "-" + contrato.N_CONTRATO.ToString() + (conveniomarco == null ? "" : " / CMARCO " + conveniomarco.N_ANO.ToString() + "-" + conveniomarco.N_CONTRATO.ToString()),
                         }).FirstOrDefault();

            if (model != null)
            {
                resultado = new Dictionary<string, datosDocumentosAsociados>();

                resultado.Add("ID_CONTRATO", new datosDocumentosAsociados() { nombre = "ID_CONTRATO", valor = model.ID_CONTRATO.ToString() });
                resultado.Add("OBJETO", new datosDocumentosAsociados() { nombre = "OBJETO", valor = model.OBJETO.ToString() });
                resultado.Add("ANO", new datosDocumentosAsociados() { nombre = "ANO", valor = model.ANO.ToString() });
                resultado.Add("NUMCONTRATO", new datosDocumentosAsociados() { nombre = "NUMCONTRATO", valor = model.NUMCONTRATO.ToString() });
                resultado.Add("ID_TIPOCONTRATO", new datosDocumentosAsociados() { nombre = "ID_TIPOCONTRATO", valor = model.ID_TIPOCONTRATO.ToString() });
                resultado.Add("ASOCIACION", new datosDocumentosAsociados() { nombre = "ASOCIACION", valor = model.ASOCIACION.ToString() });
                resultado.Add("TIPO_CONTRATO", new datosDocumentosAsociados() { nombre = "TIPO_CONTRATO", valor = model.TIPO_CONTRATO.ToString() });
                resultado.Add("CONTRATISTA", new datosDocumentosAsociados() { nombre = "CONTRATISTA", valor = model.CONTRATISTA.ToString() });
                resultado.Add("CONVENIO", new datosDocumentosAsociados() { nombre = "CONVENIO", valor = model.CONVENIO == null ? "" : model.CONVENIO.ToString() });
                resultado.Add("TEXTO_ASOCIACION", new datosDocumentosAsociados() { nombre = "TEXTO_ASOCIACION", valor = model.TEXTO_ASOCIACION.ToString() });
            }

            return resultado;
        }
    }
}