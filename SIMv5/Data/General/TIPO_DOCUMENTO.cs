namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.TIPO_DOCUMENTO")]
    public partial class TIPO_DOCUMENTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_DOCUMENTO()
        {
            TERCERO = new HashSet<TERCERO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPODOCUMENTO { get; set; }

        [Required]
        [StringLength(50)]
        public string S_NOMBRE { get; set; }

        [StringLength(10)]
        public string S_ABREVIATURA { get; set; }

        [StringLength(200)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1)]
        public string S_TIPOPERSONA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO> TERCERO { get; set; }
    }
}
