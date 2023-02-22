namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.ENC_PREGUNTA_DEPENDENCIA")]
    public partial class ENC_PREGUNTA_DEPENDENCIA
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID_PREGUNTA_PADRE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_PREGUNTA_HIJA { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal TIPO { get; set; }

        public string OPCIONES { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }
    }
}
