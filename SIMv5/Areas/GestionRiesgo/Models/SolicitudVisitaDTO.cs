namespace SIM.Areas.GestionRiesgo.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    public class SolicitudVisitaDTO
    {
        [JsonProperty("idSolicitudVisita")]
        public decimal IdSolicitudVisita { get; set; }

        [JsonProperty("solicitante")]
        [Required]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Solicitante { get; set; }

        [JsonProperty("numeroContacto")]
        [Required]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string NumeroContacto { get; set; }

        [JsonProperty("codigoTramite")]
        [Required]
        public int CodigoTramite { get; set; }

        [JsonProperty("fechaRadicadoSolicitud")]
        [Required]
        public string FechaRadicadoSolicitud { get; set; }

        [JsonProperty("radicadoSolicitud")]
        [Required]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string RadicadoSolicitud { get; set; }

        [JsonProperty("municipioId")]
        [Required]
        public int MunicipioId { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }

        [JsonProperty("barrioVereda")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string BarrioVereda { get; set; }

        [JsonProperty("direccion")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Direccion { get; set; }

        [JsonProperty("funcionarioId")]
        [Required]
        public int FuncionarioId { get; set; }

        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }

        [JsonProperty("latitud")]
        [Required]
        public float Latitud { get; set; }

        [JsonProperty("longitud")]
        [Required]
        public float Longitud { get; set; }

        [JsonProperty("tipoVisitaId")]
        [Required]
        public int TipoVisitaId { get; set; }

        [JsonProperty("tipoVisita")]
        public string TipoVisita { get; set; }

        [JsonProperty("origenId")]
        [Required]
        public int OrigenId { get; set; }

        [JsonProperty("origen")]
        public string Origen { get; set; }  

        [JsonProperty("sueloId")]
        [Required]
        public int SueloId { get; set; }

        [JsonProperty("suelo")]
        public string Suelo { get; set; }

        [JsonProperty("eventoId")]
        [Required]
        public int EventoId { get; set; }

        [JsonProperty("evento")]
        public string Evento { get; set; }

        [JsonProperty("nivelRiesgoId")]
        [Required]
        public int NivelRiesgoId { get; set; }

        [JsonProperty("nivelRiesgo")]
        public string NivelRiesgo { get; set; }

        [JsonProperty("calificacionRiesgo")]
        [Required]
        public float CalificacionRiesgo { get; set; }


        [JsonProperty("fechaVisita")]
        [Required]
        public DateTime FechaVisita { get; set; }

        [JsonProperty("esMonitoreo")]
        [Required]
        public bool EsMonitoreo { get; set; }

        [JsonProperty("fechaRadicadoSalida")]
        [Required]
        public string FechaRadicadoSalida { get; set; }

        [JsonProperty("radicadoSalida")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        [Required]
        public string RadicadoSalida { get; set; }

        [JsonProperty("numeroPersonasImpactadas")]
        [Required]
        public int NumeroPersonasImpactadas { get; set; }

        [JsonProperty("mes")]
        [Required]
        public int Mes { get; set; }

        [JsonProperty("destinatarios")]
        [MaxLength(4000, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Destinatarios { get; set; }

        [JsonProperty("quebradas")]
        [MaxLength(4000, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Quebradas { get; set; }

        [JsonProperty("viaPrincipal")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string ViaPrincipal { get; set; }

        [JsonProperty("numViaPrincipal")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string NumViaPrincipal { get; set; }

        [JsonProperty("letraViaPrincipal")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string LetraViaPrincipal { get; set; }

        [JsonProperty("sentidoViaPrincipal")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string SentidoViaPrincipal { get; set; }

        [JsonProperty("viaSecundaria")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string ViaSecundaria { get; set; }

        [JsonProperty("numViaSecundaria")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string NumViaSecundaria { get; set; }

        [JsonProperty("letraViaSecundaria")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string LetraViaSecundaria { get; set; }

        [JsonProperty("sentidoViaSecundaria")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string SentidoViaSecundaria { get; set; }

        [JsonProperty("placa")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Placa { get; set; }

        [JsonProperty("interior")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener {1} caracteres!")]
        public string Interior { get; set; }
    }
}