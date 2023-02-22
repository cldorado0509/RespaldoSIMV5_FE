namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_ASIENTO_CONTABLE")]
    public partial class TMP_ASIENTO_CONTABLE
    {
        public int CODIGO_PLANTILLA { get; set; }

        public int CODIGO_TERCERO { get; set; }

        public int CODIGO_CCOSTOS { get; set; }

        public int NUMERO_DOCUMENTO { get; set; }

        public int CODIGO_DOCUMENTO { get; set; }

        public int CODIGO_CINGRESO { get; set; }

        [StringLength(3)]
        public string TAQUILLA { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SECUENCIA { get; set; }

        public long VALOR { get; set; }

        public int CODIGO_CONCEPTO { get; set; }

        public int CODIGO_TIPO { get; set; }

        public int NUMERO_DOCUMENTO_REF { get; set; }

        public int CTERCERO_CPCOBRAR { get; set; }

        [Required]
        [StringLength(1)]
        public string DESCONTAR_BASE { get; set; }

        public long? VALOR_DESCONTAR_BASE { get; set; }

        public int? CODIGO_INTERNO_BANCO { get; set; }

        public int? CODIGO_CUENTA_BANCO { get; set; }

        [Required]
        [StringLength(1)]
        public string ASIENTO_REFERENCIA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        public long VALOR_IVA { get; set; }

        [StringLength(2)]
        public string SIGNO { get; set; }

        public decimal? TIPO_PLAN_CONTABLE { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public decimal? TIPO_CENTRO_COSTOS { get; set; }

        [StringLength(64)]
        public string CODIGO_BANCO { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        public int? TERCERO_ALTERNO { get; set; }
    }
}
