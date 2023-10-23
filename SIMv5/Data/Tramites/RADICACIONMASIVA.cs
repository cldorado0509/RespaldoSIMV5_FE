using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIM.Data.Tramites
{
    [Table("TRAMITES.MAS_RADICACIONMASIVA")]
    public class RADICACIONMASIVA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public string S_TEMA { get; set; }
        public DateTime D_FECHA { get; set; }
        public decimal FUNC_ELABORA { get; set; }
        public string S_RUTAEXCEL { get; set; }
        public string S_RUTAPLANTILLA { get; set; }
        public decimal CANTIDAD_FILAS { get; set; }
        public string S_VALIDADO { get; set; }
        public string S_REALIZADO { get; set; }
    }
}