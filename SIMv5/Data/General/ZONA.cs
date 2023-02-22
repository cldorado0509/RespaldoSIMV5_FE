namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.ZONA")]
    public partial class ZONA
    {
        [Key]
        public decimal ID_ZONA { get; set; }

        [Required]
        [StringLength(20)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }
    }
}
