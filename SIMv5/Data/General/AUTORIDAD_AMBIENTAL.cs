namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.AUTORIDAD_AMBIENTAL")]
    public partial class AUTORIDAD_AMBIENTAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AUTORIDAD_AMBIENTAL()
        {
            DIVIPOLA = new HashSet<DIVIPOLA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_AUTORIDADAMBIENTAL { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(100)]
        public string S_SIGLA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIVIPOLA> DIVIPOLA { get; set; }
    }
}
