namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.FLR_SINTOMA_EF")]
    public partial class FLR_SINTOMA_EF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SINTOMA_EF { get; set; }

        [Required]
        [StringLength(100)]
        public string S_SINTOMA_EF { get; set; }
    }
}
