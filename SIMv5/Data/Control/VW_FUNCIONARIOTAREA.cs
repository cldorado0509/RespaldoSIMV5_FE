namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_FUNCIONARIOTAREA")]
    public partial class VW_FUNCIONARIOTAREA
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODFUNCIONARIO { get; set; }

        [StringLength(246)]
        public string NOMBRECOMPLETO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODTAREA { get; set; }

        [StringLength(40)]
        public string Str_CODFUNCIONARIO { get; set; }
    }
}
