namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.PARTICULAR")]
    public partial class PARTICULAR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PARTICULAR { get; set; }

        public int? ID_TERCERO { get; set; }

        public DateTime D_INICIO { get; set; }

        [StringLength(200)]
        public string S_OBSERVACION { get; set; }
    }
}
