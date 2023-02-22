namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.AYUDAS")]
    public partial class AYUDAS
    {
        [Key]
        public decimal ID_AYUDA { get; set; }

        public decimal? ID_AYUDA_PADRE { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        public string CONTENIDO { get; set; }

        [StringLength(1)]
        public string ACTIVO { get; set; }
    }
}
