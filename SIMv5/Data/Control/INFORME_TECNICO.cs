namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INFORME_TECNICO")]
    public partial class INFORME_TECNICO
    {
        [Key]
        public decimal ID_INF { get; set; }

        [StringLength(1000)]
        public string ASUNTO { get; set; }

        [StringLength(1000)]
        public string OBSERVACION { get; set; }

        public decimal? ID_ESTADOINF { get; set; }

        public decimal? ID_VISITA { get; set; }

        public decimal? FUNCIONARIO { get; set; }

        [StringLength(1000)]
        public string URL { get; set; }

        public decimal? FUNCIONARIOACT { get; set; }

        [StringLength(1000)]
        public string URL2 { get; set; }

        public int? ID_RADICADO { get; set; }

        [StringLength(1)]
        public string TRAMITE_AVANZADO { get; set; }

        [StringLength(4000)]
        public string INDICES { get; set; }

        [StringLength(1)]
        public string DEVUELTO { get; set; }

        [ForeignKey("ID_ESTADOINF")]
        public virtual INFESTADO INFESTADO { get; set; }
    }
}
