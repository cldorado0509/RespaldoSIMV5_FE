namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.CARGO")]
    public partial class CARGO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CARGO { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_ELIMINADO { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public decimal? ID_DEPENDENCIA { get; set; }

        public int? N_CARGO { get; set; }

        [StringLength(150)]
        public string S_CODIGO { get; set; }
    }
}
