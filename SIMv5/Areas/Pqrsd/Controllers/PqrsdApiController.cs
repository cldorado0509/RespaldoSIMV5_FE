namespace SIM.Areas.Pqrsd.Controllers
{
    using DevExpress.CodeParser;
    using DevExpress.Web.Internal;
    using DocumentFormat.OpenXml.Vml.Spreadsheet;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.Pqrsd.Models;
    using SIM.Data;
    using SIM.Models;
    using SIM.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Route("api/[controller]", Name = "PqrsdApi")]
    public class PqrsdApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        private string _UrlApiPqrsd = SIM.Utilidades.Data.ObtenerValorParametro("urlApiPqrsd").ToString();
        private string urlApiSecurity = SIM.Utilidades.Data.ObtenerValorParametro("urlApiSecurity").ToString();

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Retorna el listado de los tipos de solicitud para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTipoPqrsd")]
        public async Task<JArray> ObtenerTipoPqrsdAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerTiposSolicitud", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los tipos de persona para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTipoPersona")]
        public async Task<JArray> ObtenerTipoPersonadAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerTiposPersona", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las formas de recepción de las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerFormaRecepcion")]
        public async Task<JArray> ObtenerFormaRecepcionAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerFormasRecibe", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las formas de recepción de las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerMedioRpta")]
        public async Task<JArray> ObtenerMedioRptaAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerMediosRepta", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los diferentes recursos naturales para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerRecursoAmb")]
        public async Task<JArray> ObtenerRecursoAmbAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerRecursoAmb", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las diferentes afectaciones ambientales para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerAfectacionAmb")]
        public async Task<JArray> ObtenerAfectacionAmbAsync(int IdRecurso)
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<AfectacionDTO>(_UrlApiPqrsd, "api/", $"Listados/ObtenerAfectacionAmb?IdRecurso={IdRecurso}", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<AfectacionDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los diferentes tipos de documento para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerTiposDoc")]
        public async Task<JArray> ObtenerTiposDocAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerTiposDoc", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de los departamentos del pais para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerDeptos")]
        public async Task<JArray> ObtenerDeptosAsync()
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", "Listados/ObtenerDeptos", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de las diferentes afectaciones ambientales para las PQRSD
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ObtenerCiudad")]
        public async Task<JArray> ObtenerCiudadAsync(int IdDepto)
        {
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return null;

                response = await apiService.GetListAsync<ListadosDTO>(_UrlApiPqrsd, "api/", $"Listados/ObtenerCiudad?IdDepto={IdDepto}", tokenG.Token);
                if (!response.IsSuccess) return null;
                var list = (List<ListadosDTO>)response.Result;


                var ja = JArray.FromObject(list, Js);
                return ja;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost, ActionName("RecibeArch")]
        public async Task<ResponseArchivo> RecibeArchAsync(string IdPqrsd)
        {
            ResponseArchivo archivoDTO = new ResponseArchivo();
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var httpRequest = context.Request;
            if (httpRequest.Files.Count > 0)
            {
                if (IdPqrsd != null)
                {
                    var File = httpRequest.Files[0];
                    if (File != null && File.ContentLength > 0)
                    {
                        try
                        {
                            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                            if (!response.IsSuccess)
                                throw new Exception(response.Message);
                            var tokenG = (TokenResponse)response.Result;
                            if (tokenG == null) throw new Exception("La solicitud del token arrojó un resultado null");
                            string[] fileExtensions = { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
                            var fileName = File.FileName.ToLower();
                            var isValidExtenstion = fileExtensions.Any(ext =>
                            {
                                return fileName.LastIndexOf(ext) > -1;
                            });
                            if (isValidExtenstion)
                            {
                                BinaryReader b = new BinaryReader(File.InputStream);
                                byte[] FileData = b.ReadBytes(File.ContentLength);

                                if (IdPqrsd == "0") IdPqrsd = SIM.Utilidades.Cryptografia.RandomString(10).ToUpper();

                                ArchivoAnexoDTO datos = new ArchivoAnexoDTO();
                                datos.IdPQRSD = IdPqrsd;

                                datos.Anexo = FileData;
                                datos.extension = Path.GetExtension(fileName).ToLower();

                                response = await apiService.PostFileAsync<ArchivoAnexoDTO>(_UrlApiPqrsd, "api/", "Archivos/RecibeAnexo", datos, tokenG.Token);
                                if (!response.IsSuccess)
                                {
                                    archivoDTO.IdPQRSD = "0";
                                    archivoDTO.SubidaExitosa = false;
                                    archivoDTO.MensajeError = response.Result.ToString();
                                }
                                var datosArchivo = (ResponseArchivo)response.Result;
                                archivoDTO = datosArchivo;
                            }
                            else
                            {
                                archivoDTO.IdPQRSD = IdPqrsd;
                                archivoDTO.SubidaExitosa = false;
                                archivoDTO.MensajeError = "El tipo de archivo no esta permitido";
                            }
                        }
                        catch (Exception exp)
                        {
                            archivoDTO.IdPQRSD = IdPqrsd;
                            archivoDTO.SubidaExitosa = false;
                            archivoDTO.MensajeError = exp.Message;
                        }
                    }
                }
                else
                {
                    archivoDTO.IdPQRSD = IdPqrsd;
                    archivoDTO.SubidaExitosa = false;
                    archivoDTO.MensajeError = "No se ingresó un identificador para poder subir los anexos de su solicitud";
                }
            }
            return archivoDTO;
        }

        [HttpGet, ActionName("EliminaAnexo")]
        public async Task<object> EliminaAnexoAsync(string IdAnexo, string IdPqrsd)
        {
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return new { resp ="Error", mensaje = response.Message };

                response = await apiService.GetAsync<ArchivoAnexoDTO>(_UrlApiPqrsd, "api/", $"Archivos/EliminaAnexo?IdAnexo={IdAnexo}&IdPqrsd={IdPqrsd}", tokenG.Token);
                if (!response.IsSuccess) return new { resp = "Error", mensaje = response.Message };
            }
            catch (Exception exp)
            {
                return new { resp = "Error", mensaje = "Error eliminando el archivo " + exp.Message };
            }
            return new { resp = "Ok", mensaje = "Documento eliminado correctamente" };
        }

        [HttpGet, ActionName("ValidaTramite")]
        public async Task<VerificaTramiteDTO> GatValidaTramite(string CodTramite)
        {
            ApiService apiService = new ApiService();
            VerificaTramiteDTO _resp= new VerificaTramiteDTO();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null)
                {
                    _resp.TramiteVerificado = false;
                    _resp.Mensaje = response.Message;
                    return _resp;
                }
                if (CodTramite != string.Empty)
                {
                    response = await apiService.GetAsync<VerificaTramiteDTO>(_UrlApiPqrsd, "api/", $"Pqrsd/VerificaTramite?Tramite={CodTramite}", tokenG.Token);
                    if (response.IsSuccess) _resp = (VerificaTramiteDTO)response.Result;
                }
                else
                {
                    _resp.TramiteVerificado = false;
                    _resp.Mensaje = "No se ingresó un código de trámite para validar!!";
                }
            }
            catch (Exception exp)
            {
                _resp.TramiteVerificado = false;
                _resp.Mensaje = exp.Message;
                return _resp;
            }
            return _resp;
        }

        [HttpPost, ActionName("IngresaPqrsd")]
        public async Task<object> PostIngresaPqrsd(IngresoPqrsdDTO Datos)
        {
            ApiService apiService = new ApiService();
            bool _radicar = false;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
                if (!response.IsSuccess) return null;
                var tokenG = (TokenResponse)response.Result;
                if (tokenG == null) return new { resp = "Error", mensaje = response.Message };
                string _resp = await ValidaFormularioPqrsd(Datos, tokenG);
                if (_resp.Length == 0)
                {
                    Response resp = await apiService.GetAsync<bool>(_UrlApiPqrsd, "api/", $"Pqrsd/SePuedeRadicar", tokenG.Token);
                    if (resp.IsSuccess) _radicar = (bool)resp.Result;
                    if (_radicar)
                    {
                        return new { resp = "Error", mensaje = "radicado el documento!!" };
                    }
                    else return new { resp = "Error", mensaje = "En este horario no es posible radicar el documento!!" };
                }
                else return new { resp = "Error", mensaje = _resp };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = "Error al ingresar la solicitud " + ex.Message };
            }
        }

        #region "Metodos privados del servicio"
        private async Task<string> ValidaFormularioPqrsd(IngresoPqrsdDTO Datos, TokenResponse token)
        {
            ApiService apiService = new ApiService();
            string _resp = string.Empty;
            if (Datos.TipoSolicitud.Length == 0) _resp = "Aún no ha seleccionado un tipo de solicitud!!";
            else
            {
                if (Datos.TipoSolicitud.ToUpper() == "QUEJA AMBIENTAL")
                {
                    if (Datos.Recurso.Length == 0 || Datos.Afectacion.Length == 0) _resp = "Si el tipo de solicitud es Queja Ambiental debe especificar el recurso y el tipo de afectación denunciado!!";
                }
            }
            if (Datos.TipoSolicitante.Length == 0) _resp = "Aún no ha seleccionado un tipo de solicitante!!";
            if (Datos.CorreoElectronico.Length == 0) _resp = "Debe ingresar un correo electrónico!!";
            else
            {
                string validadorMail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(Datos.CorreoElectronico, validadorMail))
                {
                    if (Regex.Replace(Datos.CorreoElectronico, validadorMail, string.Empty).Length != 0) _resp = "El formato de su correo electrónico no es válido!!";
                }
                else
                {
                    _resp = "El formato de su correo electrónico no es válido!!";
                }
            }
            if (Datos.Telefono.Length >= 0)
            {
                string validaTel = "^[0-9 ]*$";
                if (Regex.IsMatch(Datos.Telefono, validaTel))
                {
                    if (Regex.Replace(Datos.Telefono, validaTel, string.Empty).Length != 0) _resp = "El formato de su teléfono no es válido!!";
                }
                else _resp = "El formato de su teléfono no es válido!!";
            }
            if (Datos.MedioRespuesta.Length == 0) _resp = "Aún no ha seleccionado un medio de respuesta a su solicitud!!";
            else
            {
                switch (Datos.MedioRespuesta.ToUpper())
                {
                    case "CORREO CERTIFICADO":
                        if (Datos.Direccion.Length == 0) _resp = "Si el medio de respuesta es correo certificado debe proporcionar su dirección!!";
                        break;
                    case "TELEFÓNICAMENTE":
                        if (Datos.Telefono.Length == 0) _resp = "Si el medio de respuesta es telefónico debe proporcionar su número de teléfono!!";
                        break;
                }
            }
            if (Datos.TextoContenido.Length == 0) _resp = "Aún no ha ingresado la descripción de su solicitud";
            if (Datos.EmergenciaAmbiental)
            {
                if (Datos.TipoSolicitud.ToUpper() != "QUEJA AMBIENTAL" && Datos.TipoSolicitud.ToUpper() == "PETICIÓN") _resp = "Las solcitudes de UEA solo apliocan para Queja Ambiental o Petición";
            }
            if (Datos.CodTramite.Length > 0)
            {
                bool existe = false;
                try
                {
                    Response resp = await apiService.GetAsync<bool>(_UrlApiPqrsd, "api/", $"Pqrsd/ExisteTramite?Tramite={Datos.CodTramite}", token.Token);
                    if (resp.IsSuccess) existe = (bool)resp.Result;
                    if (!existe) _resp = "El código de trámite ingresado no existe!!";
                }
                catch { }
            }
            try
            {
                Response response = await apiService.PostAsync<IngresoPqrsdDTO>(_UrlApiPqrsd, "api/", $"Pqrsd/ExistePqrsd", Datos, token.Token);
                if (response.IsSuccess) _resp = response.Result.ToString();
            }
            catch { }

            return _resp;
        }
        #endregion
    }
}
