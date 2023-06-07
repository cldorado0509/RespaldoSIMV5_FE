namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VIGENCIA")]
    public partial class VIGENCIA
    {
        [Key]
        public decimal ID_VIGENCIA { get; set; }

        public decimal? TIPOVIGENCIA { get; set; }

        [StringLength(40)]
        public string FECHA_INICIO { get; set; }

        [StringLength(40)]
        public string FECHA_FIN { get; set; }

        [Column("VIGENCIA")]
        [StringLength(20)]
        public string VIGENCIA1 { get; set; }

        [StringLength(2000)]
        public string TERMINO { get; set; }

        [StringLength(200)]
        public string NOMBRE { get; set; }

        public decimal? TIPOINSTALACION { get; set; }

        public decimal? CARDINALIDAD { get; set; }

        public decimal? RADICAR { get; set; }

        [StringLength(200)]
        public string ITEM { get; set; }

        public int? ID_PREGUNTA_DESC { get; set; }

        [StringLength(50)]
        public string S_NOMBRE_ARCHIVO { get; set; }

        public int? ID_PREGUNTA_CLAVE { get; set; }

        [StringLength(1)]
        public string TIPO_TERMINOS { get; set; }

        public int? TIPO_FORMULARIO { get; set; }

        [StringLength(250)]
        public string URL_PERSONALIZADA { get; set; }
    }
}
