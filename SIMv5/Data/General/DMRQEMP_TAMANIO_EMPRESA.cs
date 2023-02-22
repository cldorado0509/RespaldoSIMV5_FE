namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DMRQEMP_TAMANIO_EMPRESA")]
    public partial class DMRQEMP_TAMANIO_EMPRESA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DMRQEMP_TAMANIO_EMPRESA()
        {
            INSTALACION = new HashSet<INSTALACION>();
        }

        public decimal ID { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(500)]
        public string S_DESCRIPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSTALACION> INSTALACION { get; set; }
    }
}
