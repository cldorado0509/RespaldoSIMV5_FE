using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ProcesosJudiciales.DTOs;
using SIM.Data;
using SIM.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Response = SIM.Models.Response;

namespace SIM.Areas.ProcesosJudiciales.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcesosJudicialesApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        string urlApiJudicial = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioJudicial").ToString();
        string urlApiGerencial = SIM.Utilidades.Data.ObtenerValorParametro("UrlMicroSitioGerencial").ToString();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaProcesosJudiciales")]
        public async Task<LoadResult> GetConsultaProcesosJudiciales(DataSourceLoadOptions loadOptions)
        {
            //urlApiJudicial= "https://localhost:7171/";
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"ProcesosJudiciales/GetConsultaProcesosJudiciales?Opciones={serloadOptions}";
                SIM.Models.Response response = await apiService.GetFilteredDataAsync(urlApiJudicial, "api/", _controller, token);
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

                    loadResult.data = dynamicResponse.data.ToObject<List<ProcesosJudicialesDTO>>();
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
        /// Obtiene un proceso judicial
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ObtenerProcesoJudicial")]
        public async Task<JObject> ObtenerProcesoJudicialAsync(int Id)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            urlApiJudicial= "https://localhost:7171/";

            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;

                ApiService apiService = new ApiService();

                ProcesoJudicialDTO proceso = new ProcesoJudicialDTO();


                Response response = await apiService.GetMicroServicioAsync<ProcesoJudicialDTO>(this.urlApiJudicial, "api/ProcesosJudiciales/", $"ObtenerProcesoJudicial?idProceso={Id}", token);
                if (!response.IsSuccess) return JObject.FromObject(proceso, Js);
                proceso = (ProcesoJudicialDTO)response.Result;
                return JObject.FromObject(proceso, Js);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaActuaciones")]
        public async Task<LoadResult> ConsultaActuaciones(DataSourceLoadOptions loadOptions)
        {
            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Listados/GetConsultaProcesosJudiciales?Opciones={serloadOptions}";



                List<ActuacionDTO> actuacionesDTOs = new List<ActuacionDTO>();
                actuacionesDTOs.Add(new ActuacionDTO { ActuacionId = 1, Etapa = "Radicación", Conciliacion = true, ValorConciliado = 23456789, Finalizado = false, ComiteVerificacion = true, Desacato = false });
                actuacionesDTOs.Add(new ActuacionDTO { ActuacionId = 2, Etapa = "Rechazo", Conciliacion = false, ValorConciliado = 0, Finalizado = false, ComiteVerificacion = true, Desacato = false });
                actuacionesDTOs.Add(new ActuacionDTO { ActuacionId = 3, Etapa = "Audiencia Inicial", Conciliacion = false, ValorConciliado = 0, Finalizado = false, ComiteVerificacion = true, Desacato = false });

                SIM.Models.Response response = new SIM.Models.Response();
                response.IsSuccess = true;
                response.Result = actuacionesDTOs;

                if (!response.IsSuccess) return null;
                if (response.IsSuccess)
                {
                    //dynamic dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                    LoadResult loadResult = new LoadResult()
                    {
                        totalCount = 2,
                        groupCount = 1,

                    };

                    loadResult.data = actuacionesDTOs;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJurisdicciones")]
        public async Task<JArray> GetJurisdicciones()
        {
            ApiService apiService = new ApiService();

            //urlApiJudicial= "https://localhost:7171/";

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Jurisdicciones";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ProcesoCodigoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ProcesoCodigoDTO>)response.Result;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetProcuradurias")]
        public async Task<JArray> GetProcuradurias()
        {
            ApiService apiService = new ApiService();
            //urlApiJudicial= "https://localhost:7171/";

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Procuradurias";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ProcuraduriaDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ProcuraduriaDTO>)response.Result;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMediosControl")]
        public async Task<JArray> GetMediosControl()
        {
            ApiService apiService = new ApiService();
            //urlApiJudicial= "https://localhost:7171/";

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/MediosControl";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ProcesoCodigoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ProcesoCodigoDTO>)response.Result;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJuzgados")]
        public async Task<JArray> GetJuzgados()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Juzgados";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetEstadoProcesos")]
        public async Task<JArray> GetEstadoProcesos()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/EstadosProceso";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTiposCuantia")]
        public async Task<JArray> GetTiposCuantia()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/TiposCuantia";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<ListadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ListadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <param name="idProceso"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaDemandados")]
        public async Task<JArray> ConsultaDemandados(int idProceso)
        {

            //urlApiJudicial = "https://localhost:7171/";

            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Demandados?idProceso=" + idProceso;

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<DemandadosDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<DemandadosDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Nombre), Js);

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
        /// <param name="idProceso"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaDemandantes")]
        public async Task<JArray> ConsultaDemandantes(int idProceso)
        {
            //urlApiJudicial = "https://localhost:7171/";
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Demandantes?idProceso=" + idProceso;

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<DemandantesDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<DemandantesDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Nombre), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCalidadEntidad")]
        public JArray GetCalidadEntidad()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "8120", Valor = "Demandado" });
                list.Add(new ListadoDTO { Id = "9120", Valor = "Demandante" });
                list.Add(new ListadoDTO { Id = "C", Valor = "Vinculado" });
                list.Add(new ListadoDTO { Id = "G", Valor = "Litis Consorte" });
                list.Add(new ListadoDTO { Id = "TV", Valor = "Vinculado" });
                list.Add(new ListadoDTO { Id = "O", Valor = "Tercero en Garantías" });
                list.Add(new ListadoDTO { Id = "V", Valor = "Víctimas" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetApoderados")]
        public async Task<JArray> GetApoderados()
        {
            //urlApiJudicial = "https://localhost:7171/";

            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Apoderados";

                Response response = await apiService.GetMicroServicioListAsync<ApoderadoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<ApoderadoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Nombre), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetErogacion")]
        public JArray GetErogacion()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Si" });
                list.Add(new ListadoDTO { Id = "0", Valor = "No" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRiesgosProcesales")]
        public JArray GetRiesgosProcesales()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Alto" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Medio alta" });
                list.Add(new ListadoDTO { Id = "3", Valor = "Medio bajo" });
                list.Add(new ListadoDTO { Id = "4", Valor = "Bajo" });
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTipoValorEconomico")]
        public JArray GetTipoValorEconomico()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Determinado" });
                list.Add(new ListadoDTO { Id = "0", Valor = "Indeterminado" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetTiposPretencion")]
        public JArray GetTiposPretencion()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "Lucro Cesanta " });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño emergente" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño material otros" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño moral" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño a la salud" });
                list.Add(new ListadoDTO { Id = "2", Valor = "Daño inmaterail - Otros" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCuantias")]
        public JArray GetCuantias()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "MENOR" });
                list.Add(new ListadoDTO { Id = "2", Valor = "MEDIA" });
                list.Add(new ListadoDTO { Id = "3", Valor = "MAYOR" });
                list.Add(new ListadoDTO { Id = "4", Valor = "SIN CUANTÍA" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetJuramentosEstimatorio")]
        public JArray GetJuramentosEstimatorio()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "RADICACIÓN" });
                list.Add(new ListadoDTO { Id = "2", Valor = "INADMISIÓN" });
                list.Add(new ListadoDTO { Id = "3", Valor = "RECHAZO" });
                list.Add(new ListadoDTO { Id = "4", Valor = "ADMISION" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetEspecialidadJuzgados")]
        public JArray GetEspecialidadJuzgados()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "ADMINISTRATIVOS " });
                list.Add(new ListadoDTO { Id = "2", Valor = "CIVILES CIRCUITO" });
                list.Add(new ListadoDTO { Id = "3", Valor = "JUZGADO CIVIL CIRCUITO EJECUCIÓN SENTENCIAS " });
                list.Add(new ListadoDTO { Id = "4", Valor = "CIVIL CIRCUITO DE RESTITUCION DE TIERRAS" });
                list.Add(new ListadoDTO { Id = "5", Valor = "EJECUCION DE PENAS Y MEDIDAS DE SEGURIDAD DEL CIRCUITO" });
                list.Add(new ListadoDTO { Id = "6", Valor = "LABORAL" });
                list.Add(new ListadoDTO { Id = "7", Valor = "FAMILIA" });
                list.Add(new ListadoDTO { Id = "8", Valor = "PENAL CIRCUITO" });
                list.Add(new ListadoDTO { Id = "9", Valor = "PENAL CIRCUITO ESPECIALIZADO" });
                list.Add(new ListadoDTO { Id = "10", Valor = "PENAL ESPECIALIZADO EN EXTINCION DE DOMINIO" });
                list.Add(new ListadoDTO { Id = "11", Valor = "PENAL CIRCUITO PARA ADOLECENTES" });
                list.Add(new ListadoDTO { Id = "12", Valor = "PEQUEÑAS CAUSAS DE COMPETENCIA MÚLTIPLE" });
                list.Add(new ListadoDTO { Id = "13", Valor = "PEQUEÑAS CAUSAS LABORAL" });
                list.Add(new ListadoDTO { Id = "14", Valor = "CIVIL MUNICIPAL " });
                list.Add(new ListadoDTO { Id = "15", Valor = "PENAL MUNICIPAL CON FUNCION DE CONOCIMIENTO" });
                list.Add(new ListadoDTO { Id = "16", Valor = "PENAL MUNICIPAL CON FUNCION DE CONTROL DE GARANTÍAS" });
                list.Add(new ListadoDTO { Id = "17", Valor = "PENAL MUNICIPAL CON FUNCION DE CONTROL DE GARANTIAS PARA ADOLESCENTES" });
                list.Add(new ListadoDTO { Id = "18", Valor = "TRIBUNAL ADMINISTRATIVO" });
                list.Add(new ListadoDTO { Id = "19", Valor = "TRIBUNAL SUPERIOR" });
                list.Add(new ListadoDTO { Id = "20", Valor = "INSPECCION MUNICIPAL" });

                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDerechosTutela")]
        public JArray GetDerechosTutela()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "DEBIDO PROCESO" });
                list.Add(new ListadoDTO { Id = "2", Valor = "DERECHO A LA HONRA" });
                list.Add(new ListadoDTO { Id = "3", Valor = "DERECHO A LA PAZ" });
                list.Add(new ListadoDTO { Id = "4", Valor = "DERECHO A LA REPARACION A POBLACION VICTIMA DEL DESPLAZAMIENTO" });
                list.Add(new ListadoDTO { Id = "5", Valor = "DERECHO AL BUEN NOMBRE" });
                list.Add(new ListadoDTO { Id = "6", Valor = "DERECHO DE PETICION" });
                list.Add(new ListadoDTO { Id = "7", Valor = "DIGNIDAD HUMANA" });
                list.Add(new ListadoDTO { Id = "8", Valor = "EDUCACION" });
                list.Add(new ListadoDTO { Id = "9", Valor = "ELEGIR Y SER ELEGIDO" });
                list.Add(new ListadoDTO { Id = "10", Valor = "ESTABILIDAD LABORAL REFORZADA" });
                list.Add(new ListadoDTO { Id = "11", Valor = "FAMILIA" });
                list.Add(new ListadoDTO { Id = "12", Valor = "HABEAS DATA" });
                list.Add(new ListadoDTO { Id = "13", Valor = "IDENTIDAD CULTURAL" });
                list.Add(new ListadoDTO { Id = "14", Valor = "IDENTIDAD SEXUAL Y DE GENERO" });
                list.Add(new ListadoDTO { Id = "15", Valor = "IGUALDAD" });
                list.Add(new ListadoDTO { Id = "16", Valor = "INTEGRIDAD PERSONAL FISICA Y PSICOLOGICA" });
                list.Add(new ListadoDTO { Id = "17", Valor = "INTERRUPCION VOLUNTARIA DEL EMBARAZO" });
                list.Add(new ListadoDTO { Id = "18", Valor = "INTIMIDAD" });
                list.Add(new ListadoDTO { Id = "19", Valor = "INTIMIDAD FAMILIAR" });
                list.Add(new ListadoDTO { Id = "20", Valor = "LIBERTAD" });
                list.Add(new ListadoDTO { Id = "21", Valor = "LIBERTAD DE CULTO" });
                list.Add(new ListadoDTO { Id = "22", Valor = "LIBERTAD DE ENSEÑANZA" });
                list.Add(new ListadoDTO { Id = "23", Valor = "LIBERTAD DE EXPRESION" });

                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDerechosAccionPupular")]
        public JArray GetDerechosAccionPupular()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "El goce de un ambiente sano, de conformidad con lo establecido en la Constitución, la ley y las disposiciones reglamentarias " });
                list.Add(new ListadoDTO { Id = "2", Valor = "La moralidad administrativa" });

                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCategoriasJuzgados")]
        public JArray GetCategoriasJuzgados()
        {
            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback= delegate { return true; };
            try
            {
                var list = new List<ListadoDTO>();
                list.Add(new ListadoDTO { Id = "1", Valor = "MUNICIPAL " });
                list.Add(new ListadoDTO { Id = "2", Valor = "CIRCUITO" });
                list.Add(new ListadoDTO { Id = "3", Valor = "TRIBUNAL" });
                list.Add(new ListadoDTO { Id = "4", Valor = "CORTE" });
                var listDto = JArray.FromObject(list.OrderBy(o => o.Valor), Js);

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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDepartamentos")]
        public async Task<JArray> GetDepartamentos()
        {
            //urlApiJudicial= "https://localhost:7171/";

            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/Departamentos";

                SIM.Models.Response response = await apiService.GetMicroServicioListAsync<DepartamentoDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<DepartamentoDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Nombre), Js);

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
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaTerceros")]
        public async Task<LoadResult> GetConsultaTerceros(DataSourceLoadOptions loadOptions)
        {
            //urlApiJudicial = "https://localhost:7171/";

            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Listados/Terceros?Opciones={serloadOptions}";
                Response response = await apiService.GetFilteredDataAsync(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                dynamic dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                LoadResult loadResult = new LoadResult()
                {
                    totalCount = dynamicResponse.totalCount,
                    groupCount = dynamicResponse.groupCount,
                    summary = dynamicResponse.summary
                };
                loadResult.data = dynamicResponse.data.ToObject<List<TerceroDTO>>();
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
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ConsultaTercerosC")]
        public async Task<LoadResult> GetConsultaTercerosC(DataSourceLoadOptions loadOptions)
        {
            //urlApiJudicial = "https://localhost:7171/"; 


            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string serloadOptions = JsonConvert.SerializeObject(loadOptions);
                string _controller = $"Listados/Terceros?Opciones={serloadOptions}";
                Response response = await apiService.GetFilteredDataAsync(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                dynamic dynamicResponse = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
                LoadResult loadResult = new LoadResult()
                {
                    totalCount = dynamicResponse.totalCount,
                    groupCount = dynamicResponse.groupCount,
                    summary = dynamicResponse.summary
                };
                loadResult.data = dynamicResponse.data.ToObject<List<TerceroDTO>>();
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
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMunicipios")]
        public async Task<JArray> GetMunicipios(int departamentoId)
        {
            //urlApiJudicial= "https://localhost:7171/";

            ApiService apiService = new ApiService();

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"Listados/ObtenerCiudad?IdDepto=" + departamentoId;

                Response response = await apiService.GetMicroServicioListAsync<MunicipioDTO>(urlApiJudicial, "api/", _controller, token);
                if (!response.IsSuccess) return null;

                var list = (List<MunicipioDTO>)response.Result;
                var listDto = JArray.FromObject(list.OrderBy(o => o.Nombre), Js);

                return listDto;
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// Almacena la Información de la Historia clínica de un Individuo
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [HttpPost, ActionName("GuardarProcesoJudicialAsync")]
        public async Task<Response> GuardarProcesoJudicialAsync(ProcesoJudicialDTO objData)
        {
            int idUsuario = 0;
            decimal funcionario = 0;

            if (!ModelState.IsValid) return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + "Datos incompletos!" };

            Response response = new Response
            {
                IsSuccess = true,
                Message = ""
            };
            ApiService apiService = new ApiService();
            try
            {
                urlApiJudicial = "https://localhost:7171/";


                System.Web.HttpContext context = System.Web.HttpContext.Current;
                ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                    funcionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                                   join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                                   where uf.ID_USUARIO == idUsuario
                                                   select f.CODFUNCIONARIO).FirstOrDefault());
                }

                decimal Id = 0;
                if (objData.ProcesoId == -1) objData.ProcesoId = 0;
                Id = objData.ProcesoId;

                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;


                if (Id > 0)
                {
                    response = await apiService.PutMicroServicioAsync<ProcesoJudicialDTO>(urlApiJudicial, "api", "/ProcesosJudiciales/ActualizarProcesoJudicial", objData, token);
                    if (!response.IsSuccess) return response;
                }
                else if (Id <= 0)
                {
                    objData.ProcesoId = 0;
                    response = await apiService.PostMicroServicioAsync<ProcesoJudicialDTO>(urlApiJudicial, "api", "/ProcesosJudiciales/GuardarProcesoJudicial", objData, token);
                    if (!response.IsSuccess) return response;

                    var result = (Response)response.Result;
                    int procesoJudicialId = 0;
                    int.TryParse(result.Result.ToString(), out procesoJudicialId);
                    objData.ProcesoId = procesoJudicialId;

                    DocumentoProcesoDTO documentoProcesoDTO = new DocumentoProcesoDTO
                    {
                        DocumentoBase64 = "actualizando idproceso al documento...",
                        DocumentoProcesolId = 0,
                        PlantillaDocumentalId = 21,
                        Identificador = "_"+ objData.Radicado,
                        ProcesoJudicialId = procesoJudicialId,
                        Version = 1,
                    };

                    response = await apiService.PostMicroServicioAsync<DocumentoProcesoDTO>(urlApiGerencial, "api", "/DocumentoProceso/PostActualizarDocumentoProceso", documentoProcesoDTO, token);
                    if (!response.IsSuccess) return response;
                }


                #region Sube documentos



                string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
                string _Ruta = _RutaBase + DateTime.Now.ToString("yyyyMM");

                DirectoryInfo dir = new DirectoryInfo(_Ruta);
                string _NomParc = @"ProcesoJudicial-" + idUsuario.ToString();

                FileInfo[] files = dir.GetFiles(_NomParc + "*", SearchOption.TopDirectoryOnly);
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var tipo = 0;
                        int.TryParse(file.Name.Substring(20, 1), out tipo);
                        byte[] byteArray = File.ReadAllBytes(file.FullName);

                        DocumentoAnexoDTO documentoAnexoDTO = new DocumentoAnexoDTO
                        {
                            Documento = Convert.ToBase64String(byteArray),
                            ProcesoJudicialId = objData.ProcesoId,
                            Tipo = tipo
                        };
                        response = await apiService.PostMicroServicioAsync<DocumentoAnexoDTO>(urlApiJudicial, "api", "/ProcesosJudiciales/AdicionarDocumentoAnexo", documentoAnexoDTO, token);
                        if (!response.IsSuccess)
                        {

                            response.Message = $"{response.Message} No se pudo almacenar el documento {file.Name}!";
                        }

                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ObtenerDocumentoAnexo")]
        public async Task<byte[]> ObtenerDocumentoAnexoAsync(int id, int tipo)
        {
            urlApiJudicial = "https://localhost:7171/";


            ApiService apiService = new ApiService();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            try
            {
                var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
                string token = _token.Value;
                string _controller = $"ProcesosJudiciales/GetDocumentoAnexo?procesoId={id}&tipo={tipo}";




                DocumentoAnexoDTO documentoA = new DocumentoAnexoDTO();


                Response response = await apiService.GetMicroServicioAsync<DocumentoAnexoDTO>(this.urlApiJudicial, "api/ProcesosJudiciales/", $"GetDocumentoAnexo?procesoId={id}&tipo={tipo}", token);
                if (!response.IsSuccess) return null;

                documentoA = (DocumentoAnexoDTO)response.Result;
                return documentoA.BytesDoc;

            }
            catch
            {
                return null;
            }
        }
    }
}
