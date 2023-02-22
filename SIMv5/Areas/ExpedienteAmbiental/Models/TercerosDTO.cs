using Newtonsoft.Json;
using System;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class TercerosDTO
    {
        [JsonProperty("idTercero")]

        public decimal IdTercero { get; set; }

        [JsonProperty("idTipoDocumento")]
        public decimal? IdTipoDocumento { get; set; }

        [JsonProperty("idDiviPola")]
        public decimal? IdDiviPola { get; set; }

        [JsonProperty("idActividadEconomica")]
        public decimal? IdActividadEconomica { get; set; }

        [JsonProperty("idEstado")]
        public decimal? IdEstado { get; set; }

        [JsonProperty("tipo")]
        public decimal? Tipo { get; set; }

        [JsonProperty("documento")]
        public decimal Documento { get; set; }

        [JsonProperty("telefono")]
        public decimal? Telefono { get; set; }

        [JsonProperty("fax")]
        public decimal? Fax { get; set; }

        [JsonProperty("formPago")]
        public decimal? FormPago { get; set; }

        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("web")]
        public string Web { get; set; }

        [JsonProperty("aAereo")]
        public string AAereo { get; set; }

        [JsonProperty("observacion")]
        public string Observacion { get; set; }

        [JsonProperty("usuario")]
        public decimal? Usuario { get; set; }

        [JsonProperty("registro")]
        public DateTime? Registro { get; set; }

        [JsonProperty("rSocial")]
        public string RSocial { get; set; }

        [JsonProperty("digitOver")]
        public decimal? DigitOver { get; set; }

        [JsonProperty("documentoN")]
        public decimal? DocumentoN { get; set; }

        [JsonProperty("celular")]
        public decimal? Celular { get; set; }

        [JsonProperty("autorizaNotifiElectro")]
        public string AutorizaNotifiElectro { get; set; }

        [JsonProperty("radicadoAutorizacion")]
        public string RadicadoAutorizacion { get; set; }

        [JsonProperty("prioritario")]
        public string Prioritario { get; set; }

    }
}