namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.MOTIVO_ANULACION")]
    public partial class MOTIVO_ANULACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_MOTIVO_ANULACION { get; set; }

        [Required]
        [StringLength(250)]
        public string S_DESCRIPCION { get; set; }
    }
}
