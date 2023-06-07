namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.INFORMATIVO_DOC")]
    public partial class INFORMATIVO_DOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INFORMATIVO_DOC()
        {
            INFORMATIVO_DOC_ARCHIVOS = new HashSet<INFORMATIVO_DOC_ARCHIVOS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INFORMATIVO_DOC { get; set; }

        public int? COD_TIPOCOMUNICACION { get; set; }

        [StringLength(250)]
        public string S_TIPOCOMUNICACION { get; set; }

        public int? ID_TERCERO { get; set; }

        [StringLength(512)]
        public string S_DESCRIPCION { get; set; }

        public DateTime D_FECHA { get; set; }

        [StringLength(3)]
        public string S_FORMULARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INFORMATIVO_DOC_ARCHIVOS> INFORMATIVO_DOC_ARCHIVOS { get; set; }
    }
}
