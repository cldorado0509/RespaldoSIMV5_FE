using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
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
    public class FirmarCertificadoApiController : ApiController
    {
        string urlApiContractual = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioContractual").ToString();

        /// <summary>
        /// Obtiene los certificados disponivbles para la firma
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCertificados")]
        public async Task<LoadResult> GetCertificados(DataSourceLoadOptions loadOptions)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Certificados/ObtenerCertificados?opciones={serloadOptions}";
                SIM.Models.Response response = await apiService.GetFilteredDataAsync(urlApiContractual, "api/", _controller, token);
                if (!response.IsSuccess) return null;
                dynamic dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                if (dynamicResponse == null) return null;
                LoadResult loadResult = new LoadResult()
                {
                    totalCount = dynamicResponse.totalCount,
                    groupCount = dynamicResponse.groupCount,
                    summary = dynamicResponse.summary
                };

                loadResult.data = dynamicResponse.data.ToObject<List<CertificadosDTO>>();
                return loadResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCert"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("VerCertificado")]
        public async Task<Response> GetVerCertificado(int IdCert)
        {
            Response resp = new Response();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                if (IdCert > 0)
                {
                    string _controller = $"Certificados/VerCertificado?IdCert={IdCert}";
                    SIM.Models.Response response = await apiService.GetAsync<Response>(urlApiContractual, "api/", _controller, token);
                    if (!response.IsSuccess) return null;
                    return (Response)response.Result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCert"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("FirmarCertificado")]
        public async Task<RespuestaFirmaDTO> FirmarCertificado(int IdCert, int IdUsuario)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            if (IdCert > 0 && IdUsuario > 0)
            {
                try
                {
                    var _claim = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                    string token = _claim.Value;
                    _claim = (User.Identity as ClaimsIdentity).Claims.Where(w => w.Type.EndsWith("nameidentifier")).FirstOrDefault();
                    var usuario = _claim.Value;
                    if (int.Parse(usuario) == IdUsuario)
                    {
                        string _controller = $"Certificados/FirmarCertificado?IdCert=" + IdCert + "&IdUsuario=" + IdUsuario;
                        SIM.Models.Response response = await apiService.GetAsync<Response>(urlApiContractual, "api/", _controller, token);
                        if (!response.IsSuccess) return new RespuestaFirmaDTO()
                        {
                            IsSuccess = false,
                            Mensaje = response.Message
                        };
                        var OpResp = (Response)response.Result;
                        var certificado = JsonConvert.DeserializeObject<RespuestaFirmaDTO>(OpResp.Result.ToString());
                        return certificado;
                    }
                    else
                    {
                        return new RespuestaFirmaDTO()
                        {
                            IsSuccess = false,
                            Mensaje = "El usuario enviado para firmar no es el mismo que se autenticó"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new RespuestaFirmaDTO()
                    {
                        IsSuccess = false,
                        Mensaje = ex.Message
                    };
                }
            }
            else
            {
                return new RespuestaFirmaDTO()
                {
                    IsSuccess = false,
                    Mensaje = "No se enviaron todos los datos para la firma del certificado!!"
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCert"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("EliminarCertificado")]
        public async Task<bool> EliminarCertificado(int IdCert)
        {
            Response resp = new Response();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                if (IdCert > 0)
                {
                    string _controller = $"Certificados/EliminarCertificado?IdCert={IdCert}";
                    SIM.Models.Response response = await apiService.GetAsync<Response>(urlApiContractual, "api/", _controller, token);
                    if (!response.IsSuccess) return false;
                    resp = (Response)response.Result;
                    return resp.IsSuccess;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
