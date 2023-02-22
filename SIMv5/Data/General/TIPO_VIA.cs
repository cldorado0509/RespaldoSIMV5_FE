namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TIPO_VIA")]
    public partial class TIPO_VIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_VIA()
        {
            INSTALACION = new HashSet<INSTALACION>();
            INSTALACION1 = new HashSet<INSTALACION>();
            VIA_EXCEPCION = new HashSet<VIA_EXCEPCION>();
            VIA_EXCEPCION1 = new HashSet<VIA_EXCEPCION>();
            VIA_EXCEPCION2 = new HashSet<VIA_EXCEPCION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOVIA { get; set; }

        [Required]
        [StringLength(20)]
        public string S_NOMBRE { get; set; }

        [StringLength(5)]
        public string S_ABREVIATURA { get; set; }

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
