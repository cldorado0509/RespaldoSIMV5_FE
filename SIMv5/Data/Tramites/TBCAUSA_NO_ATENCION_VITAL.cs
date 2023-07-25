namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Causal de no atención de solicitud de VITAL
    /// </summary>
    [Table("TRAMITES.TBCAUSA_NO_ATENCION_VITAL")]
    public partial class TBCAUSA_NO_ATENCION_VITAL
    {
        /// <summary>
        /// Clave Primaria
        /// </summary>
        [Key]
        public decimal ID { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Required]
        [StringLength(202)]
        public string S_NOMBRE { get; set; }

        /// <summary>
        /// Habilitado
        /// </summary>
        [StringLength(1)]
        public string S_HABILITADO { get; set; }

       

    }
}
