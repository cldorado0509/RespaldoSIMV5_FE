namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.DOCUMENTOS_TOMO")]
    public partial class DOCUMENTOS_TOMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DOCUMENTOTOMO { get; set; }

        public int ID_DOCUMENTO { get; set; }

        public int ID_TOMO { get; set; }

        public int? N_FOLIOINICIAL { get; set; }

        public int? N_FOLIOFINAL { get; set; }

        public DateTime? D_CREACION { get; set; }

        public int? ID_TERCERO { get; set; }

        [ForeignKey("ID_DOCUMENTO")]
        public virtual DOCUMENTOS DOCUMENTOS { get; set; }

        [ForeignKey("ID_TOMO")]
        public virtual TOMOS TOMOS { get; set; }
    }
}
