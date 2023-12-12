namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_METAS")]
    public partial class PMES_ESTRATEGIAS_METAS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_ESTRATEGIAS_GRUPO { get; set; }

        [StringLength(512)]
        public string S_META { get; set; }

        [StringLength(512)]
        public string S_MEDICION { get; set; }

        public int? N_ORDEN { get; set; }
    }
}
