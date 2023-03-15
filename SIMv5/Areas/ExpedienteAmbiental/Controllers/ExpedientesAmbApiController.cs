using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using DevExpress.Web.ASPxHtmlEditor.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ExpedienteAmbiental.Clases;
using SIM.Areas.ExpedienteAmbiental.Models;
using SIM.Areas.ExpedienteAmbiental.Models.DTO;
using SIM.Data;
using SIM.Data.Seguridad;
using SIM.Models;
using SIM.Services;

namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    /// <summary>
    /// Controlador Expedientes Ambientales APIs
    /// </summary>
    [Route("api/[controller]", Name = "ExpedientesApi")]
    public class ExpedientesAmbApiController : ApiController
    {
      
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        private string urlApiExpedienteAmbiental = "https://amaplicacion02/expedientesambientales/";
        private string urlApiTerceros = " https://sim.metropol.gov.co/tercerosp/";

        private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        //private string urlApiExpedienteAmbiental = " https://localhost:7012/";

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #region Expedientes Ambientales

        /// <summary>
        /// Retorna el Listado de los Expedientes Ambientales
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usuario</param>
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
        [HttpGet, ActionName("GetExpedientesAsync")]
        public async Task<datosConsulta> GetExpedientesAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

            response = await apiService.GetListAsync<ExpedienteAmbientalDTO>(urlApiExpedienteAmbiental, "api/", "ExpedienteAmbiental/ObtenerExpedientesAmbientales", tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ExpedienteAmbientalDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0 && skip == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }


        /// <summary>
        /// Retorna la información de un Expediente Ambiental
        /// </summary>
        /// <param name="Id">Identifica el Expediente Ambiental</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerExpedienteAsync")]
        public async Task<JObject> ObtenerExpedienteAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                ExpedienteAmbientalDTO expedienteAmbiental = new ExpedienteAmbientalDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(expedienteAmbiental, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(expedienteAmbiental, Js);

                response = await apiService.GetAsync<ExpedienteAmbientalDTO>(urlApiExpedienteAmbiental, "api/", $"ExpedienteAmbiental/ObtenerExpedienteAmbiental?idExpedienteAmbiental={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(expedienteAmbiental, Js);
                expedienteAmbiental = (ExpedienteAmbientalDTO)response.Result;
                return JObject.FromObject(expedienteAmbiental, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Elimina un Expediente Ambiente
        /// </summary>
        /// <param name="Id">identifica el Expediente</param>
        /// <returns></returns>
        [HttpPost, ActionName("EliminarExpedienteAsync")]
        public async Task<object> EliminarExpedienteAsync(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return new { resp = "Error", mensaje = "No se identifica el Punto de Control!" };
                int _id = 0;
                int.TryParse(Id, out _id);
                ApiService apiService = new ApiService();

                ExpedienteAmbientalDTO expediente = new ExpedienteAmbientalDTO
                {
                    idExpediente = _id,
                    cm = ".",
                    FechaRegistro = DateTime.Now,
                    clasificacionExpedienteId = 1,
                    descripcion = ".",
                    direccion = ".",
                    InstalacionId = 1,
                    municipioId = 1,
                    proyectoId = 1,
                    nombre = ",",
                    TerceroId = 1,
                };

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.DeleteAsync<ExpedienteAmbientalDTO>(urlApiExpedienteAmbiental, "api/", "ExpedienteAmbiental/BorrarExpedienteAmbiental", expediente, tokenG.Token);
                if (!resp.IsSuccess)
                {
                    return new { resp = "Error", mensaje = "Registro no se pudo eliminar! : " + resp.Message };
                }

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }


        /// <summary>
        /// Retorna la información del tercero asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="Id">Identifica el Expediente Ambiental</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerDatosTerceroExpedienteAsync")]
        public async Task<JObject> ObtenerDatosTerceroExpedienteAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                TercerosDTO terceroExpedienteAmbiental = new TercerosDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(terceroExpedienteAmbiental, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(terceroExpedienteAmbiental, Js);

                response = await apiService.GetAsync<TercerosDTO>(urlApiTerceros, "api/", $"Terceros/ObtenerTerceroXExpediente?id={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(terceroExpedienteAmbiental, Js);
                terceroExpedienteAmbiental = (TercerosDTO)response.Result;
                return JObject.FromObject(terceroExpedienteAmbiental, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna los Puntos de Control asociados a un Expediente Ambiental determinado
        /// </summary>
        /// <param name="idExpediente">Identifica el Expediente Ambiental</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetPuntosControlExpedienteAsync")]
        public async Task<datosConsulta> GetPuntosControlExpedienteAsync(string idExpediente)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idExpediente)) _Id = int.Parse(idExpediente);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<PuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"PuntoControl/ObtenerPuntosControlDeExpedienteAmbiental?expedienteAmbientalId={idExpediente}", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<PuntoControlDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;


                datosConsulta.numRegistros = list.Count;
                datosConsulta.datos = list;

                return datosConsulta;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Almacena la Información de un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarExpedienteAmbientalAsync")]
        public async Task<object> GuardarExpedienteAmbientalAsync(ExpedienteAmbientalDTO objData)
        {
            Response resposeF = new Response();
            try
            {
                ApiService apiService = new ApiService();

                objData.FechaRegistro = DateTime.Now;

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;
                decimal Id = 0;

                if (objData.idExpediente == -1) objData.idExpediente = 0;
               
                Id = objData.idExpediente;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<ExpedienteAmbientalDTO>(urlApiExpedienteAmbiental, "api/", "ExpedienteAmbiental/ActualizarExpedienteAmbiental", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return resposeF;
                }
                else if (Id <= 0)
                {
                    objData.proyectoId = 0;
                    resposeF = await apiService.PostAsync<ExpedienteAmbientalDTO>(urlApiExpedienteAmbiental, "api/", "ExpedienteAmbiental/GuardarExpedienteAmbiental", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return resposeF;
                }
            }
            catch (Exception e)
            {
                return new Response{  IsSuccess = false,  Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
        }


        /// <summary>
        /// Retorna el listado de los tipos de Clasificación de los Expedientes Ambientales
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerClasificacionExpedientesAsync")]
        public async Task<JArray> ObtenerClasificacionExpedientesAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ClasificacionExpedienteDTO>(urlApiExpedienteAmbiental, "api/", $"ClasificacionExpediente/ObtenerClasificacionesExpedientesAmbientales", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ClasificacionExpedienteDTO>)response.Result;
              

                var ja =  JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("ObtenerMunicipios")]
        public JArray ObtenerMunicipios()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.TBMUNICIPIO
                             orderby Mod.NOMBRE
                             select new
                             {
                                 Id = (int)Mod.CODIGO_MUNICIPIO,
                                 Nombre = Mod.NOMBRE
                             }).ToList();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Retorna la información de un Expediente Ambiental
        /// </summary>
        /// <param name="Id">Cédula o Nit del Tercero</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTerceroAsync")]
        public async Task<JObject> ObtenerTerceroAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                long _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = long.Parse(Id);

                TerceroInstalacionDTO tercero = new TerceroInstalacionDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(tercero, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(tercero, Js);

                response = await apiService.GetAsync<TerceroInstalacionDTO>(urlApiTerceros, "api/", $"Terceros/ObtenerTercero?id={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(tercero, Js);
                tercero = (TerceroInstalacionDTO)response.Result;

                return JObject.FromObject(tercero, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Retorna los Puntos de Control asociados a un Expediente Ambiental determinado
        /// </summary>
        /// <param name="idTercero">Identifica el Tercero</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetInstalacionesTerceroAsync")]
        public async Task<datosConsulta> GetInstalacionesTerceroAsync(string idTercero)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idTercero)) _Id = int.Parse(idTercero);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<ListadoInstalacionesDTO>(urlApiTerceros, "api/", $"Terceros/ListadoInstalaciones?id={idTercero}", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoInstalacionesDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;


                datosConsulta.numRegistros = list.Count;
                datosConsulta.datos = list;

                return datosConsulta;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        #endregion

        #region Puntos de Control asociados a un Expediente Ambiental

        /// <summary>
        /// Retorna el listado de los tipos de los Tipos de Solicitud Ambiental
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTiposSolicitudAmbientalAsync")]
        public async Task<JArray> ObtenerTiposSolicitudAmbientalAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<TipoSolicitudAmbientalDTO>(urlApiExpedienteAmbiental, "api/", $"TipoSolicitudAmbiental/ObtenerTiposSolicitudesAmbientales", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<TipoSolicitudAmbientalDTO>)response.Result;


                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        
        /// <summary>
        /// Almacena la Información de un Punto de Control asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarPuntoControllAsync")]
        public async Task<object> GuardarPuntoControllAsync(PuntoControlDTO objData)
        {
            Response resposeF = new Response();
            try
            {
                ApiService apiService = new ApiService();

                objData.FechaRegistro = DateTime.Now;
                objData.Conexo = ".";

                objData.IndicesSerieDocumentalDTO = new List<IndiceSerieDocumentalDTO>();

                if (objData.Indices != null)
                {
                    foreach (Indice indice in objData.Indices)
                    {
                        if (indice.OBLIGA == 1 && (indice.VALOR == null || indice.VALOR == ""))
                        {
                            return new { resp = "Error", mensaje = "Indice " + indice.INDICE + " es obligatorio y no se ingresó un valor!!" };
                        }
                        var indiceS = new IndiceSerieDocumentalDTO
                        {
                            IndiceSerieDocumentaId = indice.CODINDICE,
                            ValorString = indice.VALOR,
                        };

                        switch (indice.TIPO)
                        {
                            case 0: //Texto
                            case 3:
                            case 4:
                            case 5:
                            case 8:
                                indiceS.ValorString = indice.VALOR ?? "";
                                break;
                            case 1: //Numero
                            case 6:
                            case 7:
                                indiceS.ValorNumerico = decimal.Parse(indice.VALOR);
                                break;
                            case 2: //Fecha
                                indiceS.ValorFecha = DateTime.Parse(indice.VALOR);
                                break;
                        }

                        objData.IndicesSerieDocumentalDTO.Add(indiceS);
                    }

                    Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                    if (!response.IsSuccess) return null;
                    var tokenG = (TokenResponse)response.Result;
                    if (tokenG == null) return null;

                    decimal Id = 0;
                    Id = objData.IdPuntoControl;
                    if (Id > 0)
                    {
                        resposeF = await apiService.PutAsync<PuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "PuntoControl/ActualizarPuntoControl", objData, tokenG.Token);
                        if (!resposeF.IsSuccess) return resposeF;
                    }
                    else if (Id <= 0)
                    {
                        resposeF = await apiService.PostAsync<PuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "PuntoControl/GuardarPuntoControl", objData, tokenG.Token);
                        if (!resposeF.IsSuccess) return resposeF;
                    }
                }

            }
            catch (Exception e)
            {
                return new Response{ IsSuccess= false, Message = "Error Almacenando el registro : " + e.Message , Result = ""};
            }

            return resposeF;
        }

        /// <summary>
        /// Retorna la información de un Punto de Control asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="Id">Identifica el Punto de Control</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerPuntoConttrolAsync")]
        public async Task<JObject> ObtenerPuntoConttrolAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                PuntoControlDTO puntoControl = new PuntoControlDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(puntoControl, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(puntoControl, Js);

                response = await apiService.GetAsync<PuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"PuntoControl/ObtenerPuntoControl?idPuntoControl={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(puntoControl, Js);
                puntoControl = (PuntoControlDTO)response.Result;
                return JObject.FromObject(puntoControl, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Elimina un Punto de Control
        /// </summary>
        /// <param name="Id">identifica el Punto de Control</param>
        /// <returns></returns>
        [HttpPost, ActionName("EliminarPuntoControlAsync")]
        public async Task<object> EliminarPuntoControlAsync(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return new { resp = "Error", mensaje = "No se identifica el Punto de Control!" };
                int _id = 0;
                int.TryParse(Id, out _id);
                ApiService apiService = new ApiService();

                PuntoControlDTO puntoControl = new PuntoControlDTO
                {
                    IdPuntoControl = _id,
                    Conexo = ".",
                    Nombre = "."
                };


                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.DeleteAsync<PuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "PuntoControl/BorrarPuntoControll", puntoControl, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        /// <summary>
        /// Almacena la Información de un Punto de Control asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("VincularExpedienteDocumentalAPuntoControllAsync")]
        public async Task<object> VincularExpedienteDocumentalAPuntoControllAsync(PuntoControlExpedienteDocumentalDTO objData)
        {
            Response resposeF = new Response();
            try
            {
                ApiService apiService = new ApiService();
                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;
                    resposeF = await apiService.PutAsync<PuntoControlExpedienteDocumentalDTO>(urlApiExpedienteAmbiental, "api/", "PuntoControl/VincularExpedienteDocumentalAPuntoControl", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return new Response { IsSuccess= false, Message = "Error Vinculando el Expediente!"}; 
            }
            catch (Exception e)
            {
                return new Response{  IsSuccess= false, Message = "Error Vinculando el Expediente: " + e.Message };
            }

            return resposeF;
        }

        #endregion

        #region Estados Punto de Control

        /// <summary>
        /// Retorna los Estados de un Punto de Control
        /// </summary>
        /// <param name="idPuntoControl">Identifica el Punto de Control</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetEstadosPuntoControlExpedienteAsync")]
        public async Task<datosConsulta> GetEstadosPuntoControlExpedienteAsync(string idPuntoControl)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idPuntoControl)) _Id = int.Parse(idPuntoControl);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<EstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"EstadoPuntoControl/ObtenerEstadosPuntosControl?puntoControlId={idPuntoControl}", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<EstadoPuntoControlDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;


                datosConsulta.numRegistros = list.Count;
                datosConsulta.datos = list;

                return datosConsulta;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna la información de un Estado de un Punto de Control asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="Id">Identifica el Estado del Punto de Control</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerEstadoPuntoControlAsync")]
        public async Task<JObject> ObtenerEstadoPuntoControlAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                EstadoPuntoControlDTO estadoPuntoControl = new EstadoPuntoControlDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(estadoPuntoControl, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(estadoPuntoControl, Js);

                response = await apiService.GetAsync<EstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"EstadoPuntoControl/ObtenerEstadoPuntoControl?idEstadoPuntoControl={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(estadoPuntoControl, Js);
                estadoPuntoControl = (EstadoPuntoControlDTO)response.Result;
                return JObject.FromObject(estadoPuntoControl, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Retorna el listado de los tipos de Estado de los Puntos de Control
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTiposEstadoPuntosControlAsync")]
        public async Task<JArray> ObtenerTiposEstadoPuntosControlAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<TipoEstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"TipoEstadoPuntoControl/ObtenerTiposEstadosPuntoControl", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<TipoEstadoPuntoControlDTO>)response.Result;


           
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Almacena la Información de un Estado de un Punto de Control asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarEstadoPuntoControllAsync")]
        public async Task<object> GuardarEstadoPuntoControllAsync(EstadoPuntoControlDTO objData)
        {
            Response resposeF = new Response();
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                int codFuncionario = -1;
                int idUsuario = 0;
                if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                                  join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                                  where uf.ID_USUARIO == idUsuario
                                                  select f.CODFUNCIONARIO).FirstOrDefault());


                ApiService apiService = new ApiService();

                objData.FechaEstado = DateTime.Now;
                objData.FuncionarioId = codFuncionario;

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = objData.IdEstadoPuntoControl;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<EstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "EstadoPuntoControl/ActualizarEstadoPuntoControl", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    resposeF = await apiService.PostAsync<EstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "EstadoPuntoControl/GuardarEstadoPuntoControl", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
        }

        /// <summary>
        /// Elimina un Estado de un Punto de Control
        /// </summary>
        /// <param name="Id">identifica el Estado del Punto de Control</param>
        /// <returns></returns>
        [HttpPost, ActionName("EliminarEstadoPuntoControlAsync")]
        public async Task<object> EliminarEstadoPuntoControlAsync(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return new { resp = "Error", mensaje = "No se identifica el Estado del Punto de Control!" };
                int _id = 0;
                int.TryParse(Id, out _id);
                ApiService apiService = new ApiService();

                EstadoPuntoControlDTO puntoControl = new EstadoPuntoControlDTO
                {
                    IdEstadoPuntoControl = _id,
                     Observacion = "",
                };


                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.DeleteAsync<EstadoPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "EstadoPuntoControl/BorrarEstadoPuntoControll", puntoControl, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        #endregion

        #region Anotaciones Punto de Control
        /// <summary>
        /// Retorna las Anotaciones asociadas a un Punto de Control
        /// </summary>
        /// <param name="idPuntoControl">Identifica el Punto de Control</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetAnotacionesPuntoControlExpedienteAsync")]
        public async Task<datosConsulta> GetAnotacionesPuntoControlExpedienteAsync(string idPuntoControl)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idPuntoControl)) _Id = int.Parse(idPuntoControl);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<AnotacionPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"AnotacionPuntoControl/ObtenerAnotacionesPuntosControl?puntoControlId={idPuntoControl}", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<AnotacionPuntoControlDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;


                datosConsulta.numRegistros = list.Count;
                datosConsulta.datos = list;

                return datosConsulta;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        
        /// <summary>
        /// Retorna la información de una Anotación asociada a un Punto de Control 
        /// 
        /// <param name="Id">Identifica la Anotación vinculada al Punto de Control</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerAnotacionPuntoControlAsync")]
        public async Task<JObject> ObtenerAnotacionPuntoControlAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                AnotacionPuntoControlDTO anotacionPuntoControl = new AnotacionPuntoControlDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(anotacionPuntoControl, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(anotacionPuntoControl, Js);

                response = await apiService.GetAsync<AnotacionPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", $"AnotacionPuntoControl/ObtenerAnotacionPuntoControl?idAnotacionPuntoControl={Id}", tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(anotacionPuntoControl, Js);
                anotacionPuntoControl = (AnotacionPuntoControlDTO)response.Result;
                return JObject.FromObject(anotacionPuntoControl, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Almacena la Información de una Anotación vinculada a un Punto de Control
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarAnotacionPuntoControllAsync")]
        public async Task<object> GuardarAnotacionPuntoControllAsync(AnotacionPuntoControlDTO objData)
        {
            Response resposeF = new Response();
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                int codFuncionario = -1;
                int idUsuario = 0;
                if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                                  join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                                  where uf.ID_USUARIO == idUsuario
                                                  select f.CODFUNCIONARIO).FirstOrDefault());


                ApiService apiService = new ApiService();

                objData.FechaRegistro = DateTime.Now;
                objData.Funcionario = "."; 
                objData.FuncionarioId = codFuncionario;

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = objData.IdAnotacionPuntoControl;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<AnotacionPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "AnotacionPuntoControl/ActualizarEstadoPuntoControl", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    resposeF = await apiService.PostAsync<AnotacionPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "AnotacionPuntoControl/GuardarAnotacionPuntoControl", objData, tokenG.Token);
                    if (!resposeF.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
        }

        
        /// <summary>
        /// Elimina una Anotación de un Punto de Control
        /// </summary>
        /// <param name="Id">identifica la Anotación del Punto de Control</param>
        /// <returns></returns>
        [HttpPost, ActionName("EliminarAnotacionPuntoControlAsync")]
        public async Task<object> EliminarAnotacionPuntoControlAsync(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return new { resp = "Error", mensaje = "No se identifica la Anotación del Punto de Control!" };
                int _id = 0;
                int.TryParse(Id, out _id);
                ApiService apiService = new ApiService();

                AnotacionPuntoControlDTO anotacion = new AnotacionPuntoControlDTO
                {
                    IdAnotacionPuntoControl = _id,
                    Anotacion = ".",
                    FuncionarioId = 1,
                    Funcionario =  ".",
                    PuntoControlId = 1,
                };


                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return new { resp = "Error", mensaje = "No es posible autenticar la aplicación" };
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return new { resp = "Error", mensaje = "No es posible autenticar la aplicación" };

                var resp = await apiService.DeleteAsync<AnotacionPuntoControlDTO>(urlApiExpedienteAmbiental, "api/", "AnotacionPuntoControl/BorrarAnotacionPuntoControll", anotacion, tokenG.Token);
                if (!resp.IsSuccess) return new { resp = "Error", mensaje = resp.Message };

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        #endregion
    }
}

