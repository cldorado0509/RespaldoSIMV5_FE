namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.TIPO_CONTRATO")]
    public partial class TIPO_CONTRATO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_CONTRATO()
        {
            CONTRATO = new HashSet<CONTRATO>();
            TIPO_CONTRATO1 = new HashSet<TIPO_CONTRATO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOCONTRATO { get; set; }

        public int? ID_TIPOCONTRATOPADRE { get; set; }

        [Required]
        [StringLength(150)]
        public string S_NOMBRE { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTRATO> CONTRATO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIPO_CONTRATO> TIPO_CONTRATO1 { get; set; }

        [ForeignKey("ID_TIPOCONTRATOPADRE")]
        public virtual TIPO_CONTRATO TIPO_CONTRATO2 { get; set; }
    }
}
