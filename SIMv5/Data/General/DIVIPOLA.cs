namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DIVIPOLA")]
    public partial class DIVIPOLA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIVIPOLA()
        {
            DIVIPOLA1 = new HashSet<DIVIPOLA>();
            INSTALACION = new HashSet<INSTALACION>();
            NATURAL = new HashSet<NATURAL>();
            TERCERO = new HashSet<TERCERO>();
            AUTORIDAD_AMBIENTAL = new HashSet<AUTORIDAD_AMBIENTAL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DIVIPOLA { get; set; }

        public int? ID_DIVIPOLAPADRE { get; set; }

        [StringLength(15)]
        public string S_CODIGO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(5)]
        public string S_CODIGOEPM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIVIPOLA> DIVIPOLA1 { get; set; }

        [ForeignKey("ID_DIVIPOLAPADRE")]
        public virtual DIVIPOLA DIVIPOLA2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INSTALACION> INSTALACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NATURAL> NATURAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO> TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUTORIDAD_AMBIENTAL> AUTORIDAD_AMBIENTAL { get; set; }
    }
}
