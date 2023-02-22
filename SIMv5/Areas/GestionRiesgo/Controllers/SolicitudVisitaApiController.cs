namespace SIM.Areas.GestionRiesgo.Controllers
{
using DevExpress.Skins;
using DocumentFormat.OpenXml.EMMA;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.ControlVigilancia.Models;
    using SIM.Areas.GestionRiesgo.Models;
    using SIM.Data;
    using SIM.Data.Tramites;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using static SIM.Areas.AtencionUsuarios.Controllers.VisitaTerceroApiController;

    [Route("api/[controller]", Name = "SolicitudVisitaApi")]
    public class SolicitudVisitaApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        private string urlApiGestionRiesgo = "https://amaplicacion02/gestionriesgo/";

        [HttpGet, ActionName("GetMeses")]
        public JArray GetMeses()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                List<Mes> meses = new List<Mes>();
                meses.Add(new Mes { id = 1, name = "Enero" });
                meses.Add(new Mes { id = 2, name = "Febrero" });
                meses.Add(new Mes { id = 3, name = "Marzo" });
                meses.Add(new Mes { id = 4, name = "Abril" });
                meses.Add(new Mes { id = 5, name = "Mayo" });
                meses.Add(new Mes { id = 6, name = "Junio" });
                meses.Add(new Mes { id = 7, name = "Julio" });
                meses.Add(new Mes { id = 8, name = "Agosto" });
                meses.Add(new Mes { id = 9, name = "Septiembre" });
                meses.Add(new Mes { id = 10, name = "Octubre" });
                meses.Add(new Mes { id = 11, name = "Noviembre" });
                meses.Add(new Mes { id = 12, name = "Diciembre" });
                return JArray.FromObject(meses, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("GetTiposViasAsync")]
        public async Task<JArray> GetTiposViasAsync()
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

                response = await apiService.GetListAsync<System.Web.Mvc.SelectListItem>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ObtenerListadoVias", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<System.Web.Mvc.SelectListItem>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        [HttpGet, ActionName("GetLetrasViasAsync")]
        public async Task<JArray> GetLetrasViasAsync()
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

                response = await apiService.GetListAsync<System.Web.Mvc.SelectListItem>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ObtenerListadoLetrasVias", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<System.Web.Mvc.SelectListItem>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        
        [HttpGet, ActionName("GetSentidosViasAsync")]
        public async Task<JArray> GetSentidosViasAsync()
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

                response = await apiService.GetListAsync<System.Web.Mvc.SelectListItem>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ObtenerSentidosVias", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<System.Web.Mvc.SelectListItem>)response.Result;

                return JArray.FromObject(list, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


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
        [HttpGet, ActionName("GetSolicitudesAsync")]
        public async Task<datosConsulta> GetSolicitudesAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<SolicitudVisitaDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ObtenerSolicitudesVisitas", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<SolicitudVisitaDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.OrderByDescending(o => o.IdSolicitudVisita).AsQueryable();
                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) datosConsulta.datos = modelFiltered.ToList();
                else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();
          
                return datosConsulta;
            }
            catch (Exception ex)
            {
                return datosConsulta;
            }
        }

        /// <summary>
        /// Permite almacenar una solicitud de visita
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarSolicitudVisitaAsync")]
        public async Task<object> GuardarSolicitudVisitaAsync(SolicitudVisitaDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = objData.IdSolicitudVisita;
                if (Id > 0)
                {
                    var resp = await apiService.PutAsync<SolicitudVisitaDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ActualizarSolicitudVisita", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    var resp = await apiService.PostAsync<SolicitudVisitaDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/GuardarSolicitudVisita", objData, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ObtenerSolicitudVisitaAsync")]
        public async Task<JObject> ObtenerSolicitudVisitaAsync(string Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(Id)) _Id = int.Parse(Id);

                SolicitudVisitaDTO solicitudVisita = new SolicitudVisitaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(solicitudVisita, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(solicitudVisita, Js);

                response = await apiService.GetAsync<SolicitudVisitaDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ObtenerSolicitudVisita?id=" + Id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(solicitudVisita, Js);
                solicitudVisita = (SolicitudVisitaDTO)response.Result;
                return JObject.FromObject(solicitudVisita, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("EliminarSolicitudVisitaAsync")]
        public async Task<object> EliminarSolicitudVisitaAsync(SolicitudVisitaDTO objData)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                objData.BarrioVereda = ".";
                objData.FechaVisita = DateTime.Now;
                objData.SueloId = 1;
                objData.FechaRadicadoSolicitud = ".";
                objData.RadicadoSolicitud = ".";
                objData.TipoVisitaId = 1;
                objData.CalificacionRiesgo = 0;
                objData.CodigoTramite = 0;
                objData.Destinatarios = ",";
                objData.EventoId = 1;
                objData.FuncionarioId = 1;
                objData.Latitud = 0;
                objData.Longitud = 0;
                objData.Mes = 1;
                objData.MunicipioId = 1;
                objData.EsMonitoreo = false;
                objData.EventoId = 1;
                objData.NivelRiesgoId = 1;
                objData.NumeroContacto = "";
                objData.NumeroPersonasImpactadas = 0;
                objData.OrigenId = 1;
                objData.Quebradas = ".";
                objData.Direccion = ".";
                objData.NumeroContacto = "0";
                objData.RadicadoSalida = ",";
                objData.FechaRadicadoSalida = DateTime.Now.ToString();
                objData.Solicitante = ".";
          
                var resp = await apiService.DeleteAsync<SolicitudVisitaDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/BorrarSolicitudVisita", objData, tokenG.Token);
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

        [HttpGet, ActionName("ValidarTramiteAsync")]
        public async Task<JObject> ValidarTramiteAsync(string idTramite)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                int _Id = -1;
                if (!string.IsNullOrEmpty(idTramite)) _Id = int.Parse(idTramite);

                TramiteDTO tramite = new TramiteDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "jorge.estrada@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(tramite, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(tramite, Js);

                response = await apiService.GetAsync<TramiteDTO>(urlApiGestionRiesgo, "api/", "SolicitudVisita/ValidarTramite?idTramite=" + idTramite, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(tramite, Js);
                tramite = (TramiteDTO)response.Result;
                return JObject.FromObject(tramite, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


     
    }
}
