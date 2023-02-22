namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.DDECONO_UNIDAD_MEDIDA")]
    public partial class DDECONO_UNIDAD_MEDIDA
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DDECONO_UNIDAD_MEDIDA()
        {
            TDECONO_PRODUCTO = new HashSet<TDECONO_PRODUCTO>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TDECONO_PRODUCTO> TDECONO_PRODUCTO { get; set; }*/
    }
}
