namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.TEMP_DOC")]
    public partial class TEMP_DOC
    {
        [Key]
        [Column(Order = 0)]
        public decimal IDDOC { get; set; }

        public byte[] DOC { get; set; }

        public decimal? IDARBOL { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal TIPO { get; set; }
    }
}
