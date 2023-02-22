namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_RIESGO_EXTINCION")]
    public partial class FLR_RIESGO_EXTINCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FLR_RIESGO_EXTINCION()
        {
            FLR_ESPECIE = new HashSet<FLR_ESPECIE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_RIESGO_EXTINCION { get; set; }

        [StringLength(100)]
        public string S_RIESGO_EXTINCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FLR_ESPECIE> FLR_ESPECIE { get; set; }
    }
}
