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

    [Table("QRY_RESOLUCIONFACTURA", Schema = "FACTURACION")]
    public partial class QRY_RESOLUCIONFACTURA
    {
        [Key]
        public decimal CODTRAMITE { get; set; }
        public decimal CODDOCUMENTO { get; set; }
        public string PAGO_TOTAL { get; set; }
        public string FACTURA_ASIGNADA { get; set; }
        public string RADICADO { get; set; }
        public string FECHA_RADICADO { get; set; }
        public string ASUNTO { get; set; }
    }
}