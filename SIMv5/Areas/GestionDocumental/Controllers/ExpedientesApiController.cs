using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using SIM.Areas.ControlVigilancia.Models;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SIM.Areas.GestionDocumental.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpedientesApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        System.Web.HttpContext context = System.Web.HttpContext.Current;


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
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneExpedientes")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public datosConsulta ObtieneExpedientes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                var model = (from Exp in dbSIM.EXP_EXPEDIENTES
                             join Ser in dbSIM.TBSERIE on Exp.ID_UNIDADDOC equals Ser.CODSERIE
                             orderby Exp.D_FECHACREACION
                             select new
                             {
                                 Exp.ID_EXPEDIENTE,
                                 TIPO = Ser.NOMBRE,
                                 NOMBRE = Exp.S_NOMBRE,
                                 CODIGO = Exp.S_CODIGO,
                                 FECHACREA = Exp.D_FECHACREACION,
                                 ANULADO = Exp.S_ESTADO == "A" ? "No" : Exp.S_ESTADO == "N" ? "Si" : "N/A",
                                 ESTADO = (from ee in dbSIM.EXP_ESTADOSEXPEDIENTE
                                           join te in dbSIM.EXP_TIPOESTADO on ee.ID_ESTADO equals te.ID_ESTADO
                                           where ee.ID_EXPEDIENTE == Exp.ID_EXPEDIENTE
                                           orderby ee.D_INICIA descending
                                           select te.S_NOMBRE).FirstOrDefault()
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
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
        /// <param name="IdExpediente"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneTomos")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public datosConsulta ObtieneTomos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int IdExpediente)
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
                var model = (from Tom in dbSIM.EXP_TOMOS
                             where Tom.ID_EXPEDIENTE == IdExpediente
                             orderby Tom.N_TOMO
                             select new
                             {
                                 Tom.ID_TOMO,
                                 Tom.N_TOMO,
                                 UBICACION = Tom.S_UBICACION,
                                 FECHA = Tom.D_FECHACREACION,
                                 FUNCCREA = dbSIM.QRY_FUNCIONARIO_ALL.Where(f => f.CODFUNCIONARIO == Tom.ID_FUNCCREACION).Select(s => s.NOMBRES).FirstOrDefault(),
                                 ABIERTO = Tom.S_ABIERTO == "1" ? "Si" : "No",
                                 DOCUMENTOS = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(T => T.ID_TOMO == Tom.ID_TOMO).Count(),
                                 FOLIOS = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(T => T.ID_TOMO == Tom.ID_TOMO && T.D_FECHA == dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Max(m => m.D_FECHA)).Select(s => s.N_FOLIOFIN).FirstOrDefault()
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
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
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocsTomo")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public datosConsulta ObtieneDocsTomo(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int IdTomo)
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
                var model = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                             join Tdo in dbSIM.TBTRAMITEDOCUMENTO on Doc.ID_DOCUMENTO equals Tdo.ID_DOCUMENTO
                             where Doc.ID_TOMO == IdTomo
                             orderby Doc.N_ORDEN
                             select new
                             {
                                 Doc.ID_TOMO,
                                 Doc.ID_DOCUMENTO,
                                 ORDEN = Doc.N_ORDEN,
                                 FECASOCIA = Doc.D_FECHA,
                                 FECDIGITA = Tdo.FECHACREACION,
                                 FUNCASOCIA = dbSIM.QRY_FUNCIONARIO_ALL.Where(f => f.CODFUNCIONARIO == Doc.ID_FUNCASOCIA).Select(s => s.NOMBRES).FirstOrDefault(),
                                 TIPODOC = (from Ser in dbSIM.TBSERIE where Ser.CODSERIE == Tdo.CODSERIE select Ser.NOMBRE).FirstOrDefault(),
                                 FOLIOS = Doc.N_FOLIOINI.ToString() + " - " + Doc.N_FOLIOFIN.ToString(),
                                 IMAGENES = Doc.N_IMAGENES
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="IdExp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EstadosExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public datosConsulta ObtieneEstExpediente(int skip, int take, int IdExp)
        {
            datosConsulta resultado = new datosConsulta();
            if (IdExp <= 0)
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {

                var model = (from Est in dbSIM.EXP_ESTADOSEXPEDIENTE
                             join Tes in dbSIM.EXP_TIPOESTADO on Est.ID_ESTADO equals Tes.ID_ESTADO
                             where Est.ID_EXPEDIENTE == IdExp
                             orderby Est.D_INICIA
                             select new
                             {
                                 Est.ID_ESTADOEXPEDIENTE,
                                 ESTADO = Tes.S_NOMBRE,
                                 FUNCIONARIO = dbSIM.QRY_FUNCIONARIO_ALL.Where(w => w.CODFUNCIONARIO == Est.ID_FUNCIONARIOESTADO).Select(s => s.NOMBRES).FirstOrDefault(),
                                 FECHAINI = Est.D_INICIA,
                                 FECHAFIN = Est.D_FIN.HasValue ? Est.D_FIN.Value : Est.D_FIN
                             });
                resultado.numRegistros = model.Count();
                if (skip == 0 && take == 0) resultado.datos = model.ToList();
                else resultado.datos = model.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaEstadosExp")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetListaEstadosExp()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Est in dbSIM.EXP_TIPOESTADO
                             where Est.S_ESTADOINICIAL != "1"
                             orderby Est.S_NOMBRE
                             select new
                             {
                                 Est.ID_ESTADO,
                                 ESTADO = Est.S_NOMBRE
                             }).Distinct();
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaSeries")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetListaSeries()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var vigencia = (from vig in dbSIM.TBVIGENCIA_TRD orderby vig.D_INICIOVIGENCIA descending select vig.COD_VIGENCIA_TRD).FirstOrDefault();
                if (vigencia <= 0)
                {
                    throw new Exception("No se encontró una vigencia de la TRD!");
                }
                var model = (from Ser in dbSIM.TBSERIE_DOCUMENTAL
                             join Sub in dbSIM.TBSUBSERIE_DOCUMENTAL on Ser.CODSERIE_DOCUMENTAL equals Sub.CODSERIE_DOCUMENTAL
                             join Uni in dbSIM.TBSERIE on Sub.CODSUBSERIE_DOCUMENTAL equals Uni.CODSUBSERIE_DOCUMENTAL
                             join Vud in dbSIM.TBUNIDADESDOC_VIGENCIATRD on Uni.CODSERIE equals Vud.CODUNIDAD_DOCUMENTAL
                             where Uni.S_DEFINEEXPEDIENTE == "1" && Uni.ACTIVO == "1" && Vud.CODVIGENCIA_TRD == vigencia
                             orderby Ser.NOMBRE
                             select new
                             {
                                 Ser.CODSERIE_DOCUMENTAL,
                                 Ser.NOMBRE
                             }).Distinct();
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
        /// <param name="CodSerie"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaSubSeries")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetListaSubSeries(int CodSerie)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var vigencia = (from vig in dbSIM.TBVIGENCIA_TRD orderby vig.D_INICIOVIGENCIA descending select vig.COD_VIGENCIA_TRD).FirstOrDefault();
                if (vigencia <= 0)
                {
                    throw new Exception("No se encontró una vigencia de la TRD!");
                }
                var model = (from Sub in dbSIM.TBSUBSERIE_DOCUMENTAL
                             where Sub.CODSERIE_DOCUMENTAL == CodSerie
                             orderby Sub.NOMBRE
                             select new
                             {
                                 CODSUBSERIE = Sub.CODSUBSERIE_DOCUMENTAL,
                                 Sub.NOMBRE
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
        /// <param name="CodSubSerie"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaUnidades")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetListaUnidades(int CodSubSerie)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var vigencia = (from vig in dbSIM.TBVIGENCIA_TRD orderby vig.D_INICIOVIGENCIA descending select vig.COD_VIGENCIA_TRD).FirstOrDefault();
                if (vigencia <= 0)
                {
                    throw new Exception("No se encontró una vigencia de la TRD!");
                }
                var model = (from Uni in dbSIM.TBSERIE
                             join Vud in dbSIM.TBUNIDADESDOC_VIGENCIATRD on Uni.CODSERIE equals Vud.CODUNIDAD_DOCUMENTAL
                             where Uni.CODSUBSERIE_DOCUMENTAL == CodSubSerie && Uni.S_DEFINEEXPEDIENTE == "1" && Uni.ACTIVO == "1" && Vud.CODVIGENCIA_TRD == vigencia && Uni.S_ADMINMODULO == "0"
                             orderby Uni.NOMBRE
                             select new
                             {
                                 Uni.CODSERIE,
                                 Uni.NOMBRE
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
        /// <param name="IdExpediente"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DetalleExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JObject GetExpediente(string IdExpediente)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                decimal _IdExpediente = -1;
                if (IdExpediente != null && IdExpediente != "") _IdExpediente = decimal.Parse(IdExpediente);

                var Expediente = (from Exp in dbSIM.EXP_EXPEDIENTES
                                  where Exp.ID_EXPEDIENTE == _IdExpediente
                                  select Exp).FirstOrDefault();
                var Tomos = (from Tom in dbSIM.EXP_TOMOS
                             where Tom.ID_EXPEDIENTE == _IdExpediente
                             select Tom).Count();
                var Documentos = (from Tom in dbSIM.EXP_TOMOS
                                  join Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE on Tom.ID_TOMO equals Doc.ID_TOMO
                                  where Tom.ID_EXPEDIENTE == _IdExpediente
                                  select Doc).Count();
                DatosExpediente DatosExp = new DatosExpediente();
                DatosExp.IdExpediente = Expediente.ID_EXPEDIENTE;
                DatosExp.UnidadDoc = (from Uni in dbSIM.TBSERIE where Uni.CODSERIE == Expediente.ID_UNIDADDOC select Uni.NOMBRE).FirstOrDefault();
                DatosExp.Nombre = Expediente.S_NOMBRE;
                DatosExp.Codigo = Expediente.S_CODIGO;
                DatosExp.Descripcion = Expediente.S_DESCRIPCION;
                DatosExp.Responsable = (from Fun in dbSIM.QRY_FUNCIONARIO_ALL where Fun.CODFUNCIONARIO == Expediente.ID_FUNCCREACION select Fun.NOMBRES).FirstOrDefault();
                DatosExp.FechaCrea = Expediente.D_FECHACREACION.Year > 1990 ? Expediente.D_FECHACREACION.ToString("yyyy-MM-dd HH:mm:ss") : "";
                DatosExp.UltEstado = (from ee in dbSIM.EXP_ESTADOSEXPEDIENTE
                                      join te in dbSIM.EXP_TIPOESTADO on ee.ID_ESTADO equals te.ID_ESTADO
                                      where ee.ID_EXPEDIENTE == Expediente.ID_EXPEDIENTE
                                      orderby ee.D_INICIA descending
                                      select te.S_NOMBRE).FirstOrDefault();
                DatosExp.Anulado = Expediente.S_ESTADO == "A" ? "No" : Expediente.S_ESTADO == "N" ? "Si" : "";
                DatosExp.Tomos = Tomos;
                DatosExp.Documentos = Documentos;
                DatosExp.Indices = (from Ind in dbSIM.EXP_INDICES
                                    join Ise in dbSIM.TBINDICESERIE on Ind.CODINDICE equals Ise.CODINDICE
                                    join lista in dbSIM.TBSUBSERIE on (decimal)Ise.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                    from pdis in l.DefaultIfEmpty()
                                    where Ind.ID_EXPEDIENTE == _IdExpediente
                                    orderby Ise.ORDEN
                                    select new Indice
                                    {
                                        CODINDICE = Ind.CODINDICE,
                                        INDICE = Ise.INDICE,
                                        TIPO = Ise.TIPO,
                                        LONGITUD = Ise.LONGITUD,
                                        OBLIGA = Ise.OBLIGA,
                                        VALORDEFECTO = Ise.VALORDEFECTO,
                                        VALOR = Ind.VALOR_FEC != null ? Ind.VALOR_FEC.ToString() : Ind.VALOR_NUM != null ? Ind.VALOR_NUM.ToString() : Ind.VALOR_TXT != null ? Ind.VALOR_TXT.Trim() : "",
                                        ID_LISTA = Ise.CODIGO_SUBSERIE,
                                        TIPO_LISTA = pdis.TIPO,
                                        CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                                    }).ToList();
                return JObject.FromObject(DatosExp, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdExpediente"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EditarExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JObject EditExpediente(int IdExpediente)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdExpediente > 0)
                {

                    DatosExpediente DatosExp = new DatosExpediente();
                    var Expediente = (from Exp in dbSIM.EXP_EXPEDIENTES
                                      where Exp.ID_EXPEDIENTE == IdExpediente
                                      select Exp).FirstOrDefault();
                    DatosExp.IdExpediente = Expediente.ID_EXPEDIENTE;
                    DatosExp.IdSerieDoc = (from Sub in dbSIM.TBSUBSERIE_DOCUMENTAL
                                           join Ser in dbSIM.TBSERIE on Sub.CODSUBSERIE_DOCUMENTAL equals Ser.CODSUBSERIE_DOCUMENTAL
                                           where Ser.CODSERIE == Expediente.ID_UNIDADDOC
                                           select Sub.CODSERIE_DOCUMENTAL).FirstOrDefault().Value;
                    DatosExp.IdSubSerieDoc = dbSIM.TBSERIE.Where(s => s.CODSERIE == Expediente.ID_UNIDADDOC).Select(s => s.CODSUBSERIE_DOCUMENTAL).FirstOrDefault().Value;
                    DatosExp.IdUnidadDoc = Expediente.ID_UNIDADDOC;
                    DatosExp.Nombre = Expediente.S_NOMBRE;
                    DatosExp.Codigo = Expediente.S_CODIGO;
                    DatosExp.Descripcion = Expediente.S_DESCRIPCION;
                    DatosExp.UltEstado = (from ee in dbSIM.EXP_ESTADOSEXPEDIENTE
                                          join te in dbSIM.EXP_TIPOESTADO on ee.ID_ESTADO equals te.ID_ESTADO
                                          where ee.ID_EXPEDIENTE == Expediente.ID_EXPEDIENTE
                                          orderby ee.D_INICIA descending
                                          select te.S_NOMBRE).FirstOrDefault();
                    DatosExp.Anulado = Expediente.S_ESTADO;
                    DatosExp.Indices = (from Ind in dbSIM.EXP_INDICES
                                        join Ise in dbSIM.TBINDICESERIE on Ind.CODINDICE equals Ise.CODINDICE
                                        join lista in dbSIM.TBSUBSERIE on (decimal)Ise.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                        from pdis in l.DefaultIfEmpty()
                                        where Ind.ID_EXPEDIENTE == IdExpediente
                                        orderby Ise.ORDEN
                                        select new Indice
                                        {
                                            CODINDICE = Ind.CODINDICE,
                                            INDICE = Ise.INDICE,
                                            TIPO = Ise.TIPO,
                                            LONGITUD = Ise.LONGITUD,
                                            OBLIGA = Ise.OBLIGA,
                                            VALORDEFECTO = Ise.VALORDEFECTO,
                                            VALOR = Ind.VALOR_FEC != null ? Ind.VALOR_FEC.ToString() : Ind.VALOR_NUM != null ? Ind.VALOR_NUM.ToString() : Ind.VALOR_TXT != null ? Ind.VALOR_TXT.Trim() : "",
                                            ID_LISTA = Ise.CODIGO_SUBSERIE,
                                            TIPO_LISTA = pdis.TIPO,
                                            CAMPO_NOMBRE = pdis.CAMPO_NOMBRE
                                        }).ToList();
                    return JObject.FromObject(DatosExp, Js);
                }
                else throw new Exception("El expediente no se puede editar ya que no se encontró en la base de datos");
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EditarTomo")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JObject GetEditarTomo(long IdTomo)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdTomo > 0)
                {
                    Tomo tomo = new Tomo();
                    var DatoTomo = (from Tom in dbSIM.EXP_TOMOS
                                    where Tom.ID_TOMO == IdTomo
                                    select Tom).FirstOrDefault();
                    tomo.IdTomo = DatoTomo.ID_TOMO;
                    tomo.NumeroTomo = DatoTomo.N_TOMO;
                    tomo.Ubicacion = DatoTomo.S_UBICACION;
                    tomo.Abierto = DatoTomo.S_ABIERTO;
                    tomo.CantFolios = DatoTomo.N_FOLIOS;
                    return JObject.FromObject(tomo, Js);
                }
                else throw new Exception("La carpeta no se puede editar ya que no se encontró en la base de datos");
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Estado"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("InsertaEstadoExp")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object GuardaEstadoExp(EstadoExp Estado)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el estado del expediente" };
            int idUsuario = 0;
            decimal funcionario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                funcionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
            }
            var TipoEst = dbSIM.EXP_TIPOESTADO.Where(w => w.ID_ESTADO == Estado.IdEstado).FirstOrDefault();

            if (TipoEst.S_ESTADOCIERRE == "1")
            {
                var Foliado = (from Tom in dbSIM.EXP_TOMOS
                               join Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE on Tom.ID_TOMO equals Doc.ID_TOMO
                               where Tom.ID_EXPEDIENTE == Estado.IdExpediente && (Doc.N_FOLIOINI <= 0 || Doc.N_FOLIOFIN <= 0)
                               select Doc).Count();
                if (Foliado > 0)
                {
                    return new { resp = "Error", mensaje = "No se puede cerrar archivar el expediente ya que posee documentos sin foliado" };
                }
                var TomCer = (from Tom in dbSIM.EXP_TOMOS
                              where Tom.ID_EXPEDIENTE == Estado.IdExpediente && Tom.S_ABIERTO == "1"
                              select Tom).Count();
                if (TomCer > 0)
                {
                    return new { resp = "Error", mensaje = "No se puede cerrar archivar el expediente ya que posee carpetas sin cerrar" };
                }
            }
            var EstFin = dbSIM.EXP_ESTADOSEXPEDIENTE.Where(w => w.ID_EXPEDIENTE == Estado.IdExpediente && w.D_FIN == null).OrderByDescending(o => o.D_INICIA).FirstOrDefault();
            EstFin.D_FIN = Estado.FechaIni;
            dbSIM.Entry(EstFin).State = EntityState.Modified;
            dbSIM.SaveChanges();
            EXP_ESTADOSEXPEDIENTE _Estado = new EXP_ESTADOSEXPEDIENTE();
            _Estado.ID_ESTADO = Estado.IdEstado;
            _Estado.ID_EXPEDIENTE = Estado.IdExpediente;
            _Estado.ID_FUNCIONARIOESTADO = funcionario;
            _Estado.D_INICIA = Estado.FechaIni;
            dbSIM.EXP_ESTADOSEXPEDIENTE.Add(_Estado);
            dbSIM.SaveChanges();

            return new { resp = "OK", mensaje = "Estado del expediente creado correctamente" };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object GuardaExpediente(DatosExpediente objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el expediente" };
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
                if (objData.Indices != null)
                {
                    foreach (Indice indice in objData.Indices)
                    {
                        if (indice.OBLIGA == 1 && (indice.VALOR == null || indice.VALOR == ""))
                        {
                            return new { resp = "Error", mensaje = "Indice " + indice.INDICE + " es obligatorio y no se ingresó un valor!!" };
                        }
                        if (objData.IdExpediente > 1)
                        {
                            EXP_INDICES indiceDoc = dbSIM.EXP_INDICES.Where(i => i.ID_EXPEDIENTE == objData.IdExpediente && i.CODINDICE == indice.CODINDICE).FirstOrDefault();
                            if (indiceDoc != null)
                            {
                                switch (indice.TIPO)
                                {
                                    case 0: //Texto
                                    case 3:
                                    case 4:
                                    case 5:
                                    case 8:
                                        indiceDoc.VALOR_TXT = indice.VALOR ?? "";
                                        break;
                                    case 1: //Numero
                                    case 6:
                                    case 7:
                                        indiceDoc.VALOR_NUM = decimal.Parse(indice.VALOR);
                                        break;
                                    case 2: //Fecha
                                        indiceDoc.VALOR_FEC = DateTime.Parse(indice.VALOR);
                                        break;
                                }
                                dbSIM.Entry(indiceDoc).State = EntityState.Modified;
                                dbSIM.SaveChanges();
                            }
                            else
                            {
                                indiceDoc = new EXP_INDICES();

                                indiceDoc.ID_EXPEDIENTE = objData.IdExpediente;
                                indiceDoc.CODINDICE = indice.CODINDICE;
                                switch (indice.TIPO)
                                {
                                    case 0: //Texto
                                    case 3:
                                    case 4:
                                    case 5:
                                    case 8:
                                        indiceDoc.VALOR_TXT = indice.VALOR ?? "";
                                        break;
                                    case 1: //Numero
                                    case 6:
                                    case 7:
                                        indiceDoc.VALOR_NUM = decimal.Parse(indice.VALOR);
                                        break;
                                    case 2: //Fecha
                                        indiceDoc.VALOR_FEC = DateTime.Parse(indice.VALOR);
                                        break;
                                }

                                dbSIM.Entry(indiceDoc).State = EntityState.Added;
                                dbSIM.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var indiceObliga = (from ind in dbSIM.TBINDICESERIE
                                        where ind.CODSERIE == objData.IdUnidadDoc && ind.OBLIGA == 1
                                        select ind).FirstOrDefault();
                    if (indiceObliga != null)
                    {
                        return new { resp = "Error", mensaje = "No se han ingresado indices y el tipo de expediente tiene indices obligatorios!!" };
                    }
                }
                if (objData.IdExpediente > 0)
                {
                    var Expediente = (from Exp in dbSIM.EXP_EXPEDIENTES
                                      where Exp.ID_EXPEDIENTE == objData.IdExpediente
                                      select Exp).FirstOrDefault();
                    if (Expediente != null)
                    {
                        Expediente.S_NOMBRE = objData.Nombre;
                        Expediente.S_CODIGO = objData.Codigo;
                        Expediente.S_DESCRIPCION = objData.Descripcion;
                        Expediente.S_ESTADO = objData.Anulado;
                        dbSIM.Entry(Expediente).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
                else if (objData.IdExpediente <= 0)
                {
                    //dbSIM.Database.Log = s => MyLogger.Log("EFApp", s);
                    EXP_EXPEDIENTES Expediente = new EXP_EXPEDIENTES();
                    Expediente.ID_UNIDADDOC = objData.IdUnidadDoc;
                    Expediente.S_NOMBRE = objData.Nombre;
                    Expediente.S_CODIGO = objData.Codigo;
                    Expediente.S_DESCRIPCION = objData.Descripcion;
                    Expediente.D_FECHACREACION = DateTime.Now;
                    Expediente.ID_FUNCCREACION = funcionario;
                    Expediente.S_ESTADO = "A";
                    dbSIM.EXP_EXPEDIENTES.Add(Expediente);
                    dbSIM.SaveChanges();
                    EXP_TOMOS Tomo = new EXP_TOMOS();
                    Tomo.ID_EXPEDIENTE = Expediente.ID_EXPEDIENTE;
                    Tomo.N_TOMO = 1;
                    Tomo.S_ABIERTO = "1";
                    Tomo.S_UBICACION = "Acervo - Archivo";
                    Tomo.ID_FUNCCREACION = funcionario;
                    Tomo.D_FECHACREACION = DateTime.Now;
                    Tomo.N_FOLIOS = 200;
                    dbSIM.Entry(Tomo).State = EntityState.Added;
                    dbSIM.SaveChanges();
                    if (objData.Indices != null)
                    {
                        foreach (Indice indice in objData.Indices)
                        {
                            if (indice.VALOR != null && indice.VALOR.Length > 0)
                            {
                                EXP_INDICES indiceDoc = new EXP_INDICES();

                                indiceDoc.ID_EXPEDIENTE = Expediente.ID_EXPEDIENTE;
                                indiceDoc.CODINDICE = indice.CODINDICE;
                                switch (indice.TIPO)
                                {
                                    case 0: //Texto
                                    case 3:
                                    case 4:
                                    case 5:
                                    case 8:
                                        indiceDoc.VALOR_TXT = indice.VALOR ?? "";
                                        break;
                                    case 1: //Numero
                                    case 6:
                                    case 7:
                                        indiceDoc.VALOR_NUM = decimal.Parse(indice.VALOR);
                                        break;
                                    case 2: //Fecha
                                        indiceDoc.VALOR_FEC = DateTime.Parse(indice.VALOR);
                                        break;
                                }
                                dbSIM.Entry(indiceDoc).State = EntityState.Added;
                                dbSIM.SaveChanges();
                            }
                        }
                    }
                    EXP_ESTADOSEXPEDIENTE Estado = new EXP_ESTADOSEXPEDIENTE();
                    Estado.ID_EXPEDIENTE = Expediente.ID_EXPEDIENTE;
                    Estado.D_INICIA = Expediente.D_FECHACREACION;
                    Estado.ID_ESTADO = dbSIM.EXP_TIPOESTADO.Where(w => w.S_ESTADOINICIAL == "1").Select(s => s.ID_ESTADO).FirstOrDefault();
                    Estado.ID_FUNCIONARIOESTADO = Expediente.ID_FUNCCREACION;
                    dbSIM.EXP_ESTADOSEXPEDIENTE.Add(Estado);
                    dbSIM.SaveChanges();
                    string _Sql = "INSERT INTO TRAMITES.EXP_BUSQUEDA (COD_SERIE,S_INDICE,FECHADIGITALIZA,ID_EXPEDIENTE) SELECT EXP.ID_UNIDADDOC AS COD_SERIE, ('EXPEDIENTE: ' || EXP.S_CODIGO || ' - ' || 'EXPEDIENTE: ' || UPPER(EXP.S_NOMBRE) || ' - ' || (SELECT LISTAGG(IND.INDICE || ': ' || CASE WHEN IDO.VALOR_TXT IS NOT NULL THEN IDO.VALOR_TXT WHEN IDO.VALOR_NUM IS NOT NULL THEN TO_CHAR(IDO.VALOR_NUM) WHEN IDO.VALOR_FEC IS NOT NULL THEN TO_CHAR(IDO.VALOR_FEC, 'DD-MM-YYYY') END, ' - ') WITHIN GROUP(ORDER BY IDO.ID_EXPEDIENTE) AS valor FROM TRAMITES.EXP_INDICES IDO INNER JOIN TRAMITES.TBINDICESERIE IND ON IDO.CODINDICE = IND.CODINDICE WHERE IDO.ID_EXPEDIENTE = EXP.ID_EXPEDIENTE GROUP BY IDO.ID_EXPEDIENTE)) AS S_INDICE,EXP.D_FECHACREACION AS FECHADIGITALIZA,EXP.ID_EXPEDIENTE FROM TRAMITES.EXP_EXPEDIENTES EXP WHERE EXP.ID_EXPEDIENTE= " + Expediente.ID_EXPEDIENTE;
                    dbSIM.Database.ExecuteSqlCommand(_Sql);
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente creado correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaTomo")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object GuardaTomo(Tomo objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el tomo" };
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
                if (objData.IdTomo > 0)
                {
                    var Tomo = (from Tom in dbSIM.EXP_TOMOS
                                where Tom.ID_TOMO == objData.IdTomo
                                select Tom).FirstOrDefault();
                    if (Tomo != null)
                    {
                        Tomo.S_UBICACION = objData.Ubicacion;
                        Tomo.N_TOMO = objData.NumeroTomo;
                        Tomo.N_FOLIOS = objData.CantFolios;
                        if (Tomo.S_ABIERTO != objData.Abierto && objData.Abierto == "1")
                        {
                            var Foliado = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                           where Doc.ID_TOMO == objData.IdTomo && (Doc.N_FOLIOINI <= 0 || Doc.N_FOLIOFIN <= 0)
                                           select Doc).Count();
                            if (Foliado > 0)
                            {
                                return new { resp = "Error", mensaje = "No se puede cerrar el tomo ya que posee documentos sin foliado" };
                            }
                            Tomo.S_ABIERTO = objData.Abierto;
                        }
                        dbSIM.SaveChanges();
                    }
                }
                else if (objData.IdTomo <= 0)
                {
                    var Abiertos = (from Exp in dbSIM.EXP_TOMOS
                                    where Exp.ID_EXPEDIENTE == objData.IdExpediente && Exp.S_ABIERTO == "1"
                                    select Exp).ToList();
                    if (Abiertos != null && Abiertos.Count > 0)
                    {
                        return new { resp = "Error", mensaje = "No se puede crear una nueva carpeta hasta que todas las anteriores estén cerradas" };
                        //    foreach(var TomoAb in Abiertos)
                        //    {
                        //        TomoAb.S_ABIERTO = "0";
                        //    }
                        //    dbSIM.SaveChanges();
                    }

                    EXP_TOMOS Tomo = new EXP_TOMOS();
                    Tomo.S_UBICACION = objData.Ubicacion;
                    Tomo.N_TOMO = objData.NumeroTomo;
                    Tomo.N_FOLIOS = objData.CantFolios;
                    Tomo.S_ABIERTO = "1";
                    Tomo.D_FECHACREACION = DateTime.Now;
                    Tomo.ID_FUNCCREACION = funcionario;
                    Tomo.ID_EXPEDIENTE = objData.IdExpediente;
                    dbSIM.EXP_TOMOS.Add(Tomo);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente creado correctamente" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdExpediente"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EliminaExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object EliminaExpediente(int IdExpediente)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el expediente" };
            try
            {
                if (IdExpediente > 0)
                {
                    var Documentos = (from Tom in dbSIM.EXP_TOMOS
                                      join Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE on Tom.ID_TOMO equals Doc.ID_TOMO
                                      where Tom.ID_EXPEDIENTE == IdExpediente
                                      select Doc).Count();
                    if (Documentos > 0) return new { resp = "Error", mensaje = "El expediente no se puede eliminar ya que posee documentos" };
                    dbSIM.EXP_INDICES.RemoveRange(dbSIM.EXP_INDICES.Where(i => i.ID_EXPEDIENTE == IdExpediente));
                    dbSIM.SaveChanges();
                    dbSIM.EXP_TOMOS.RemoveRange(dbSIM.EXP_TOMOS.Where(t => t.ID_EXPEDIENTE == IdExpediente));
                    dbSIM.SaveChanges();
                    dbSIM.EXP_ESTADOSEXPEDIENTE.RemoveRange(dbSIM.EXP_ESTADOSEXPEDIENTE.Where(e => e.ID_EXPEDIENTE == IdExpediente));
                    dbSIM.SaveChanges();
                    var Busqueda = (from Bus in dbSIM.EXP_BUSQUEDA
                                    where Bus.ID_EXPEDIENTE == IdExpediente
                                    select Bus).FirstOrDefault();
                    if (Busqueda != null) dbSIM.EXP_BUSQUEDA.Remove(Busqueda);
                    dbSIM.SaveChanges();
                    var Expediente = (from Exp in dbSIM.EXP_EXPEDIENTES
                                      where Exp.ID_EXPEDIENTE == IdExpediente
                                      select Exp).FirstOrDefault();
                    dbSIM.EXP_EXPEDIENTES.Remove(Expediente);
                    dbSIM.SaveChanges();

                }
                else return new { resp = "Error", mensaje = "El expediente no se puede elimiar ya que no se encontró en la base de datos" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente eliminado correctamente!!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EliminaTomo")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object EliminaTomo(int IdTomo)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la carpeta" };
            try
            {
                if (IdTomo > 0)
                {
                    var Documentos = (from Tom in dbSIM.EXP_TOMOS
                                      join Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE on Tom.ID_TOMO equals Doc.ID_TOMO
                                      where Tom.ID_TOMO == IdTomo
                                      select Doc).Count();
                    if (Documentos > 0) return new { resp = "Error", mensaje = "La carpeta no se puede eliminar ya que posee documentos" };
                    dbSIM.EXP_TOMOS.Remove(dbSIM.EXP_TOMOS.Where(t => t.ID_TOMO == IdTomo).FirstOrDefault());
                    dbSIM.SaveChanges();
                }
                else return new { resp = "Error", mensaje = "La carpeta no se puede elimiar ya que no se encontró en la base de datos" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente eliminado correctamente!!" };
        }

        /// <summary>
        /// Retrona los indices de un documento especifico
        /// </summary>
        /// <param name="IdDocumento">Identificador del documento</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("IndicesDocumento")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray IndicesDoc(long IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdDocumento <= 0) return null;
                var _CodTramite = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.ID_DOCUMENTO == IdDocumento).Select(s => s.CODTRAMITE).FirstOrDefault();
                var _CodDdoc = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.ID_DOCUMENTO == IdDocumento).Select(s => s.CODDOCUMENTO).FirstOrDefault();
                var Indices = (from Ind in dbSIM.TBINDICEDOCUMENTO
                               where Ind.CODTRAMITE == _CodTramite && Ind.CODDOCUMENTO == _CodDdoc
                               select new
                               {
                                   INDICE = dbSIM.TBINDICESERIE.Where(I => I.CODINDICE == Ind.CODINDICE).Select(s => s.INDICE).FirstOrDefault(),
                                   Ind.VALOR
                               }).ToList();
                return JArray.FromObject(Indices, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdExp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("IndicesExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray IndicesExpediente(long IdExp)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdExp <= 0) return null;
                var Indices = (from Ind in dbSIM.EXP_INDICES
                               where Ind.ID_EXPEDIENTE == IdExp
                               select new
                               {
                                   INDICE = dbSIM.TBINDICESERIE.Where(I => I.CODINDICE == Ind.CODINDICE).Select(s => s.INDICE).FirstOrDefault(),
                                   VALOR = Ind.VALOR_FEC != null ? Ind.VALOR_FEC.ToString() : Ind.VALOR_NUM != null ? Ind.VALOR_NUM.ToString() : Ind.VALOR_TXT != null ? Ind.VALOR_TXT.Trim() : "",
                               }).ToList();
                return JArray.FromObject(Indices, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AsociaDocumento")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public async Task<object> AsociaDocumento(decimal IdDocumento, decimal IdTomo)
        {
            decimal codFuncionario = -1;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error asociando el documento" };
            try
            {
                if (IdTomo > 0)
                {
                    var Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                      where Doc.ID_TOMO == IdTomo && Doc.ID_DOCUMENTO == IdDocumento
                                      select Doc).Count();
                    if (Documentos > 0) return new { resp = "Error", mensaje = "El documento ya se encuentra asociado a la carpeta" };
                    var Expe = dbSIM.EXP_TOMOS.Where(e => e.ID_TOMO == IdTomo).Select(s => s.ID_EXPEDIENTE).FirstOrDefault();
                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                  join Tom in dbSIM.EXP_TOMOS on Doc.ID_TOMO equals Tom.ID_TOMO
                                  where Tom.ID_EXPEDIENTE == Expe && Doc.ID_DOCUMENTO == IdDocumento
                                  select Doc).Count();
                    if (Documentos > 0) return new { resp = "Error", mensaje = "El documento ya se encuentra asociado al expediente" };
                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                  where Doc.ID_TOMO == IdTomo
                                  select Doc).Count();
                    decimal _UltimoFolio = 0;
                    int _UltOrden = 0;
                    if (Documentos > 0)
                    {
                        var FolioMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                        where Fol.ID_TOMO == IdTomo
                                        orderby Fol.N_ORDEN
                                        select Fol.N_FOLIOFIN).Max();
                        if (FolioMax > 0) _UltimoFolio = FolioMax.Value;
                        var OrdenMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                        where Fol.ID_TOMO == IdTomo
                                        orderby Fol.N_ORDEN
                                        select Fol.N_ORDEN).Max();
                        if (OrdenMax > 0) _UltOrden = OrdenMax;
                    }

                    try
                    {
                        MemoryStream _DocPdf = await Utilidades.Archivos.AbrirDocumento((long)IdDocumento);
                        PDFDocument Origen = new PDFDocument(_DocPdf);
                        Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                        int _Paginas = Origen.Pages.Count();
                        int _PagFol = (int)Math.Ceiling((decimal)Origen.Pages.Count() / 2);
                        Origen.Dispose();

                        EXP_DOCUMENTOSEXPEDIENTE _Doc = new EXP_DOCUMENTOSEXPEDIENTE();
                        _Doc.ID_DOCUMENTO = IdDocumento;
                        _Doc.ID_TOMO = IdTomo;
                        _Doc.N_ORDEN = _UltOrden + 1;
                        _Doc.N_FOLIOINI = _UltimoFolio + 1;
                        _Doc.N_FOLIOFIN = _Doc.N_FOLIOINI + _PagFol;
                        _Doc.N_IMAGENES = _Paginas;
                        _Doc.ID_FUNCASOCIA = codFuncionario;
                        _Doc.D_FECHA = DateTime.Now;
                        dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Add(_Doc);
                        dbSIM.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return new { resp = "Error", mensaje = "Ocurrió un error abriendo el documento. " + ex.Message };
                    }
                }
                else return new { resp = "Error", mensaje = "No se ha seleccionado una carpeta para adicionar el documento" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente eliminado correctamente!!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListaIdDocumentos"></param>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AsociaDocumento")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public async Task<object> AsociaDocumento(string ListaIdDocumentos, decimal IdTomo)
        {
            decimal codFuncionario = -1;
            List<decimal> IdDocsOrd = new List<decimal>();
            List<string> arrIdDocs = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string[]>(ListaIdDocumentos).ToList();
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error asociando el documento" };
            try
            {
                if (IdTomo > 0)
                {
                    if (arrIdDocs.Count > 0)
                    {
                        var Docs = dbSIM.TBTRAMITEDOCUMENTO.Where(w => arrIdDocs.Contains(w.ID_DOCUMENTO.ToString())).OrderBy(o => o.FECHACREACION).Select(s => s.ID_DOCUMENTO).ToList();
                        if (Docs.Count > 0)
                        {
                            foreach (var idDoc in Docs)
                            {
                                decimal IdDocumento = idDoc;
                                if (IdDocumento > 0)
                                {
                                    var Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                      where Doc.ID_TOMO == IdTomo && Doc.ID_DOCUMENTO == IdDocumento
                                                      select Doc).Count();
                                    if (Documentos > 0) return new { resp = "Error", mensaje = $"El documento {IdDocumento} ya se encuentra asociado a la carpeta" };
                                    var Expe = dbSIM.EXP_TOMOS.Where(e => e.ID_TOMO == IdTomo).Select(s => s.ID_EXPEDIENTE).FirstOrDefault();
                                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                  join Tom in dbSIM.EXP_TOMOS on Doc.ID_TOMO equals Tom.ID_TOMO
                                                  where Tom.ID_EXPEDIENTE == Expe && Doc.ID_DOCUMENTO == IdDocumento
                                                  select Doc).Count();
                                    if (Documentos > 0) return new { resp = "Error", mensaje = $"El documento {IdDocumento} ya se encuentra asociado al expediente" };
                                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                  where Doc.ID_TOMO == IdTomo
                                                  select Doc).Count();
                                    decimal _UltimoFolio = 0;
                                    int _UltOrden = 0;
                                    if (Documentos > 0)
                                    {
                                        var FolioMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                        where Fol.ID_TOMO == IdTomo
                                                        orderby Fol.N_ORDEN
                                                        select Fol.N_FOLIOFIN).Max();
                                        if (FolioMax > 0) _UltimoFolio = FolioMax.Value;
                                        var OrdenMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                        where Fol.ID_TOMO == IdTomo
                                                        orderby Fol.N_ORDEN
                                                        select Fol.N_ORDEN).Max();
                                        if (OrdenMax > 0) _UltOrden = OrdenMax;
                                    }
                                    try
                                    {
                                        MemoryStream _DocPdf = await Utilidades.Archivos.AbrirDocumento((long)IdDocumento);
                                        PDFDocument Origen = new PDFDocument(_DocPdf);
                                        Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                                        int _Paginas = Origen.Pages.Count();
                                        int _PagFol = (int)Math.Ceiling((decimal)Origen.Pages.Count() / 2);
                                        Origen.Dispose();

                                        EXP_DOCUMENTOSEXPEDIENTE _Doc = new EXP_DOCUMENTOSEXPEDIENTE();
                                        _Doc.ID_DOCUMENTO = IdDocumento;
                                        _Doc.ID_TOMO = IdTomo;
                                        _Doc.N_ORDEN = _UltOrden + 1;
                                        _Doc.N_FOLIOINI = _UltimoFolio + 1;
                                        _Doc.N_FOLIOFIN = _Doc.N_FOLIOINI + _PagFol;
                                        _Doc.N_IMAGENES = _Paginas;
                                        _Doc.ID_FUNCASOCIA = codFuncionario;
                                        _Doc.D_FECHA = DateTime.Now;
                                        dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Add(_Doc);
                                        dbSIM.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        return new { resp = "Error", mensaje = $"Ocurrió un error abriendo el documento {IdDocumento}. " + ex.Message };
                                    }
                                }
                            }
                        }
                    }
                    else return new { resp = "Error", mensaje = "No se ha seleccionado documentos para adicionar a la carpeta!!" };
                }
                else return new { resp = "Error", mensaje = "No se ha seleccionado una carpeta para adicionar los documentos!!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente eliminado correctamente!!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListaIdDocumentos"></param>
        /// <param name="IdExp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AsociaDocumento")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public async Task<object> AsociaDocumento(string ListaIdDocumentos, string IdExp)
        {
            decimal codFuncionario = -1;
            decimal IdExpediente = decimal.Parse(IdExp);
            List<string> arrIdDocs = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string[]>(ListaIdDocumentos).ToList();
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                int idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = clsGenerales.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error asociando el documento" };
            try
            {
                var IdTomo = dbSIM.EXP_TOMOS.Where(w => w.ID_EXPEDIENTE.Equals(IdExpediente) && w.S_ABIERTO == "1").Select(s => s.ID_TOMO).FirstOrDefault();
                if (IdTomo > 0)
                {
                    if (arrIdDocs.Count > 0)
                    {
                        var Docs = dbSIM.TBTRAMITEDOCUMENTO.Where(w => arrIdDocs.Contains(w.ID_DOCUMENTO.ToString())).OrderBy(o => o.FECHACREACION).Select(s => s.ID_DOCUMENTO).ToList();
                        if (Docs.Count > 0)
                        {
                            foreach (var idDoc in Docs)
                            {
                                decimal IdDocumento = idDoc;
                                if (IdDocumento > 0)
                                {
                                    var Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                      where Doc.ID_TOMO == IdTomo && Doc.ID_DOCUMENTO == IdDocumento
                                                      select Doc).Count();
                                    if (Documentos > 0) return new { resp = "Error", mensaje = $"El documento {IdDocumento} ya se encuentra asociado a la carpeta" };
                                    var Expe = dbSIM.EXP_TOMOS.Where(e => e.ID_TOMO == IdTomo).Select(s => s.ID_EXPEDIENTE).FirstOrDefault();
                                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                  join Tom in dbSIM.EXP_TOMOS on Doc.ID_TOMO equals Tom.ID_TOMO
                                                  where Tom.ID_EXPEDIENTE == Expe && Doc.ID_DOCUMENTO == IdDocumento
                                                  select Doc).Count();
                                    if (Documentos > 0) return new { resp = "Error", mensaje = $"El documento {IdDocumento} ya se encuentra asociado al expediente" };
                                    Documentos = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                  where Doc.ID_TOMO == IdTomo
                                                  select Doc).Count();
                                    decimal _UltimoFolio = 0;
                                    int _UltOrden = 0;
                                    if (Documentos > 0)
                                    {
                                        var FolioMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                        where Fol.ID_TOMO == IdTomo
                                                        orderby Fol.N_ORDEN
                                                        select Fol.N_FOLIOFIN).Max();
                                        if (FolioMax > 0) _UltimoFolio = FolioMax.Value;
                                        var OrdenMax = (from Fol in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                                        where Fol.ID_TOMO == IdTomo
                                                        orderby Fol.N_ORDEN
                                                        select Fol.N_ORDEN).Max();
                                        if (OrdenMax > 0) _UltOrden = OrdenMax;
                                    }
                                    try
                                    {
                                        MemoryStream _DocPdf = await Utilidades.Archivos.AbrirDocumento((long)IdDocumento);
                                        PDFDocument Origen = new PDFDocument(_DocPdf);
                                        Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                                        int _Paginas = Origen.Pages.Count();
                                        int _PagFol = (int)Math.Ceiling((decimal)Origen.Pages.Count() / 2);
                                        Origen.Dispose();

                                        EXP_DOCUMENTOSEXPEDIENTE _Doc = new EXP_DOCUMENTOSEXPEDIENTE();
                                        _Doc.ID_DOCUMENTO = IdDocumento;
                                        _Doc.ID_TOMO = IdTomo;
                                        _Doc.N_ORDEN = _UltOrden + 1;
                                        _Doc.N_FOLIOINI = _UltimoFolio + 1;
                                        _Doc.N_FOLIOFIN = _Doc.N_FOLIOINI + _PagFol;
                                        _Doc.N_IMAGENES = _Paginas;
                                        _Doc.ID_FUNCASOCIA = codFuncionario;
                                        _Doc.D_FECHA = DateTime.Now;
                                        dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Add(_Doc);
                                        dbSIM.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        return new { resp = "Error", mensaje = $"Ocurrió un error abriendo el documento {IdDocumento}. " + ex.Message };
                                    }
                                }
                            }
                        }
                    }
                    else return new { resp = "Error", mensaje = "No se ha seleccionado documentos para adicionar a la carpeta!!" };
                }
                else return new { resp = "Error", mensaje = "No se encontró una carpeta abierta para adicionar los documentos!!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Expediente: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Expediente eliminado correctamente!!" };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("CerrarTomo")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object CerrarTomo(int IdTomo)
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
            if (IdTomo <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de carpeta para cerrar!!" };
            var Foliados = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                            where Doc.ID_TOMO == IdTomo && (Doc.N_FOLIOINI == null || Doc.N_FOLIOFIN == null)
                            select Doc).Count();
            if (Foliados > 0) return new { resp = "Error", mensaje = "La carpeta posee documentos sin foliado, se cierra cuando el foliado este correcto!!" };
            decimal IdExp = dbSIM.EXP_TOMOS.Where(T => T.ID_TOMO == IdTomo).Select(s => s.ID_EXPEDIENTE).FirstOrDefault();
            var Tomos = (from Tom in dbSIM.EXP_TOMOS
                         where Tom.ID_EXPEDIENTE == IdExp
                         orderby Tom.N_TOMO
                         select new
                         {
                             Tom.ID_TOMO,
                             Tom.N_TOMO,
                             Inicia = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(t => t.ID_TOMO == Tom.ID_TOMO).Select(d => d.N_FOLIOINI).Min(),
                             Final = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(t => t.ID_TOMO == Tom.ID_TOMO).Select(d => d.N_FOLIOFIN).Min(),
                             Tom.S_ABIERTO
                         }).ToList();
            decimal _Final = 0;
            foreach (var Tom in Tomos)
            {
                if (Tom.Inicia != (_Final + 1) && Tom.Inicia != null && Tom.S_ABIERTO == "0") return new { resp = "Error", mensaje = "El foliado entre carpetas esta mal establecido, se cierra cuando el foliado este correcto!!" };
                _Final = Tom.Final != null ? Tom.Final.Value : 0;
            }
            var DocFol = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                          where Doc.ID_TOMO == IdTomo
                          orderby Doc.N_ORDEN
                          select Doc).ToList();
            decimal _Anterior = 0;
            bool _primero = true;
            foreach (EXP_DOCUMENTOSEXPEDIENTE doc in DocFol)
            {
                if (doc.N_FOLIOINI > doc.N_FOLIOFIN) return new { resp = "Error", mensaje = "El documento " + doc.N_ORDEN + " de la carpeta no esta bien foliado, por favor corrijalo, se cierra cuando el foliado este correcto!!" };
                if (!_primero)
                {
                    if (doc.N_FOLIOINI != (_Anterior + 1)) return new { resp = "Error", mensaje = "El foliado entre documentos esta mal establecido, se cierra cuando el foliado este correcto!!" };
                }
                _Anterior = doc.N_FOLIOFIN.Value;
                _primero = false;
            }
            try
            {
                var Carpeta = (from Tom in dbSIM.EXP_TOMOS where Tom.ID_TOMO == IdTomo select Tom).FirstOrDefault();
                Carpeta.S_ABIERTO = "0";
                Carpeta.D_FECHACIERRE = DateTime.Now;
                Carpeta.ID_FUNCCIERRA = funcionario;
                string _Clave = IdTomo.ToString() + IdExp.ToString() + Tomos.Where(t => t.ID_TOMO == IdTomo).Select(s => s.N_TOMO).ToString() + Tomos.Where(t => t.ID_TOMO == IdTomo).Select(s => s.Inicia).ToString() + Tomos.Where(t => t.ID_TOMO == IdTomo).Select(s => s.Final).ToString() + Carpeta.D_FECHACIERRE.ToString();
                Carpeta.S_HASH = Utilidades.Cryptografia.GetStringSha256Hash(_Clave);
                dbSIM.Entry(Carpeta).State = EntityState.Modified;
                dbSIM.SaveChanges();
                return new { resp = "Ok", mensaje = "Se cerró la carpeta correctamente!!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el cierre de la carpeta: " + e.Message };
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneFolioDoc")]
        public JObject ObtieneFolioDoc(int IdTomo, decimal IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdTomo > 0 && IdDocumento > 0)
                {
                    var Docu = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                where Doc.ID_DOCUMENTO == IdDocumento && Doc.ID_TOMO == IdTomo
                                select new
                                {
                                    OrdenDoc = Doc.N_ORDEN,
                                    FolioIni = Doc.N_FOLIOINI,
                                    FolioFin = Doc.N_FOLIOFIN,
                                    Imagenes = Doc.N_IMAGENES
                                }).FirstOrDefault();
                    return JObject.FromObject(Docu, Js);
                }
                else
                {
                    throw new Exception("No se ha ingresado alguno de los datos para obtener datos del documento!!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTomo"></param>
        /// <param name="IdDocumento"></param>
        /// <param name="Orden"></param>
        /// <param name="FolioIni"></param>
        /// <param name="FolioFin"></param>
        /// <param name="Imagenes"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("FoliarDocumento")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public object FoliarDocumento(int IdTomo, decimal IdDocumento, int Orden, int FolioIni, int FolioFin, int Imagenes)
        {
            int idUsuario = 0;
            decimal funcionario = 0;
            bool _Accion = false;
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
            if (FolioIni > FolioFin) return new { resp = "Error", mensaje = "El rango de folios inical y final esta mal establecido!!" };
            if (IdTomo <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de carpeta para foliar!!" };
            if (IdDocumento <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de documento para foliar!!" };
            decimal IdExp = dbSIM.EXP_TOMOS.Where(T => T.ID_TOMO == IdTomo).Select(s => s.ID_EXPEDIENTE).FirstOrDefault();
            try
            {
                var DocOld = (from d in dbSIM.EXP_DOCUMENTOSEXPEDIENTE where d.ID_DOCUMENTO == IdDocumento && d.ID_TOMO == IdTomo select d).FirstOrDefault();
                var MinOrden = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).Select(s => s.N_ORDEN).Min();
                var MaxOrden = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).Select(s => s.N_ORDEN).Max();
                if (Orden < MinOrden || Orden > MaxOrden) return new { resp = "Error", mensaje = "El orden ingresado para el documento esta mal establecido!" };
                if (DocOld.N_ORDEN != Orden) _Accion = true;
                if (DocOld.N_FOLIOINI != FolioIni || DocOld.N_FOLIOFIN != FolioFin) _Accion = true;
                if (_Accion)
                {
                    var Doc = (from doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                               where doc.ID_DOCUMENTO == IdDocumento && doc.ID_TOMO == IdTomo
                               select doc).FirstOrDefault();
                    Doc.N_ORDEN = Orden;
                    Doc.N_FOLIOINI = FolioIni;
                    Doc.N_FOLIOFIN = FolioFin;
                    dbSIM.Entry(Doc).State = EntityState.Modified;
                    dbSIM.SaveChanges();
                    var Docs = (from D in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                where D.ID_TOMO == IdTomo
                                orderby D.N_ORDEN
                                select D).ToList();
                    decimal _FolioFin = -1;
                    decimal _Orden = -1;
                    bool _primero = true;
                    foreach (EXP_DOCUMENTOSEXPEDIENTE _doc in Docs)
                    {
                        if (_doc.N_FOLIOFIN != null && _doc.N_FOLIOINI != null)
                        {
                            if (!_primero)
                            {
                                decimal _Folios = _doc.N_FOLIOFIN.Value - _doc.N_FOLIOINI.Value;
                                _doc.N_ORDEN = (int)(_Orden + 1);
                                _doc.N_FOLIOINI = _FolioFin + 1;
                                _doc.N_FOLIOFIN = _doc.N_FOLIOINI + _Folios;
                                dbSIM.Entry(_doc).State = EntityState.Modified;
                                dbSIM.SaveChanges();
                            }
                            _primero = false;
                            _FolioFin = _doc.N_FOLIOFIN.Value;
                            _Orden = _doc.N_ORDEN;
                        }
                    }
                    return new { resp = "Ok", mensaje = "Se modificó correctamnete el foliado del documento!" };
                }
                return new { resp = "Error", mensaje = "No fué necesrio modificar el foliado del documento!" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el foliado del documento: " + ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdExp"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("NombreExpediente")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public string NombreExpediente(decimal IdExp)
        {
            if (IdExp <= 0) return "";
            var NomExp = dbSIM.EXP_EXPEDIENTES.Where(w => w.ID_EXPEDIENTE == IdExp).Select(s => s.S_NOMBRE).FirstOrDefault();
            return NomExp;
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ArbolExpediente")]
        public JArray GetDatosArbolExpediente(decimal IdExp)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var nodoRaiz = "[{ ID: '0', NOMBRE: '" + dbSIM.EXP_EXPEDIENTES.Where(w => w.ID_EXPEDIENTE == IdExp).Select(s => s.S_NOMBRE).FirstOrDefault() + "', icon: 'product'}]";
            try
            {
                if (IdExp <= 0) return (JArray)JsonConvert.DeserializeObject(nodoRaiz);
                nodoRaiz = "[{ ID: '0', NOMBRE: '" + dbSIM.EXP_EXPEDIENTES.Where(w => w.ID_EXPEDIENTE == IdExp).Select(s => s.S_NOMBRE).FirstOrDefault() + "', icon: 'product', expanded: true},";
                var Carpetas = (from Ind in dbSIM.EXP_TOMOS
                                where Ind.ID_EXPEDIENTE == IdExp
                                orderby Ind.N_TOMO
                                select new
                                {
                                    ID = Ind.ID_TOMO,
                                    NOMBRE = "Carpeta " + Ind.N_TOMO.ToString()
                                }).ToList();
                if (Carpetas != null)
                {
                    nodoRaiz += "{ ID: '0.1', NOMBRE: 'Carpetas', PADRE: '0', icon: 'folder'},";
                    foreach (var Carp in Carpetas)
                    {
                        nodoRaiz += "{ ID: '0.1." + Carp.ID + "', NOMBRE: '" + Carp.NOMBRE + "' , PADRE: '0.1', icon: 'folder', TOMO: true},";
                        string _Sql = "SELECT DOC.CODSERIE,(SELECT SER.NOMBRE FROM TRAMITES.TBSERIE SER WHERE SER.CODSERIE = DOC.CODSERIE) AS SERIE,DEX.ID_TOMO FROM TRAMITES.EXP_DOCUMENTOSEXPEDIENTE DEX INNER JOIN TRAMITES.TBTRAMITEDOCUMENTO DOC ON DOC.ID_DOCUMENTO = DEX.ID_DOCUMENTO WHERE DEX.ID_TOMO=" + Carp.ID + " GROUP BY DOC.CODSERIE,DEX.ID_TOMO";
                        var Series = dbSIM.Database.SqlQuery<Series>(_Sql).ToList();
                        if (Series.Count > 0)
                        {
                            foreach (var ser in Series)
                            {
                                nodoRaiz += "{ ID: '0.1." + Carp.ID + "." + ser.CODSERIE + "', NOMBRE: '" + ser.SERIE + "', PADRE: '0.1." + Carp.ID + "', icon: 'unselectall', DOCS: true },";
                            }
                        }
                    }
                }
                nodoRaiz += "{ ID: '0.2', NOMBRE: 'Anexos', PADRE: '0', icon: 'event'},";
                nodoRaiz = nodoRaiz.Substring(0, nodoRaiz.Length - 1);
                nodoRaiz += "]";
                return (JArray)JsonConvert.DeserializeObject(nodoRaiz);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdUniDoc"></param>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocs")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetDatosObtieneDocs(long IdUniDoc, long IdTomo)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdTomo <= 0) return null;
                if (IdUniDoc <= 0) return null;
                var Docs = (from DocExp in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                            join Doc in dbSIM.TBTRAMITEDOCUMENTO on DocExp.ID_DOCUMENTO equals Doc.ID_DOCUMENTO
                            where DocExp.ID_TOMO == IdTomo && Doc.CODSERIE == IdUniDoc
                            orderby DocExp.N_ORDEN
                            select new
                            {
                                Documento = Doc.ID_DOCUMENTO,
                                Datos = dbSIM.BUSQUEDA_DOCUMENTO.Where(w => w.ID_DOCUMENTO == Doc.ID_DOCUMENTO).Select(s => s.S_INDICE).FirstOrDefault(),
                                Fecha = Doc.FECHACREACION
                            });
                var ListaDocumentos = Docs.ToList();
                return JArray.FromObject(ListaDocumentos, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTomo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocs")]
        [Authorize(Roles = "VEXPEDIENTES")]
        public JArray GetDatosObtieneDocs(long IdTomo)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdTomo <= 0) return null;
                var Docs = (from DocExp in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                            join Doc in dbSIM.TBTRAMITEDOCUMENTO on DocExp.ID_DOCUMENTO equals Doc.ID_DOCUMENTO
                            where DocExp.ID_TOMO == IdTomo
                            orderby DocExp.N_ORDEN
                            select new
                            {
                                Documento = Doc.ID_DOCUMENTO,
                                Datos = dbSIM.BUSQUEDA_DOCUMENTO.Where(w => w.ID_DOCUMENTO == Doc.ID_DOCUMENTO).Select(s => s.S_INDICE).FirstOrDefault(),
                                Fecha = Doc.FECHACREACION.ToString()
                            });
                var ListaDocumentos = Docs.ToList();
                return JArray.FromObject(ListaDocumentos, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesSerieDocumental")]
        public dynamic GetObtenerIndicesSerieDocumental(int codSerie)
        {
            var indicesSerieDocumental = from i in dbSIM.TBINDICESERIE
                                         join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                                         from pdis in l.DefaultIfEmpty()
                                         where i.CODSERIE == codSerie && i.MOSTRAR == "1" && i.INDICE_RADICADO == null
                                         orderby i.ORDEN
                                         select new Indice
                                         {
                                             CODINDICE = i.CODINDICE,
                                             INDICE = i.INDICE,
                                             TIPO = i.TIPO,
                                             LONGITUD = i.LONGITUD,
                                             OBLIGA = i.OBLIGA,
                                             VALORDEFECTO = i.VALORDEFECTO,
                                             VALOR = "",
                                             ID_LISTA = i.CODIGO_SUBSERIE,
                                             TIPO_LISTA = pdis.TIPO,
                                             CAMPO_NOMBRE = pdis.CAMPO_NOMBRE,
                                             MAXIMO = i.VALORMAXIMO.Length > 0 ? i.VALORMAXIMO : "",
                                             MINIMO = i.VALORMINIMO.Length > 0 ? i.VALORMINIMO : ""
                                         };
            var listaInd = indicesSerieDocumental.ToList();
            return listaInd;
        }

        [Authorize]
        [HttpGet, ActionName("ModificaOrdenDoc")]
        public object GetModificaOrdenDoc(int IdTomo, decimal IdDocumento, int NuevoOrden)
        {
            int idUsuario = 0;
            decimal funcionario = 0;
            bool _Accion = false;
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
            if (IdTomo <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de carpeta para foliar!!" };
            if (IdDocumento <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de documento para foliar!!" };
            try
            {
                var DocOld = (from d in dbSIM.EXP_DOCUMENTOSEXPEDIENTE where d.ID_DOCUMENTO == IdDocumento && d.ID_TOMO == IdTomo select d).FirstOrDefault();
                var MinOrden = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).Select(s => s.N_ORDEN).Min();
                var MaxOrden = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).Select(s => s.N_ORDEN).Max();
                if (NuevoOrden < MinOrden || NuevoOrden > MaxOrden) return new { resp = "Error", mensaje = "El orden ingresado para el documento esta mal establecido!" };
                if (DocOld.N_ORDEN != NuevoOrden) _Accion = true;
                if (_Accion)
                {
                    var Docs = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).OrderBy(o => o.N_ORDEN).ToList();
                    int DocAct = DocOld.N_ORDEN;
                    int _Aux = NuevoOrden;

                    foreach (EXP_DOCUMENTOSEXPEDIENTE _doc in Docs)
                    {
                        if (_doc.N_ORDEN == _Aux)
                        {
                            if (_doc.N_ORDEN == DocAct)
                            {
                                _doc.N_ORDEN = NuevoOrden;
                            }
                            else
                            {
                                _Aux++;
                                _doc.N_ORDEN = _Aux;
                            }
                            dbSIM.Entry(_doc).State = EntityState.Modified;
                            dbSIM.SaveChanges();
                        }
                        else if (_doc.N_ORDEN == DocAct)
                        {
                            _doc.N_ORDEN = NuevoOrden;
                            _Aux++;
                            dbSIM.Entry(_doc).State = EntityState.Modified;
                            dbSIM.SaveChanges();
                        }
                        else if (_doc.N_ORDEN == NuevoOrden)
                        {
                            _doc.N_ORDEN = DocAct;
                            dbSIM.Entry(_doc).State = EntityState.Modified;
                            dbSIM.SaveChanges();
                        }
                        else if (_doc.N_ORDEN == (DocAct + 1))
                        {
                            _doc.N_ORDEN = DocAct;
                            dbSIM.Entry(_doc).State = EntityState.Modified;
                            dbSIM.SaveChanges();
                        }
                    }
                    if (ReordenarTomo(IdTomo)) return new { resp = "Ok", mensaje = "Se modificó correctamnete el orden de los documentos!" };
                }
                return new { resp = "Error", mensaje = "No fué necesario modificar el foliado del documento!" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el foliado del documento: " + ex.Message };
            }
        }


        [Authorize]
        [HttpGet, ActionName("DesasociaDoc")]
        public object GetDesasociaDoc(int IdTomo, decimal IdDocumento)
        {
            if (IdTomo <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de carpeta para desasociar el documento!!" };
            if (IdDocumento <= 0) return new { resp = "Error", mensaje = "No se ingresó un número de documento para desasociar!!" };
            try
            {
                var _docMover = IdDocumento;
                var _TomoMover = IdTomo;
                var _docEliminar = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_DOCUMENTO == IdDocumento && w.ID_TOMO == IdTomo).FirstOrDefault();
                if (_docEliminar != null)
                {
                    dbSIM.Entry(_docEliminar).State = EntityState.Deleted;
                    dbSIM.SaveChanges();
                }
                else return new { resp = "Error", mensaje = "No se encontró el documento para desasociar!!" };
                if (ReordenarTomo(IdTomo)) return new { resp = "Ok", mensaje = "Se desasoció correctamnete el documento y se reordenó la carpeta!" };
                else return new { resp = "Error", mensaje = "Error al reordenar la carpeta!" };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Error al desasociar el documento: " + ex.Message };
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneOrdenDoc")]
        public JObject GetObtieneOrdenDoc(int IdTomo, decimal IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (IdTomo > 0 && IdDocumento > 0)
                {
                    var Docu = (from Doc in dbSIM.EXP_DOCUMENTOSEXPEDIENTE
                                where Doc.ID_DOCUMENTO == IdDocumento && Doc.ID_TOMO == IdTomo
                                select new
                                {
                                    OrdenDoc = Doc.N_ORDEN,
                                    MaxOrden = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).Select(s => s.N_ORDEN).Max()
                                }).FirstOrDefault();
                    return JObject.FromObject(Docu, Js);
                }
                else
                {
                    throw new Exception("No se ha ingresado alguno de los datos para obtener datos del documento!!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ReordenarTomo(int IdTomo)
        {
            bool _resp = false;
            try
            {
                var Docs = dbSIM.EXP_DOCUMENTOSEXPEDIENTE.Where(w => w.ID_TOMO == IdTomo).OrderBy(o => o.N_ORDEN).ToList();
                int Cont = 1;
                EXP_DOCUMENTOSEXPEDIENTE _Aux = null;
                foreach (EXP_DOCUMENTOSEXPEDIENTE _doc in Docs)
                {
                    if (_doc.N_ORDEN != Cont)
                    {
                        _doc.N_ORDEN = Cont;
                        if (_Aux != null) _doc.N_FOLIOINI = _Aux.N_FOLIOFIN.Value + 1;
                        else _doc.N_FOLIOINI = 1;
                        _doc.N_FOLIOFIN = _doc.N_FOLIOINI + _doc.N_IMAGENES;
                        dbSIM.Entry(_doc).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                        _Aux = _doc;
                    }
                    else
                    {
                        if (_Aux != null) _doc.N_FOLIOINI = _Aux.N_FOLIOFIN.Value + 1;
                        else _doc.N_FOLIOINI = 1;
                        _doc.N_FOLIOFIN = _doc.N_FOLIOINI + _doc.N_IMAGENES;
                        dbSIM.Entry(_doc).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                        _Aux = _doc;
                    }
                    Cont++;
                }
                _resp = true;
            }
            catch
            {
                _resp = false;
            }
            return _resp;
        }
    }


    public class DatosExpediente
    {
        public Decimal IdExpediente { get; set; }
        public Decimal IdSerieDoc { get; set; }
        public Decimal IdSubSerieDoc { get; set; }
        public Decimal IdUnidadDoc { get; set; }
        public string UnidadDoc { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string FechaCrea { get; set; }
        public string Responsable { get; set; }
        public string Anulado { get; set; }
        public string UltEstado { get; set; }
        public int Tomos { get; set; }
        public int Documentos { get; set; }
        public List<Indice> Indices { get; set; }
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
        public string MAXIMO { get; set; }
        public string MINIMO { get; set; }
    }
    public class Tomo
    {
        public decimal IdTomo { get; set; }
        public decimal IdExpediente { get; set; }
        public decimal NumeroTomo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal IdFuncCreaION { get; set; }
        public string Ubicacion { get; set; }
        public string Abierto { get; set; }
        public int CantFolios { get; set; }
    }
    public class EstadoExp
    {
        public decimal IdExpediente { get; set; }
        public decimal IdEstado { get; set; }
        public DateTime FechaIni { get; set; }
    }
    public class Series
    {
        public int CODSERIE { get; set; }
        public string SERIE { get; set; }
    }
}

