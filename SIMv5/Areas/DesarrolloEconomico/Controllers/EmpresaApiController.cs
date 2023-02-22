namespace SIM.Areas.DesarrolloEconomico.Controllers
{
    using System.Data.Entity;
    using System.Web.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SIM.Areas.DesarrolloEconomico.Models;
    using SIM.Data.DesarrolloEconomico;
    using SIM.Data;

    [Route("api/[controller]", Name = "EmpresaApi")]
    public class EmpresaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Consulta de Lista de Unidades de Medida
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
        [HttpGet, ActionName("GetEmpresas")]
        public datosConsulta GetEmpresas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.VDECONO_TERCERODE.OrderBy(f => f.S_RAZON_SOCIAL);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ObtenerEmpresa")]
        public JObject ObtenerEmpresa(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var empresa = this.dbSIM.VDECONO_TERCERODE.Where(f => f.ID == _Id).FirstOrDefault();

                Empresa Empresa = new Empresa();
                Empresa.Id = empresa.ID;
                Empresa.RazonSocial = empresa.S_RAZON_SOCIAL;
                Empresa.Nit = empresa.S_NIT;
                Empresa.Descripcion = empresa.S_DESCRIPCION;
                Empresa.Categoria = empresa.CATEGORIA;
                Empresa.Municipio = empresa.MUNICIPIO;
                Empresa.Direccion = empresa.S_DIRECCION;
                Empresa.EMail = empresa.S_EMAIL;
                Empresa.Instagram = empresa.S_URL_INSTAGRAM;
                Empresa.Web = empresa.S_URL_WEB;
                Empresa.Logo = empresa.S_URL_LOGO;
                Empresa.Facebook = empresa.S_URL_FACEBOOK;
                Empresa.Foto = empresa.S_URL_FOTO_EMPRENDEDOR;
                Empresa.IdTercero = empresa.ID_TERCERO;
                Empresa.Telefono1 = empresa.S_TELEFONO1;
                Empresa.Telefono2 = empresa.S_TELEFONO2;
                Empresa.Activo = empresa.B_ACTIVO == "1" ? true : false;
                return JObject.FromObject(Empresa, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarEmpresa")]
        public object GuardarEmpresa(Empresa objData)
        {
            if (!ModelState.IsValid) return null;
            int idCategoria = 0;
            int idMunicipo = 0;
            int.TryParse(objData.Municipio, out idMunicipo);
            int.TryParse(objData.Categoria, out idCategoria);
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Activo ? "1" : "0";
                if (Id > 0)
                {
                    var _Empresa = dbSIM.DDECONO_TERCERODE.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Empresa != null)
                    {
                        _Empresa.ID_TERCERO = 1;
                        _Empresa.S_DESCRIPCION = objData.Descripcion;
                        _Empresa.S_DIRECCION = objData.Direccion;
                        _Empresa.S_EMAIL = objData.EMail;
                        _Empresa.S_NIT = objData.Nit;
                        _Empresa.S_RAZON_SOCIAL = objData.RazonSocial;
                        _Empresa.S_TELEFONO1 = objData.Telefono1;
                        _Empresa.S_TELEFONO2 = objData.Telefono2;
                        _Empresa.S_URL_FACEBOOK = objData.Facebook;
                        _Empresa.S_URL_FOTO_EMPRENDEDOR = objData.Foto;
                        _Empresa.S_URL_INSTAGRAM = objData.Instagram;
                        _Empresa.S_URL_LOGO = objData.Logo;
                        _Empresa.S_URL_WEB = objData.Web;
                        _Empresa.B_ACTIVO = objData.Activo == true ? "1" : "0";

                        var _categoriaEmpresa = dbSIM.DDECONO_CATEGORIA_TERCERO.Where(f => f.DDECONO_TERCERODEID == _Empresa.ID).FirstOrDefault();
                        if (_categoriaEmpresa != null)
                        {
                            _categoriaEmpresa.DDECONO_CATEGORIAID = idCategoria;
                        }
                        else
                        {
                            _categoriaEmpresa = new DDECONO_CATEGORIA_TERCERO
                            {
                                 DDECONO_CATEGORIAID = idCategoria,
                                 DDECONO_TERCERODEID = _Empresa.ID,
                            };
                            dbSIM.DDECONO_CATEGORIA_TERCERO.Add(_categoriaEmpresa);
                        }

                        var _municipioEmpresa = dbSIM.DDECONO_MUNICIPIO_TERCERO.Where(f => f.DDECONO_TERCERODEID == _Empresa.ID).FirstOrDefault();
                        if (_municipioEmpresa != null)
                        {
                            _municipioEmpresa.DDECONO_MUNICIPIOID = idMunicipo;
                        }
                        else
                        {
                            _municipioEmpresa = new DDECONO_MUNICIPIO_TERCERO
                            {
                                 DDECONO_MUNICIPIOID = idMunicipo,
                                 DDECONO_TERCERODEID = _Empresa.ID,
                            };
                            dbSIM.DDECONO_MUNICIPIO_TERCERO.Add(_municipioEmpresa);
                        }
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    DDECONO_TERCERODE _Empresa = new DDECONO_TERCERODE
                    {
                        ID_TERCERO = 1,
                        S_DIRECCION = objData.Direccion,
                        S_EMAIL = objData.EMail,
                        S_NIT = objData.Nit,
                        S_TELEFONO1 = objData.Telefono1,
                        S_TELEFONO2 = objData.Telefono2,
                        S_URL_FACEBOOK = objData.Facebook,
                        S_URL_FOTO_EMPRENDEDOR = objData.Foto,
                        S_URL_INSTAGRAM = objData.Instagram,
                        S_URL_LOGO = objData.Logo,
                        S_URL_WEB = objData.Web,
                        S_RAZON_SOCIAL = objData.RazonSocial,
                        S_DESCRIPCION = objData.Descripcion,
                        B_ACTIVO = objData.Activo == true ? "1" : "0",
                    };
                    dbSIM.DDECONO_TERCERODE.Add(_Empresa);
                    dbSIM.SaveChanges();

                    var _categoriaEmpresa = new DDECONO_CATEGORIA_TERCERO
                    {
                         DDECONO_CATEGORIAID = idCategoria,
                         DDECONO_TERCERODEID = _Empresa.ID,
                    };
                    dbSIM.DDECONO_CATEGORIA_TERCERO.Add(_categoriaEmpresa);

                    var _municipioEmpresa = new DDECONO_MUNICIPIO_TERCERO
                    {
                         DDECONO_MUNICIPIOID = idMunicipo,
                         DDECONO_TERCERODEID = _Empresa.ID,
                    };
                    dbSIM.DDECONO_MUNICIPIO_TERCERO.Add(_municipioEmpresa);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría almacenada correctamente" };
        }


        [HttpGet, ActionName("EliminarEmpresa")]
        public object EliminarEmpresa(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando la Empresa" };
            try
            {
                int Id = -1;
                if (objData != null && objData != "") Id = int.Parse(objData);
                if (Id > 0)
                {
                    var empresa = this.dbSIM.DDECONO_TERCERODE.Where(f => f.ID == Id).FirstOrDefault();
                    var categoriaTer = this.dbSIM.DDECONO_CATEGORIA_TERCERO.Where(f => f.DDECONO_TERCERODEID == empresa.ID);
                    var municipioTer = this.dbSIM.DDECONO_MUNICIPIO_TERCERO.Where(f => f.DDECONO_TERCERODEID == empresa.ID);
                    this.dbSIM.DDECONO_CATEGORIA_TERCERO.RemoveRange(categoriaTer);
                    this.dbSIM.DDECONO_MUNICIPIO_TERCERO.RemoveRange(municipioTer);
                    this.dbSIM.DDECONO_TERCERODE.Remove(empresa);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Empresa eliminada correctamente!!" };
        }

        [HttpGet, ActionName("GetCategorias")]
        public JArray GetCategorias()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.DDECONO_CATEGORIA
                             orderby Mod.S_NOMBRE
                             select new
                             {
                                 Id = (int)Mod.ID,
                                 Nombre = Mod.S_NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("GetMunicipios")]
        public JArray GetMunicipios()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.DDECONO_MUNICIPIO
                             orderby Mod.S_NOMBRE
                             select new
                             {
                                 Id = (int)Mod.ID,
                                 Nombre = Mod.S_NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("GetUnidadesMedida")]
        public JArray GetUnidadesMedida()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.DDECONO_UNIDAD_MEDIDA
                             orderby Mod.S_NOMBRE
                             select new
                             {
                                 Id = (int)Mod.ID,
                                 Nombre = Mod.S_NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Consulta de Lista de Unidades de Medida
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
        [HttpGet, ActionName("GetProductos")]
        public datosConsulta GetProductos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords,int Id)
        {
            dynamic model = null;
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();

            model = dbSIM.TDECONO_PRODUCTO.Where(f => f.DDECONO_TERCERODEID == Id).OrderBy(f => f.S_NOMBRE);
            modelData = model;
            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [HttpGet, ActionName("ObtenerProducto")]
        public JObject ObtenerProducto(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var producto = this.dbSIM.TDECONO_PRODUCTO.Include(i => i.DDECONO_UNIDAD_MEDIDA).Where(f => f.ID == _Id).FirstOrDefault();

                Producto Producto = new Producto();
                Producto.Id = producto.ID;
                Producto.DescripcionProducto = producto.S_DESCRIPCION;
                Producto.NombreProducto = producto.S_NOMBRE;
                Producto.TerceroId = producto.DDECONO_TERCERODEID;
                Producto.UrlImagen = producto.S_URL_IMAGEN;
                Producto.UnidadMed = producto.DDECONO_UNIDAD_MEDIDA.S_NOMBRE;
                Producto.ValorUnitario = producto.N_VALOR_UNIDAD;
                Producto.Activo = producto.B_ACTIVO == "1" ? true : false;
                return JObject.FromObject(Producto, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("GuardarProducto")]
        public object GuardarProducto(Producto objData)
        {
            if (!ModelState.IsValid) return null;
            int idUnidadMed = 0;
            int.TryParse(objData.UnidadMed, out idUnidadMed);
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                string _Estado = objData.Activo ? "1" : "0";
                if (Id > 0)
                {
                    var _Producto = dbSIM.TDECONO_PRODUCTO.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Producto != null)
                    {
                        _Producto.DDECONO_TERCERODEID = objData.TerceroId;
                        _Producto.DDECONO_UNIDAD_MEDIDAID = idUnidadMed;
                        _Producto.S_NOMBRE = objData.NombreProducto;
                        _Producto.S_DESCRIPCION = objData.DescripcionProducto;
                        _Producto.S_URL_IMAGEN = objData.UrlImagen;
                        _Producto.N_VALOR_UNIDAD = objData.ValorUnitario;
                        _Producto.B_ACTIVO = objData.Activo == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TDECONO_PRODUCTO _Producto = new TDECONO_PRODUCTO
                    {
                        DDECONO_TERCERODEID = objData.TerceroId,
                        DDECONO_UNIDAD_MEDIDAID = idUnidadMed,
                        S_NOMBRE = objData.NombreProducto,
                        S_DESCRIPCION = objData.DescripcionProducto,
                        S_URL_IMAGEN = objData.UrlImagen,
                        N_VALOR_UNIDAD = objData.ValorUnitario,
                        B_ACTIVO = objData.Activo == true ? "1" : "0",
                    };
                    dbSIM.TDECONO_PRODUCTO.Add(_Producto);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Categoría almacenada correctamente" };
        }

        [HttpGet, ActionName("EliminarProducto")]
        public object EliminarProducto(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el Producto" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var producto = this.dbSIM.TDECONO_PRODUCTO.Where(f => f.ID == Id).FirstOrDefault();

                    this.dbSIM.TDECONO_PRODUCTO.Remove(producto);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Producto eliminado correctamente!!" };
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
