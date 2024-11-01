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

    [Table("TIPO_FACTURA", Schema="FACTURACION")]
    public partial class TIPO_FACTURA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIPO_FACTURA()
        {
            this.CONCEPTO_TIPOFACTURA = new HashSet<CONCEPTO_TIPOFACTURA>();
            this.CUENTA_TIPOFACTURA = new HashSet<CUENTA_TIPOFACTURA>();
            this.FACTURA = new HashSet<FACTURA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TIPOFACTURA { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_DESCRIPCIONNOMBRE { get; set; }
        public string S_DESCRIPCIONCANTIDAD { get; set; }
        public string S_DESCRIPCIONCOSTO { get; set; }
        public string S_DESCRIPCIONTOTAL { get; set; }
        public string S_CUENTA { get; set; }
        public string S_BANCO { get; set; }
        public string S_CLAVE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONCEPTO_TIPOFACTURA> CONCEPTO_TIPOFACTURA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CUENTA_TIPOFACTURA> CUENTA_TIPOFACTURA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACTURA> FACTURA { get; set; }
    }
}
