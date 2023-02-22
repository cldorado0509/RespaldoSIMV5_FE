namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TIPO_INSTALACION")]
    public partial class TIPO_INSTALACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_INSTALACION()
        {
            TIPO_INSTALACION1 = new HashSet<TIPO_INSTALACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOINSTALACION { get; set; }

        public int? ID_TIPOINSTALACIONPADRE { get; set; }

        [Required]
        [StringLength(100)]
        public string S_NOMBRE { get; set; }

        [Required]
        [StringLength(50)]
        public string S_CODIGO { get; set; }

        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_PRINCIPAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIPO_INSTALACION> TIPO_INSTALACION1 { get; set; }

        [ForeignKey("ID_TIPOINSTALACIONPADRE")]
        public virtual TIPO_INSTALACION TIPO_INSTALACION2 { get; set; }
    }
}
