namespace SIM.Areas.ExpedienteAmbiental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EstadoTramiteSIMDTO
    {
        /// <summary>
        /// Número de VITAL
        /// </summary>
        public string NumeroVital { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de Creación del Registro
        /// </summary>
        public string FechaCreacion { get; set; } = string.Empty;

        /// <summary>
        /// Texto descriptivo del Documento
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Número del Expediente o Trámite asignado por el SIM en el AMVA
        /// </summary>
        public string IdExpediente { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la Etapa: Audiencia Pública, Cesión Trámites y Derechos,Evaluación,Liquidación,Modificación de Licencia,Salvocinducto,Sancionatorio,Seguimiento,Solicitud,Tercero Interveniente
        /// </summary>
        public string EtaNombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del Acto
        /// </summary>
        public string DescripcionActo { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del Acto
        /// </summary>
        public string AddNombre { get; set; } = string.Empty;

        /// <summary>
        /// Identifica el Tipo de Documento (Ver Tabla en documentación de VITAL)
        /// </summary>
        public string TipoDocumento { get; set; } = string.Empty;
    }
}