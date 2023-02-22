namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_COMPROBANTE_INGRESO")]
    public partial class TMP_COMPROBANTE_INGRESO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CPCOBRAR { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CONCEPTO { get; set; }

        public long VALOR { get; set; }

        public int CONSECUTIVO { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_CCOSTOS { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }
    }
}
