namespace SIM.Areas.GestionRiesgo.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.GestionRiesgo.Models;
    using SIM.Data;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Route("api/[controller]", Name = "TiposVisitaApi")]
    public class TiposVisitaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        private string urlApiGestionRiesgo = "https://amaplicacion02/gestionriesgo/";


        /// <summary>
        /// Retorna el Listado de los tipos de vista
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
        [HttpGet, ActionName("GetTiposVisitaAsync")]
        public async Task<datosConsulta> GetTiposVisitaAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

            response = await apiService.GetListAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/ObtenerTiposVisitas", tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<TipoVisitaDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("GetTiposVisitasAsync")]
        public async Task<JArray> GetTiposVisitasAsync()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/ObtenerTiposVisitas", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<TipoVisitaDTO>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpPost, ActionName("GuardarTipoVisitaAsync")]
        public async Task<object> GuardarTipoVisitaAsync(TipoVisitaDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = objData.IdTipoVisita;
                if (Id > 0)
                {
                    var resp = await apiService.PutAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/ActualizarTipoVisita", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    var resp = await apiService.PostAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/GuardarTipoVisita", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ObtenerTipoVisitaAsync")]
        public async Task<JObject> ObtenerTipoVisitaAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                TipoVisitaDTO evento = new TipoVisitaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);

                response = await apiService.GetAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/ObtenerTipoVisita?idtipoVisita=" + Id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (TipoVisitaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("EliminarTipoVisitaAsync")]
        public async Task<object> EliminarTipoVisitaAsync(TipoVisitaDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.DeleteAsync<TipoVisitaDTO>(urlApiGestionRiesgo, "api/", "TipoVisita/BorrarTipoVisita", objData, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
