namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBINDICETRAMITE")]
    public partial class TBINDICETRAMITE
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODINDICE { get; set; }

        [StringLength(510)]
        public string VALOR { get; set; }

        public DateTime? FECHAACTUALIZA { get; set; }

        [StringLength(254)]
        public string VALORID { get; set; }

        public DateTime? FECHAREGISTRO { get; set; }

        public DateTime? FECHALIMITE { get; set; }

        public virtual TBINDICEPROCESO TBINDICEPROCESO { get; set; }
    }
}
