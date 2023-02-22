namespace SIM.Data.Control
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.VISITA_SOLICITUD")]
    public class VISITA_SOLICITUD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("ID_DETALLE_REPOSICION")]
        public int ID_DETALLE_REPOSICION { get; set; }

        [Column("ID_VISITA")]
        public int ID_VISITA { get; set; }
    }
}