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
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Route("api/[controller]", Name = "AtencionesApi")]
    [Authorize]
    public class AtencionesApiController : ApiController
    {

        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        private string urlApiSecurity = "https://sim.metropol.gov.co/seguridad/";
        private string urlApiATU = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioATU").ToString();
        private string urlApiTerceros= SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();
        //private string urlApiATU = "https://localhost:7188/";

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

            response = await apiService.GetAsync<TerceroNaturalDTO>(urlApiTerceros, "api/", "Terceros/ObtenerTerceroNatural?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var list = (TerceroNaturalDTO)response.Result;
            if (list == null) return JObject.FromObject("", Js);
            return JObject.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("PersonaNatural")]
        public async Task<JObject> GetPersonaNaturalAsync(decimal id)
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

            response = await apiService.GetAsync<TerceroNaturalDTO>(urlApiTerceros, "api/", "Terceros/ObtenerTerceroNatural?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var list = (TerceroNaturalDTO)response.Result;
            if (list == null) return JObject.FromObject("", Js);
            return JObject.FromObject(list, Js);
        }

        [HttpGet, ActionName("Atenciones")]
        public async Task<datosConsulta> GetAtencionesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoAtencionesDTO>(urlApiATU, "api/", "Atencion/ListadoAtenciones?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoAtencionesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet]
        [ActionName("GetTipoAtencionAsync")]
        public async Task<JArray> GetTipoAtencionAsync()
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

            response = await apiService.GetListAsync<TipoAtencionDTO>(urlApiATU, "api/", "Atencion/ObtenerTiposAtencion", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TipoAtencionDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpGet]
        [ActionName("GetClasesAtencionAsync")]
        public async Task<JArray> GetClasesAtencionAsync(decimal id)
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

            response = await apiService.GetListAsync<ClaseAtencionDTO>(urlApiATU, "api/", "Atencion/ObtenerClasesAtencion?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<ClaseAtencionDTO>)response.Result;
            if (list == null) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpGet]
        [ActionName("GetFormaAtencionAsync")]
        public async Task<JArray> GetFormaAtencionAsync()
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

            response = await apiService.GetListAsync<FormaAtencionDTO>(urlApiATU, "api/", "Atencion/ObtenerFormaAtencion", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<FormaAtencionDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpGet]
        [ActionName("GetListaChequeoAsync")]
        public async Task<JArray> GetListaChequeoAsync(decimal id)
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

            response = await apiService.GetListAsync<ChequeoDTO>(urlApiATU, "api/", "Atencion/ObtenerChequeos?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<ChequeoDTO>)response.Result;
            if (list == null) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpGet]
        [ActionName("GetCompromisosAsync")]
        public async Task<JArray> GetCompromisosAsync()
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

            response = await apiService.GetListAsync<CompromisoDTO>(urlApiATU, "api/", "Atencion/ObtenerCompromisos", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<CompromisoDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);

        }

        [HttpPost, ActionName("GuardarAtencionTAsync")]
        public async Task<object> GuardarAtencionTAsync(AtencionDTO atencion)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            long idUsuario = -1;
            long codFuncionario = -1;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            else codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                decimal idA = -1;
                idA= atencion.IdAtencion;

                if (idA <= 0) {

                    atencion.IdUsuarioFuncionario = idUsuario;
                    atencion.IdAtencion = 0;
                    var resp = await apiService.PostAsync<AtencionDTO>(urlApiATU, "api/", "Atencion/GuardarAtencion", atencion, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                if(idA > 0) {
                    var resp = await apiService.PostAsync<AtencionDTO>(urlApiATU, "api/", "Atencion/ActualizarAtencion", atencion, tokenG.Token);
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
        [ActionName("ObtenerAtencion")]
        public async Task<JObject> ObtenerAtencionAsync(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                AtencionTipoDTO evento = new AtencionTipoDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<AtencionTipoDTO>(urlApiATU, "api/", "Atencion/ObtenerAtencion?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (AtencionTipoDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpGet, ActionName("ListadoSolicitudes")]
        public async Task<datosConsulta> GetListadoSolicitudesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoSolicitudesDTO>(urlApiATU, "api/", "Atencion/ListadoSolicitudes?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoSolicitudesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("BuscarSolicitud")]
        public async Task<datosConsulta> GetSolicitudTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                datosConsulta.numRegistros = 0;
                datosConsulta.datos = null;

                return datosConsulta;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<ListadoSolicitudesDTO>(urlApiATU, "api/", "Atencion/BuscarSolicitudes", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoSolicitudesDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta.numRegistros = modelFiltered.Count();
                if (take == 0) datosConsulta.datos = modelFiltered.ToList();
                else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();
            }
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

        [HttpPost, ActionName("VincularSolicitudTAsync")]
        public async Task<object> VincularSolicitudTAsync(AtencionProyectoDTO solicitud)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            

            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

               
                    var resp = await apiService.PostAsync<AtencionProyectoDTO>(urlApiATU, "api/", "Atencion/VincularSolicitud", solicitud, tokenG.Token);
                    if (!resp.IsSuccess) return null;
               
               
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularSolicitudTAsync")]
        public async Task<object> DesvincularSolicitudTAsync(AtencionProyectoDTO solicitud)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

          

                var resp = await apiService.DeleteAsync<AtencionProyectoDTO>(urlApiATU, "api/", "Atencion/DesvincularSolicitud", solicitud, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpPost]
        [ActionName("ActualizarTerceroNatural")]
        public async Task<object> PostTerceroNaturalAsync(TerceroNaturalDTO natural)
        {
            decimal Id = -1;
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                    var resp = await apiService.PostAsync<TerceroNaturalDTO>(urlApiTerceros, "api/", "Terceros/ActualizarTerceroNatural", natural, tokenG.Token);
                    if (!resp.IsSuccess) return null;
               
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!", id = Id };

        }

        [HttpGet, ActionName("ListadoQuejas")]
        public async Task<datosConsulta> GetListadoQuejasTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoQuejasDTO>(urlApiATU, "api/", "Atencion/ListadoQuejas?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoQuejasDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("BuscarQuejas")]
        public async Task<datosConsulta> GetQuejasTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) 
            {
                datosConsulta.numRegistros = 0;
                datosConsulta.datos = null;

                return datosConsulta;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<ListadoQuejasDTO>(urlApiATU, "api/", "Atencion/BuscarQuejas", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoQuejasDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta.numRegistros = modelFiltered.Count();
                if (take == 0) datosConsulta.datos = modelFiltered.ToList();
                else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();
            }
            return datosConsulta;
        }

        [HttpPost, ActionName("VincularQueja")]
        public async Task<object> VincularQuejaTAsync(QuejaAtencionDTO queja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PostAsync<QuejaAtencionDTO>(urlApiATU, "api/", "Atencion/VincularQueja", queja, tokenG.Token);
                if (!resp.IsSuccess) return null;


            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularQueja")]
        public async Task<object> DesvincularQuejaTAsync(QuejaAtencionDTO queja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<QuejaAtencionDTO>(urlApiATU, "api/", "Atencion/DesvincularQueja", queja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet, ActionName("ListadoDocumentos")]
        public async Task<datosConsulta> GetListadoDocumentosTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoDocumentosDTO>(urlApiATU, "api/", "Atencion/ListadoDocumentos?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoDocumentosDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpPost, ActionName("VincularDocumento")]
        public async Task<object> VincularDocumento(AtencionDocumentoDTO documento)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PostAsync<AtencionDocumentoDTO>(urlApiATU, "api/", "Atencion/VincularDocumento", documento, tokenG.Token);
                if (!resp.IsSuccess) return null;


            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularDocumento")]
        public async Task<object> DesvincularDocumentoTAsync(AtencionDocumentoDTO documento)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<AtencionDocumentoDTO>(urlApiATU, "api/", "Atencion/DesvincularDocumento", documento, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet, ActionName("ListadoTramites")]
        public async Task<datosConsulta> GetListadoTramitesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoTramitesDTO>(urlApiATU, "api/", "Atencion/ListadoTramites?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoTramitesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("BuscarTramites")]
        public async Task<datosConsulta> GetTramitesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic model = null;
            dynamic modelData;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                datosConsulta.numRegistros = 0;
                datosConsulta.datos = null;

                return datosConsulta;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return datosConsulta;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return datosConsulta;

                response = await apiService.GetListAsync<ListadoTramitesDTO>(urlApiATU, "api/", "Atencion/BuscarTramites", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoTramitesDTO>)response.Result;
                if (list == null || list.Count == 0) return datosConsulta;

                model = list.AsQueryable();
                modelData = model;

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta.numRegistros = modelFiltered.Count();
                if (take == 0) datosConsulta.datos = modelFiltered.ToList();
                else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();
            }
            return datosConsulta;
        }

        [HttpPost, ActionName("VincularTramite")]
        public async Task<object> VincularTramiteTAsync(AtencionTramiteDTO tramite)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PostAsync<AtencionTramiteDTO>(urlApiATU, "api/", "Atencion/VincularTramite", tramite, tokenG.Token);
                if (!resp.IsSuccess) return null;


            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularTramite")]
        public async Task<object> DesvincularTramiteTAsync(AtencionTramiteDTO tramite)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<AtencionTramiteDTO>(urlApiATU, "api/", "Atencion/DesvincularTramite", tramite, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }



        [HttpGet, ActionName("ReporteAtenciones")]
        public async Task<datosConsulta> GetReporteAtencionesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, string fechaI, string fechaF)
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

            response = await apiService.GetListAsync<ListadoAtencionesDTO>(urlApiATU, "api/", "Atencion/ListadoReporteAtenciones?fechaI=" + fechaI + "&fechaF=" + fechaF, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoAtencionesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

    }


}



// esto es para sacar el funcionario que esta logueado

//if (((System.Security.Claims.ClaimsPrincipal) context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
//   {
//       idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal) context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
//       codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);
//   }else codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);


//Para hacer que el grid llegue vacio

//if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
//{
//    datosConsulta.numRegistros = 0;
//    datosConsulta.datos = null;

//    return datosConsulta;
//}
//else
//{

