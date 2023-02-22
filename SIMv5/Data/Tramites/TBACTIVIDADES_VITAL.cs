namespace SIM.Data.Tramites
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("TRAMITES.TBACTIVIDADES_VITAL")]
    public class TBACTIVIDADES_VITAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CODIGO { get; set; }

        [Required]
        [StringLength(40)]
        public string CODIGO_VITAL { get; set; }
        
        [Required]
        public DateTime D_FECHA { get; set; }

        [Required]
        public decimal CODTRAMITE_TAREA { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }
    }
}