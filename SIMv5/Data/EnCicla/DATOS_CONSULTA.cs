namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.DATOS_CONSULTA")]
    public partial class DATOS_CONSULTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDDATOSCONSULTA { get; set; }

        public int IDTERCERO { get; set; }

        [StringLength(150)]
        public string NOMBRE { get; set; }

        public DateTime FECHA { get; set; }

        public string DATOS { get; set; }
    }
}
