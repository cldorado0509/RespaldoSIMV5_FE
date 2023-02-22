namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROYECCION_DOC")]
    public partial class PROYECCION_DOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROYECCION_DOC()
        {
            PROYECCION_DOC_FIRMAS = new HashSet<PROYECCION_DOC_FIRMAS>();
            PROYECCION_DOC_COM = new HashSet<PROYECCION_DOC_COM>();
            PROYECCION_DOC_INDICES = new HashSet<PROYECCION_DOC_INDICES>();
            TRAMITES_PROYECCION = new HashSet<TRAMITES_PROYECCION>();
            PROYECCION_DOC_ARCHIVOS = new HashSet<PROYECCION_DOC_ARCHIVOS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROYECCION_DOC { get; set; }

        [Required]
        [StringLength(10)]
        public string S_CENTRO_COSTOS { get; set; }

        public int CODSERIE { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(3)]
        public string S_FORMULARIO { get; set; }

        public DateTime D_FECHA_CREACION { get; set; }

        public int CODFUNCIONARIO { get; set; }

        public int? ID_RADICADODOC { get; set; }

        public int? CODFUNCIONARIO_ACTUAL { get; set; }

        [StringLength(4000)]
        public string S_TRAMITES { get; set; }

        [StringLength(256)]
        public string S_PROCESOS { get; set; }

        [StringLength(1)]
        public string S_TRAMITE_NUEVO { get; set; }

        public DateTime? D_FECHA_TRAMITE { get; set; }

        [StringLength(1)]
        public string S_NO_AVANZAR { get; set; }

        public int? ID_GRUPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECCION_DOC_FIRMAS> PROYECCION_DOC_FIRMAS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECCION_DOC_COM> PROYECCION_DOC_COM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECCION_DOC_INDICES> PROYECCION_DOC_INDICES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRAMITES_PROYECCION> TRAMITES_PROYECCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROYECCION_DOC_ARCHIVOS> PROYECCION_DOC_ARCHIVOS { get; set; }
    }
}
