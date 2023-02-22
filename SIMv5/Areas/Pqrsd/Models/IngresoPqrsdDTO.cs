using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Pqrsd.Models
{
    public class IngresoPqrsdDTO
    {
        [JsonProperty("idPQRSD")]
        public string IdPQRSD { get; set; }
        [JsonProperty("tipoSolicitante")]
        public string TipoSolicitante { get; set; }
        [JsonProperty("tipoSolicitud")]
        public string TipoSolicitud { get; set; }
        [JsonProperty("medioRespuesta")]
        public string MedioRespuesta { get; set; }
        [JsonProperty("correoElectronico")]
        public string CorreoElectronico { get; set; }
        [JsonProperty("direccion")]
        public string Direccion { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("barrioComuna")]
        public string BarrioComuna { get; set; }
        [JsonProperty("telefono")]
        public string Telefono { get; set; }
        [JsonProperty("pais")]
        public string Pais { get; set; }
        [JsonProperty("departamento")]
        public string Departamento { get; set; }
        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }
        [JsonProperty("documento")]
        public string Documento { get; set; }
        [JsonProperty("asunto")]
        public string Asunto { get; set; }
        [JsonProperty("textoContenido")]
        public string TextoContenido { get; set; }
        [JsonProperty("recurso")]
        public string Recurso { get; set; }
        [JsonProperty("afectacion")]
        public string Afectacion { get; set; }
        [JsonProperty("proyecto")]
        public string Proyecto { get; set; }
        [JsonProperty("codTramite")]
        public string CodTramite { get; set; }
        [JsonProperty("emergenciaAmbiental")]
        public bool EmergenciaAmbiental { get; set; }
    }
}