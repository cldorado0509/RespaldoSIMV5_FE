namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VW_INSTALACION")]
    public partial class VW_INSTALACION
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INSTALACION { get; set; }

        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(50)]
        public string S_TELEFONO { get; set; }

        [StringLength(20)]
        public string S_CEDULACATASTRAL { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }
    }
}
