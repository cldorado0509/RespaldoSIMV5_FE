namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.CPC")]
    public partial class CPC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CPC()
        {
            USO_CPC = new HashSet<USO_CPC>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CPC { get; set; }

        [Required]
        [StringLength(100)]
        public string S_CPC { get; set; }

        public int? ID_UNIDAD { get; set; }

        public decimal? ID_CPCPADRE { get; set; }

        [StringLength(10)]
        public string S_CODIGO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USO_CPC> USO_CPC { get; set; }
    }
}
