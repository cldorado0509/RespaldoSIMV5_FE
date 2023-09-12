namespace SIM.Areas.GestionDocumental.Models
{
    public class ResponseFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string IdSolicitud { get; set; }
        /// <summary>
        /// Indica si el documento se pudo subir o no
        /// </summary>
        public bool SubidaExitosa { get; set; }
        /// <summary>
        /// Corresponde al mensaje motivo si no se pudo subir el documento
        /// </summary>
        public string MensajeError { get; set; }
        /// <summary>
        /// Mensaje en caso de que la subida fue exitosa
        /// </summary>
        public string MensajeExito { get; set; }
        /// <summary>
        /// identificador del archivo subido como anexo
        /// </summary>
        public string IdArchivo { get; set; } = string.Empty;
    }
}