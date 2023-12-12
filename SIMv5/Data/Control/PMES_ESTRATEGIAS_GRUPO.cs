namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.PMES_ESTRATEGIAS_GRUPO")]
    public partial class PMES_ESTRATEGIAS_GRUPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PMES_ESTRATEGIAS_GRUPO()
        {
            PMES_ESTRATEGIAS = new HashSet<PMES_ESTRATEGIAS>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        public int? N_ORDEN { get; set; }

        [StringLength(250)]
        public string S_TITULO { get; set; }

        public int? ID_ENCABEZADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMES_ESTRATEGIAS> PMES_ESTRATEGIAS { get; set; }
    }
}
