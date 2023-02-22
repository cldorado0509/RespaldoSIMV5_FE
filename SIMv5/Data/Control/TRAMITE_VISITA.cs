namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.TRAMITE_VISITA")]
    public partial class TRAMITE_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TRAMITEVISITA { get; set; }

        public int ID_TRAMITE { get; set; }

        public int ID_VISITA { get; set; }

        [StringLength(50)]
        public string S_CODTRAMITE { get; set; }

        [ForeignKey("ID_VISITA")]
        public virtual VISITA VISITA { get; set; }
    }
}
