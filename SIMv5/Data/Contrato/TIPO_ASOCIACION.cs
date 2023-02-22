namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.TIPO_ASOCIACION")]
    public partial class TIPO_ASOCIACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_ASOCIACION()
        {
            CONTRATO = new HashSet<CONTRATO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOASOCIACION { get; set; }

        [Required]
        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTRATO> CONTRATO { get; set; }
    }
}
