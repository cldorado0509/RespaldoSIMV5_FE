using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIM.Areas.CAV.Models
{
    /// <summary>
    /// Listado de Registros de Ingreso de individuos al CAV
    /// </summary>
    public class ListRegistroCAVDTO
    {
        /// <summary>
        /// Listado de Ingresos de individuos al CAV
        /// </summary>
        public List<RegistroIndCAV> registroCAVs { get; set; }
    }

    /// <summary>
    /// Registro de un Individuo al CAV
    /// </summary>
    public class RegistroIndCAV
    {
        /// <summary>
        /// Identifica el Registro de Ingreso de un Individuo al CAV
        /// </summary>
        [JsonProperty("registroCAVId")]
        public decimal RegistroCAVId { get; set; }

        /// <summary>
        /// Dirección de Procedencia del Individuo
        /// </summary>
        [MaxLength(200, ErrorMessage = "DireccionProcedenciaMaxLength")]
        [JsonProperty("direccionProcedencia")]
        public string DireccionProcedencia { get; set; }

        /// <summary>
        /// Barrio de Procedencia
        /// </summary>
        [JsonProperty("barrioProcedencia")]
        public string BarrioProcedencia { set; get; }

        /// <summary>
        /// Identifia el Municipio de la direccíón de procedencia
        /// </summary>
        [JsonProperty("codigoMunicipio")]
        public decimal? CodigoMunicipio { get; set; }


        /// <summary>
        /// Longitud Coordenada
        /// </summary>
        [JsonProperty("longitud")]
        public float? Longitud { get; set; }


        /// <summary>
        /// Latitud Coordenada
        /// </summary>
        [JsonProperty("latitud")]
        public float? Latitud { get; set; }

        /// <summary>
        /// Altitud Coordenada
        /// </summary>
        [JsonProperty("altitud")]
        public float? Altitud { get; set; }


        /// <summary>
        /// Declaración
        /// </summary>
        [MaxLength(4000, ErrorMessage = "DeclaracionMaxLength")]
        [JsonProperty("declaracion")]
        public string Declaracion { get; set; }


        /// <summary>
        /// Fecha de Entrega del Individuo
        /// </summary>
        [JsonProperty("fechaEntrega")]
        public DateTime? FechaEntrega { get; set; }


        /// <summary>
        /// Fecha de Entrega del Individuo
        /// </summary>
        [JsonProperty("horaEntrega")]
        public DateTime? HoraEntrega { get; set; }


        /// <summary>
        /// Nombre del Tenedor del Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "TenedorMaxLength")]
        [JsonProperty("tenedor")]
        public string Tenedor { get; set; }


        /// <summary>
        /// Identificacion del Tenedor del Individuo
        /// </summary>
        [MaxLength(50, ErrorMessage = "IdentificacionTenedorMaxLength")]
        [JsonProperty("identificacionTenedor")]
        public string IdentificacionTenedor { get; set; }


        /// <summary>
        /// Observacion relacionadas con el Individuo (declaracion
        /// </summary>
        [MaxLength(1000, ErrorMessage = "ObservacionMaxLength")]
        [JsonProperty("observacion")]
        public string Observacion { get; set; }



        /// <summary>
        /// Dirección del Tenedor del Individuo
        /// </summary>
        [MaxLength(500, ErrorMessage = "DireccionMaxLength")]
        [JsonProperty("direccionTenedor")]
        public string DireccionTenedor { get; set; }


        /// <summary>
        /// Teléfono del Tenedor del Individuo
        /// </summary>
        [JsonProperty("telefonoTenedor")]
        public string TelefonoTenedor { get; set; }


        /// <summary>
        /// Identifica la Especie
        /// </summary>
        [Required]
        [JsonProperty("especieFaunaId")]
        public decimal EspecieFaunaId { get; set; }

        /// <summary>
        /// Nombre Comun
        /// </summary>
        [JsonProperty("nombreComun")]
        public string NombreComun { get; set; } = string.Empty;


        /// <summary>
        /// Nombre Cientifico
        /// </summary>
        [JsonProperty("nombreCientifico")]
        public string NombreCientifico { get; set; } = string.Empty;

        /// <summary>
        /// Acto Administrativo
        /// </summary>
        [JsonProperty("actoAdministrativo")]
        [MaxLength(50, ErrorMessage = "ActoAdminMaxLength")]
        public string ActoAdministrativo { get; set; }

        /// <summary>
        /// Prefijo
        /// </summary>
        [JsonProperty("prefijo")]
        public string Prefijo { get; set; }

        /// <summary>
        /// Número de Identificación del individuo
        /// </summary>
        [JsonProperty("numeroIdentificacion")]
        public long NumeroIdentificacion { get; set; }

        /// <summary>
        /// Cantidas de Registros
        /// </summary>
        [JsonProperty("cantidad")]
        public int Cantidad { get; set; }


        /// <summary>
        /// Identifica el tiempo de cautiverio del Individuo
        /// </summary>
        [JsonProperty("tiempoId")]
        public decimal? TiempoId { get; set; }


        /// <summary>
        /// Codigo de la Entidad que realiza la remisión del individuo
        /// </summary>
        [JsonProperty("codigoAutoridadRemision")]
        public decimal? CodigoAutoridadRemision { get; set; }

        /// <summary>
        /// Funcionario Responsable
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioResponsableMaxLength")]
        [JsonProperty("funcionarioResponsable")]
        public string FuncionarioResponsable { get; set; }


        /// <summary>
        /// Funcionario Responsable
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioResponsableMaxLength")]
        [JsonProperty("cedulaResponsable")]
        public string CedulaResponsable { get; set; }


        /// <summary>
        /// Funcionario que realiza el procedimiento
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioProcedimientobleMaxLength")]
        [JsonProperty("funcionarioProcedimiento")]
        public string FuncionarioProcedimiento { get; set; }


        /// <summary>
        /// Cargo del Funcionario responsable
        /// </summary>
        [MaxLength(200, ErrorMessage = "CargoFuncionarioResponsableMaxLength")]
        [JsonProperty("cargoFuncionarioResponsable")]
        public string CargoFuncionarioResponsable { get; set; }


        /// <summary>
        /// Cédula del Funcionario Procedimiento 
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioProcedimientoMaxLength")]
        [JsonProperty("cedulaProcedimiento")]
        public string CedulaProcedimiento { get; set; }


        /// <summary>
        /// Funcionario que realiza la constancia de registro CAV
        /// </summary>
        [MaxLength(200, ErrorMessage = "FuncionarioConstanciaCAVMaxLength")]
        [JsonProperty("funcionarioConstanciaCAV")]
        public string FuncionarioConstanciaCAV { get; set; }


        /// <summary>
        /// Cédula del Funcionario realiza la constancia de registro CAV
        /// </summary>
        [MaxLength(20, ErrorMessage = "CedulaFuncionarioConstanciaCAVMaxLength")]
        [JsonProperty("cedulaFuncionarioConstanciaCAV")]
        public string CedulaFuncionarioConstanciaCAV { get; set; }

        /// <summary>
        /// Funcionario que realiza constancia de registro CAV
        /// </summary>
        [MaxLength(200, ErrorMessage = "cargoFuncionarioConstanciaCAVMaxLength")]
        [JsonProperty("cargoFuncionarioConstancia")]
        public string CargoFuncionarioConstanciaCAV { get; set; }

        /// <summary>
        /// Sensibilización
        /// </summary>
        [MaxLength(2, ErrorMessage = "SensibilizaciónMaxLength")]
        [JsonProperty("sensibilización")]
        public string Sensibilización { get; set; }

        /// <summary>
        /// Establece si el registro se encuentra en modo de edición
        /// </summary>
        [JsonProperty("isEdit")]
        public bool IsEdit { get; set; }

    }
}