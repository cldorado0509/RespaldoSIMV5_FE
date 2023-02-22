namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.VW_PROGRAMACIONES_PENDIENTES")]
    public partial class VW_PROGRAMACIONES_PENDIENTES
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODPROCESO { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODTAREA { get; set; }

        [StringLength(510)]
        public string CM { get; set; }

        [StringLength(1767)]
        public string S_ASUNTO { get; set; }

        public decimal? ID_TERCERO { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal ID_FUNCIONARIO { get; set; }

        public decimal? ID_USUARIO { get; set; }

        public int? N_COD_MUNICIPIO { get; set; }

        [StringLength(510)]
        public string S_MUNICIPIO { get; set; }

        [StringLength(510)]
        public string S_DIRECCION { get; set; }
    }
}
