namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.ESTACION")]
    public partial class ESTACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ESTACION()
        {
            HISTORICO = new HashSet<HISTORICO>();
            HISTORICO1 = new HashSet<HISTORICO>();
            OPERACION = new HashSet<OPERACION>();
            OPERACION1 = new HashSet<OPERACION>();
            TERCERO_ESTACION = new HashSet<TERCERO_ESTACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESTACION { get; set; }

        public int ID_TIPOESTACION { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(100)]
        public string S_DERECCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [ForeignKey("ID_TIPOESTACION")]
        public virtual TIPO_ESTACION TIPO_ESTACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_ESTACION> TERCERO_ESTACION { get; set; }
    }
}
