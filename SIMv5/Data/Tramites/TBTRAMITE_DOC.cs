namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTRAMITE_DOC")]
    public partial class TBTRAMITE_DOC
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }
        public decimal ID_DOCUMENTO { get; set; }
    }
}
