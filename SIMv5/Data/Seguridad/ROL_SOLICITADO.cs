namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.ROL_SOLICITADO")]
    public partial class ROL_SOLICITADO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ROL_SOLICITADO { get; set; }

        public int ID_USUARIO { get; set; }

        [StringLength(100)]
        public string S_ROLES_SOL { get; set; }

        public DateTime? D_FECHA_SOL { get; set; }

        [StringLength(1)]
        public string S_ESTADO { get; set; }

        public int? ID_USUARIO_ADM { get; set; }

        [StringLength(100)]
        public string S_ROLES_ASIG { get; set; }

        public DateTime? D_FECHA_ASIG_RECHAZO { get; set; }

        public int? ID_TERCERO { get; set; }

        [StringLength(1)]
        public string S_ADMINISTRADOR { get; set; }

        public long? N_DOCUMENTO { get; set; }

        [StringLength(1000)]
        public string S_RSOCIAL { get; set; }

        public int? CODTRAMITE { get; set; }

        [StringLength(1024)]
        public string S_COMENTARIOS { get; set; }
    }
}
