namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.ROL_NOTIFICACION")]
    public partial class ROL_NOTIFICACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ROL { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_EMAIL_NOTIFICACION { get; set; }
    }
}
