namespace SIM.Data.Tramites
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRAMITES.TBVISITADOCUMENTO")]
    public class TBVISITADOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CODVISITA { get; set; }

        [Required]
        public decimal CODDOCUMENTO { get; set; }

        [Required]
        public decimal CODTRAMITE { get; set; }

        [Required]
        public decimal CODFUNCIONARIO { get; set; }

        public DateTime FECHA { get; set; }

        public decimal IDDOCUMENTO { get; set; }
    }
}