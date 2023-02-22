namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.UNIDAD_TIPO")]
    public partial class UNIDAD_TIPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_UNIDADTIPO { get; set; }

        public int ID_UNIDADDOCUMENTAL { get; set; }

        public int ID_TIPODOCUMENTAL { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }
    }
}
