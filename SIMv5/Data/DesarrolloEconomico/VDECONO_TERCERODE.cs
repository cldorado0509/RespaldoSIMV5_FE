namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.VDECONO_TERCERODE")]
    public partial class VDECONO_TERCERODE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_TERCERO { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(200)]
        public string S_DIRECCION { get; set; }

        [StringLength(200)]
        public string S_EMAIL { get; set; }

        [StringLength(20)]
        public string S_NIT { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string S_RAZON_SOCIAL { get; set; }

        [StringLength(200)]
        public string S_URL_INSTAGRAM { get; set; }

        [StringLength(200)]
        public string S_URL_FACEBOOK { get; set; }

        [StringLength(20)]
        public string S_TELEFONO1 { get; set; }

        [StringLength(20)]
        public string S_TELEFONO2 { get; set; }

        [StringLength(200)]
        public string S_URL_LOGO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        [StringLength(200)]
        public string S_URL_WEB { get; set; }

        [StringLength(200)]
        public string S_URL_FOTO_EMPRENDEDOR { get; set; }

        [StringLength(200)]
        public string MUNICIPIO { get; set; }

        [StringLength(200)]
        public string CATEGORIA { get; set; }
    }
}
