namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.MAE_COMERCIAL")]
    public partial class MAE_COMERCIAL
    {
        public int CONSECUTIVO { get; set; }

        public DateTime FECHA_REGISTRO { get; set; }

        [Required]
        [StringLength(4)]
        public string TIPO { get; set; }

        [Required]
        [StringLength(1000)]
        public string DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string ESTADO { get; set; }

        public DateTime FECHA_DOCUMENTO { get; set; }

        public int TOTAL_REGISTROS { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_COMERCIAL { get; set; }

        public int? CODIGO_USUARIO_REGISTRA { get; set; }

        public int? CODIGO_DEPENDENCIA { get; set; }

        [StringLength(8)]
        public string CODIGO_BANCO { get; set; }

        public DateTime? FECHA_ANULACION { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        public short? VIGENCIA { get; set; }

        public int? VALOR_TOTAL { get; set; }

        public virtual TIPOS_COMERCIAL TIPOS_COMERCIAL { get; set; }
    }
}
