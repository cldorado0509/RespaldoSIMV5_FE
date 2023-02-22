namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTRAMITETAREA")]
    public partial class TBTRAMITETAREA
    {
        [Key]
        [Column(Order = 0)]
        public decimal CODTRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal CODTAREA { get; set; }

        public decimal? NIVEL { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal CODFUNCIONARIO { get; set; }

        public DateTime FECHAINI { get; set; }

        public DateTime? FECHAFIN { get; set; }

        public DateTime? FECHALIM { get; set; }

        public decimal? ESTADO { get; set; }

        [StringLength(1000)]
        public string COMENTARIO { get; set; }

        public decimal? DELEGADO { get; set; }

        public decimal? RECHAZADO { get; set; }

        public int? RECIBIDA { get; set; }

        public decimal? IMPORTANTE { get; set; }

        public decimal? CODIGO { get; set; }

        public decimal? CODTAREA_ANTERIOR { get; set; }

        public decimal? CODTAREA_SIGUIENTE { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int COPIA { get; set; }

        public decimal? TIEMPO_TAREA { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal ORDEN { get; set; }

        public decimal? DIAS_ACUMULADOS { get; set; }

        public decimal? HORAS_ACUMULADAS { get; set; }

        public decimal? MINUTOS_ACUMULADOS { get; set; }

        public decimal? DIAS_ACUMULADOS_HABILES { get; set; }

        public decimal? HORAS_ACUMULADAS_HABILES { get; set; }

        public decimal? MINUTOS_ACUMULADOS_HABILES { get; set; }

        [StringLength(200)]
        public string TABLA { get; set; }

        [StringLength(200)]
        public string CAMPO { get; set; }

        public decimal? CODIGO_REGISTRO { get; set; }

        [StringLength(1)]
        public string DEVOLUCION { get; set; }

        public int? CODGRUPOT { get; set; }

        [ForeignKey("CODTAREA")]
        public virtual TBTAREA TBTAREA { get; set; }

        [ForeignKey("CODTRAMITE")]
        public virtual TBTRAMITE TBTRAMITE { get; set; }
    }
}
