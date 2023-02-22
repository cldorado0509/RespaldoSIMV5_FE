namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.MAESTRO_TIPO")]
    public partial class MAESTRO_TIPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_TIPO { get; set; }

        [StringLength(50)]
        public string NOMBRE_TIPO { get; set; }

        [StringLength(250)]
        public string DESC_TIPO { get; set; }

        public DateTime? D_REGISTRO { get; set; }
    }
}
