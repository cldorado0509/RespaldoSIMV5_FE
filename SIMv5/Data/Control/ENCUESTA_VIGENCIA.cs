namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENCUESTA_VIGENCIA")]
    public partial class ENCUESTA_VIGENCIA
    {
        [Key]
        public decimal ID_ENCUESTA_VIGENCIA { get; set; }

        public decimal? ID_VIGENCIA { get; set; }

        public int ID_ENCUESTA { get; set; }

        [ForeignKey("ID_ENCUESTA")]
        public virtual ENC_ENCUESTA ENC_ENCUESTA { get; set; }
    }
}
