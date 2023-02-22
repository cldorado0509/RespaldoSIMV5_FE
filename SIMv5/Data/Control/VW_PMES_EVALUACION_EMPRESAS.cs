namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_PMES_EVALUACION_EMPRESAS")]
    public partial class VW_PMES_EVALUACION_EMPRESAS
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }

        [StringLength(510)]
        public string CM { get; set; }

        [StringLength(510)]
        public string S_VALOR_VIGENCIA { get; set; }

        [StringLength(500)]
        public string S_ASUNTO { get; set; }

        public decimal? ID_TERCERO { get; set; }

        public decimal? N_DOCUMENTO { get; set; }

        [StringLength(1000)]
        public string S_TERCERO { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_FUNCIONARIO { get; set; }

        public decimal? ID_USUARIO { get; set; }

        public int? ID_EET { get; set; }

        [StringLength(510)]
        public string S_RADICADO { get; set; }

        public DateTime? D_RADICADO { get; set; }

        public int? N_COD_MUNICIPIO { get; set; }

        [StringLength(1)]
        public string S_VERSION { get; set; }

        public int ID_EVALUACION_TIPO { get; set; }

        [StringLength(1)]
        public string S_COPIA { get; set; }
    }
}
