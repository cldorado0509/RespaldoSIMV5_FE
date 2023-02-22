namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_PAG_REPARTO_NOGEO")]
    public partial class VW_PAG_REPARTO_NOGEO
    {
        [StringLength(245)]
        public string CODTRAMITETAREA { get; set; }

        [Key]
        [Column(Order = 0)]
        public decimal CODFUNCIONARIO { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string PROCESO { get; set; }

        [StringLength(500)]
        public string TAREA { get; set; }

        public DateTime? FECHAINICIOTRAMITE { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime FECHAINI { get; set; }

        public DateTime? FECHAFIN { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal CODTAREA { get; set; }

        public decimal? TIEMPO_TAREA { get; set; }

        [StringLength(9)]
        public string ESTADO { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal ORDEN { get; set; }

        [StringLength(1763)]
        public string ASUNTO { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime DFECHAINI { get; set; }

        public DateTime? DFECHAINICIOTRAMITE { get; set; }

        [StringLength(510)]
        public string DIRECCION { get; set; }

        [StringLength(510)]
        public string MUNICIPIO { get; set; }

        [StringLength(40)]
        public string STR_CODTRAMITE { get; set; }

        public decimal? R { get; set; }
    }
}
