namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSERIE")]
    public partial class TBSERIE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CODSERIE { get; set; }

        [Required]
        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        public decimal TIEMPO_CENTRAL { get; set; }

        public decimal TIEMPO_HISTORICO { get; set; }

        public decimal TIEMPO_GESTION { get; set; }

        public decimal? CODSUBSERIE_DOCUMENTAL { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }
        public string S_DEFINEEXPEDIENTE { get; set; }
        [StringLength(1)]
        public string S_ADMINMODULO { get; set; }

        public virtual ICollection<RADICADO_DOCUMENTO> RADICADO_DOCUMENTO { get; set; }

        public virtual ICollection<RADICADO_UNIDADDOC> RADICADO_UNIDADDOC { get; set; }

        public virtual ICollection<RADICADOS> RADICADOS { get; set; }

        public virtual ICollection<TBINDICESERIE> TBINDICESERIE { get; set; }

        public virtual ICollection<TBTRAMITEDOCUMENTO> TBTRAMITEDOCUMENTO { get; set; }

        public virtual ICollection<TIPO_ACTO> TIPO_ACTO { get; set; }
    }
}
