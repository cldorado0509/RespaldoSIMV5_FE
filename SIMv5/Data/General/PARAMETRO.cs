namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PARAMETRO")]
    public partial class PARAMETRO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PARAMETRO()
        {
            PARAMETRO1 = new HashSet<PARAMETRO>();
            PERIODOBALANCE_PARAMETRO = new HashSet<PERIODOBALANCE_PARAMETRO>();
            PARAMETRO_INSTALACION = new HashSet<PARAMETRO_INSTALACION>();
            PARAMETRO_TERCERO = new HashSet<PARAMETRO_TERCERO>();
            PARAMETRO_TERCEROINSTALACION = new HashSet<PARAMETRO_TERCEROINSTALACION>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PARAMETRO { get; set; }

        public int? ID_PARAMETROPADRE { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(500)]
        public string S_DESCRIPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO> PARAMETRO1 { get; set; }

        [ForeignKey("ID_PARAMETROPADRE")]
        public virtual PARAMETRO PARAMETRO2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERIODOBALANCE_PARAMETRO> PERIODOBALANCE_PARAMETRO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_INSTALACION> PARAMETRO_INSTALACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_TERCERO> PARAMETRO_TERCERO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARAMETRO_TERCEROINSTALACION> PARAMETRO_TERCEROINSTALACION { get; set; }
    }
}
