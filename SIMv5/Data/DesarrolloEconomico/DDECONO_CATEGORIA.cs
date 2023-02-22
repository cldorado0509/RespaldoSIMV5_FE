namespace SIM.Data.DesarrolloEconomico
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLLO_ECONOMICO.DDECONO_CATEGORIA")]
    public partial class DDECONO_CATEGORIA
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DDECONO_CATEGORIA()
        {
            DDECONO_CATEGORIA_TERCERO = new HashSet<DDECONO_CATEGORIA_TERCERO>();
        }*/

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(4000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string B_ACTIVO { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DDECONO_CATEGORIA_TERCERO> DDECONO_CATEGORIA_TERCERO { get; set; }*/
    }
}
