namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_ASIGNACION")]
    public partial class PROGRAMACION_ASIGNACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROGRAMACION_ASIGNACION { get; set; }

        public int ID_ASIGNACION { get; set; }

        public int TIPO { get; set; }

        public int? CODFUNCIONARIO { get; set; }
    }
}
