namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_NUEVOS_ASUNTOS")]
    public partial class PROGRAMACION_NUEVOS_ASUNTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROGRAMACION_NUEVOS_A { get; set; }

        public int ID_PROGRAMACION { get; set; }

        public int ID_ASUNTO { get; set; }

        public int? CODTRAMITE_NUEVO { get; set; }

        public int? ID_USUARIO_ASIGNADO { get; set; }

        public int? CODTAREA_ASIGNADA { get; set; }

        [ForeignKey("ID_PROGRAMACION")]
        public virtual PROGRAMACION_ACTUACION PROGRAMACION_ACTUACION { get; set; }
    }
}
