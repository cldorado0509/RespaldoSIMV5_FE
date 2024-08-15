using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Contractual.DTO;
using SIM.Models;
using SIM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SIM.Areas.Contractual.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]", Name = "CertificadosApi")]
    public class CertificadosApiController : ApiController
    {
        string urlApiContractual = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioContractual").ToString();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListaContratos")]
        public async Task<JArray> GetContratosTercero(int IdTercero)
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            if (IdTercero <= 0) return null;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Certificados/ContratosTercero?IdTerc=" + IdTercero;

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ContratosTerceroDTO>(urlApiContractual, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ContratosTerceroDTO>)response.Result;
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
        /// <param name="doc"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ValidaTercero")]
        public async Task<JObject> GetValidaTercero(string doc)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;

                string _controller = $"Certificados/ValidaTercero?Documento={doc}";
                SIM.Models.Response response = await apiService.GetAsync<Response>(urlApiContractual, "api/", _controller, token);
                if (!response.IsSuccess) return null;
                Response dynamicResponse = (Response)response.Result;
                return JObject.FromObject(dynamicResponse.Result, Js);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("PrevisualizaCetificado")]
        public async Task<CertificadoDTO> PostPrevisualizaCetificado(RequestCertificadoDTO datos)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (ModelState.IsValid)
            {
                try
                {
                    var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                    string token = _claim.Value;
                    _claim = (User.Identity as ClaimsIdentity).Claims.Where(w => w.Type.EndsWith("nameidentifier")).FirstOrDefault();
                    var usuario = _claim.Value;
                    datos.UsuarioGenera = int.Parse(usuario);
                    string _controller = $"Certificados/PrevisualizaCertificado";
                    SIM.Models.Response response = await apiService.PostAsync<RequestCertificadoDTO>(urlApiContractual, "api/", _controller, datos, token);
                    if (!response.IsSuccess) return null;
                    var OpResp = (OperationResponse)response.Result;
                    var certificado = JsonConvert.DeserializeObject<CertificadoDTO>(OpResp.Result.ToString());
                    return certificado;

                }
                catch (Exception ex)
                {
                    return new CertificadoDTO()
                    {
                        isSucceded = false,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                return new CertificadoDTO()
                {
                    isSucceded = false,
                    Message = "No se enviaron todos los datos para la generacion del certificado!!"
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost, ActionName("CetificadoWord")]
        public async Task<CertificadoDTO> PostCetificadoWord(RequestCertificadoDTO datos)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (ModelState.IsValid)
            {
                try
                {
                    var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                    string token = _claim.Value;
                    _claim = (User.Identity as ClaimsIdentity).Claims.Where(w => w.Type.EndsWith("nameidentifier")).FirstOrDefault();
                    var usuario = _claim.Value;
                    datos.UsuarioGenera = int.Parse(usuario);
                    string _controller = $"Certificados/CertificadoWord";
                    SIM.Models.Response response = await apiService.PostAsync<RequestCertificadoDTO>(urlApiContractual, "api/", _controller, datos, token);
                    if (!response.IsSuccess) return null;
                    var OpResp = (OperationResponse)response.Result;
                    var certificado = JsonConvert.DeserializeObject<CertificadoDTO>(OpResp.Result.ToString());
                    return certificado;

                }
                catch (Exception ex)
                {
                    return new CertificadoDTO()
                    {
                        isSucceded = false,
                        Message = ex.Message
                    };
                }

            }
            else
            {
                return new CertificadoDTO()
                {
                    isSucceded = false,
                    Message = "No se enviaron todos los datos para la generacion del certificado!!"
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GeneraCetificado")]
        public async Task<CertificadoDTO> PostGeneraCetificado(RequestCertificadoDTO datos)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (ModelState.IsValid)
            {
                try
                {
                    var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                    string token = _claim.Value;
                    _claim = (User.Identity as ClaimsIdentity).Claims.Where(w => w.Type.EndsWith("nameidentifier")).FirstOrDefault();
                    var usuario = _claim.Value;
                    datos.UsuarioGenera = int.Parse(usuario);
                    string _controller = $"Certificados/GeneraCertificado";
                    SIM.Models.Response response = await apiService.PostAsync<RequestCertificadoDTO>(urlApiContractual, "api/", _controller, datos, token);
                    if (!response.IsSuccess) return null;
                    var OpResp = (OperationResponse)response.Result;
                    var certificado = JsonConvert.DeserializeObject<CertificadoDTO>(OpResp.Result.ToString());
                    //Response dynamicResponse = (CertificadoDTO)response.Result;
                    return certificado;  //JObject.FromObject(certificado, Js);

                }
                catch (Exception ex)
                {
                    return new CertificadoDTO()
                    {
                        isSucceded = false,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                return new CertificadoDTO()
                {
                    isSucceded = false,
                    Message = "No se enviaron todos los datos para la generacion del certificado!!"
                };
            }
        }
    }
}
