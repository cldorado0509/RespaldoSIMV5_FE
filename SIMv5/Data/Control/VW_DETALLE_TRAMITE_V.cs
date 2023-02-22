namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_DETALLE_TRAMITE_V")]
    public partial class VW_DETALLE_TRAMITE_V
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_VISITA { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TRAMITE { get; set; }

        [StringLength(500)]
        public string ASUNTO { get; set; }

        public DateTime? FECHAINI { get; set; }

        [StringLength(510)]
        public string DIRECCION { get; set; }

        [StringLength(510)]
        public string MUNICIPIO { get; set; }
    }
}
