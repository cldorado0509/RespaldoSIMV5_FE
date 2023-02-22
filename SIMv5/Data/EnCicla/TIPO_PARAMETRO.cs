namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.TIPO_PARAMETRO")]
    public partial class TIPO_PARAMETRO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_PARAMETRO()
        {
            PARAMETRO = new HashSet<PARAMETRO_EN>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOPARAMETRO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_EN> PARAMETRO { get; set; }
    }
}
