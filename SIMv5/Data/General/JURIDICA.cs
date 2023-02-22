namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.JURIDICA")]
    public partial class JURIDICA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        public int? ID_TERCEROREPLEGAL { get; set; }

        [StringLength(50)]
        public string S_CODCGN { get; set; }

        [StringLength(500)]
        public string S_RSOCIAL { get; set; }

        [StringLength(500)]
        public string S_NCOMERCIAL { get; set; }

        [StringLength(50)]
        public string S_SIGLA { get; set; }

        [StringLength(1)]
        public string S_NATURALEZA { get; set; }

        [StringLength(1)]
        public string S_ORDEN { get; set; }

        [StringLength(50)]
        public string S_DESCORDEN { get; set; }

        [StringLength(50)]
        public string S_REGIMEN { get; set; }

        [StringLength(1)]
        public string S_AUTORETENEDOR { get; set; }

        [StringLength(1)]
        public string S_GRANCONTRIBUYENTE { get; set; }

        [StringLength(1)]
        public string S_RESPONSABLEIVA { get; set; }

        [StringLength(1)]
        public string S_RETIENEIVA { get; set; }

        [StringLength(1)]
        public string S_RETIENEICA { get; set; }

        [StringLength(50)]
        public string S_MATRICULA { get; set; }

        [StringLength(50)]
        public string S_DCAMCOMERCIO { get; set; }

        public int? N_ESTABLECIMIENTOS { get; set; }

        public DateTime? D_CONSTITUCION { get; set; }

        //public int? ID_TAMANIO_EMPRESA { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
