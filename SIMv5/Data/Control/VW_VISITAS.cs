namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_VISITAS")]
    public partial class VW_VISITAS
    {
        [StringLength(10)]
        public string D_ASIGNACIONVISITA { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_VISITA { get; set; }

        [StringLength(400)]
        public string S_ASUNTO { get; set; }

        public DateTime? D_ASIGNACION { get; set; }

        public DateTime? D_ACEPTACION { get; set; }

        public DateTime? D_INICIAL { get; set; }

        public DateTime? D_INICIOREV { get; set; }

        public DateTime? D_FINAL { get; set; }

        [StringLength(4000)]
        public string S_OBSERVACION { get; set; }

        public int? ID_ESTADOVISITA { get; set; }

        [StringLength(200)]
        public string ESTADO_VISITA { get; set; }

        [StringLength(50)]
        public string RADICADO_VISITA { get; set; }

        public int? ID_TIPOVISITA { get; set; }

        [StringLength(200)]
        public string S_NOMBRE_TIPOVISITA { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        [StringLength(1000)]
        public string TRAMITES { get; set; }

        [StringLength(1000)]
        public string COPIAS { get; set; }

        [StringLength(3000)]
        public string NOMBRECOPIAS { get; set; }

        [StringLength(1000)]
        public string RESPONSABLE { get; set; }

        [StringLength(3000)]
        public string NOMBRERESPONSABLE { get; set; }

        public decimal? ID_INFORME { get; set; }

        [StringLength(40)]
        public string STR_ID_VISITA { get; set; }

        public decimal? R { get; set; }
    }
}
