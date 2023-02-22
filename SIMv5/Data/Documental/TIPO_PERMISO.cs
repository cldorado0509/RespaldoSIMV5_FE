namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.TIPO_PERMISO")]
    public partial class TIPO_PERMISO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPOPERMISO { get; set; }

        [StringLength(250)]
        public string S_TIPOPERMISO { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }
    }
}
