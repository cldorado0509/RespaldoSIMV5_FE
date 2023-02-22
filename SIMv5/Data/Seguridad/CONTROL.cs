namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.CONTROL")]
    public partial class CONTROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONTROL()
        {
            GRUPO_CONTROL = new HashSet<GRUPO_CONTROL>();
            USUARIO_CONTROLES = new HashSet<USUARIO_CONTROLES>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTROL { get; set; }

        public int? ID_PANEL { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(10)]
        public string S_ATRIBUTO { get; set; }

        public int? CODTAREA { get; set; }

        [ForeignKey("ID_PANEL")]
        public virtual PANEL PANEL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_CONTROL> GRUPO_CONTROL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_CONTROLES> USUARIO_CONTROLES { get; set; }
    }
}
