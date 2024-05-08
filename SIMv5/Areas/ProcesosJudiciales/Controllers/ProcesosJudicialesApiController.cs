using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ProcesosJudiciales.DTOs;
using SIM.Data;
using SIM.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SIM.Areas.ProcesosJudiciales.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcesosJudicialesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        string urlApiJudicial = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioJudicial").ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool AcceptAllCertifications(
            object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaProcesosJudiciales")]
        public async Task<LoadResult> GetConsultaProcesosJudiciales(DataSourceLoadOptions loadOptions)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"ProcesosJudiciales/GetConsultaProcesosJudiciales?Opciones={serloadOptions}";
                SIM.Models.Response response = await apiService.GetFilteredDataAsync(urlApiJudicial, "api/", _controller, token);
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

                    loadResult.data = dynamicResponse.data.ToObject<List<ProcesosJudicialesDTO>>();
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJurisdicciones")]
        public async Task<JArray> GetJurisdicciones()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Jurisdicciones";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list, Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMediosControl")]
        public async Task<JArray> GetMediosControl()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/MediosControl";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list, Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJuzgados")]
        public async Task<JArray> GetJuzgados()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Juzgados";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list, Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetEstadoProcesos")]
        public async Task<JArray> GetEstadoProcesos()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/EstadosProceso";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTiposCuantia")]
        public async Task<JArray> GetTiposCuantia()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/TiposCuantia";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCalidadEntidad")]
        public JArray GetCalidadEntidad()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "8120", Valor = "Entidad demandada" });
                list.Add(new ListadoDTO { Id = "9120", Valor = "Demanda la entidad" });
                list.Add(new ListadoDTO { Id = "C", Valor = "Entidad citada" });
                list.Add(new ListadoDTO { Id = "G", Valor = "Llamada en garantías" });
                list.Add(new ListadoDTO { Id = "TV", Valor = "Tercero vinculado" });
                list.Add(new ListadoDTO { Id = "O", Valor = "Otros" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetErogacion")]
        public JArray GetErogacion()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Si" });
                list.Add(new ListadoDTO { Id = "0", Valor = "No" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRiesgosProcesales")]
        public JArray GetRiesgosProcesales()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Alto" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Medio alta" });
                list.Add(new ListadoDTO { Id = "3", Valor = "Medio bajo" });
                list.Add(new ListadoDTO { Id = "4", Valor = "Bajo" });
                var listDto = JArray.FromObject(list, Js);

                return listDto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTipoValorEconomico")]
        public JArray GetTipoValorEconomico()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Determinado" });
                list.Add(new ListadoDTO { Id = "0", Valor = "Indeterminado" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTiposPretencion")]
        public JArray GetTiposPretencion()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Lucro Cesanta " });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño emergente" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño material otros" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño moral" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño a la salud" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño inmaterail - Otros" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDepartamentos")]
        public async Task<JArray> GetDepartamentos()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Departamentos";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMunicipios")]
        public async Task<JArray> GetMunicipios(int departamentoId)
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/ObtenerCiudad?IdDepto=" + departamentoId;

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }
    }
}
