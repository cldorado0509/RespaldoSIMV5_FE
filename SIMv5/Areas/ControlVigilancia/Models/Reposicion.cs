namespace SIM.Areas.ControlVigilancia.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class Reposicion
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("codigoSolicitud")]
        [Required]
        public decimal CodigoSolicitud { get; set; }

        [JsonProperty("codigoActoAdministrativo")]
        [Required]
        public decimal CodigoActoAdministrativo { get; set; }

        [JsonProperty("talaSolicitada")]
        public decimal? TalaSolicitada { get; set; }

        [JsonProperty("talaAutorizada")]
        public decimal? TalaAutorizada { get; set; }

        [JsonProperty("dAPMen10Solicitada")]
      
        public decimal? DAPMen10Solicitada { get; set; }

        [JsonProperty("dAPMen10Autorizada")]
        public decimal? DAPMen10Autorizada { get; set; }

        [JsonProperty("volumenAutorizado")]
        public decimal? VolumenAutorizado { get; set; }

        [JsonProperty("transplanteSolicitado")]
        public decimal? TransplanteSolicitado { get; set; }

        [JsonProperty("transplanteAutorizado")]
        public decimal? TransplanteAutorizado { get; set; }

        [JsonProperty("podaSolicitada")]
        public decimal? PodaSolicitada { get; set; }

        [JsonProperty("podaAutorizada")]
        public decimal? PodaAutorizada { get; set; }

        [JsonProperty("conservacionSolicitada")]
        public decimal? ConservacionSolicitada { get; set; }

        [JsonProperty("conservacionAutorizada")]
        public decimal? ConservacionAutorizada { get; set; }

        [JsonProperty("reposicionPropuesta")]
        public decimal? ReposicionPropuesta { get; set; }

        [JsonProperty("reposicionMinimaObligatoria")]
        public decimal? ReposicionMinimaObligatoria { get; set; }

        [JsonProperty("reposicionAutorizada")]
        
        public decimal? ReposicionAutorizada { get; set; }
       
        [JsonProperty("tipoMedidaId")]
        public decimal? TipoMedidaId { get; set; }

        [JsonProperty("autorizado")]
        public decimal? Autorizado { get; set; }
        
        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        [JsonProperty("cm")]
        [Required]
        public string CM { get; set; }

        [JsonProperty("proyecto")]
        public string Proyecto { get; set; }

        [JsonProperty("asunto")]
        [Required]
        public string Asunto { get; set; }

        [JsonProperty("coordenadaX")]
        public decimal? CoordenadaX { get; set; }

        [JsonProperty("coordenadaY")]
        public decimal? CoordenadaY { get; set; }

        [JsonProperty("nroLeniosSolicitados")]
        public decimal? NroLeniosSolicitados { get; set; }

        [JsonProperty("nroLeniosAutorizados")]
        public decimal? NroLeniosAutorizados { get; set; }

        [JsonProperty("valoracionInventarioForestal")]
        public decimal? ValoracionInventarioForestal { get; set; }

        [JsonProperty("valoracionTala")]
        public decimal? ValoracionTala { get; set; }

        [JsonProperty("inversionReposicionMinima")]
        public decimal? InversionReposicionMinima { get; set; }

        [JsonProperty("inversionMedidasAdicionales")]
        public decimal? InversionMedidasAdicionales { get; set; }

        [JsonProperty("cantidadSiembraAdicional")]
        public decimal? CantidadSiembraAdicional { get; set; }

        [JsonProperty("inversionMedidaAdicionalSiembra")]
        public decimal? InversionMedidaAdicionalSiembra { get; set; }
        
        [JsonProperty("cantidadMantenimiento")]
        public decimal? CantidadMantenimiento { get; set; }

        [JsonProperty("inversionMedidaAdicionalMantenimiento")]
        public decimal? InversionMedidaAdicionalMantenimiento { get; set; }

        [JsonProperty("cantidadDestoconado")]
        public decimal? CantidadDestoconado { get; set; }

        [JsonProperty("inversionMedidaAdicionalDestoconado")]
        public decimal? InversionMedidaAdicionalDestoconado { get; set; }

        [JsonProperty("cantidadLevantamientoPiso")]
        public decimal? CantidadLevantamientoPiso { get; set; }

        [JsonProperty("inversionMedidaAdicionalLevantamientoPiso")]
        public decimal? InversionMedidaAdicionalLevantamientoPiso { get; set; }

        [JsonProperty("pagoFondoVerdeMetropolitano")]
        public decimal? PagoFondoVerdeMetropolitano { get; set; }

        [JsonProperty("esTramiteNuevo")]
        public string EsTramiteNuevo { get; set; }

        [JsonProperty("nombreProyecto")]
        public string NombreProyecto { get; set; }

        [JsonProperty("codigoTramite")]
        public string CodigoTramite { get; set; }

        [JsonProperty("codigoDocumento")]
        public string CodigoDocumento { get; set; }

    }
}