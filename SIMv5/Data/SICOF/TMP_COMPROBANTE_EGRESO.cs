namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_COMPROBANTE_EGRESO")]
    public partial class TMP_COMPROBANTE_EGRESO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_ORDEN_PAGO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CODIGO_RUBRO { get; set; }

        public long VALOR { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        public int? CODIGO_TERCERO { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }
    }
}
