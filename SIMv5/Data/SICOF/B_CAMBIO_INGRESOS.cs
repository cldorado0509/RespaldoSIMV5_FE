namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.B_CAMBIO_INGRESOS")]
    public partial class B_CAMBIO_INGRESOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_COMERCIAL { get; set; }

        public decimal NIT { get; set; }

        public int FACTURA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }
    }
}
