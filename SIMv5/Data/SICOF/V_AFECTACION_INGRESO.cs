namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.V_AFECTACION_INGRESO")]
    public partial class V_AFECTACION_INGRESO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NO_INGRESO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RUBRO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_RESUMIDO { get; set; }

        [StringLength(200)]
        public string NOMBRE_RUBRO { get; set; }

        public decimal? VALOR { get; set; }
    }
}
