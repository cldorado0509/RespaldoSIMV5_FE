namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_PERIODICIDAD")]
    public partial class TPOEAIR_PERIODICIDAD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TPOEAIR_PERIODICIDAD()
        {
            TPOEAIR_ACCIONES_PLAN = new HashSet<TPOEAIR_ACCIONES_PLAN>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Periodicidad")]
        public string S_NOMBRE { get; set; }

        [StringLength(511)]
        public string S_DESCRIPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }
    }
}
