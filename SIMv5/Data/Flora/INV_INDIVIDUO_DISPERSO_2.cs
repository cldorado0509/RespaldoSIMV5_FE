namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.INV_INDIVIDUO_DISPERSO_2")]
    public partial class INV_INDIVIDUO_DISPERSO_2
    {
        [Key]
        public decimal ID_INDIVIDUO_DISPERSO { get; set; }

        public int? ID_PU_INVEN { get; set; }

        public decimal? ID_ESPECIE { get; set; }

        public int? ID_LOCALIZACION_ARBOL { get; set; }

        public int? ID_EDAD_SIEMBRA { get; set; }

        public decimal? ID_ESTADO { get; set; }

        public int? ID_PROCEDENCIA { get; set; }

        public int? ID_UBICACION { get; set; }

        [StringLength(15)]
        public string COD_INDIVIDUO_DISPERSO { get; set; }

        [StringLength(1)]
        public string L_SETO { get; set; }

        public int? N_INDIVIDUOS_SETO { get; set; }

        public int? N_LADO_MANZANA { get; set; }

        public int? N_ORDEN { get; set; }

        public int? ID_INDIVIDUO_DISPERSO_ORIGEN { get; set; }

        public DateTime? D_INGRESO { get; set; }

        public DateTime? D_ACTUALIZACION { get; set; }

        public int? ID_TIPO_ARBOL { get; set; }
    }
}
