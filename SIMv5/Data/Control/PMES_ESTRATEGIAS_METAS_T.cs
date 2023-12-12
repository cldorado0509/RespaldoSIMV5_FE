namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_METAS_T")]
    public partial class PMES_ESTRATEGIAS_METAS_T
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_ESTRATEGIA_TERCERO { get; set; }

        public int ID_ESTRATEGIAS_METAS { get; set; }

        public decimal? N_VALOR { get; set; }
    }
}
