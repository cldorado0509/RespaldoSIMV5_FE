namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.MENSAJE")]
    public partial class MENSAJE
    {
        [Key]
        public decimal ID_MENSAJE { get; set; }

        [Required]
        [StringLength(2000)]
        public string NOMBRE { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }

        [StringLength(1)]
        public string ELIMINADO { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        public int ID_GRUPO { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }
    }
}
