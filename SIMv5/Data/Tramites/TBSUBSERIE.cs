namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSUBSERIE")]
    public partial class TBSUBSERIE
    {
        [Key]
        public decimal CODIGO_SUBSERIE { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(500)]
        public string DESCRIPCION { get; set; }

        public int TIPO { get; set; }

        [StringLength(100)]
        public string TABLA { get; set; }

        [StringLength(100)]
        public string CAMPO_ID { get; set; }

        [StringLength(100)]
        public string CAMPO_NOMBRE { get; set; }

        public decimal? CODIGO_SUBSERIE_DEPENDE { get; set; }

        [StringLength(4000)]
        public string CADENA_CONEXION { get; set; }

        [StringLength(4000)]
        public string SQL { get; set; }

        [StringLength(1)]
        public string TIPO_CONEXION { get; set; }

        [StringLength(1)]
        public string EDITABLE { get; set; }
    }
}
