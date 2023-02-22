namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.DDECONO_TERCERODE")]
    public partial class DDECONO_TERCERODE
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DDECONO_TERCERODE()
        {
            DDECONO_CATEGORIA_TERCERO = new HashSet<DDECONO_CATEGORIA_TERCERO>();
            DDECONO_MUNICIPIO_TERCERO = new HashSet<DDECONO_MUNICIPIO_TERCERO>();
            TDECONO_PRODUCTO = new HashSet<TDECONO_PRODUCTO>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal ID_TERCERO { get; set; }

        [Required]
        [StringLength(200)]
        public string S_RAZON_SOCIAL { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(200)]
        public string S_URL_INSTAGRAM { get; set; }

        [StringLength(200)]
        public string S_URL_FACEBOOK { get; set; }

        [StringLength(200)]
        public string S_DIRECCION { get; set; }

        [StringLength(20)]
        public string S_TELEFONO1 { get; set; }

        [StringLength(20)]
        public string S_TELEFONO2 { get; set; }

        [StringLength(200)]
        public string S_URL_LOGO { get; set; }

        [StringLength(200)]
        public string S_EMAIL { get; set; }

        [Required]
        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        [StringLength(200)]
        public string S_URL_WEB { get; set; }

        [StringLength(200)]
        public string S_URL_FOTO_EMPRENDEDOR { get; set; }

        [StringLength(20)]
        public string S_NIT { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DDECONO_CATEGORIA_TERCERO> DDECONO_CATEGORIA_TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DDECONO_MUNICIPIO_TERCERO> DDECONO_MUNICIPIO_TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TDECONO_PRODUCTO> TDECONO_PRODUCTO { get; set; }*/
    }
}
