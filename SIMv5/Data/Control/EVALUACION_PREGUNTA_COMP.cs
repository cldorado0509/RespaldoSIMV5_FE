namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EVALUACION_PREGUNTA_COMP")]
    public partial class EVALUACION_PREGUNTA_COMP
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int ID_GRUPO_COMP { get; set; }

        [Required]
        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        public int N_ORDEN { get; set; }
    }
}
