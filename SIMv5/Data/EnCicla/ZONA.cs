namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.ZONA")]
    public partial class ZONA_EN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZONA_EN()
        {
            ESTRATEGIA_ZONA = new HashSet<ESTRATEGIA_ZONA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ZONA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_DESCRIOCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ESTRATEGIA_ZONA> ESTRATEGIA_ZONA { get; set; }
    }
}
