namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_TP")]
    public partial class PMES_ESTRATEGIAS_TP
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_ESTRATEGIA_TERCERO { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        [StringLength(250)]
        public string S_OTRO { get; set; }

        [StringLength(250)]
        public string S_PUBLICO_OBJETIVO { get; set; }

        [Required]
        [StringLength(20)]
        public string S_TIPOEVIDENCIA { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual PMES_ESTRATEGIAS PMES_ESTRATEGIAS { get; set; }

        [ForeignKey("ID_ESTRATEGIA_TERCERO")]
        public virtual PMES_ESTRATEGIAS_TERCERO PMES_ESTRATEGIAS_TERCERO { get; set; }
    }
}
