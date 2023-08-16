namespace SIM.Areas.ExpedienteAmbiental.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DatoRadicacionDTO
    {
        /// <summary>
        /// Número SILPA retornado por VITAL al inicio de la solicitud del Usuario de Vital
        /// </summary>
        public string NumeroSilpa { get; set; } = string.Empty;

        /// <summary>
        /// Número del Formulario retornado por VITAL al inicio de la solicitud del Usuario de Vital
        /// </summary>
        public string NumeroFormulario { get; set; } = string.Empty;

        /// <summary>
        /// Radicado de VITAL retornado al inicio de la solicitud del Usuario de Vital
        /// </summary>
        public string IdRadicacion { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de la Solicitud (YYYY-MM-DDT00:00:00)
        /// </summary>
        public string FechaSolicitud { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de la Radicación del Documento(YYYY-MM-DDT00:00:00)
        /// </summary>
        public string FechaRadicacion { get; set; } = string.Empty;

        /// <summary>
        /// Número del Trámite generado el el Sistema SIM para esta solicitud del usuario
        /// </summary>
        public string NumeroRadicadoAA { get; set; } = string.Empty;

    }
}