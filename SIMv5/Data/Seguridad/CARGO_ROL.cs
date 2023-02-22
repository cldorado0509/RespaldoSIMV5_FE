namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.CARGO_ROL")]
    public partial class CARGO_ROL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CARGO_ROL { get; set; }

        public int ID_ROL { get; set; }

        public int ID_CARGO { get; set; }
    }
}
