namespace SIM.Areas.AdministracionDocumental.Controllers
{
    using System.Data.Entity;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SIM.Data;
    using SIM.Areas.AdministracionDocumental.Models;
    using SIM.Data.Tramites;
    using System.Linq.Dynamic;

    [Route("api/[controller]", Name = "AdminDocumentalAPI")]
    public class AdminDocumentalAPIController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        #region Series Documentales
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
        [HttpGet, ActionName("GetSeriesDocumentales")]
        public datosConsulta GetSeriesDocumentales(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TBSERIE_DOCUMENTAL.OrderBy(f => f.NOMBRE).Where(f => f.VERSION == 2);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ObtenerSerieDocumental")]
        public JObject ObtenerSerieDocumental(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var empresa = this.dbSIM.TBSERIE_DOCUMENTAL.Where(f => f.CODSERIE_DOCUMENTAL== _Id).FirstOrDefault();

                SerieDocumental SerieDoc = new SerieDocumental();
                SerieDoc.Id = empresa.CODSERIE_DOCUMENTAL;
                SerieDoc.Nombre = empresa.NOMBRE;
                SerieDoc.Descripcion = empresa.DESCRIPCION;
                SerieDoc.Radicado = empresa.RADICADO == "1" ? true : false;
                SerieDoc.Habilitado = empresa.ACTIVO == "1" ? true : false;
                return JObject.FromObject(SerieDoc, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarSerieDocumental")]
        public object GuardarSerieDocumental(SerieDocumental objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Habilitado ? "1" : "0";
                if (Id > 0)
                {
                    var _SerieDoc = dbSIM.TBSERIE_DOCUMENTAL.Where(f => f.CODSERIE_DOCUMENTAL == Id).FirstOrDefault();
                    if (_SerieDoc != null)
                    {
                        _SerieDoc.NOMBRE = objData.Nombre;
                        _SerieDoc.DESCRIPCION = objData.Descripcion;
                        _SerieDoc.ACTIVO = objData.Habilitado == true ? "1" : "0";
                        _SerieDoc.COD_INTERNO = objData.CodInterno;
                    }
                }
                else
                {
                    TBSERIE_DOCUMENTAL _SerieDocN = new TBSERIE_DOCUMENTAL
                    {
                        NOMBRE = objData.Nombre,
                        DESCRIPCION = objData.Descripcion,
                        RADICADO = "0",
                        ACTIVO = objData.Habilitado == true ? "1" : "0",
                        COD_INTERNO = objData.CodInterno,
                    };
                    dbSIM.TBSERIE_DOCUMENTAL.Add(_SerieDocN);
                }
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría almacenada correctamente" };
        }

        [HttpGet, ActionName("EliminarSerieDocumental")]
        public object EliminarSerieDocumental(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la Serie Documental" };
            try
            {
                int Id = -1;
                if (objData != null && objData != "") Id = int.Parse(objData);
                if (Id > 0)
                {
                    var serieDoc = this.dbSIM.TBSERIE_DOCUMENTAL.Where(f => f.CODSERIE_DOCUMENTAL == Id).FirstOrDefault();
                    this.dbSIM.TBSERIE_DOCUMENTAL.Remove(serieDoc);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error  eliminando la Serie Documental: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Serie Documental eliminada correctamente!!" };
        }
        #endregion

        #region SubSeries Documentales
        
        /// <summary>
        /// Retorna el Listado de las SubSeries Documentales aociados a la Serie Documental seleccionada
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
        ///<param name="Id">Identifica la Serie Documental seleccionada</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetSubSeriesDocumentales")]
        public datosConsulta GetSubSeriesDocumentales(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Id)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TBSUBSERIE_DOCUMENTAL.Where(f => f.CODSERIE_DOCUMENTAL == Id).OrderBy(f => f.NOMBRE);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        /// <summary>
        /// Retorna la SubSerie Documental buscada
        /// </summary>
        /// <param name="Id">Identifica la SubSerie Documental</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerSubSerieDocumental")]
        public JObject ObtenerSubSerieDocumental(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var subSerie = this.dbSIM.TBSUBSERIE_DOCUMENTAL.Where(f => f.CODSUBSERIE_DOCUMENTAL == _Id).FirstOrDefault();
                SubSerieDocumental SubSerieDocumental = new SubSerieDocumental();
                SubSerieDocumental.Id = subSerie.CODSUBSERIE_DOCUMENTAL;
                SubSerieDocumental.Nombre = subSerie.NOMBRE;
                SubSerieDocumental.Descripcion = subSerie.DESCRIPCION;
                SubSerieDocumental.Habilitado = subSerie.ACTIVO == "1" ? true : false;
                return JObject.FromObject(SubSerieDocumental, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarSubSerieDocumental")]
        public object GuardarSubSerieDocumental(SubSerieDocumental objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Habilitado ? "1" : "0";
                if (Id > 0)
                {
                    var _SubSerieDoc = dbSIM.TBSUBSERIE_DOCUMENTAL.Where(f => f.CODSUBSERIE_DOCUMENTAL == Id).FirstOrDefault();
                    if (_SubSerieDoc != null)
                    {
                        _SubSerieDoc.CODSERIE_DOCUMENTAL = objData.SerieId;
                        _SubSerieDoc.NOMBRE = objData.Nombre;
                        _SubSerieDoc.DESCRIPCION = objData.Descripcion;
                        _SubSerieDoc.ACTIVO = objData.Habilitado == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBSUBSERIE_DOCUMENTAL _SubSerie = new TBSUBSERIE_DOCUMENTAL
                    {
                        CODSERIE_DOCUMENTAL = objData.SerieId,
                        NOMBRE = objData.Nombre,
                        DESCRIPCION = objData.Descripcion,
                        ACTIVO = objData.Habilitado == true ? "1" : "0",
                    };
                    dbSIM.TBSUBSERIE_DOCUMENTAL.Add(_SubSerie);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la SubSerie Documental : " + e.Message };
            }
            return new { resp = "OK", mensaje = "SubSerie Documental almacenada correctamente!" };
        }

        [HttpGet, ActionName("EliminarSubSerieDocumental")]
        public object EliminarSubSerieDocumental(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la SubSerie Documental" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var subSerieDoc = this.dbSIM.TBSUBSERIE_DOCUMENTAL.Where(f => f.CODSUBSERIE_DOCUMENTAL == Id).FirstOrDefault();

                    this.dbSIM.TBSUBSERIE_DOCUMENTAL.Remove(subSerieDoc);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error al eliminar la SubSerie Documental : " + e.Message };
            }
            return new { resp = "OK", mensaje = "SubSerie Documental eliminada correctamente!" };
        }
        #endregion

        #region Unidades Documentales
        /// <summary>
        /// Retorna el Listado de las Unidades Documentales aociados a la SubSerie Documental seleccionada
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
        ///<param name="Id">Identifica la SubSerie Documental seleccionada</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetUnidadesDocumentales")]
        public datosConsulta GetUnidadesDocumentales(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Id)
        {
            //dynamic model = null;
            //dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            var model = dbSIM.TBSERIE.Where(f => f.CODSUBSERIE_DOCUMENTAL == Id).OrderBy(f => f.NOMBRE).ToList();
           // modelData = model;
            //IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = model.Count();
            resultado.datos = model;
       

            return resultado;
        }

        /// <summary>
        /// Retorna la Unidad Documental buscada
        /// </summary>
        /// <param name="Id">Identifica la Unidad Documental</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerUnidadDocumental")]
        public JObject ObtenerUnidadDocumental(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var unidad = this.dbSIM.TBSERIE.Where(f => f.CODSERIE == _Id).FirstOrDefault();
                UnidadDocumental UnidadDocumental = new UnidadDocumental();
                UnidadDocumental.Id = unidad.CODSERIE;
                UnidadDocumental.Nombre = unidad.NOMBRE;
                UnidadDocumental.Descripcion = unidad.DESCRIPCION;
                UnidadDocumental.RutaDocumentos = unidad.RUTA_DOCUMENTOS;
                UnidadDocumental.TiempoGestion = unidad.TIEMPO_GESTION;
                UnidadDocumental.TiempoCentral = unidad.TIEMPO_CENTRAL;
                UnidadDocumental.TiempoHistorico= unidad.TIEMPO_HISTORICO;
                UnidadDocumental.Habilitado = unidad.ACTIVO == "1" ? true : false;
                UnidadDocumental.Radicado = unidad.RADICADO == "1" ? true : false;
                return JObject.FromObject(UnidadDocumental, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarUnidadDocumental")]
        public object GuardarUnidadDocumental(UnidadDocumental objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Habilitado ? "1" : "0";
                string _Radicado = objData.Radicado ? "1" : "0";
                if (Id > 0)
                {
                    var _UnidadDoc = dbSIM.TBSERIE.Where(f => f.CODSERIE == Id).FirstOrDefault();
                    if (_UnidadDoc != null)
                    {
                        _UnidadDoc.CODSUBSERIE_DOCUMENTAL = objData.SubSerieId;
                        _UnidadDoc.NOMBRE = objData.Nombre;
                        _UnidadDoc.DESCRIPCION = objData.Descripcion;
                        _UnidadDoc.RUTA_DOCUMENTOS = objData.RutaDocumentos;
                        _UnidadDoc.TIEMPO_GESTION = objData.TiempoGestion;
                        _UnidadDoc.TIEMPO_CENTRAL = objData.TiempoCentral;
                        _UnidadDoc.TIEMPO_HISTORICO = objData.TiempoHistorico;
                        _UnidadDoc.RADICADO = objData.Radicado == true ? "1" : "0";
                        _UnidadDoc.ACTIVO = objData.Habilitado == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBSERIE _Unidad = new TBSERIE
                    {
                        CODSUBSERIE_DOCUMENTAL = objData.SubSerieId,
                        NOMBRE = objData.Nombre,
                        DESCRIPCION = objData.Descripcion,
                        RUTA_DOCUMENTOS = objData.RutaDocumentos,
                        TIEMPO_GESTION = objData.TiempoGestion,
                        TIEMPO_CENTRAL = objData.TiempoCentral,
                        TIEMPO_HISTORICO = objData.TiempoHistorico,
                        RADICADO = objData.Radicado == true ? "1" : "0",
                        ACTIVO = objData.Habilitado == true ? "1" : "0",
                    };
                    dbSIM.TBSERIE.Add(_Unidad);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Unidad Documental : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Unidad Documental almacenada correctamente!" };
        }

        [HttpGet, ActionName("EliminarUnidadDocumental")]
        public object EliminarUnidadDocumental(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la Unidad Documental" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var unidad = this.dbSIM.TBSERIE.Where(f => f.CODSERIE == Id).FirstOrDefault();

                    this.dbSIM.TBSERIE.Remove(unidad);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error al eliminar la Unidad Documental : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Unidad Documental eliminada correctamente!" };
        }
        #endregion

        #region Metadatos Descriptivos
        /// <summary>
        /// Retorna el Listado de los Metadatos Descriptivo o índices asociados a la Unidad Documental seleccionada
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
        ///<param name="Id">Identifica la Unidad Documental seleccionada</param>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetMetadatosUnidadeDocumental")]
        public datosConsulta GetMetadatosUnidadeDocumental(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Id)
        {
           datosConsulta resultado = new datosConsulta();

            var model = dbSIM.TBINDICESERIE.Where(f => f.CODSERIE == Id).ToList();
           
            resultado.numRegistros = model.Count();
            resultado.datos = model;

            return resultado;
        }

        /// <summary>
        /// Retorna el Metadato buscado
        /// </summary>
        /// <param name="Id">Identifica el Metadato</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerMetadato")]
        public JObject ObtenerMetadato(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var metadato = this.dbSIM.TBINDICESERIE.Where(f => f.CODINDICE == _Id).FirstOrDefault();
                Metadato Metadato = new Metadato();
                Metadato.Id = metadato.CODINDICE;
                Metadato.Nombre = metadato.INDICE;
                Metadato.Tipo = metadato.TIPO;
                Metadato.Longitud = metadato.LONGITUD;
                Metadato.Obligatorio = metadato.OBLIGA == 1 ? true: false;
                Metadato.ListadoId = metadato.CODIGO_SUBSERIE;
                Metadato.ValorDefecto = metadato.VALORDEFECTO;
                Metadato.Mostrar = metadato.MOSTRAR == "1" ? true : false;
                Metadato.MostrarEnGrid = metadato.MOSTRAR_EN_GRID == "1" ? true : false;
                Metadato.Orden = metadato.ORDEN != null ? (int)metadato.ORDEN.Value :  0;
                return JObject.FromObject(Metadato, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarMetadato")]
        public object GuardarMetadato(Metadato objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
               
                if (Id > 0)
                {
                    var _Metadato = dbSIM.TBINDICESERIE.Where(f => f.CODINDICE == Id).FirstOrDefault();
                    if (_Metadato != null)
                    {
                        _Metadato.CODSERIE = objData.unidadId;
                        _Metadato.INDICE = objData.Nombre;
                        _Metadato.TIPO = objData.Tipo;
                        _Metadato.LONGITUD = objData.Longitud;
                        _Metadato.CODIGO_SUBSERIE = objData.ListadoId;
                        _Metadato.OBLIGA = objData.Obligatorio == true? 1: 0;
                        _Metadato.VALORDEFECTO = objData.ValorDefecto;
                        _Metadato.MOSTRAR = objData.Mostrar == true ? "1" : "0";
                        _Metadato.MOSTRAR_EN_GRID = objData.MostrarEnGrid == true ? "1" : "0";
                        _Metadato.ORDEN = objData.Orden;
                        _Metadato.CODSECUENCIA = objData.SecuenciaId;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBINDICESERIE _Metadato = new TBINDICESERIE
                    {
                        CODSERIE = objData.unidadId,
                        INDICE = objData.Nombre,
                        TIPO = objData.Tipo,
                        LONGITUD = objData.Longitud,
                        CODIGO_SUBSERIE = objData.ListadoId,
                        OBLIGA = objData.Obligatorio == true ? 1 : 0,
                        VALORDEFECTO = objData.ValorDefecto,
                        MOSTRAR = objData.Mostrar == true ? "1" : "0",
                        MOSTRAR_EN_GRID = objData.MostrarEnGrid == true ? "1" : "0",
                        ORDEN = objData.Orden,
                        CODSECUENCIA = objData.SecuenciaId,

                    };
                    dbSIM.TBINDICESERIE.Add(_Metadato);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el Metadato : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Metadato almacenado correctamente!" };
        }

        [HttpGet, ActionName("EliminarMetadato")]
        public object EliminarMetadato(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el Metadato" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var metadato = this.dbSIM.TBINDICESERIE.Where(f => f.CODINDICE == Id).FirstOrDefault();

                    this.dbSIM.TBINDICESERIE.Remove(metadato);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error al eliminar el Metadato : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Metadato eliminado correctamente!" };
        }


        [HttpGet, ActionName("GetTipoDatos")]
        public JArray GetTipoDatos()
        {
            return  JArray.Parse(@"[ { ""Id"": ""0"", ""Nombre"": ""Alfanumérico""},{ ""Id"": ""1"", ""Nombre"": ""Numérico""},{ ""Id"": ""2"", ""Nombre"": ""Fecha""},{ ""Id"": ""3"", ""Nombre"": ""Hora""},{ ""Id"": ""4"", ""Nombre"": ""Booleano""},{ ""Id"": ""5"", ""Nombre"": ""Listado""},{ ""Id"": ""8"", ""Nombre"": ""Dirección""}]");
        }

        

        [HttpGet, ActionName("GetListados")]
        public JArray GetListados()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.TBSUBSERIE
                             orderby Mod.NOMBRE
                             select new
                             {
                                 Id = (int)Mod.CODIGO_SUBSERIE,
                                 Nombre = Mod.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        #endregion

        #region Tablas de Retención Documental
        
        [HttpGet, ActionName("GetTablasRetencionDocumental")]
        public datosConsulta GetTablasRetencionDocumental(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            datosConsulta resultado = new datosConsulta();

            var model = dbSIM.TBVIGENCIA_TRD.OrderBy(f => f.D_INICIOVIGENCIA).ToList();

            resultado.numRegistros = model.Count();
            resultado.datos = model;


            return resultado;
        }
        
        [HttpPost, ActionName("GuardarTablaRetencion")]
        public object GuardarTablaRetencion(TablaRetencionDocumental objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _TablaRetencionDoc = dbSIM.TBVIGENCIA_TRD.Where(f => f.COD_VIGENCIA_TRD == Id).FirstOrDefault();
                    if (_TablaRetencionDoc != null)
                    {
                        _TablaRetencionDoc.S_NOMBRE = objData.Nombre;
                        _TablaRetencionDoc.S_DESCRIPCION = objData.Descripcion;
                        _TablaRetencionDoc.D_INICIOVIGENCIA = objData.VigenteDesde;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TBVIGENCIA_TRD _TablaRetencionDoc = new TBVIGENCIA_TRD
                    {
                        S_NOMBRE = objData.Nombre,
                        S_DESCRIPCION = objData.Descripcion,
                        D_INICIOVIGENCIA = objData.VigenteDesde,
                    };
                    dbSIM.TBVIGENCIA_TRD.Add(_TablaRetencionDoc);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Tabla de Retención Documental: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Tabla de Retención Documental almacenada correctamente!" };
        }


        [HttpGet, ActionName("ObtenerTablaRetencionDocumental")]
        public JObject ObtenerTablaRetencionDocumental(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var tablaRD = this.dbSIM.TBVIGENCIA_TRD.Where(f => f.COD_VIGENCIA_TRD == _Id).FirstOrDefault();

                TablaRetencionDocumental TablaRD = new TablaRetencionDocumental();
                TablaRD.Id = tablaRD.COD_VIGENCIA_TRD;
                TablaRD.Nombre = tablaRD.S_NOMBRE;
                TablaRD.Descripcion = tablaRD.S_DESCRIPCION;
                TablaRD.VigenteDesde = tablaRD.D_INICIOVIGENCIA;
                return JObject.FromObject(TablaRD, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        #endregion

        #region Unidades por Tabla de Retención Documental
        [HttpGet, ActionName("GetUnidadesTablasRetencionDocumental")]
        public datosConsulta GetUnidadesTablasRetencionDocumental(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Id)
        {
            datosConsulta resultado = new datosConsulta();

            var model = dbSIM.TBSERIE.OrderBy(f => f.NOMBRE).ToList();

            List<UnidadTablaRetencion> ListaUnidades = new List<UnidadTablaRetencion>();

            foreach(TBSERIE tBSERIE in model)
            {
                var unidadTabla = dbSIM.TBUNIDADESDOC_VIGENCIATRD.Where(f => f.CODVIGENCIA_TRD == Id && f.CODUNIDAD_DOCUMENTAL == tBSERIE.CODSERIE).FirstOrDefault();
                UnidadTablaRetencion unidadTablaRetencion = new UnidadTablaRetencion
                {
                    Id = unidadTabla == null ? 0 : unidadTabla.CODUNIDADDOC_VIGENCIA,
                    SerieDocumental = "",
                    SubSerieDocumental = "",
                    UnidadId = tBSERIE.CODSERIE,
                    UnidadDocumental = tBSERIE.NOMBRE,
                    Asignada = unidadTabla == null ? false : true,
                };
                ListaUnidades.Add(unidadTablaRetencion);
            }

            resultado.numRegistros = ListaUnidades.Count();
            resultado.datos = ListaUnidades;

            return resultado;
        }

        
        [HttpGet, ActionName("CambiarEstado")]
        public JObject CambiarEstado(string Id,string IdTR)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                int _IdTR = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);
                if (!string.IsNullOrEmpty(IdTR)) _IdTR = int.Parse(IdTR);

                UnidadTablaRetencion unidadTablaRetencion = new UnidadTablaRetencion
                {
                    SerieDocumental = "",
                    SubSerieDocumental = "",
                    UnidadId = _Id,
                    UnidadDocumental = "",
                };


                var tablaRD = this.dbSIM.TBUNIDADESDOC_VIGENCIATRD.Where(f => f.CODUNIDAD_DOCUMENTAL == _Id && f.CODVIGENCIA_TRD == _IdTR).FirstOrDefault();
                if (tablaRD == null)
                {
                    TBUNIDADESDOC_VIGENCIATRD tBUNIDADESDOC_VIGENCIATRD = new TBUNIDADESDOC_VIGENCIATRD
                    {
                        CODVIGENCIA_TRD = _IdTR,
                        CODUNIDAD_DOCUMENTAL = _Id,
                    };
                    this.dbSIM.TBUNIDADESDOC_VIGENCIATRD.Add(tBUNIDADESDOC_VIGENCIATRD);
                    unidadTablaRetencion.Id = tBUNIDADESDOC_VIGENCIATRD.CODUNIDADDOC_VIGENCIA;
                }
                else
                {
                    this.dbSIM.TBUNIDADESDOC_VIGENCIATRD.Remove(tablaRD);
                    unidadTablaRetencion.Id = 0;
                }
                this.dbSIM.SaveChanges();

              
                return JObject.FromObject(unidadTablaRetencion, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        #endregion


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
