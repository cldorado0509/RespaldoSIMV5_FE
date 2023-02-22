namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_OPCION_RESPUESTA")]
    public partial class ENC_OPCION_RESPUESTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_RESPUESTA { get; set; }

        public int ID_PREGUNTA { get; set; }

        [StringLength(1000)]
        public string S_VALOR { get; set; }

        [StringLength(100)]
        public string S_CODIGO { get; set; }

        public int? N_ORDEN { get; set; }

        [ForeignKey("ID_PREGUNTA")]
        public virtual ENC_PREGUNTA ENC_PREGUNTA { get; set; }
    }
}
