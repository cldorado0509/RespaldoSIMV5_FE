namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_PREGUNTA")]
    public partial class PMES_ESTRATEGIAS_PREGUNTA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? ID_ENCABEZADO { get; set; }

        [Required]
        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        public int N_TIPO_RESPUESTA { get; set; }

        public int? N_ORDEN { get; set; }

        [StringLength(1)]
        public string S_VERSION { get; set; }
    }
}
