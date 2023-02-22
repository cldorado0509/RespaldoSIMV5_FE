namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.ESTRATEGIA")]
    public partial class ESTRATEGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ESTRATEGIA()
        {
            ASIGNACION = new HashSet<ASIGNACION>();
            ESTACION = new HashSet<ESTACION>();
            ESTRATEGIA_ZONA = new HashSet<ESTRATEGIA_ZONA>();
            HISTORICO = new HashSet<HISTORICO>();
            OPERADOR = new HashSet<OPERADOR>();
            PARAMETRO = new HashSet<PARAMETRO_EN>();
            TERCERO_HISTORICO = new HashSet<TERCERO_HISTORICO>();
            TERCERO_ROL = new HashSet<TERCERO_ROL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESTRATEGIA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [StringLength(500)]
        public string S_CORREO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASIGNACION> ASIGNACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ESTACION> ESTACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ESTRATEGIA_ZONA> ESTRATEGIA_ZONA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERADOR> OPERADOR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_EN> PARAMETRO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_HISTORICO> TERCERO_HISTORICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO_ROL> TERCERO_ROL { get; set; }
    }
}
