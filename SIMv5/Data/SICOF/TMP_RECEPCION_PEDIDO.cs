namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_RECEPCION_PEDIDO")]
    public partial class TMP_RECEPCION_PEDIDO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_RECEPCION { get; set; }

        public long VALOR { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }
    }
}
