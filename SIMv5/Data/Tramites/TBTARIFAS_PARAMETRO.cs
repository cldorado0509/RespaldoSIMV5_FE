namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTARIFAS_PARAMETRO")]
    public partial class TBTARIFAS_PARAMETRO
    {
        [Key]
        public decimal ID_PARAMETRO { get; set; }

        [Required]
        [StringLength(200)]
        public string NOMBRE { get; set; }

        public decimal VALOR { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        [Required]
        [StringLength(4)]
        public string ANO { get; set; }
    }
}
