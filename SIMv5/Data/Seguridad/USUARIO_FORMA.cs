namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_FORMA")]
    public partial class USUARIO_FORMA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public string S_IMPRIMIR { get; set; }

        [StringLength(1)]
        public string S_ADJUNTAR { get; set; }

        [StringLength(4000)]
        public string S_WHERE { get; set; }

        [Required]
        [StringLength(1)]
        public string S_REQUIERE_AUTORIZACION { get; set; }

        [ForeignKey("ID_FORMA")]
        public virtual MENU MENU { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
