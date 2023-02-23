namespace SIM.Areas.ExpedienteAmbiental.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SIM.Areas.ExpedienteAmbiental.Clases;
    using SIM.Areas.ExpedienteAmbiental.Models;
    using SIM.Areas.ExpedienteAmbiental.Models.DTO;
    using SIM.Data;
    using SIM.Models;
    using SIM.Services;

    [Route("api/[controller]", Name = "VitalApi")]
    public class VitalApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        //private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        private string urlApiExpedienteAmbiental = "https://amaplicacion02/vital/";
        private string urlApiTerceros = " https://sim.metropol.gov.co/tercerosp/";

        private string urlApiSecurity = "https://amaplicacion02/seguridad/";
        //private string urlApiExpedienteAmbiental = " https://localhost:7012/";

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Retorna el Listado de los Expedientes Ambientales
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
        [HttpGet, ActionName("GetSolicitudesVITALenSIMAsync")]
        public async Task<datosConsulta> GetSolicitudesVITALenSIMAsync(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

            response = await apiService.GetListAsync<SolicitudVITALDTO>(urlApiExpedienteAmbiental, "api/", "SolicitudVITAL/ObtenerSolicitudesVITALPorEstado?atendidas=false", tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<SolicitudVITALDTO>)response.Result;
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
        /// Retorna el Listado de los documentos requeridos para un tipo de trímite ammbiental
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;


            string claseAtencionVITALId = numeroVITAL.Substring(0, 2);

            response = await apiService.GetListAsync<DocumentosRequeridosTramitesDTO>(urlApiExpedienteAmbiental, "api/", "DocumentosRequeridosTramites/ObtenerDocumentosRequeridosTramitesPorClaseAtencion?claseAtencionVITALId=" + claseAtencionVITALId, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<DocumentosRequeridosTramitesDTO>)response.Result;
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return datosConsulta;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return datosConsulta;

                       
            response = await apiService.GetListAsync<DocumentoAportadoDTO>(urlApiExpedienteAmbiental, "api/", "SolicitudVITAL/ObtenerDocumentosSolicitudVital?RadicadoVITAL=" + radicadoVITAL, tokenG.Token);
            if (!response.IsSuccess) return datosConsulta;
            var list = (List<DocumentoAportadoDTO>)response.Result;
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

            Response response = await apiService.GetTokenAsync(urlApiSecurity, "api/", "Users/CreateToken", new TokenRequest { Password = "Admin123", Username = "expediente.ambiental@metropol.gov.co", RememberMe = true });
            if (!response.IsSuccess) return null;
            var tokenG = (TokenResponse)response.Result;
            if (tokenG == null) return null;


            response = await apiService.GetListAsync<Response>(urlApiExpedienteAmbiental, "api/", "SolicitudVITAL/ObtenerDocumentoVital?RadicadoVITAL=" + radicado + "&NombreArchivo=" + nombreDocumento, tokenG.Token);
            if (!response.IsSuccess) return null;
            var documento = (byte[])response.Result;
            if (documento == null) return null;

            return documento;
         
            
        }



    }
}
