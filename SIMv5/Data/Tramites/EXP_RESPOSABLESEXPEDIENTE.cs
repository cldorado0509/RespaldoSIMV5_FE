namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_RESPOSABLESEXPEDIENTE")]
    public class EXP_RESPOSABLESEXPEDIENTE
    {
        [Key]
        public decimal ID_RESPONSABLE { get; set; }
        [ForeignKey("EXP_EXPEDIENTES")]
        public decimal ID_EXPEDIENTE { get; set; }
        [ForeignKey("TBFUNCIONARIOS")]
        public decimal ID_FUNCIONARIO { get; set; }
        public DateTime D_INICIA { get; set; }
        public DateTime D_FIN { get; set; }
        public virtual ICollection<EXP_EXPEDIENTES> EXP_EXPEDIENTES  { get; set; }
        public virtual ICollection<TBFUNCIONARIO> TBFUNCIONARIOS { get; set; }

    }
}