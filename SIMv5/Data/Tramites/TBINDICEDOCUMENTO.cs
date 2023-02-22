namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBINDICEDOCUMENTO")]
    public partial class TBINDICEDOCUMENTO
    {
        public int CODINDICE { get; set; }

        public decimal CODDOCUMENTO { get; set; }

        [StringLength(510)]
        public string VALOR { get; set; }

        public DateTime? FECHAACTUALIZA { get; set; }

        [StringLength(254)]
        public string VALORID { get; set; }

        public DateTime FECHAREGISTRO { get; set; }

        public DateTime? FECHALIMITE { get; set; }

        public decimal? CODTRAMITE { get; set; }

        public decimal? CODSERIE { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_INDICE_DOCUMENTO { get; set; }
        public decimal? ID_DOCUMENTO { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
