namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.PBCATTBL")]
    public partial class PBCATTBL
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string PBT_TNAM { get; set; }

        public decimal? PBT_TID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string PBT_OWNR { get; set; }

        public decimal? PBD_FHGT { get; set; }

        public decimal? PBD_FWGT { get; set; }

        [StringLength(1)]
        public string PBD_FITL { get; set; }

        [StringLength(1)]
        public string PBD_FUNL { get; set; }

        public decimal? PBD_FCHR { get; set; }

        public decimal? PBD_FPTC { get; set; }

        [StringLength(18)]
        public string PBD_FFCE { get; set; }

        public decimal? PBH_FHGT { get; set; }

        public decimal? PBH_FWGT { get; set; }

        [StringLength(1)]
        public string PBH_FITL { get; set; }

        [StringLength(1)]
        public string PBH_FUNL { get; set; }

        public decimal? PBH_FCHR { get; set; }

        public decimal? PBH_FPTC { get; set; }

        [StringLength(18)]
        public string PBH_FFCE { get; set; }

        public decimal? PBL_FHGT { get; set; }

        public decimal? PBL_FWGT { get; set; }

        [StringLength(1)]
        public string PBL_FITL { get; set; }

        [StringLength(1)]
        public string PBL_FUNL { get; set; }

        public decimal? PBL_FCHR { get; set; }

        public decimal? PBL_FPTC { get; set; }

        [StringLength(18)]
        public string PBL_FFCE { get; set; }

        [StringLength(254)]
        public string PBT_CMNT { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }
    }
}
