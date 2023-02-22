namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.PROGRAMACION_ACTUACION")]
    public partial class PROGRAMACION_ACTUACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROGRAMACION_ACTUACION()
        {
            PROGRAMACION_EJECUCION = new HashSet<PROGRAMACION_EJECUCION>();
            PROGRAMACION_NUEVOS_ASUNTOS = new HashSet<PROGRAMACION_NUEVOS_ASUNTOS>();
            PROGRAMACION_TRAMITES = new HashSet<PROGRAMACION_TRAMITES>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PROGRAMACION { get; set; }

        public int ID_USUARIO { get; set; }

        public int TIPO { get; set; }

        public decimal? CODIGO_PROYECTO { get; set; }

        public int? CODIGO_SOLICITUD { get; set; }

        public int? ID_ZONA { get; set; }

        public int? ANO { get; set; }

        public int? MES { get; set; }

        [StringLength(1024)]
        public string OBSERVACIONES { get; set; }

        public DateTime? FECHA_PROGRAMACION { get; set; }

        public int? ESTADO { get; set; }

        public DateTime? FECHA_EJECUCION { get; set; }

        [StringLength(2048)]
        public string S_TRAMITES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROGRAMACION_EJECUCION> PROGRAMACION_EJECUCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROGRAMACION_NUEVOS_ASUNTOS> PROGRAMACION_NUEVOS_ASUNTOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROGRAMACION_TRAMITES> PROGRAMACION_TRAMITES { get; set; }
    }
}
