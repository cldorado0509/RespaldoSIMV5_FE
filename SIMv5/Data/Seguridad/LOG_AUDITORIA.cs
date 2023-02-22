namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.LOG_AUDITORIA")]
    public partial class LOG_AUDITORIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_LOG_AUDITORIA { get; set; }

        public int ID_USUARIO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [StringLength(30)]
        public string S_TABLA { get; set; }

        [StringLength(100)]
        public string S_ID_CAMPO { get; set; }

        [StringLength(100)]
        public string S_ID_VALOR { get; set; }

        [StringLength(30)]
        public string S_CAMPO { get; set; }

        [StringLength(4000)]
        public string S_OLD_VALOR { get; set; }

        [StringLength(4000)]
        public string S_NEW_VALOR { get; set; }
    }
}
