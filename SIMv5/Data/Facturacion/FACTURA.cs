//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIM.Data.Facturacion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FACTURA", Schema = "FACTURACION")]
    public partial class FACTURA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FACTURA()
        {
            this.DETALLE_FACTURA = new HashSet<DETALLE_FACTURA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FACTURA { get; set; }
        public int ID_TIPOFACTURA { get; set; }
        public Nullable<int> CODIGO_COMERCIAL { get; set; }
        public string S_DESCRIPCION { get; set; }
        public string S_NOTAS { get; set; }
        public Nullable<decimal> N_NUIP { get; set; }
        public Nullable<System.DateTime> D_PAGO { get; set; }
        public Nullable<int> N_FACTURA { get; set; }
        public string S_TERCERO { get; set; }
        public string S_DIRECCIONTERCERO { get; set; }
        public string S_TELEFONOTERCERO { get; set; }
        public string S_CIUDAD { get; set; }
        public string S_DEPARTAMENTO { get; set; }
        public string S_TIPOTERCERO { get; set; }
        public Nullable<System.DateTime> D_ELABORACION { get; set; }
        public Nullable<int> N_FACTURASVENCIDAS { get; set; }
        public Nullable<decimal> N_VALORFACTURASVENCIDAS { get; set; }
        public decimal N_TIPOFACTURA { get; set; }
        public string S_DIGITO { get; set; }
        public Nullable<decimal> ID_TERCEROSIM { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLE_FACTURA> DETALLE_FACTURA { get; set; }

        [ForeignKey("ID_TIPOFACTURA")]
        public virtual TIPO_FACTURA TIPO_FACTURA { get; set; }
    }
}