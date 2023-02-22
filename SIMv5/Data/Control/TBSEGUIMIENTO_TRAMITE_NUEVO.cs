namespace SIM.Data.Control
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.TBSEGUIMIENTO_TRAMITE_NUEVO")]
    public class TBSEGUIMIENTO_TRAMITE_NUEVO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("REPOSICION_ID")]
        public int REPOSICION_ID { get; set; }

        [Column("DOCUMENTO_ID")]
        public int DOCUMENTO_ID { get; set; }

        [Column("TRAMITE_ID")]
        public int TRAMITE_ID { get; set; }

        [Column("NUMERO_ACTO")]
        public string NUMERO_ACTO { get; set; }

        [Column("ANIO_ACTO")]
        public int ANIO_ACTO { get; set; }

        [Column("TECNICO")]
        public string TECNICO { get; set; }

        [Column("ESTADO")]
        public string ESTADO { get; set; }

        [Column("FECHA_SEGUIMIENTO")]
        public DateTime FECHA_SEGUIMIENTO { get; set; }


    }
}