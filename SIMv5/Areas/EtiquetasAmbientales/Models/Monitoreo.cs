namespace SIM.Areas.EtiquetasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Monitoreo
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("idEtiqueta")]
        public decimal? IdEtiqueta { get; set; }

        [JsonProperty("placa")]
        [Required]
        public string Placa { get; set; }

        [JsonProperty("kilometraje")]
        [Required]
        public int Kilometraje { get; set; }

        [JsonProperty("fechaMonitoreo")]
        [Required]
        public DateTime FechaMonitoreo { get; set; }

        [JsonProperty("coordenadaX")]
        public float CoordenadaX { get; set; }

        [JsonProperty("coordenadaY")]
        public float CoordenadaY { get; set; }

        [JsonProperty("foto1")]
        public string Foto1 { get; set; }

        [JsonProperty("foto2")]
        public string Foto2 { get; set; }

        [JsonProperty("foto3")]
        public string Foto3 { get; set; }

        [JsonProperty("empresa")]
        [Required]
        public string Empresa { get; set; }

        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        [JsonProperty("usuario")]
        [Required]
        public string Usuario { get; set; }

        [JsonProperty("estado")]
        [Required]
        public string Estado { get; set; }

        [JsonProperty("rPMRalenti")]
        [Required]
        public long RPMRalenti { get; set; }

        [JsonProperty("valorMonitoreo")]
        [Required]
        public float ValorMonitoreo { get; set; }

        public override string ToString()
        {
            return $"{this.Placa}";
        }

    }
}