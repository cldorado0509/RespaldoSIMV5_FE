namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.MENU")]
    public partial class MENU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MENU()
        {
            USUARIO_FORMA = new HashSet<USUARIO_FORMA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FORMA { get; set; }

        public int ID_PADRE { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(200)]
        public string S_ARCHIVO { get; set; }

        [StringLength(1)]
        public string S_NUEVO { get; set; }

        [StringLength(1)]
        public string S_EDITAR { get; set; }

        [StringLength(1)]
        public string S_ELIMINAR { get; set; }

        [StringLength(1)]
        public string S_BUSCAR { get; set; }

        [StringLength(1)]
        public string S_IMPRIMIR { get; set; }

        [StringLength(1)]
        public string S_ADJUNTAR { get; set; }

        [StringLength(1)]
        public string S_AYUDAS { get; set; }

        public int ORDEN { get; set; }

        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [StringLength(1)]
        public string S_VISIBLE_MENU { get; set; }

        [StringLength(30)]
        public string S_CONTROLADOR { get; set; }

        [StringLength(200)]
        public string S_RUTA { get; set; }

        [StringLength(1)]
        public string S_VERSION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_FORMA> USUARIO_FORMA { get; set; }
    }
}
