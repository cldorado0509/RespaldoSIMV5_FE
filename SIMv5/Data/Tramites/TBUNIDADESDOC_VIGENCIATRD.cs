namespace SIM.Data.Tramites
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRAMITES.TBUNIDADESDOC_VIGENCIATRD")]
    public class TBUNIDADESDOC_VIGENCIATRD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CODUNIDADDOC_VIGENCIA { get; set; }

        [Required]
        public decimal CODVIGENCIA_TRD { get; set; }

        [ForeignKey("CODVIGENCIA_TRD")]
        public TBVIGENCIA_TRD TBVIGENCIA_TRD { get; set; }

        [Required]
        public decimal CODUNIDAD_DOCUMENTAL { get; set; }

        [ForeignKey("CODUNIDAD_DOCUMENTAL")]
        public TBSERIE TBSERIE { get; set; }
    }
}