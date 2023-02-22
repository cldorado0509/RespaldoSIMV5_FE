namespace SIM.Data.Control
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.TBTRAMITES_SEGUIMIENTO")]
    public class TBTRAMITES_SEGUIMIENTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("CODTRAMITE")]
        public int CODTRAMITE { get; set; }

        [Column("CODDOCUMENTO")]
        public int CODDOCUMENTO { get; set; }

        [Column("CODIGO_SOLICITUD")]
        public int CODIGO_SOLICITUD { get; set; }

        [Column("RADICADO")]
        public string RADICADO { get; set; }

        [Column("ANIO")]
        public int ANIO { get; set; }

        [Column("CM")]
        public string CM { get; set; }

        [Column("SOLICITUD")]
        public string SOLICITUD { get; set; }

        [Column("TECNICO")]
        public string TECNICO { get; set; }

        [Column("APOYO")]
        public string APOYO { get; set; }

        [Column("RADICADO_SALIDA")]
        public string RADICADO_SALIDA { get; set; }

        [Column("PROYECTO")]
        public string PROYECTO { get; set; }

        [Column("SOLICITANTE")]
        public string SOLICITANTE { get; set; }

    }
}