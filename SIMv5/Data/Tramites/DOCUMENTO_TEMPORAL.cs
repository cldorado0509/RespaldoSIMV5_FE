namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.DOCUMENTO_TEMPORAL")]
    public partial class DOCUMENTO_TEMPORAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_DOCUMENTO { get; set; }

        public decimal CODTRAMITE { get; set; }

        public decimal CODTAREA { get; set; }

        public decimal N_ORDEN { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_RUTA { get; set; }

        public decimal N_VERSION { get; set; }

        public decimal? COPIA { get; set; }

        public DateTime D_VERSION { get; set; }

        [Required]
        [StringLength(1)]
        public string APROBADO { get; set; }

        public int? CODTIPODOC { get; set; }

        [StringLength(1)]
        public string S_FIRMADO { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }

        public int? CODDOCUMENTO { get; set; }

        [StringLength(200)]
        public string NOMBRE_ARCHIVO { get; set; }

        [StringLength(20)]
        public string RADICADO_VITAL { get; set; }

        public decimal? NRO_SILPA { get; set; }

        [ForeignKey("CODFUNCIONARIO")]
        public virtual TBFUNCIONARIO TBFUNCIONARIO { get; set; }

        [ForeignKey("CODTAREA")]
        public virtual TBTAREA TBTAREA { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
