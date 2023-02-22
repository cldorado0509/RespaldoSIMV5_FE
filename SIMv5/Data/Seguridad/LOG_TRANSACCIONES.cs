namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.LOG_TRANSACCIONES")]
    public partial class LOG_TRANSACCIONES
    {
        [Key]
        public decimal ID_LOG { get; set; }

        public DateTime FECHA { get; set; }

        public decimal? ID_FORMA { get; set; }

        public decimal? ID_PANEL { get; set; }

        public decimal? ID_GRID { get; set; }

        public decimal ID_DATAKEY { get; set; }

        [Required]
        [StringLength(250)]
        public string CAMPO { get; set; }

        [StringLength(4000)]
        public string VALOR_ANTERIOR { get; set; }

        [StringLength(4000)]
        public string VALOR_NUEVO { get; set; }

        public decimal? ID_USUARIO { get; set; }
    }
}
