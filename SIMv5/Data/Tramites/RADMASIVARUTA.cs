using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIM.Data.Tramites
{
    [Table("TRAMITES.MAS_RUTAMASIVO")]
    public class RADMASIVARUTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public decimal ID_RADMASIVO { get; set; }
        public decimal CODFUNCIONARIO { get; set; }
        public DateTime FECHA_RUTA { get; set; }
        public string S_COMENTARIO { get; set; }
    }
}