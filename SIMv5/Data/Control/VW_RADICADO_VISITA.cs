namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_RADICADO_VISITA")]
    public partial class VW_RADICADO_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_RADICADOS_VISITA { get; set; }

        public int? ID_VISITA { get; set; }

        [StringLength(50)]
        public string S_RADICADO { get; set; }

        public int? ID_TIPO_RADICADO { get; set; }

        public DateTime? D_FECHA { get; set; }

        public DateTime? D_FIN { get; set; }
    }
}
