namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DEVOLUCION_TAREAMOTIVO")]
    public partial class DEVOLUCION_TAREAMOTIVO
    {
        [Key]
        public decimal ID_MOTIVO { get; set; }

        [StringLength(200)]
        public string S_MOTIVO { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }
    }
}
