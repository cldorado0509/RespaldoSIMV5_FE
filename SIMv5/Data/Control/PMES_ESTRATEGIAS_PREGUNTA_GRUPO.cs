namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_PREGUNTA_GRUPO")]
    public partial class PMES_ESTRATEGIAS_PREGUNTA_GRUPO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public int N_ORDEN { get; set; }

        public int? ID_GRUPO_PADRE { get; set; }
    }
}
