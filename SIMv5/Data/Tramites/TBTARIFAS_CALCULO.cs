namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTARIFAS_CALCULO")]
    public partial class TBTARIFAS_CALCULO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_CALCULO { get; set; }

        [Required]
        [StringLength(20)]
        public string NIT { get; set; }

        public decimal CODIGO_TRAMITE { get; set; }

        public decimal NRO_TECNICOS { get; set; }

        public decimal VALOR_PROYECTO { get; set; }

        public decimal COSTOS_A { get; set; }

        public decimal COSTOS_B { get; set; }

        public decimal? COSTOS_C { get; set; }

        public decimal COSTOS_D { get; set; }

        public decimal TOPES { get; set; }

        public decimal VALOR { get; set; }

        public decimal ID_USR { get; set; }

        public DateTime FECHA { get; set; }

        [StringLength(50)]
        public string VALIDADOR { get; set; }

        [Required]
        [StringLength(1)]
        public string TIPO { get; set; }

        [StringLength(128)]
        public string SESION { get; set; }

        [StringLength(20)]
        public string CM { get; set; }

        [StringLength(2000)]
        public string OBSERVACION { get; set; }

        public decimal NRO_TRAMITES { get; set; }

        public decimal NRO_ITEMS { get; set; }

        [Required]
        [StringLength(1)]
        public string SOPORTES { get; set; }

        [Required]
        [StringLength(1)]
        public string RELIQUIDACION { get; set; }

        public decimal? ID_CALCULOREL { get; set; }

        public decimal SALDO_USUARIO { get; set; }

        public decimal SALDO_ENTIDAD { get; set; }

        public decimal VALOR_REL { get; set; }

        public decimal INFORME { get; set; }

        public decimal N_VISITAS { get; set; }

        public decimal COSTOS_A_UNI { get; set; }

        public decimal VISITA { get; set; }

        [StringLength(1)]
        public string S_DIGITO { get; set; }

        [StringLength(100)]
        public string S_TERCERO { get; set; }

        [StringLength(100)]
        public string S_DIRECCIONTERCERO { get; set; }

        public int? ID_CIUDAD { get; set; }

        [StringLength(100)]
        public string S_CIUDAD { get; set; }

        public int? ID_DEPARTAMENTO { get; set; }

        [StringLength(100)]
        public string S_DEPARTAMENTO { get; set; }

        [StringLength(100)]
        public string S_TELEFONOTERCERO { get; set; }

        public int? N_CONSECUTIVO { get; set; }

        public DateTime? D_ELABORACION { get; set; }

        public DateTime? D_PAGO { get; set; }

        public int? ID_TIPOFACTURA { get; set; }

        [StringLength(100)]
        public string S_TIPOFACTURA { get; set; }

        [StringLength(1)]
        public string S_EXISTESICOF { get; set; }

        [StringLength(200)]
        public string S_TRAMITE { get; set; }

        [StringLength(50)]
        public string S_NOMBRE1 { get; set; }

        [StringLength(50)]
        public string S_NOMBRE2 { get; set; }

        [StringLength(50)]
        public string S_APELLIDO1 { get; set; }

        [StringLength(50)]
        public string S_APELLIDO2 { get; set; }

        public int? PUBLICACION { get; set; }

        public decimal? CODTRAMITE { get; set; }

        public decimal? CODDOCUMENTO { get; set; }
        public decimal? NRO_LINEAS { get; set; }
        public decimal? NRO_NORMAS { get; set; }
    }
}
