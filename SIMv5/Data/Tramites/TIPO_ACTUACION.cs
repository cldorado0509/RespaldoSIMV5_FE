namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.TIPO_ACTUACION")]
    public partial class TIPO_ACTUACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPO_ACTUACION { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }
    }
}
