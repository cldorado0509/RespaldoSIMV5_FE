namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_DET_ASIENTO_CONTABLE")]
    public partial class TMP_DET_ASIENTO_CONTABLE
    {
        public int CODIGO_CUENTA { get; set; }

        public int CODIGO_TERCERO { get; set; }

        public int CODIGO_CCOSTOS { get; set; }

        public int NUMERO_DOCUMENTO { get; set; }

        [StringLength(3)]
        public string TAQUILLA { get; set; }

        public long VALOR_DEBITO { get; set; }

        public long VALOR_CREDITO { get; set; }

        public int CODIGO_BANCO { get; set; }

        [Required]
        [StringLength(1)]
        public string TIPO_CUENTA { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SECUENCIA { get; set; }

        public int CTERCERO_CPCOBRAR { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        public decimal? VALOR_BASE { get; set; }

        public decimal? VALOR_BASE_IVA { get; set; }
    }
}
