namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTARIFAS_TRAMITE")]
    public partial class TBTARIFAS_TRAMITE
    {
        [Key]
        public decimal ID_TRAMITE { get; set; }

        [Required]
        [StringLength(300)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(1)]
        public string TIPO_ACTUACION { get; set; }

        public int AUTO_INICIO { get; set; }

        public decimal? VISITA { get; set; }

        public decimal? INFORME { get; set; }

        public int RESOLUCION { get; set; }

        public decimal CODIGO_TRAMITE { get; set; }

        public int? CODIGO_TIPO_SOLICITUD { get; set; }

        public decimal? TECNICOS { get; set; }

        public int? N_VISITAS { get; set; }

        public decimal? N_RELACION { get; set; }

        [StringLength(50)]
        public string S_UNIDAD { get; set; }

        [StringLength(5)]
        public string CONCEPTO_CONTABLE { get; set; }

        [StringLength(5)]
        public string CONCEPTO_PUBLICACION { get; set; }

        [StringLength(1000)]
        public string S_DOCUMENTOSOPORTE { get; set; }
        public decimal? N_VISIBLE { get; set; }
    }
}
