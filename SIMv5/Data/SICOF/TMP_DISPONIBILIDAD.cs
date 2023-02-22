namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.TMP_DISPONIBILIDAD")]
    public partial class TMP_DISPONIBILIDAD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_RUBRO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_CCOSTOS { get; set; }

        public long VALOR { get; set; }

        public DateTime? FECHA_DOCUMENTO { get; set; }

        public int CONSECUTIVO { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(1)]
        public string ESTADO { get; set; }

        public decimal? ID_PROCESO { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }
    }
}
