using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ExpedienteAmbiental.Clases;
using SIM.Areas.ExpedienteAmbiental.Models;
using SIM.Areas.ExpedienteAmbiental.Models.DTO;
using SIM.Data;
using SIM.Models;
using SIM.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;


namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    /// <summary>
    /// Controlador
    /// </summary>
    [Route("api/[controller]", Name = "ExpedientesApi")]
    public class ExpedientesAmbApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiTerceros = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();
        private string urlApiGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UrlApiGateWay").ToString());
        private string userApiExpAGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiExpAGateWay").ToString());
        private string userApiExpAGateWayS = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiExpAGateWayS").ToString());
        private string urlApiExpedienteAmbiental = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioExpedienteAmbiental").ToString();
        private string urlApiSecurity = SIM.Utilidades.Data.ObtenerValorParametro("urlApiSecurity").ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certification"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
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

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
            if (response.ExpiresIn == 0) return datosConsulta;


            SIM.Models.Response responseS = await apiService.GetListAsync<ExpedienteAmbientalDTO>(this.urlApiGateWay, "ExpA/ExpedienteAmbiental/", "ObtenerExpedientesAmbientales", response.JwtToken);
            if (!responseS.IsSuccess) return datosConsulta;
            var list = (List<ExpedienteAmbientalDTO>)responseS.Result;
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
        /// Almacena la Información de un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarExpedienteAmbientalAsync")]
        public async Task<object> GuardarExpedienteAmbientalAsync(ExpedienteAmbientalDTO objData)
        {
            SIM.Models.Response resposeF = new SIM.Models.Response();

            try
            {
                ApiService apiService = new ApiService();

                objData.FechaRegistro = DateTime.Now;

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return resposeF;

                decimal Id = 0;

                if (objData.idExpediente == -1) objData.idExpediente = 0;

                Id = objData.idExpediente;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<ExpedienteAmbientalDTO>(this.urlApiGateWay, "", "ExpA/ExpedienteAmbiental/ActualizarExpedienteAmbiental", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return resposeF;
                }
                else if (Id <= 0)
                {
                    objData.proyectoId = 0;
                    resposeF = await apiService.PostAsync<ExpedienteAmbientalDTO>(this.urlApiGateWay, "ExpA/", "ExpedienteAmbiental/GuardarExpedienteAmbiental", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return resposeF;
                }
            }
            catch (Exception e)
            {
                return new SIM.Models.Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return JObject.FromObject(expedienteAmbiental, Js);



                SIM.Models.Response responsef = await apiService.GetAsync<ExpedienteAmbientalDTO>(urlApiGateWay, "ExpA/ExpedienteAmbiental/", $"ObtenerExpedienteAmbiental?idExpedienteAmbiental={Id}", response.JwtToken);
                if (!responsef.IsSuccess) return JObject.FromObject(expedienteAmbiental, Js);
                expedienteAmbiental = (ExpedienteAmbientalDTO)responsef.Result;
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


                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                var resp = await apiService.DeleteAsync<ExpedienteAmbientalDTO>(urlApiGateWay, "ExpA/ExpedienteAmbiental/", "BorrarExpedienteAmbiental", expediente, response.JwtToken);
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

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
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
                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;


                var responseF = await apiService.GetListAsync<ClasificacionExpedienteDTO>(this.urlApiGateWay, "ExpA/ClasificacionExpediente/", $"ObtenerClasificacionesExpedientesAmbientales", response.JwtToken);
                if (!responseF.IsSuccess) return null;
                var list = (List<ClasificacionExpedienteDTO>)responseF.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Retorna el Listado de Municipios
        /// </summary>
        /// <returns></returns>
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return datosConsulta;

                var responseF = await apiService.GetListAsync<PuntoControlDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", $"ObtenerPuntosControlDeExpedienteAmbiental/expedienteAmbientalId/{idExpediente}", response.JwtToken);
                if (!responseF.IsSuccess) return datosConsulta;
                var list = (List<PuntoControlDTO>)responseF.Result;
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                var responseF = await apiService.GetListAsync<TipoSolicitudAmbientalDTO>(this.urlApiGateWay, "ExpA/TipoSolicitudAmbiental/", $"ObtenerTiposSolicitudesAmbientales", response.JwtToken);
                if (!responseF.IsSuccess) return null;
                var list = (List<TipoSolicitudAmbientalDTO>)responseF.Result;


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
            SIM.Models.Response resposeF = new SIM.Models.Response();
            try
            {
                decimal Id = 0;
                Id = objData.IdPuntoControl;
                int idUsuario = 0;
                int funcionario = 0;

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

                ApiService apiService = new ApiService();

                objData.IndicesSerieDocumentalDTO = new List<IndiceSerieDocumentalDTO>();
                objData.UnidadDocumentalId =  int.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdCodSerieHistoriasAmbientales").ToString());

                if (Id <= 0)
                {
                    foreach (var item in objData.Indices)
                    {

                        var indE = new IndiceSerieDocumentalDTO();
                        indE.IndiceSerieDocumentaId = item.CODINDICE;
                        indE.ValorString = item.VALOR;
                        switch (item.TIPO)
                        {
                            case 0: //Texto
                            case 3:
                            case 4:
                            case 5:
                            case 8:
                                indE.ValorString = item.VALOR ?? "";
                                break;
                            case 1: //Numero
                            case 6:
                            case 7:
                                indE.ValorNumerico = decimal.Parse(item.VALOR);
                                break;
                            case 2: //Fecha
                                indE.ValorFecha = DateTime.Parse(item.VALOR);
                                break;
                        }

                        objData.IndicesSerieDocumentalDTO.Add(indE);
                    };
                }

                objData.FechaRegistro = DateTime.Now;
                objData.Conexo = ".";
                objData.ObservacionEstado = "";
                objData.FuncionarioId = funcionario;


                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;


                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<PuntoControlDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", "ActualizarPuntoControl", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return resposeF;
                }
                else if (Id <= 0)
                {
                    resposeF = await apiService.PostAsync<PuntoControlDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", "GuardarPuntoControl", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return resposeF;

                }
            }
            catch (Exception e)
            {
                return new SIM.Models.Response { IsSuccess= false, Message = "Error Almacenando el registro : " + e.Message, Result = "" };
            }

            return resposeF;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codSerie"></param>
        /// <param name="codExpediente"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesSerieDocumental")]
        public async Task<dynamic> ObtenerIndicesSerieDocumental(int codSerie, int codExpediente)
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

            ApiService apiService = new ApiService();
            ExpedienteAmbientalDTO expedienteAmbiental = new ExpedienteAmbientalDTO();


            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
            if (response.ExpiresIn == 0) return listaInd;

            var responseF = await apiService.GetAsync<ExpedienteAmbientalDTO>(this.urlApiGateWay, "ExpA/ExpedienteAmbiental/", $"ObtenerExpedienteAmbiental?idExpedienteAmbiental={codExpediente}", response.JwtToken);
            if (!responseF.IsSuccess) return listaInd;

            expedienteAmbiental = (ExpedienteAmbientalDTO)responseF.Result;
            foreach (var item in listaInd)
            {
                if (item.INDICE == "CM")
                {
                    item.VALOR = expedienteAmbiental.cm;
                }

                if (item.INDICE == "DIRECCIÓN")
                {
                    item.VALOR = expedienteAmbiental.direccion;
                }
                if (item.INDICE == "NOMBRE ESPECÍFICO")
                {
                    item.VALOR = expedienteAmbiental.nombre;
                }
                if (item.INDICE == "MUNICIPIO")
                {
                    item.VALOR = expedienteAmbiental.municipio;
                }
            }


            return listaInd;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="codExpedienteDocumental"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet, ActionName("ObtenerIndicesExpedienteDocumental")]
        public dynamic ObtenerIndicesExpedienteDocumental(int codExpedienteDocumental)
        {
            var IdCodSerie = dbSIM.EXP_EXPEDIENTES.Where(f => f.ID_EXPEDIENTE == codExpedienteDocumental).Select(s => s.ID_UNIDADDOC).FirstOrDefault();
            List<Indice> indices = new List<Indice>();
            var indicesS = from i in dbSIM.TBINDICESERIE
                           join lista in dbSIM.TBSUBSERIE on (decimal)i.CODIGO_SUBSERIE equals lista.CODIGO_SUBSERIE into l
                           from pdis in l.DefaultIfEmpty()
                           where i.CODSERIE == IdCodSerie && i.MOSTRAR == "1" && i.INDICE_RADICADO == null
                           orderby i.ORDEN
                           select new Indice
                           {
                               CODINDICE = i.CODINDICE,
                               INDICE = i.INDICE,
                               TIPO = i.TIPO,
                               LONGITUD = i.LONGITUD,
                               OBLIGA = i.OBLIGA,
                               VALORDEFECTO = i.VALORDEFECTO,
                               VALOR = dbSIM.EXP_INDICES.Where(w => w.ID_EXPEDIENTE == (decimal)codExpedienteDocumental && w.CODINDICE == i.CODINDICE).Select(s => s.VALOR_TXT).FirstOrDefault(),
                               ID_LISTA = i.CODIGO_SUBSERIE,
                               TIPO_LISTA = pdis.TIPO,
                               CAMPO_NOMBRE = pdis.CAMPO_NOMBRE,
                               MAXIMO = i.VALORMAXIMO.Length > 0 ? i.VALORMAXIMO : "",
                               MINIMO = i.VALORMINIMO.Length > 0 ? i.VALORMINIMO : ""
                           };
            var li = indicesS.ToList();
            return li;
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


                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return JObject.FromObject(puntoControl, Js);

                var responseF = await apiService.GetAsync<PuntoControlDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", $"ObtenerPuntoControl/idPuntoControl/{Id}", response.JwtToken);
                if (!responseF.IsSuccess) return JObject.FromObject(puntoControl, Js);

                puntoControl = (PuntoControlDTO)responseF.Result;
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
                    Nombre = ".",
                    IndicesSerieDocumentalDTO= new List<IndiceSerieDocumentalDTO>(),
                    UnidadDocumentalId = 0,
                    TipoSolicitudAmbientalId = 0,
                    ExpedienteAmbientalId = 0,
                };

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;


                var resp = await apiService.PostAsync<PuntoControlDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", $"BorrarPuntoControll/", puntoControl, response.JwtToken);

                return resp.Result;
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
            SIM.Models.Response resposeF = new SIM.Models.Response();
            try
            {
                ApiService apiService = new ApiService();

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                resposeF = await apiService.PutAsync<PuntoControlExpedienteDocumentalDTO>(this.urlApiGateWay, "ExpA/PuntoControl/", "VincularExpedienteDocumentalAPuntoControl", objData, response.JwtToken);
                if (!resposeF.IsSuccess) return new SIM.Models.Response { IsSuccess= false, Message = "Error Vinculando el Expediente!" };
            }
            catch (Exception e)
            {
                return new SIM.Models.Response { IsSuccess= false, Message = "Error Vinculando el Expediente: " + e.Message };
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return datosConsulta;

                var responseF = await apiService.GetListAsync<EstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/EstadoPuntoControl/", $"ObtenerEstadosPuntosControl?puntoControlId={idPuntoControl}", response.JwtToken);
                if (!responseF.IsSuccess) return datosConsulta;
                var list = (List<EstadoPuntoControlDTO>)responseF.Result;
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return JObject.FromObject(estadoPuntoControl, Js);

                var responseF = await apiService.GetAsync<EstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/EstadoPuntoControl/", $"ObtenerEstadoPuntoControl?idEstadoPuntoControl={Id}", response.JwtToken);
                if (!responseF.IsSuccess) return JObject.FromObject(estadoPuntoControl, Js);
                estadoPuntoControl = (EstadoPuntoControlDTO)responseF.Result;
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
                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                var responseF = await apiService.GetListAsync<TipoEstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/TipoEstadoPuntoControl/", $"ObtenerTiposEstadosPuntoControl", response.JwtToken);
                if (!responseF.IsSuccess) return null;
                var list = (List<TipoEstadoPuntoControlDTO>)responseF.Result;

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
            SIM.Models.Response resposeF = new SIM.Models.Response();
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                decimal Id = -1;
                Id = objData.IdEstadoPuntoControl;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<EstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/EstadoPuntoControl/", "ActualizarEstadoPuntoControl", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    resposeF = await apiService.PostAsync<EstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/EstadoPuntoControl/", "GuardarEstadoPuntoControl", objData, response.JwtToken);
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                var resp = await apiService.DeleteAsync<EstadoPuntoControlDTO>(this.urlApiGateWay, "ExpA/EstadoPuntoControl/", "BorrarEstadoPuntoControll", puntoControl, response.JwtToken);
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return datosConsulta;

                var responseF = await apiService.GetListAsync<AnotacionPuntoControlDTO>(this.urlApiGateWay, "ExpA/AnotacionPuntoControl/", $"ObtenerAnotacionesPuntosControl?puntoControlId={idPuntoControl}", response.JwtToken);
                if (!responseF.IsSuccess) return datosConsulta;
                var list = (List<AnotacionPuntoControlDTO>)responseF.Result;
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return JObject.FromObject(anotacionPuntoControl, Js);

                var responseF = await apiService.GetAsync<AnotacionPuntoControlDTO>(this.urlApiGateWay, "ExpA/AnotacionPuntoControl/", $"ObtenerAnotacionPuntoControl?idAnotacionPuntoControl={Id}", response.JwtToken);
                if (!responseF.IsSuccess) return JObject.FromObject(anotacionPuntoControl, Js);
                anotacionPuntoControl = (AnotacionPuntoControlDTO)responseF.Result;
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
            SIM.Models.Response resposeF = new SIM.Models.Response();
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                decimal Id = -1;
                Id = objData.IdAnotacionPuntoControl;
                if (Id > 0)
                {
                    resposeF = await apiService.PutAsync<AnotacionPuntoControlDTO>(this.urlApiGateWay, "ExpA/AnotacionPuntoControl/", "ActualizarAnotacionPuntoControl", objData, response.JwtToken);
                    if (!resposeF.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    resposeF = await apiService.PostAsync<AnotacionPuntoControlDTO>(this.urlApiGateWay, "ExpA/AnotacionPuntoControl/", "GuardarAnotacionPuntoControl", objData, response.JwtToken);
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

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return new { resp = "Error", mensaje = "No es posible autenticar la aplicación" };

                var resp = await apiService.PostAsync<AnotacionPuntoControlDTO>(this.urlApiGateWay, "ExpA/AnotacionPuntoControl/", "BorrarAnotacionPuntoControll/", anotacion, response.JwtToken);
                if (!resp.IsSuccess) return new { resp = "Error", mensaje = resp.Message };

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        #endregion

        #region Trámites asociados al Expediente Documental


        /// <summary>
        /// Consulta de Lista de Trámites asociados al Expediente Ambiental
        /// </summary>
        ///<param name="codExpediente"></param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetTramitesExpedienteAsync")]
        public JArray GetTramitesExpedienteAsync(int codExpediente)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            var model = dbSIM.Database.SqlQuery<TramitesExpedienteDTO>(@"SELECT distinct tea.codtramite as CODTRAMITE, CODIGO_PROYECTO , FECHAINI,FECHAFIN, p.nombre AS PROYECTO,t.COMENTARIOS as COMENTARIOS,T.MENSAJE AS MENSAJE, decode(estado, 0, 'Abierto', 'Cerrado') as ESTADO " +
                      " from TRAMITES.TRAMITE_EXPEDIENTE_AMBIENTAL tea " +
                      " inner join TRAMITES.TBTRAMITE t on t.codtramite = tea.codtramite " +
                      " inner join TRAMITES.TBPROCESO p on p.codproceso = t.codproceso " +
                      " where CODIGO_SOLICITUD IS NULL AND CODIGO_PROYECTO = " + codExpediente.ToString());

            return JArray.FromObject(model.ToList(), Js);
        }


        /// <summary>
        /// Consulta de Lista de Trámites asociados al Punto de Control
        /// </summary>
        ///<param name="codigoSolicitudId"></param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("GetTramitesPuntoAsync")]
        public JArray GetTramitesPuntoAsync(int codigoSolicitudId)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            var model = dbSIM.Database.SqlQuery<TramitesExpedienteDTO>(@"SELECT distinct tea.codtramite as CODTRAMITE, CODIGO_PROYECTO , FECHAINI,FECHAFIN, p.nombre AS PROYECTO,t.COMENTARIOS as COMENTARIOS,T.MENSAJE AS MENSAJE, decode(estado, 0, 'Abierto', 'Cerrado') as ESTADO " +
                      " from TRAMITES.TRAMITE_EXPEDIENTE_AMBIENTAL tea " +
                      " inner join TRAMITES.TBTRAMITE t on t.codtramite = tea.codtramite " +
                      " inner join TRAMITES.TBPROCESO p on p.codproceso = t.codproceso " +
                      " where CODIGO_SOLICITUD  = " + codigoSolicitudId.ToString());


            return JArray.FromObject(model.ToList(), Js);
        }

        #endregion

        #region Abogado asociados al Expediente

        /// <summary>
        /// Retorna los abogados asociados a un Expediente Ambiental determinado
        /// </summary>
        /// <param name="idExpediente">Identifica el Expediente Ambiental</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetAbogadosExpedienteAsync")]
        public async Task<JArray> GetAbogadosExpedienteAsync(string idExpediente)
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idExpediente)) _Id = int.Parse(idExpediente);

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                var responseF = await apiService.GetListAsync<AbogadoExpedienteDTO>(this.urlApiGateWay, "ExpA/AbogadoExpediente/", $"ObtenerAbogadosExpediente?expedienteId={idExpediente}", response.JwtToken);
                if (!responseF.IsSuccess) return null;
                var list = (List<AbogadoExpedienteDTO>)responseF.Result;

                var ja = JArray.FromObject(list, Js);
                return ja;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Almacena la Información de un Abodago asociado a un Expediente Ambiental
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("AsignarExpedienteAmbientalAsync")]
        public async Task<object> AsignarExpedienteAmbientalAsync(AbogadoExpedienteDTO objData)
        {
            SIM.Models.Response resposeF = new SIM.Models.Response();
            try
            {
                objData.FechaAsignacion = DateTime.Now;
                objData.Observacion = "";
                objData.Abogado = "";
                ApiService apiService = new ApiService();

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiExpAGateWayS, UserName = this.userApiExpAGateWay });
                if (response.ExpiresIn == 0) return null;

                resposeF = await apiService.PostAsync<AbogadoExpedienteDTO>(this.urlApiGateWay, "ExpA/AbogadoExpediente/", "GuardarAbogadoExpediente", objData, response.JwtToken);
                if (!resposeF.IsSuccess) return new SIM.Models.Response { IsSuccess= false, Message = "Error Vinculando el Abogado!" };
            }
            catch (Exception e)
            {
                return new SIM.Models.Response { IsSuccess= false, Message = "Error Vinculando el Abogado: " + e.Message };
            }

            return resposeF;
        }


        /// <summary>
        /// Retorna el listado de los tipos de Abogados
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerAbogadosAsync")]
        public async Task<JArray> ObtenerAbogadosAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = await (from Mod in dbSIM.QRY_ABOGADOS
                                   orderby Mod.NOMBRES
                                   select new AbogadoDTO
                                   {
                                       IdAbogado = (int)Mod.CODFUNCIONARIO,
                                       Nombre = Mod.NOMBRES
                                   }).ToListAsync();
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        #endregion
    }
}

