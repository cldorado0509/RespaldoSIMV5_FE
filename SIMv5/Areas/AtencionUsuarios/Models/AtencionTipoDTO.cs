

namespace SIM.Areas.AtencionUsuarios.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class AtencionTipoDTO
    {
        [JsonProperty("idAtencion")]
        public decimal IdAtencion { get; set; }

        [JsonProperty("idTercero")]
        public decimal IdTercero { get; set; }

        [JsonProperty("idClaseAtencion")]
        public decimal IdClaseAtencion { get; set; }

        [JsonProperty("idFormaAtencion")]
        public decimal IdFormaAtencion { get; set; }

        [JsonProperty("idTipoAtencion")]
        public decimal IdTipoAtencion { get; set; }

        [JsonProperty("idTramite")]
        public decimal? IdTramite { get; set; }

        [JsonProperty("detalle")]
        public string Detalle { get; set; }

        [JsonProperty("fechaAtencion")]
        public DateTime FechaAtencion { get; set; }

        [JsonProperty("idUsuarioFuncionario")]
        public decimal IdUsuarioFuncionario { get; set; }

        [JsonProperty("idTipoCompromiso")]
        public decimal? IdTipoCompromiso { get; set; }

        [JsonProperty("fechaCompromiso")]
        public DateTime? FechaCompromiso { get; set; }

        [JsonProperty("estado")]
        public decimal? Estado { get; set; }

        [JsonProperty("codigoUsuario")]
        public decimal? CodigoUsuario { get; set; }

        [JsonProperty("codigoCompromiso")]
        public decimal? CodigoCompromiso { get; set; }

        [JsonProperty("codigoComponente")]
        public decimal? CodigoComponente { get; set; }

        [JsonProperty("radicado")]
        public string Radicado { get; set; }

        [JsonProperty("nroFicha")]
        public decimal? NroFicha { get; set; }

        [JsonProperty("salvoConducto")]
        public string SalvoConducto { get; set; }

        [JsonProperty("idTerceroRepresentado")]
        public decimal? IdTerceroRepresentado { get; set; }


        [JsonProperty("nombreTerceroRepresentado")]
        public string NombreTerceroRepresentado { get; set; }


        [JsonProperty("idInstalacionAtencion")]
        public decimal? IdInstalacionAtencion { get; set; }
    }
}