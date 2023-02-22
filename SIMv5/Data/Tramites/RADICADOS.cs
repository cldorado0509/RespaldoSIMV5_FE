namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.RADICADOS")]
    public partial class RADICADOS
    {
        [Key]
        public decimal ID_RADICADO { get; set; }

        public decimal CODSERIE { get; set; }

        public decimal N_RADICADO { get; set; }

        public DateTime D_FECHA_RECIBO { get; set; }

        public DateTime D_FECHA_INGRESO { get; set; }

        [Required]
        [StringLength(30)]
        public string S_ETIQUETA { get; set; }

        [StringLength(1)]
        public string S_DIGITALIZADO { get; set; }

        public decimal ID_USUARIO { get; set; }

        public decimal? CODTRAMITE { get; set; }

        public decimal? CODDOCUMENTO { get; set; }

        [ForeignKey("CODSERIE")]
        public virtual TBSERIE TBSERIE { get; set; }
    }
}
