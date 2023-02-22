namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.INV_ESTADO_INDIVIDUO")]
    public partial class INV_ESTADO_INDIVIDUO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ESTADO { get; set; }

        [StringLength(100)]
        public string S_ESTADO { get; set; }
    }
}
