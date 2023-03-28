namespace SIM.Controllers
{
    using DevExpress.CodeParser;
    using DevExpress.DataProcessing.InMemoryDataProcessor;
    using DevExpress.Office.Utils;
    using DevExpress.Web;
    using DevExpress.Xpo;
    using DocumentFormat.OpenXml.Drawing.Charts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Data;
    using SIM.Data.Contrato;
    using SIM.Data.General;
    using SIM.Data.Tramites;
    using SIM.Utilidades;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    
    public class UtilidadesApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("BuscarDoc")]
        public datosConsulta GetBuscarDoc(int skip, int take, int IdUnidadDoc, string Buscar)
        {
            datosConsulta resultado = new datosConsulta();
            if (Buscar == "")
            {
                resultado.numRegistros = 0;
                resultado.datos = null;
                return resultado;
            }
            else
            {
                if (Buscar != "" && Buscar != null)
                {
                    string[] _Buscar = Buscar.Split(';');
                    string _Sql = "";
                    if (_Buscar.Length > 0)
                    {
                        switch (_Buscar[0])
                        {
                            case "T":
                                _Sql = "SELECT DISTINCT DOC.ID_DOCUMENTO,DOC.CODTRAMITE,DOC.CODDOCUMENTO,SER.NOMBRE,(SELECT BUS.S_INDICE FROM TRAMITES.BUSQUEDA_DOCUMENTO BUS WHERE BUS.COD_TRAMITE = DOC.CODTRAMITE AND BUS.COD_DOCUMENTO = DOC.CODDOCUMENTO) AS INDICES,DOC.FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO DOC INNER JOIN TRAMITES.TBSERIE SER ON DOC.CODSERIE = SER.CODSERIE WHERE DOC.CODTRAMITE= " + _Buscar[1].ToString().Trim() + " ORDER BY DOC.ID_DOCUMENTO DESC";
                                var TraDocs = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = TraDocs.Skip(skip).Take(take).ToList();
                                else resultado.datos = TraDocs.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                            case "F":
                                var _condicion = _Buscar[1].ToString().Replace("\r\n", string.Empty); //.Replace(" ", String.Empty);
                                _condicion = _condicion.Replace("\\", "").Replace("\"", "");
                                _condicion = _condicion.Replace("[", "").Replace("]", "");
                                if (_condicion != null)
                                {

                                    _condicion = ProcesaWhereOracle(_condicion);
                                    if (_condicion != "")
                                    {
                                        _Sql = ObtenerSqlDocOracle(IdUnidadDoc, _condicion);
                                        var SqlDocs = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                        if (skip > 0 && take > 0) resultado.datos = SqlDocs.Skip(skip).Take(take).ToList();
                                        else resultado.datos = SqlDocs.ToList();
                                        resultado.numRegistros = resultado.datos.Count();
                                    }
                                    else
                                    {
                                        resultado.numRegistros = 0;
                                        resultado.datos = null;
                                    }
                                }
                                break;
                            case "B":
                                _Sql = "SELECT DISTINCT DOC.ID_DOCUMENTO,DOC.CODTRAMITE,DOC.CODDOCUMENTO,SER.NOMBRE,BUS.S_INDICE AS INDICES,DOC.FECHACREACION FROM TRAMITES.BUSQUEDA_DOCUMENTO BUS INNER JOIN TRAMITES.TBTRAMITEDOCUMENTO DOC ON BUS.COD_TRAMITE = DOC.CODTRAMITE AND BUS.COD_DOCUMENTO = DOC.CODDOCUMENTO INNER JOIN TRAMITES.TBSERIE SER ON BUS.COD_SERIE = SER.CODSERIE WHERE CONTAINS(BUS.S_INDICE, '%" + _Buscar[1].ToString().ToUpper().Trim() + "%') > 0 ORDER BY DOC.ID_DOCUMENTO DESC";
                                var BusDocs = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = BusDocs.Skip(skip).Take(take).ToList();
                                else resultado.datos = BusDocs.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                        }

                    }
                }
                else
                {
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    return resultado;
                }

                return resultado;
            }
        }

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="Buscar">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("BuscarExp")]
        public datosConsulta GetBuscarExp(int skip, int take, int IdUnidadDoc, string Buscar)
        {
            datosConsulta resultado = new datosConsulta();
            if (Buscar == "")
            {
                resultado.numRegistros = 0;
                resultado.datos = null;
                return resultado;
            }
            else
            {
                if (Buscar != "" && Buscar != null)
                {
                    string[] _Buscar = Buscar.Split(';');
                    string _Sql = "";
                    if (_Buscar.Length > 0)
                    {
                        switch (_Buscar[0])
                        {
                            case "C":
                                if (_Buscar[1].ToString() != "")
                                {
                                    _Sql = "SELECT DISTINCT EXP.ID_EXPEDIENTE,EXP.S_NOMBRE AS EXPEDIENTE,EXP.S_CODIGO AS CODIGO,SER.NOMBRE,(SELECT BUS.S_INDICE FROM TRAMITES.EXP_BUSQUEDA BUS WHERE BUS.ID_EXPEDIENTE = EXP.ID_EXPEDIENTE) AS INDICES FROM TRAMITES.EXP_EXPEDIENTES EXP INNER JOIN TRAMITES.TBSERIE SER ON EXP.ID_UNIDADDOC = SER.CODSERIE WHERE EXP.S_CODIGO LIKE '%" + _Buscar[1].ToString().Trim() + "%' ORDER BY EXP.ID_EXPEDIENTE DESC";
                                    var TraDocs = dbSIM.Database.SqlQuery<DatosExps>(_Sql);
                                    if (skip > 0 && take > 0) resultado.datos = TraDocs.Skip(skip).Take(take).ToList();
                                    else resultado.datos = TraDocs.ToList();
                                    resultado.numRegistros = resultado.datos.Count();
                                }
                                else
                                {
                                    resultado.numRegistros = 0;
                                    resultado.datos = null;
                                }
                                break;
                            case "F":
                                var _condicion = _Buscar[1].ToString().Replace("\r\n", string.Empty).Replace(" ", String.Empty);
                                //_condicion = _condicion.Replace("\\", "").Replace("\"", "");
                                _condicion = _condicion.Replace("[", "").Replace("]", "");
                                if (_condicion != null)
                                {

                                    _condicion = ProcesaWhereOracle(_condicion);
                                    if (_condicion != "")
                                    {
                                        _Sql = ObtenerSqlExpOracle(IdUnidadDoc, _condicion);
                                        var SqlDocs = dbSIM.Database.SqlQuery<DatosExps>(_Sql);
                                        if (skip > 0 && take > 0) resultado.datos = SqlDocs.Skip(skip).Take(take).ToList();
                                        else resultado.datos = SqlDocs.ToList();
                                        resultado.numRegistros = resultado.datos.Count();
                                    }
                                    else
                                    {
                                        resultado.numRegistros = 0;
                                        resultado.datos = null;
                                    }
                                }
                                break;
                            case "B":
                                if (_Buscar[1].ToString() != "")
                                {
                                    _Sql = "SELECT DISTINCT EXP.ID_EXPEDIENTE,EXP.S_NOMBRE AS EXPEDIENTE,EXP.S_CODIGO AS CODIGO,SER.NOMBRE,BUS.S_INDICE AS INDICES FROM TRAMITES.EXP_BUSQUEDA BUS INNER JOIN TRAMITES.EXP_EXPEDIENTES EXP ON BUS.ID_EXPEDIENTE = EXP.ID_EXPEDIENTE INNER JOIN TRAMITES.TBSERIE SER ON BUS.COD_SERIE = SER.CODSERIE WHERE CONTAINS(BUS.S_INDICE, '%" + _Buscar[1].ToString().ToUpper().Trim() + "%') > 0 ORDER BY EXP.ID_EXPEDIENTE DESC";
                                    var BusDocs = dbSIM.Database.SqlQuery<DatosExps>(_Sql);
                                    if (skip > 0 && take > 0) resultado.datos = BusDocs.Skip(skip).Take(take).ToList();
                                    else resultado.datos = BusDocs.ToList();
                                    resultado.numRegistros = resultado.datos.Count();
                                }
                                else
                                {
                                    resultado.numRegistros = 0;
                                    resultado.datos = null;
                                }
                                break;
                        }

                    }
                }
                else
                {
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    return resultado;
                }

                return resultado;
            }
        }

        /// <summary>
        /// Consulta de Lista de Trámites con filtros y agrupación
        /// </summary>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="Buscar">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("BuscarTramites")]
        public datosConsulta GetBuscarTramite(int skip, int take, int IdTipoProceso, string Buscar)
        {
            datosConsulta resultado = new datosConsulta();
            if (Buscar == "")
            {
                resultado.numRegistros = 0;
                resultado.datos = null;
                return resultado;
            }
            else
            {
                if (Buscar != "" && Buscar != null)
                {
                    string[] _Buscar = Buscar.Split(';');
                    string _Sql = "";
                    if (_Buscar.Length > 0)
                    {
                        switch (_Buscar[0])
                        {
                            case "T":
                                string[] _TraVital = _Buscar[1].ToString().Split(' ');
                                if (_TraVital[0].Length > 0 && _TraVital[1].Length > 0) _Sql = $"SELECT DISTINCT TRA.CODTRAMITE,PRO.NOMBRE AS TIPOPROCESO,TRA.FECHAINI,CASE WHEN TRA.ESTADO = 1 THEN 'TERMINADO' ELSE 'ABIERTO' END AS ESTADO FROM TRAMITES.TBTRAMITE TRA INNER JOIN TRAMITES.TBPROCESO PRO ON TRA.CODPROCESO = PRO.CODPROCESO WHERE TRA.CODTRAMITE = {_TraVital[0].ToString().Trim()} AND TRA.NUMERO_VITAL = '{_TraVital[1].ToString().Trim()}' ORDER BY TRA.CODTRAMITE DESC";
                                else if (_TraVital[0].Length > 0) _Sql = $"SELECT DISTINCT TRA.CODTRAMITE,PRO.NOMBRE AS TIPOPROCESO,TRA.FECHAINI,CASE WHEN TRA.ESTADO = 1 THEN 'TERMINADO' ELSE 'ABIERTO' END AS ESTADO FROM TRAMITES.TBTRAMITE TRA INNER JOIN TRAMITES.TBPROCESO PRO ON TRA.CODPROCESO = PRO.CODPROCESO WHERE TRA.CODTRAMITE = {_TraVital[0].ToString().Trim()} ORDER BY TRA.CODTRAMITE DESC";
                                else if (_TraVital[1].Length > 0) _Sql = $"SELECT DISTINCT TRA.CODTRAMITE,PRO.NOMBRE AS TIPOPROCESO,TRA.FECHAINI,CASE WHEN TRA.ESTADO = 1 THEN 'TERMINADO' ELSE 'ABIERTO' END AS ESTADO FROM TRAMITES.TBTRAMITE TRA INNER JOIN TRAMITES.TBPROCESO PRO ON TRA.CODPROCESO = PRO.CODPROCESO WHERE TRA.NUMERO_VITAL = '{_TraVital[1].ToString().Trim()}' ORDER BY TRA.CODTRAMITE DESC";
                                var TraDocs = dbSIM.Database.SqlQuery<DatosTramites>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = TraDocs.Skip(skip).Take(take).ToList();
                                else resultado.datos = TraDocs.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                            case "F":
                                var _condicion = _Buscar[1].ToString().Replace("\r\n", string.Empty); //.Replace(" ", String.Empty);
                                _condicion = _condicion.Replace("\\", "").Replace("\"", "");
                                _condicion = _condicion.Replace("[", "").Replace("]", "");
                                if (_condicion != null)
                                {

                                    _condicion = ProcesaWhereOracle(_condicion);
                                    if (_condicion != "")
                                    {
                                        _Sql = ObtenerSqlProcesoOracle(IdTipoProceso, _condicion);
                                        var SqlDocs = dbSIM.Database.SqlQuery<DatosTramites>(_Sql);
                                        if (skip > 0 && take > 0) resultado.datos = SqlDocs.Skip(skip).Take(take).ToList();
                                        else resultado.datos = SqlDocs.ToList();
                                        resultado.numRegistros = resultado.datos.Count();
                                    }
                                    else
                                    {
                                        resultado.numRegistros = 0;
                                        resultado.datos = null;
                                    }
                                }
                                break;
                            case "U":
                                _Sql = $"SELECT DISTINCT TRA.CODTRAMITE,PRO.NOMBRE AS TIPOPROCESO,TRA.FECHAINI,CASE WHEN TRA.ESTADO = 1 THEN 'TERMINADO' ELSE 'ABIERTO' END AS ESTADO FROM TRAMITES.TBTRAMITE TRA INNER JOIN TRAMITES.TBPROCESO PRO ON TRA.CODPROCESO = PRO.CODPROCESO INNER JOIN TRAMITES.TBTRAMITETAREA TTA ON TTA.CODTRAMITE=TRA.CODTRAMITE WHERE TTA.COPIA=0 AND TTA.ESTADO=0 AND CODFUNCIONARIO={_Buscar[1].ToString().Trim()} ORDER BY TRA.CODTRAMITE DESC";
                                var BusDocs = dbSIM.Database.SqlQuery<DatosTramites>(_Sql);
                                if (skip > 0 && take > 0) resultado.datos = BusDocs.Skip(skip).Take(take).ToList();
                                else resultado.datos = BusDocs.ToList();
                                resultado.numRegistros = resultado.datos.Count();
                                break;
                        }

                    }
                }
                else
                {
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    return resultado;
                }
                return resultado;
            }
        }

        /// <summary>
        /// Consulta los mensajes relacionados con un tramite 
        /// </summary>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="CodTramite">Codigo del tramite para mostrar los mensajes</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("MensajesTra")]
        public JArray GetMensajesTra(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {

                var model = (from TCO in dbSIM.TBTAREACOMENTARIO
                             join TAR in dbSIM.TBTAREA on TCO.CODTAREA equals TAR.CODTAREA
                             join FUN in dbSIM.QRY_FUNCIONARIO_ALL on TCO.CODFUNCIONARIO equals FUN.CODFUNCIONARIO
                             where TCO.CODTRAMITE == CodTramite
                             orderby TCO.FECHA descending
                             select new
                             {
                                 TCO.FECHA,
                                 ACTIVIDAD = TAR.NOMBRE,
                                 FUNCIONARIO = FUN.NOMBRES,
                                 TCO.COMENTARIO,
                                 IMPORTANCIA = TCO.IMPORTANCIA == "0" ? "Normal" : TCO.IMPORTANCIA == "1" ? "Media" : TCO.IMPORTANCIA == "2" ? "Alta" : "N/A"
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna los indices del tramite
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("IndicesTra")]
        public JArray GetIndicesTra(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {
                var model = (from Ind in dbSIM.TBINDICETRAMITE
                             join Din in dbSIM.TBINDICEPROCESO on Ind.CODINDICE equals Din.CODINDICE
                             where Ind.CODTRAMITE == CodTramite
                             orderby Din.ORDEN
                             select new
                             {
                                 INDICE = Din.INDICE,
                                 VALOR = Ind.VALOR
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna los documentos de un tramite
        /// </summary>
        /// <param name="CodTramite">Codigo del trámite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DocsTra")]
        public JArray GetDocumentosTramite(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {
                var model = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                             join Ser in dbSIM.TBSERIE on Doc.CODSERIE equals Ser.CODSERIE
                             where Doc.CODTRAMITE == CodTramite
                             orderby Doc.CODDOCUMENTO descending
                             select new Documento
                             {
                                 ID_DOCUMENTO = Doc.ID_DOCUMENTO,
                                 CODDOC = Doc.CODDOCUMENTO,
                                 SERIE = Ser.NOMBRE,
                                 FECHA = Doc.FECHACREACION.Value,
                                 ESTADO = Doc.S_ESTADO == "N" ? "Anulado" : "",
                                 ADJUNTO = Doc.S_ADJUNTO != "1" ? "No" : "Si",
                                 CODTRAMITE = Doc.CODTRAMITE
                             }).ToList();
                if (model != null)
                {
                    foreach (var doc in model)
                    {
                        if (doc.ESTADO != "Anulado")
                        {
                            var estDoc = (from A in dbSIM.ANULACION_DOC
                                          join D in dbSIM.TRAMITES_PROYECCION on A.ID_PROYECCION_DOC equals D.ID_PROYECCION_DOC
                                          where D.CODTRAMITE == doc.CODTRAMITE && D.CODDOCUMENTO == doc.CODDOC && A.S_ESTADO == "P"
                                          select A.ID_ANULACION_DOC).FirstOrDefault();
                            if (estDoc > 0) doc.ESTADO = "En proceso";
                        }
                    }
                }
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DocsTemp")]
        public JArray GetDocsTemporalesTramite(int CodTramite, int Orden)
        {
            JsonSerializer Js = new JsonSerializer();
            int idUsuario = 0;
            decimal funcionario = 0;

            Js = JsonSerializer.CreateDefault();
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            }
            if (CodTramite < 0) return null;
            try
            {
                var TraOp = dbSIM.TBTRAMITE.Where(w => w.CODTRAMITE == CodTramite).Select(s => s.ESTADO).FirstOrDefault();
                var FunAct = dbSIM.TBTRAMITETAREA.Where(w => w.CODTRAMITE == CodTramite).Where(w => w.ORDEN == Orden).Select(s => s.CODFUNCIONARIO).FirstOrDefault();

                var model = (from Doc in dbSIM.DOCUMENTO_TEMPORAL
                             join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Doc.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                             where Doc.CODTRAMITE == CodTramite
                             orderby Doc.D_VERSION descending
                             select new
                             {
                                 IDDOCUMENTO = Doc.ID_DOCUMENTO,
                                 FECHAVER = Doc.D_VERSION,
                                 DESCRIPCION = Doc.S_DESCRIPCION,
                                 VERSION = Doc.N_VERSION,
                                 FUNCIONARIO = Fun.NOMBRES,
                                 ESTTRA = TraOp,
                                 PUEDEVER = FunAct == funcionario ? "SI" : "NO",
                                 ESULTVER = dbSIM.DOCUMENTO_TEMPORAL.Where(w => w.S_DESCRIPCION == Doc.S_DESCRIPCION && w.CODTRAMITE == CodTramite).Select(s => s.N_VERSION).Max() == Doc.N_VERSION ? true : false,
                             });
                return JArray.FromObject(model.ToList(), Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        /// <summary>
        /// Retorna los indices del tramite
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("IndicesDoc")]
        public JArray GetIndicesDoc(int IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (IdDocumento < 0) return null;
            try
            {
                var model = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                             join Ind in dbSIM.TBINDICEDOCUMENTO on Doc.CODTRAMITE equals Ind.CODTRAMITE
                             join Din in dbSIM.TBINDICESERIE on Ind.CODINDICE equals Din.CODINDICE
                             where (Doc.CODDOCUMENTO == Ind.CODDOCUMENTO) && (Doc.ID_DOCUMENTO == IdDocumento)
                             orderby Din.ORDEN
                             select new
                             {
                                 CODINDICE = Ind.CODINDICE,
                                 INDICE = Din.INDICE,
                                 VALOR = Ind.VALOR
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Obtiene la ruta de un tramite
        /// </summary>
        /// <param name="CodTramite">Codigo del tramite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RutaTra")]
        public JArray GetRutaTra(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {
                var model = (from Tra in dbSIM.TBTRAMITETAREA
                             join Tar in dbSIM.TBTAREA on Tra.CODTAREA equals Tar.CODTAREA
                             join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Tra.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                             where Tra.CODTRAMITE == CodTramite
                             orderby Tra.ORDEN, Tra.FECHAINI
                             select new
                             {
                                 Tra.ORDEN,
                                 ACTIVIDAD = Tar.NOMBRE,
                                 ENVIADAA = (Tra.COPIA != 0 ? string.Empty : Fun.NOMBRES),
                                 CONCOPIAA = (Tra.COPIA != 1 ? string.Empty : Fun.NOMBRES),
                                 Tra.COMENTARIO,
                                 FECHAINICIAL = Tra.FECHAINI,
                                 FECHAFINAL = Tra.FECHAFIN,
                                 ESTADO = Tra.ESTADO == 1 ? "Terminada" : "Ejecución",
                                 TIPO = Tra.DEVOLUCION == "1" ? "Devolución" : "Flujo"
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Obtiene la ruta seguida por el tramite con respwecto a VItal
        /// </summary>
        /// <param name="CodTramite">Codigo del tramite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RutaVital")]
        public JArray GetRutaVital(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {
                var Vit = (from Tra in dbSIM.TBTRAMITE where Tra.CODTRAMITE == CodTramite && Tra.NUMERO_VITAL.Length > 0 select Tra.NUMERO_VITAL).FirstOrDefault();
                if (Vit != null)
                {
                    var model = (from Acv in dbSIM.TBACTIVIDADES_VITAL
                                 join Tar in dbSIM.TBTAREA on Acv.CODTRAMITE_TAREA equals Tar.CODTAREA
                                 where Acv.CODIGO_VITAL == Vit
                                 orderby Acv.D_FECHA
                                 select new
                                 {
                                     Acv.CODIGO,
                                     ACTIVIDAD = Tar.NOMBRE,
                                     FECHA = Acv.D_FECHA,
                                     COMENTARIO = Acv.S_DESCRIPCION
                                 }).ToList();
                    return JArray.FromObject(model, Js);
                }
                else return null;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Obtiene un listado de los documentos temporales en su utlima version
        /// </summary>
        /// <param name="CodTramite">Codigo del tramite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaTemp")]
        public JArray GetListaTemp(int CodTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (CodTramite < 0) return null;
            try
            {
                var model = (from Temp in dbSIM.DOCUMENTO_TEMPORAL
                             where Temp.CODTRAMITE == CodTramite
                             group Temp by Temp.S_DESCRIPCION into g
                             select new { DESCRIPCION = g.Key, VERSION = g.Max(s => s.N_VERSION) + 1, ID = g.Select(s => s.ID_DOCUMENTO) }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Permite gurdar los datos del documneto temnporal subido al sistema
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaTemporal")]
        public object PostGuardaTemporal(DatosTemp objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el documento Temporal" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _RutaDocuTemp = SIM.Utilidades.Data.ObtenerValorParametro("DocumentosTemporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("DocumentosTemporales").ToString() : "";
            int idUsuario = 0;
            decimal funcionario = 0;
            string _incorrectas = "áéíóúüñÁÉÍÓÚÜÑ^-%''";
            string _correctas = "aeiouunAEIOUUN ____";
            string Ruta = "";
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
                string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
                if (objData.CodTramite > 0 && objData.Version > 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(_Ruta);
                    string _NomParc = objData.CodTramite.ToString() + "-" + objData.Version.ToString() + "-";
                    FileInfo[] files = dir.GetFiles(_NomParc + "*", SearchOption.TopDirectoryOnly);
                    if (files.Length > 0)
                    {
                        Ruta = _RutaDocuTemp + "\\" + SIM.Utilidades.Archivos.GetRutaDocumento((ulong)objData.CodTramite, 100);
                        string fileExt = Path.GetExtension(files[0].FullName).ToLower();
                        if (!File.Exists(Ruta)) Directory.CreateDirectory(Ruta);
                        Ruta += objData.CodTramite.ToString().Trim() + "-" + objData.Descripcion + "-" + objData.Version.ToString().Trim() + fileExt;
                        for (int j = 0; j < _incorrectas.Length; j++)
                        {
                            if (Ruta.Contains(_incorrectas[j].ToString()))
                            {
                                Ruta = Ruta.Replace(_incorrectas[j].ToString(), _correctas[j].ToString());
                            }
                        }
                        if (File.Exists(Ruta)) File.Delete(Ruta);
                        if (Ruta.Contains("..")) Ruta = Ruta.Replace("..", ".");
                        files[0].MoveTo(Ruta);
                        var Tarea = (from Tar in dbSIM.TBTRAMITETAREA
                                     where Tar.CODTRAMITE == objData.CodTramite && Tar.ORDEN == objData.Orden
                                     select Tar).FirstOrDefault();
                        if (Tarea != null)
                        {

                            DOCUMENTO_TEMPORAL Tempo = new DOCUMENTO_TEMPORAL();
                            Tempo.N_VERSION = objData.Version;
                            Tempo.CODTRAMITE = objData.CodTramite;
                            Tempo.CODTAREA = Tarea.CODTAREA;
                            Tempo.CODFUNCIONARIO = Tarea.CODFUNCIONARIO;
                            Tempo.N_ORDEN = Tarea.ORDEN;
                            Tempo.COPIA = Tarea.COPIA;
                            Tempo.CODTIPODOC = 0;
                            Tempo.APROBADO = "1";
                            Tempo.S_DESCRIPCION = objData.Descripcion;
                            Tempo.S_RUTA = Ruta.ToUpper();
                            Tempo.S_VISIBLE = "1";
                            Tempo.D_VERSION = DateTime.Now;
                            Tempo.CODTIPODOC = 0;
                            Tempo.S_FIRMADO = "0";
                            Tempo.CODDOCUMENTO = 0;
                            Tempo.NOMBRE_ARCHIVO = "";
                            Tempo.RADICADO_VITAL = "";
                            Tempo.NRO_SILPA = 0;
                            dbSIM.DOCUMENTO_TEMPORAL.Add(Tempo);
                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            return new { resp = "Error", mensaje = "No se encontró una actual para el trámite y orden ingresado" };
                        }
                    }
                    else
                    {
                        return new { resp = "Error", mensaje = "No ha subido el documento Temporal" };
                    }
                }
            }
            catch (Exception er)
            {
                return new { resp = "Error", mensaje = er.Message + " Ponerse en contacto con el Administrador del Sistema!" };
            }
            return new { resp = "OK", mensaje = "Documento temporal creado correctamente" };
        }

        /// <summary>
        /// Graba el mensaje con el tramite especifico
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaMensaje")]
        public object PostGuardaMensdaje(DatosMsg objData)
        {
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
                var Tarea = (from Tar in dbSIM.TBTRAMITETAREA
                             where Tar.CODTRAMITE == objData.CodTramite && Tar.ORDEN == objData.Orden
                             select Tar).FirstOrDefault();
                if (Tarea != null)
                {
                    var Importancia = objData.Importancia == "Normal" ? "0" : objData.Importancia == "Media" ? "1" : objData.Importancia == "Alta" ? "2" : "0";
                    TBTAREACOMENTARIO Comentario = new TBTAREACOMENTARIO();
                    Comentario.CODFUNCIONARIO = funcionario;
                    Comentario.CODTAREA = Tarea.CODTAREA;
                    Comentario.CODTRAMITE = objData.CodTramite;
                    Comentario.COMENTARIO = objData.Mensaje.ToUpper();
                    Comentario.FECHA = DateTime.Now;
                    Comentario.IMPORTANCIA = Importancia;
                    dbSIM.TBTAREACOMENTARIO.Add(Comentario);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception er)
            {
                return new { resp = "Error", mensaje = er.Message + " Ponerse en contacto con el Administrador del Sistema!" };
            }
            return new { resp = "OK", mensaje = "Documento temporal creado correctamente" };
        }
        private string ProcesaWhereOracle(string _Where)
        {
            ArrayList condicion = new ArrayList();
            string orderby = string.Empty;
            bool _And = true;

            if (_Where != null && _Where.Trim() != "")
            {
                _Where = _Where.Replace("\\", "").Replace("\"", "");
                _And = _Where.ToLower().Contains(",or,") ? false : true;
                string[] filtros = _Where.Split(',');
                for (int contFiltro = 0; contFiltro < filtros.Length; contFiltro += 4)
                {
                    filtros[contFiltro] = filtros[contFiltro].Contains(@"\") ? filtros[contFiltro].Replace(@"\", "") : filtros[contFiltro];
                    switch (filtros[contFiltro + 1])
                    {
                        case "contains":
                            condicion.Add(" INSTR(LOWER(\"" + filtros[contFiltro] + "\"),'" + filtros[contFiltro + 2].Trim().ToLower() + "') > 0");
                            break;
                        case "notcontains":
                            condicion.Add(" INSTR(LOWER(\"" + filtros[contFiltro] + "\"),'" + filtros[contFiltro + 2].Trim().ToLower() + "') = 0");
                            break;
                        case "startswith":
                            condicion.Add(" LOWER(\"" + filtros[contFiltro] + "\") LIKE '" + filtros[contFiltro + 2].Trim().ToLower() + "%'");
                            break;
                        case "endswith":
                            condicion.Add(" LOWER(\"" + filtros[contFiltro] + "\") LIKE '%" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "=":
                            condicion.Add("LOWER(\"" + filtros[contFiltro] + "\") = '" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "<>":
                            condicion.Add("LOWER(\"" + filtros[contFiltro] + "\") <> '" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                            condicion.Add("LOWER(\"" + filtros[contFiltro] + "\") " + filtros[contFiltro + 1] + " @" + filtros[contFiltro + 2].Trim().ToLower());
                            break;
                    }
                }
            }

            if (condicion.Count > 0)
            {
                string _strCondicion = " AND ";
                if (!_And) _strCondicion = " OR ";
                return String.Join(_strCondicion, condicion.ToArray());
            }
            else return "";
        }
        public string ObtenerSqlDocOracle(int IdUnidadDocumental, string _criterio)
        {
            string _aux = "";
            decimal CodSerie = (decimal)IdUnidadDocumental;
            if (CodSerie > 0)
            {
                _aux = "SELECT DISTINCT QRY.ID_DOCUMENTO,QRY.CODTRAMITE,QRY.CODDOCUMENTO,(SELECT SER.NOMBRE FROM TRAMITES.TBSERIE SER WHERE QRY.CODSERIE = SER.CODSERIE) AS NOMBRE,(SELECT DISTINCT BUS.S_INDICE FROM TRAMITES.BUSQUEDA_DOCUMENTO BUS WHERE BUS.ID_DOCUMENTO = QRY.ID_DOCUMENTO) AS INDICES, QRY.FECHACREACION ";
                if (_criterio != "" && _criterio != null)
                {
                    string _codindices = "";
                    _aux += " FROM (SELECT DISTINCT IND.CODTRAMITE,IND.CODDOCUMENTO,";
                    var IndicesSerie = (from Ind in dbSIM.TBINDICESERIE
                                        where Ind.CODSERIE == CodSerie
                                        orderby Ind.INDICE
                                        select new
                                        {
                                            Ind.CODINDICE,
                                            Ind.INDICE,
                                            Ind.TIPO
                                        }).ToList();
                    if (IndicesSerie != null)
                    {
                        foreach (var fila in IndicesSerie)
                        {
                            if (_criterio.Contains(fila.INDICE.ToUpper()))
                            {
                                _aux += "(SELECT TID.VALOR FROM TRAMITES.TBINDICEDOCUMENTO TID WHERE TID.CODTRAMITE = IND.CODTRAMITE AND TID.CODDOCUMENTO=IND.CODDOCUMENTO AND TID.CODINDICE=" + fila.CODINDICE.ToString() + ") AS \"" + fila.INDICE.ToString().ToUpper() + "\",";
                                _codindices += fila.CODINDICE.ToString().Trim() + ",";
                            }
                        }
                    }
                    _aux += "(SELECT DOC.CODSERIE FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS CODSERIE, ";
                    _aux += "(SELECT DOC.ID_DOCUMENTO FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS ID_DOCUMENTO, ";
                    _aux += "(SELECT DOC.FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS FECHACREACION ";
                    _codindices = _codindices.Substring(0, _codindices.Length - 1);
                    _aux += " FROM TRAMITES.TBINDICEDOCUMENTO IND WHERE IND.CODINDICE IN (" + _codindices + ")) QRY ";
                    _aux += " WHERE " + _criterio + " ORDER BY QRY.CODTRAMITE DESC, QRY.CODDOCUMENTO DESC";
                }
                else
                {
                    _aux += "FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODSERIE=" + CodSerie + " ORDER BY DOC.ID_DOCUMENTO DESC";
                }
            }
            return _aux;
        }

        public string ObtenerSqlProcesoOracle(int IdTipoProceso, string _criterio)
        {
            string _aux = "";
            decimal TipoProceso = (decimal)IdTipoProceso;
            if (TipoProceso > 0)
            {
                _aux = "SELECT DISTINCT QRY.CODTRAMITE,(SELECT PRO.NOMBRE FROM TRAMITES.TBPROCESO PRO WHERE QRY.CODPROCESO = PRO.CODPROCESO) AS TIPOPROCESO,QRY.FECHAINI,CASE WHEN QRY.ESTADO = 1 THEN 'Terminado' ELSE 'Abierto' END AS ESTADO ";
                if (_criterio != "" && _criterio != null)
                {
                    string _codindices = "";
                    _aux += "FROM (SELECT DISTINCT IND.CODTRAMITE,";
                    var IndicesProceso = (from Ind in dbSIM.TBINDICEPROCESO
                                        where Ind.CODPROCESO == TipoProceso
                                        orderby Ind.INDICE
                                        select new
                                        {
                                            Ind.CODINDICE,
                                            Ind.INDICE,
                                            Ind.TIPO
                                        }).ToList();
                    if (IndicesProceso != null)
                    {
                        foreach (var fila in IndicesProceso)
                        {
                            if (_criterio.Contains(fila.INDICE.ToUpper()))
                            {
                                _aux += $"(SELECT TID.VALOR FROM TRAMITES.TBINDICETRAMITE TID WHERE TID.CODTRAMITE = IND.CODTRAMITE AND TID.CODINDICE={fila.CODINDICE.ToString()}) AS \"{fila.INDICE.ToString().ToUpper()}\",";
                                _codindices += fila.CODINDICE.ToString().Trim() + ",";
                            }
                        }
                    }
                    _aux += "(SELECT TRA.CODPROCESO FROM TRAMITES.TBTRAMITE TRA WHERE TRA.CODTRAMITE = ind.codtramite) AS CODPROCESO, ";
                    _aux += "(SELECT TRA.FECHAINI FROM TRAMITES.TBTRAMITE TRA WHERE TRA.CODTRAMITE = ind.codtramite) AS FECHAINI, ";
                    _aux += "(SELECT TRA.ESTADO FROM TRAMITES.TBTRAMITE TRA WHERE TRA.CODTRAMITE = ind.codtramite) AS ESTADO";
                    _codindices = _codindices.Substring(0, _codindices.Length - 1);
                    _aux += " FROM TRAMITES.TBINDICETRAMITE IND WHERE IND.CODINDICE IN (" + _codindices + ")) QRY ";
                    _aux += " WHERE " + _criterio + " ORDER BY QRY.CODTRAMITE DESC";
                }
                else
                {
                    _aux += $"FROM TRAMITES.TBTRAMITE QRY WHERE TRA.CODPROCESO={TipoProceso} ORDER BY QRY.CODTRAMITE DESC";
                }
            }
            return _aux;
        }

        public string ObtenerSqlExpOracle(int IdUnidadDocumental, string _criterio)
        {
            string _aux = "";
            decimal CodSerie = (decimal)IdUnidadDocumental;
            if (CodSerie > 0)
            {
                _aux = "SELECT DISTINCT QRY.ID_EXPEDIENTE,QRY.EXPEDIENTE,QRY.CODIGO,(SELECT SER.NOMBRE FROM TRAMITES.TBSERIE SER WHERE QRY.ID_UNIDADDOC = SER.CODSERIE) AS NOMBRE,(SELECT BUS.S_INDICE FROM TRAMITES.EXP_BUSQUEDA BUS WHERE BUS.ID_EXPEDIENTE = QRY.ID_EXPEDIENTE) AS INDICES ";
                if (_criterio != "" && _criterio != null)
                {
                    string _codindices = "";
                    _aux += "FROM (SELECT DISTINCT IND.ID_EXPEDIENTE,";
                    var IndicesSerie = (from Ind in dbSIM.TBINDICESERIE
                                        where Ind.CODSERIE == CodSerie
                                        orderby Ind.INDICE
                                        select new
                                        {
                                            Ind.CODINDICE,
                                            Ind.INDICE,
                                            Ind.TIPO
                                        }).ToList();
                    if (IndicesSerie != null)
                    {
                        string _Indice = "";
                        foreach (var fila in IndicesSerie)
                        {
                            _Indice = fila.INDICE.ToUpper().Replace(" ", "");
                            if (_criterio.Contains(_Indice))
                            {
                                _aux += "(SELECT CASE WHEN DIN.TIPO IN (0,3,4,5,8) THEN TID.VALOR_TXT WHEN DIN.TIPO IN(1,6,7) THEN TO_CHAR(TID.VALOR_NUM) WHEN DIN.TIPO = 2 THEN TO_CHAR(TID.VALOR_FEC, 'DD-MM-YYYY') ELSE '' END FROM TRAMITES.EXP_INDICES TID INNER JOIN TRAMITES.TBINDICESERIE DIN ON TID.CODINDICE = DIN.CODINDICE WHERE TID.ID_EXPEDIENTE = IND.ID_EXPEDIENTE AND TID.CODINDICE = " + fila.CODINDICE.ToString() + ")  AS \"" + _Indice + "\",";
                                _codindices += fila.CODINDICE.ToString().Trim() + ",";
                            }
                        }

                    }
                    _aux += "(SELECT EXP.S_NOMBRE FROM TRAMITES.EXP_EXPEDIENTES EXP WHERE EXP.ID_EXPEDIENTE = IND.ID_EXPEDIENTE) AS EXPEDIENTE,";
                    _aux += "(SELECT EXP.S_CODIGO FROM TRAMITES.EXP_EXPEDIENTES EXP WHERE EXP.ID_EXPEDIENTE = IND.ID_EXPEDIENTE) AS CODIGO,";
                    _aux += "(SELECT EXP.ID_UNIDADDOC FROM TRAMITES.EXP_EXPEDIENTES EXP WHERE EXP.ID_EXPEDIENTE = IND.ID_EXPEDIENTE) AS ID_UNIDADDOC";
                    _codindices = _codindices.Substring(0, _codindices.Length - 1);
                    _aux += " FROM TRAMITES.EXP_INDICES IND WHERE IND.CODINDICE IN (" + _codindices + ")) QRY ";
                    _aux += " WHERE " + _criterio + " ORDER BY QRY.ID_EXPEDIENTE DESC";
                }
                else
                {
                    _aux += "FROM TRAMITES.EXP_INDICES IND INNER JOIN TRAMITES.EXP_EXPEDIENTES EXP ON IND.ID_EXPEDIENTE = EXP.ID_EXPEDIENTE WHERE EXP.ID_UNIDADDOC=" + CodSerie + " ORDER BY EXP.ID_EXPEDIENTE DESC";
                }
            }
            return _aux;
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DocsVital")]
        public JArray GetDocsVital(string Vital)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (string.IsNullOrEmpty(Vital) || Vital == "-1") return null;
            List<DocumentosVital> documentosVital = new List<DocumentosVital>();
            try
            {
                var IdVital = (from Vit in dbSIM.TBSOLICITUDES_VITAL
                               where Vit.NUMERO_VITAL == Vital
                               select Vit.IDENTIFICADOR).FirstOrDefault();
                if (!string.IsNullOrEmpty(IdVital))
                {
                    string[] _datos = IdVital.Split('|');
                    long.TryParse(_datos[0], out long idRadicado);
                    try
                    {
                        WSPQ03 ws = new WSPQ03();
                        string xmlData = ws.ObtenerDocumentosRadicacion(idRadicado);
                        string[] Documentos = xmlData.Split(';');
                        foreach (string s in Documentos)
                        {
                            if (s.Trim() != "")
                            {
                                documentosVital.Add(new DocumentosVital { ID_RADICACION = (int)idRadicado, Documento = s.Trim() });
                            }
                        }
                        var NumVitalAso = (from Vit in dbSIM.TBSOLICITUDES_VITAL
                                           where Vit.NUMERO_VITAL_ASOCIADO == Vital
                                           select Vit.NUMERO_VITAL).ToList();
                        if (NumVitalAso.Count() > 0)
                        {
                            var AuxIdVital = "";
                            foreach (string nv in NumVitalAso)
                            {
                                AuxIdVital = (from Vit in dbSIM.TBSOLICITUDES_VITAL
                                              where Vit.NUMERO_VITAL == nv
                                              select Vit.IDENTIFICADOR).FirstOrDefault();
                                _datos = AuxIdVital.Split('|');
                                long.TryParse(_datos[0], out idRadicado);
                                xmlData = ws.ObtenerDocumentosRadicacion(idRadicado);
                                Documentos = xmlData.Split(';');
                                foreach (string s in Documentos)
                                {
                                    if (s.Trim() != "")
                                    {
                                        documentosVital.Add(new DocumentosVital { ID_RADICACION = (int)idRadicado, Documento = s.Trim() });
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
                return JArray.FromObject(documentosVital, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.HttpPost, System.Web.Http.ActionName("SubeDocVital")]
        public object PostSubeDocVital(DatosDocVital objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el documento Temporal" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _RutaDocuTemp = SIM.Utilidades.Data.ObtenerValorParametro("DocumentosTemporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("DocumentosTemporales").ToString() : "";
            int idUsuario = 0;
            decimal funcionario = 0;
            string _incorrectas = "áéíóúüñÁÉÍÓÚÜÑ^-%''";
            string _correctas = "aeiouunAEIOUUN ____";
            string Ruta = "";
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
                string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
                if (objData.CodTramite > 0 && objData.DocumentoVital != "")
                {
                    string _ExtDoc = Path.GetExtension(objData.DocumentoVital);
                    Ruta = _RutaDocuTemp + "\\" + SIM.Utilidades.Archivos.GetRutaDocumento((ulong)objData.CodTramite, 100);
                    if (!File.Exists(Ruta)) Directory.CreateDirectory(Ruta);
                    Ruta += objData.CodTramite.ToString().Trim() + "-" + objData.Descripcion.ToUpper() + "-1" + _ExtDoc;
                    for (int j = 0; j < _incorrectas.Length; j++)
                    {
                        if (Ruta.Contains(_incorrectas[j].ToString()))
                        {
                            Ruta = Ruta.Replace(_incorrectas[j].ToString(), _correctas[j].ToString());
                        }
                    }
                    if (File.Exists(Ruta)) File.Delete(Ruta);
                    if (Ruta.Contains("..")) Ruta = Ruta.Replace("..", ".");
                    WSPQ03 ws = new WSPQ03();
                    Byte[] _Documento = ws.ObtenerDocumentoRadicacion(objData.RadicadoVital, objData.DocumentoVital);
                    if (_Documento.Length > 0)
                    {
                        File.WriteAllBytes(Ruta, _Documento);
                        var Tarea = (from Tar in dbSIM.TBTRAMITETAREA
                                     where Tar.CODTRAMITE == objData.CodTramite && Tar.ORDEN == objData.Orden
                                     select Tar).FirstOrDefault();
                        if (Tarea != null)
                        {

                            DOCUMENTO_TEMPORAL Tempo = new DOCUMENTO_TEMPORAL();
                            Tempo.N_VERSION = 1;
                            Tempo.CODTRAMITE = objData.CodTramite;
                            Tempo.CODTAREA = Tarea.CODTAREA;
                            Tempo.CODFUNCIONARIO = Tarea.CODFUNCIONARIO;
                            Tempo.N_ORDEN = Tarea.ORDEN;
                            Tempo.COPIA = Tarea.COPIA;
                            Tempo.CODTIPODOC = 0;
                            Tempo.APROBADO = "1";
                            Tempo.S_DESCRIPCION = objData.DocumentoVital;
                            Tempo.S_RUTA = Ruta.ToUpper();
                            Tempo.S_VISIBLE = "1";
                            Tempo.D_VERSION = DateTime.Now;
                            Tempo.CODTIPODOC = 0;
                            Tempo.S_FIRMADO = "0";
                            Tempo.CODDOCUMENTO = 0;
                            Tempo.NOMBRE_ARCHIVO = "";
                            Tempo.RADICADO_VITAL = "";
                            Tempo.NRO_SILPA = 0;
                            dbSIM.DOCUMENTO_TEMPORAL.Add(Tempo);
                            dbSIM.SaveChanges();
                        }
                        else
                        {
                            return new { resp = "Error", mensaje = "No se encontró una actual para el trámite y orden ingresado" };
                        }
                    }
                }
                else
                {
                    return new { resp = "Error", mensaje = "No ha subido el documento Temporal" };
                }

            }
            catch (Exception er)
            {
                return new { resp = "Error", mensaje = er.Message + " Ponerse en contacto con el Administrador del Sistema!" };
            }
            return new { resp = "OK", mensaje = "Documento temporal creado correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EditarIndicesTramite")]
        public dynamic GetEditarIndicesTramite(int CodTramite)
        {
            var Indices = (from Ind in dbSIM.TBINDICETRAMITE
                           join Ise in dbSIM.TBINDICEPROCESO on Ind.CODINDICE equals Ise.CODINDICE
                           join lista in dbSIM.TBSUBSERIE on (decimal)Ise.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                           from pdis in l.DefaultIfEmpty()
                           where Ind.CODTRAMITE == CodTramite
                           orderby Ise.ORDEN
                           select new Indice
                           {
                               CODINDICE = (int)Ind.CODINDICE,
                               INDICE = Ise.INDICE,
                               TIPO = (byte)Ise.TIPO,
                               LONGITUD = (long)Ise.LONGITUD,
                               OBLIGA = (int)Ise.OBLIGA,
                               VALORDEFECTO = Ise.VALORDEFECTO,
                               VALOR = Ind.VALOR,
                               ID_LISTA = (int)Ise.CODIGO_SUBSERIE,
                               TIPO_LISTA = pdis.TIPO,
                               CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                           }).ToList();
            return Indices.ToList();
        }

        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaindicesTramite")]
        public object PostGuardaindicesTramite(IndicesTramite objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando los indices del trámite" };
            try
            {
                if (objData.Indices != null)
                {
                    foreach (Indice indice in objData.Indices)
                    {
                        if (indice.OBLIGA == 1 && (indice.VALOR == null || indice.VALOR == ""))
                        {
                            return new { resp = "Error", mensaje = "Indice " + indice.INDICE + " es obligatorio y no se ingresó un valor!!" };
                        }
                        if (objData.CodTramite > 1)
                        {
                            TBINDICETRAMITE indiceDoc = dbSIM.TBINDICETRAMITE.Where(i => i.CODTRAMITE == objData.CodTramite && i.CODINDICE == indice.CODINDICE).FirstOrDefault();
                            if (indiceDoc != null)
                            {
                                indiceDoc.VALOR = indice.VALOR ?? "";
                                dbSIM.Entry(indiceDoc).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                indiceDoc = new TBINDICETRAMITE();

                                indiceDoc.CODTRAMITE = objData.CodTramite;
                                indiceDoc.CODINDICE = indice.CODINDICE;
                                indiceDoc.VALOR = indice.VALOR ?? "";
                                dbSIM.Entry(indiceDoc).State = System.Data.Entity.EntityState.Added;
                            }
                            dbSIM.SaveChanges();
                        }
                    }
                }
                else
                {
                    var CodProceso = dbSIM.TBTRAMITE.Where(w => w.CODTRAMITE == objData.CodTramite).Select(s => s.CODPROCESO).FirstOrDefault();
                    var indiceObliga = (from ind in dbSIM.TBINDICEPROCESO
                                        where ind.CODPROCESO == CodProceso && ind.OBLIGA == 1
                                        select ind).FirstOrDefault();
                    if (indiceObliga != null)
                    {
                        return new { resp = "Error", mensaje = "No se han ingresado indices y el tipo de procreso tiene indices obligatorios!!" };
                    }
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando los indices: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Indices ingresados correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EditarIndicesDocumento")]
        public dynamic GetEditarIndicesDocumento(int IdDocumento)
        {
            var Indices = (from Ind in dbSIM.TBINDICEDOCUMENTO
                           join Ise in dbSIM.TBINDICESERIE on Ind.CODINDICE equals Ise.CODINDICE
                           join lista in dbSIM.TBSUBSERIE on (decimal)Ise.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                           from pdis in l.DefaultIfEmpty()
                           where Ind.ID_DOCUMENTO == IdDocumento
                           orderby Ise.ORDEN
                           select new Indice
                           {
                               CODINDICE = (int)Ind.CODINDICE,
                               INDICE = Ise.INDICE,
                               TIPO = (byte)Ise.TIPO,
                               LONGITUD = (long)Ise.LONGITUD,
                               OBLIGA = (int)Ise.OBLIGA,
                               VALORDEFECTO = Ise.VALORDEFECTO,
                               VALOR = Ind.VALOR,
                               ID_LISTA = (int)Ise.CODIGO_SUBSERIE,
                               TIPO_LISTA = pdis.TIPO,
                               CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                           }).ToList();
            return Indices.ToList();
        }

        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaindicesDocumento")]
        public object PostGuardaindicesDocumento(IndicesDocumento objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando los indices del documento" };
            try
            {
                if (objData.Indices != null)
                {

                    foreach (Indice indice in objData.Indices)
                    {
                        if (indice.OBLIGA == 1 && (indice.VALOR == null || indice.VALOR == ""))
                        {
                            return new { resp = "Error", mensaje = "Indice " + indice.INDICE + " es obligatorio y no se ingresó un valor!!" };
                        }
                        if (objData.IdDocumento > 1)
                        {
                            TBINDICEDOCUMENTO indiceDoc = dbSIM.TBINDICEDOCUMENTO.Where(i => i.ID_DOCUMENTO == objData.IdDocumento && i.CODINDICE == indice.CODINDICE).FirstOrDefault();
                            if (indiceDoc != null)
                            {
                                indiceDoc.VALOR = indice.VALOR ?? "";
                                dbSIM.Entry(indiceDoc).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                indiceDoc = new TBINDICEDOCUMENTO();

                                indiceDoc.ID_DOCUMENTO = objData.IdDocumento;
                                indiceDoc.CODTRAMITE = 
                                indiceDoc.CODINDICE = indice.CODINDICE;
                                indiceDoc.VALOR = indice.VALOR ?? "";
                                dbSIM.Entry(indiceDoc).State = System.Data.Entity.EntityState.Added;
                            }
                            dbSIM.SaveChanges();
                        }
                    }
                }
                else
                {
                    var UniDoc = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.ID_DOCUMENTO == objData.IdDocumento).Select(s => s.CODSERIE).FirstOrDefault();
                    var indiceObliga = (from ind in dbSIM.TBINDICESERIE
                                        where ind.CODSERIE == UniDoc && ind.OBLIGA == 1
                                        select ind).FirstOrDefault();
                    if (indiceObliga != null)
                    {
                        return new { resp = "Error", mensaje = "No se han ingresado indices y el tipo de procreso tiene indices obligatorios!!" };
                    }
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando los indices: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Indices ingresados correctamente" };
        }

    }


    public class Where
    {
        public Condicion CondIzq { get; set; }
        public string ConectorPpal { get; set; }
        public Condicion CondDer { get; set; }
    }

    public class Condicion
    {
        public string Izquierdo { get; set; }
        public string Conector { get; set; }
        public string Derecho { get; set; }
    }

    public class DatosDocs
    {
       public decimal ID_DOCUMENTO { get; set; }
       public decimal CODTRAMITE { get; set; }
       public decimal CODDOCUMENTO { get; set; }
       public string NOMBRE { get; set; }
       public string INDICES { get; set; }
       public DateTime FECHACREACION { get; set; }
    }
    public class DatosExps
    {
        public decimal ID_EXPEDIENTE { get; set; }
        public string EXPEDIENTE { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string INDICES { get; set; }
    }

    public class DatosTemp
    {
        public int CodTramite { get; set; }
        public int Orden { get; set; }
        public int Version  { get; set; }
        public string Descripcion { get; set; }
    }

    public class DatosMsg
    {
        public int CodTramite { get; set; }
        public int Orden { get; set; }
        public string Mensaje { get; set; }
        public string Importancia { get; set; }
    }

    public class DocumentosVital 
    {
        public int ID_RADICACION { get; set; }
        public string Documento { get; set; }
    }
    public class DatosDocVital
    {
        public int CodTramite { get; set; }
        public int Orden { get; set; }
        public string DocumentoVital { get; set; }
        public string Descripcion { get; set; }
        public int RadicadoVital { get; set; }
    }

    public class Indice
    {
        public int CODINDICE { get; set; }
        public string INDICE { get; set; }
        public byte TIPO { get; set; }
        public long LONGITUD { get; set; }
        public int OBLIGA { get; set; }
        public string VALORDEFECTO { get; set; }
        public string VALOR { get; set; }
        public Nullable<int> ID_LISTA { get; set; }
        public Nullable<int> TIPO_LISTA { get; set; }
        public string CAMPO_NOMBRE { get; set; }
    }

    public class IndicesTramite
    {
        public int CodTramite { get; set; }
        public List<Indice> Indices { get; set; }
    }

    public class Documento
    {
        public decimal ID_DOCUMENTO { get; set; }
        public decimal CODDOC { get; set; }
        public string SERIE { get; set; }
        public DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string ADJUNTO { get; set; }
        public decimal CODTRAMITE { get; set; }
    }

    public class IndicesDocumento
    {
        public int IdDocumento { get; set; }
        public List<Indice> Indices { get; set; }
    }

    public class DatosTramites
    {
        public decimal CODTRAMITE { get; set; }
        public string TIPOPROCESO { get; set; }
        public DateTime FECHAINI { get; set; }
        public string ESTADO { get; set; }
    }
}
