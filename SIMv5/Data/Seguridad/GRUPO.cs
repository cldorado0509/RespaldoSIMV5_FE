namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.GRUPO")]
    public partial class GRUPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GRUPO()
        {
            GRUPO_FORMA = new HashSet<GRUPO_FORMA>();
            GRUPO_CONTROL = new HashSet<GRUPO_CONTROL>();
            GRUPO_PANEL = new HashSet<GRUPO_PANEL>();
            USUARIO = new HashSet<USUARIO>();
            MANUAL = new HashSet<MANUAL>();
            MENSAJE = new HashSet<MENSAJE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_GRUPO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        public int? N_PADRE { get; set; }

        [StringLength(1000)]
        public string S_URL { get; set; }

        [StringLength(1)]
        public string REQUIERE_TERCERO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_FORMA> GRUPO_FORMA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_CONTROL> GRUPO_CONTROL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_PANEL> GRUPO_PANEL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO> USUARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MANUAL> MANUAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MENSAJE> MENSAJE { get; set; }
    }
}
