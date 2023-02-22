namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.TERCERO_HISTORICO")]
    public partial class TERCERO_HISTORICO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCEROHISTORICO { get; set; }

        public int ID_TERCEROROL { get; set; }

        public int ID_TERCEROESTADO { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime D_FIN { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_ROL { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [ForeignKey("ID_ROL")]
        public virtual ROL_EN ROL { get; set; }

        [ForeignKey("ID_TERCEROESTADO")]
        public virtual TERCERO_ESTADO_EN TERCERO_ESTADO { get; set; }

        [ForeignKey("ID_TERCEROROL")]
        public virtual TERCERO_ROL TERCERO_ROL { get; set; }
    }
}
