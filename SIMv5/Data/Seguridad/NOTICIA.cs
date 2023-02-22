namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.NOTICIA")]
    public partial class NOTICIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NOTICIA()
        {
            USUARIO = new HashSet<USUARIO>();
        }

        [Key]
        public decimal ID_NOTICIA { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(4000)]
        public string S_NOTICIA { get; set; }

        public DateTime D_PUBLICACION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO> USUARIO { get; set; }
    }
}
