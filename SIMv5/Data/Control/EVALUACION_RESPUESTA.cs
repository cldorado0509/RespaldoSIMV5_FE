namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_RESPUESTA")]
    public partial class EVALUACION_RESPUESTA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ID_PREGUNTA { get; set; }

        public decimal? N_RESPUESTA { get; set; }

        [StringLength(4000)]
        public string S_RESPUESTA { get; set; }

        public int? ID_EVALUACION_ENCUESTA { get; set; }

        public int? ID_EVALUACION_ENCUESTA_TERCERO { get; set; }

        public decimal? N_RESPUESTA_AUX { get; set; }

        [StringLength(4000)]
        public string S_RESPUESTA_AUX { get; set; }


        [ForeignKey("ID_EVALUACION_ENCUESTA")]
        public virtual EVALUACION_ENCUESTA EVALUACION_ENCUESTA { get; set; }

        [ForeignKey("ID_EVALUACION_ENCUESTA_TERCERO")]
        public virtual EVALUACION_ENCUESTA_TERCERO EVALUACION_ENCUESTA_TERCERO { get; set; }

        [ForeignKey("ID_PREGUNTA")]
        public virtual EVALUACION_PREGUNTA EVALUACION_PREGUNTA { get; set; }
    }
}
