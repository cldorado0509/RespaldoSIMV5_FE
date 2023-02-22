namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_RIESGO")]
    public partial class FLR_RIESGO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_RIESGO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_RIESGO { get; set; }
    }
}
