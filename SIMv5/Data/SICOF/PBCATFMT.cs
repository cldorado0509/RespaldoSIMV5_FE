namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.PBCATFMT")]
    public partial class PBCATFMT
    {
        [StringLength(30)]
        public string PBF_NAME { get; set; }

        [StringLength(254)]
        public string PBF_FRMT { get; set; }

        [Key]
        public decimal PBF_TYPE { get; set; }

        public decimal? PBF_CNTR { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }
    }
}
