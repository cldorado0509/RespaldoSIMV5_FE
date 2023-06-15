namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.RADICADO_DOCUMENTO")]
    public partial class RADICADO_DOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_RADICADODOC { get; set; }

        public decimal? CODTRAMITE { get; set; }

        public decimal? CODDOCUMENTO { get; set; }

        public decimal ID_RADICADO { get; set; }

        public decimal CODSERIE { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        [Required]
        [StringLength(30)]
        public string S_RADICADO { get; set; }

        [Required]
        [StringLength(200)]
        public string S_ETIQUETA { get; set; }

        public DateTime D_RADICADO { get; set; }

        public decimal N_CONSECUTIVO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [StringLength(200)]
        public string S_OBSERVACION { get; set; }

        
        [ForeignKey("ID_RADICADO")]
        public virtual DEFRADICADOS DEFRADICADOS { get; set; }

        [ForeignKey("CODSERIE")]
        public virtual TBSERIE TBSERIE { get; set; }
        public decimal? ID_DOCUMENTO { get; set; }
    }
}
