namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRY_INSTALACION_EMPRESA_MRQ")]
    public partial class QRY_INSTALACION_EMPRESA_MRQ
    {
        public int? ID_TERCERO { get; set; }

        [StringLength(10)]
        public string TIPO_DOCUMENTO { get; set; }

        public long? NRO_DOCUMENTO { get; set; }

        [StringLength(200)]
        public string RAZON_SOCIAL { get; set; }

        [StringLength(50)]
        public string CODIGO_CIIU_ORGANIZACION { get; set; }

        [StringLength(200)]
        public string CIIU_ORGANIZACION { get; set; }

        public int? TELEFONO_ORGANIZACION { get; set; }

        [StringLength(100)]
        public string EMAIL_ORGANIZACION { get; set; }

        [StringLength(403)]
        public string SIGLA { get; set; }

        public decimal? DOCUMENTO_REPLEGAL { get; set; }

        public decimal? DIGITO_VER_DOCUMENTO_REPLEGAL { get; set; }

        [StringLength(200)]
        public string REPRESENTANTE_LEGAL { get; set; }

        public decimal? DOCUMENTO_CONTACTO { get; set; }

        public decimal? DIGITO_VER_DOCUMENTO_CONTACTO { get; set; }

        [StringLength(200)]
        public string CONTACTO { get; set; }

        [StringLength(1)]
        public string TIPO { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INSTALACION { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(12)]
        public string CM { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CODIGO_TIPO_INSTALACION { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string TIPO_INSTALACION { get; set; }

        [StringLength(50)]
        public string CODIGO_CIIU_INSTALACION { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string CIIU_INSTALACION { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string MUNICIPIO { get; set; }

        [StringLength(191)]
        public string DIRECCION { get; set; }

        [StringLength(100)]
        public string DIRECCION_ESPECIAL { get; set; }

        [StringLength(100)]
        public string NOMBRE_INSTALACION { get; set; }

        [StringLength(50)]
        public string TELEFONO_INSTALACION { get; set; }

        public decimal? CONTADOR { get; set; }

        public int? ID_DIVIPOLA { get; set; }
    }
}
