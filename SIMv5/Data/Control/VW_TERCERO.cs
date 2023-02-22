namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_TERCERO")]
    public partial class VW_TERCERO
    {
        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [StringLength(40)]
        public string DOCUMENTO { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string TIPO_DOCUMENTO { get; set; }

        [StringLength(40)]
        public string TELEFONO { get; set; }

        [StringLength(100)]
        public string EMAIL { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? IDDOCUMENTO { get; set; }

        public long? DOCUMENTOINT { get; set; }
    }
}
