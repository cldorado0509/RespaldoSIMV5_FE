namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_ESTADOSEXPEDIENTE")]
    public class EXP_ESTADOSEXPEDIENTE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_ESTADOEXPEDIENTE { get; set; }
        public decimal ID_ESTADO { get; set; }
        public decimal ID_EXPEDIENTE { get; set; }
        public DateTime D_INICIA { get; set; }
        public DateTime? D_FIN { get; set; }
        public decimal ID_FUNCIONARIOESTADO { get; set; }
    }
}