namespace SIM.Data.Contrato
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTRATO.CONTRATO")]
    public partial class CONTRATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTRATO { get; set; }

        public int? ID_TIPOCONTRATO { get; set; }

        public int? ID_TIPOFORMAPAGO { get; set; }

        public decimal? ID_DEPENDENCIA { get; set; }

        public DateTime? D_CONTRATO { get; set; }

        public int N_CONTRATO { get; set; }

        public int? N_ANO { get; set; }

        [StringLength(2000)]
        public string S_OBJETO { get; set; }

        public int? ID_TIPOPROCESO { get; set; }

        public int? ID_TIPOASOCIACION { get; set; }

        public int? ID_MODALIDAD { get; set; }

        [StringLength(200)]
        public string S_PLAZO { get; set; }

        public decimal? N_VALORINICIAL { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ARRENDAMIENTO { get; set; }

        public decimal? ID_PRECONTRATO { get; set; }

        public decimal? ID_FUNCIONARIO { get; set; }

        [StringLength(50)]
        public string S_CONTRATO_OTRO { get; set; }

        [StringLength(1000)]
        public string S_OBSERVACION { get; set; }

        public DateTime D_REGISTRO { get; set; }

        public decimal? ID_CONVENIO { get; set; }

        public decimal? N_PLAZO { get; set; }

        public decimal? N_VALORAMVA { get; set; }

        public decimal? N_VALOROTROS { get; set; }

        public string S_NROPROCESO { get; set; }

        [ForeignKey("ID_TIPOASOCIACION")]
        public virtual TIPO_ASOCIACION TIPO_ASOCIACION { get; set; }

        [ForeignKey("ID_TIPOCONTRATO")]
        public virtual TIPO_CONTRATO TIPO_CONTRATO { get; set; }

        [ForeignKey("ID_TIPOPROCESO")]
        public virtual TIPO_PROCESO TIPO_PROCESO { get; set; }
    }
}
