namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    using DevExpress.Pdf;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraRichEdit;
    using DevExtreme.AspNet.Data.ResponseModel;
    using DevExtreme.AspNet.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SIM.Areas.ExpedienteAmbiental.Models;
    using SIM.Data;
    using SIM.Data.Tramites;
    using SIM.Models;
    using SIM.Services;
    using SIM.Utilidades;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using datosConsulta = Utilidades.datosConsulta;
    using Response = SIM.Models.Response;
    using TramiteDTO = Models.TramiteDTO;

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]", Name = "VitalApi")]
    public class VitalApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        private string idProcessDefault = SIM.Utilidades.Data.ObtenerValorParametro("IdProcesoRecibeTramitesGestionAutoridadVITAL").ToString();
        private string urlApiTerceros = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioTerceros").ToString();
        private string urlApiGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UrlApiGateWay").ToString());
        private string userApiVITALGateWay = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiVITALGateWay").ToString());
        private string userApiVITALGateWayS = SecurityLibraryNetFramework.ToolsSecurity.Decrypt(SIM.Utilidades.Data.ObtenerValorParametro("UserApiVITALGateWayS").ToString());
        private string urlApiSecurity = SIM.Utilidades.Data.ObtenerValorParametro("urlApiSecurity").ToString();
        private string descargarSolicitudesEnVITAL = SIM.Utilidades.Data.ObtenerValorParametro("DescargarSolitudesEnVITAL").ToString().ToUpper();
        private string urlApiMicoservicio = SIM.Utilidades.Data.ObtenerValorParametro("URLMicroSitioCAV").ToString();
        private string token = string.Empty;
        private ApiService apiService;
        private Response response = new Response();

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public VitalApiController()
        {
            if (Environment.MachineName == "SISTEMD26")
            {
                urlApiGateWay = "https://localhost:5000/";
            }
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            var _token = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type.EndsWith("Token")).FirstOrDefault();
            token = _token.Value;
            apiService = new ApiService();

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
        /// Retorna el Listado de los Tipos de Adquisición de FAUNA
        /// </summary>
        /// <param name="opciones"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetSolicitudesVITALenSIMAsync")]
        public async Task<LoadResult> GetSolicitudesVITALenSIMAsync(DataSourceLoadOptions opciones)
        {
            var opcionesSerialize = JsonConvert.SerializeObject(opciones);

            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            string descargarVital = descargarSolicitudesEnVITAL == "S" ? "true" : "false";

            this.urlApiMicoservicio = "https://localhost:7068/";

            response = await apiService.GetFilteredDataAsync(this.urlApiMicoservicio, "api/VITAL/SolicitudVITAL/", "ObtenerDescargarSolicitudesVITALPorEstado?Opciones=" + opcionesSerialize + "&atendidas=false&descargarEnVITAL=" + descargarVital, token);
            if (!response.IsSuccess) return null;
            dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Result.ToString());
            LoadResult ret = new LoadResult()
            {
                totalCount = result.totalCount,
                summary = result.summary,
                groupCount = result.groupCount
            };

            ret.data = result.data.ToObject<List<SolicitudVITALDTO>>();
            return ret;

        }

        /// <summary>
        /// Retorna el Listado de los documentos requeridos para un tipo de trámite ambiental
        /// </summary>
        /// <param name="filter">Criterio de Búsqueda dado por el usuario</param>
        /// <param name="numeroVITAL">Número VITAL</param>
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
        [HttpGet, ActionName("GetDocumentosRequeridosTramitesync")]
        public async Task<datosConsulta> GetDocumentosRequeridosTramitesync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, string numeroVITAL)
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

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return datosConsulta;

            string claseAtencionVITALId = numeroVITAL.Substring(0, 2);


            SIM.Models.Response responseS = await apiService.GetListAsync<DocumentosRequeridosTramitesDTO>(this.urlApiGateWay, "VITAL/DocumentosRequeridosTramites/", $"ObtenerDocumentosRequeridosTramitesPorClaseAtencion?claseAtencionVITALId={claseAtencionVITALId}", response.JwtToken);

            if (!responseS.IsSuccess) return datosConsulta;

            var list = (List<DocumentosRequeridosTramitesDTO>)responseS.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0 && skip == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }


        /// <summary>
        /// Retorna el Listado de los documentos aportados por el usuario en la plataforma de VITAL
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
        [HttpGet, ActionName("GetDocumentosAportadosync")]
        public async Task<datosConsulta> GetDocumentosAportadosync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, string radicadoVITAL)
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


            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return datosConsulta;

            SIM.Models.Response responseS = await apiService.GetListAsync<DocumentoAportadoDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerDocumentosSolicitudVital?RadicadoVITAL={radicadoVITAL}", response.JwtToken);

            if (!responseS.IsSuccess) return datosConsulta;
            var list = (List<DocumentoAportadoDTO>)responseS.Result;
            if (list == null || list.Count == 0) return datosConsulta;

            model = list.AsQueryable();
            modelData = model;

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta.numRegistros = modelFiltered.Count();
            if (take == 0 && skip == 0) datosConsulta.datos = modelFiltered.ToList();
            else datosConsulta.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return datosConsulta;
        }


        /// <summary>
        /// Permite abrir un Documento almacenado en VITAL
        /// </summary>
        /// <param name="nombreDocumento">Nombre del Documento</param>
        /// <param name="radicado">Radicado VITAL</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetDocumentoAsync")]
        public async Task<byte[]> GetDocumentoAsync(string nombreDocumento, string radicado)
        {
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return null;

            SIM.Models.Response responseS = await apiService.GetListAsync<Response>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerDocumentoVital/RadicadoVITAL/{radicado}/nombreDocumento/{nombreDocumento}", response.JwtToken);

            if (!responseS.IsSuccess) return null;
            var documento = (byte[])responseS.Result;
            if (documento == null) return null;

            return documento;


        }

        /// <summary>
        /// Retorna el listado de procesos activos
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetProcesos")]
        public async Task<JArray> GetProcesos()
        {
            dynamic model = null;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return null;

            SIM.Models.Response responseS = await apiService.GetListAsync<ProcesoDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerProcesos", response.JwtToken);

            if (!responseS.IsSuccess) return null;
            var list = (List<ProcesoDTO>)responseS.Result;
            if (list == null || list.Count == 0) return null;

            decimal _idProccessDefault = 0;

            decimal.TryParse(idProcessDefault, out _idProccessDefault);

            foreach (var item in list)
            {
                if (item.CODPROCESO == _idProccessDefault)
                {
                    item.S_DEFECTO = 1;
                }
                else
                {
                    item.S_DEFECTO = 0;
                }
            }

            model = list.AsQueryable().OrderByDescending(o => o.S_DEFECTO).ThenBy(o => o.NOMBRE);

            return JArray.FromObject(model, Js);


        }


        /// <summary>
        /// Retorna el listado de actividades de un proceso
        /// </summary>
        /// <param name="procesoId">Identifica el Proceso</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetActividades")]
        public async Task<JArray> GetActividades(int procesoId)
        {
            dynamic model = null;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return null;

            SIM.Models.Response responseS = await apiService.GetListAsync<TareaSIMDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerTareasProceso?procesoId={procesoId}", response.JwtToken);

            if (!responseS.IsSuccess) return null;

            var list = (List<TareaSIMDTO>)responseS.Result;

            if (list == null || list.Count == 0) return null;

            model = list.AsQueryable().OrderBy(o => o.Nombre);

            return JArray.FromObject(model, Js);


        }

        /// <summary>
        /// Retorna el listado de responsables de una actividad
        /// </summary>
        /// <param name="tareaId">Identifica la tarea</param>
        /// <returns></returns>
        [HttpGet, ActionName("GetResponsables")]
        public async Task<JArray> GetResponsables(string tareaId)
        {
            dynamic model = null;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);


            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return null;


            SIM.Models.Response responseS = await apiService.GetListAsync<ResponsableTareaDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerResponsablesTareaSIM?TareaId={tareaId}", response.JwtToken);

            if (!responseS.IsSuccess) return null;

            var list = (List<ResponsableTareaDTO>)responseS.Result;

            if (list == null || list.Count == 0) return null;

            model = list.AsQueryable().OrderBy(o => o.Funcionario);

            return JArray.FromObject(model, Js);


        }

        /// <summary>
        /// Avanza una solicitud de VITAL al SIM
        /// </summary>
        /// <param name="tramiteDTO">Información del Trámite</param>
        /// <returns></returns>
        [HttpPost, ActionName("AvanzarEnSIMAsync")]
        public async Task<object> AvanzarEnSIMAsync(TramiteDTO tramiteDTO)
        {
            SIM.Models.Response responseF = new SIM.Models.Response();
            try
            {
                SIM.Utilidades.Radicador radicador = new Radicador();

                if (radicador.SePuedeGenerarRadicado(DateTime.Now))
                {
                    if (string.IsNullOrEmpty(tramiteDTO.NumeroVitalPadre)) tramiteDTO.NumeroVitalPadre = "";

                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    decimal codFuncionario = -1;

                    int idUsuario = 0;
                    if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    }

                    ApiService apiService = new ApiService();

                    tramiteDTO.FechaIni = DateTime.Now;


                    AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
                    if (response.ExpiresIn == 0) return null;


                    SIM.Models.Response responseS = await apiService.GetListAsync<DocumentoAportadoDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerDocumentosSolicitudVital?RadicadoVITAL={tramiteDTO.RadicadoVITAL}", response.JwtToken);

                    string documentosAnexos = string.Empty;

                    if (responseS.IsSuccess)
                    {
                        var list = (List<DocumentoAportadoDTO>)responseS.Result;
                        foreach (var item in list)
                        {
                            documentosAnexos = $"{documentosAnexos}{item.Nombre}-";
                        }
                        if (!string.IsNullOrEmpty(documentosAnexos) && documentosAnexos.Length > 0)
                        {
                            documentosAnexos = documentosAnexos.Substring(0, documentosAnexos.Length - 2);
                        }
                    }


                    responseF = await apiService.PostAsync<TramiteDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", "IniciarTramiteAmbientalSIM", tramiteDTO, response.JwtToken);

                    if (!responseF.IsSuccess)
                    {
                        return responseF;
                    }

                    var datos = ((SIM.Models.OperationResponse)responseF.Result).Message.Split('-');

                    var tramiteSIMId = datos[0];


                    #region Genera la Comuniciación Oficial Recibida
                    var radicado = radicador.GenerarRadicado(10, idUsuario, DateTime.Now);
                    Radicado01Report radicado01Report = new Radicado01Report();
                    var labelRadicado = radicador.GenerarEtiquetaRadicado(radicado.IdRadicado, radicado01Report, "PNG");

                    #region Crear PDF desde HTML
                    MemoryStream _DocRad = new MemoryStream();
                    using (RichEditDocumentServer server = new RichEditDocumentServer())
                    {

                        TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                        string body = string.Empty;
                        MemoryStream _msPdf = new MemoryStream();

                        using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(@"~/Areas/ExpedienteAmbiental/Templates/TemplateCOR.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        if (body.Length > 0)
                        {
                            CultureInfo cultureInfo = new CultureInfo("es-CO");
                            body = body.Replace("[Fecha]", DateTime.Now.Date.ToString("dd 'de ' MMMM ' de' yyyy"));
                            body = body.Replace("[Para]", "ÁREA METROPOLITANA DEL VALLE DE ABURRÁ");
                            body = body.Replace("[Asunto]", "Solicitud de trámite en la plataforma VITAL");
                            body = body.Replace("[Contenido]", "Se aportan los siguientes documentos :" + documentosAnexos);

                            body = body.Replace("[Usuario]", datos[3]);
                            body = body.Replace("[TipoDocumento]", datos[1]);
                            body = body.Replace("[NumeroDocumento]", datos[2]);
                            body = body.Replace("[NroVITAL]", tramiteDTO.NumeroVital);

                        }

                        byte[] byteArray = Encoding.UTF8.GetBytes(body);
                        MemoryStream stream = new MemoryStream(byteArray);

                        server.LoadDocument(stream, DocumentFormat.OpenXml);

                        PdfExportOptions options = new PdfExportOptions();
                        options.DocumentOptions.Author = "SIM";
                        options.Compressed = false;
                        options.ImageQuality = PdfJpegImageQuality.Highest;

                        //Save to PDF  
                        server.ExportToPdf(_DocRad);

                        if (_DocRad.Length > 0)
                        {
                            _DocRad.Seek(0, SeekOrigin.Begin);
                            Bitmap _BmpRad = new Bitmap(labelRadicado);
                            if (_BmpRad != null)
                            {
                                using (PdfDocumentProcessor _Processor = new PdfDocumentProcessor())
                                {
                                    _Processor.CreateEmptyDocument();
                                    _Processor.AppendDocument(_DocRad);
                                    DevExpress.Pdf.PdfPage page = _Processor.Document.Pages[0];
                                    using (DevExpress.Pdf.PdfGraphics GrphRad = _Processor.CreateGraphics())
                                    {
                                        GrphRad.DrawImage(_BmpRad, new Rectangle(300, 30, 288, 72));
                                        GrphRad.AddToPageForeground(page, 72f, 72f);
                                    }
                                    _Processor.SaveDocument(_DocRad);
                                }
                            }

                            #region Adiciona la Comunicación Oficial Recibida a los documentos del Trámite Generado
                            var fecha = DateTime.Now;


                            var r = Utilidades.Archivos.SubirDocumentoServidorSinCifrar(_DocRad, ".pdf", tramiteSIMId, long.Parse(tramiteDTO.CodProceso.ToString()), fecha.Minute);

                            decimal codmaxDoc = 1;
                            var codtramite = decimal.Parse(tramiteSIMId);
                            var documentosTra = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite).ToList();
                            if (documentosTra != null && documentosTra.Count > 0) codmaxDoc = documentosTra.Max(f => f.CODDOCUMENTO) + 1;

                            TBTRAMITEDOCUMENTO tBTRAMITEDOCUMENTO = new TBTRAMITEDOCUMENTO
                            {
                                CODTRAMITE =  decimal.Parse(tramiteSIMId),
                                CODDOCUMENTO = codmaxDoc,
                                TIPODOCUMENTO = 2,
                                FECHACREACION = DateTime.Now,
                                CODFUNCIONARIO = 420,
                                NOMBRE = "00000001",
                                CIFRADO = "0",
                                RUTA = r,
                                MAPAARCHIVO = "M",
                                CODSERIE = 10
                            };

                            this.dbSIM.TBTRAMITEDOCUMENTO.Add(tBTRAMITEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            TBTRAMITE_DOC reltradoc = new TBTRAMITE_DOC();
                            reltradoc.CODTRAMITE = codtramite;
                            reltradoc.CODDOCUMENTO = codmaxDoc;
                            reltradoc.ID_DOCUMENTO = tBTRAMITEDOCUMENTO.ID_DOCUMENTO;
                            this.dbSIM.TBTRAMITE_DOC.Add(reltradoc);
                            this.dbSIM.SaveChanges();

                            var tramiteDocu = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite && f.CODDOCUMENTO == codmaxDoc).FirstOrDefault();

                            string IdIndiceRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicionRecibidaRadicado");
                            string IdIndiceAsunto = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidadAsunto");
                            string IdIndiceFechaRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaFechaRadicado");
                            string IdIndiceHoraRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaHoraRadicado");
                            string IdIndiceRemitente = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaRemitente");
                            string IdIndiceEmailSolicitante = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaEmailSolicitante");

                            TBINDICEDOCUMENTO tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRadicado),
                                CODDOCUMENTO =codmaxDoc,
                                VALOR = radicado.Radicado
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceAsunto),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = "Solicitud de trámite desde la plataforma VITAL"
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceFechaRadicado),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy")
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRemitente),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = "Remite XXX"
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            #endregion

                            #region Envía a VITAL el Documento 

                            SIM.Models.Response responseSolVital = await apiService.GetAsync<SolicitudVITALDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerSolicitudVITAL?idSolicitudVITAL={tramiteDTO.IdSolicitudVITAL}", response.JwtToken);
                            if (!responseSolVital.IsSuccess) return null;

                            var solicitudVITALDTO = (SolicitudVITALDTO)responseSolVital.Result;

                            DatoRadicacionDTO datoRadicacionDTO = new DatoRadicacionDTO
                            {
                                FechaRadicacion = Utilidades.Data.ObtenerFecha(DateTime.Now),
                                FechaSolicitud = Utilidades.Data.ObtenerFecha(solicitudVITALDTO.Fecha),
                                IdRadicacion = solicitudVITALDTO.RadicacionId,
                                NumeroFormulario = solicitudVITALDTO.FormularioId,
                                NumeroRadicadoAA = IdIndiceRadicado,
                                NumeroSilpa = solicitudVITALDTO.NumeroSILPA,
                            };

                            var responseV = await apiService.PostAsync<DatoRadicacionDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", "EnviarAVITALDatosRadicacion", datoRadicacionDTO, response.JwtToken);

                            #endregion

                            #region Envía correo electrónico al usuario de VITAL
                            var destinatario = datos[4];
                            SIM.Utilidades.Email.EnviarEmail("metropol@metropol.gov.co", destinatario, "", "", "Solicitud VITAL:" + solicitudVITALDTO.NumeroVITAL, "El Área Metropolitana del Valle de Aburrá recibió su solicitud desde la plataforma VITAL con número de VITAL : " + tramiteDTO.NumeroVital + ". Le informamos que damos inicio al proceso de atención de la misma con número de trámite AMVA: " + tramiteSIMId + " Se anexa la comunicación oficial recibida generada desde la solicitud hecha en VITAL y que fué radicada con el número: " + IdIndiceRadicado + " del " +  radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy"), "172.16.0.5", false, "", "", _DocRad, "ComunicacionOficialRecibida.pdf");
                            #endregion

                        }

                    }
                    #endregion

                    #endregion

                }
            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return responseF;
        }



        /// <summary>
        /// Se retira una solicitud de VITAL sin ser atendida en el SIM
        /// </summary>
        /// <param name="tramiteDTO">Información del Trámite</param>
        /// <returns></returns>
        [HttpPost, ActionName("DescartarEnSIM")]
        public async Task<object> DescartarEnSIM(TramiteDTO tramiteDTO)
        {
            Response resposeF = new Response
            {
                IsSuccess = false,
                Message = "",
                Result = null,
            };

            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                decimal codFuncionario = -1;
                int idUsuario = 0;

                if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                codFuncionario = Convert.ToInt32((from uf in dbSIM.USUARIO_FUNCIONARIO
                                                  join f in dbSIM.TBFUNCIONARIO on uf.CODFUNCIONARIO equals f.CODFUNCIONARIO
                                                  where uf.ID_USUARIO == idUsuario
                                                  select f.CODFUNCIONARIO).FirstOrDefault());

                var Funcionario = dbSIM.TBFUNCIONARIO.Where(f => f.CODFUNCIONARIO ==codFuncionario).FirstOrDefault();
                if (Funcionario == null) Funcionario = new SIM.Data.Tramites.TBFUNCIONARIO();


                ApiService apiService = new ApiService();

                AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
                if (response.ExpiresIn == 0) return null;

                tramiteDTO.Mensaje = $"{tramiteDTO.Mensaje} - Funcionario:  {Funcionario.NOMBRES} {Funcionario.APELLIDOS} - Fecha: {DateTime.Now.ToString("MMM-dd-yyyy HH:mm:ss")} ";



                SIM.Utilidades.Radicador radicador = new Radicador();

                resposeF = await apiService.PostAsync<TramiteDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", "DescartarTramiteAmbientalSIM", tramiteDTO, response.JwtToken);
                if (!resposeF.IsSuccess)
                {
                    return resposeF;
                }

                var datos = ((SIM.Models.OperationResponse)resposeF.Result).Message.Split('-');

                var tramiteSIMId = datos[0];
                decimal tramiteSim = 0;
                decimal.TryParse(tramiteSIMId, out tramiteSim);
                if (tramiteSim > 0)
                {
                    SIM.Models.Response responseS = await apiService.GetListAsync<DocumentoAportadoDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerDocumentosSolicitudVital?RadicadoVITAL={tramiteDTO.RadicadoVITAL}", response.JwtToken);
                    string documentosAnexos = string.Empty;
                    if (responseS.IsSuccess)
                    {
                        var list = (List<DocumentoAportadoDTO>)responseS.Result;
                        foreach (var item in list)
                        {
                            documentosAnexos = $"{documentosAnexos}{item.Nombre}<br/>";
                        }

                        if (!string.IsNullOrEmpty(documentosAnexos) && documentosAnexos.Length > 0)
                        {
                            documentosAnexos = documentosAnexos.Substring(0, documentosAnexos.Length - 2);
                        }
                    }

                    #region Genera la Comuniciación Oficial Recibida
                    var radicado = radicador.GenerarRadicado(10, idUsuario, DateTime.Now);
                    Radicado01Report radicado01Report = new Radicado01Report();
                    var labelRadicado = radicador.GenerarEtiquetaRadicado(radicado.IdRadicado, radicado01Report, "PNG");

                    #region Crear PDF desde HTML
                    MemoryStream _DocRad = new MemoryStream();
                    using (RichEditDocumentServer server = new RichEditDocumentServer())
                    {

                        TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                        string body = string.Empty;
                        MemoryStream _msPdf = new MemoryStream();

                        using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(@"~/Areas/ExpedienteAmbiental/Templates/TemplateCOR.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        if (body.Length > 0)
                        {
                            CultureInfo cultureInfo = new CultureInfo("es-CO");
                            body = body.Replace("[Fecha]", DateTime.Now.Date.ToString("dd 'de ' MMMM ' de' yyyy"));
                            body = body.Replace("[Para]", "ÁREA METROPOLITANA DEL VALLE DE ABURRÁ");
                            body = body.Replace("[Asunto]", "Solicitud de trámite en la plataforma VITAL");
                            body = body.Replace("[Contenido]", "Se aportan los siguientes documentos : <br/>" + documentosAnexos);

                            body = body.Replace("[Usuario]", datos[3]);
                            body = body.Replace("[TipoDocumento]", datos[1]);
                            body = body.Replace("[NumeroDocumento]", datos[2]);
                            body = body.Replace("[NroVITAL]", tramiteDTO.NumeroVital);
                        }

                        byte[] byteArray = Encoding.UTF8.GetBytes(body);
                        MemoryStream stream = new MemoryStream(byteArray);

                        server.LoadDocument(stream, DocumentFormat.OpenXml);

                        PdfExportOptions options = new PdfExportOptions();
                        options.DocumentOptions.Author = "SIM";
                        options.Compressed = false;
                        options.ImageQuality = PdfJpegImageQuality.Highest;

                        //Save to PDF  
                        server.ExportToPdf(_DocRad);

                        if (_DocRad.Length > 0)
                        {
                            _DocRad.Seek(0, SeekOrigin.Begin);
                            Bitmap _BmpRad = new Bitmap(labelRadicado);
                            if (_BmpRad != null)
                            {
                                using (PdfDocumentProcessor _Processor = new PdfDocumentProcessor())
                                {
                                    _Processor.CreateEmptyDocument();
                                    _Processor.AppendDocument(_DocRad);
                                    DevExpress.Pdf.PdfPage page = _Processor.Document.Pages[0];
                                    using (DevExpress.Pdf.PdfGraphics GrphRad = _Processor.CreateGraphics())
                                    {
                                        GrphRad.DrawImage(_BmpRad, new Rectangle(300, 30, 288, 72));
                                        GrphRad.AddToPageForeground(page, 72f, 72f);
                                    }
                                    _Processor.SaveDocument(_DocRad);
                                }
                            }

                            #region Adiciona la Comunicación Oficial Recibida a los documentos del Trámite Generado
                            var fecha = DateTime.Now;

                            string codProcesoTramitesNoAtendidos = Utilidades.Data.ObtenerValorParametro("IdProcesoSolicitudesRechazadasVITAL");

                            var r = Utilidades.Archivos.SubirDocumentoServidorSinCifrar(_DocRad, ".pdf", tramiteSIMId, long.Parse(codProcesoTramitesNoAtendidos), fecha.Minute);

                            decimal codmaxDoc = 1;
                            var codtramite = decimal.Parse(tramiteSIMId);
                            var documentosTra = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite).ToList();
                            if (documentosTra != null && documentosTra.Count > 0) codmaxDoc = documentosTra.Max(f => f.CODDOCUMENTO) + 1;

                            TBTRAMITEDOCUMENTO tBTRAMITEDOCUMENTO = new TBTRAMITEDOCUMENTO
                            {
                                CODTRAMITE =  decimal.Parse(tramiteSIMId),
                                CODDOCUMENTO = codmaxDoc,
                                TIPODOCUMENTO = 2,
                                FECHACREACION = DateTime.Now,
                                CODFUNCIONARIO = 420,
                                NOMBRE = "00000001",
                                CIFRADO = "0",
                                RUTA = r,
                                MAPAARCHIVO = "M",
                                CODSERIE = 10
                            };

                            this.dbSIM.TBTRAMITEDOCUMENTO.Add(tBTRAMITEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            TBTRAMITE_DOC reltradoc = new TBTRAMITE_DOC();
                            reltradoc.CODTRAMITE = codtramite;
                            reltradoc.CODDOCUMENTO = codmaxDoc;
                            reltradoc.ID_DOCUMENTO = tBTRAMITEDOCUMENTO.ID_DOCUMENTO;
                            this.dbSIM.TBTRAMITE_DOC.Add(reltradoc);
                            this.dbSIM.SaveChanges();

                            var tramiteDocu = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite && f.CODDOCUMENTO == codmaxDoc).FirstOrDefault();

                            string IdIndiceRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicionRecibidaRadicado");
                            string IdIndiceAsunto = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidadAsunto");
                            string IdIndiceFechaRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaFechaRadicado");
                            string IdIndiceHoraRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaHoraRadicado");
                            string IdIndiceRemitente = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaRemitente");
                            string IdIndiceEmailSolicitante = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaEmailSolicitante");

                            TBINDICEDOCUMENTO tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRadicado),
                                CODDOCUMENTO =codmaxDoc,
                                VALOR = radicado.Radicado
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceAsunto),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = "Solicitud de trámite desde la plataforma VITAL"
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceFechaRadicado),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy")
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRemitente),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR =  datos[3]
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            #endregion

                            #region Envía a VITAL el Documento 

                            SIM.Models.Response responseSolVital = await apiService.GetAsync<SolicitudVITALDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerSolicitudVITAL?idSolicitudVITAL={tramiteDTO.IdSolicitudVITAL}", response.JwtToken);
                            if (!responseSolVital.IsSuccess) return null;

                            var solicitudVITALDTO = (SolicitudVITALDTO)responseSolVital.Result;

                            DatoRadicacionDTO datoRadicacionDTO = new DatoRadicacionDTO
                            {
                                FechaRadicacion = Utilidades.Data.ObtenerFecha(DateTime.Now),
                                FechaSolicitud = Utilidades.Data.ObtenerFecha(solicitudVITALDTO.Fecha),
                                IdRadicacion = solicitudVITALDTO.RadicacionId,
                                NumeroFormulario = solicitudVITALDTO.FormularioId,
                                NumeroRadicadoAA = IdIndiceRadicado,
                                NumeroSilpa = solicitudVITALDTO.NumeroSILPA,
                            };

                            var responseV = await apiService.PostAsync<DatoRadicacionDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", "EnviarAVITALDatosRadicacion", datoRadicacionDTO, response.JwtToken);

                            #endregion
                            try
                            {
                                #region Envía correo electrónico al usuario de VITAL
                                string destinatario = datos[4];
                                SIM.Utilidades.Email.EnviarEmail("metropol@metropol.gov.co", destinatario, "", "", "Solicitud VITAL:" + solicitudVITALDTO.NumeroVITAL, "El Área Metropolitana del Valle de Aburrá recibió su solicitud desde la plataforma VITAL con número de VITAL : " + tramiteDTO.NumeroVital + ". Le informamos que damos inicio al proceso de atención de la misma con número de trámite AMVA: " + tramiteSIMId + " Se anexa la comunicación oficial recibida generada desde la solicitud hecha en VITAL y que fué radicada con el número: " + IdIndiceRadicado + " del " +  radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy"), "172.16.0.5", false, "", "", _DocRad, "ComunicacionOficialRecibida.pdf");
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Enviar Correo Usuario VITAL - " + tramiteDTO.NumeroVital + " Comunicación Oficial Recibidad con Radicado : " + datoRadicacionDTO.NumeroRadicadoAA + " Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(ex));
                            }
                        }
                    }
                    #endregion

                    #endregion

                    #region Genera la Comunicación Oficial Despachada
                    //try
                    //{
                    //    #region Envía correo electrónico al usuario de VITAL
                    //    SIM.Utilidades.Email.EnviarEmail("metropol@metropol.gov.co", "jorgeestradacorrea@gmail.com", "", "", "Solicitud VITAL:" +  tramiteDTO.NumeroVital, "El Área Metropolitana del Valle de Aburrá dando respuesta a su solicitud desde la plataforma VITAL : " + tramiteDTO.NumeroVital + ", le informa que " + tramiteDTO.Comentarios , "172.16.0.5", false, "", "", _DocRad, "ComunicacionOficialDespachada.pdf");
                    //    #endregion
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //    Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Enviar Correo Usuario VITAL - " + solicitudVITALDTO.NumeroVITAL + " ] : Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(ex));
                    //}

                    #endregion
                }



                resposeF.IsSuccess = true;
                resposeF.Message = "Transacción realizada satisfactoriamente!";

            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
        }

        /// <summary>
        /// Avanza una solicitud de VITAL al un Trámite ya existente en el SIM
        /// </summary>
        /// <param name="tramiteDTO">Información del Trámite</param>
        /// <returns></returns>
        [HttpPost, ActionName("AsignarTramiteSIMAsync")]
        public async Task<object> AsignarTramiteSIMAsync(TramiteDTO tramiteDTO)
        {
            Response resposeF = new Response();
            string radicadoCOR = "";
            try
            {
                SIM.Utilidades.Radicador radicador = new Radicador();


                if (radicador.SePuedeGenerarRadicado(DateTime.Now))
                {

                    var trmiteSIM = dbSIM.TBTRAMITE.Where(f => f.CODTRAMITE == tramiteDTO.CodTramite && f.ESTADO == 0).FirstOrDefault();

                    if (trmiteSIM == null) return new Response { IsSuccess = false, Result  = "Trámite no encontrado", Message = "El Trámite dado no existe en el SIM! " };

                    ApiService apiService = new ApiService();
                    tramiteDTO.FechaIni = DateTime.Now;

                    AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
                    if (response.ExpiresIn == 0) return null;

                    SIM.Models.Response responseS = await apiService.GetListAsync<DocumentoAportadoDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerDocumentosSolicitudVital?RadicadoVITAL={tramiteDTO.RadicadoVITAL}", response.JwtToken);

                    string documentosAnexos = string.Empty;

                    if (responseS.IsSuccess)
                    {
                        var list = (List<DocumentoAportadoDTO>)responseS.Result;
                        foreach (var item in list)
                        {
                            documentosAnexos = $"{documentosAnexos}{item.Nombre}<br/>";
                        }
                        if (!string.IsNullOrEmpty(documentosAnexos) && documentosAnexos.Length > 0)
                        {
                            documentosAnexos = documentosAnexos.Substring(0, documentosAnexos.Length - 2);
                        }
                    }

                    resposeF = await apiService.PostAsync<TramiteDTO>(urlApiGateWay, "VITAL/SolicitudVITAL/", "IniciarTramiteAmbientalSIM", tramiteDTO, response.JwtToken);

                    if (!resposeF.IsSuccess) return resposeF;

                    #region Genera la Comuniciación Oficial Recibida

                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    decimal codFuncionario = -1;

                    int idUsuario = 0;
                    if (((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        idUsuario = Convert.ToInt32(((ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                    }

                    var radicado = radicador.GenerarRadicado(10, idUsuario, DateTime.Now);
                    radicadoCOR = radicado.Radicado;
                    Radicado01Report radicado01Report = new Radicado01Report();
                    var labelRadicado = radicador.GenerarEtiquetaRadicado(radicado.IdRadicado, radicado01Report, "PNG");

                    var datos = ((SIM.Models.OperationResponse)resposeF.Result).Message.Split('-');

                    var tramiteSIMId = datos[0];

                    #region Crear PDF desde HTML
                    MemoryStream _DocRad = new MemoryStream();
                    using (RichEditDocumentServer server = new RichEditDocumentServer())
                    {

                        TextInfo texto = new CultureInfo("es-CO", false).TextInfo;
                        string body = string.Empty;
                        MemoryStream _msPdf = new MemoryStream();

                        using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(@"~/Areas/ExpedienteAmbiental/Templates/TemplateCOR.html")))
                        {
                            body = reader.ReadToEnd();
                        }

                        if (body.Length > 0)
                        {
                            CultureInfo cultureInfo = new CultureInfo("es-CO");
                            body = body.Replace("[Fecha]", DateTime.Now.Date.ToString("dd 'de ' MMMM ' de' yyyy"));
                            body = body.Replace("[Para]", "ÁREA METROPOLITANA DEL VALLE DE ABURRÁ");
                            body = body.Replace("[Asunto]", "Solicitud de trámite en la plataforma VITAL");
                            body = body.Replace("[Contenido]", "Se aportan los siguientes documentos : <br/>" + documentosAnexos);

                            body = body.Replace("[Usuario]", datos[3]);
                            body = body.Replace("[TipoDocumento]", datos[1]);
                            body = body.Replace("[NumeroDocumento]", datos[2]);
                            body = body.Replace("[NroVITAL]", tramiteDTO.NumeroVital);

                        }

                        byte[] byteArray = Encoding.UTF8.GetBytes(body);
                        MemoryStream stream = new MemoryStream(byteArray);

                        server.LoadDocument(stream, DocumentFormat.OpenXml);

                        PdfExportOptions options = new PdfExportOptions();
                        options.DocumentOptions.Author = "SIM";
                        options.Compressed = false;
                        options.ImageQuality = PdfJpegImageQuality.Highest;

                        //Save to PDF  
                        server.ExportToPdf(_DocRad);

                        if (_DocRad.Length > 0)
                        {
                            _DocRad.Seek(0, SeekOrigin.Begin);
                            Bitmap _BmpRad = new Bitmap(labelRadicado);
                            if (_BmpRad != null)
                            {
                                using (PdfDocumentProcessor _Processor = new PdfDocumentProcessor())
                                {
                                    _Processor.CreateEmptyDocument();
                                    _Processor.AppendDocument(_DocRad);
                                    DevExpress.Pdf.PdfPage page = _Processor.Document.Pages[0];
                                    using (DevExpress.Pdf.PdfGraphics GrphRad = _Processor.CreateGraphics())
                                    {
                                        GrphRad.DrawImage(_BmpRad, new Rectangle(300, 30, 288, 72));
                                        GrphRad.AddToPageForeground(page, 72f, 72f);
                                    }
                                    _Processor.SaveDocument(_DocRad);
                                }
                            }

                            #region Adiciona la Comunicación Oficial Recibida a los documentos del Trámite Generado
                            var fecha = DateTime.Now;


                            var r = Utilidades.Archivos.SubirDocumentoServidorSinCifrar(_DocRad, ".pdf", tramiteSIMId, long.Parse(tramiteDTO.CodProceso.ToString()), fecha.Minute);

                            decimal codmaxDoc = 1;
                            var codtramite = decimal.Parse(tramiteSIMId);
                            var documentosTra = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite).ToList();
                            if (documentosTra != null && documentosTra.Count > 0) codmaxDoc = documentosTra.Max(f => f.CODDOCUMENTO) + 1;

                            TBTRAMITEDOCUMENTO tBTRAMITEDOCUMENTO = new TBTRAMITEDOCUMENTO
                            {
                                CODTRAMITE =  decimal.Parse(tramiteSIMId),
                                CODDOCUMENTO = codmaxDoc,
                                TIPODOCUMENTO = 2,
                                FECHACREACION = DateTime.Now,
                                CODFUNCIONARIO = 420,
                                NOMBRE = "00000001",
                                CIFRADO = "0",
                                RUTA = r,
                                MAPAARCHIVO = "M",
                                CODSERIE = 10
                            };

                            this.dbSIM.TBTRAMITEDOCUMENTO.Add(tBTRAMITEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            TBTRAMITE_DOC reltradoc = new TBTRAMITE_DOC();
                            reltradoc.CODTRAMITE = codtramite;
                            reltradoc.CODDOCUMENTO = codmaxDoc;
                            reltradoc.ID_DOCUMENTO = tBTRAMITEDOCUMENTO.ID_DOCUMENTO;
                            this.dbSIM.TBTRAMITE_DOC.Add(reltradoc);
                            this.dbSIM.SaveChanges();

                            var tramiteDocu = this.dbSIM.TBTRAMITEDOCUMENTO.Where(f => f.CODTRAMITE == codtramite && f.CODDOCUMENTO == codmaxDoc).FirstOrDefault();

                            string IdIndiceRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicionRecibidaRadicado");
                            string IdIndiceAsunto = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidadAsunto");
                            string IdIndiceFechaRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaFechaRadicado");
                            string IdIndiceHoraRadicado = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaHoraRadicado");
                            string IdIndiceRemitente = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaRemitente");
                            string IdIndiceEmailSolicitante = Utilidades.Data.ObtenerValorParametro("IdIndiceComunicacionRecibidaEmailSolicitante");

                            TBINDICEDOCUMENTO tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRadicado),
                                CODDOCUMENTO =codmaxDoc,
                                VALOR = radicado.Radicado
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceAsunto),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = "Autodeclaración de Vertimientos de Tasas Retributivas"
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceFechaRadicado),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy")
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            tBINDICEDOCUMENTO = new TBINDICEDOCUMENTO
                            {
                                CODTRAMITE = decimal.Parse(tramiteSIMId),
                                CODINDICE = int.Parse(IdIndiceRemitente),
                                CODDOCUMENTO = codmaxDoc,
                                VALOR = "Remite XXX"
                            };

                            this.dbSIM.TBINDICEDOCUMENTO.Add(tBINDICEDOCUMENTO);
                            this.dbSIM.SaveChanges();

                            #endregion

                            #region Envía a VITAL el Documento 

                            SIM.Models.Response responseSolVital = await apiService.GetAsync<SolicitudVITALDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", $"ObtenerSolicitudVITAL?idSolicitudVITAL={tramiteDTO.IdSolicitudVITAL}", response.JwtToken);
                            if (!responseSolVital.IsSuccess) return null;

                            var solicitudVITALDTO = (SolicitudVITALDTO)responseSolVital.Result;

                            DatoRadicacionDTO datoRadicacionDTO = new DatoRadicacionDTO
                            {
                                FechaRadicacion = Utilidades.Data.ObtenerFecha(DateTime.Now),
                                FechaSolicitud = Utilidades.Data.ObtenerFecha(solicitudVITALDTO.Fecha),
                                IdRadicacion = solicitudVITALDTO.RadicacionId,
                                NumeroFormulario = solicitudVITALDTO.FormularioId,
                                NumeroRadicadoAA = IdIndiceRadicado,
                                NumeroSilpa = solicitudVITALDTO.NumeroSILPA,
                            };

                            var responseV = await apiService.PostAsync<DatoRadicacionDTO>(this.urlApiGateWay, "VITAL/SolicitudVITAL/", "EnviarAVITALDatosRadicacion", datoRadicacionDTO, response.JwtToken);

                            #endregion

                            #region Envía correo electrónico al usuario de VITAL
                            try
                            {
                                string destinatario = datos[4];
                                SIM.Utilidades.Email.EnviarEmail("metropol@metropol.gov.co", destinatario, "", "", "Solicitud VITAL:" + solicitudVITALDTO.NumeroVITAL, "El Área Metropolitana del Valle de Aburrá recibió su solicitud desde la plataforma VITAL con número de VITAL : " + tramiteDTO.NumeroVital + ". Le informamos que damos inicio al proceso de atención de la misma con número de trámite AMVA: " + tramiteSIMId + " Se anexa la comunicación oficial recibida generada desde la solicitud hecha en VITAL y que fué radicada con el número: " + IdIndiceRadicado + " del " +  radicado.Fecha.ToString("dd 'de ' MMMM ' de' yyyy"), "172.16.0.5", false, "", "", _DocRad, "ComunicacionOficialRecibida.pdf");
                            }
                            catch (Exception ex)
                            {
                                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Enviar Correo Usuario VITAL - " + tramiteDTO.NumeroVital + " Comunicación Oficial Recibidad con Radicado : " + datoRadicacionDTO.NumeroRadicadoAA + " Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(ex));
                            }
                            #endregion

                        }

                    }
                    #endregion



                    #endregion

                }

            }
            catch (Exception e)
            {
                Utilidades.Log.EscribirRegistro(HostingEnvironment.MapPath("~/LogErrores/" + DateTime.Today.ToString("yyyyMMdd") + ".txt"), "Atención Trámite VITAL - " + tramiteDTO.NumeroVital + " Comunicación Oficial Recibidad con Radicado : " + radicadoCOR + " Se presentó un error. Se pudo haber almacenado parcialmente la Información.\r\n" + Utilidades.LogErrores.ObtenerError(e));
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }

            return resposeF;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="codTramite"></param>
        /// <returns></returns>
        [HttpPost, ActionName("BuscarTramiteEnSIMAsync")]
        public async Task<object> BuscarTramiteEnSIMAsync(string codTramite)
        {
            Response resposeF = new Response();
            try
            {
                decimal codt = 0;
                decimal.TryParse(codTramite, out codt);

                SIM.Utilidades.Radicador radicador = new Radicador();

                var trmiteSIM = dbSIM.TBTRAMITE.Where(f => f.CODTRAMITE == codt && f.ESTADO == 0).FirstOrDefault();

                if (trmiteSIM == null) return new Response { IsSuccess = false, Result  = "Trámite no encontrado", Message = "El Trámite dado no existe en el SIM! " };

                var tareaActual = dbSIM.TBTRAMITETAREA.Where(f => f.CODTRAMITE == codt && f.ESTADO == 0).FirstOrDefault();
                if (tareaActual == null) return new Response { IsSuccess = false, Result  = "No existen tareas vigentes", Message = "El Trámite dado no posee tareas vigentes en el SIM! " };

                if (tareaActual.CODFUNCIONARIO == 1111111914)
                {
                    return new Response { IsSuccess = true, Message ="usuario externo" };
                }
                else
                {
                    return new Response { IsSuccess = true, Message ="usuario no externo" };
                }

            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Result  = "", Message = "Error Almacenando el registro : " + e.Message };
            }
        }





        /// <summary>
        /// Retorna el listado de las Causas de no Atencíon
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("GetCausasNoAtencion")]
        public async Task<JArray> GetCausasNoAtencion()
        {
            dynamic model = null;
            ApiService apiService = new ApiService();
            datosConsulta datosConsulta = new datosConsulta
            {
                datos = null,
                numRegistros = 0,
            };

            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            AuthenticationResponse response = await apiService.GetTokenMicroServiciosAsync(this.urlApiGateWay, "api/", "Account", new AuthenticationRequest { Password = this.userApiVITALGateWayS, UserName = this.userApiVITALGateWay });
            if (response.ExpiresIn == 0) return null;


            SIM.Models.Response responseS = await apiService.GetListAsync<CausaNoAtencionVITALDTO>(this.urlApiGateWay, "VITAL/CausaNoAtencionVITAL/", $"ObtenerCausasNoAtencion", response.JwtToken);


            if (!responseS.IsSuccess) return null;
            var list = (List<CausaNoAtencionVITALDTO>)responseS.Result;
            if (list == null || list.Count == 0) return null;

            list.Add(new CausaNoAtencionVITALDTO { CausaNoAtencionVITALId= 0, Nombre = "", Habilitado = "1" });

            model = list.AsQueryable().OrderBy(o => o.Nombre);

            return JArray.FromObject(model, Js);

        }
    }
}
