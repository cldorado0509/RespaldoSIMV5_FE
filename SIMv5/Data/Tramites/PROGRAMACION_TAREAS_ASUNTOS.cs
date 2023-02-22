namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_TAREAS_ASUNTOS")]
    public partial class PROGRAMACION_TAREAS_ASUNTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROGRAMACION_TAREAS_ASUNTOS { get; set; }

        public int ID_ASUNTO { get; set; }

        public int CODTAREA { get; set; }
    }
}
