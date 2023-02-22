namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.PROPIETARIO")]
    public partial class PROPIETARIO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROPIETARIO { get; set; }

        public int? ID_TERCERO { get; set; }

        public int? ID_INSTALACION { get; set; }

        public int? ID_USUARIO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
