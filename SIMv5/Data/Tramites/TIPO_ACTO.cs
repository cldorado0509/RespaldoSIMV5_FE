namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TIPO_ACTO")]
    public partial class TIPO_ACTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_ACTO()
        {
            ACTUACION_DOCUMENTO = new HashSet<ACTUACION_DOCUMENTO>();
            FORMULARIO_TIPOACTO = new HashSet<FORMULARIO_TIPOACTO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOACTO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        public decimal ID_SERIE { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACTUACION_DOCUMENTO> ACTUACION_DOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FORMULARIO_TIPOACTO> FORMULARIO_TIPOACTO { get; set; }

        [ForeignKey("ID_SERIE")]
        public virtual TBSERIE TBSERIE { get; set; }
    }
}
