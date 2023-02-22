namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTARIFAS_TOPES")]
    public partial class TBTARIFAS_TOPES
    {
        [Key]
        public decimal ID_TOPE { get; set; }

        public decimal? MINIMO { get; set; }

        public decimal? MAXIMO { get; set; }

        public decimal? TARIFA { get; set; }

        public long? TOPE { get; set; }
    }
}
