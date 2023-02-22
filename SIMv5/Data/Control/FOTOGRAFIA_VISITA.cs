namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FOTOGRAFIA_VISITA")]
    public partial class FOTOGRAFIA_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FOTOGRAFIAVISITA { get; set; }

        public int ID_FOTOGRAFIA { get; set; }

        public int ID_VISITA { get; set; }

        [ForeignKey("ID_FOTOGRAFIA")]
        public virtual FOTOGRAFIA FOTOGRAFIA { get; set; }

        [ForeignKey("ID_VISITA")]
        public virtual VISITA VISITA { get; set; }
    }
}
