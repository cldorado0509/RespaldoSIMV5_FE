namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_TIPOESTADO")]
    public class EXP_TIPOESTADO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_ESTADO { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_DESCRIPCION { get; set; }
        public string S_ESTADOINICIAL { get; set; }
        public string S_ESTADOCIERRE { get; set; }
    }
}