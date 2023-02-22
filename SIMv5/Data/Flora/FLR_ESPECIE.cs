namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_ESPECIE")]
    public partial class FLR_ESPECIE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESPECIE { get; set; }

        public int ID_FAMILIA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_GENERO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_ESPECIE { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE_CIENTIFICO { get; set; }

        [StringLength(200)]
        public string S_NOMBRE_COMUN { get; set; }

        public int? ID_HABITO_CRECIMIENTO { get; set; }

        public int? ID_TIPO_ORIGEN { get; set; }

        [StringLength(100)]
        public string S_ORIGEN { get; set; }

        public int? ID_GREMIO_ECOLOGICO { get; set; }

        public int? ID_TALLA { get; set; }

        public int? ID_COPA { get; set; }

        public int? ID_CRECIMIENTO { get; set; }

        public int? ID_LONGEVIDAD { get; set; }

        public int? ID_RIESGO_EXTINCION { get; set; }

        public int? ID_APORTE_PAISAJISTICO { get; set; }

        public int? ID_SERVICIOS_AMBIENTALES { get; set; }

        public int? ID_SENSIBILIDAD_CONTAMINACION { get; set; }

        public int? ID_ORIGEN_ESPECIE { get; set; }

        [ForeignKey("ID_RIESGO_EXTINCION")]
        public virtual FLR_RIESGO_EXTINCION FLR_RIESGO_EXTINCION { get; set; }
    }
}
