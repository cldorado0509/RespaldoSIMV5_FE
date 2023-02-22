namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PERMISO_AMBIENTAL")]
    public partial class PERMISO_AMBIENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PERMISO_AMBIENTAL()
        {
            DGA = new HashSet<DGA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERMISOAMBIENTAL { get; set; }

        [Required]
        [StringLength(50)]
        public string S_PERMISOAMBIENTAL { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DGA> DGA { get; set; }
    }
}
