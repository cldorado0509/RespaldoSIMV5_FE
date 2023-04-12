namespace SIM.Areas.AdministracionDocumental.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class TablaRetencionDocumental
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }

        [JsonProperty("nombre", Required = Required.Always)]
        [Required]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("vigenteDesde")]
        public DateTime VigenteDesde { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre}";
        }
    }
}