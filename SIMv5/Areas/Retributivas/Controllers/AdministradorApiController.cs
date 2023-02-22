
namespace SIM.Areas.Retributivas.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.Retributivas.Models;
    using System.Security.Claims;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using SIM.Data.Agua;
    using SIM.Data;

    //[Authorize]
    public class AdministradorApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        [Authorize(Roles = "VADMINISTRADOR")]

        /// <summary>
        /// TRIBUTARY
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Tributary")]
        public datosConsulta Tributary(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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

                modelData = (from trn in dbSIM.TSIMTASA_CUENCAS
                             join P in dbSIM.MUNICIPIOS on trn.ID_MUNICIPIO equals P.ID_MUNI
                             join T in dbSIM.TSIMTASA_TRAMOS on trn.TSIMTASA_TRAMOS_ID equals T.ID
                             join C in dbSIM.TSIMTASA_TIPO_CUENCAS on trn.TSIMTASA_TIPO_CUENCAS_ID equals C.ID
                             
                             select new
                             {
                                 ID= trn.ID,
                                 CODIGO =trn.CODIGO,
                                 NOMBRE = trn.NOMBRE,
                                 AREA = trn.AREA,
                                 CAUDAL = trn.CAUDAL,
                                 TSIMTASA_TIPO_CUENCAS_ID= trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 TIPO= C.NOMBRE,
                                 LONGITUD = trn.LONGITUD,
                                 ID_MUNICIPIO = trn.ID_MUNICIPIO,
                                 TSIMTASA_TRAMOS_ID = trn.TSIMTASA_TRAMOS_ID,
                                 TRAMO = T.NOMBRE,
                                 MUNICIPIO =P.NOMBRE,
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveTributary")]
        public object GetRemoveTributary(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_CUENCAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_CUENCAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadTributary")]
        public JObject loadTributary(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_CUENCAS
                             where trn.ID == Id
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.NOMBRE,
                                 trn.CODIGO,
                                 trn.AREA,
                                 trn.CAUDAL,
                                 trn.LONGITUD,
                                 trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 trn.ID_MUNICIPIO,
                                 trn.TSIMTASA_TRAMOS_ID

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertTributary")]
        [HttpPost]
        public object InsertTributary(TSIMTASA_CUENCAS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_CUENCAS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.CODIGO = objData.CODIGO;
                            _turnUpdate.NOMBRE = objData.NOMBRE;
                            _turnUpdate.AREA = objData.AREA;
                            _turnUpdate.CAUDAL = objData.CAUDAL;
                            _turnUpdate.LONGITUD = objData.LONGITUD;
                            _turnUpdate.TSIMTASA_TIPO_CUENCAS_ID = objData.TSIMTASA_TIPO_CUENCAS_ID;
                            _turnUpdate.ID_MUNICIPIO = objData.ID_MUNICIPIO;
                            _turnUpdate.TSIMTASA_TRAMOS_ID = objData.TSIMTASA_TRAMOS_ID;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_CUENCAS _newTurn = new TSIMTASA_CUENCAS
                        {
                            NOMBRE = objData.NOMBRE,
                            CODIGO = objData.CODIGO,
                            AREA = objData.AREA,
                            CAUDAL = objData.CAUDAL,
                            LONGITUD = objData.LONGITUD,
                            TSIMTASA_TIPO_CUENCAS_ID = objData.TSIMTASA_TIPO_CUENCAS_ID,
                            ID_MUNICIPIO = objData.ID_MUNICIPIO,
                            TSIMTASA_TRAMOS_ID = objData.TSIMTASA_TRAMOS_ID,
                        };
                        dbSIM.TSIMTASA_CUENCAS.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error almacenando el cuerpo de Agua: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Cuerpo de agua ingresado con exito" };
            }
        }




        // ************************************************************************************



        /// <summary>
        /// Shedding
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Shedding")]
        public datosConsulta Shedding(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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

                modelData = (from trn in dbSIM.TSIMTASA_CUENCAS_TERCERO
                             orderby trn.ID
                             select new
                             {
                                 trn.ID,
                                 trn.NICK,
                                 trn.ID_INSTALACION,
                                 trn.ID_TERCERO,
                                 trn.LATITUD,
                                 trn.LONGITUD,
                                 trn.NO_RESOLUCION,
                                 trn.TIPO_AGUA_RESIDUAL,
                                 trn.TIPO_DESCARGA,
                                 trn.TSIMTASA_CUENCAS_ID1,
                                 trn.FECHA_RESOLUCION,
                                 trn.CAUDAL_AUTORIZADO,
                                 trn.ANOS_VIGENCIA
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [Authorize(Roles = "EADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveShedding")]
        public object GetRemoveShedding(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_CUENCAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_CUENCAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadShedding")]
        public JObject loadShedding(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_CUENCAS
                             where trn.ID == Id
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.NOMBRE,
                                 trn.CODIGO,
                                 trn.AREA,
                                 trn.CAUDAL,
                                 trn.LONGITUD,
                                 trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 trn.ID_MUNICIPIO

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertShedding")]
        [HttpPost]
        public object InsertShedding(TSIMTASA_CUENCAS_TERCERO objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_CUENCAS_TERCERO.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.TIPO_DESCARGA = objData.TIPO_DESCARGA;
                            _turnUpdate.TIPO_AGUA_RESIDUAL = objData.TIPO_AGUA_RESIDUAL;
                            _turnUpdate.NO_RESOLUCION = objData.NO_RESOLUCION;
                            _turnUpdate.FECHA_RESOLUCION = objData.FECHA_RESOLUCION;
                            _turnUpdate.ANOS_VIGENCIA = objData.ANOS_VIGENCIA;
                            _turnUpdate.LATITUD = objData.LATITUD;
                            _turnUpdate.LONGITUD = objData.LONGITUD;
                            _turnUpdate.CAUDAL_AUTORIZADO = objData.CAUDAL_AUTORIZADO;
                            _turnUpdate.ID_INSTALACION = objData.ID_INSTALACION;
                            _turnUpdate.TSIMTASA_CUENCAS_ID1 = objData.TSIMTASA_CUENCAS_ID1;
                            _turnUpdate.NICK = objData.NICK;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_CUENCAS_TERCERO _newTurn = new TSIMTASA_CUENCAS_TERCERO
                        {
                            TIPO_DESCARGA = objData.TIPO_DESCARGA,
                            TIPO_AGUA_RESIDUAL = objData.TIPO_AGUA_RESIDUAL,
                            NO_RESOLUCION = objData.NO_RESOLUCION,
                            FECHA_RESOLUCION = objData.FECHA_RESOLUCION,
                            ANOS_VIGENCIA = objData.ANOS_VIGENCIA,
                            LONGITUD = objData.LONGITUD,
                            LATITUD = objData.LATITUD,
                            CAUDAL_AUTORIZADO = objData.CAUDAL_AUTORIZADO,
                            ID_TERCERO = objData.ID_TERCERO,
                            ID_INSTALACION = objData.ID_INSTALACION,
                            TSIMTASA_CUENCAS_ID1 = objData.TSIMTASA_CUENCAS_ID1,
                            NICK = objData.NICK,
                        };
                        dbSIM.TSIMTASA_CUENCAS_TERCERO.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }



        // ************************************************************************************
        // **************************** FACTOR REGIONAL *********************************************
        // ************************************************************************************

        /// <summary>
        /// QUINQUENO
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getRegionalFactor")]
        public datosConsulta RegionalFactor(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from R in dbSIM.TSIMTASA_FACTOR_REGIONAL
                             join P in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on R.PARAMETROS_AMBIENTAL_ID equals P.ID
                             join Pe in dbSIM.TSIMTASA_PERIODO on R.TSIMTASA_PERIODOS_ID equals Pe.ID
                             join Q in dbSIM.TSIMTASA_QUINQUENO on Pe.TSIMTASA_QUINQUENO_ID equals Q.ID
                             join T in dbSIM.TSIMTASA_TRAMOS on R.TSIMTASA_TRAMOS_ID equals T.ID
                             select new
                             {
                                 ID_FACTOR_REGIONAL = R.ID,
                                 R.ANO,
                                 R.FACTOR,      
                                 R.CARGA_OBTENIDA,
                                 R.RESOLUCION,
                                 R.CUMPLE_META,
                                 R.TSIMTASA_TRAMOS_ID,
                                 T.DESCRIPCION,
                                 NOMBRE_TRAMO = T.NOMBRE,
                                 R.PARAMETROS_AMBIENTAL_ID,
                                 R.TSIMTASA_PERIODOS_ID,
                                 P.NOMBRE,
                                 P.ABREVIATURA,
                                 PERIODO = "Periodo: " + Pe.NO_PERIODO + " Quinqueño: " + Q.DESCRIPCION,
                             });

                Console.WriteLine(modelData);

                try
                {
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                    resultado.numRegistros = modelFiltered.Count();
                    if (skip == 0 && take == 0)
                    {
                        resultado.datos = modelFiltered.ToArray();
                    }
                    else
                    {
                        resultado.datos = modelFiltered.Take(take).ToList();
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.InnerException.Message);

                }


                return resultado;

            }

        }


        // ************************************************************************************
        // **************************** FACTOR REGIONAL simple *********************************************
        // ************************************************************************************


        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getRegionalFactorSingle")]
        [Authorize(Roles = "VADMINISTRADOR")]
        public JArray RegionalFactorSingle()
        {
                JsonSerializer Js = new JsonSerializer();
                Js = JsonSerializer.CreateDefault();
                try
                {
                 var model = (from R in dbSIM.TSIMTASA_FACTOR_REGIONAL
                             select new
                             {
                                 ID_FACTOR_REGIONAL = R.ID,
                                 R.ANO,
                                 R.FACTOR,
                                 R.CARGA_OBTENIDA,
                                 R.RESOLUCION,
                                 R.CUMPLE_META,
                                 R.TSIMTASA_TRAMOS_ID,
                                 R.PARAMETROS_AMBIENTAL_ID,
                                 R.TSIMTASA_PERIODOS_ID,
                             });

                    return JArray.FromObject(model, Js);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
        }



        // ************************************************************************************
        // ****************************calcular Carga Obenida *********************************************
        // ************************************************************************************

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("getCargaObtenida")]
        public JObject cargaObtenida(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var carga = 0;
            try
            {
                if (Id <= 0) return null;


                var fr = (from R in dbSIM.TSIMTASA_FACTOR_REGIONAL
                          join P in dbSIM.TSIMTASA_PERIODO on R.TSIMTASA_PERIODOS_ID equals P.NO_PERIODO
                          where R.ID == Id
                          select new
                          {
                              ID = R.ID,
                              periodo = P.NO_PERIODO,
                              parametro = R.PARAMETROS_AMBIENTAL_ID,
                              fechaIicio = P.INICIA,
                              FechaFin = P.TERMINA,
                              periodoID = P.ID,
                              tramo = R.TSIMTASA_TRAMOS_ID,
                              carga = R.CARGA_OBTENIDA
                          }).FirstOrDefault();


                var reportes1 = (from R in dbSIM.TSIMTASA_REPORTES
                                join C in dbSIM.TSIMTASA_CUENCAS on R.TSIMTASA_CUENCAS_TERCERO_ID equals C.ID
                                where C.TSIMTASA_TRAMOS_ID == fr.tramo
                                where R.ANO == fr.fechaIicio.Value.Year
                                && R.MES >= fr.fechaIicio.Value.Month
                                && C.TSIMTASA_TRAMOS_ID == fr.tramo
                                select R).ToList();

                var reportes2 = (from R in dbSIM.TSIMTASA_REPORTES
                                join C in dbSIM.TSIMTASA_CUENCAS on R.TSIMTASA_CUENCAS_TERCERO_ID equals C.ID
                                where C.TSIMTASA_TRAMOS_ID == fr.tramo
                                where R.ANO == fr.FechaFin.Value.Year
                                && R.MES <= fr.FechaFin.Value.Month
                                && C.TSIMTASA_TRAMOS_ID == fr.tramo
                                select R).ToList();



                var metaUpdate = (from MIUpdate in dbSIM.TSIMTASA_FACTOR_REGIONAL
                                  where MIUpdate.ID == Id
                                  select MIUpdate).First();


                if (fr.parametro == 1)
                {
                    var cargaAcumuladaSST = reportes1.Sum(x => x.REPORTE_SST);
                     cargaAcumuladaSST = cargaAcumuladaSST + reportes2.Sum(x => x.REPORTE_SST);

                    //fr.CARGA_OBTENIDA = decimal.ToDouble(cargaAcumuladaSST) / 1000000;
                    metaUpdate.CARGA_OBTENIDA = decimal.ToDouble(cargaAcumuladaSST);

                }
                else
                {
                    var cargaAcumuladaDBO = reportes1.Sum(x => x.REPORTE_DBO);
                     cargaAcumuladaDBO = cargaAcumuladaDBO + reportes1.Sum(x => x.REPORTE_DBO);
                    //fr.CARGA_OBTENIDA = decimal.ToDouble(cargaAcumuladaDBO) / 1000000;
                    metaUpdate.CARGA_OBTENIDA = decimal.ToDouble(cargaAcumuladaDBO);

                }

                    dbSIM.SaveChanges();


                return JObject.FromObject(fr, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        // ************************************************************************************
        // ****************************calcular Carga Obenida individual *********************************************
        // ************************************************************************************

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("getCargaObtenidaMI")]
        public JObject cargaObtenidaMI(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var carga = 0;
            try
            {
                if (Id <= 0) return null;


                var metas = (from MI in dbSIM.TSIMTASA_METAS_INDIVIDUALES
                             join P in dbSIM.TSIMTASA_PERIODO on MI.TSIMTASA_PERIODO_ID equals P.NO_PERIODO
                             where MI.ID == Id
                             select new
                             {
                                 ID = MI.ID,
                                 tercero = MI.ID_TERCERO,
                                 periodoMI = MI.TSIMTASA_PERIODO_ID,
                                 parametro = MI.TSIMTASA_PARAMETROS_AMBIENTAL_ID,
                                 fechaIicio = P.INICIA,
                                 FechaFin = P.TERMINA,
                                 periodoID = P.ID,
                             }).FirstOrDefault();



                    var reportes1 = (from Report in dbSIM.TSIMTASA_REPORTES
                                    join c in dbSIM.TSIMTASA_CUENCAS_TERCERO on Report.TSIMTASA_CUENCAS_TERCERO_ID equals c.ID
                                    where
                                    Report.ANO == metas.fechaIicio.Value.Year
                                    && Report.MES >= metas.fechaIicio.Value.Month
                                    && c.ID_TERCERO == metas.tercero
                                    select Report).ToList();

                var reportes2 = (from Report in dbSIM.TSIMTASA_REPORTES
                                join c in dbSIM.TSIMTASA_CUENCAS_TERCERO on Report.TSIMTASA_CUENCAS_TERCERO_ID equals c.ID
                                where
                                Report.ANO == metas.FechaFin.Value.Year
                                && Report.MES <= metas.FechaFin.Value.Month
                                && c.ID_TERCERO == metas.tercero
                                select Report).ToList();

                var metaUpdate = (from MIUpdate in dbSIM.TSIMTASA_METAS_INDIVIDUALES
                                      where MIUpdate.ID == Id
                                      select MIUpdate).First();
                 

                    if (metas.parametro == 1)
                    {
                        var cargaAcumuladaSST = reportes1.Sum(x => x.REPORTE_SST);
                         cargaAcumuladaSST = cargaAcumuladaSST + reportes2.Sum(x => x.REPORTE_SST);
                        //la carga viene en mg,

                        metaUpdate.CARGA_OBTENIDA = cargaAcumuladaSST;

                    }
                    else
                    {
                        var cargaAcumuladaDBO = reportes1.Sum(x => x.REPORTE_DBO);
                         cargaAcumuladaDBO = cargaAcumuladaDBO + reportes2.Sum(x => x.REPORTE_DBO);
                        metaUpdate.CARGA_OBTENIDA = cargaAcumuladaDBO;

                    }
             

                    dbSIM.SaveChanges();
                    return JObject.FromObject(metaUpdate, Js);
                


                
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




        // ************************************************************************************
        // **************************** AJUSTAR ACTOR REGIONAL simple *********************************************
        // ************************************************************************************

        [Authorize(Roles = "AADMINISTRADOR")]
        [HttpGet, ActionName("getAjustarFR")]
        public JObject AjustarFR(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var ajustador = 0;
            try
            {
                if (Id <= 0) return null;


                var fr = (from R in dbSIM.TSIMTASA_FACTOR_REGIONAL
                             join mi in dbSIM.TSIMTASA_METAS_GRUPALES on R.TSIMTASA_PERIODOS_ID equals mi.PERIODO
                             where R.ID == Id
                             where R.PARAMETROS_AMBIENTAL_ID == mi.PARAMETRO
                             select R).FirstOrDefault();


                var pe = (from R in dbSIM.TSIMTASA_METAS_GRUPALES
                             join mi in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.PERIODO equals mi.TSIMTASA_PERIODOS_ID
                             where mi.ID == Id
                             where R.PARAMETRO == mi.PARAMETROS_AMBIENTAL_ID
                          select R).FirstOrDefault();

                if (fr.CARGA_OBTENIDA > decimal.ToDouble(pe.META))
                {

                    fr.CUMPLE_META = "NO CUMPLE";
                    var nuevo = decimal.ToDouble(fr.FACTOR) + (fr.CARGA_OBTENIDA / decimal.ToDouble(pe.META));
                    if (nuevo > 5.5d)
                    {
                        fr.FACTOR = 5.5m;
                    }
                    else
                    {
                        fr.FACTOR = (Decimal)nuevo;
                    }

                
                }
                else
                {
                    fr.CUMPLE_META = "CUMPLE";
                };

                dbSIM.SaveChanges();

                return JObject.FromObject(fr, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        // ************************************************************************************
        // **************************** AJUSTAR fACTOR REGIONAL META INDIVIDUAL *********************************************
        // ************************************************************************************

        [Authorize(Roles = "AADMINISTRADOR")]
        [HttpGet, ActionName("calcularFR")]
        public JObject calcularFR(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            var ajustador = 0;
            try
            {
                if (Id <= 0) return null;


                var mi = (from R in dbSIM.TSIMTASA_METAS_INDIVIDUALES
                             where R.ID == Id
                             select R).FirstOrDefault();
                
                
                var fr = (from R in dbSIM.TSIMTASA_FACTOR_REGIONAL
                             join mg in dbSIM.TSIMTASA_METAS_GRUPALES on R.TSIMTASA_PERIODOS_ID equals mg.PERIODO
                             where R.PARAMETROS_AMBIENTAL_ID == mi.TSIMTASA_PARAMETROS_AMBIENTAL_ID
                             select R).FirstOrDefault();


                //var pe = (from R in dbSIM.TSIMTASA_METAS_GRUPALES
                //             join mi in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.PERIODO equals mi.TSIMTASA_PERIODOS_ID
                //             where mi.ID == Id
                //             where R.PARAMETRO == mi.PARAMETROS_AMBIENTAL_ID
                //          select R).FirstOrDefault();

                if (mi.CARGA_OBTENIDA > mi.META)
                {

                    mi.CUMPLE_META = "NO CUMPLE";
                    var nuevo = (mi.FACTOR_REGIONAL) + (mi.CARGA_OBTENIDA / (mi.META));
                    if (decimal.ToDouble(nuevo) > 5.5d)
                    {
                        mi.FACTOR_REGIONAL = 5.5m;
                    }
                    else
                    {
                        mi.FACTOR_REGIONAL = (Decimal)nuevo;
                    }

                
                }
                else
                {
                    mi.CUMPLE_META = "CUMPLE";
                };

                dbSIM.SaveChanges();

                return JObject.FromObject(mi, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




        // ************************************************************************************
        // **************************** Metas GRUPALES *********************************************
        // ************************************************************************************

        /// <summary>
        /// METAS GRUPALES
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getGroupsGoal")]
        public datosConsulta GroupsGoal(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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


                modelData = (from R in dbSIM.TSIMTASA_METAS_GRUPALES
                             join A in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on R.PARAMETRO equals A.ID

                             select new modelMetasGrupales
                             {
                                 ID_META_GRUPAL = R.ID,
                                 META = R.META,
                                 PERIODO = R.PERIODO,
                                 PARAMETRO_AMBIENTAL_ID = R.PARAMETRO,
                                 PARAMETRO_AMBIENTAL = A.NOMBRE,
                                 TSIMTASA_TRAMO_ID = R.TSIMTASA_TRAMO_ID,
                             });


                try
                {
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                    resultado.numRegistros = modelFiltered.Count();
                    if (skip == 0 && take == 0)
                    {
                        resultado.datos = modelFiltered.ToArray();
                    }
                    else
                    {
                        resultado.datos = modelFiltered.Take(take).ToList();
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.InnerException.Message);

                }


                return resultado;

            }

        }

        [Authorize(Roles = "EADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveMI")]
        public object GetRemoveMI(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_METAS_INDIVIDUALES.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_METAS_INDIVIDUALES.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: META Eliminada Satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        // ************************************************************************************
        // **************************** Insert META Individuales *********************************************
        // ************************************************************************************
        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertMI")]
        [HttpPost]
        public object InsertMI(modelMetasIndividuales objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID_META_INDIVIDUAL;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_METAS_INDIVIDUALES.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {

                            _turnUpdate.META = objData.META;
                            _turnUpdate.CARGA_OBTENIDA = objData.CARGA_OBTENIDA;
                            _turnUpdate.CUMPLE_META = objData.CUMPLE_META;
                            _turnUpdate.ID_TERCERO = objData.ID_TERCERO;
                            _turnUpdate.TSIMTASA_PERIODO_ID = objData.PERIODO_ID;
                            _turnUpdate.TSIMTASA_PARAMETROS_AMBIENTAL_ID = objData.PARAMETRO_AMBIENTAL_ID;
                            _turnUpdate.FACTOR_REGIONAL = objData.FACTOR_REGIONAL_ID;
                            

                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_METAS_INDIVIDUALES _newMI = new TSIMTASA_METAS_INDIVIDUALES
                        {
                            ID = objData.ID_META_INDIVIDUAL,
                            META = objData.META,
                            CARGA_OBTENIDA = objData.CARGA_OBTENIDA,
                            CUMPLE_META = objData.CUMPLE_META,
                            ID_TERCERO = objData.ID_TERCERO,
                            TSIMTASA_PERIODO_ID = objData.PERIODO_ID,
                            TSIMTASA_PARAMETROS_AMBIENTAL_ID = objData.PARAMETRO_AMBIENTAL_ID,
                            FACTOR_REGIONAL = objData.FACTOR_REGIONAL_ID,
                        };
                        dbSIM.TSIMTASA_METAS_INDIVIDUALES.Add(_newMI);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadMI")]
        public JObject loadMI(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            dynamic modelData;
            try
            {
                if (Id <= 0) return null;

                modelData = (from R in dbSIM.TSIMTASA_METAS_INDIVIDUALES
                             join P in dbSIM.TSIMTASA_PERIODO on R.TSIMTASA_PERIODO_ID equals P.ID
                             join Q in dbSIM.TSIMTASA_QUINQUENO on P.TSIMTASA_QUINQUENO_ID equals Q.ID
                             join A in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on R.TSIMTASA_PARAMETROS_AMBIENTAL_ID equals A.ID
                             //join F in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.TSIMTASA_FACTOR_REGIONAL_ID equals F.ID
                             join T in dbSIM.TERCERO on R.ID_TERCERO equals T.ID_TERCERO

                             orderby R.ID
                             where R.ID == Id

                             select new modelMetasIndividuales
                             {
                                 ID_META_INDIVIDUAL = R.ID,
                                 META = R.META,
                                 CARGA_OBTENIDA = R.CARGA_OBTENIDA,
                                 CUMPLE_META = R.CUMPLE_META,
                                 ID_TERCERO = R.ID_TERCERO,
                                 NAME_TERCERO = T.S_RSOCIAL,
                                 PERIODO_ID = R.TSIMTASA_PERIODO_ID,
                                 PERIODO = "Periodo: " + P.NO_PERIODO + " Quinqueño: " + Q.DESCRIPCION,
                                 PARAMETRO_AMBIENTAL_ID = R.TSIMTASA_PARAMETROS_AMBIENTAL_ID,
                                 PARAMETRO_AMBIENTAL = A.NOMBRE,
                                 PARAMETRO_AMBIENTAL_ABREV = A.ABREVIATURA,
                                 FACTOR_REGIONAL_ID = R.FACTOR_REGIONAL,
                                 FACTOR_REGIONAL = R.FACTOR_REGIONAL,
                             }).FirstOrDefault();

                                return JObject.FromObject(modelData, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        // ************************************************************************************
        // **************************** Metas Individuales *********************************************
        // ************************************************************************************

        /// <summary>
        /// QUINQUENO
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getGoalIndividual")]
        public datosConsulta goalIndividual(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from R in dbSIM.TSIMTASA_METAS_INDIVIDUALES
                             join P in dbSIM.TSIMTASA_PERIODO on R.TSIMTASA_PERIODO_ID equals P.ID
                             join Q in dbSIM.TSIMTASA_QUINQUENO on P.TSIMTASA_QUINQUENO_ID equals Q.ID
                             join A in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on R.TSIMTASA_PARAMETROS_AMBIENTAL_ID equals A.ID
                             //join F in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.TSIMTASA_FACTOR_REGIONAL_ID equals F.ID
                             join T in dbSIM.TERCERO on R.ID_TERCERO equals T.ID_TERCERO


                             select new modelMetasIndividuales
                             {
                                 ID_META_INDIVIDUAL = R.ID,
                                 META = R.META,
                                 CARGA_OBTENIDA  = R.CARGA_OBTENIDA,
                                 CUMPLE_META = R.CUMPLE_META,
                                 ID_TERCERO =R.ID_TERCERO,
                                 NAME_TERCERO = T.S_RSOCIAL,
                                 PERIODO_ID = R.TSIMTASA_PERIODO_ID,
                                 PERIODO = "Periodo: " + P.NO_PERIODO + " Quinqueño: " + Q.DESCRIPCION,
                                 PARAMETRO_AMBIENTAL_ID = R.TSIMTASA_PARAMETROS_AMBIENTAL_ID,
                                 PARAMETRO_AMBIENTAL = A.NOMBRE,
                                 PARAMETRO_AMBIENTAL_ABREV = A.ABREVIATURA,
                                 FACTOR_REGIONAL_ID=R.FACTOR_REGIONAL,
                                 FACTOR_REGIONAL= R.FACTOR_REGIONAL,
                             });


                try
                {
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                    resultado.numRegistros = modelFiltered.Count();
                    if (skip == 0 && take == 0)
                    {
                        resultado.datos = modelFiltered.ToArray();
                    }
                    else
                    {
                        resultado.datos = modelFiltered.Take(take).ToList();
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.InnerException.Message);

                }


                return resultado;

            }



        }


        /// <summary>
        /// Estados de reportes
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getPeriodQuinqueño")]
        public JArray getPeriodQuinqueño()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from R in dbSIM.TSIMTASA_PERIODO
                             join Q in dbSIM.TSIMTASA_QUINQUENO on R.TSIMTASA_QUINQUENO_ID equals Q.ID
                             select new
                             {
                                 ID_PERIODO = R.ID,
                                 R.NO_PERIODO,
                                 R.INICIA,
                                 R.TERMINA,
                                 QUINQUENO = Q.DESCRIPCION,
                                 concatenado = "Periodo: " + R.NO_PERIODO + " Quinqueño: " + Q.DESCRIPCION,
                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        // ************************************************************************************
        // **************************** Periodo *********************************************
        // ************************************************************************************

        /// <summary>
        /// QUINQUENO
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getPeriod")]
        public datosConsulta Period(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from R in dbSIM.TSIMTASA_PERIODO
                             join Q in dbSIM.TSIMTASA_QUINQUENO on R.TSIMTASA_QUINQUENO_ID equals Q.ID 
                             select new
                             {
                                 ID_PERIODO = R.ID,
                                 R.NO_PERIODO,
                                 R.INICIA,
                                 R.TERMINA,
                                 QUINQUENO= Q.DESCRIPCION
                             });

                Console.WriteLine(modelData);

                try
                {
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                    resultado.numRegistros = modelFiltered.Count();
                    if (skip == 0 && take == 0)
                    {
                        resultado.datos = modelFiltered.ToArray();
                    }
                    else
                    {
                        resultado.datos = modelFiltered.Take(take).ToList();
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.InnerException.Message);

                }


                return resultado;

            }
                
                
            
        }




        // ************************************************************************************
        // **************************** Quinqueno *********************************************
        // ************************************************************************************

        /// <summary>
        /// QUINQUENO
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getQuinqueno")]
        public datosConsulta Quinqueno(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from R in dbSIM.TSIMTASA_QUINQUENO
                            orderby R.ID ascending
                             select new 
                             {
                                 ID_QUINQUENO = R.ID,
                                 DESCRIPCION = R.DESCRIPCION,
                                 INICIO = R.INICIO,
                                 TERMINA = R.TERMINA,
                                 ACUERDO = R.ACUERDO,

                             });

                Console.WriteLine(modelData);

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }





        // ************************************************************************************
        // ************************************************************************************
        // ************************************************************************************


        /// <summary>
        /// Reports
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Reports")]
        public datosConsulta Reports(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from R in dbSIM.TSIMTASA_REPORTES
                             join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on R.TSIMTASA_CUENCAS_TERCERO_ID equals C.ID
                             join CU in dbSIM.TSIMTASA_CUENCAS on C.TSIMTASA_CUENCAS_ID1 equals CU.ID
                             join E in dbSIM.TSIMTASA_ESTADO_REPORTE on R.TSIMTASA_ESTADO_REPORTE_ID equals E.ID
                             join T in dbSIM.TSIMTASA_TIPO_REPORTE on R.TSIMTASA_TIPO_REPORTE_ID equals T.ID
                             join M in dbSIM.TSIMTASA_MESES on R.MES equals M.ID
                             join TE in dbSIM.TERCERO on C.ID_TERCERO equals TE.ID_TERCERO
                             join L in dbSIM.TSIMTASA_LIQUIDACIONES on R.ID equals L.REPORTES_ID
                             orderby R.ID ascending
                             select new modelReports
                             {
                                 ID_REPORTE = R.ID,
                                 VERTIMIENTO = CU.NOMBRE,
                                 AGNO = R.ANO,
                                 MES_ID = R.MES,
                                 MES = M.MES,
                                 NO_DESCARGAS_DIA = R.CANTIDAD_VERTIMIENTOS,
                                 HORAS_DESCARGAS_DIA = R.HORAS_VERTIMIENTO,
                                 DIAS_DESCARGAS_MES = R.DIAS_VERTIMIENTOS,
                                 ID_TERCERO = C.ID_TERCERO,
                                 NAME_TERCERO = TE.S_RSOCIAL,
                                 ESTADO_REPORTE_ID = E.ID,
                                 ESTADO_REPORTE = E.NOMBRE,
                                 TIPO_REPORTE_ID = T.ID,
                                 TIPO_REPORTE = T.NOMBRE,
                                 DBO = R.REPORTE_DBO,
                                 SST = R.REPORTE_SST,
                                 CAUDAL = R.CAUDAL_PROMEDIO,
                                 RADICADO = R.CODTRAMITE,
                                 BILLING_DBO = L.LIQUIDACION_DBO,
                                 BILLING_SST = L.LIQUIDACION_SST,
                                 DBOKGM = L.DBOKGM,
                                 SSTKGM = L.SSTKGM,
                                 TOTAL_BILLING = L.LIQUIDACION,
                                 NICK = C.NICK

                             });

                Console.WriteLine(modelData);

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [Authorize(Roles = "EADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveReport")]
        public object GetRemoveReports(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_REPORTES.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_REPORTES.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Material Eliminado Satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("billing")]
        public object Getbilling(int Id)
        {

            var factorConversion = 0.0036;

            if (Id > 0)
            {
                var _b = (from R in dbSIM.TSIMTASA_REPORTES
                          join F in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.ANO equals F.ANO
                          join FF in dbSIM.TSIMTASA_FACTOR_REGIONAL on R.ANO equals FF.ANO
                          join Tsst in dbSIM.TSIMTASA_TARIFAS_MINIMAS on R.ANO equals Tsst.ANO
                          join Tdbo in dbSIM.TSIMTASA_TARIFAS_MINIMAS on R.ANO equals Tdbo.ANO
                          join Psst in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on Tsst.TSIMTASAS_FACTOR_AMBIENTAL_ID equals Psst.ID
                          join Pdbo in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on Tdbo.TSIMTASAS_FACTOR_AMBIENTAL_ID equals Pdbo.ID
                          join Ct in dbSIM.TSIMTASA_CUENCAS_TERCERO on R.TSIMTASA_CUENCAS_TERCERO_ID equals Ct.ID
                          join C in dbSIM.TSIMTASA_CUENCAS on Ct.TSIMTASA_CUENCAS_ID1 equals C.ID
                          join M in dbSIM.TSIMTASA_METAS_INDIVIDUALES on Ct.ID_TERCERO equals M.ID_TERCERO
                          where R.ID == Id 
                          where Psst.ID == 1
                          where Pdbo.ID == 2
                          where F.TSIMTASA_TRAMOS_ID == C.TSIMTASA_TRAMOS_ID
                          where FF.TSIMTASA_TRAMOS_ID == C.TSIMTASA_TRAMOS_ID
                          where F.PARAMETROS_AMBIENTAL_ID == Psst.ID
                          where FF.PARAMETROS_AMBIENTAL_ID == Pdbo.ID

                                  select new
                                  {
                                      ID_REPORTE = R.ID,
                                      AGNO = R.ANO,
                                      MES_ID = R.MES,
                                      FR_TRAMO_SST = F.FACTOR,
                                      FR_TRAMO_DBO = FF.FACTOR,
                                      TARIFA_MINIMA_SST = Tsst.TARIFA,
                                      TARIFA_MINIMA_DBO = Tdbo.TARIFA,
                                      REPORT_DBO = R.REPORTE_DBO,
                                      REPORT_SST = R.REPORTE_SST,
                                      Cc_DBO = (double)(R.CAUDAL_PROMEDIO) * (double)(R.REPORTE_DBO) * (double)(R.HORAS_VERTIMIENTO) * factorConversion * (double)R.DIAS_VERTIMIENTOS,
                                      Cc_SST = (double)R.CAUDAL_PROMEDIO * (double)R.REPORTE_SST * (double)R.HORAS_VERTIMIENTO * factorConversion * (double)R.DIAS_VERTIMIENTOS,
                                      BILLING_DBO = (double)Tdbo.TARIFA * (double)FF.FACTOR,
                                      BILLING_SST = (double)Tsst.TARIFA * (double)F.FACTOR,
                                  });

                decimal SSTkgMES = new decimal();
                decimal DBOkgMES = new decimal();
                decimal billingSST = new decimal();
                decimal billingDBO = new decimal();
                var billing = new decimal();
                decimal idReport = new decimal();
                
                foreach (var item in _b)
                { 
                    DBOkgMES = (decimal)item.Cc_DBO;
                    SSTkgMES = (decimal)item.Cc_SST;
                    idReport = item.ID_REPORTE;
                    billingDBO = (decimal)item.BILLING_DBO * DBOkgMES;
                    billingSST = (decimal)item.BILLING_SST * SSTkgMES;
                }

                billing = billingSST + billingDBO;

                var _liquidacion = dbSIM.TSIMTASA_LIQUIDACIONES.Where(f => f.REPORTES_ID == idReport).FirstOrDefault();
                _liquidacion.LIQUIDACION_DBO = billingDBO;
                _liquidacion.LIQUIDACION_SST = billingSST;
                _liquidacion.DBOKGM = DBOkgMES;
                _liquidacion.SSTKGM = SSTkgMES;
                _liquidacion.LIQUIDACION = billing;
                _liquidacion.FECHA = DateTime.Now;
                _liquidacion.REPORTES_ID = idReport;

                dbSIM.SaveChanges();

                return new { response = "OK", mensaje = "SE FACTURÓ." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadReport")]
        public JObject loadReport(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            dynamic modelData;
            try
            {
                if (Id <= 0) return null;


                modelData = (from R in dbSIM.TSIMTASA_REPORTES
                             join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on R.TSIMTASA_CUENCAS_TERCERO_ID equals C.ID
                             join CU in dbSIM.TSIMTASA_CUENCAS on C.TSIMTASA_CUENCAS_ID1 equals CU.ID
                             join E in dbSIM.TSIMTASA_ESTADO_REPORTE on R.TSIMTASA_ESTADO_REPORTE_ID equals E.ID
                             join T in dbSIM.TSIMTASA_TIPO_REPORTE on R.TSIMTASA_TIPO_REPORTE_ID equals T.ID
                             join M in dbSIM.TSIMTASA_MESES on R.MES equals M.ID
                             join TE in dbSIM.TERCERO on C.ID_TERCERO equals TE.ID_TERCERO

                             orderby R.ID
                             where R.ID == Id
                             select new modelReports
                             {
                                 ID_REPORTE = R.ID,
                                 VERTIMIENTO_ID = CU.ID,
                                 VERTIMIENTO = CU.NOMBRE,
                                 AGNO = R.ANO,
                                 MES_ID = R.MES,
                                 MES = M.MES,
                                 NO_DESCARGAS_DIA = R.CANTIDAD_VERTIMIENTOS,
                                 HORAS_DESCARGAS_DIA = R.HORAS_VERTIMIENTO,
                                 DIAS_DESCARGAS_MES = R.DIAS_VERTIMIENTOS,
                                 ID_TERCERO = C.ID_TERCERO,
                                 NAME_TERCERO = TE.S_RSOCIAL,
                                 ESTADO_REPORTE_ID = E.ID,
                                 ESTADO_REPORTE = E.NOMBRE,
                                 TIPO_REPORTE_ID = T.ID,
                                 TIPO_REPORTE = T.NOMBRE,
                                 DBO = R.REPORTE_DBO,
                                 SST = R.REPORTE_SST,
                                 CAUDAL = R.CAUDAL_PROMEDIO,
                                 RADICADO = R.CODTRAMITE
                             }).FirstOrDefault();

                return JObject.FromObject(modelData, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertReport")]
        [HttpPost]
        public object InsertReport(modelReports objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID_REPORTE;

                    var cuenca_tercero = (from C in dbSIM.TSIMTASA_CUENCAS_TERCERO
                                          where C.ID_TERCERO == objData.ID_TERCERO & C.TSIMTASA_CUENCAS_ID1 == objData.VERTIMIENTO_ID
                                          select new { C.ID }
                        ).FirstOrDefault(); 


                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_REPORTES.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null) 
                        {
                            _turnUpdate.CANTIDAD_VERTIMIENTOS = objData.NO_DESCARGAS_DIA;
                            _turnUpdate.CAUDAL_PROMEDIO = objData.CAUDAL;
                            _turnUpdate.HORAS_VERTIMIENTO = objData.HORAS_DESCARGAS_DIA;
                            _turnUpdate.DIAS_VERTIMIENTOS = objData.DIAS_DESCARGAS_MES;
                            _turnUpdate.MES = objData.MES_ID;
                            _turnUpdate.ANO = objData.AGNO;
                            _turnUpdate.REPORTE_SST = objData.SST;
                            _turnUpdate.REPORTE_DBO = objData.DBO;
                            _turnUpdate.TSIMTASA_CUENCAS_TERCERO_ID = cuenca_tercero.ID;
                            _turnUpdate.TSIMTASA_ESTADO_REPORTE_ID = objData.ESTADO_REPORTE_ID;
                            _turnUpdate.TSIMTASA_TIPO_REPORTE_ID = objData.TIPO_REPORTE_ID;

                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {

                        TSIMTASA_REPORTES _newTurn = new TSIMTASA_REPORTES
                        {
                            CANTIDAD_VERTIMIENTOS = objData.NO_DESCARGAS_DIA,
                            CAUDAL_PROMEDIO = objData.CAUDAL,
                            HORAS_VERTIMIENTO = objData.HORAS_DESCARGAS_DIA,
                            DIAS_VERTIMIENTOS = objData.DIAS_DESCARGAS_MES,
                            MES = objData.MES_ID,
                            ANO = objData.AGNO,
                            REPORTE_SST = objData.SST,
                            REPORTE_DBO = objData.DBO,
                            TSIMTASA_CUENCAS_TERCERO_ID = cuenca_tercero.ID, 
                            TSIMTASA_ESTADO_REPORTE_ID = objData.ESTADO_REPORTE_ID,
                            TSIMTASA_TIPO_REPORTE_ID = objData.TIPO_REPORTE_ID,
                        };
                        dbSIM.TSIMTASA_REPORTES.Add(_newTurn);
                        dbSIM.SaveChanges();

                        decimal newid = _newTurn.ID;

                        TSIMTASA_LIQUIDACIONES tasa = new TSIMTASA_LIQUIDACIONES
                        {
                            REPORTES_ID = newid,
                            FECHA = DateTime.Now,
                            LIQUIDACION_DBO = 0,
                            LIQUIDACION_SST = 0,

                        };

                        dbSIM.TSIMTASA_LIQUIDACIONES.Add(tasa);
                        dbSIM.SaveChanges();

                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("LoadTributaryFactory")]
        public JArray LoadTributaryFactory(int IdTercero)
        {


            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                var modelData = (from cu in dbSIM.TSIMTASA_CUENCAS
                                 join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on cu.ID equals C.TSIMTASA_CUENCAS_ID1
                                 where C.ID_TERCERO == IdTercero
                                 orderby cu.NOMBRE
                                 select new
                                 {
                                     Vertimiento_Id = cu.ID,
                                     Vertimiento = cu.NOMBRE,

                                 });

                return JArray.FromObject(modelData, Js);


            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Consulta de Lista de unidades
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("MonthsReports")]
        public JArray MonthsReports()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_MESES
                             orderby P.ID
                             select new
                             {
                                 Mes_Id = P.ID,
                                 Mes = P.MES

                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Consulta de Lista de unidades
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AgnosReports")]
        public JArray AgnosReports()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_AGNOS_REPORTES
                             orderby P.AGNO
                             select new
                             {
                                 Agno = P.AGNO

                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        /// <summary>
        /// tipo de liquidaciones
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("TypeSettlement")]
        public JArray TypeSettlement()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_TIPO_REPORTE
                             orderby P.NOMBRE
                             select new
                             {
                                 Tipo_Reporte_Id = P.ID,
                                 Tipo_Reporte = P.NOMBRE,

                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Estados de reportes
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ReportStatus")]
        public JArray ReportStatus()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_ESTADO_REPORTE
                             orderby P.NOMBRE
                             select new
                             {
                                 Estado_Reporte_Id = P.ID,
                                 Estado_Reporte = P.NOMBRE,

                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Estados de reportes
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetTerceroS")]
        public JArray getTerceros()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TERCERO
                             orderby P.S_RSOCIAL
                             select new
                             {
                                 Id_Tercero = P.ID_TERCERO,
                                 Name_Tercero = P.S_RSOCIAL,


                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        /// <summary>
        /// Estados de reportes
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetParametro")]
        public JArray Parametro()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL
                             orderby P.ID
                             select new
                             {
                                 Id_Parametro = P.ID,
                                 Name_Parametro = P.NOMBRE,
                                 Descripcion = P.DESCRIPCION,
                                 Abreviatura = P.ABREVIATURA,


                             });

                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




        [Authorize(Roles = "VADMINISTRADOR")]
        public int get_Id_Tercero()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            //USUARIO _currentUser = new USUARIO();
            //USUARIO_FUNCIONARIO _currenFuncionario = new USUARIO_FUNCIONARIO();

            long idUsuario;
            int _id_Tercero;

            try
            {

                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                int user_actual = (int)(idUsuario);

                var _obj_Tercero = dbSIM.PROPIETARIO.Where(t => t.ID_USUARIO == user_actual).FirstOrDefault();
                _id_Tercero = Convert.ToInt32(_obj_Tercero.ID_TERCERO);

            }
            catch (Exception e)
            {
                var p = new { resp = "Error", mensaje = "Error Consultanto el Tercero Asociado al Usuario: " + e.Message };

                Console.WriteLine(e.InnerException.Message);

                return 7777;
            }

            return _id_Tercero;

        }



        // ************************************************************************************



        /// <summary>
        /// Parameter Enviroment
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("getParameter")]
        public datosConsulta Parameter(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             join P in dbSIM.TSIMTASA_PARAMETROS_AMBIENTAL on trn.TSIMTASAS_FACTOR_AMBIENTAL_ID equals P.ID

                             orderby trn.ID
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,
                                 P.ABREVIATURA,
                                 P.NOMBRE,
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveParameter")]
        public object GetRemoveParameter(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_TARIFAS_MINIMAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Parametro Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "No se pudo eliminar el parametro." };
            }
        }


        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadParameter")]
        public JObject loadParameter(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             where trn.ID == Id
                             orderby trn.ANO
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertParameter")]
        [HttpPost]
        public object InsertParameter(TSIMTASA_TARIFAS_MINIMAS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.ANO = objData.ANO;
                            _turnUpdate.TARIFA = objData.TARIFA;
                            _turnUpdate.TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID;

                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_TARIFAS_MINIMAS _newTurn = new TSIMTASA_TARIFAS_MINIMAS
                        {
                            ANO = objData.ANO,
                            TARIFA = objData.TARIFA,
                            TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID
                        };
                        dbSIM.TSIMTASA_TARIFAS_MINIMAS.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error al registar el Parameto ambiental: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Parametro ingresado con exito" };
            }
        }








        // ************************************************************************************



        /// <summary>
        /// Solidos Suspendidos Totales
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
        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("TSS")]
        public datosConsulta TSS(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             where trn.TSIMTASAS_FACTOR_AMBIENTAL_ID == 1
                             orderby trn.ID
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [Authorize(Roles = "EADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveTSS")]
        public object GetRemoveTSS(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_TARIFAS_MINIMAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadTSS")]
        public JObject loadTSS(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             where trn.ID == Id
                             orderby trn.ANO
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertTSS")]
        [HttpPost]
        public object InsertTSS(TSIMTASA_TARIFAS_MINIMAS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.ANO = objData.ANO;
                            _turnUpdate.TARIFA = objData.TARIFA;
                            _turnUpdate.TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID;

                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_TARIFAS_MINIMAS _newTurn = new TSIMTASA_TARIFAS_MINIMAS
                        {
                            ANO = objData.ANO,
                            TARIFA = objData.TARIFA,
                            TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID
                        };
                        dbSIM.TSIMTASA_TARIFAS_MINIMAS.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }













        // ************************************************************************************



        /// <summary>
        /// DEMANDA BIOQUIMICA DE OXIGENO
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
        [Authorize(Roles = "VADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("BOD")]
        public datosConsulta BOD(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            if (tipoData is null)
            {
                throw new ArgumentNullException(nameof(tipoData));
            }

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
                modelData = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             where trn.TSIMTASAS_FACTOR_AMBIENTAL_ID == 2
                             orderby trn.ID
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [Authorize(Roles = "EADMINISTRADOR")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveBOD")]
        public object GetRemoveBOD(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_TARIFAS_MINIMAS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadBOD")]
        public JObject loadBOD(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_TARIFAS_MINIMAS
                             where trn.ID == Id
                             orderby trn.ANO
                             select new
                             {
                                 trn.ID,
                                 trn.ANO,
                                 trn.TARIFA,
                                 trn.TSIMTASAS_FACTOR_AMBIENTAL_ID,

                             }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "CADMINISTRADOR")]
        [System.Web.Http.ActionName("InsertBOD")]
        [HttpPost]
        public object InsertBOD(TSIMTASA_TARIFAS_MINIMAS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_TARIFAS_MINIMAS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.ANO = objData.ANO;
                            _turnUpdate.TARIFA = objData.TARIFA;
                            _turnUpdate.TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID;

                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_TARIFAS_MINIMAS _newTurn = new TSIMTASA_TARIFAS_MINIMAS
                        {
                            ANO = objData.ANO,
                            TARIFA = objData.TARIFA,
                            TSIMTASAS_FACTOR_AMBIENTAL_ID = objData.TSIMTASAS_FACTOR_AMBIENTAL_ID
                        };
                        dbSIM.TSIMTASA_TARIFAS_MINIMAS.Add(_newTurn);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando El turno: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Turno ingresado con exito" };
            }
        }



        //**************************************** Maestra Municipios***********************************************
        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("loadCounty")]
        public JArray loadCounty()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string metropol = "S";
                var model = (from M in dbSIM.MUNICIPIOS
                             where M.AMVA == metropol
                             orderby M.CODIGO
                             select new
                             {
                                 M.ID_MUNI,
                                 M.CODIGO,
                                 M.NOMBRE,
                                 M.AMVA
                             }); 
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("getBasinType")]
        public JArray loadBasinType()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string metropol = "S";
                var model = (from M in dbSIM.TSIMTASA_TIPO_CUENCAS
                             select new
                             {
                                 M.ID,
                                 M.NOMBRE,
                             }); 
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Authorize(Roles = "VADMINISTRADOR")]
        [HttpGet, ActionName("getTramos")]
        public JArray tramos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                string metropol = "S";
                var model = (from M in dbSIM.TSIMTASA_TRAMOS
                             select new
                             {
                                 M.ID,
                                 M.NOMBRE,
                                 M.DESCRIPCION,
                             }); 
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



    }
}

