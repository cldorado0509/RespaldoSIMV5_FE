namespace SIM.Data.Control
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.TBDETALLE_REPOSICION")]
    public class TBDETALLE_REPOSICION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("REPOSICION_ID")]
        public int REPOSICION_ID { get; set; }

        [Column("TIPO_DOCUMENTO")]
        public string TIPO_DOCUMENTO { get; set; }

        [Column("DOCUMENTO_ID")]
        public int DOCUMENTO_ID { get; set; }

        [Column("NUMERO_ACTO")]
        public string NUMERO_ACTO { get; set; }

        [Column("FECHA_ACTO")]
        public DateTime FECHA_ACTO { get; set; }

        [Column("ANIO_ACTO")]
        public int ANIO_ACTO { get; set; }

        [Column("TALA_EJECUTADA")]
        public int TALA_EJECUTADA { get; set; }

        [Column("DAP_MEN_10_EJECUTADA")]
        public int DAP_MEN_10_EJECUTADA { get; set; }

        [Column("VOLUMEN_EJECUTADO")]
        public float VOLUMEN_EJECUTADO { get; set; }

        [Column("TRASPLANTE_EJECUTADO")]
        public int TRASPLANTE_EJECUTADO { get; set; }

        [Column("PODA_EJECUTADA")]
        public int PODA_EJECUTADA { get; set; }

        [Column("CONSERVACION_EJECUTADA")]
        public int CONSERVACION_EJECUTADA { get; set; }

        [Column("REPOSICION_EJECUTADA")]
        public int REPOSICION_EJECUTADA { get; set; }

        [Column("MEDIDA_ADICIONAL_EJECUTADA")]
        public float MEDIDA_ADICIONAL_EJECUTADA { get; set; }

        [Column("FECHA_CONTROL")]
        public DateTime FECHA_CONTROL { get; set; }

        [Column("ID_USUARIOVISITA")]
        public int? ID_USUARIOVISITA { get; set; }

        [Column("OBSERVACIONES_VISITA")]
        public string OBSERVACIONES_VISITA { get; set; }

        [Column("FECHA_VISITA")]
        public DateTime? FECHA_VISITA { get; set; }

        [Column("RADICADO_VISITA")]
        public string RADICADO_VISITA { get; set; }

        [Column("FECHA_RADICADO_VISITA")]
        public DateTime? FECHA_RADICADO_VISITA { get; set; }

        [Column("CODIGO_TRAMITE")]
        public string CODIGO_TRAMITE { get; set; }
    }
}