namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VISITAESTADO")]
    public partial class VISITAESTADO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_VISITAESTADO { get; set; }

        public int? ID_TERCERO { get; set; }

        public int ID_VISITA { get; set; }

        public int ID_ESTADOVISITA { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_ESTADOVISITA")]
        public virtual ESTADOVISITA ESTADOVISITA { get; set; }

        [ForeignKey("ID_VISITA")]
        public virtual VISITA VISITA { get; set; }
    }
}
