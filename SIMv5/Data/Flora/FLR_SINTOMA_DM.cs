namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_SINTOMA_DM")]
    public partial class FLR_SINTOMA_DM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FLR_SINTOMA_DM()
        {
            INT_INTERVENCION = new HashSet<INT_INTERVENCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SINTOMA_DM { get; set; }

        [Required]
        [StringLength(100)]
        public string S_SINTOMA_DM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INT_INTERVENCION> INT_INTERVENCION { get; set; }
    }
}
