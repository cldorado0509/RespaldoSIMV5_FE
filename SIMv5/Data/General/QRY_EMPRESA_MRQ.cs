namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_EMPRESA_MRQ")]
    public partial class QRY_EMPRESA_MRQ
    {
        public long? NIT { get; set; }

        public byte? DIGITO_VERIFICACION { get; set; }

        [StringLength(200)]
        public string RAZON_SOCIAL { get; set; }

        [StringLength(50)]
        public string NOMBRE { get; set; }

        public DateTime? FECHA_CREACION { get; set; }

        [StringLength(100)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string CODIGO_CIIU { get; set; }

        [Key]
        [StringLength(200)]
        public string CIIU { get; set; }

        public decimal? DOCUMENTO_REPLEGAL { get; set; }

        public decimal? DIGITO_VER_DOCUMENTO_REPLEGAL { get; set; }

        [StringLength(200)]
        public string REPRESENTANTE_LEGAL { get; set; }

        public decimal? DOCUMENTO_CONTACTO { get; set; }

        public decimal? DIGITO_VER_DOCUMENTO_CONTACTO { get; set; }

        [StringLength(200)]
        public string CONTACTO { get; set; }

        public long? N_DOCUMENTON { get; set; }
    }
}
