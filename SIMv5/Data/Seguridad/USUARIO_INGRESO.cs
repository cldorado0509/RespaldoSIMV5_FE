namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_INGRESO")]
    public partial class USUARIO_INGRESO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_USUARIOINGRESO { get; set; }

        public int ID_USUARIO { get; set; }

        public DateTime? D_INGRESO { get; set; }

        public DateTime? D_SALIDA { get; set; }
    }
}
