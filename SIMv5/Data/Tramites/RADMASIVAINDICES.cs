using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIM.Data.Tramites
{
    [Table("TRAMITES.MAS_INDICES")]
    public class RADMASIVAINDICES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public decimal ID_RADMASIVO { get; set; }
        public decimal CODINDICE { get; set; }
        public string S_VALORASIGNADO { get; set; } = string.Empty;
        public string S_VALOREXCEL { get; set; } = string.Empty;
    }
}