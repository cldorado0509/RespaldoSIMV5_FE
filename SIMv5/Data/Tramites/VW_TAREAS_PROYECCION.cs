namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.VW_TAREAS_PROYECCION")]
    public partial class VW_TAREAS_PROYECCION
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODPROCESO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string PROCESO { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal CODTAREA { get; set; }

        [StringLength(500)]
        public string TAREA { get; set; }

        [StringLength(1767)]
        public string S_ASUNTO { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal ID_FUNCIONARIO { get; set; }

        public decimal? ID_USUARIO { get; set; }

        [StringLength(20)]
        public string S_FORMULARIO { get; set; }
    }
}
