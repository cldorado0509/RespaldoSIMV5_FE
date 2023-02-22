namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_CUENTA_COBRAR")]
    public partial class TMP_CUENTA_COBRAR
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CONCEPTO { get; set; }

        public long VALOR { get; set; }

        public int CODIGO_TERCERO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CCOSTOS { get; set; }

        public int CONSECUTIVO { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        public DateTime? FECHA_VENCIMIENTO { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODIGO_CONCEPTO_REF { get; set; }

        public decimal? PORCENTAJE { get; set; }

        public decimal? CANTIDAD { get; set; }

        public decimal? VALOR_UNITARIO { get; set; }
    }
}
