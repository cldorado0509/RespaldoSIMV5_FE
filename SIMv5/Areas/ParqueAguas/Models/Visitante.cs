namespace SIM.Areas.ParqueAguas.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    public class Visitante
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tipoDocumentoId")]
        public int TipoDocumentoId { get; set; }

        [JsonProperty("numeroDocumento")]
        public long NumeroDocumento { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("nombres")]
        public string Nombres { get; set; }

        [JsonProperty("edad")]
        public int Edad { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("categoriaId")]
        public int CategoriaId { get; set; }

        [JsonProperty("responsable")]
        public bool Responsable { get; set; }
    }
}