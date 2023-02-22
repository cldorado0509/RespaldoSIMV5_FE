namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;


    [Table("TRAMITES.BUSQUEDA_DOCUMENTO")]
    public class BUSQUEDA_DOCUMENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_BUSQUEDADOC { get; set; }
        public decimal COD_SERIE { get; set; }
        public decimal? COD_DOCUMENTO { get; set; }
        public string S_INDICE { get; set; }
        public decimal? COD_TRAMITE { get; set; }
        public DateTime? FECHADIGITALIZA { get; set; }
        public decimal? ID_DOCUMENTO { get; set; }
        public decimal? ID_EXPEDIENTE { get; set; }
    }
}