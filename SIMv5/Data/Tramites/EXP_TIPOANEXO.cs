namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.EXP_TIPOANEXO")]
    public class EXP_TIPOANEXO
    {
        [Key]
        public decimal ID_TIPOANEXO { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_DESCRIPCION { get; set; }
    }
}