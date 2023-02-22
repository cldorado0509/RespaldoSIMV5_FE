namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.PERMISO_OBJETO")]
    public partial class PERMISO_OBJETO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PERMISOOBJETO { get; set; }

        public int? ID_USUARIO { get; set; }

        public int? ID_TIPOOBJETO { get; set; }

        public int? ID_OBJETO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_TIPOOBJETO")]
        public virtual TIPO_OBJETO TIPO_OBJETO { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
