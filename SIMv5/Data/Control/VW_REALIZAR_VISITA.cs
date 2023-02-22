namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_REALIZAR_VISITA")]
    public partial class VW_REALIZAR_VISITA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDVISITA { get; set; }

        [StringLength(400)]
        public string ASUNTO { get; set; }

        [StringLength(4000)]
        public string OBSEVACION { get; set; }

        public int? IDTERCERO { get; set; }

        [StringLength(1000)]
        public string NOMBRETERCERO { get; set; }

        public int? IDINSTALACION { get; set; }

        [StringLength(100)]
        public string INSTALACION { get; set; }
    }
}
