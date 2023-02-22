namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.TERCERO_VISITA")]
    public partial class TERCERO_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCEROVISITA { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_VISITA { get; set; }

        public int ID_ROL { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_ROL")]
        public virtual ROL_VISITA ROL_VISITA { get; set; }

        [ForeignKey("ID_VISITA")]
        public virtual VISITA VISITA { get; set; }
    }
}
