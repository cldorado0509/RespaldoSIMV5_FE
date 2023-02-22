namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.LETRA_VIA")]
    public partial class LETRA_VIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LETRA_VIA()
        {
            INSTALACION = new HashSet<INSTALACION>();
            INSTALACION1 = new HashSet<INSTALACION>();
            VIA_EXCEPCION = new HashSet<VIA_EXCEPCION>();
            VIA_EXCEPCION1 = new HashSet<VIA_EXCEPCION>();
            VIA_EXCEPCION2 = new HashSet<VIA_EXCEPCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_LETRAVIA { get; set; }

        [Required]
        [StringLength(2)]
        public string S_NOMBRE { get; set; }

        public byte? N_CODIGO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSTALACION> INSTALACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSTALACION> INSTALACION1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VIA_EXCEPCION> VIA_EXCEPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VIA_EXCEPCION> VIA_EXCEPCION1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VIA_EXCEPCION> VIA_EXCEPCION2 { get; set; }
    }
}
