namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TRAMITE_EXPEDIENTE_QUEJA")]
    public partial class TRAMITE_EXPEDIENTE_QUEJA
    {
        [Key]
        public decimal ID_TRAMITE_EXPEDIENTE { get; set; }

        public decimal CODTRAMITE { get; set; }

        public decimal CODIGO_QUEJA { get; set; }

        [ForeignKey("CODIGO_QUEJA")]
        public virtual TBQUEJA TBQUEJA { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
