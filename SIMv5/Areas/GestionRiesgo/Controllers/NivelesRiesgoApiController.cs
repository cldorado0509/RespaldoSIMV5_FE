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

    [Route("api/[controller]", Name = "EventosApi")]
    public class NivelesRiesgoApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        private string urlApiGestionRiesgo = "https://amaplicacion02/gestionriesgo/";


        /// <summary>
        /// Retorna el Listado de los Derechos de Petición Registrados
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usaurio</param>
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
        [HttpGet, ActionName("GetNivelesRiesgoAsync")]
        public async Task<datosConsulta> GetNivelesRiesgoAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest{ Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe= true});
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;
          
            response = await apiService.GetListAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/ObtenerNivelesRiesgo", tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<NivelRiesgoDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }


        [HttpGet, ActionName("GetListNivelesRiesgoAsync")]
        public async Task<JArray> GetListNivelesRiesgoAsync()
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

                response = await apiService.GetListAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/ObtenerNivelesRiesgo", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<NivelRiesgoDTO>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }




        [HttpPost, ActionName("GuardarNivelRiesgoAsync")]
        public async Task<object> GuardarNivelRiesgoAsync(NivelRiesgoDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = objData.IdNivelRiesgo;
                if (Id > 0)
                {
                    var resp = await apiService.PutAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/ActualizarNivelRiesgo", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    var resp = await apiService.PostAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/GuardarNivelRiesgo", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ObtenerNivelRiesgoAsync")]
        public async Task<JObject> ObtenerNivelRiesgoAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                NivelRiesgoDTO nivelRiesgo = new NivelRiesgoDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(nivelRiesgo, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(nivelRiesgo, Js);

                response = await apiService.GetAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/ObtenerNivelRiesgo?idNivelRiesgo=" + Id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(nivelRiesgo, Js);
                nivelRiesgo = (NivelRiesgoDTO)response.Result;
                return JObject.FromObject(nivelRiesgo, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("EliminarNivelRiesgoAsync")]
        public async Task<object> EliminarNivelRiesgoAsync(NivelRiesgoDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.DeleteAsync<NivelRiesgoDTO>(urlApiGestionRiesgo, "api/", "NivelRiesgo/BorrarNivelRiesgo", objData, tokenG.Token);
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
