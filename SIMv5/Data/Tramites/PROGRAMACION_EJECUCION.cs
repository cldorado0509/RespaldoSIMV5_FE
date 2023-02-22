namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_EJECUCION")]
    public partial class PROGRAMACION_EJECUCION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PROGRAMACION_EJECUCION { get; set; }

        public int ID_PROGRAMACION { get; set; }

        public DateTime FECHA_EJECUCION { get; set; }

        [ForeignKey("ID_PROGRAMACION")]
        public virtual PROGRAMACION_ACTUACION PROGRAMACION_ACTUACION { get; set; }
    }
}
