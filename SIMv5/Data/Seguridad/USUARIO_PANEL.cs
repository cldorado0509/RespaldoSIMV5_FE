namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_PANEL")]
    public partial class USUARIO_PANEL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PANEL { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_NUEVO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_EDITAR { get; set; }

        [Required]
        [StringLength(4000)]
        public string S_ELIMINAR { get; set; }

        [Required]
        [StringLength(1)]
        public string S_BUSCAR { get; set; }

        [Required]
        [StringLength(1)]
        public string S_IMPRIMIR { get; set; }

        [Required]
        [StringLength(1)]
        public string S_REQUIERE_AUTORIZACION { get; set; }

        [ForeignKey("ID_PANEL")]
        public virtual PANEL PANEL { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
