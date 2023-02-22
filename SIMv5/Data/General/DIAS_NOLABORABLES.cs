namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.DIAS_NOLABORABLES")]
    public partial class DIAS_NOLABORABLES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_NOLABORAL { get; set; }

        public DateTime D_DIA { get; set; }
    }
}
