using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIM.Data.Tramites
{
    [Table("TRAMITES.MAS_FIRMAS")]
    public class RADMASIVAFIRMAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public decimal ID_RADMASIVO { get; set; }
        public decimal FUNC_FIRMA { get; set; }
        public decimal ORDEN_FIRMA { get; set; }
        public DateTime D_FECHAFIRMA { get; set; }
        public string S_FIRMADO { get; set; }
    }
}