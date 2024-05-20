using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ExpedienteAmbiental.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using SIM.Models;
using SIM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace SIM.Areas.Seguridad.Controllers
{
    public class UsuarioApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certification"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        private string urlApiSeguridad = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioSeguridad").ToString();
        private string urlApiTerceros = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();
        private string urlApiSecurity = SIM.Utilidades.Data.ObtenerValorParametro("urlApiSecurity").ToString();

        // GET api/<controller>
        [HttpGet]
        [ActionName("Usuarios")]
        public datosConsulta GetUsuarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                            var propietario = (from propietarioUsuario in dbSIM.PROPIETARIO
                                               where propietarioUsuario.ID_USUARIO == idUsuario
                                               select propietarioUsuario).FirstOrDefault();

                            if (propietario == null)
                            {
                                datosConsulta resultadoVacio = new datosConsulta();
                                resultadoVacio.numRegistros = 0;
                                resultadoVacio.datos = null;

                                return resultadoVacio;
                            }
                            else
                            {
                                var model = (from usuario in dbSIM.USUARIO
                                             join solicitado in dbSIM.ROL_SOLICITADO on usuario.ID_USUARIO equals solicitado.ID_USUARIO
                                             where solicitado.S_ESTADO == "V" && solicitado.ID_TERCERO == propietario.ID_TERCERO
                                             select new
                                             {
                                                 usuario.ID_USUARIO,
                                                 usuario.S_LOGIN,
                                                 usuario.S_APELLIDOS,
                                                 usuario.S_NOMBRES
                                             });

                                modelData = model;
                            }
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         select new
                                         {
                                             usuario.ID_USUARIO,
                                             usuario.S_LOGIN,
                                             usuario.S_APELLIDOS,
                                             usuario.S_NOMBRES
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         select new
                                         {
                                             ID_POPUP = usuario.ID_USUARIO,
                                             S_NOMBRE_LOOKUP = usuario.S_NOMBRES + " " + usuario.S_APELLIDOS + " (" + usuario.S_LOGIN + ")",
                                         });

                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <param name="sort"></param>
        ///// <param name="group"></param>
        ///// <param name="skip"></param>
        ///// <param name="take"></param>
        ///// <param name="searchValue"></param>
        ///// <param name="searchExpr"></param>
        ///// <param name="comparation"></param>
        ///// <param name="noFilterNoRecords"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[ActionName("ObtieneUsuarios")]
        //public async Task<DatosConsultaDTO> GetObtieneUsuarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, bool noFilterNoRecords)
        //{
        //    ApiService apiService = new ApiService();
        //    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        //    DatosConsultaDTO datosConsulta = new DatosConsultaDTO
        //    {
        //        datos = null,
        //        numRegistros = 0,
        //    };
        //    var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
        //    string token = _token.Value;
        //    string _controller = $"Usuarios/ObtenerUsuarios?filter={filter}&group={group}&skip={skip}&take={take}&sort={sort}&noFilterRecords={noFilterNoRecords}";
        //    Response response = await apiService.GetFilteredDataAsync<UsuariosDTO>(urlApiSeguridad, "api/", _controller, token);
        //    if (!response.IsSuccess) return datosConsulta;
        //    // var data = (DatosConsultaDTO)response.Result;
        //    // if (data == null) return datosConsulta;
        //    if (response.IsSuccess)
        //    {
        //        var lista = (List<UsuariosDTO>)response.Result;
        //        datosConsulta.numRegistros = (int)response.TotalItems;
        //        datosConsulta.datos = lista;
        //    }
        //    else
        //    {
        //        datosConsulta.datos = null;
        //        datosConsulta.numRegistros = 0;
        //    }
        //    return datosConsulta;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUsuarios")]
        public async Task<LoadResult> GetUsuarios(DataSourceLoadOptions loadOptions)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Usuarios/GetUsuarios?opciones={serloadOptions}";
                SIM.Models.Response response = await apiService.GetFilteredDataAsync(urlApiSeguridad, "api/", _controller, token);
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

                    loadResult.data = dynamicResponse.data.ToObject<List<UsuariosDTO>>();
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
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DetalleUsuario")]
        public async Task<JObject> GetUsuario(int IdUsuario)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            ApiService apiService = new ApiService();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                DatosUsuarioDTO datosUsuario = new DatosUsuarioDTO();
                string _controller = $"Usuarios/ObtenerDatosUsuario?IdUsr={IdUsuario}";
                Response response = await apiService.GetAsync<DatosUsuarioDTO>(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return JObject.FromObject(response, Js);
                datosUsuario = (DatosUsuarioDTO)response.Result;
                return JObject.FromObject(datosUsuario, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GuardaUsuario")]
        public async Task<object> PostGuardaUsuario(DatosUsuarioDTO objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el usuario" };
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                DatosUsuarioDTO datosUsuario = new DatosUsuarioDTO();
                var response = new SIM.Models.Response();
                string _controller = "";
                if (objData.Password != null && objData.Password != "")
                {
                    objData.Password = string.Join("", (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(objData.Password))).Select(x => x.ToString("X2")).ToArray());
                }
                if (objData.IdUsuario > 0)
                {
                    _controller = $"Usuarios/ActualizarUsuario";
                    response = await apiService.PostMicroServicioAsync<DatosUsuarioDTO>(urlApiSeguridad, "api/", _controller, objData, token);
                    if (!response.IsSuccess) return new { resp = "Error", mensaje = "Error Almacenando el usuario - " + response.Message };
                }
                else if (objData.IdUsuario <= 0)
                {
                    _controller = $"Usuarios/CrearUsuario";
                    response = await apiService.PostMicroServicioAsync<DatosUsuarioDTO>(urlApiSeguridad, "api/", _controller, objData, token);
                    if (!response.IsSuccess) return new { resp = "Error", mensaje = "Error Almacenando el usuario - " + response.Message };
                }
                var result = JsonConvert.DeserializeObject<Response>(response.Result.ToString());
                if (result.IsSuccess) return new { resp = "Ok", mensaje = "Datos Almacenados correctamente" };
                else return new { resp = "Error", mensaje = "Error Almacenando el usuario : " + result.Message };
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = ex.Message };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ListaGrupos")]
        public async Task<JArray> GetListaGrupos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<ListadoDTO> Grupos = new List<ListadoDTO>();
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Usuarios/ObtenerListaGrupos";
                Response response = await apiService.GetListAsync<ListadoDTO>(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return JArray.FromObject(Grupos, Js);
                return JArray.FromObject(response.Result, Js);
            }
            catch
            {
                return JArray.FromObject(Grupos, Js);
            }
        }

        [HttpGet]
        [ActionName("ListaFuncionarios")]
        public async Task<JArray> GetListaFuncionarios()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<ListadoDTO> Grupos = new List<ListadoDTO>();
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Usuarios/ObtenerListaFuncionarios";
                Response response = await apiService.GetListAsync<ListadoDTO>(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return JArray.FromObject(Grupos, Js);
                return JArray.FromObject(response.Result, Js);
            }
            catch
            {
                return JArray.FromObject(Grupos, Js);
            }
        }

        [HttpGet]
        [ActionName("LoginDisponible")]
        public async Task<bool> LoginDisponible(string strLogin)
        {
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Usuarios/LoginDisponible?strLogin={strLogin}";
                Response response = await apiService.GetAsync<bool>(urlApiSeguridad, "api/", _controller, token);
                if (!response.IsSuccess) return response.IsSuccess;
                return (bool)response.Result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FiltroTercero"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("BuscarTercero")]
        public async Task<JArray> GetBuscarTercero(string FiltroTercero)
        {
            if (string.IsNullOrEmpty(FiltroTercero)) return null;
            string[] Filtro = FiltroTercero.Split(';');
            string _documento = Filtro[0];
            string _nombre = Filtro[1];
            ApiService apiService = new ApiService();
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;

                var listDoc = new List<TercerosDTO>();
                var listNom = new List<TercerosDTO>();
                if (!string.IsNullOrEmpty(_documento))
                {
                    var response = await apiService.GetAsync<List<TercerosDTO>>(urlApiTerceros, "api/", "Terceros/BuscarTerceroDocumento?Documento=" + _documento, token);
                    if (response.IsSuccess) listDoc = (List<TercerosDTO>)response.Result;
                }
                if (!string.IsNullOrEmpty(_nombre))
                {
                    var response = await apiService.GetAsync<List<TercerosDTO>>(urlApiTerceros, "api/", "Terceros/BuscarTerceroNombre?Nombre=" + _nombre, token);
                    if (response.IsSuccess) listNom = (List<TercerosDTO>)response.Result;
                }

                if (listDoc.Count > 0)
                {
                    if (listNom.Count > 0)
                    {
                        foreach (var item in listNom) listDoc.Add(item);
                    }
                    return JArray.FromObject(listDoc, Js);
                }
                else if (listNom.Count > 0)
                {
                    return JArray.FromObject(listNom, Js);
                }
                return JArray.FromObject(listNom, Js);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [ActionName("AsociarTercero")]
        public async Task<object> PostAsociarTercero(int IdUsuario, int IdTercero)
        {
            ApiService apiService = new ApiService();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Usuarios/AsociaTercero?IdUsuario={IdUsuario}&IdTercero={IdTercero}";
                var response = await apiService.PostAsync<Response>(urlApiSeguridad, "api/", _controller, null, token);
                if (!response.IsSuccess) return new { resp = "Error", mensaje = response.Message };
                return new { resp = "Ok", mensaje = "Datos Almacenados correctamente" };
            }
            catch
            {
                return new { resp = "Error", mensaje = "Ocurrió un error asociando el tercero" };
            }

        }

        [HttpGet]
        [ActionName("Funcionarios")]
        public datosConsulta GetFuncionarios(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;

            if ((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                switch (tipoData)
                {
                    case "f": // full
                        {
                            var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                                         where f.ACTIVO == "1"
                                         select new
                                         {
                                             CODFUNCIONARIO = f.CODFUNCIONARIO,
                                             NOMBRE = f.NOMBRES,
                                             DEPENDENCIA = f.DEPENDENCIA
                                         });

                            modelData = model;
                        }
                        break;
                    case "r": // reduced
                        {
                            var model = (from f in dbSIM.QRY_FUNCIONARIO_ALL
                                         where f.ACTIVO == "1"
                                         select new
                                         {
                                             f.CODFUNCIONARIO,
                                             NOMBRE = f.NOMBRES,
                                             DEPENDENCIA = f.DEPENDENCIA
                                         });

                            modelData = model;
                        }
                        break;
                    default: // (l) lookup
                        {
                            var model = (from usuario in dbSIM.USUARIO
                                         join funcionario in dbSIM.USUARIO_FUNCIONARIO on usuario.ID_USUARIO equals funcionario.ID_USUARIO
                                         select new
                                         {
                                             ID_POPUP = funcionario.CODFUNCIONARIO,
                                             S_NOMBRE_LOOKUP = usuario.S_NOMBRES + " " + usuario.S_APELLIDOS
                                         });

                            modelData = model;
                        }
                        break;
                }

                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
    }
}