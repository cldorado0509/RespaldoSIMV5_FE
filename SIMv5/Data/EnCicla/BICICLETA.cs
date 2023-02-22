namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.BICICLETA")]
    public partial class BICICLETA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BICICLETA()
        {
            ASIGNACION = new HashSet<ASIGNACION>();
            HISTORICO = new HashSet<HISTORICO>();
            OPERACION = new HashSet<OPERACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_BICICLETA { get; set; }

        [Required]
        [StringLength(6)]
        public string S_CODIGO { get; set; }

        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ELECTRICA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASIGNACION> ASIGNACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORICO> HISTORICO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION> OPERACION { get; set; }
    }
}
