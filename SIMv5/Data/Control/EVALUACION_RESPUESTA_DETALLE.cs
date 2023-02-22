namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_RESPUESTA_DETALLE")]
    public partial class EVALUACION_RESPUESTA_DETALLE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? ID_EVALUACION_RESPUESTA { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(125)]
        public string S_OPCIONES { get; set; }
    }
}
