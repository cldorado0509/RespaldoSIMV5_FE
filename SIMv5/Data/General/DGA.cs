namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DGA")]
    public partial class DGA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DGA()
        {
            PERSONAL_DGA = new HashSet<PERSONAL_DGA>();
            PERMISO_AMBIENTAL = new HashSet<PERMISO_AMBIENTAL>();

            S_COMPARTEDGA = "N";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DGA { get; set; }

        public int ID_TERCERO { get; set; }

        public DateTime D_ANO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESSGA { get; set; }

        [StringLength(4000)]
        public string S_SGA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESSGC { get; set; }

        [StringLength(4000)]
        public string S_SGC { get; set; }

        [Required]
        [StringLength(1)]
        public string S_PRODUCCIONMASLIMPIA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESECOETIQUETADO { get; set; }

        [StringLength(4000)]
        public string S_ECOETIQUETADO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_COMPARTEDGA { get; set; }

        [StringLength(500)]
        public string S_COMPARTEEMPRESA { get; set; }

        [Required]
        [StringLength(1)]
        public string S_AGREMIACION { get; set; }

        [StringLength(500)]
        public string S_AGREMIACIONASESORIA { get; set; }

        public long? N_ACTIVO { get; set; }

        [StringLength(500)]
        public string S_FILIAL { get; set; }

        [StringLength(4000)]
        public string S_FUNCION { get; set; }

        [StringLength(500)]
        public string S_ORGANIGRAMA { get; set; }

        public DateTime? D_FREPORTE { get; set; }

        public int? N_EMPLEADOS { get; set; }

        [StringLength(4000)]
        public string S_SEGUIMIENTO { get; set; }

        public int? ID_ESTADO { get; set; }

        public long? N_INGRESOS { get; set; }

        public int? N_VERSION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERSONAL_DGA> PERSONAL_DGA { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual ESTADO ESTADO { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERMISO_AMBIENTAL> PERMISO_AMBIENTAL { get; set; }
    }
}
