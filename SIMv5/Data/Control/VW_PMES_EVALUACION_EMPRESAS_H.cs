namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_PMES_EVALUACION_EMPRESAS_H")]
    public partial class VW_PMES_EVALUACION_EMPRESAS_H
    {
        public int? CODTRAMITE { get; set; }

        [StringLength(20)]
        public string CM { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string S_VALOR_VIGENCIA { get; set; }

        [StringLength(500)]
        public string S_ASUNTO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        public long? N_DOCUMENTO { get; set; }

        [StringLength(1000)]
        public string S_TERCERO { get; set; }

        public decimal? ID_FUNCIONARIO { get; set; }

        public decimal? ID_USUARIO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_EET { get; set; }

        [StringLength(20)]
        public string S_RADICADO { get; set; }

        public DateTime? D_RADICADO { get; set; }

        public int? N_COD_MUNICIPIO { get; set; }
    }
}
