namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBSERIE")]
    public partial class TBSERIE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBSERIE()
        {
            RADICADO_DOCUMENTO = new HashSet<RADICADO_DOCUMENTO>();
            RADICADO_UNIDADDOC = new HashSet<RADICADO_UNIDADDOC>();
            RADICADOS = new HashSet<RADICADOS>();
            TBINDICESERIE = new HashSet<TBINDICESERIE>();
            TBTRAMITEDOCUMENTO = new HashSet<TBTRAMITEDOCUMENTO>();
            TIPO_ACTO = new HashSet<TIPO_ACTO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public decimal CODSERIE { get; set; }

        [Required]
        [StringLength(1000)]
        public string NOMBRE { get; set; }

        [StringLength(4000)]
        public string DESCRIPCION { get; set; }

        [StringLength(400)]
        public string RUTA_DOCUMENTOS { get; set; }

        [Required]
        [StringLength(1)]
        public string ACTIVO { get; set; }

        public decimal TIEMPO_CENTRAL { get; set; }

        public decimal TIEMPO_HISTORICO { get; set; }

        public decimal TIEMPO_GESTION { get; set; }

        public decimal? CODSUBSERIE_DOCUMENTAL { get; set; }

        public string S_DEFINEEXPEDIENTE { get; set; }

        [StringLength(1)]
        public string RADICADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADO_DOCUMENTO> RADICADO_DOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADO_UNIDADDOC> RADICADO_UNIDADDOC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RADICADOS> RADICADOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBINDICESERIE> TBINDICESERIE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBTRAMITEDOCUMENTO> TBTRAMITEDOCUMENTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIPO_ACTO> TIPO_ACTO { get; set; }
    }
}
