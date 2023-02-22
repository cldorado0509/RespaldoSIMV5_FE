namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.ROL")]
    public partial class ROL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ROL { get; set; }

        [Required]
        [StringLength(30)]
        public string S_NOMBRE { get; set; }

        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }
    }
}
