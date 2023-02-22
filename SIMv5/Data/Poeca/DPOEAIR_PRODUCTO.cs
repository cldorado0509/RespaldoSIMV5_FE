namespace SIM.Data.Poeca
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("POECA.DPOEAIR_PRODUCTO")]
    public partial class DPOEAIR_PRODUCTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DPOEAIR_PRODUCTO()
        {
            TPOEAIR_ACCIONES_PLAN = new HashSet<TPOEAIR_ACCIONES_PLAN>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Nombre del producto")]
        public string S_NOMBRE_PRODUCTO { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string S_DESCRIPCION { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }
    }
}
