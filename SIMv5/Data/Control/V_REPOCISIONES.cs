namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.V_REPOCISIONES")]
    public class V_REPOCISIONES
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public int CODIGO_SOLICITUD { get; set; }
        public int CODIGO_ACTOADMINISTRATIVO { get; set; }
        public int? TALA_AUTORIZADO { get; set; }
        public int? DAP_MEN_10_AUTORIZADO { get; set; }
        public float? VOLUMEN_AUTORIZADO { get; set; }
        public int? TRASPLANTE_AUTORIZADO { get; set; }
        public int? PODA_AUTORIZADO { get; set; }
        public int? CONSERVACION_AUTORIZADO { get; set; }
        public int? REPOSICION_AUTORIZADO { get; set; }
        public int? TIPO_MEDIDAID { get; set; } 
        public int? AUTORIZADO { get; set; }
        public int? IDDETAIL     { get; set; }
        public string OBSERVACIONES { get; set; }
        public string CM { get; set; }
        public string ASUNTO { get; set; }
        public float? COORDENADAX { get; set; }
        public float? COORDENADAY { get; set; }
        public int? TIPO_MEDIDAADICIONAL_ID { get; set; }
        public string TIPOMEDIDAADICIONAL { get; set; }
        public float? MEDIDA_ADICIONAL_ASIGNADA { get; set; }
        public string PROYECTO { get; set; }
        public int? NRO_LENIOS_SOLICITADOS { get; set; }
        public int? VALORACION_INVENTARIO_FORESTAL { get; set; }
        public int? VALORACION_TALA { get; set; }
        public int? INVERSION_REPOSICION_MINIMA { get; set; }
        public int? INVERSION_MEDIDAS_ADICIONALES { get; set; }
        public int?  INVERSION_MEDIDA_ADICIONAL_SIEMBRA { get; set; }
        public int? CANTIDAD_MANTENIMIENTO { get; set; }
        public int? INVERSION_MEDIDA_ADICIONAL_MANTENIMIENTO { get; set; }
        public int? CANTIDAD_DESTOCONADO { get; set; }
        public int? INVERSION_MEDIDA_ADICIONAL_DESTOCONADO { get; set; }
        public int? CANTIDAD_LEVANTAMIENTO_PISO { get; set; }
        public int? INVERSION_MEDIDA_ADICIONAL_LEVANTAMIENTO_PISO { get; set; }
        public int? PAGO_FONDO_VERDE_METROPOLITANO { get; set; }
        public string ES_TRAMITE_NUEVO { get; set; }
        public int? REPOSICION_PROPUESTA { get; set; }
        public int? REPOSICION_MINIMA_OBLIGATORIA { get; set; }
        public int? NRO_LENIOS_AUTORIZADOS { get; set; }
        public int? TALA_SOLICITADA { get; set; }
        public int? DAP_MEN_10_SOLICITADO { get; set; }
        public int? TRASPLANTE_SOLICITADO { get; set; }
        public int? PODA_SOLICITADO { get; set; }
        public int? CONSERVACION_SOLICITADO { get; set; }
        public string NOMBRE_PROYECTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_ACTO { get; set; }
        public DateTime? FECHA_ACTO { get; set; }
        public int? ANIO_ACTO { get; set; }
        public int? TALA_EJECUTADA { get; set; }
        public int? DAP_MEN_10_EJECUTADA { get; set; }
        public int? VOLUMEN_EJECUTADO { get; set; }
        public int? TRASPLANTE_EJECUTADO { get; set; }
        public int? PODA_EJECUTADA { get; set; }
        public int? CONSERVACION_EJECUTADA { get; set; }
        public int? REPOSICION_EJECUTADA { get; set; }
        public float? MEDIDA_ADICIONAL_EJECUTADA { get; set; }
        public DateTime? FECHA_CONTROL { get; set; }
        public int? ID_USUARIOVISITA { get; set; }
        public string OBSERVACIONVISITA { get; set; }
        public DateTime? FECHAVISITA { get; set; }
        public string RADICADOVISITA { get; set; }
        public DateTime? FECHA_RADICADO_VISITA { get; set; }
        public string DIRECCION { get; set; }
        public string NOMBREPROYECTO { get; set; }
        public string ENTIDAD_PUBLICA { get; set; }
        public string CODIGO_TRAMITE { get; set; }
        public string MUNICIPIO { get; set; }
        public string TECNICO { get; set; }
      

    }
}