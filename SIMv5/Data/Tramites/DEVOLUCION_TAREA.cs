namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DEVOLUCION_TAREA")]
    public partial class DEVOLUCION_TAREA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDDEVOLUCION { get; set; }

        public decimal CODTRAMITE { get; set; }

        public decimal CODTAREA { get; set; }

        public decimal CODTAREA_DEV { get; set; }

        public decimal ORDEN { get; set; }

        public decimal CODFUCNIONARIO { get; set; }

        public decimal? ID_MOTIVODEV { get; set; }
    }
}
