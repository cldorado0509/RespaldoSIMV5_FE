namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TERCERO_INSTALACION_PROYECTO")]
    public partial class TERCERO_INSTALACION_PROYECTO
    {
        [Key]
        [Column(Order = 0)]
        public int ID_INSTALACION { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ID_TERCERO { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODIGO_PROYECTO { get; set; }

        [ForeignKey("ID_INSTALACION, ID_TERCERO")]
        public virtual TERCERO_INSTALACION TERCERO_INSTALACION { get; set; }
    }
}
