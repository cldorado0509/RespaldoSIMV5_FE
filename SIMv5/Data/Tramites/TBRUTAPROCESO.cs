namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBRUTAPROCESO")]
    public partial class TBRUTAPROCESO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODRUTA { get; set; }

        [Required]
        [StringLength(254)]
        public string PATH { get; set; }

        public DateTime? FECHA { get; set; }

        [StringLength(100)]
        public string USUARIO { get; set; }

        [StringLength(254)]
        public string PASSWORD { get; set; }

        public decimal? CODPROCESO { get; set; }

        [StringLength(1)]
        public string LLENO { get; set; }

        public DateTime? FECHALLENO { get; set; }
    }
}
