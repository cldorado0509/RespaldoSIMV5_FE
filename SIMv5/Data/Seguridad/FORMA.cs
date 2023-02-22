namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.FORMA")]
    public partial class FORMA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FORMA()
        {
            CAMPO = new HashSet<CAMPO>();
            GRUPO_FORMA = new HashSet<GRUPO_FORMA>();
            PANEL = new HashSet<PANEL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORMA { get; set; }

        public int ID_MODULO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(200)]
        public string S_URL { get; set; }

        [Required]
        [StringLength(128)]
        public string S_NUEVO { get; set; }

        [Required]
        [StringLength(128)]
        public string S_EDITAR { get; set; }

        [Required]
        [StringLength(128)]
        public string S_ELIMINAR { get; set; }

        [Required]
        [StringLength(128)]
        public string S_BUSCAR { get; set; }

        [Required]
        [StringLength(128)]
        public string S_IMPRIMIR { get; set; }

        [Required]
        [StringLength(128)]
        public string S_ADJUNTAR { get; set; }

        [Required]
        [StringLength(128)]
        public string S_AYUDAS { get; set; }

        [Required]
        [StringLength(128)]
        public string S_TIPO { get; set; }

        [StringLength(2000)]
        public string S_IMAGEN { get; set; }

        public int ORDEN { get; set; }

        public int? COD_FUNCIONARIO_RESPONSABLE { get; set; }

        [Required]
        [StringLength(1)]
        public string S_BLOQUEADO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_VISIBLE_MENU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPO> CAMPO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRUPO_FORMA> GRUPO_FORMA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PANEL> PANEL { get; set; }

        [ForeignKey("ID_MODULO")]
        public virtual MODULO MODULO { get; set; }
    }
}
