namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_MEDIDA_ACCION")]
    public partial class TPOEAIR_MEDIDA_ACCION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TPOEAIR_MEDIDA_ACCION()
        {
            TPOEAIR_ACCIONES_PLAN = new HashSet<TPOEAIR_ACCIONES_PLAN>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name ="Acción")]
        public int ID_ACCION { get; set; }

        public int ID_MEDIDA_SECTOR { get; set; }

        public virtual DPOEAIR_ACCION DPOEAIR_ACCION { get; set; }
        public virtual TPOEAIR_SECTOR_MEDIDA TPOEAIR_SECTOR_MEDIDA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }

        //public virtual TPOEAIR_SECTOR_MEDIDA TPOEAIR_SECTOR_MEDIDA { get; set; }
    }
}
