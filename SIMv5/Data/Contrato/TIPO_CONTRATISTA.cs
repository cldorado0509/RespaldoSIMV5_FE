namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.TIPO_CONTRATISTA")]
    public partial class TIPO_CONTRATISTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_CONTRATISTA()
        {
            CONTRATO_TERCERO = new HashSet<CONTRATO_TERCERO>();
        }

        [Key]
        public decimal ID_TIPOCONTRATISTA { get; set; }

        [Required]
        [StringLength(300)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTRATO_TERCERO> CONTRATO_TERCERO { get; set; }
    }
}
