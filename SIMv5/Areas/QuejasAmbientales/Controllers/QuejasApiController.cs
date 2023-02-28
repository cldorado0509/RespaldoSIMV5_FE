namespace SIM.Areas.QuejasAmbientales.Controllers
{
    using DevExpress.XtraRichEdit;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.QuejasAmbientales.Models;
    using SIM.Data;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;

    [Route("api/[controller]", Name = "QuejasApi")]
    [Authorize]
    public class QuejasApiController : ApiController
    {
        EntitiesControlOracle dbControl = new EntitiesControlOracle();
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        private string urlApiSecurity = "https://sim.metropol.gov.co/seguridad/";

        //private string urlApiQuejasAmbientales = "https://localhost:7286/";
        //private string urlApiGeneral = "https://localhost:7012/";
        //private string urlApiTerceros = "https://localhost:7154/";
        //private string urlApiATU = "https://localhost:7188/";

       private string urlApiATU = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioATU").ToString();
       private string urlApiGeneral = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioGeneral").ToString();
       private string urlApiQuejasAmbientales = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioQuejasAmbientales").ToString();
       private string urlApiTerceros = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        [HttpGet, ActionName("ListadoQuejas")]
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

                response = await apiService.GetListAsync<ListadoQuejasDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoQuejas", tokenG.Token);
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

        [HttpGet]
        [ActionName("Recursosasync")]
        public async Task<JArray> Recursosasync()
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

            response = await apiService.GetListAsync<TbRecursoDTO>(urlApiQuejasAmbientales, "api/", "TbRecurso/ObtenerRecursos", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TbRecursoDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("Afectacionsasync")]
        public async Task<JArray> Afectacionsasync(decimal id)
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

            response = await apiService.GetListAsync<TbAfectacionDTO>(urlApiQuejasAmbientales, "api/", "TbAfectacion/ObtenerAfectaciones?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TbAfectacionDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("Municipiosasync")]
        public async Task<JArray> Municipiosasync()
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

            response = await apiService.GetListAsync<TbMunicipioDTO>(urlApiQuejasAmbientales, "api/", "TbMunicipio/ObtenerMunicipios", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TbMunicipioDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("Responsablesasync")]
        public async Task<JArray> Responsablesasync()
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

            var cod = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("CodFuncRespQueja").ToString());

            response = await apiService.GetListAsync<FuncionarioDTO>(urlApiGeneral, "api/", "Funcionario/ObtenerFuncionario?cod=" + cod, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<FuncionarioDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("FormasQuejassasync")]
        public async Task<JArray> FormasQuejassasync()
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

            response = await apiService.GetListAsync<TbFormaQuejaDTO>(urlApiQuejasAmbientales, "api/", "TbFormaQueja/ObtenerFormasQueja", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TbFormaQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet, ActionName("BuscarFuncionario")]
        public async Task<object> BuscarFuncionario()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            long idUsuario = -1;
            long funcionario = -1;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                               join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                               where uf.ID_USUARIO == idUsuario
                                               select f.CODFUNCIONARIO).FirstOrDefault());
            }

            var cod = funcionario;

            try
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

                response = await apiService.GetListAsync<ListaFuncionariosDTO>(urlApiGeneral, "api/", "Funcionario/ObtenerQryFuncionario?cod=" + cod, tokenG.Token);
                if (!response.IsSuccess)
                {
                    var Msg = "Ocurrio un error en la consula " + response.Message;
                    return JArray.FromObject(Msg, Js);
                }
                var list = (List<ListaFuncionariosDTO>)response.Result;
                if (list == null) return JArray.FromObject("", Js);
                return JArray.FromObject(list, Js);
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet]
        [ActionName("Consecutivo")]
        public async Task<int> Consecutivo()
        {

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la seguridad " + response.Message;
                return 0;
            }
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null)
            {
                return 0;
            }

            response = await apiService.GetAsync<int>(urlApiQuejasAmbientales, "api/", "Quejas/Consecutivo", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return 1;
            }
            var list = response.Result;

            return (int)list;
        }

        [HttpPost]
        [ActionName("GuardarQueja")]
        public async Task<object> PostQuejaAsync(TbQuejaDTO queja)
        {
            decimal cod = -1;
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                queja.CodigoAbogado = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("CodFuncRespQueja").ToString());

                cod = queja.CodigoQueja;
                if (cod > 0)
                {
                    var resp = await apiService.PostAsync<TbQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarQueja", queja, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                }
                else if (cod <= 0)
                {

                    var resp = await apiService.PostAsync<TbQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/GuardarQueja", queja, tokenG.Token);
                    if (!resp.IsSuccess) return null;
                    else cod = decimal.Parse(resp.Message);
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!", cod = cod };

        }

        [HttpGet]
        [ActionName("ObtenerQueja")]
        public async Task<JObject> ObtenerQueja(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                EditarQuejaDTO evento = new EditarQuejaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<EditarQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerQueja?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (EditarQuejaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("VincularExpediente")]
        public async Task<object> VincularExpediente(TbQuejaDTO queja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PostAsync<TbQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularExpediente", queja, tokenG.Token);
                if (!resp.IsSuccess) return null;


            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpGet, ActionName("ListadoQuejaTerceros")]
        public async Task<datosConsulta> GetListadoQuejaTercerosTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal cod)
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

            response = await apiService.GetListAsync<ListadoQuejaTercerosDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoQuejaTerceros?cod=" + cod, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoQuejaTercerosDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpPost, ActionName("VincularQuejaTerceroTAsync")]
        public async Task<object> VincularQuejaTerceroTAsync(QuejaTerceroDTO quejaTercero)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var sw = quejaTercero.IdQuejaTercero;

                if (sw <= 0)
                {
                    var resp = await apiService.PostAsync<QuejaTerceroDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularTerceroQueja", quejaTercero, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<QuejaTerceroDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarTerceroQueja", quejaTercero, tokenG.Token);
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
        [ActionName("TipoTerceroQuejaasync")]
        public async Task<JArray> TipoTerceroQuejaasync()
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

            response = await apiService.GetListAsync<TipoTerceroQuejaDTO>(urlApiQuejasAmbientales, "api/", "QuejaS/ObtenerTipoTerceroQueja", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TipoTerceroQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("TerceroNatural")]
        public async Task<JObject> GetTerceroNaturalAsync(decimal id)
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
        [ActionName("Tercero")]
        public async Task<JObject> GetTerceroAsync(decimal id)
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

            response = await apiService.GetAsync<TerceroInstalacionDTO>(urlApiTerceros, "api/", "Terceros/ObtenerTercero?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var list = (TerceroInstalacionDTO)response.Result;
            if (list == null) return JObject.FromObject("", Js);
            return JObject.FromObject(list, Js);
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

        [HttpGet]
        [ActionName("TerceroJuridico")]
        public async Task<JObject> GetTerceroJuridicoAsync(decimal id)
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

            response = await apiService.GetAsync<TerceroJuridicoDTO>(urlApiTerceros, "api/", "Terceros/ObtenerTerceroJuridico?id=" + id, tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JObject.FromObject(Msg, Js);
            }
            var list = (TerceroJuridicoDTO)response.Result;
            if (list == null) return JObject.FromObject("", Js);
            return JObject.FromObject(list, Js);
        }

        [HttpPost]
        [ActionName("ActualizarTerceroJuridico")]
        public async Task<object> PostTerceroJuridicoAsync(TerceroJuridicoDTO natural)
        {
            decimal Id = -1;
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.PostAsync<TerceroJuridicoDTO>(urlApiTerceros, "api/", "Terceros/ActualizarTerceroJuridico", natural, tokenG.Token);
                if (!resp.IsSuccess) return null;

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!", id = Id };

        }

        [HttpGet]
        [ActionName("ObtenerTerceroQueja")]
        public async Task<JObject> ObtenerTerceroQueja(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                EditarQuejaTerceroDTO evento = new EditarQuejaTerceroDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<EditarQuejaTerceroDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerQuejaTercero?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (EditarQuejaTerceroDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost]
        [ActionName("DesvincularTerceroQueja")]
        public async Task<object> DesvincularTerceroQuejaAsync(QuejaTerceroDTO quejaTercero)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<QuejaTerceroDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularTerceroQueja", quejaTercero, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet, ActionName("ListadoAutos")]
        public async Task<datosConsulta> GetListadoAutosTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoAutosDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoAutos?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoAutosDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("ListadoResoluciones")]
        public async Task<datosConsulta> GetListadoResolucionesTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoResolucionesDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoResoluciones?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoResolucionesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("ListadoRespuestas")]
        public async Task<datosConsulta> GetListadoRespuestasTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoRespuestasDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoRespuestas?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoRespuestasDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpPost, ActionName("VincularRespuestaQueja")]
        public async Task<object> VincularRespuestaQuejaAsync(ContestaQuejaDTO contestaQueja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var sw = contestaQueja.CodContestaQueja;

                if (sw <= 0)
                {
                    var resp = await apiService.PostAsync<ContestaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularRespuestaQueja", contestaQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<ContestaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarRespuestaQueja", contestaQueja, tokenG.Token);
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
        [ActionName("ObtenerRespuestaQueja")]
        public async Task<JObject> ObtenerRespuestaQuejaAsync(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                ContestaQuejaDTO evento = new ContestaQuejaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<ContestaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerRespuestaQueja?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (ContestaQuejaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost]
        [ActionName("DesvincularRespuestaQueja")]
        public async Task<object> DesvincularRespuestaQuejaTAsync(ContestaQuejaDTO contestaQueja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<ContestaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularRespuestaQueja", contestaQueja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        /// Estados Queja
        
        [HttpGet, ActionName("ListadoEstados")]
        public async Task<datosConsulta> GetListadoEstadosTAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoEstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoEstadoQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoEstadoQuejaDTO>)response.Result;
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
        [ActionName("TipoEstadoQueja")]
        public async Task<JArray> TipoEstadoQueja()
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

            response = await apiService.GetListAsync<TipoEstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "QuejaS/ObtenerTipoEstadoQueja", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<TipoEstadoQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpGet]
        [ActionName("GetFuncionarioAsync")]
        public async Task<JArray> GetFuncionarioAsync()
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

              response = await apiService.GetListAsync<ListaFuncionariosDTO>(urlApiGeneral, "api/", "Funcionario/ObtenerQryFuncionarios", tokenG.Token);
                if (!response.IsSuccess)
                {
                    var Msg = "Ocurrio un error en la consulta " + response.Message;
                    return JArray.FromObject(Msg, Js);
                }
                var list = (List<ListaFuncionariosDTO>)response.Result;
                if (list == null /*|| list.Count == 0*/) return JArray.FromObject("", Js); ;
                return JArray.FromObject(list, Js);
            

        }

        [HttpPost, ActionName("VincularEstadoQueja")]
        public async Task<object> VincularEstadoQueja(EstadoQuejaDTO estadoQueja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var sw = estadoQueja.CodEstadoQueja;

                if (sw <= 0)
                {
                    var resp = await apiService.PostAsync<EstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularEstadoQueja", estadoQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<EstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarEstadoQueja", estadoQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularEstadoQueja")]
        public async Task<object> DesvincularEstadoQueja(EstadoQuejaDTO estadoQueja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<EstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularEstadoQueja", estadoQueja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet]
        [ActionName("ObtenerEstadoQueja")]
        public async Task<JObject> ObtenerEstadoQueja(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                EstadoQuejaDTO evento = new EstadoQuejaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<EstadoQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerEstadoQueja?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (EstadoQuejaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        //citaciones

        [HttpGet, ActionName("ListadoCitaciones")]
        public async Task<datosConsulta> ListadoCitaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoCitacionesQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoCitacionQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoCitacionesQuejaDTO>)response.Result;
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
        [ActionName("ObjetoCitacion")]
        public async Task<JArray> ObjetoCitacion()
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

            response = await apiService.GetListAsync<ObjetoCitacionDTO>(urlApiQuejasAmbientales, "api/", "QuejaS/ObtenerObjetoCitacion", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<ObjetoCitacionDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpPost, ActionName("VincularCitacionQueja")]
        public async Task<object> VincularCitacionQueja(CitacionQuejaDTO citacionQueja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var sw = citacionQueja.CodCitacionQueja;

                if (sw <= 0)
                {
                    var resp = await apiService.PostAsync<CitacionQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularCitacionQueja", citacionQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<CitacionQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarCitacionQueja", citacionQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularCitacionQueja")]
        public async Task<object> DesvincularCitacionQueja(CitacionQuejaDTO citacionQueja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<CitacionQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularCitacionQueja", citacionQueja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet]
        [ActionName("ObtenerCitacionQueja")]
        public async Task<JObject> ObtenerCitacionQueja(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                CitacionQuejaDTO evento = new CitacionQuejaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<CitacionQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerCitacionQueja?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (CitacionQuejaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        //comentarios
        [HttpGet, ActionName("ListadoQuejaComentario")]
        public async Task<datosConsulta> ListadoQuejaComentario(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoQuejaComentarioDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoQuejaComentario?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoQuejaComentarioDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpPost, ActionName("VincularQuejaComentario")]
        public async Task<object> VincularQuejaComentario(QuejaComentarioDTO quejaComentario)
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

                var sw = quejaComentario.IdQuejaComentario;

                if (sw <= 0)
                {
                    quejaComentario.IdFuncionario = codFuncionario;
                    var resp = await apiService.PostAsync<QuejaComentarioDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularQuejaComentario", quejaComentario, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<QuejaComentarioDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarQuejaComentario", quejaComentario, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularQuejaComentario")]
        public async Task<object> DesvincularQuejaComentario(QuejaComentarioDTO quejaComentario)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<QuejaComentarioDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularQuejaComentario", quejaComentario, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet]
        [ActionName("ObtenerQuejaComentario")]
        public async Task<JObject> ObtenerQuejaComentario(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                QuejaComentarioDTO evento = new QuejaComentarioDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<QuejaComentarioDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerQuejaComentario?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (QuejaComentarioDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        //Visitas

        [HttpGet, ActionName("ListadoVisitasQueja")]
        public async Task<datosConsulta> ListadoVisitasQueja(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoVisitaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoVisitaQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoVisitaQuejaDTO>)response.Result;
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
        [ActionName("ObjetoVisita")]
        public async Task<JArray> ObjetoVisita()
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

            response = await apiService.GetListAsync<ObjetoVisitaDTO>(urlApiQuejasAmbientales, "api/", "QuejaS/ObtenerObjetoVisita", tokenG.Token);
            if (!response.IsSuccess)
            {
                var Msg = "Ocurrio un error en la consula " + response.Message;
                return JArray.FromObject(Msg, Js);
            }
            var list = (List<ObjetoVisitaDTO>)response.Result;
            if (list == null || list.Count == 0) return JArray.FromObject("", Js);
            return JArray.FromObject(list, Js);
        }

        [HttpPost, ActionName("VincularVisitaQueja")]
        public async Task<object> VincularVisitaQueja(VisitaQuejaDTO visitaQueja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var sw = visitaQueja.CodVisitaQueja;

                if (sw <= 0)
                {
                    var resp = await apiService.PostAsync<VisitaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularVisitaQueja", visitaQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

                if (sw > 0)
                {
                    var resp = await apiService.PostAsync<VisitaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ActualizarVisitaQueja", visitaQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;

                }

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularVisitaQueja")]
        public async Task<object> DesvincularVisitaQueja(VisitaQuejaDTO visitaQueja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<VisitaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularVisitaQueja", visitaQueja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        [HttpGet]
        [ActionName("ObtenerVisitaQueja")]
        public async Task<JObject> ObtenerVisitaQueja(decimal id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {


                VisitaQuejaDTO evento = new VisitaQuejaDTO();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return JObject.FromObject(evento, Js);


                response = await apiService.GetAsync<VisitaQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ObtenerVisitaQueja?id=" + id, tokenG.Token);
                if (!response.IsSuccess) return JObject.FromObject(evento, Js);
                evento = (VisitaQuejaDTO)response.Result;
                return JObject.FromObject(evento, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// oficios
        
        [HttpGet, ActionName("ListadoOficioQueja")]
        public async Task<datosConsulta> ListadoOficioQueja(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoOficioQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoOficioQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoOficioQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        /// atenciones
        

        [HttpGet, ActionName("QuejaAtenciones")]
        public async Task<datosConsulta> QuejaAtenciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoAtencionesDTO>(urlApiATU, "api/", "Atencion/ListadoAtencionesQueja?id=" + id, tokenG.Token);
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

        // informes

        [HttpGet, ActionName("ListadoInformeQueja")]
        public async Task<datosConsulta> ListadoInformeQueja(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoInformeQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoInformeQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoInformeQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("BuscarInforme")]
        public async Task<datosConsulta> BuscarInforme(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

                response = await apiService.GetListAsync<ListadoInformeQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/BuscarInformes", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoInformeQuejaDTO>)response.Result;
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

        [HttpPost, ActionName("VincularInformeQueja")]
        public async Task<object> VincularInformeQueja(InformeQuejaDTO informeQueja)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;
               
                    var resp = await apiService.PostAsync<InformeQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularInformeQueja", informeQueja, tokenG.Token);
                    if (!resp.IsSuccess) return null;
          

            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularInformeQueja")]
        public async Task<object> DesvincularInformeQueja(InformeQuejaDTO informeQueja)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<InformeQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularInformeQueja", informeQueja, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        //expedientes

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

            response = await apiService.GetListAsync<ListadoSolicitudesDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoSolicitudes?id=" + id, tokenG.Token);
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

                response = await apiService.GetListAsync<ListadoSolicitudesDTO>(urlApiQuejasAmbientales, "api/", "Quejas/BuscarSolicitudes", tokenG.Token);
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

        [HttpPost, ActionName("VincularSolicitudTAsync")]
        public async Task<object> VincularSolicitudTAsync(QuejaProyectoDTO solicitud)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;


                var resp = await apiService.PostAsync<QuejaProyectoDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularSolicitud", solicitud, tokenG.Token);
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
        public async Task<object> DesvincularSolicitudTAsync(QuejaProyectoDTO solicitud)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<QuejaProyectoDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularSolicitud", solicitud, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }

        //notificaciones

        [HttpGet, ActionName("ListadoNotificaciones")]
        public async Task<datosConsulta> ListadoNotificaciones(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoNotificacionesDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoNotificacionesQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoNotificacionesDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        //tramites

        [HttpGet, ActionName("ListadoTramiteQueja")]
        public async Task<datosConsulta> ListadoTramiteQueja(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal id)
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

            response = await apiService.GetListAsync<ListadoTramitesQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/ListadoTramitesQueja?id=" + id, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoTramitesQuejaDTO>)response.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }

        [HttpGet, ActionName("BuscarTramite")]
        public async Task<datosConsulta> BuscarTramite(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

                response = await apiService.GetListAsync<ListadoTramitesQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/BuscarTramites", tokenG.Token);
                if (!response.IsSuccess) return datosConsulta;
                var list = (List<ListadoTramitesQuejaDTO>)response.Result;
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

        [HttpPost, ActionName("VincularTramiteQueja")]
        public async Task<object> VincularTramiteQueja(TramiteExpedienteQuejaDTO tramite)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;


            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                var resp = await apiService.PostAsync<TramiteExpedienteQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/VincularTramiteQueja", tramite, tokenG.Token);
                if (!resp.IsSuccess) return null;


            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el registro : " + e.Message };
            }
            return new { resp = "OK", mensaje = "Registro almacenado correctamente!" };
        }

        [HttpPost]
        [ActionName("DesvincularTramiteQueja")]
        public async Task<object> DesvincularTramiteQueja(TramiteExpedienteQuejaDTO tramite)
        {
            try
            {
                ApiService apiService = new ApiService();

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "federico.martinez@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;



                var resp = await apiService.DeleteAsync<TramiteExpedienteQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/DesvincularTramite", tramite, tokenG.Token);
                if (!resp.IsSuccess) return null;

                return new { resp = "OK", mensaje = "Registro desvinculado correctamente!" };
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = e.Message };
            }
        }


        [HttpGet, ActionName("BuscarTramitePorId")]
        public async Task<datosConsulta> BuscarTramitePorId(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, decimal cod)
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

            response = await apiService.GetListAsync<ListadoTramitesQuejaDTO>(urlApiQuejasAmbientales, "api/", "Quejas/BuscarTramite?cod=" + cod, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<ListadoTramitesQuejaDTO>)response.Result;
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