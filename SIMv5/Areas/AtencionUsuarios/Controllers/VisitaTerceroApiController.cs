namespace SIM.Areas.AtencionUsuarios.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.AtencionUsuarios.Models;
    using SIM.Data;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Route("api/[controller]", Name = "VisitaTerceroApi")]
    [Authorize]
    public class VisitaTerceroApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiSecurity = "https://sim.metropol.gov.co/seguridad/";
        private string urlApiATU = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioATU").ToString();
        private string urlApiGeneral = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioGeneral").ToString();



        [HttpGet, ActionName("Visitas")]
        public async Task<datosConsulta> GetVisitasTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, DateTime fecha)
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

            response = await apiService.GetListAsync<ListadoVisitasDTO>(urlApiATU, "api/", "VisitaTercero/ListadoVisitasFecha?fecha=" + fecha.ToString("dd-MM-yyyy hh:mm:ss"), tokenG.Token);
            //response = await apiService.GetListAsync<ListadoVisitasDTO>(urlApiATU, "api/", "VisitaTercero/ListadoVisitas ", tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoVisitasDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("ReporteMes")]
        public async Task<datosConsulta> GetReporteMesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, int mes, int anno)
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

            response = await apiService.GetListAsync<ListadoVisitasDTO>(urlApiATU, "api/", "VisitaTercero/ListadoVisitasReporteMes?mes=" + mes + "&anno=" + anno, tokenG.Token);

            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoVisitasDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        [HttpGet]
        [ActionName("MotivoVisita1async")]
        public async Task<JArray> MotivoVisita1async()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la seguridad " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null)
            {
                return JArray.FromObject("", Js);
            }

            response = await apiService.GetListAsync<MotivoVisitaDTO>(urlApiATU, "api/", "MotivoVisita/ObtenerMotivosVisitas", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<MotivoVisitaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("GetDependenciaAsync")]
        public async Task<JArray> GetDependenciaAsync()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la seguridad " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null)
            {
                return JArray.FromObject("", Js);
            }

            response = await apiService.GetListAsync<DependenciaDTO>(urlApiGeneral, "api/", "Dependencia/ObtenerDependencias", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<DependenciaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpGet]
        [ActionName("GetFuncionarioAsync")]
        public async Task<JArray> GetFuncionarioAsync(decimal dep)
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la seguridad " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null)
            {
                return JArray.FromObject("", Js);
            }



            if (dep > 0)
            {
                response = await apiService.GetListAsync<FuncionarioDTO>(urlApiGeneral, "api/", "Funcionario/ObtenerFuncionarioDependencia?dep=" + dep, tokenG.Token);
                if (!response.IsSuccess)
                {
                    var Msg = "Ocurrio un error en la consulta " + response.Message;
                    return JArray.FromObject(Msg, Js);
                }
                var list = (List<FuncionarioDTO>)response.Result;
                if (list == null /*|| list.Count == 0*/) return JArray.FromObject("", Js); ;
                return JArray.FromObject(list, Js);
            }
            else
            {
                response = await apiService.GetListAsync<FuncionarioDTO>(urlApiGeneral, "api/", "Funcionario/ObtenerQryFuncionarios", tokenG.Token);
                if (!response.IsSuccess)
                {
                    var Msg = "Ocurrio un error en la consulta " + response.Message;
                    return JArray.FromObject(Msg, Js);
                }
                var list = (List<FuncionarioDTO>)response.Result;
                if (list == null /*|| list.Count == 0*/) return JArray.FromObject("", Js); ;
                return JArray.FromObject(list, Js);
            }

        }

        [HttpGet]
        [ActionName("Persona")]
        public async Task<JObject> GetPersonaAsync(decimal id)
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la seguridad " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null)
            {
                return JObject.FromObject("", Js);
            }

            response = await apiService.GetAsync<PersonaDTO>(urlApiATU, "api/", "Persona/ObtenerPersona?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var list = (PersonaDTO)response.Result;
            if (list == null) return JObject.FromObject("", Js);
            return JObject.FromObject(list, Js);
        }

        [HttpPost]
        [ActionName("GuardarPersona")]
        public async Task<object> PostPersonaAsync(PersonaDTO persona)
        {
            decimal Id = -1;
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;




                Id = persona.IdPersona;
                if (Id > 0)
                {
                    var resp = await apiService.PutAsync<PersonaDTO>(urlApiATU, "api/", "Persona/ActualizarPersona", persona, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (Id <= 0)
                {

                    var resp = await apiService.PostAsync<PersonaDTO>(urlApiATU, "api/", "Persona/GuardarPersona", persona, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                    else Id = decimal.Parse(resp.Message);
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!", id = Id };

        }

        [HttpPost, ActionName("GuardarVisita")]
        public async Task<object> GuardarVisitaTAsync(VisitaTerceroDTO visitaT)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal Id = -1;
                Id = visitaT.IdVisitaTercero;
                if (Id > 0)
                {
                    var resp = await apiService.PutAsync<VisitaTerceroDTO>(urlApiATU, "api/", "VisitaTercero/ActualizarVisitaTercero", visitaT, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (Id <= 0)
                {
                    var resp = await apiService.PostAsync<VisitaTerceroDTO>(urlApiATU, "api/", "VisitaTercero/GuardarVisitaTercero", visitaT, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet]
        [ActionName("ObtenerVisita")]
        public async Task<JObject> ObtenerVisitaAsync(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                VisitaTerceroDTO evento = new VisitaTerceroDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<VisitaTerceroDTO>(urlApiATU, "api/", "VisitaTercero/ObtenerVisita?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (VisitaTerceroDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPut]
        [ActionName("RegistrarSalida")]
        public async Task<object> PutSalidaVisistaAsync(RegistroSalidaDTO salida)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PutAsync<RegistroSalidaDTO>(urlApiATU, "api/", "VisitaTercero/ActualizarSalidaVisita", salida, tokenG.Token);
                if (!resp.IsSuccess) return null;

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };

        }

        [HttpPost]
        [ActionName("EliminarVisita")]
        public async Task<object> EliminarVisitasTAsync(VisitaTerceroDTO visita)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                //var T = new VisitaTerceroDTO();

                var resp = await apiService.DeleteAsync<VisitaTerceroDTO>(urlApiATU, "api/", "VisitaTercero/BorrarVisita", visita, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro eliminado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }


    }
}