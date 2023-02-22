namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.MAE_CONCEPTOS_COMERCIAL")]
    public partial class MAE_CONCEPTOS_COMERCIAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAE_CONCEPTOS_COMERCIAL()
        {
            MAE_CONCEPTOS_COMERCIAL1 = new HashSet<MAE_CONCEPTOS_COMERCIAL>();
        }

        [Required]
        [StringLength(50)]
        public string CODIGO_CONCEPTO { get; set; }

        public int? CODIGO_CONCEPTO_INGRESO { get; set; }

        [Required]
        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        [Required]
        [StringLength(1)]
        public string TIPO { get; set; }

        public int? CODIGO_PLANTILLA { get; set; }

        public int? CODIGO_RUBRO { get; set; }

        [Required]
        [StringLength(1)]
        public string CRUCE_CUENTAS { get; set; }

        public int? CODIGO_CONCEPTO_CONTABLE { get; set; }

        public int? CODIGO_TIPO_CONTABLE { get; set; }

        public int? CODIGO_ARTICULO { get; set; }

        [Required]
        [StringLength(1)]
        public string SIGNO { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_DISPONIBILIDAD { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_COMPROMISO { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_OPAGO { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_COMPROBANTE { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_FACTURA { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_INGRESO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_INTERNO { get; set; }

        [Required]
        [StringLength(1)]
        public string BORRADO { get; set; }

        public int? CODIGO_TIPO_DOC_DISP { get; set; }

        public int? CODIGO_TIPO_DOC_COMP { get; set; }

        public int? CODIGO_CONCEPTO_REF { get; set; }

        public int? CODIGO_RUBRO_RESERVA { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERA_PAGO { get; set; }

        [Required]
        [StringLength(1)]
        public string DESCUENTA_BASE { get; set; }

        public int? CODIGO_DEPARTAMENTO { get; set; }

        public int? CODIGO_CIUDAD { get; set; }

        [StringLength(1)]
        public string ESTADO_DISPONIBILIDAD { get; set; }

        [StringLength(1)]
        public string ESTADO_COMPROMISO { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        public decimal? ASOCIA_DOC { get; set; }

        public decimal? CODIGO_TERCERO { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [StringLength(1)]
        public string TIPO_NUMERACIÓN { get; set; }

        [StringLength(1)]
        public string TIPO_NUMERACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAE_CONCEPTOS_COMERCIAL> MAE_CONCEPTOS_COMERCIAL1 { get; set; }

        public virtual MAE_CONCEPTOS_COMERCIAL MAE_CONCEPTOS_COMERCIAL2 { get; set; }
    }
}
