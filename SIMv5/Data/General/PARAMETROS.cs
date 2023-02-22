namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PARAMETROS")]
    public partial class PARAMETROS
    {
        [Key]
        public decimal ID_PARAMETRO { get; set; }

        [Required]
        [StringLength(50)]
        public string CLAVE { get; set; }

        [Required]
        [StringLength(255)]
        public string VALOR { get; set; }
    }
}
