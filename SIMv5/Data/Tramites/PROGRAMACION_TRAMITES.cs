namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_TRAMITES")]
    public partial class PROGRAMACION_TRAMITES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROGRAMACION_TRAMITES { get; set; }

        public int ID_PROGRAMACION { get; set; }

        public int CODTRAMITE { get; set; }

        public int? CODTAREA { get; set; }

        public int? CODTAREA_ASIGNADA { get; set; }

        public int? ID_USUARIO_ASIGNADO { get; set; }

        [StringLength(250)]
        public string S_ASUNTO { get; set; }

        [StringLength(50)]
        public string CM { get; set; }

        [ForeignKey("ID_PROGRAMACION")]
        public virtual PROGRAMACION_ACTUACION PROGRAMACION_ACTUACION { get; set; }
    }
}
