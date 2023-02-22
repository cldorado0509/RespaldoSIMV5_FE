namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DEPARTAMENTOS")]
    public partial class DEPARTAMENTOS
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID_DEPTO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string CODIGO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string NOMBRE { get; set; }
    }
}
