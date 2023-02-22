namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_SECTOR_MEDIDA")]
    public partial class TPOEAIR_SECTOR_MEDIDA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TPOEAIR_SECTOR_MEDIDA()
        {
            TPOEAIR_MEDIDA_ACCION = new HashSet<TPOEAIR_MEDIDA_ACCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Sector")]
        public int ID_SECTOR { get; set; }

        [Display(Name = "Medida")]
        public int ID_MEDIDA { get; set; }

        public virtual DPOEAIR_MEDIDA DPOEAIR_MEDIDA { get; set; }

        public virtual DPOEAIR_SECTOR DPOEAIR_SECTOR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_MEDIDA_ACCION> TPOEAIR_MEDIDA_ACCION { get; set; }
    }
}
