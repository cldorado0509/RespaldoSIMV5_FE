namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TBTRAMITES_BLOQUEADOS")]
    public partial class TBTRAMITES_BLOQUEADOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CODTRAMITE_BLOQUEADO { get; set; }

        public int CODTRAMITE { get; set; }

        public int CODTAREA { get; set; }

        public int ORDEN { get; set; }

        public DateTime FECHABLOQUEO { get; set; }

        public int CODFUNCIONARIOBLOQUEO { get; set; }

        public DateTime? FECHADESBLOQUEO { get; set; }

        public int? CODFUNCIONARIODESBLOQUEO { get; set; }

        [Required]
        [StringLength(2000)]
        public string OBSERVACIONES { get; set; }

        [StringLength(20)]
        public string RADICADO { get; set; }

        public decimal CODSERIE { get; set; }

        public DateTime? FECHARADICADO { get; set; }

        [StringLength(1)]
        public string BLOQUEO_VITAL { get; set; }
    }
}
