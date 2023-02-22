

namespace SIM.Areas.QuejasAmbientales.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    public class FuncionarioDTO
    {
        [JsonProperty("codFuncionario")]
        public decimal CodFuncionario { get; set; }


        [JsonProperty("nombres")]
        public string Nombres { get; set; }


        [JsonProperty("codCargo")]
        public decimal CodCargo { get; set; }

        [JsonProperty("oficina")]
        public string Oficina { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("codGrupo")]
        public decimal? CodGrupo { get; set; }

        [JsonProperty("tipo")]
        public decimal? Tipo { get; set; }

        [JsonProperty("codGrupoT")]
        public decimal? CodGrupoT { get; set; }

        [JsonProperty("activo")]
        public string Activo { get; set; }

        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }

        [JsonProperty("cedula")]
        public string Cedula { get; set; }

        [JsonProperty("mostrarTareasGrupo")]
        public string MostrarTareasGrupo { get; set; }


        [JsonProperty("firmaDigital")]
        public string FirmaDigital { get; set; }

        [JsonProperty("usuarioFirma")]
        public string UsuarioFirma { get; set; }

        [JsonProperty("fechaFinFirma")]
        public DateTime? FechaFinFirma { get; set; }
    }
}