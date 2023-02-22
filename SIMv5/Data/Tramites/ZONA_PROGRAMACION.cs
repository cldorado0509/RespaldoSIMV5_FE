namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.ZONA_PROGRAMACION")]
    public partial class ZONA_PROGRAMACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ZONA { get; set; }

        [Required]
        [StringLength(20)]
        public string CODIGO { get; set; }

        [Required]
        [StringLength(100)]
        public string NOMBRE { get; set; }
    }
}
