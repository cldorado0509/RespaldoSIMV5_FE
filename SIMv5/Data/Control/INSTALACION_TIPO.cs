namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INSTALACION_TIPO")]
    public partial class INSTALACION_TIPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_INS_TIPO { get; set; }

        public decimal? ID_TIPO { get; set; }

        public decimal? ID_INSTALACION { get; set; }

        public DateTime? D_REGISTRO { get; set; }
    }
}
