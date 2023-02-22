namespace SIM.Data.Poeca
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("POECA.DPOEAIR_SECTOR")]
    public partial class DPOEAIR_SECTOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_SECTOR()
        {
            TPOEAIR_SECTOR_MEDIDA = new HashSet<TPOEAIR_SECTOR_MEDIDA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Nombre del Sector")]
        public string S_NOMBRE { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string S_DESCRIPCION { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [Display(Name = "Medidas")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SECTOR_MEDIDA> TPOEAIR_SECTOR_MEDIDA { get; set; }
    }
}
