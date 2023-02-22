namespace SIM.Data.Agua
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AGUA.TSIMTASA_CONSUMOS")]
    public class TSIMTASA_CONSUMOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }
        public decimal MENSUAL { get; set; }
        public decimal DIARIO { get; set; }
        public decimal ID_TERCERO { get; set; }
        public decimal TSIMTASA_MATERIAS_PRIMA_ID { get; set; }
        public decimal TSIMTASA_UNIDADES_ID { get; set; }

    }
}