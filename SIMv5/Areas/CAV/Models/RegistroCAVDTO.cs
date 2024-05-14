using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{

    /// <summary>
    /// RegistroCAVDTO
    /// </summary>
    public class RegistroCAVDTO
    {

        /// <summary>
        /// Identifica el Registro de Ingreso de un Individuo al CAV
        /// </summary>
        [JsonProperty("RegistroCAVId")]
        public decimal RegistroCAVId { get; set; }


        /// <summary>
        /// Identifica la Familia de la Especie
        /// </summary>
        [Required]
        [JsonProperty("FamiliaFaunaId")]
        public decimal FamiliaFaunaId { get; set; }

        /// <summary>
        /// Identifica la Especie
        /// </summary>
        [Required]
        [JsonProperty("EspecieFaunaId")]
        public decimal EspecieFaunaId { get; set; }

        /// <summary>
        /// Nombre Comun
        /// </summary>
        [JsonProperty("NombreComun")]
        public string NombreComun { get; set; } = string.Empty;


        /// <summary>
        /// Nombre Cientifico
        /// </summary>
        [JsonProperty("NombreCientifico")]
        public string NombreCientifico { get; set; } = string.Empty;

        /// <summary>
        /// Identifica el Tipo de Estado del Individuo
        /// </summary>
        [JsonProperty("TipoEstadoIndividuoId")]
        public decimal? TipoEstadoIndividuoId { get; set; }


        /// <summary>
        /// Tipo de Estado del Individuo
        /// </summary>
        [JsonProperty("TipoEstadoIndividuo")]
        public string TipoEstadoIndividuo { get; set; }


        /// <summary>
        /// Identifica el Tipo de Destino del Individuo
        /// </summary>
        [JsonProperty("TipoDestinoId")]
        public decimal? TipoDestinoId { get; set; }


        /// <summary>
        /// Identifica el tipo de edad del Individuo
        /// </summary>
        [JsonProperty("TipoEdadId")]
        public decimal? TipoEdadId { get; set; }


        /// <summary>
        /// Identifica el tipo de Marca del Individuo
        /// </summary>
        [JsonProperty("TipoMarcaId")]
        public decimal? TipoMarcaId { get; set; }


        /// <summary>
        /// Identifica la Autoridad
        /// </summary>
        [JsonProperty("AutoridadId")]
        public decimal? AutoridadId { get; set; }


        /// <summary>
        /// Identifica la Autoridad
        /// </summary>
        [JsonProperty("TipoEntregaId")]
        public decimal? TipoEntregaId { get; set; }


        /// <summary>
        /// Identifica la Autoridad
        /// </summary>
        [JsonProperty("TiempoId")]
        public decimal? TiempoId { get; set; }


        /// <summary>
        /// Identifica EL Tipo de Adquisición del Individuo
        /// </summary>
        [JsonProperty("TipoAdquisicionId")]
        public decimal? TipoAdquisicionId { get; set; }

        /// <summary>
        /// Número de Identificación del individuo
        /// </summary>
        [Required]
        [JsonProperty("NumeroIdentificacion")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de Ingreso del Individuo al CAV
        /// </summary>
        [JsonProperty("FechaLLegada")]
        public DateTime? FechaLLegada { get; set; }

        /// <summary>
        /// Recomendaciones relacionadas con el Individuo
        /// </summary>
        [JsonProperty("Recomendaciones")]
        [MaxLength(500, ErrorMessage = "NombreMaxLength")]
        public string Recomendaciones { get; set; }


        /// <summary>
        /// Recomendaciones relacionadas con el Individuo
        /// </summary>
        [JsonProperty("Sexo")]
        [MaxLength(10, ErrorMessage = "SexoMaxLength")]
        public string Sexo { get; set; }

        /// <summary>
        /// Recomendaciones relacionadas con el Individuo
        /// </summary>
        [JsonProperty("Activo")]
        [MaxLength(1, ErrorMessage = "ActivoMaxLength")]
        public string Activo { get; set; }

        /// <summary>
        /// Fecha de Salida del Individuo del CAV
        /// </summary>
        [JsonProperty("FechaSalida")]
        public DateTime? FechaSalida { get; set; }

        /// <summary>
        /// Observacion relacionadas con el Individuo
        /// </summary>
        [JsonProperty("Observacion")]
        [MaxLength(1000, ErrorMessage = "ObservacionMaxLength")]
        public string Observacion { get; set; }

        /// <summary>
        /// Foto del Individuo
        /// </summary>
        [JsonProperty("foto")]
        public byte[] Foto { get; set; }


        /// <summary>
        /// Identifica el Responsable
        /// </summary>
        [JsonProperty("CodigoResponsable")]
        public decimal? CodigoResponsable { get; set; }

        /// <summary>
        /// Identifica la Causa de Ingreso del Individuo al CAV
        /// </summary>
        [JsonProperty("causaIngresoId")]
        public decimal? CausaIngresoId { get; set; }

        /// <summary>
        /// Fecha de Destino del Individuo
        /// </summary>
        [JsonProperty("FechaDestino")]
        public DateTime? FechaDestino { get; set; }


        /// <summary>
        /// Observacion relacionadas con el Individuo
        /// </summary>
        [JsonProperty("ActoAdministrativo")]
        [MaxLength(50, ErrorMessage = "ActoAdminMaxLength")]
        public string ActoAdministrativo { get; set; }

        /// <summary>
        /// Código Registro
        /// </summary>
        [JsonProperty("CodigoRegistro")]
        public decimal? CodigoRegistro { get; set; }


        /// <summary>
        /// Peso del Individuo
        /// </summary>
        [JsonProperty("Peso")]
        public float? Peso { get; set; }

        /// <summary>
        /// Largo del cuerpo del Individuo
        /// </summary>
        [JsonProperty("LargoCuerpo")]
        public float? LargoCuerpo { get; set; }

        /// <summary>
        /// Largo de la cola del Individuo
        /// </summary>
        [JsonProperty("LargoCola")]
        public float? LargoCola { get; set; }

        /// <summary>
        /// Largo del Femur del Individuo
        /// </summary>
        [JsonProperty("LargoFemur")]
        public float? LargoFemur { get; set; }

        /// <summary>
        /// Largo del Húmero del Individuo
        /// </summary>
        [JsonProperty("LargoHumero")]
        public float? LargoHumero { get; set; }


        /// <summary>
        /// Tenedor del Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "TenedorMaxLength")]
        [JsonProperty("Tenedor")]
        public string Tenedor { get; set; }

        /// <summary>
        /// Identificacion del Tenedor del Individuo
        /// </summary>
        [MaxLength(50, ErrorMessage = "IdentificacionTenedorMaxLength")]
        [JsonProperty("IdentificacionTenedor")]
        public string IdentificacionTenedor { get; set; }


        /// <summary>
        /// Dirección del Tenedor del Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "DireccionMaxLength")]
        [JsonProperty("DireccionTenedor")]
        public string DireccionTenedor { get; set; }


        /// <summary>
        /// Otro Teléfono del Tenedor del Individuo
        /// </summary>
        [JsonProperty("OtroTelefonoTenedor")]
        public decimal? OtroTelefonoTenedor { get; set; }

        /// <summary>
        /// Dirección del Tenedor del Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "EmailMaxLength")]
        [JsonProperty("EMailTenedor")]
        public string EMailTenedor { get; set; }


        /// <summary>
        /// Radicado de la Empresa
        /// </summary>
        [MaxLength(20, ErrorMessage = "RadicadoEmpresaMaxLength")]
        [JsonProperty("RadicadoEmpresa")]
        public string RadicadoEmpresa { get; set; }


        /// <summary>
        /// Fecha de Entrega del Individuo
        /// </summary>
        [JsonProperty("FechaEntrega")]
        public DateTime? FechaEntrega { get; set; }



        /// <summary>
        /// Identifia el Departamento de la direccíón de procedencia
        /// </summary>
        [JsonProperty("CodigoDepartamento")]
        public decimal? CodigoDepartamento { get; set; }


        /// <summary>
        /// Identifia el Municipio de la direccíón de procedencia
        /// </summary>
        [JsonProperty("CodigoMunicipio")]
        public decimal? CodigoMunicipio { get; set; }


        /// <summary>
        /// Identifia la Vereda del Tenedor del Individuo
        /// </summary>
        [JsonProperty("CodigoVereda")]
        public decimal? CodigoVereda { get; set; }

        /// <summary>
        /// Codigo de la Entidad que realiza la remisión del individuo
        /// </summary>
        [JsonProperty("CodigoAutoridadRemision")]
        public decimal? CodigoAutoridadRemision { get; set; }

        /// <summary>
        /// Fecha de Remisión del Individuo
        /// </summary>
        [JsonProperty("FechaRemision")]
        public DateTime? FechaRemision { get; set; }


        /// <summary>
        /// Número del Salvaconducto
        /// </summary>
        [MaxLength(20, ErrorMessage = "NroSalvoconductoMaxLength")]
        [JsonProperty("NroSalvoconducto")]
        public string NroSalvoconducto { get; set; }


        /// <summary>
        /// Temperatura del Individuo
        /// </summary>
        [JsonProperty("Temperatura")]
        public decimal? Temperatura { get; set; }


        /// <summary>
        /// Enfermedades padecidas por el Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "EnfermedadesPadecidasMaxLength")]
        [JsonProperty("EnfermedadesPadecidas")]
        public string EnfermedadesPadecidas { get; set; }

        /// <summary>
        /// Tratamiento recibido por el Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "TratamientoRecibidoMaxLength")]
        [JsonProperty("TratamientoRecibido")]
        public string TratamientoRecibido { get; set; }

        /// <summary>
        /// Vacunas efectuadas al Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "VacunasEfectuadasMaxLength")]
        [JsonProperty("TacunasEfectuadas")]
        public string VacunasEfectuadas { get; set; }



        /// <summary>
        /// Cirugías efectuadas al Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "CirugiasEfectuadasMaxLength")]
        [JsonProperty("Cirujias")]
        public string Cirujias { get; set; }


        /// <summary>
        /// Código de Liberación
        /// </summary>
        [JsonProperty("CodigoLiberacion")]
        public decimal? CodigoLiberacion { get; set; }


        /// <summary>
        /// Fecha de Remisión del Individuo
        /// </summary>
        [JsonProperty("FechaRegistro")]
        public DateTime? FechaRegistro { get; set; } = DateTime.Now;


        /// <summary>
        /// Código CAV Anterior
        /// </summary>
        [JsonProperty("CodigoCAVAnterior")]
        public decimal? CodigoCAVAnterior { get; set; }


        /// <summary>
        /// Largo del Plastron del Individuo
        /// </summary>
        [JsonProperty("LargoPlastron")]
        public float? LargoPlastron { get; set; }

        /// <summary>
        /// Ancho del Plastron del Individuo
        /// </summary>
        [JsonProperty("AnchoPlastron")]
        public float? AnchoPlastron { get; set; }

        /// <summary>
        /// Código relacionado con la Entrega del Individuo
        /// </summary>
        [JsonProperty("CodigoEntrega")]
        public decimal? CodigoEntrega { get; set; }

        /// <summary>
        /// Teléfono del Tenedor del Individuo
        /// </summary>
        [JsonProperty("TelefonoTenedor")]
        public string TelefonoTenedor { get; set; }

        /// <summary>
        /// Nombres de los Padres
        /// </summary>
        [MaxLength(1000, ErrorMessage = "NombresPadresMaxLength")]
        [JsonProperty("NombrePadres")]
        public string NombrePadres { get; set; }

        /// <summary>
        /// Vereda
        /// </summary>
        [MaxLength(1000, ErrorMessage = "VeredaMaxLength")]
        [JsonProperty("Vereda")]
        public string Vereda { get; set; }


        /// <summary>
        /// Longitud Coordenada
        /// </summary>
        [JsonProperty("Longitud")]
        public float? Longitud { get; set; }


        /// <summary>
        /// Latitud Coordenada
        /// </summary>
        [JsonProperty("Latitud")]
        public float? Latitud { get; set; }

        /// <summary>
        /// Altitud Coordenada
        /// </summary>
        [JsonProperty("Altitud")]
        public float? Altitud { get; set; }

        /// <summary>
        /// Dirección de Procedencia del Individuo
        /// </summary>
        [MaxLength(200, ErrorMessage = "DireccionProcedenciaMaxLength")]
        [JsonProperty("DireccionProcedencia")]
        public string DireccionProcedencia { get; set; }

        /// <summary>
        /// Barrio de Procedencia del Individuo
        /// </summary>
        [MaxLength(200, ErrorMessage = "BarrioProcedenciaMaxLength")]
        [JsonProperty("BarrioProcedencia")]
        public string BarrioProcedencia { get; set; }

        /// <summary>
        /// Declaración
        /// </summary>
        [MaxLength(4000, ErrorMessage = "DeclaracionMaxLength")]
        [JsonProperty("Declaracion")]
        public string Declaracion { get; set; }

        /// <summary>
        /// Sensibilización
        /// </summary>
        [MaxLength(1, ErrorMessage = "SensibilizaciónMaxLength")]
        [JsonProperty("Sensibilización")]
        public string Sensibilización { get; set; }


        /// <summary>
        /// Funcionario Responsable
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioResponsableMaxLength")]
        [JsonProperty("FuncionarioResponsable")]
        public string FuncionarioResponsable { get; set; }


        /// <summary>
        /// Cédula Funcionario Responsable
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioResponsableMaxLength")]
        [JsonProperty("CedulaResponsable")]
        public string CedulaResponsable { get; set; }

        /// <summary>
        /// Funcionario que realiza el procedimiento
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioProcedimientobleMaxLength")]
        [JsonProperty("FuncionarioProcedimiento")]
        public string FuncionarioProcedimiento { get; set; }


        /// <summary>
        /// Cédula del Funcionario Procedimiento 
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioProcedimientoMaxLength")]
        [JsonProperty("CedulaProcedimiento")]
        public string CedulaProcedimiento { get; set; }

        /// <summary>
        /// Funcionario que realiza la constancia de registro CAV
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioConstanciaCAVMaxLength")]
        [JsonProperty("FuncionarioConstanciaCAV")]
        public string FuncionarioConstanciaCAV { get; set; }


        /// <summary>
        /// Cédula del Funcionario realiza la constancia de registro CAV
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioConstanciaCAVMaxLength")]
        [JsonProperty("CedulaFuncionarioConstanciaCAV")]
        public string CedulaFuncionarioConstanciaCAV { get; set; }

    }

}
