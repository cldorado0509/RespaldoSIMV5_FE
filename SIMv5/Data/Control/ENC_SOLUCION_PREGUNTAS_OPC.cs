namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_SOLUCION_PREGUNTAS_OPC")]
    public partial class ENC_SOLUCION_PREGUNTAS_OPC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SOLUCION_PREGUNTAS_OPC { get; set; }

        public int? ID_SOLUCION_PREGUNTAS { get; set; }

        public int? ID_RESPUESTA { get; set; }
    }
}
