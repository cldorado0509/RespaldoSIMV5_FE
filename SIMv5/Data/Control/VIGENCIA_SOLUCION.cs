namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VIGENCIA_SOLUCION")]
    public partial class VIGENCIA_SOLUCION
    {
        [Key]
        public decimal ID_VIGENCIA_SOL { get; set; }

        public decimal ID_VIGENCIA { get; set; }

        [Required]
        [StringLength(20)]
        public string VALOR { get; set; }

        public decimal? ID_ESTADO { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual FRM_GENERICO_ESTADO FRM_GENERICO_ESTADO { get; set; }
    }
}
