namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.MUNICIPIOS")]
    public partial class MUNICIPIOS
    {
        [Key]
        public decimal ID_MUNI { get; set; }

        public decimal ID_DEPTO { get; set; }

        [Required]
        [StringLength(20)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(1)]
        public string AMVA { get; set; }
    }
}
