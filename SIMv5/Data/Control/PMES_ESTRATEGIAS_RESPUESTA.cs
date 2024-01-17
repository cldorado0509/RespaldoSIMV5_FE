namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_RESPUESTA")]
    public partial class PMES_ESTRATEGIAS_RESPUESTA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ID_PREGUNTA { get; set; }

        public decimal? N_RESPUESTA { get; set; }

        [StringLength(4000)]
        public string S_RESPUESTA { get; set; }

        public int? ID_ESTRATEGIA_TERCERO { get; set; }

        [ForeignKey("ID_ESTRATEGIA_TERCERO")]
        public virtual PMES_ESTRATEGIAS_TERCERO PMES_ESTRATEGIAS_TERCERO { get; set; }

        [ForeignKey("ID_PREGUNTA")]
        public virtual PMES_ESTRATEGIAS_PREGUNTA PMES_ESTRATEGIAS_PREGUNTA { get; set; }
    }
}
