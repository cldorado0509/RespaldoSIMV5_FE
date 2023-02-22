namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.ACTIVIDAD_ECONOMICA")]
    public partial class ACTIVIDAD_ECONOMICA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACTIVIDAD_ECONOMICA()
        {
            TERCERO = new HashSet<TERCERO>();
            //ACTIVIDAD_ECONOMICA1 = new HashSet<ACTIVIDAD_ECONOMICA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ACTIVIDADECONOMICA { get; set; }

        public int? ID_ACTIVIDADECONOMICAPADRE { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(50)]
        public string S_CODIGO { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(5)]
        public string S_VERSION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TERCERO> TERCERO { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACTIVIDAD_ECONOMICA> ACTIVIDAD_ECONOMICA1 { get; set; }

        public virtual ACTIVIDAD_ECONOMICA ACTIVIDAD_ECONOMICA2 { get; set; }*/
    }
}
