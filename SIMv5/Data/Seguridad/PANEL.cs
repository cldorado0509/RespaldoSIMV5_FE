namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.PANEL")]
    public partial class PANEL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PANEL()
        {
            CONTROL = new HashSet<CONTROL>();
            GRUPO_PANEL = new HashSet<GRUPO_PANEL>();
            USUARIO_PANEL = new HashSet<USUARIO_PANEL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PANEL { get; set; }

        public int ID_FORMA { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(10)]
        public string S_ATRIBUTO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_NUEVO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_EDITAR { get; set; }

        [StringLength(1)]
        public string S_ELIMINAR { get; set; }

        [Required]
        [StringLength(1)]
        public string S_BUSCAR { get; set; }

        [Required]
        [StringLength(1)]
        public string S_IMPRIMIR { get; set; }

        public int? COD_FUNCIONARIO_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTROL> CONTROL { get; set; }
        
        [ForeignKey("ID_FORMA")]
        public virtual FORMA FORMA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_PANEL> GRUPO_PANEL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_PANEL> USUARIO_PANEL { get; set; }
    }
}
