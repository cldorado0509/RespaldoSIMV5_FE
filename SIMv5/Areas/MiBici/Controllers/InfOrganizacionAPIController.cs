namespace SIM.Areas.MiBici.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.MiBici.Models;
    using SIM.Data;
    using SIM.Data.MiBici;

    [Route("api/[controller]", Name = "InfOrganizacionAPI")]
    public class InfOrganizacionAPIController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        /// <summary>
        /// Retorna el Listado de las Instalaciones que pertenecen a un Tercero dado
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetIntalacionesTercero")]
        public datosConsulta GetIntalacionesTercero(int TerceroId)
        {
            datosConsulta resultado = new datosConsulta();
            try
            {
                JsonSerializer Js = new JsonSerializer();
                Js = JsonSerializer.CreateDefault();
                var instalaciones = dbSIM.QRYINSTALACION_TERCERO
                    .Where(f => f.ID_TERCERO == TerceroId).OrderBy(f => f.INSTALACION).ToList();

                resultado.numRegistros = instalaciones.Count();
                resultado.datos = instalaciones;
                resultado.Status = true;

                return resultado;
            }
            catch (Exception exp)
            {
                resultado.numRegistros = 0;
                resultado.MessageError = exp.Message;
                resultado.Status = false;
                return resultado;
            }
        }

               
        

        [HttpGet, ActionName("GetFotoInstalacion")]
        public FotoModel GetFotoInstalacion(string IdInstalacion, string IdTercero)
        {
            try
            {

                int _IdInstalacion = -1;
                if (!string.IsNullOrEmpty(IdInstalacion)) _IdInstalacion = int.Parse(IdInstalacion);

                int _IdTercero = -1;
                if (!string.IsNullOrEmpty(IdTercero)) _IdTercero = int.Parse(IdTercero);

                var _detalleEmpresa = this.dbSIM.DMIBICI_DETALLE_EMPRESA.Where(f => f.ID_TERCERO == _IdTercero && f.ID_INSTALACION == _IdInstalacion).FirstOrDefault();

                return new FotoModel
                {
                    Objid = _IdInstalacion,
                    photo_file = _detalleEmpresa.I_FOTO_EMPRESA,
                    photo_id = _detalleEmpresa.S_DESCRIPCION,

                };
            }
            catch (HttpResponseException exps)
            {
                HttpResponseMessage msgS = new HttpResponseMessage();
                msgS.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                msgS.ReasonPhrase = "Evento no esperado al cargar el logotipo de la empresa!";
                msgS.Content = new StringContent("There are currently no photos listed under this shop!");
                throw new HttpResponseException(msgS);
            }
        }


        [HttpGet, ActionName("ObtenerInformacionOrganizacion")]
        public JObject ObtenerInformacionOrganizacion(string IdInstalacion,string IdTercero)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _IdInstalacion = -1;
                if (!string.IsNullOrEmpty(IdInstalacion)) _IdInstalacion = int.Parse(IdInstalacion);

                int _IdTercero = -1;
                if (!string.IsNullOrEmpty(IdTercero)) _IdTercero = int.Parse(IdTercero);

                var _detalleEmpresa = this.dbSIM.DMIBICI_DETALLE_EMPRESA.Where(f => f.ID_TERCERO == _IdTercero && f.ID_INSTALACION == _IdInstalacion).FirstOrDefault();

                DetalleEmpresa detalleEmpresa = new DetalleEmpresa
                {
                    IdTercero = _IdTercero,
                    IdInstalacion = _IdInstalacion,
                    IdCategoria = 1,
                    Descripcion = "",
                    Direccion = "",
                    EMail = "",
                    Facebook = "",
                    Instagram = "",
                    SitioWeb = "",
                    Telefono = "",
                    WhatsApp = "",
                };
                if (_detalleEmpresa != null)
                {
                    detalleEmpresa.Descripcion = _detalleEmpresa.S_DESCRIPCION;
                    detalleEmpresa.Direccion = _detalleEmpresa.S_DIRECCION;
                    detalleEmpresa.EMail = _detalleEmpresa.S_EMAIL_VENTAS;
                    detalleEmpresa.Facebook = _detalleEmpresa.S_CTA_FACEBOOK;
                    detalleEmpresa.Foto = _detalleEmpresa.I_FOTO_EMPRESA;
                    detalleEmpresa.IdCategoria = _detalleEmpresa.ID_CATEGORIA;
                    detalleEmpresa.Instagram = _detalleEmpresa.S_CTA_INSTAGRAM;
                    detalleEmpresa.Logo = _detalleEmpresa.I_LOGOTIPO;
                    detalleEmpresa.SitioWeb = _detalleEmpresa.S_SITIOWEB;
                    detalleEmpresa.Telefono = _detalleEmpresa.S_TELEFONO;
                    detalleEmpresa.WhatsApp = _detalleEmpresa.S_WHATSAPP;
                }
               
                return JObject.FromObject(detalleEmpresa, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpPost, ActionName("GuardarInformacion")]
        public object GuardarInformacion(DetalleEmpresa objData)
        {
            if (!ModelState.IsValid) return null;
            byte[] _FileContentPhoto = null;
            try
            {
                string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() : "";
                string FilePhoto = $"{_RutaBase}Foto{(int)objData.IdInstalacion}";

                DirectoryInfo di = new DirectoryInfo(_RutaBase);
                if (!di.Exists)
                {
                    di.Create();
                }
                foreach (var fi in di.GetFiles())
                {
                    if (fi.Name.Contains($"Foto{(int)objData.IdInstalacion}"))
                    {
                        FilePhoto = $"{FilePhoto}.{fi.Extension}";
                        _FileContentPhoto = SIM.Utilidades.Archivos.LeeArchivo(FilePhoto);
                        System.IO.File.Delete(fi.FullName);
                    }
                }
                

                var _InfoE = dbSIM.DMIBICI_DETALLE_EMPRESA.Where(f => f.ID_TERCERO == objData.IdTercero && f.ID_INSTALACION == objData.IdInstalacion).FirstOrDefault();
                if (_InfoE != null)
                {
                    _InfoE.S_DESCRIPCION = objData.Descripcion;
                    _InfoE.ID_CATEGORIA = (int)objData.IdCategoria;
                    _InfoE.ID_INSTALACION = (int)objData.IdInstalacion;
                    _InfoE.ID_TERCERO = (int)objData.IdTercero;
                    _InfoE.S_CTA_FACEBOOK = objData.Facebook;
                    _InfoE.S_CTA_INSTAGRAM = objData.Instagram;
                    _InfoE.S_DIRECCION = objData.Direccion;
                    _InfoE.S_EMAIL_VENTAS = objData.EMail;
                    _InfoE.S_SITIOWEB = objData.SitioWeb;
                    _InfoE.S_TELEFONO = objData.Telefono;
                    _InfoE.S_WHATSAPP = objData.WhatsApp;
                    _InfoE.I_FOTO_EMPRESA = _FileContentPhoto != null ? _FileContentPhoto : _InfoE.I_FOTO_EMPRESA;
                }
                else
                {
                    DMIBICI_DETALLE_EMPRESA _InfoEN = new DMIBICI_DETALLE_EMPRESA
                    {
                        S_DESCRIPCION = objData.Descripcion,
                        ID_CATEGORIA = (int)objData.IdCategoria,
                        ID_INSTALACION = (int)objData.IdInstalacion,
                        ID_TERCERO = (int)objData.IdTercero,
                        S_CTA_FACEBOOK = objData.Facebook,
                        S_CTA_INSTAGRAM = objData.Instagram,
                        S_DIRECCION = objData.Direccion,
                        S_EMAIL_VENTAS = objData.EMail,
                        S_SITIOWEB = objData.SitioWeb,
                        S_TELEFONO = objData.Telefono,
                        S_WHATSAPP = objData.WhatsApp,
                        I_FOTO_EMPRESA = _FileContentPhoto
                    };
                    dbSIM.DMIBICI_DETALLE_EMPRESA.Add(_InfoEN);
                }
                dbSIM.SaveChanges();
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la información: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Datos almacenados correctamente" };
        }

        [System.Web.Http.HttpPost, System.Web.Http.ActionName("SubirFoto")]
        public async Task<object> SubirFoto(string id)
        {

            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return new { resp = "Error", mensaje = "No se encontraron archivos" };
                string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() : "";
                if (!Directory.Exists(_RutaBase)) Directory.CreateDirectory(_RutaBase);

                var provider = new CODMultipartFormDataStreamProvider(_RutaBase);
                await Request.Content.ReadAsMultipartAsync(provider);
                if (provider.Contents.Count == 0) return false;

                string _RutaOrigen = "";

                foreach (MultipartFileData _File in provider.FileData)
                {
                    _RutaOrigen = _RutaBase + _File.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    string _Ext = Path.GetExtension(_File.LocalFileName);
                    string _RutaDest = $"{_RutaBase}{id}.{_Ext}";
                    if (File.Exists(_RutaDest)) File.Delete(_RutaDest);

                    File.Move(_RutaOrigen, _RutaDest);
                    System.IO.File.Delete(_RutaOrigen);
                }
                return new { resp = "OK", mensaje = "Archivos subidos con exito" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos " + e.Message };
            }

        }

        public async Task<object> SubirFotoP(string id)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return new { resp = "Error", mensaje = "No se encontraron archivos" };
                string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() : "";
                if (!Directory.Exists(_RutaBase)) Directory.CreateDirectory(_RutaBase);

                var provider = new CODMultipartFormDataStreamProvider(_RutaBase);
                await Request.Content.ReadAsMultipartAsync(provider);
                if (provider.Contents.Count == 0) return false;

                string _RutaOrigen = "";

                foreach (MultipartFileData _File in provider.FileData)
                {
                    _RutaOrigen = _RutaBase + _File.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    string _Ext = Path.GetExtension(_File.LocalFileName);
                    string _RutaDest = $"P{_RutaBase}{id}.{_Ext}";
                    if (File.Exists(_RutaDest)) File.Delete(_RutaDest);

                    File.Move(_RutaOrigen, _RutaDest);
                    System.IO.File.Delete(_RutaOrigen);

                   
                }
                return new { resp = "OK", mensaje = "Archivos subidos con exito" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos " + e.Message };
            }

        }


        
        [HttpPost, ActionName("GuardarFotoProducto")]
        public object GuardarFotoProducto(string id)
        {

            byte[] _FileContentPhoto = null;

            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaTemporalFotosMiBici").ToString() : "";
            string FilePhoto = $"{_RutaBase}Foto{id}";

            DirectoryInfo di = new DirectoryInfo(_RutaBase);
            foreach (var fi in di.GetFiles())
            {
                if (fi.Name.Contains($"Foto{id}"))
                {
                    FilePhoto = $"{FilePhoto}.{fi.Extension}";
                    _FileContentPhoto = SIM.Utilidades.Archivos.LeeArchivo(FilePhoto);
                    System.IO.File.Delete(fi.FullName);
                }
            }


            decimal idProducto = 0;
            decimal.TryParse(id, out idProducto);

            TMIBICI_FOTOS_PRODUCTO _fotoPro = new TMIBICI_FOTOS_PRODUCTO
            {
                TMBICI_PRODUCTOID = idProducto,
                I_FOTO = _FileContentPhoto,
                S_DESCRIPCION = "",
            };
            dbSIM.TMIBICI_FOTOS_PRODUCTOS.Add(_fotoPro);
            dbSIM.SaveChanges();
            System.IO.File.Delete(FilePhoto);

            return new { resp = "OK", mensaje = "Categoría almacenada correctamente" };
        }

        /// <summary>
        /// Consulta de Lista de Productos
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetProductos")]
        public datosConsulta GetProductos(string IdInstalacion, string IdTercero)
        {
            datosConsulta resultado = new datosConsulta();
            try
            {
                int _IdInstalacion = -1;
                if (!string.IsNullOrEmpty(IdInstalacion)) _IdInstalacion = int.Parse(IdInstalacion);

                int _IdTercero = -1;
                if (!string.IsNullOrEmpty(IdTercero)) _IdTercero = int.Parse(IdTercero);

                var productos = dbSIM.TMIBICI_PRODUCTO.Where(f => f.ID_TERCERO == _IdTercero && f.ID_INSTALACION == _IdInstalacion).OrderBy(f => f.S_NOMBRE).ToList();

                resultado.numRegistros = productos.Count();
                resultado.datos = productos;
                return resultado;
            }
            catch (Exception exp)
            {
                resultado.numRegistros = 0;
                resultado.MessageError = exp.Message;
                resultado.Status = false;
                return resultado;
            }
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

                var producto = this.dbSIM.TMIBICI_PRODUCTO.Where(f => f.ID == _Id).FirstOrDefault();

                Producto Producto = new Producto();
                Producto.Id = producto.ID;
                Producto.DescripcionProducto = producto.S_DESCRIPCION;
                Producto.NombreProducto = producto.S_NOMBRE;
                Producto.TerceroId = producto.ID_TERCERO;
                Producto.InstalacionId = producto.ID_INSTALACION;
                Producto.UnidadMed = producto.DMIBICI_UNIDAD_MEDIDA.S_NOMBRE;
                Producto.ValorUnitario = producto.N_VALOR_UNIDAD;
                Producto.Activo = producto.B_MOSTRAR == "1" ? true : false;
                return JObject.FromObject(Producto, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("ObtenerProductoF")]
        public JObject ObtenerProductoF(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var producto = this.dbSIM.TMIBICI_PRODUCTO.Where(f => f.ID == _Id).FirstOrDefault();

                ProductoFotos Producto = new ProductoFotos();
                Producto.Id = producto.ID;
                Producto.DescripcionProducto = producto.S_DESCRIPCION;
                Producto.NombreProducto = producto.S_NOMBRE;
                Producto.TerceroId = producto.ID_TERCERO;
                Producto.InstalacionId = producto.ID_INSTALACION;
                Producto.Fotos = new List<byte[]>();

                var fotos = this.dbSIM.TMIBICI_FOTOS_PRODUCTOS.Where(f => f.TMBICI_PRODUCTOID == producto.ID).ToList();
                foreach(TMIBICI_FOTOS_PRODUCTO fotop in fotos)
                {
                    Producto.Fotos.Add(fotop.I_FOTO);
                }
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
                    var _Producto = dbSIM.TMIBICI_PRODUCTO.Where(f => f.ID == Id).FirstOrDefault();
                    if (_Producto != null)
                    {
                        _Producto.ID_TERCERO = (int)objData.TerceroId;
                        _Producto.ID_INSTALACION = (int)objData.InstalacionId;
                        _Producto.DMIBICI_UNIDAD_MEDIDAID = idUnidadMed;
                        _Producto.S_NOMBRE = objData.NombreProducto;
                        _Producto.S_DESCRIPCION = objData.DescripcionProducto;
                        _Producto.N_VALOR_UNIDAD = (int)objData.ValorUnitario;
                        _Producto.B_MOSTRAR = objData.Activo == true ? "1" : "0";
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TMIBICI_PRODUCTO _Producto = new TMIBICI_PRODUCTO
                    {
                        ID_TERCERO = (int)objData.TerceroId,
                        ID_INSTALACION =  (int)objData.InstalacionId,
                        DMIBICI_UNIDAD_MEDIDAID = idUnidadMed,
                        S_NOMBRE = objData.NombreProducto,
                        S_DESCRIPCION = objData.DescripcionProducto,
                        N_VALOR_UNIDAD = (int)objData.ValorUnitario,
                        B_MOSTRAR = objData.Activo == true ? "1" : "0",
                    };
                    dbSIM.TMIBICI_PRODUCTO.Add(_Producto);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Categoría: " + e.Message,idinstalacion = (int)objData.InstalacionId, idtercero= (int)objData.TerceroId };
            }
            return new { resp = "OK", mensaje = "Categoría almacenada correctamente", idinstalacion = (int)objData.InstalacionId, idtercero = (int)objData.TerceroId };
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
                    var producto = this.dbSIM.TMIBICI_PRODUCTO.Where(f => f.ID == Id).FirstOrDefault();

                    this.dbSIM.TMIBICI_PRODUCTO.Remove(producto);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Producto eliminado correctamente!!" };
        }

        [HttpGet, ActionName("GetUnidadesMedida")]
        public JArray GetUnidadesMedida()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.DMIBICI_UNIDAD_MEDIDA
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

        [HttpGet, ActionName("GetCategorias")]
        public JArray GetCategorias()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.DMIBICI_CATEGORIA
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

        #region Registro de Ventas
        /// <summary>
        /// Consulta de Lista de los registro de Registro de Ventas
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetVentas")]
        public datosConsulta GetVentas(string IdInstalacion, string IdTercero)
        {
            datosConsulta resultado = new datosConsulta();
            try
            {
                int _IdInstalacion = -1;
                if (!string.IsNullOrEmpty(IdInstalacion)) _IdInstalacion = int.Parse(IdInstalacion);

                int _IdTercero = -1;
                if (!string.IsNullOrEmpty(IdTercero)) _IdTercero = int.Parse(IdTercero);

                var productos = dbSIM.TMIBICI_VENTAS_EMPRESAS.Where(f => f.ID_TERCERO == _IdTercero && f.ID_INSTALACION == _IdInstalacion).OrderByDescending(f => f.D_VENTA).ToList();

                resultado.numRegistros = productos.Count();
                resultado.datos = productos;
                return resultado;
            }
            catch (Exception exp)
            {
                resultado.numRegistros = 0;
                resultado.MessageError = exp.Message;
                resultado.Status = false;
                return resultado;
            }
        }

        [HttpGet, ActionName("ObtenerVenta")]
        public JObject ObtenerVenta(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var _venta = this.dbSIM.TMIBICI_VENTAS_EMPRESAS.Where(f => f.ID == _Id).FirstOrDefault();
                if (_venta == null) return null;
                Venta venta = new Venta();
                venta.Id = _venta.ID;
                venta.FechaVenta = _venta.D_VENTA;
                venta.IdentificacionCliente = _venta.S_DOCUMENTO_CLIENTE;
                venta.TerceroId = _venta.ID_TERCERO;
                venta.InstalacionId = _venta.ID_INSTALACION;
                venta.Marca = _venta.S_MARCA;
                venta.Referencia = _venta.S_REFERENCIA;
                venta.Serial = _venta.S_SERIAL;
                return JObject.FromObject(venta, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpPost, ActionName("GuardarVenta")]
        public object GuardarVenta(Venta objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _venta = dbSIM.TMIBICI_VENTAS_EMPRESAS.Where(f => f.ID == Id).FirstOrDefault();
                    if (_venta != null)
                    {
                        _venta.ID_TERCERO = (int)objData.TerceroId;
                        _venta.ID_INSTALACION = (int)objData.InstalacionId;
                        _venta.D_VENTA = objData.FechaVenta;
                        _venta.S_DOCUMENTO_CLIENTE = objData.IdentificacionCliente;
                        _venta.S_MARCA = objData.Marca;
                        _venta.S_REFERENCIA = objData.Referencia;
                        _venta.S_SERIAL = objData.Serial;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TMIBICI_VENTAS_EMPRESA _Venta = new TMIBICI_VENTAS_EMPRESA
                    {
                        ID_TERCERO = (int)objData.TerceroId,
                        ID_INSTALACION = (int)objData.InstalacionId,
                        D_VENTA = objData.FechaVenta,
                        S_DOCUMENTO_CLIENTE = objData.IdentificacionCliente,
                        S_MARCA = objData.Marca,
                        S_REFERENCIA = objData.Referencia,
                        S_SERIAL = objData.Serial,
                    };
                    dbSIM.TMIBICI_VENTAS_EMPRESAS.Add(_Venta);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando la Venta: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Venta almacenada correctamente" };
        }


        [HttpGet, ActionName("EliminarVenta")]
        public object EliminarVenta(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el registro de venta" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var venta = this.dbSIM.TMIBICI_VENTAS_EMPRESAS.Where(f => f.ID == Id).FirstOrDefault();

                    this.dbSIM.TMIBICI_VENTAS_EMPRESAS.Remove(venta);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro de venta eliminado correctamente!!" };
        }
        #endregion

        #region Registro de Eventos
        /// <summary>
        /// Consulta de Lista de los registro de Registro de Eventos
        /// </summary>
        /// <returns>Registros resultado de la consulta</returns>
        [HttpGet, ActionName("GetEventos")]
        public datosConsulta GetEventos(string IdInstalacion, string IdTercero)
        {
            datosConsulta resultado = new datosConsulta();
            try
            {
                int _IdInstalacion = -1;
                if (!string.IsNullOrEmpty(IdInstalacion)) _IdInstalacion = int.Parse(IdInstalacion);

                int _IdTercero = -1;
                if (!string.IsNullOrEmpty(IdTercero)) _IdTercero = int.Parse(IdTercero);

                var eventos = dbSIM.TMIBICI_EVENTOS.Where(f => f.ID_TERCERO == _IdTercero && f.ID_INSTALACION == _IdInstalacion).OrderByDescending(f => f.D_EVENTO).ToList();

                resultado.numRegistros = eventos.Count();
                resultado.datos = eventos;
                return resultado;
            }
            catch (Exception exp)
            {
                resultado.numRegistros = 0;
                resultado.MessageError = exp.Message;
                resultado.Status = false;
                return resultado;
            }
        }

        [HttpGet, ActionName("ObtenerEvento")]
        public JObject ObtenerEvento(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                var _evento = this.dbSIM.TMIBICI_EVENTOS.Where(f => f.ID == _Id).FirstOrDefault();
                if (_evento == null) return null;
                Evento evento = new Evento();
                evento.Id = _evento.ID;
                evento.FechaEvento = _evento.D_EVENTO;
                evento.DescripcionEvento = _evento.S_DESCRIPCION;
                evento.TerceroId = _evento.ID_TERCERO;
                evento.InstalacionId = _evento.ID_INSTALACION;
                evento.Lugar = _evento.S_LUGAR;
                evento.Valor = _evento.N_VALOR;
                evento.Url = _evento.S_URL;
                evento.InformacionContacto= _evento.S_INFORMACION_CONTACTO;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpPost, ActionName("GuardarEVento")]
        public object GuardarEVento(Evento objData)
        {
            if (!ModelState.IsValid) return null;
            try
            {
                decimal Id = -1;
                Id = objData.Id;
                if (Id > 0)
                {
                    var _eventa = dbSIM.TMIBICI_EVENTOS.Where(f => f.ID == Id).FirstOrDefault();
                    if (_eventa != null)
                    {
                        _eventa.ID_TERCERO = (int)objData.TerceroId;
                        _eventa.ID_INSTALACION = (int)objData.InstalacionId;
                        _eventa.D_EVENTO = objData.FechaEvento;
                        _eventa.S_DESCRIPCION = objData.DescripcionEvento;
                        _eventa.S_LUGAR = objData.Lugar;
                        _eventa.N_VALOR = objData.Valor;
                        _eventa.S_URL = objData.Url;
                        _eventa.S_INFORMACION_CONTACTO = objData.InformacionContacto;
                        dbSIM.SaveChanges();
                    }
                }
                else if (Id <= 0)
                {
                    TMIBICI_EVENTOS _Evento = new TMIBICI_EVENTOS
                    {
                        ID_TERCERO = (int)objData.TerceroId,
                        ID_INSTALACION = (int)objData.InstalacionId,
                        D_EVENTO = objData.FechaEvento,
                        S_DESCRIPCION = objData.DescripcionEvento,
                        S_LUGAR = objData.Lugar,
                        N_VALOR = objData.Valor,
                        S_URL = objData.Url,
                        S_INFORMACION_CONTACTO = objData.InformacionContacto,
                    };
                    dbSIM.TMIBICI_EVENTOS.Add(_Evento);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el Evento: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Eventa almacenado correctamente" };
        }


        [HttpGet, ActionName("EliminarEvento")]
        public object EliminarEvento(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el registro del evento" };

            try
            {
                int Id = -1;

                if (objData != null && objData != "") Id = int.Parse(objData);

                if (Id > 0)
                {
                    var evento = this.dbSIM.TMIBICI_EVENTOS.Where(f => f.ID == Id).FirstOrDefault();

                    this.dbSIM.TMIBICI_EVENTOS.Remove(evento);
                    dbSIM.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro de venta eliminado correctamente!!" };
        }
        #endregion

        /// <summary>
        /// Estructura que almacena el resultado de una consulta, entregando los datos y número de registros en total.
        /// </summary>
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
            public bool Status;
            public string MessageError;
        }


    }

    public class CODMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CODMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty);
        }
    }
}
