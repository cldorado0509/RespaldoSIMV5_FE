namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_SECTOR")]
    public partial class QRY_SECTOR
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SECTOR { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string SECTOR { get; set; }
    }
}
