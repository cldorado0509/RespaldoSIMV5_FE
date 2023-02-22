namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_SOLUCION_PREGUNTAS")]
    public partial class ENC_SOLUCION_PREGUNTAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SOLUCION_PREGUNTAS { get; set; }

        public int ID_SOLUCION { get; set; }

        public int ID_PREGUNTA { get; set; }

        public int? ID_VALOR { get; set; }

        [StringLength(2000)]
        public string S_VALOR { get; set; }

        public DateTime? D_VALOR { get; set; }

        public decimal? N_VALOR { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACION { get; set; }

        [StringLength(100)]
        public string X_VALOR { get; set; }

        [StringLength(100)]
        public string Y_VALOR { get; set; }

        [ForeignKey("ID_PREGUNTA")]
        public virtual ENC_PREGUNTA ENC_PREGUNTA { get; set; }

        [ForeignKey("ID_SOLUCION")]
        public virtual ENC_SOLUCION ENC_SOLUCION { get; set; }
    }
}
