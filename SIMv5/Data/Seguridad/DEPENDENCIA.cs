namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.DEPENDENCIA")]
    public partial class DEPENDENCIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPENDENCIA()
        {
            FUNCIONARIO_DEPENDENCIA = new HashSet<FUNCIONARIO_DEPENDENCIA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_DEPENDENCIA { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_ELIMINADO { get; set; }

        [Required]
        [StringLength(20)]
        public string S_ACTIVO { get; set; }

        public decimal? N_CODDEPENDENCIA { get; set; }

        [StringLength(2)]
        public string CODENTIDAD { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        public decimal? ID_DEPENDENCIAPADRE { get; set; }

        [StringLength(250)]
        public string S_VERSION { get; set; }

        [StringLength(10)]
        public string S_CODOFICINA { get; set; }

        [StringLength(1)]
        public string S_ORDENADORGASTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FUNCIONARIO_DEPENDENCIA> FUNCIONARIO_DEPENDENCIA { get; set; }
    }
}
