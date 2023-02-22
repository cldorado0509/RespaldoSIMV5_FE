namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBINDICESERIE")]
    public partial class TBINDICESERIE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODINDICE { get; set; }

        public decimal CODSERIE { get; set; }

        [Required]
        [StringLength(60)]
        public string INDICE { get; set; }

        public byte TIPO { get; set; }

        public long LONGITUD { get; set; }

        public int OBLIGA { get; set; }

        [StringLength(510)]
        public string VALORDEFECTO { get; set; }

        public int? CODIGO_SUBSERIE { get; set; }

        [StringLength(1)]
        public string UNICO { get; set; }

        [StringLength(1)]
        public string AUTO { get; set; }

        [StringLength(4000)]
        public string SCRIPT { get; set; }

        [Required]
        [StringLength(1)]
        public string MOSTRAR { get; set; }

        [StringLength(1)]
        public string CM { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }

        [StringLength(1)]
        public string CONJUNTO_VALORES { get; set; }

        public decimal? ORDEN { get; set; }

        public decimal CODGRUPO_INDICE { get; set; }

        [StringLength(20)]
        public string VALORMAXIMO { get; set; }

        [StringLength(20)]
        public string VALORMINIMO { get; set; }

        public decimal? CODSECUENCIA { get; set; }

        [StringLength(1)]
        public string MOSTRAR_EN_GRID { get; set; }

        [StringLength(1)]
        public string INDICE_RADICADO { get; set; }

        [ForeignKey("CODSERIE")]
        public virtual TBSERIE TBSERIE { get; set; }
    }
}
