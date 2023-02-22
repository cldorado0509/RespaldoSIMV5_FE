
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
    using SIM.Data.Seguridad;
    using SIM.Data;
    using DevExpress.UnitConversion;
    using SIM.Models;


    public class ReportesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();


        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }



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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("TributaryFactory")]
        public datosConsulta TributaryFactory(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Id_Tercero)
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

                modelData = (from cu in dbSIM.TSIMTASA_CUENCAS
                             join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on cu.ID equals C.TSIMTASA_CUENCAS_ID1
                             where C.ID_TERCERO == Id_Tercero
                             orderby cu.NOMBRE
                             select new
                             {
                                 cu.ID,
                                 cu.NOMBRE,
                                 cu.CODIGO,
                                 cu.AREA,
                                 cu.CAUDAL,
                                 cu.TSIMTASA_TIPO_CUENCAS_ID,
                                 cu.LONGITUD,
                                 cu.ID_MUNICIPIO
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }







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
                             join M in dbSIM.MUNICIPIOS on trn.ID_MUNICIPIO equals M.ID_MUNI
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.CODIGO,
                                 trn.NOMBRE,
                                 trn.AREA,
                                 trn.CAUDAL,
                                 trn.TSIMTASA_TIPO_CUENCAS_ID,
                                 trn.LONGITUD,
                                 trn.ID_MUNICIPIO,
                                 MUNICIPIO = M.NOMBRE
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }


        /// <summary>
        /// All TRIBUTARY
        /// </summary>

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AllTributary")]
        public JArray AllTributary()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                var modelData = (from trn in dbSIM.TSIMTASA_CUENCAS
                                 join M in dbSIM.MUNICIPIOS on trn.ID_MUNICIPIO equals M.ID_MUNI
                                 orderby trn.NOMBRE
                                 select new
                                 {
                                     trn.ID,
                                     trn.CODIGO,
                                     trn.NOMBRE,
                                     trn.AREA,
                                     trn.CAUDAL,
                                     trn.TSIMTASA_TIPO_CUENCAS_ID,
                                     trn.LONGITUD,
                                     trn.ID_MUNICIPIO,
                                     CuencaMunicipio = trn.NOMBRE + " - " + M.NOMBRE 
                                 });

                return JArray.FromObject(modelData, Js);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// TIPOS DE DESCARGA
        /// </summary>

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetDownloadType")]
        public JArray DownloadType()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var modelData = (from trn in dbSIM.TSIMTASA_TIPO_DESCARGA
                                 orderby trn.NOMBRE
                                 select new
                                 {
                                     Tipo_Descarga_Id = trn.ID,
                                     Tipo_Descarga = trn.NOMBRE
                                 });
                return JArray.FromObject(modelData, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// TIPOS DE DESCARGA
        /// </summary>

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetWasteWater")]
        public JArray WasteWater()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var modelData = (from trn in dbSIM.TSIMTASA_TIPO_AGUA_RESIDUAL
                                 orderby trn.NOMBRE
                                 select new
                                 {
                                     Tipo_Agua_residual_Id = trn.ID,
                                     Tipo_Agua_Residual = trn.NOMBRE
                                 });
                return JArray.FromObject(modelData, Js);
            }
            catch (Exception exp)
            {
                throw exp;
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
                                 trn.ID_MUNICIPIO
                                

                             }).FirstOrDefault();



                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("LoadTributaryFactory")]
        public JArray LoadTributaryFactory(int IdTercero)
        {

            IdTercero = get_Id_Tercero();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                var modelData = (from cu in dbSIM.TSIMTASA_CUENCAS
                                 join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on cu.ID equals C.TSIMTASA_CUENCAS_ID1
                                 join M in dbSIM.MUNICIPIOS on cu.ID_MUNICIPIO equals M.ID_MUNI
                                 where C.ID_TERCERO == IdTercero
                                 orderby cu.NOMBRE
                                 select new
                                 {
                                     Vertimiento_Id = cu.ID,
                                     Vertimiento = cu.NOMBRE + " - " + M.NOMBRE,
                                     

                                 });

                return JArray.FromObject(modelData, Js);


            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("LoadTributaryFactoryId")]
        public JArray LoadTributaryFactoryId(int IdTributaryFactory)
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                var modelData = (from cu in dbSIM.TSIMTASA_CUENCAS
                                 join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on cu.ID equals C.TSIMTASA_CUENCAS_ID1
                                 join M in dbSIM.MUNICIPIOS on cu.ID_MUNICIPIO equals M.ID_MUNI
                                 where C.ID == IdTributaryFactory
                                 orderby cu.NOMBRE
                                 select new
                                 {
                                     Vertimiento_Id = cu.ID,
                                     Vertimiento = cu.NOMBRE + " - " + M.NOMBRE,
                                     Nick = C.NICK


                                 });

                return JArray.FromObject(modelData, Js);


            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




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
                            ID_MUNICIPIO = objData.ID_MUNICIPIO
                        };
                        dbSIM.TSIMTASA_CUENCAS.Add(_newTurn);
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Shedding")]
        public datosConsulta Shedding(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {

            var Id_Tercero = get_Id_Tercero();

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

                modelData = (from C in dbSIM.TSIMTASA_CUENCAS_TERCERO
                             join CU in dbSIM.TSIMTASA_CUENCAS on C.TSIMTASA_CUENCAS_ID1 equals CU.ID
                             join M in dbSIM.MUNICIPIOS on CU.ID_MUNICIPIO equals M.ID_MUNI
                             join dw in dbSIM.TSIMTASA_TIPO_DESCARGA on C.TIPO_DESCARGA equals dw.ID
                             join w in dbSIM.TSIMTASA_TIPO_AGUA_RESIDUAL on C.TIPO_AGUA_RESIDUAL equals w.ID
                             where C.ID_TERCERO == Id_Tercero
                             orderby C.ID
                             select new
                             {
                                 Id_Cuenca_Tercero = C.ID,
                                 Cuenca = CU.NOMBRE,
                                 Nick = C.NICK,
                                 Cuenca_Id = C.TSIMTASA_CUENCAS_ID1,
                                 Id_Tercero = C.ID_TERCERO,
                                 Latitud = C.LATITUD,
                                 Longitud = C.LONGITUD,
                                 No_Resolucion = C.NO_RESOLUCION,
                                 Tipo_Agua_Residual_Id = w.ID,
                                 Tipo_Agua_Residual = w.NOMBRE,
                                 Tipo_Descarga_Id = dw.ID,
                                 Tipo_Descarga = dw.NOMBRE,
                                 Fecha_Resolucion = C.FECHA_RESOLUCION,
                                 Caudal = C.CAUDAL_AUTORIZADO,
                                 Agnos_Vigencia = C.ANOS_VIGENCIA,
                                 Municipio = M.NOMBRE

                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveShedding")]
        public object GetRemoveShedding(int Id)
        {
            try
            {

                if (Id > 0)
                {
                    var turn = dbSIM.TSIMTASA_CUENCAS_TERCERO.Where(pd => pd.ID == Id).FirstOrDefault();
                    this.dbSIM.TSIMTASA_CUENCAS_TERCERO.Remove(turn);
                    this.dbSIM.SaveChanges();
                    return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
                }
                else
                {
                    return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
                }
            }
            catch (Exception e)
            {
                var p = new { resp = "Error", mensaje = ": Esta Corriente Esta Relacionada en un Reporte y no podrá ser Eliminada " };

                Console.WriteLine(e.InnerException.Message);
                return p;
            }

        }



        [HttpGet, ActionName("loadShedding")]
        public JObject loadShedding(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var modelData = (from C in dbSIM.TSIMTASA_CUENCAS_TERCERO
                                 join CU in dbSIM.TSIMTASA_CUENCAS on C.TSIMTASA_CUENCAS_ID1 equals CU.ID
                                 join dw in dbSIM.TSIMTASA_TIPO_DESCARGA on C.TIPO_DESCARGA equals dw.ID
                                 join w in dbSIM.TSIMTASA_TIPO_AGUA_RESIDUAL on C.TIPO_AGUA_RESIDUAL equals w.ID
                                 where C.ID == Id
                                 orderby C.ID
                                 select new
                                 {
                                     Id_Cuenca_Tercero = C.ID,
                                     Cuenca = CU.NOMBRE,
                                     Nick = C.NICK,
                                     Cuenca_Id = C.TSIMTASA_CUENCAS_ID1,
                                     Id_Tercero = C.ID_TERCERO,
                                     Latitud = C.LATITUD,
                                     Longitud = C.LONGITUD,
                                     No_Resolucion = C.NO_RESOLUCION,
                                     Tipo_Agua_Residual_Id = w.ID,
                                     Tipo_Agua_Residual = w.NOMBRE,
                                     Tipo_Descarga_Id = dw.ID,
                                     Tipo_Descarga = dw.NOMBRE,
                                     Fecha_Resolucion = C.FECHA_RESOLUCION,
                                     Caudal = C.CAUDAL_AUTORIZADO,
                                     Agnos_Vigencia = C.ANOS_VIGENCIA,
                                 }).FirstOrDefault();

                return JObject.FromObject(modelData, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



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




        [System.Web.Http.ActionName("InsertShedding")]
        [HttpPost]
        public object InsertShedding(TSIMTASA_CUENCAS_TERCERO objData)
        {



            try
            {
                decimal Id = -1;
                Id = objData.ID;
                objData.ID_TERCERO = get_Id_Tercero();


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
                        ID_INSTALACION = objData.ID_INSTALACION,
                        TSIMTASA_CUENCAS_ID1 = objData.TSIMTASA_CUENCAS_ID1,
                        ID_TERCERO = objData.ID_TERCERO,
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
                var Id_Tercero = get_Id_Tercero();
                modelData = (from R in dbSIM.TSIMTASA_REPORTES
                             join C in dbSIM.TSIMTASA_CUENCAS_TERCERO on R.TSIMTASA_CUENCAS_TERCERO_ID equals C.ID
                             join CU in dbSIM.TSIMTASA_CUENCAS on C.TSIMTASA_CUENCAS_ID1 equals CU.ID
                             join E in dbSIM.TSIMTASA_ESTADO_REPORTE on R.TSIMTASA_ESTADO_REPORTE_ID equals E.ID
                             join T in dbSIM.TSIMTASA_TIPO_REPORTE on R.TSIMTASA_TIPO_REPORTE_ID equals T.ID
                             join M in dbSIM.TSIMTASA_MESES on R.MES equals M.ID
                             join D in dbSIM.TSIMTASA_TIPO_DESCARGA on C.TIPO_DESCARGA equals D.ID
                             join AR in dbSIM.TSIMTASA_TIPO_AGUA_RESIDUAL on C.TIPO_AGUA_RESIDUAL equals AR.ID
                             orderby R.ID
                             where C.ID_TERCERO == Id_Tercero
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
                                 ESTADO_REPORTE_ID = E.ID,
                                 ESTADO_REPORTE = E.NOMBRE,
                                 TIPO_REPORTE_ID = T.ID,
                                 TIPO_REPORTE = T.NOMBRE,
                                 TIPO_DESCARGA_ID = D.ID,
                                 TIPO_DESCARGA = D.NOMBRE,
                                 TIPO_AGUA_RESIDUAL_ID = AR.ID,
                                 TIPO_AGUA_RESIDUAL = AR.NOMBRE,
                                 DBO = R.REPORTE_DBO,
                                 SST = R.REPORTE_SST,
                                 CAUDAL = R.CAUDAL_PROMEDIO,
                                 RADICADO = R.CODTRAMITE,
                                 NICK = C.NICK

                             });

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveReport")]
        public object GetRemoveReports(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_REPORTES.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_REPORTES.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }



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
                             join D in dbSIM.TSIMTASA_TIPO_DESCARGA on C.TIPO_DESCARGA equals D.ID

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
                                 ESTADO_REPORTE_ID = E.ID,
                                 ESTADO_REPORTE = E.NOMBRE,
                                 TIPO_REPORTE_ID = T.ID,
                                 TIPO_REPORTE = T.NOMBRE,
                                 TIPO_DESCARGA_ID = D.ID,
                                 TIPO_DESCARGA = D.NOMBRE,
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


        [System.Web.Http.ActionName("InsertReport")]
        [HttpPost]
        public object InsertReport(modelReports objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID_REPORTE;
                    var _Id_Tercero = get_Id_Tercero();

                    var cuenca_tercero = (from C in dbSIM.TSIMTASA_CUENCAS_TERCERO
                                          where C.ID_TERCERO == _Id_Tercero & C.TSIMTASA_CUENCAS_ID1 == objData.VERTIMIENTO_ID
                                          select new { C.ID }
                        ).FirstOrDefault();

                    var contadorReportes = (from R in dbSIM.TSIMTASA_REPORTES
                                            where R.TSIMTASA_CUENCAS_TERCERO_ID == _Id_Tercero
                                            && R.MES == objData.MES_ID
                                            && R.ANO == objData.AGNO
                                            select new
                                            {
                                                R.ID,
                                            }
                        );



                    var contadorCuencas = (from R in dbSIM.TSIMTASA_REPORTES
                                           where R.TSIMTASA_CUENCAS_TERCERO_ID == _Id_Tercero
                                           select new { R.ID }
                          );




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


                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0  && contadorReportes.Count() < contadorCuencas.Count())
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
                            TSIMTASA_ESTADO_REPORTE_ID = 4,
                            TSIMTASA_TIPO_REPORTE_ID = 1,
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
                    else
                    {
                        return new { resp = "Error", mensaje = "Ha superado el numero de reportes para este mes" };

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



        [HttpGet, ActionName("loadTSS")]
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

        /// <summary>
        /// Consulta de los años habililtados para el reporte
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
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
        /// Consulta de Lista de unidades
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
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


    }
}

