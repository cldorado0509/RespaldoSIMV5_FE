namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_TAMANIO_EMPRESA_MRQ")]
    public partial class QRY_TAMANIO_EMPRESA_MRQ
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(500)]
        public string S_DESCRIPCION { get; set; }
    }
}
