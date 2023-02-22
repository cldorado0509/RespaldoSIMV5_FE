namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_BUSQUEDA")]
    public class EXP_BUSQUEDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_BUSQUEDAEXP { get; set; }
        public decimal COD_SERIE { get; set; }
        public string S_INDICE { get; set; }
        public DateTime FECHADIGITALIZA { get; set; }
        public decimal ID_EXPEDIENTE { get; set; }
    }
}