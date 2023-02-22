namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.RADICADO_UNIDADDOC")]
    public partial class RADICADO_UNIDADDOC
    {
        [Key]
        public decimal ID_RADICADOUNIDADDOC { get; set; }

        public decimal ID_RADICADO { get; set; }

        public decimal CODSERIE { get; set; }

        public decimal ID_INDICE { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        [ForeignKey("ID_RADICADO")]
        public virtual DEFRADICADOS DEFRADICADOS { get; set; }

        [ForeignKey("CODSERIE")]
        public virtual TBSERIE TBSERIE { get; set; }
    }
}
