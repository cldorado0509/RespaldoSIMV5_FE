namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_INTERVENCION")]
    public partial class FLR_INTERVENCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FLR_INTERVENCION()
        {
            INT_INTERVENCION = new HashSet<INT_INTERVENCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_INTERVENCION { get; set; }

        [StringLength(100)]
        public string S_INTERVENCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INT_INTERVENCION> INT_INTERVENCION { get; set; }
    }
}
