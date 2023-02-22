namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TRAMITE_EXPEDIENTE_AMBIENTAL")]
    public partial class TRAMITE_EXPEDIENTE_AMBIENTAL
    {
        [Key]
        public decimal ID_TRAMITE_EXPEDIENTE { get; set; }

        public decimal CODTRAMITE { get; set; }

        public decimal CODIGO_PROYECTO { get; set; }

        public decimal? CODIGO_SOLICITUD { get; set; }

        [ForeignKey("CODIGO_PROYECTO")]
        public virtual TBPROYECTO TBPROYECTO { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
