namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO")]
    public partial class USUARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USUARIO()
        {
            PERMISO_OBJETO = new HashSet<PERMISO_OBJETO>();
            PROPIETARIO = new HashSet<PROPIETARIO>();
            USUARIO_CONTROLES = new HashSet<USUARIO_CONTROLES>();
            USUARIO_FORMA = new HashSet<USUARIO_FORMA>();
            USUARIO_PANEL = new HashSet<USUARIO_PANEL>();
            USUARIO_FUNCIONARIO = new HashSet<USUARIO_FUNCIONARIO>();
            USUARIO_ROL = new HashSet<USUARIO_ROL>();
            NOTICIA = new HashSet<NOTICIA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_USUARIO { get; set; }

        public int? ID_GRUPO { get; set; }

        [Required]
        [StringLength(200)]
        public string S_APELLIDOS { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRES { get; set; }

        [Required]
        [StringLength(100)]
        public string S_LOGIN { get; set; }

        [StringLength(100)]
        public string S_PASSWORDOLD { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESTADO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_SUPERUSUARIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        public DateTime? D_VENCE { get; set; }

        [StringLength(200)]
        public string S_EMAIL { get; set; }

        [StringLength(100)]
        public string S_PASSWORD { get; set; }

        public decimal? N_SALTOS { get; set; }

        [StringLength(100)]
        public string S_VALIDADOR { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERMISO_OBJETO> PERMISO_OBJETO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROPIETARIO> PROPIETARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_CONTROLES> USUARIO_CONTROLES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_FORMA> USUARIO_FORMA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_PANEL> USUARIO_PANEL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_FUNCIONARIO> USUARIO_FUNCIONARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_ROL> USUARIO_ROL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTICIA> NOTICIA { get; set; }
    }
}
