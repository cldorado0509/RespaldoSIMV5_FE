namespace SIM.Data.Seguridad
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SEGURIDAD.ROL_FORMA")]
    public partial class ROL_FORMA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ROL_FORMA { get; set; }

        public int ID_ROL { get; set; }

        public int ID_FORMA { get; set; }

        [StringLength(1)]
        public string S_NUEVO { get; set; }

        [StringLength(1)]
        public string S_EDITAR { get; set; }

        [StringLength(1)]
        public string S_ELIMINAR { get; set; }

        [StringLength(1)]
        public string S_BUSCAR { get; set; }

        [StringLength(1)]
        public string S_ADMINISTRADOR { get; set; }

        [StringLength(1)]
        public string S_GENERAR_DOCUMENTO { get; set; }

        [ForeignKey("ID_ROL")]
        public virtual ROL ROL { get; set; }

        [ForeignKey("ID_FORMA")]
        public virtual MENU MENU { get; set; }
    }
}
