namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.INFO_INDIVIDUO_DISPERSO")]
    public partial class INFO_INDIVIDUO_DISPERSO
    {
        [Key]
        public decimal ID_INDIVIDUO_DISPERSO { get; set; }

        public decimal? ID_ESPECIE { get; set; }

        [StringLength(200)]
        public string NOMBRE_COMUN { get; set; }

        [StringLength(100)]
        public string NOMBRE_CIENTIFICO { get; set; }

        [StringLength(15)]
        public string COD_INDIVIDUO_DISPERSO { get; set; }

        public DateTime? D_ACTUALIZACION { get; set; }

        [StringLength(150)]
        public string S_LOCALIZACION_ARBOL { get; set; }

        public DateTime? D_FECHA { get; set; }

        [StringLength(100)]
        public string TIPO_INTERVENCION { get; set; }

        public decimal? DIAMETRO_NORMAL { get; set; }

        public decimal? ALTURA_TOTAL { get; set; }

        public decimal? ALTURA_COPA { get; set; }

        public decimal? DIAM_COPA_MAYOR { get; set; }

        public decimal? DIAM_COPA_MENOR { get; set; }

        [StringLength(100)]
        public string ESTADO_ARBOL { get; set; }

        [StringLength(100)]
        public string COBERTURA_PIE { get; set; }

        public short? BIFURCACIONES { get; set; }
    }
}
