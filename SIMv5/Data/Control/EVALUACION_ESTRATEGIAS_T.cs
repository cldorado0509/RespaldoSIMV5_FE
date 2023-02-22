namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_ESTRATEGIAS_T")]
    public partial class EVALUACION_ESTRATEGIAS_T
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ID_EVALUACION_TERCERO { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        [StringLength(250)]
        public string S_OTRO { get; set; }

        [StringLength(250)]
        public string S_INDICADOR_MEDICION { get; set; }

        [StringLength(20)]
        public string S_UNIDADES_META { get; set; }

        public decimal N_VALOR_META { get; set; }

        public decimal? N_VALOR_META_ALCANZAR { get; set; }

        public decimal N_PRESUPUESTO { get; set; }

        [Required]
        [StringLength(2)]
        public string S_TIPO { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual EVALUACION_ESTRATEGIAS EVALUACION_ESTRATEGIAS { get; set; }
    }
}
