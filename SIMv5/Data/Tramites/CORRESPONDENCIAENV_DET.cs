namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.CORRESPONDENCIAENV_DET")]
    public partial class CORRESPONDENCIAENV_DET
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_CODDET { get; set; }

        public decimal? ID_COD { get; set; }

        [StringLength(512)]
        public string S_DESTINATARIO { get; set; }

        [StringLength(512)]
        public string S_DIRECCION { get; set; }

        [StringLength(200)]
        public string S_CIUDAD { get; set; }

        public decimal? N_PESO { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACIONES { get; set; }

        [StringLength(50)]
        public string S_REFERENCIA { get; set; }

        [StringLength(512)]
        public string S_CONTENIDO { get; set; }

        public decimal? CODTRAMITE { get; set; }

        public decimal? CODDOCUMENTO { get; set; }

        [StringLength(1)]
        public string S_DEVOLUCION { get; set; }

        public DateTime? D_FECHADEV { get; set; }

        [StringLength(100)]
        public string S_NOVEDAD { get; set; }
    }
}
