namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRYINSTALACIONTODAS")]
    public partial class QRYINSTALACIONTODAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INSTALACION { get; set; }

        [StringLength(100)]
        public string MUNICIPIO { get; set; }

        [StringLength(191)]
        public string DIRECCION { get; set; }

        [StringLength(100)]
        public string NOMBRE_INSTALACION { get; set; }
    }
}
