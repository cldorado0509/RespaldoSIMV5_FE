

namespace SIM.Areas.Retributivas.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.Retributivas.Models;
    using SIM.Data;
    using SIM.Data.Agua;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DevExpress.Web;

    //[Route("api/[controller]", Name = "EmpresasApi")]
    public class EmpresasApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxLabel label = new ASPxLabel();
            label.ID = "txtNameEmpresa";
            label.Text = "Label Text";
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("factory")]
        public datosConsulta factory( int Id)
        {


            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (Id<1) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }else {
                
                modelData = (from f in dbSIM.JURIDICA
                        where f.ID_TERCERO == Id
                        select new
                        {
                            Id = f.ID_TERCERO,
                            NIT= f.ID_TERCERO,
                            ID_RL = f.ID_TERCEROREPLEGAL,
                            RAZON_SOCIAL = f.S_RSOCIAL,
                            SIGLA = f.S_SIGLA,
                            NO_ESTABLECIMIENTOS = f.N_ESTABLECIMIENTOS,
                            NATURALEZA = f.S_NATURALEZA,
                            COMERCIAL =f.S_NCOMERCIAL,
                            REGIMEN =f.S_REGIMEN,
                            ORDEN = f.S_ORDEN
                        });

                modelData = (   from P in dbSIM.INFORMACION_INDUSTRIA
                                    join Pd in dbSIM.TSIMTASA_PRODUCCION on P.ID_TERCERO equals Pd.ID_TERCERO
                                    join Pro in dbSIM.TSIMTASA_PRODUCTOS on Pd.TSIMTASA_PRODUCTOS_ID equals Pro.ID
                                    join Uni in dbSIM.TSIMTASA_UNIDADES on Pd.TSIMTASA_UNIDADES_ID equals Uni.ID
                                     where P.ID_TERCERO == Id
                                select new
                             {
                                 Pd.ID,
                                 NameUnits = Uni.NOMBRE,
                                 IdUnits = Uni.ID,
                                 IdProducts = Pro.ID,
                                 NameProducts = Pro.NOMBRE,
                                 Pd.DIARIO,
                                 Pd.MENSUAL,
                                 Pd.ID_TERCERO
                             });


                resultado.datos = modelData;
                return resultado;
            }
        }



        /// <summary>
        /// <returns>productos para alimentar listas</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Products")]
        public JArray GetProducts()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_PRODUCTOS
                             orderby P.NOMBRE
                             select new
                             {
                                 IdProducts = P.ID,
                                 Nombre = P.NOMBRE,
                                 Decripcion = P.DESCRIPCION
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Consulta de Lista de Producción
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Product")]
        public datosConsulta Product(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                
                modelData = (from P in dbSIM.TSIMTASA_PRODUCTOS
                             orderby P.NOMBRE
                             select new
                             {
                                 ID = P.ID,
                                 IdProducts = P.ID,
                                 Nombre = P.NOMBRE,
                                 Descripcion = P.DESCRIPCION
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        /// <summary>
        /// Consulta de Lista de unidades
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("UnitsOfMeasurement")]
        public JArray GetUnitsOfMeasurement()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            { 
             var model = (from P in dbSIM.TSIMTASA_UNIDADES
                          orderby P.NOMBRE
                          select new
                          {
                            IdUnits = P.ID,
                              Nombre = P.NOMBRE
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("TypeFactory")]
        public JArray TypeFactory()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_TIPO_EMPRESA
                             orderby P.ID
                             select new
                             {
                                 IdType = P.ID,
                                 Nombre = P.NOMBRE,
                                 Abr =P.ABREVIATURA
                             });


                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Materials")]
        public JArray GetMaterials()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from P in dbSIM.TSIMTASA_MATERIAS_PRIMA
                             orderby P.NOMBRE
                             select new
                             {
                                 IdMaterials = P.ID,
                                 Nombre = P.NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

       /// <summary>
        /// guardar Productos
        /// </summary>
        /// <param name="values">Identificadore del producto a ingresar</param>
        /// <returns>Estado de la solicitud</returns>
        [System.Web.Http.ActionName("InsertProduction")]
        [HttpPost]
        public object InsertProduction(TSIMTASA_PRODUCCION objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var ProductIonUpdate = dbSIM.TSIMTASA_PRODUCCION.Where(f => f.ID == Id).FirstOrDefault();
                        if (ProductIonUpdate != null)
                        {
                            ProductIonUpdate.TSIMTASA_PRODUCTOS_ID = objData.TSIMTASA_PRODUCTOS_ID;
                            ProductIonUpdate.TSIMTASA_UNIDADES_ID = objData.TSIMTASA_UNIDADES_ID;
                            ProductIonUpdate.DIARIO = objData.DIARIO;
                            ProductIonUpdate.MENSUAL = objData.MENSUAL;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_PRODUCCION _newProduction = new TSIMTASA_PRODUCCION
                        {
                            ID_TERCERO = objData.ID_TERCERO,
                            TSIMTASA_PRODUCTOS_ID = objData.TSIMTASA_PRODUCTOS_ID,
                            TSIMTASA_UNIDADES_ID = objData.TSIMTASA_UNIDADES_ID,
                            MENSUAL = objData.MENSUAL,
                            DIARIO = objData.DIARIO
                        };
                        dbSIM.TSIMTASA_PRODUCCION.Add(_newProduction);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "producto ingresado con exito" };
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveProduction")]
        public object GetRemoveProduction(int Id)
        {
            Console.Write("ingreo al API delete de productos");
            

            if (Id > 0)
            {
                var production = dbSIM.TSIMTASA_PRODUCCION.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_PRODUCCION.Remove(production);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Producto Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveConsumption")]
        public object GetRemoveConsumption(int Id)
        {
            Console.Write("ingreo al API delete de productos");


            if (Id > 0)
            {
                var consumption = dbSIM.TSIMTASA_CONSUMOS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_CONSUMOS.Remove(consumption);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Producto Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveProduct")]
        public object GetRemoveProduct(int Id)
        {

            if (Id > 0)
            {
                var products = dbSIM.TSIMTASA_PRODUCTOS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_PRODUCTOS.Remove(products);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Producto Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }


        /// <summary>
        /// Actualizar Productos
        /// </summary>
        /// <param name="values">Identificadore del producto a Eliminar</param>
        /// <returns>Estado de la solicitud</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("SaveProducts")]
        public object GetSaveProducts(TSIMTASA_PRODUCTO data)
        {
            try
            {
                decimal Id = -1;
                if (Id > 0)
                {
                    var Producto = dbSIM.TSIMTASA_PRODUCTO.Where(f => f.ID == Id).FirstOrDefault();
                    if (Producto != null)
                    {
                        //_Producto.DDECONO_TERCERODEID = objData.TerceroId;
                        //_Producto.DDECONO_UNIDAD_MEDIDAID = idUnidadMed;
                        //_Producto.S_NOMBRE = objData.NombreProducto;
                        //_Producto.S_DESCRIPCION = objData.DescripcionProducto;
                        //_Producto.S_URL_IMAGEN = objData.UrlImagen;
                        //_Producto.N_VALOR_UNIDAD = objData.ValorUnitario;
                        //_Producto.B_ACTIVO = objData.Activo == true ? "1" : "0";
                        //dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TSIMTASA_PRODUCTO Producto = new TSIMTASA_PRODUCTO
                    {
                        ID_TERCERO = 310993,
                        NOMBRE = data.NOMBRE,
                        DESCRIPCION = data.DESCRIPCION,
                        CANTIDAD = data.CANTIDAD,
                        UNIDADES = data.UNIDADES
                    };
                    dbSIM.TSIMTASA_PRODUCTO.Add(Producto);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }

            return new { resp = "ok", mensaje = "producto ingresado con exito" };


        }





        /// <summary>
        /// Consulta de Lista de Producción
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Production")]
        public datosConsulta GetProduction(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int idTercero)
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
                modelData = (from P in dbSIM.INFORMACION_INDUSTRIA
                             join Pd in dbSIM.TSIMTASA_PRODUCCION on P.ID_TERCERO equals Pd.ID_TERCERO
                             join Pro in dbSIM.TSIMTASA_PRODUCTOS on Pd.TSIMTASA_PRODUCTOS_ID equals Pro.ID
                             join Uni in dbSIM.TSIMTASA_UNIDADES on Pd.TSIMTASA_UNIDADES_ID equals Uni.ID
                             where P.ID_TERCERO == idTercero
                             select new
                             {
                                 Pd.ID,
                                 NameUnits = Uni.NOMBRE,
                                 IdUnits= Uni.ID,
                                 IdProducts= Pro.ID,
                                 NameProducts = Pro.NOMBRE,
                                 Pd.DIARIO,
                                 Pd.MENSUAL,
                                 Pd.ID_TERCERO
                             }) ;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }




        [HttpGet, ActionName("loadProduct")]
        public JObject loadProduct(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
               var _product = (from prdto in dbSIM.TSIMTASA_PRODUCTOS
                                where prdto.ID == Id
                                orderby prdto.NOMBRE
                             select new
                             {
                                 ID = prdto.ID,
                                 IdProducts = prdto.ID,
                                 Nombre = prdto.NOMBRE,
                                 Descripcion = prdto.DESCRIPCION
                             }).FirstOrDefault(); 


                return JObject.FromObject(_product, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("loadProduction")]
        public JObject loadProduction(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                if (Id <= 0) return null;
                //     var production = this.dbSIM.TSIMTASA_PRODUCCION.Where(f => f.ID == _Id).FirstOrDefault();
                var production = (from Pcion in dbSIM.TSIMTASA_PRODUCCION
                                  join Pdt in dbSIM.TSIMTASA_PRODUCTOS on Pcion.TSIMTASA_PRODUCTOS_ID equals Pdt.ID
                                  join Uni in dbSIM.TSIMTASA_UNIDADES on Pcion.TSIMTASA_UNIDADES_ID equals Uni.ID
                                  where Pcion.ID == Id
                                  select new modelProduction
                                  {
                                      TSIMTASA_PRODUCTOS_ID = Pdt.ID,
                                      PRODUCTOS_NAME = Pdt.NOMBRE,
                                      ID_TERCERO = Pcion.ID_TERCERO,
                                      ID = Pcion.ID,
                                      MENSUAL = Pcion.MENSUAL,
                                      DIARIO = Pcion.DIARIO,
                                      TSIMTASA_UNIDADES_ID = Uni.ID,
                                      UNIDADES_NAME = Uni.NOMBRE

                                  }).FirstOrDefault();

  
                 return JObject.FromObject(production, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("loadConsumption")]
        public JObject loadConsumption(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {

                if (Id <= 0) return null;
                //     var production = this.dbSIM.TSIMTASA_PRODUCCION.Where(f => f.ID == _Id).FirstOrDefault();
                var consumption = (from Pcion in dbSIM.TSIMTASA_CONSUMOS
                                  join Pdt in dbSIM.TSIMTASA_MATERIAS_PRIMA on Pcion.TSIMTASA_MATERIAS_PRIMA_ID equals Pdt.ID
                                  join Uni in dbSIM.TSIMTASA_UNIDADES on Pcion.TSIMTASA_UNIDADES_ID equals Uni.ID
                                  where Pcion.ID == Id
                                  select new modelConsumption
                                  {
                                      TSIMTASA_MATERIAS_PRIMA_ID = Pdt.ID,
                                      MATERIALS_NAME = Pdt.NOMBRE,
                                      ID_TERCERO = Pcion.ID_TERCERO,
                                      ID = Pcion.ID,
                                      MENSUAL = Pcion.MENSUAL,
                                      DIARIO = Pcion.DIARIO,
                                      TSIMTASA_UNIDADES_ID = Uni.ID,
                                      UNIDADES_NAME = Uni.NOMBRE

                                  }).FirstOrDefault();


                return JObject.FromObject(consumption, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.ActionName("InsertConsumptions")]
        [HttpPost]
        public object InsertConsumptions(TSIMTASA_CONSUMOS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var consumptionUpdate = dbSIM.TSIMTASA_CONSUMOS.Where(f => f.ID == Id).FirstOrDefault();
                        if (consumptionUpdate != null)
                        {
                            consumptionUpdate.TSIMTASA_MATERIAS_PRIMA_ID = objData.TSIMTASA_MATERIAS_PRIMA_ID;
                            consumptionUpdate.TSIMTASA_UNIDADES_ID = objData.TSIMTASA_UNIDADES_ID;
                            consumptionUpdate.DIARIO = objData.DIARIO;
                            consumptionUpdate.MENSUAL = objData.MENSUAL;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_CONSUMOS _newConsumption = new TSIMTASA_CONSUMOS
                        {
                            ID_TERCERO = objData.ID_TERCERO,
                            TSIMTASA_MATERIAS_PRIMA_ID = objData.TSIMTASA_MATERIAS_PRIMA_ID,
                            TSIMTASA_UNIDADES_ID = objData.TSIMTASA_UNIDADES_ID,
                            MENSUAL = objData.MENSUAL,
                            DIARIO = objData.DIARIO
                        };
                        dbSIM.TSIMTASA_CONSUMOS.Add(_newConsumption);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "su consumo  de materias primas fue regsitrado con exito" };
            }
        }


        [System.Web.Http.ActionName("InsertProduct")]
        [HttpPost]
        public object InsertProduct(TSIMTASA_PRODUCTOS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var productUpdate = dbSIM.TSIMTASA_PRODUCTOS.Where(f => f.ID == Id).FirstOrDefault();
                        if (productUpdate != null)
                        {
                            productUpdate.NOMBRE = objData.NOMBRE;
                            productUpdate.DESCRIPCION = objData.DESCRIPCION;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_PRODUCTOS _newProduct = new TSIMTASA_PRODUCTOS
                        {
                            NOMBRE = objData.NOMBRE,
                            DESCRIPCION = objData.DESCRIPCION,
                        };
                        dbSIM.TSIMTASA_PRODUCTOS.Add(_newProduct);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "producto ingresado con exito" };
            }
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("MisMateriales")]
        public datosConsulta GetMisMateriales(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int Idtercero)
        {
            dynamic DataMaterials;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                DataMaterials = (from Q in dbSIM.INFORMACION_INDUSTRIA
                                 join Qd in dbSIM.TSIMTASA_CONSUMOS on Q.ID_TERCERO equals Qd.ID_TERCERO
                                 join Mtr in dbSIM.TSIMTASA_MATERIAS_PRIMA on Qd.TSIMTASA_MATERIAS_PRIMA_ID equals Mtr.ID
                                 join Uni in dbSIM.TSIMTASA_UNIDADES on Qd.TSIMTASA_UNIDADES_ID equals Uni.ID
                                 where Q.ID_TERCERO == Idtercero
                                 select new
                                 {
                                     Qd.ID,
                                     UnitsName = Uni.NOMBRE,
                                     IdUnits = Uni.ID,
                                     IdMaterials = Mtr.ID,
                                     MaterialsName = Mtr.NOMBRE,
                                     Qd.DIARIO,
                                     Qd.MENSUAL,
                                     Qd.ID_TERCERO
                             }) ;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(DataMaterials, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
                return resultado;
            }
        }




        ///////////////////////
        ///MATERILAES E INSUMOS
        /////////////////////////
        ///

        /// <summary>
        /// Consulta de Lista de Producción
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Material")]
        public datosConsulta Material(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

                modelData = (from mtrl in dbSIM.TSIMTASA_MATERIAS_PRIMA
                             orderby mtrl.NOMBRE
                             select new
                             {
                                 mtrl.ID,
                                 mtrl.NOMBRE,
                                 mtrl.DESCRIPCION
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveMaterial")]
        public object GetRemoveMaterial(int Id)
        {

            if (Id > 0)
            {
                var Materials = dbSIM.TSIMTASA_MATERIAS_PRIMA.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_MATERIAS_PRIMA.Remove(Materials);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }



        [HttpGet, ActionName("loadMaterial")]
        public JObject loadMaterial(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _Material = (from mtrl in dbSIM.TSIMTASA_MATERIAS_PRIMA
                                 where mtrl.ID == Id
                                orderby mtrl.NOMBRE
                                select new
                                {
                                   mtrl.ID,
                                   mtrl.NOMBRE,
                                   mtrl.DESCRIPCION

                                }).FirstOrDefault();

                return JObject.FromObject(_Material, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [System.Web.Http.ActionName("InsertMaterial")]
        [HttpPost]
        public object InsertMaterial(TSIMTASA_MATERIAS_PRIMA objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var MaterialUpdate = dbSIM.TSIMTASA_MATERIAS_PRIMA.Where(f => f.ID == Id).FirstOrDefault();
                        if (MaterialUpdate != null)
                        {
                            MaterialUpdate.NOMBRE = objData.NOMBRE;
                            MaterialUpdate.DESCRIPCION = objData.DESCRIPCION;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_MATERIAS_PRIMA _newMaterial = new TSIMTASA_MATERIAS_PRIMA
                        {
                            NOMBRE = objData.NOMBRE,
                            DESCRIPCION = objData.DESCRIPCION,
                        };
                        dbSIM.TSIMTASA_MATERIAS_PRIMA.Add(_newMaterial);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var p = new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };

                    Console.WriteLine(e.InnerException.Message);
                    return p;
                }
                return new { resp = "ok", mensaje = "Materialo ingresado con exito" };
            }
        }



        ///////////////////////
        ///TURNOS
        /////////////////////////
        ///

        /// <summary>
        /// Consulta de Lista de Producción
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
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Turns")]
        public datosConsulta Turns(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int IdTercero)
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

                modelData = (from trn in dbSIM.TSIMTASA_TURNOS
                             where trn.ID_TERCERO == IdTercero
                             orderby trn.NOMBRE
                             select new
                             {
                                 trn.ID,
                                 trn.NOMBRE,
                                 trn.DESCRIPCION,
                                 trn.INICIA,
                                 trn.TERMINA,
                                 trn.NO_OPERARIOS,
                                 trn.ID_TERCERO
                             });
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Take(take).ToList();
                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RemoveTurn")]
        public object GetRemoveTurn(int Id)
        {

            if (Id > 0)
            {
                var turn = dbSIM.TSIMTASA_TURNOS.Where(pd => pd.ID == Id).FirstOrDefault();
                this.dbSIM.TSIMTASA_TURNOS.Remove(turn);
                this.dbSIM.SaveChanges();
                return new { response = "OK", mensaje = "Hecho: Materialo Eliminado satisfactoriomente." };
            }
            else
            {
                return new { response = "ERROR", mensaje = "Procedimiento Inválido." };
            }
        }



        [HttpGet, ActionName("loadTurn")]
        public JObject loadTurn(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                if (Id <= 0) return null;
                var _turn = (from trn in dbSIM.TSIMTASA_TURNOS
                             where trn.ID == Id
                                 orderby trn.NOMBRE
                                 select new
                                 {
                                     trn.ID,
                                     trn.NOMBRE,
                                     trn.DESCRIPCION,
                                     trn.INICIA,
                                     trn.TERMINA,
                                     trn.NO_OPERARIOS,
                                     trn.ID_TERCERO

                                 }).FirstOrDefault();

                return JObject.FromObject(_turn, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [System.Web.Http.ActionName("InsertTurn")]
        [HttpPost]
        public object InsertTurn(TSIMTASA_TURNOS objData)
        {
            {
                try
                {
                    decimal Id = -1;
                    Id = objData.ID;

                    if (Id > 0)
                    {
                        var _turnUpdate = dbSIM.TSIMTASA_TURNOS.Where(f => f.ID == Id).FirstOrDefault();
                        if (_turnUpdate != null)
                        {
                            _turnUpdate.NOMBRE = objData.NOMBRE;
                            _turnUpdate.DESCRIPCION = objData.DESCRIPCION;
                            _turnUpdate.INICIA = objData.INICIA;
                            _turnUpdate.TERMINA = objData.TERMINA;
                            _turnUpdate.NO_OPERARIOS = objData.NO_OPERARIOS;
                            dbSIM.SaveChanges();
                        }
                    }
                    else if (Id <= 0)
                    {
                        TSIMTASA_TURNOS _newTurn = new TSIMTASA_TURNOS
                        {
                            NOMBRE = objData.NOMBRE,
                            DESCRIPCION = objData.DESCRIPCION,
                            INICIA = objData.INICIA,
                            TERMINA = objData.TERMINA,
                            NO_OPERARIOS = objData.NO_OPERARIOS,
                            ID_TERCERO = objData.ID_TERCERO
                    };
                        dbSIM.TSIMTASA_TURNOS.Add(_newTurn);
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





    }
}
