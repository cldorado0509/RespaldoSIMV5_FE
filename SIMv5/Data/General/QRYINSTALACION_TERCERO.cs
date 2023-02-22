namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRYINSTALACION_TERCERO")]
    public partial class QRYINSTALACION_TERCERO
    {
        [StringLength(100)]
        public string INSTALACION { get; set; }

        [StringLength(100)]
        public string DIRECCION_ESPECIAL { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INSTALACION { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        [StringLength(100)]
        public string MUNICIPIO { get; set; }

        [StringLength(191)]
        public string DIRECCION { get; set; }

        [StringLength(1)]
        public string ACTUAL { get; set; }
    }
}
