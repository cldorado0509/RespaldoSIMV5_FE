namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.FRM_RESIDUOS_FOTOGRAFIA")]
    public partial class FRM_RESIDUOS_FOTOGRAFIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_RECIDUOS { get; set; }

        public int? ID_ESTADO { get; set; }

        public int? ID_FOTOGRAFIA { get; set; }

        public int? ID_FORMULARIO { get; set; }

        [ForeignKey("ID_FORMULARIO")]
        public virtual FORMULARIO FORMULARIO { get; set; }

        [ForeignKey("ID_FOTOGRAFIA")]
        public virtual FOTOGRAFIA FOTOGRAFIA { get; set; }
    }
}
