namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.V_PARAMETROS_SISTEMA")]
    public partial class V_PARAMETROS_SISTEMA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CODIGO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string VALOR { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string DESCRIPCION { get; set; }
    }
}
