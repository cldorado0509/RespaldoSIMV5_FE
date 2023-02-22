namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_ORDEN_PAGO")]
    public partial class TMP_ORDEN_PAGO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_RUBRO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_DISPONIBILIDAD { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_COMPROMISO { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CCOSTOS { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_ARTICULO { get; set; }

        public decimal IVA { get; set; }

        public long VALOR { get; set; }

        [Required]
        [StringLength(1)]
        public string NOMINA { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        [Required]
        [StringLength(10)]
        public string VALOR_AMORTIZADO { get; set; }

        public long VALOR_IVA { get; set; }

        public long BASE_NO_GRAVABLE { get; set; }

        public DateTime? FECHA_VENCIMIENTO { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }
    }
}
