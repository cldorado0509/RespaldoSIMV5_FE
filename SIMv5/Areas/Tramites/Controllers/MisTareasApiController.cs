using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Data.Entity;
using System.Transactions;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SIM.Data;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SIM.Areas.Tramites.Controllers
{
    [Authorize]
    public class MisTareasApiController : ApiController
    {

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        SIM.Utilidades.Tramites Tramites = new SIM.Utilidades.Tramites();

        public struct datosRetorno
        {
            public IEnumerable<dynamic> datos;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("MisTareas")]
        public datosConsulta GetMisTareas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, long CodFuncionario, string estado, string tipo)
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
                int intEstado = 0;
                int intTipo = tipo == "1" ? 0 : 1;
                dynamic model = null;
                if (estado == "2") intEstado = 1;
                if (estado == "3")
                {
                    model = (from TTE in dbSIM.TBTRAMITETAREA
                             join TRA in dbSIM.TBTRAMITE on TTE.CODTRAMITE equals TRA.CODTRAMITE
                             join PRO in dbSIM.TBPROCESO on TRA.CODPROCESO equals PRO.CODPROCESO
                             join TAR in dbSIM.TBTAREA on TTE.CODTAREA equals TAR.CODTAREA
                             join INE in dbSIM.QRY_INDICE_EXPEDIENTETRAMITE on TTE.CODTRAMITE equals INE.CODTRAMITE
                             join PRI in dbSIM.QRY_TRAMITEPRIORITARIO on TTE.CODTRAMITE equals PRI.CODTRAMITE into PRIS
                             from PRI in PRIS.DefaultIfEmpty()
                             where TTE.CODFUNCIONARIO == CodFuncionario && TTE.COPIA == intTipo
                             select new
                             {
                                 CODTRAMITE = TTE.CODTRAMITE.ToString(),
                                 VITAL = TRA.NUMERO_VITAL,
                                 PROCESO = PRO.NOMBRE,
                                 TAREA = TAR.NOMBRE,
                                 INICIOTRAMITE = TRA.FECHAINI,
                                 TTE.COPIA,
                                 TTE.RECIBIDA,
                                 INE.EXPEDIENTE,
                                 INICIOTAREA = TTE.FECHAINI,
                                 TTE.DEVOLUCION,
                                 PRI.PRIORITARIO,
                                 MARCAR = PRO.S_MARCATAREA,
                                 COLOR = PRO.S_COLORMARCA,
                                 ASUNTO = TRA.MENSAJE,
                                 TIPO = TTE.DEVOLUCION == "1" ? "Devolución" : "Normal",
                                 FINTAREA = TTE.FECHAFIN
                             });
                } else
                {
                    model = (from TTE in dbSIM.TBTRAMITETAREA
                             join TRA in dbSIM.TBTRAMITE on TTE.CODTRAMITE equals TRA.CODTRAMITE
                             join PRO in dbSIM.TBPROCESO on TRA.CODPROCESO equals PRO.CODPROCESO
                             join TAR in dbSIM.TBTAREA on TTE.CODTAREA equals TAR.CODTAREA
                             join INE in dbSIM.QRY_INDICE_EXPEDIENTETRAMITE on TTE.CODTRAMITE equals INE.CODTRAMITE
                             join PRI in dbSIM.QRY_TRAMITEPRIORITARIO on TTE.CODTRAMITE equals PRI.CODTRAMITE into PRIS
                             from PRI in PRIS.DefaultIfEmpty()
                             where TTE.CODFUNCIONARIO == CodFuncionario && TTE.COPIA == intTipo && TTE.ESTADO == intEstado
                             select new
                             {
                                 CODTRAMITE = TTE.CODTRAMITE.ToString(),
                                 VITAL = TRA.NUMERO_VITAL,
                                 PROCESO = PRO.NOMBRE,
                                 TAREA = TAR.NOMBRE,
                                 INICIOTRAMITE = TRA.FECHAINI,
                                 TTE.COPIA,
                                 TTE.RECIBIDA,
                                 INE.EXPEDIENTE,
                                 INICIOTAREA = TTE.FECHAINI,
                                 TTE.DEVOLUCION,
                                 PRI.PRIORITARIO,
                                 MARCAR = PRO.S_MARCATAREA,
                                 COLOR = PRO.S_COLORMARCA,
                                 ASUNTO = TRA.MENSAJE,
                                 TTE.CODTAREA,
                                 TTE.ORDEN,
                                 TIPO = TTE.DEVOLUCION == "1" ? "Devolución" : "Normal",
                                 FINTAREA = TTE.FECHAFIN
                             });
                }
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
                return resultado;
            }
        }

        /// <summary>
        /// Consulta y retorna el funcionario anterior 
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <param name="CodTarea"></param>
        /// <param name="Orden"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("FuncionarioAnterior")]
        public string GetFuncionarioTareaAnterior(long CodTramite, long CodTarea, long Orden)
        {
            string _Rpta = "";
            if (CodTramite > 0 && CodTarea > 0 && Orden > 0)
            {
                try
                {
                    long _TarAnt = SIM.Utilidades.Tramites.ObtieneTareaAnterior(CodTramite, CodTarea, Orden);
                    if (_TarAnt > 0)
                    {
                            var FunAnt = (from Tta in dbSIM.TBTRAMITETAREA
                                      join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Tta.CODFUNCIONARIO equals Fun.CODFUNCIONARIO
                                      where (Tta.CODTRAMITE == CodTramite) && (Tta.CODTAREA == _TarAnt) && (Tta.ORDEN == (Orden - 1))
                                      select new
                                      {
                                          Tta.CODFUNCIONARIO,
                                          Fun.NOMBRES
                                      }).FirstOrDefault();
                            if (FunAnt != null)
                            {
                                var FunWeb = SIM.Utilidades.Data.ObtenerValorParametro("FuncionarioPQRSD").ToString() != "" ? long.Parse(SIM.Utilidades.Data.ObtenerValorParametro("FuncionarioPQRSD").ToString()) : -1;
                                var _TareaPQRSD = SIM.Utilidades.Data.ObtenerValorParametro("TareaIniciaPQRSD").ToString() != "" ? long.Parse(SIM.Utilidades.Data.ObtenerValorParametro("TareaIniciaPQRSD").ToString()) : -1;
                                if (FunWeb > 0 && _TareaPQRSD > 0)
                                {
                                    if (FunWeb == FunAnt.CODFUNCIONARIO)
                                    {
                                        var _Funcionarios = SIM.Utilidades.Tramites.ObtenerCodFuncionariosResponsables((int)_TareaPQRSD);
                                        if (_Funcionarios != null)
                                        {
                                            if (_Funcionarios.Count() == 1)
                                            {
                                                string[] _Fun = _Funcionarios.First().Split(';');
                                                _Rpta = _Fun[1].ToUpper();
                                            }
                                            else if (_Funcionarios.Count() > 1)
                                            {
                                                var _MenosCarga = SIM.Utilidades.Tramites.ObtenerFuncMenosCargaTarea((int)_TareaPQRSD);
                                                string[] _AuxFun;
                                                foreach (var F in _Funcionarios)
                                                {
                                                   _AuxFun = F.Split(';');
                                                   if (_MenosCarga == decimal.Parse(_AuxFun[0])) _Rpta = _AuxFun[1].ToUpper();
                                                }
                                            }
                                            else _Rpta = "No se encontró un funcionario en la tarea anterior";
                                    }
                                        else _Rpta = "No se encontró un funcionario en la tarea anterior";
                                    }
                                    else _Rpta = FunAnt.NOMBRES.ToUpper();
                                }
                                else _Rpta = FunAnt.NOMBRES.ToUpper();
                            }
                            else _Rpta = "No se encontró un funcionario en la tarea anterior";
                    }
                }
                catch (Exception ex)
                {
                    _Rpta = ex.Message;
                }
            }
            else
                _Rpta = "No ha ingresado los datos necesarios";
            return _Rpta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("MotivosDevolucion")]
        public JArray GetListaMotivoDevolucion()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            //datosRetorno objDatos = new datosRetorno();
            try
            {
                var model = (from Mot in dbSIM.DEVOLUCION_TAREAMOTIVO
                             where Mot.S_ACTIVO == "1"
                             orderby Mot.S_MOTIVO
                             select new
                             {
                                 IdMotivo =Mot.ID_MOTIVO,
                                 Motivo = Mot.S_MOTIVO
                             });
               // objDatos.datos = model.ToArray();
                //var json = JsonConvert.SerializeObject(objDatos);
                //return json;
                return JArray.FromObject(model, Js);
                //return Ok(objDatos);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DevolverTarea")]
        public string DevolverTarea(long CodTramite,long CodTarea,long Orden, string Funcionario, string Comentario, string Motivos)
        {
            if (CodTramite == 0) return "No se ha ingresado un código de trámite";
            if (CodTarea == 0) return "No se ha ingresado un código de tarea";
            if (Orden == 0) return "No se ha ingresado un orden para el trámite";
            if (Comentario == "" || Comentario == null) return "No se ha ingresado mensaje con la devolución del trámite";
            return Tramites.DevolverTramite(CodTramite, CodTarea, Funcionario, (int)Orden, Comentario, Motivos);
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("TerminaCopia")]
        public string GetTerminaCopia(int CodTramite, int CodTarea, int Orden)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();
            string _Resp = "";
            if (codFuncionario > 0)
            {
                try
                {
                    var model = (from tar in dbSIM.TBTRAMITETAREA
                                 where tar.CODTRAMITE == CodTramite && tar.CODTAREA == CodTarea && tar.CODFUNCIONARIO == codFuncionario && tar.ORDEN == Orden
                                 select tar).FirstOrDefault();
                    if (model != null)
                    {
                        model.ESTADO = 1;
                        model.FECHAFIN = DateTime.Now;
                        dbSIM.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    _Resp = "Ocurrio problema.. " + exp.Message;
                }
            }
            else _Resp = "Problemas con el usuario o no esta autenticado!!";
            return _Resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EsTramiteBloqueado")]
        public bool GetEsTramiteBloqueado(int CodTramite)
        {
            bool resp = false;
            if (CodTramite <= 0) return false;
            try {
                var bloq = (from blo in dbSIM.TBTRAMITES_BLOQUEADOS
                            where blo.CODTRAMITE == CodTramite && blo.CODFUNCIONARIODESBLOQUEO == null
                            select blo.CODTRAMITE_BLOQUEADO).FirstOrDefault();
                if (bloq > 0) resp = true; else resp = false;
            }
            catch 
            {
                resp = false;
            }
            return resp;
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DesbloqueaTramite")]
        public string GetDesbloqueaTramite(long CodTramite, long  CodFuncionario, string Comentario)
        {
            string resp = "";
            if (CodTramite == 0) return "No se ha ingresado un código de trámite";
            if (CodFuncionario == 0) return "No se ha ingresado el código del funcionario";
            if (Comentario == "" || Comentario == null) return "No se ha ingresado mensaje con la descripción del desbloqueo del trámite";
            try
            {
                var Bloq = (from Blo in dbSIM.TBTRAMITES_BLOQUEADOS
                            where Blo.CODTRAMITE == CodTramite && Blo.CODFUNCIONARIODESBLOQUEO == null
                            select Blo).FirstOrDefault();
                if (Bloq != null && Bloq.CODTRAMITE == CodTramite)
                {
                    Bloq.CODFUNCIONARIODESBLOQUEO = (int)CodFuncionario;
                    Bloq.FECHADESBLOQUEO = DateTime.Now;
                    Bloq.OBSERVACIONES = Comentario;
                    dbSIM.SaveChanges();
                }
                else resp = "No se encontró el tramite bloqueado!";
            }
            catch (Exception exp)
            {
                resp = "Ocurrio problema.. " + exp.Message;
            }
            return resp;
        }
    }
}
