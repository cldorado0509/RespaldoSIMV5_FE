namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_TIPOEVIDENCIA")]
    public partial class PMES_ESTRATEGIAS_TIPOEVIDENCIA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string S_TIPOEVIDENCIA { get; set; }

        public int? N_ORDEN { get; set; }
    }
}
