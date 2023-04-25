namespace SIM.Data.Control
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.TBREPOSICION")]
    public class TBREPOSICION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Column("CODIGO_SOLICITUD")]
        public decimal CODIGO_SOLICITUD { get; set; }

        [Column("CODIGO_ACTOADMINISTRATIVO")]
        public decimal CODIGO_ACTOADMINISTRATIVO { get; set; }

        [Column("TALA_SOLICITADA")]
        public decimal? TALA_SOLICITADA { get; set; }

        [Column("TALA_AUTORIZADO")]
        public decimal? TALA_AUTORIZADO { get; set; }

        [Column("DAP_MEN_10_SOLICITADO")]
        public decimal? DAP_MEN_10_SOLICITADO { get; set; }

        [Column("DAP_MEN_10_AUTORIZADO")]
        public decimal? DAP_MEN_10_AUTORIZADO { get; set; }
                
        [Column("VOLUMEN_AUTORIZADO")]
        public decimal? VOLUMEN_AUTORIZADO { get; set; }
                
        [Column("TRASPLANTE_SOLICITADO")]
        public decimal? TRASPLANTE_SOLICITADO { get; set; }

        [Column("TRASPLANTE_AUTORIZADO")]
        public decimal? TRASPLANTE_AUTORIZADO { get; set; }
        
        [Column("PODA_SOLICITADO")]
        public decimal? PODA_SOLICITADO { get; set; }

        [Column("PODA_AUTORIZADO")]
        public decimal? PODA_AUTORIZADO { get; set; }

        [Column("CONSERVACION_SOLICITADO")]
        public decimal? CONSERVACION_SOLICITADO { get; set; }
        
        [Column("CONSERVACION_AUTORIZADO")]
        public decimal? CONSERVACION_AUTORIZADO { get; set; }

        [Column("REPOSICION_AUTORIZADO")]
        public decimal? REPOSICION_AUTORIZADO { get; set; }

        [Column("REPOSICION_PROPUESTA")]
        public decimal? REPOSICION_PROPUESTA { get; set; }

        [Column("REPOSICION_MINIMA_OBLIGATORIA")]
        public decimal? REPOSICION_MINIMA_OBLIGATORIA { get; set; }

        [Column("TIPO_MEDIDAID")]
        public decimal? TIPO_MEDIDAID { get; set; }

        [Column("AUTORIZADO")]
        public decimal? AUTORIZADO { get; set; }
        
        [Column("OBSERVACIONES")]
        public string OBSERVACIONES { get; set; }

        [Column("CM")]
        public string CM { get; set; }

        [Column("ASUNTO")]
        public string ASUNTO { get; set; }

        [Column("PROYECTO")]
        public string PROYECTO { get; set; }

        [Column("COORDENADAX")]
        public float COORDENADAX { get; set; }

        [Column("COORDENADAY")]
        public float COORDENADAY { get; set; }

        [Column("TIPO_MEDIDAADICIONAL_ID")]
        public decimal? TIPO_MEDIDAADICIONAL_ID { get; set; }

        [Column("MEDIDA_ADICIONAL_ASIGNADA")]
        public decimal? MEDIDA_ADICIONAL_ASIGNADA { get; set; }

        [Column("NRO_LENIOS_SOLICITADOS")]
        public decimal? NRO_LENIOS_SOLICITADOS { get; set; }

        [Column("NRO_LENIOS_AUTORIZADOS")]
        public decimal? NRO_LENIOS_AUTORIZADOS { get; set; }

        [Column("VALORACION_INVENTARIO_FORESTAL")]
        public decimal? VALORACION_INVENTARIO_FORESTAL { get; set; }

        [Column("VALORACION_TALA")]
        public decimal? VALORACION_TALA { get; set; }

        [Column("INVERSION_REPOSICION_MINIMA")]
        public decimal? INVERSION_REPOSICION_MINIMA { get; set; }

        [Column("INVERSION_MEDIDAS_ADICIONALES")]
        public decimal? INVERSION_MEDIDAS_ADICIONALES { get; set; }

        [Column("CANTIDAD_SIEMBRA_ADICIONAL")]
        public decimal? CANTIDAD_SIEMBRA_ADICIONAL { get; set; }

        [Column("INVERSION_MEDIDA_ADICIONAL_SIEMBRA")]
        public decimal? INVERSION_MEDIDA_ADICIONAL_SIEMBRA { get; set; }

        [Column("CANTIDAD_MANTENIMIENTO")]
        public decimal? CANTIDAD_MANTENIMIENTO { get; set; }

        [Column("INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO")]
        public decimal? INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO { get; set; }

        [Column("CANTIDAD_DESTOCONADO")]
        public decimal? CANTIDAD_DESTOCONADO { get; set; }

        [Column("INVERSION_MEDIDA_ADICIONAL_DESTOCONADO")]
        public decimal? INVERSION_MEDIDA_ADICIONAL_DESTOCONADO { get; set; }

        [Column("CANTIDAD_LEVANTAMIENTO_PISO")]
        public decimal? CANTIDAD_LEVANTAMIENTO_PISO { get; set; }

        [Column("INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO")]
        public decimal? INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO { get; set; }

        [Column("PAGO_FONDO_VERDE_METROPOLITANO")]
        public decimal? PAGO_FONDO_VERDE_METROPOLITANO { get; set; }

        [Column("ES_TRAMITE_NUEVO")]
        public string ES_TRAMITE_NUEVO { get; set; }

        [Column("NOMBRE_PROYECTO")]
        public string NOMBRE_PROYECTO { get; set; }

        [Column("CODIGO_TRAMITE")]
        public string CODIGO_TRAMITE { get; set; }

        [Column("CODIGO_DOCUMENTO")]
        public decimal? CODIGO_DOCUMENTO { get; set; }


    }
}