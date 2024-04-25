namespace SIM.Areas.Seguridad.Models
{
    public class OpcionesFiltroDTO
    {
        public string filter { get; set; }
        public string sort { get; set; }
        public string group { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public bool noFilterRecords { get; set; }
    }
}