namespace SIM.Data.Tramites
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRAMITES.TBVIGENCIA_TRD")]
    public class TBVIGENCIA_TRD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal COD_VIGENCIA_TRD { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }


        [Required]
        public DateTime D_INICIOVIGENCIA { get; set; }


    }
}