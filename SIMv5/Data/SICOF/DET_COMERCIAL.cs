namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.DET_COMERCIAL")]
    public partial class DET_COMERCIAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DET_COMERCIAL()
        {
            DET_COMERCIAL1 = new HashSet<DET_COMERCIAL>();
        }

        [Required]
        [StringLength(50)]
        public string CODIGO_CONCEPTO { get; set; }

        public long VALOR { get; set; }

        public decimal NIT { get; set; }

        [Required]
        [StringLength(30)]
        public string CCOSTOS { get; set; }

        public DateTime FECHA_PAGO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CONSECUTIVO { get; set; }

        public int CODIGO_COMERCIAL { get; set; }

        public int NUMERO_DOCUMENTO { get; set; }

        public int? VALOR2 { get; set; }

        public int? CONSECUTIVO_REF { get; set; }

        public decimal? NIT2 { get; set; }

        [Required]
        [StringLength(4)]
        public string TIPO { get; set; }

        public int SEQ { get; set; }

        [StringLength(50)]
        public string NOMBRE_TERCERO { get; set; }

        public int? NO_COMPROMISO { get; set; }

        public int? SEQ_REF { get; set; }

        [StringLength(20)]
        public string APELLIDO1 { get; set; }

        [StringLength(20)]
        public string APELLIDO2 { get; set; }

        [StringLength(50)]
        public string NOMBRE1 { get; set; }

        [StringLength(20)]
        public string NOMBRE2 { get; set; }

        [StringLength(50)]
        public string DIRECCION { get; set; }

        [StringLength(50)]
        public string TELEFONO { get; set; }

        [StringLength(10)]
        public string CODIGO_CIUDAD { get; set; }

        [StringLength(10)]
        public string CODIGO_DEPARTAMENTO { get; set; }

        [Required]
        [StringLength(1)]
        public string TERCERO { get; set; }

        public int? NO_OPAGO { get; set; }

        public int? NO_DISPONIBILIDAD { get; set; }

        public long? VALOR_DESCUENTA_BASE { get; set; }

        [StringLength(30)]
        public string NUMERO_CONTRATO { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION_CONTRATO { get; set; }

        public DateTime? FECHA_VENCIMIENTO { get; set; }

        public long? VALOR_IVA { get; set; }

        public decimal? CANTIDAD { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        [StringLength(8)]
        public string CODIGO_BANCO { get; set; }

        [StringLength(40)]
        public string NUM_CUENTA { get; set; }

        [StringLength(2)]
        public string TIPO_CUENTA { get; set; }

        public decimal? PORC_IVA { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        public long? VALOR_UNITARIO { get; set; }

        [StringLength(256)]
        public string REFERENCIA { get; set; }

        [StringLength(1)]
        public string ANULADO { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        public decimal? PORCENTAJE_REFERENCIA { get; set; }

        public decimal? COD_CONCEPTO_REF_CXC { get; set; }

        public decimal? PORCENTAJE_DISPERCION { get; set; }

        public decimal? VALOR3 { get; set; }

        public virtual TIPOS_COMERCIAL TIPOS_COMERCIAL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_COMERCIAL> DET_COMERCIAL1 { get; set; }

        public virtual DET_COMERCIAL DET_COMERCIAL2 { get; set; }
    }
}
