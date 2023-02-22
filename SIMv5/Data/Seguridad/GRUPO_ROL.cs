namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.GRUPO_ROL")]
    public partial class GRUPO_ROL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_GRUPO_ROL { get; set; }

        public int ID_ROL { get; set; }

        public int ID_GRUPO { get; set; }
    }
}
