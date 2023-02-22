namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_ROL")]
    public partial class USUARIO_ROL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_USUARIO_ROL { get; set; }

        public int ID_ROL { get; set; }

        public int ID_USUARIO { get; set; }
    }
}
