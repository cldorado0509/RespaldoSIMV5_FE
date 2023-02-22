namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.CORRESPONDENCIAENVIADA")]
    public partial class CORRESPONDENCIAENVIADA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_COD { get; set; }

        public DateTime D_FECHA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_TIPOSERVICIO { get; set; }

        [StringLength(20)]
        public string S_ORDEN { get; set; }
    }
}
