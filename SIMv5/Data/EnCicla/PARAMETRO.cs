namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.PARAMETRO")]
    public partial class PARAMETRO_EN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PARAMETRO { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        public int ID_TIPOPARAMETRO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_VALOR { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }

        [ForeignKey("ID_TIPOPARAMETRO")]
        public virtual TIPO_PARAMETRO TIPO_PARAMETRO { get; set; }
    }
}
