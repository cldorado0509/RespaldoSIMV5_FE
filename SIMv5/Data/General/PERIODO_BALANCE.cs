namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PERIODO_BALANCE")]
    public partial class PERIODO_BALANCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PERIODO_BALANCE()
        {
            PERIODOBALANCE_PARAMETRO = new HashSet<PERIODOBALANCE_PARAMETRO>();
            USO_CPC = new HashSet<USO_CPC>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PERIODOBALANCE { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_INSTALACION { get; set; }

        public DateTime D_PBINICIO { get; set; }

        public DateTime D_PBFIN { get; set; }

        public DateTime D_DILIGENCIA { get; set; }

        public int? ID_CONTACTO { get; set; }

        public int ID_ESTADO { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_ESTADO")]
        public virtual ESTADO ESTADO { get; set; }

        [ForeignKey("ID_INSTALACION")]
        public virtual INSTALACION INSTALACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERIODOBALANCE_PARAMETRO> PERIODOBALANCE_PARAMETRO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USO_CPC> USO_CPC { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
