namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.PBCATCOL")]
    public partial class PBCATCOL
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string PBC_TNAM { get; set; }

        public decimal? PBC_TID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string PBC_OWNR { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string PBC_CNAM { get; set; }

        public decimal? PBC_CID { get; set; }

        [StringLength(254)]
        public string PBC_LABL { get; set; }

        public decimal? PBC_LPOS { get; set; }

        [StringLength(254)]
        public string PBC_HDR { get; set; }

        public decimal? PBC_HPOS { get; set; }

        public decimal? PBC_JTFY { get; set; }

        [StringLength(31)]
        public string PBC_MASK { get; set; }

        public decimal? PBC_CASE { get; set; }

        public decimal? PBC_HGHT { get; set; }

        public decimal? PBC_WDTH { get; set; }

        [StringLength(31)]
        public string PBC_PTRN { get; set; }

        [StringLength(1)]
        public string PBC_BMAP { get; set; }

        [StringLength(254)]
        public string PBC_INIT { get; set; }

        [StringLength(254)]
        public string PBC_CMNT { get; set; }

        [StringLength(31)]
        public string PBC_EDIT { get; set; }

        [StringLength(254)]
        public string PBC_TAG { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }
    }
}
