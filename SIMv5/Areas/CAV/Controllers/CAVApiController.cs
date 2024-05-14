using AMVA.CAV.Core.DTOs;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.CAV.Models;
using SIM.Data;
using SIM.Services;
using SIM.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Response = SIM.Models.Response;

namespace SIM.Areas.CAV.Controllers
{
    /// <summary>
    /// Controlador Apis CAV
    /// </summary>
    [Route("api/[controller]", Name = "CAVApi")]
    public class CAVApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiTerceros = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();
        private string urlApiMicoservicio = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioCAV").ToString();
        private string urlApiGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UrlApiGateWay").ToString());
        private string userApiExpAGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiExpAGateWay").ToString());
        private string userApiExpAGateWayS = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiExpAGateWayS").ToString());
        private string token = string.Empty;
        private ApiService apiService;
        private Response response = new Response();

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public CAVApiController()
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            token = _token.Value;
            apiService = new ApiService();
        }

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

        #region Tipos Adquisición de Fauna

        /// <summary>
        /// Retorna el Listado de los Tipos de Adquisición de FAUNA
        /// </summary>
        /// <param name="opciones"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetTiposAdquisicionAsync")]
        public async Task<LoadResult> GetTiposAdquisicionAsync(DataSourceLoadOptions opciones)
        {
            var opcionesSerialize = JsonConvert.SerializeObject(opciones);

            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            response = await apiService.GetFilteredDataAsync(this.urlApiMicoservicio, "api/TipoAdquisicionEndPoint/", $"GetTiposAdquisicionAsync?opciones={opcionesSerialize}", token);
            if (!response.IsSuccess) return null;
            dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
            LoadResult ret = new LoadResult()
            {
                totalCount = result.totalCount,
                summary = result.summary,
                groupCount = result.groupCount
            };

            ret.data = result.data.ToObject<List<RegistroCAVDTO>>();
            return ret;

        }

        /// <summary>
        /// Retorna la información de un Tipo de Adquisición de Fauna
        /// </summary>
        /// <param name="Id">Identifica el Tipo de Adquisición de Fauna</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetTipoAdqusicionsync")]
        public async Task<JObject> GetTipoAdqusicionsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                TipoAdquisicionDTO tipoAdquisicion = new TipoAdquisicionDTO();
                response = await apiService.GetMicroServicioAsync<TipoAdquisicionDTO>(this.urlApiMicoservicio, "api/TipoAdquisicionEndPoint/", $"GetTipoAdquisicionByIdAsync?id={Id}", token);
                if (!response.IsSuccess) return JObject.FromObject(tipoAdquisicion, Js);
                tipoAdquisicion = (TipoAdquisicionDTO)response.Result;
                return JObject.FromObject(tipoAdquisicion, Js);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Almacena la Información de un Tipo de Adquisición de Fauna
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarTipoAdquisicionAsync")]
        public async Task<Response> GuardarTipoAdquisicionAsync(TipoAdquisicionDTO objData)
        {

            Response response = new Response();
            try
            {

                decimal Id = 0;
                if (objData.TipoAdquisicionId == -1) objData.TipoAdquisicionId = 0;
                Id = objData.TipoAdquisicionId;

                if (Id > 0)
                {
                    response = await apiService.PutMicroServicioAsync<TipoAdquisicionDTO>(this.urlApiMicoservicio, "api", "/TipoAdquisicionEndPoint/PutTipoAdquisicionAsync", objData, token);
                    if (!response.IsSuccess) return response;
                }
                else if (Id <= 0)
                {
                    objData.TipoAdquisicionId = 0;
                    response = await apiService.PostMicroServicioAsync<TipoAdquisicionDTO>(this.urlApiGateWay, "api", "/TipoAdquisicionEndPoint/PostTipoAdquisicionAsync", objData, token);
                    if (!response.IsSuccess) return response;
                }
            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return response;
        }


        /// <summary>
        /// Elimina un Tipo de Adquisición
        /// </summary>
        /// <param name="Id">Identifica el Tipo de Adquisición</param>
        /// <returns></returns>
        [HttpDelete, ActionName("EliminarTipoAdquisicionAsync")]
        public async Task<object> EliminarTipoAdquisicionAsync(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id)) return new { resp = "Error", mensaje = "No se identifica el Punto de Control!" };
                int _id = 0;
                int.TryParse(Id, out _id);

                TipoAdquisicionDTO registro = new TipoAdquisicionDTO
                {
                    TipoAdquisicionId = _id,
                    Activo = true,
                    Descripcion = "",
                    Eliminado = false,
                    Nombre = ".",
                };

                var resp = await apiService.DeleteMicroServicioAsync<TipoAdquisicionDTO>(urlApiMicoservicio, "api/TipoAdquisicionEndPoint/", "DeleteTipoAdquisicionAsync", registro, token);
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

        #endregion


        #region Registro CAV

        /// <summary>
        /// Retorna el Listado de los  Registros de los Ingresos al CAV
        /// </summary>
        /// <param name="opciones"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetIngresosCAVAsync")]
        public async Task<LoadResult> GetIngresosCAVAsync(DataSourceLoadOptions opciones)
        {
            var opcionesSerialize = JsonConvert.SerializeObject(opciones);

            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };


            response = await apiService.GetFilteredDataAsync(this.urlApiMicoservicio, "api/RegistroCAVEndPoint/", $"GetListRegistrosCAVAsync?opciones={opcionesSerialize}", token);
            if (!response.IsSuccess) return null;
            dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
            LoadResult ret = new LoadResult()
            {
                totalCount = result.totalCount,
                summary = result.summary,
                groupCount = result.groupCount
            };

            ret.data = result.data.ToObject<List<RegistroCAVDTO>>();

            return ret;

        }


        /// <summary>
        /// Retorna la información de un ingreso de un individuo al CAV
        /// </summary>
        /// <param name="Id">Identifica el ingreso del individuo al CAV</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetIngresoCAVAsync")]
        public async Task<JObject> GetIngresoCAVAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                RegistroCAVDTO ingresoCAV = new RegistroCAVDTO();

                response = await apiService.GetMicroServicioAsync<RegistroCAVDTO>(this.urlApiMicoservicio, "api/RegistroCAVEndPoint/", $"GetRegistroCAVByIdAsync?id={Id}", token);
                if (!response.IsSuccess) return JObject.FromObject(ingresoCAV, Js);
                ingresoCAV = (RegistroCAVDTO)response.Result;
                return JObject.FromObject(ingresoCAV, Js);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetHistoriaClinicaIndividuoAsync")]
        public async Task<JObject> GetHistoriaClinicaIndividuoAsync(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            HistoriaDTO historia = new HistoriaDTO();

            response = await apiService.GetMicroServicioAsync<HistoriaDTO>(this.urlApiMicoservicio, "api/HistoriaClinicaEndPoint/", "GetHistoriaClinicaByIdAsync?id=" + Id, token);
            if (!response.IsSuccess) return JObject.FromObject(historia, Js);
            historia = (HistoriaDTO)response.Result;

            return JObject.FromObject(historia, Js);
        }

        /// <summary>
        /// Retorna el listado de las Familias de Fauna
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerFamiliasAsync")]
        public async Task<JArray> ObtenerFamiliasAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var responseF = await apiService.GetListAsync<FamiliaFaunaDTO>(this.urlApiMicoservicio, "api/FamiliaFaunaEndPoint/", $"GetFamiliasFaunaAsync", token);
                if (!responseF.IsSuccess) return null;
                var list = (List<FamiliaFaunaDTO>)responseF.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las Especies que pertenecen a la familia seleccionada
        /// </summary>
        /// <param name="idFamilia">Identifica la Familia</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerEspeciesAsync")]
        public async Task<JArray> ObtenerEspeciesAsync(int idFamilia)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<EspecieFaunaDTO>(this.urlApiMicoservicio, "api/EspecieFaunaEndPoint/", $"GetEspeciesFaunaByFamilyIdAsync?familiaId=" + idFamilia, token);
                if (!response.IsSuccess) return null;
                var list = (List<EspecieFaunaDTO>)response.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        /// <summary>
        /// Almacena la Información de la Historia clínica de un Individuo
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarHistoriaAsync")]
        public async Task<Response> GuardarHistoriaAsync(HistoriaDTO objData)
        {

            Response response = new Response();
            try
            {

                decimal Id = 0;
                if (objData.HistoriaId == -1) objData.HistoriaId = 0;
                Id = objData.HistoriaId;

                if (Id > 0)
                {
                    response = await apiService.PutMicroServicioAsync<HistoriaDTO>(this.urlApiMicoservicio, "api", "/HistoriaClinicaEndPoint/PutHistoriaClinicaAsync", objData, token);
                    if (!response.IsSuccess) return response;
                }
                else if (Id <= 0)
                {
                    objData.HistoriaId = 0;
                    response = await apiService.PostMicroServicioAsync<HistoriaDTO>(this.urlApiMicoservicio, "api", "/HistoriaClinicaEndPoint/PostHistoriaClinicaAsync", objData, token);
                    if (!response.IsSuccess) return response;
                }
            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return response;
        }

        /// <summary>
        /// Retorna el listado de los Departamentos
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerDepartamentosAsync")]
        public async Task<JArray> ObtenerDepartamentosAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<DepartamentoDTO>(this.urlApiMicoservicio, "api/RegistroCAVEndPoint/", $"GetDepartamentosAsync", token);
                if (!response.IsSuccess) return null;
                var list = (List<DepartamentoDTO>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los Municipios qu pertenecen a un Departamento dado
        /// </summary>
        /// <param name="idDepartamento">Identifica la Familia</param>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerMunicipiosAsync")]
        public async Task<JArray> ObtenerMunicipiosAsync(int idDepartamento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<MunicipioDTO>(this.urlApiMicoservicio, "api/RegistroCAVEndPoint/", $"GetMunicipiosByDepartamentoIdAsync?departamentoId=" + idDepartamento, token);
                if (!response.IsSuccess) return null;
                var list = (List<MunicipioDTO>)response.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los Tipos de Destino (Disposiciones del los Individuos
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerDisposicionesIndividuosAsync")]
        public async Task<JArray> ObtenerDisposicionesIndividuosAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<TipoDestinoDTO>(this.urlApiMicoservicio, "api/TipoDestinoEndPoint/", $"GetTiposDestinoAsync", token);
                if (!response.IsSuccess) return null;
                var list = (List<TipoDestinoDTO>)response.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las Causas de Ingreso de los Individuos al CAV
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerCausasIngresoAsync")]
        public async Task<JArray> ObtenerCausasIngresoAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<CausaIngresoCAVDTO>(this.urlApiMicoservicio, "api/CausaIngresoCAVEndPoint/", $"GetCausasIngresoCAVAsync", token);
                if (!response.IsSuccess) return null;
                var list = (List<CausaIngresoCAVDTO>)response.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las Autoridades o entidades que realizan el procedimiento
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerAutoridadesAsync")]
        public async Task<JArray> ObtenerAutoridadesAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var responseF = await apiService.GetMicroServicioListAsync<AutoridadDTO>(this.urlApiMicoservicio, "api/AutoridadEndPoint/", $"GetAutoridadesAsync", token);
                if (!responseF.IsSuccess) return null;
                var list = (List<AutoridadDTO>)responseF.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los Tiempos de Cautiverio de los Individuos
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTiemposCautiverioAsync")]
        public async Task<JArray> ObtenerTiemposCautiverioAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                response = await apiService.GetMicroServicioListAsync<TiempoDTO>(this.urlApiMicoservicio, "api/TiempoEndPoint/", $"GetTiemposAsync", token);
                if (!response.IsSuccess) return null;
                var list = (List<TiempoDTO>)response.Result;
                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Registra un Lote de ingresos de individuos al CAV
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarLoteRegistrosIngresoCAVAsync")]
        public async Task<object> GuardarLoteRegistrosIngresoCAVAsync(ListRegistroCAVDTO objData)
        {

            if (!ModelState.IsValid) return response;

            try
            {
                JsonSerializer Js = new JsonSerializer();
                Js = JsonSerializer.CreateDefault();
                response = await apiService.PostMicroServicioAsync<ListRegistroCAVDTO>(this.urlApiMicoservicio, "api/", "RegistroCAVEndPoint/PostRegistroLoteCAVAsync", objData, token);
                if (!response.IsSuccess) return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error Almacenando el registro : " + e.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Retorna el Listado de los  Registros de los Ingresos al CAV
        /// </summary>
        /// <param name="opciones"></param>
        /// <param name="historiaId">Identifica la Historia Clínica</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetExamenesHistoriaAsync")]
        public async Task<LoadResult> GetExamenesHistoriaAsync(DataSourceLoadOptions opciones, int historiaId)
        {
            var opcionesSerialize = JsonConvert.SerializeObject(opciones);

            if (opciones == null) return null;
            var _idHistoria = opciones;

            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };
            response = await apiService.GetFilteredDataAsync(this.urlApiMicoservicio, "api/ExamenLaboratorioEndPoint/", $"GetListExameneslaboratorioAsync?Opciones={opcionesSerialize}&HistoriaId={historiaId}", token);
            if (!response.IsSuccess) return null;
            dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
            LoadResult ret = new LoadResult()
            {
                totalCount = result.totalCount,
                summary = result.summary,
                groupCount = result.groupCount
            };
            var t = result.data.ToObject<List<RegistroCAVDTO>>();
            if (t.Count > 0)
            {
                ret.data = t;
            }
            else
            {
                List<RegistroCAVDTO> list = new List<RegistroCAVDTO>();
                ret.totalCount = 0;
                ret.data = list;
            }
            return ret;

        }


        #endregion
    }

}

/// <summary>
/// 
/// </summary>
public class DataSourceLoadOptionsExt : DataSourceLoadOptions
{
    /// <summary>
    /// 
    /// </summary>
    public int IdPadre { get; set; }
}
