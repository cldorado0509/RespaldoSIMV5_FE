namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.DDECONO_MUNICIPIO")]
    public partial class DDECONO_MUNICIPIO
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DDECONO_MUNICIPIO()
        {
            DDECONO_MUNICIPIO_TERCERO = new HashSet<DDECONO_MUNICIPIO_TERCERO>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DDECONO_MUNICIPIO_TERCERO> DDECONO_MUNICIPIO_TERCERO { get; set; }*/
    }
}
