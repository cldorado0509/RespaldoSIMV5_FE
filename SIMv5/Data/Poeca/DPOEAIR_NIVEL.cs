namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.DPOEAIR_NIVEL")]
    public partial class DPOEAIR_NIVEL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_NIVEL()
        {
            TPOEAIR_ACCIONES_PLAN = new HashSet<TPOEAIR_ACCIONES_PLAN>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Nivel de aplicación")]
        public string S_NOMBRE_NIVEL { get; set; }

        [StringLength(200)]
        public string S_DESCRIPCION { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }
    }
}
