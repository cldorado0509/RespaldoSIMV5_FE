namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_CONTROLES")]
    public partial class USUARIO_CONTROLES
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTROL { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }

        [StringLength(1)]
        public string S_HABILITADO { get; set; }

        [ForeignKey("ID_CONTROL")]
        public virtual CONTROL CONTROL { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
