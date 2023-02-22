namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DETALLE_REGLA")]
    public partial class DETALLE_REGLA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DETALLEREGLA { get; set; }

        public int CODTAREA { get; set; }

        public int CODTAREASIGUIENTE { get; set; }

        [Required]
        [StringLength(1)]
        public string S_PORDEFECTO { get; set; }

        [StringLength(20)]
        public string S_FORMULARIO { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }
    }
}
