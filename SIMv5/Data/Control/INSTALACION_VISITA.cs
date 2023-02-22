namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INSTALACION_VISITA")]
    public partial class INSTALACION_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INSTALACIONVISITA { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_INSTALACION { get; set; }

        public int ID_VISITA { get; set; }

        [ForeignKey("ID_VISITA")]
        public virtual VISITA VISITA { get; set; }
    }
}
