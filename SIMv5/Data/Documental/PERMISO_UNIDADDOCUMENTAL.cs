namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.PERMISO_UNIDADDOCUMENTAL")]
    public partial class PERMISO_UNIDADDOCUMENTAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDPERMISOUNIDADDOCUMENTAL { get; set; }

        public int ID_UNIDADDOCUMENTAL { get; set; }

        public int? ID_DEPENDENCIA { get; set; }

        public int? ID_CARGO { get; set; }

        public int? ID_NIVEL { get; set; }

        public int ID_TIPOPERMISO { get; set; }
    }
}
