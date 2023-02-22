namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.TERCERO_ESTACION")]
    public partial class TERCERO_ESTACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCEROESTACION { get; set; }

        public int ID_ESTACION { get; set; }

        public int ID_TERCEROROL { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_ESTACION")]
        public virtual ESTACION ESTACION { get; set; }

        [ForeignKey("ID_TERCEROROL")]
        public virtual TERCERO_ROL TERCERO_ROL { get; set; }
    }
}
