namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSUBSERIE_DOCUMENTAL")]
    public partial class TBSUBSERIE_DOCUMENTAL
    {
        [Key]
        public decimal CODSUBSERIE_DOCUMENTAL { get; set; }

        public decimal? CODSERIE_DOCUMENTAL { get; set; }

        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        [StringLength(1)]
        public string ELIMINADO { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }

        [ForeignKey("CODSERIE_DOCUMENTAL")]
        public virtual TBSERIE_DOCUMENTAL TBSERIE_DOCUMENTAL { get; set; }
    }
}
