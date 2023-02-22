namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.VISITA")]
    public partial class VISITA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VISITA()
        {
            FOTOGRAFIA_VISITA = new HashSet<FOTOGRAFIA_VISITA>();
            INSTALACION_VISITA = new HashSet<INSTALACION_VISITA>();
            TERCERO_VISITA = new HashSet<TERCERO_VISITA>();
            TRAMITE_VISITA = new HashSet<TRAMITE_VISITA>();
            VISITAESTADO = new HashSet<VISITAESTADO>();
            VISITA_INFORME = new HashSet<VISITA_INFORME>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_VISITA { get; set; }

        [StringLength(400)]
        public string S_ASUNTO { get; set; }

        [StringLength(4000)]
        public string S_OBSERVACION { get; set; }

        [StringLength(20)]
        public string S_RADICADO { get; set; }

        public int? ID_TIPOVISITA { get; set; }

        [StringLength(100)]
        public string S_RUTA { get; set; }

        public int? ID_USUARIO { get; set; }

        public DateTime? D_FECHA_CREACION { get; set; }

        public DateTime? D_FECHA { get; set; }

        public int? DOCUMENTO_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FOTOGRAFIA_VISITA> FOTOGRAFIA_VISITA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSTALACION_VISITA> INSTALACION_VISITA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_VISITA> TERCERO_VISITA { get; set; }

        [ForeignKey("ID_TIPOVISITA")]
        public virtual TIPO_VISITA TIPO_VISITA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITE_VISITA> TRAMITE_VISITA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VISITAESTADO> VISITAESTADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VISITA_INFORME> VISITA_INFORME { get; set; }
    }
}
