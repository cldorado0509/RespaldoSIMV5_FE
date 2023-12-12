namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_ACTIVIDADES")]
    public partial class PMES_ESTRATEGIAS_ACTIVIDADES
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int ID_ESTRATEGIA_TP { get; set; }

        [Required]
        [StringLength(250)]
        public string S_ACTIVIDAD { get; set; }

        [Required]
        public int ID_PERIODICIDAD { get; set; }
    }
}
