using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Seguridad.Models;
using SIM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SIM.Areas.Seguridad.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FuncionarioApiController : ApiController
    {
        private string urlApiSeguridad = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioSeguridadLocal").ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetFuncionarios")]
        public async Task<LoadResult> GetFuncionarios(DataSourceLoadOptions loadOptions)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Funcionarios/GetFuncionarios?opciones={serloadOptions}";
                SIM.Models.Response response = await apiService.GetFilteredDataAsync(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return null;
                if (response.IsSuccess)
                {
                    dynamic dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                    LoadResult loadResult = new LoadResult()
                    {
                        totalCount = dynamicResponse.totalCount,
                        groupCount = dynamicResponse.groupCount,
                        summary = dynamicResponse.summary
                    };

                    loadResult.data = dynamicResponse.data.ToObject<List<FuncionariosDTO>>();
                    return loadResult;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DetalleFuncionario")]
        public async Task<JObject> GetUsuario(int CodFuncionario)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                DatosFuncionarioDTO datosFuncionario = new DatosFuncionarioDTO();
                string _controller = $"Funcionarios/ObtenerDatosFuncionario?CodFunc={CodFuncionario}";
                SIM.Models.Response response = await apiService.GetAsync<DatosFuncionarioDTO>(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return JObject.FromObject(response, Js);
                datosFuncionario = (DatosFuncionarioDTO)response.Result;
                return JObject.FromObject(datosFuncionario, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
