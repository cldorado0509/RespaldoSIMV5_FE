namespace SIM.Areas.ProcesosJudiciales.DTOs
{
    public class FichaComiteModel
    {
        public int IdPlantilla { get; set; }
        public string DocumentoBasee64 { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Despacho { get; set; } = string.Empty;
        public string MedioControl { get; set; } = string.Empty;
        public string Radicado { get; set; } = string.Empty;
        public string Instancia { get; set; } = string.Empty;
        public string Convocante { get; } = string.Empty;
        public string Convocado { get; set; } = string.Empty;
        public string FechaNotificacion { get; set; } = string.Empty;
        public string FichaAudicencia { get; set; } = string.Empty;
        public string RiesgoProfesional { get; set; } = string.Empty;
        public string Cuantia { get; set; } = string.Empty;
        public string PoliticaInstitucional { get; set; } = string.Empty;
        public string LlamaGarantia { get; set; } = string.Empty;
        public string Apoderado { get; set; } = string.Empty;
    }
}
