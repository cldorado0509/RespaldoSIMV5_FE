namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.MANUAL")]
    public partial class MANUAL
    {
        [Key]
        public decimal ID_MANUAL { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(500)]
        public string S_RUTA { get; set; }

        public int ID_GRUPO { get; set; }

        public int ID_MODULO { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }

        [ForeignKey("ID_MODULO")]
        public virtual MODULO MODULO { get; set; }
    }
}
