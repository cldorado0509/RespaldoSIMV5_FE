namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.V_FACTURAS_SICOF")]
    public partial class V_FACTURAS_SICOF
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NO_FACTURA { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime FECHA { get; set; }

        public DateTime? FECHA_VENCE { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }

        [StringLength(20)]
        public string CODIGO_CONCEPTO { get; set; }

        [StringLength(500)]
        public string DESCRP_CONCEPTO { get; set; }

        public decimal? VALOR_CONCEPTO { get; set; }

        public int? CODIGO_COMERCIAL { get; set; }

        public decimal? NIT { get; set; }

        [StringLength(200)]
        public string NOMBRE { get; set; }

        [StringLength(50)]
        public string DIRECCION { get; set; }

        [StringLength(30)]
        public string TELEFONO { get; set; }

        public int? CUENTAS_VENCIDAS { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VALOR_VENCIDO { get; set; }

        [StringLength(30)]
        public string CIUDAD { get; set; }

        [StringLength(50)]
        public string DEPARTAMENTO { get; set; }
    }
}
