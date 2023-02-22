namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.CONTRATO_TERCERO")]
    public partial class CONTRATO_TERCERO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTRATOTERCERO { get; set; }

        public int? ID_TERCERO { get; set; }

        public int? ID_CONTRATO { get; set; }

        public int? ID_TIPOTERCEROCONTRATO { get; set; }

        public int? N_ORDEN { get; set; }

        public decimal? N_APORTE { get; set; }

        public DateTime? D_APORTE { get; set; }

        public decimal? N_PARTICIPACION { get; set; }

        public int? ID_TIPOREGIMEN { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        public decimal? ID_TIPOCONTRATISTA { get; set; }

        [StringLength(1)]
        public string S_PRINCIPAL { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ACTIVO { get; set; }

        [ForeignKey("ID_TIPOCONTRATISTA")]
        public virtual TIPO_CONTRATISTA TIPO_CONTRATISTA { get; set; }
    }
}
