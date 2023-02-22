namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC_ARCHIVOS")]
    public partial class PROYECCION_DOC_ARCHIVOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROYECCION_DOC_ARCHIVOS()
        {
            PROYECCION_DOC_DET_ARCH = new HashSet<PROYECCION_DOC_DET_ARCH>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC_ARCHIVOS { get; set; }

        public int ID_PROYECCION_DOC { get; set; }

        [Required]
        [StringLength(128)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        public int N_TIPO { get; set; }

        public int? N_ORDEN { get; set; }

        [ForeignKey("ID_PROYECCION_DOC")]
        public virtual PROYECCION_DOC PROYECCION_DOC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECCION_DOC_DET_ARCH> PROYECCION_DOC_DET_ARCH { get; set; }
    }
}
