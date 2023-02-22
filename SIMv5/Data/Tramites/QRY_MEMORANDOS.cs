namespace SIM.Data.Tramites
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TRAMITES.QRY_MEMORANDOS")]
    public class QRY_MEMORANDOS
    {
        [Key]
        [Column(Order = 1)]
        public decimal CODDOCUMENTO { get; set; }
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }
        public decimal ID_DOCUMENTO { get; set; }
        public DateTime FECHADIGITALIZACION { get; set; }
        public decimal AGNO { get; set; }
        public string RADICADO { get; set; }
        public DateTime? FECHA { get; set; }
        public string ASUNTO { get; set; }
        public string DE { get; set; }
        public string PARA { get; set; }

    }
}