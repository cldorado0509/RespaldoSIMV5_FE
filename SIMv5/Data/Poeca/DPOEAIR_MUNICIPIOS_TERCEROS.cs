namespace SIM.Data.Poeca
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("POECA.DPOEAIR_MUNICIPIOS_TERCEROS")]
    public partial class DPOEAIR_MUNICIPIOS_TERCEROS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TERCERO { get; set; }

        public string S_NOMBRE { get; set; }
    }
}
