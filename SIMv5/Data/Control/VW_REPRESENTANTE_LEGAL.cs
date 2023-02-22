namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_REPRESENTANTE_LEGAL")]
    public partial class VW_REPRESENTANTE_LEGAL
    {
        public long? DOCUMENTO { get; set; }

        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [Key]
        [Column(Order = 0)]
        public decimal ID_JURIDICO { get; set; }

        [StringLength(10)]
        public string FECHA_ACTUALIZACION { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        [StringLength(100)]
        public string mail { get; set; }
    }
}
